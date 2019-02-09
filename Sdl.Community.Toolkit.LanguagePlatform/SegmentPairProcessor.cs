using System;
using System.Globalization;
using System.IO;
using Sdl.Community.Toolkit.LanguagePlatform.Models;
using Sdl.Community.Toolkit.LanguagePlatform.Visitors;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using Action = Sdl.LanguagePlatform.TranslationMemory.Action;
using WordCounts = Sdl.Community.Toolkit.LanguagePlatform.Models.WordCounts;

namespace Sdl.Community.Toolkit.LanguagePlatform
{
	/// <summary>
	/// The SegmentPair processer offers the possiblity to recover additional information from the ISegmentPair (FileTypeSupport.Framework)
	/// through the LanguagePlatform, that includes the tokenized segments and word counts.
	/// 
	/// The processor creates a tempoaray translation memory given the settings provided; the TM is then used to recover the LanguagePlatform
	/// segment, which is a sequence of Sdl.LanguagePlatform.Core.SegmentElements.	
	/// </summary>
	public class SegmentPairProcessor
	{
		private readonly Settings _settings;

		private readonly PathInfo _pathInfo;

		private readonly FileBasedTranslationMemory _temporaryTm;

		/// <summary>
		/// The SegmentPair processer offers the possiblity to recover additional information from the ISegmentPair (FileTypeSupport.Framework)
		/// through the LanguagePlatform, that includes the tokenized segments and word counts
		/// </summary>
		/// <param name="settings">Settings used primarily to setup the translation memory</param>
		/// <param name="pathInfo">Path information</param>
		public SegmentPairProcessor(Settings settings, PathInfo pathInfo)
		{
			_settings = settings;

			_pathInfo = pathInfo;

			_temporaryTm = CreateTranslationMemory();
		}

		/// <summary>
		/// Retruns the SegmentPairInfo, which includes the LanguagePlatform representation of the ISegmentPair, e.g. the tokenized segment and word counts
		/// </summary>
		/// <param name="segmentPair">Represents a source and target segment pair in a paragraph unit.</param>
		/// <returns>SegmentPairInfo</returns>
		public SegmentPairInfo GetSegmentPairInfo(ISegmentPair segmentPair)
		{
			return GetSegmentPairInfo(
				SegmentVisitor(segmentPair.Source, _temporaryTm.LanguageDirection.SourceLanguage).Segment,
				SegmentVisitor(segmentPair.Target, _temporaryTm.LanguageDirection.TargetLanguage).Segment);
		}

		/// <summary>
		/// Retruns the SegmentPairInfo, which includes the LanguagePlatform representation of the ISegmentPair, e.g. the tokenized segment and word counts
		/// </summary>
		/// <param name="source">The source segment</param>
		/// <param name="target">The target segment</param>
		/// <returns>SegmentPairInfo</returns>
		public SegmentPairInfo GetSegmentPairInfo(ISegment source, ISegment target)
		{
			return GetSegmentPairInfo(
				SegmentVisitor(source, _temporaryTm.LanguageDirection.SourceLanguage).Segment,
				SegmentVisitor(target, _temporaryTm.LanguageDirection.TargetLanguage).Segment);
		}

		/// <summary>
		/// Attempts to delete any temporary files used during processing, e.g. the temporary TM
		/// </summary>
		public void CleanupTemporaryFiles()
		{
			var tmPath = GetTemporaryTmPath();

			if (File.Exists(tmPath))
			{
				try
				{
					File.Delete(tmPath);
				}
				catch (Exception ex)
				{
					throw new Exception($"Unable to delete translation memory {tmPath}\r\n" + ex.Message);
				}
			}			
		}

		private SegmentPairInfo GetSegmentPairInfo(Segment sourceSegment, Segment targetSegment)
		{
			if (sourceSegment.Elements.Count == 0)
			{
				return null;
			}

			if (targetSegment.Elements.Count == 0)
			{
				targetSegment.Elements.AddRange(sourceSegment.Elements);
			}

			var tuImport = AddTranslationUnit(sourceSegment, targetSegment);

			var searchResults = _temporaryTm?.LanguageDirection.SearchSegment(GetSearchSettings(), sourceSegment);

			_temporaryTm?.LanguageDirection.DeleteTranslationUnit(tuImport.TuId);

			return GeSegmentPairInfoResult(searchResults);
		}

