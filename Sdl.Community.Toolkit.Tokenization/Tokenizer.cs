using System;
using System.Globalization;
using System.IO;
using Sdl.Community.Toolkit.Tokenization.Model;
using Sdl.Community.Toolkit.Tokenization.Reader;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.Core.Tokenization;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using Action = Sdl.LanguagePlatform.TranslationMemory.Action;

namespace Sdl.Community.Toolkit.Tokenization
{
	public class Tokenizer
	{
		private readonly EnvironmentPaths _environmentPaths;

		private readonly FileBasedTranslationMemory _temporaryTm;

		private CultureInfo SourceCultureInfo { get; }

		private CultureInfo TargetCultureInfo { get; }

		public Tokenizer(CultureInfo sourceCultureInfo, CultureInfo targetCultureInfo, EnvironmentPaths environmentPaths)
		{
			SourceCultureInfo = sourceCultureInfo;
			TargetCultureInfo = targetCultureInfo;

			_environmentPaths = environmentPaths;

			_temporaryTm = CreateTranslationMemory();
		}

		public TokenizedSegment TokenizeSegment(ISegmentPair segmentPair)
		{
			return TokenizeSegment(SegmentVisitor(segmentPair.Source, _temporaryTm.LanguageDirection.SourceLanguage).Segment,
				SegmentVisitor(segmentPair.Target, _temporaryTm.LanguageDirection.TargetLanguage).Segment);
		}

		public TokenizedSegment TokenizeSegment(Segment sourceSegment, Segment targetSegment)
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

			return GeTokenizedSegmentResult(searchResults);
		}

		private FileBasedTranslationMemory CreateTranslationMemory()
		{
			var tmPath = GetTemporaryTmPath();

			if (File.Exists(tmPath))
			{
				return new FileBasedTranslationMemory(tmPath);
			}

			var temporaryTm = new FileBasedTranslationMemory(tmPath
				, "Temporary TM"
				, SourceCultureInfo
				, TargetCultureInfo
				, FuzzyIndexes.SourceWordBased
				, BuiltinRecognizers.RecognizeAll
				, TokenizerFlags.DefaultFlags
				, WordCountFlags.DefaultFlags
				, false);
			temporaryTm.FieldDefinitions.Add(new FieldDefinition("FileIndex", FieldValueType.Integer));
			temporaryTm.Save();

			return temporaryTm;

		}

		private string GetTemporaryTmPath()
		{
			var tmName = "TM." + SourceCultureInfo.Name + "-" + TargetCultureInfo.Name + ".sdltm";
			var tmPath = Path.Combine(_environmentPaths.MyTmPath, tmName);
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

		private static TokenizedSegment GeTokenizedSegmentResult(SearchResults searchResults)
		{
			var result = new TokenizedSegment();

			var searchResult = searchResults?.Results?[0];

			if (searchResult != null)
			{
				result.SourceSegment = searchResult.MemoryTranslationUnit.SourceSegment;
				result.TargetSegment = searchResult.MemoryTranslationUnit.TargetSegment;
				result.SourceWordCounts = new Model.WordCounts
				{
					Words = searchResults.SourceWordCounts.Words,
					Characters = searchResults.SourceWordCounts.Characters,
					Tags = searchResults.SourceWordCounts.Tags,
					Placeables = searchResults.SourceWordCounts.Placeables
				};
			}

			return result;
		}

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