		private FileBasedTranslationMemory CreateTranslationMemory()
		{
			var tmPath = GetTemporaryTmPath();

			if (File.Exists(tmPath))
			{
				var existingTm = new FileBasedTranslationMemory(tmPath);
				if (Equals(existingTm.LanguageDirection.SourceLanguage, _settings.SourceLanguage) &&
					Equals(existingTm.LanguageDirection.TargetLanguage, _settings.TargetLanguage) &&
					existingTm.Recognizers == _settings.Recognizers &&
					existingTm.TokenizerFlags == _settings.TokenizerFlags &&
					existingTm.WordCountFlags == _settings.WordCountFlags)
				{
					return existingTm;
				}
			}

			var temporaryTm = new FileBasedTranslationMemory(tmPath
				, "Temporary TM"
				, _settings.SourceLanguage
				, _settings.TargetLanguage
				, FuzzyIndexes.SourceWordBased
				, _settings.Recognizers
				, _settings.TokenizerFlags
				, _settings.WordCountFlags
				, false);
			temporaryTm.FieldDefinitions.Add(new FieldDefinition("FileIndex", FieldValueType.Integer));
			temporaryTm.Save();

			return temporaryTm;
		}

		private string GetTemporaryTmPath()
		{
			var tmName = _pathInfo.TmName;

			if (tmName.Contains("[SourceLanguageName]"))
			{
				tmName = tmName.Replace("[SourceLanguageName]", _settings.SourceLanguage.Name);
			}
			if (tmName.Contains("[TargetLanguageName]"))
			{
				tmName = tmName.Replace("[TargetLanguageName]", _settings.TargetLanguage.Name);
			}

			var tmPath = Path.Combine(_pathInfo.TemporaryResourcesPath, tmName);


			return tmPath;
		}
		
		private ImportResult AddTranslationUnit(Segment sourceSegment, Segment targetSegment)
		{
			if (_temporaryTm == null)
			{
				throw new Exception($"Unable to locate the temporary TM: {GetTemporaryTmPath()}");
			}

			var unit = new TranslationUnit(sourceSegment, targetSegment);

			var tuResult = _temporaryTm.LanguageDirection.AddTranslationUnit(
				unit, GetImportSettings());

			if (tuResult.Action == Action.Error)
			{
				throw new Exception($"Unable to add TU to the temporary TM: {GetTemporaryTmPath()}");
			}

			return tuResult;
		}

		private static SegmentVisitor SegmentVisitor(ISegment seg, CultureInfo culture)
		{
			var segment = new Segment(culture);

			var visitor = new SegmentVisitor(segment, false);

			visitor.VisitSegment(seg);

			return visitor;
		}

		private static SegmentPairInfo GeSegmentPairInfoResult(SearchResults searchResults)
		{
			var result = new SegmentPairInfo();

			var searchResult = searchResults?.Results?[0];

			if (searchResult != null)
			{
				result.SourceSegment = searchResult.MemoryTranslationUnit.SourceSegment;
				result.TargetSegment = searchResult.MemoryTranslationUnit.TargetSegment;
				result.SourceWordCounts = new WordCounts
				{
					Words = searchResults.SourceWordCounts.Words,
					Characters = searchResults.SourceWordCounts.Characters,
					Tags = searchResults.SourceWordCounts.Tags,
					Placeables = searchResults.SourceWordCounts.Placeables
				};
			}

			return result;
		}

		//
		// Summary:
		//     Represents a set of settings relevant for search opeartions.
		private static SearchSettings GetSearchSettings()
		{
			var settings = new SearchSettings
			{
				MaxResults = 1,
				MinScore = 100,
				Mode = SearchMode.ExactSearch,
				Penalties = null,
				Filters = null,
				ComputeTranslationProposal = false
			};
			return settings;
		}

		//
		// Summary:
		//     Represents a group of settings that control the way the import is executed.
		private static ImportSettings GetImportSettings()
		{
			var settings = new ImportSettings
			{
				ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.Overwrite
			};
			return settings;
		}
	}
}
