using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Sdl.Community.Toolkit.FileType.Processors.API;
using Sdl.Community.Toolkit.FileType.Processors.Internal.Comparers;
using Sdl.FileTypeSupport.Framework.Core.Utilities.Formatting;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdl.Community.Toolkit.FileType.Processors.Internal.Services
{
	internal class RegexProcessorService
	{
		internal List<RegexMatch> ApplyRegexRules(string plainString, List<IMatchRule> matchRules)
		{
			var searchResults = new List<RegexMatch>();
			var matchedIndexes = new List<int>();

			//iterate over tag pairs first
			ProcessTagPairRules(plainString, matchRules, searchResults, matchedIndexes);

			//then iterate over all rules and treat them all as placeholders (doing actual placeholders first)
			ProcessPlaceholderRules(plainString, matchRules, searchResults, matchedIndexes);

			//sort results by match index so that they are in the correct order for later processing
			searchResults.Sort(new RegexSearchResultsComparer());

			return searchResults;
		}

		internal bool HasEmbeddedContext(IList<string> structureInfos, IContextProperties contexts)
		{
			if (contexts == null)
			{
				return false;
			}

			return contexts.Contexts.Any(context => structureInfos.Contains(context.ContextType));
		}

		internal IStartTagProperties CreateStartTagProperties(IPropertiesFactory factory, string tagContent, IMatchRule rule)
		{
			var startProperties = factory.CreateStartTagProperties(tagContent);

			ApplyInlineTagProperties(startProperties, rule);
			startProperties.DisplayText = GetDisplayName(tagContent);
			startProperties.Formatting = FormattingInflator.InflateFormatting(rule.Formatting);
			startProperties.SegmentationHint = rule.SegmentationHint;
			return startProperties;
		}

		internal IEndTagProperties CreateEndTagProperties(IPropertiesFactory factory, string tagContent, IMatchRule rule)
		{
			var endProperties = factory.CreateEndTagProperties(tagContent);

			ApplyInlineTagProperties(endProperties, rule);
			endProperties.DisplayText = GetDisplayName(tagContent);

			return endProperties;
		}

		internal IPlaceholderTagProperties CreatePlaceholderTagProperties(IPropertiesFactory factory, string tagContent, IMatchRule rule)
		{
			var placeholderProps = factory.CreatePlaceholderTagProperties(tagContent);

			ApplyInlineTagProperties(placeholderProps, rule);
			placeholderProps.DisplayText = GetDisplayName(tagContent);
			placeholderProps.SegmentationHint = rule.SegmentationHint;
			placeholderProps.TagContent = tagContent;

			if (!string.IsNullOrEmpty(rule.TextEquivalent))
			{
				placeholderProps.TextEquivalent = rule.TextEquivalent;
			}
			return placeholderProps;
		}

		internal string GetDisplayName(string tagContent)
		{
			//trim the start and end to get rid of opening and closing XML tags
			var displayName = tagContent.TrimStart('<', '/');
			displayName = displayName.TrimEnd('>', '/');

			var spaceIndex = displayName.IndexOf(" ", StringComparison.Ordinal);
			if (spaceIndex > 0 && spaceIndex < 10)
			{
				//return first word
				displayName = displayName.Substring(0, spaceIndex);
			}
			else if (displayName.Length > 10)
			{
				//restrict to first 10 characters
				displayName = displayName.Substring(0, 10);
			}

			return displayName;
		}

		private static void ProcessTagPairRules(string plainString, IEnumerable<IMatchRule> matchRules,
			ICollection<RegexMatch> searchResults, ICollection<int> matchedIndexes)
		{
			foreach (var rule in matchRules)
			{
				if (rule.TagType == TagTypeOption.TagPair)
				{
					var startTagRegex = rule.BuildStartTagRegex();
					var endTagRegex = rule.BuildEndTagRegex();

					//make sure that \n etc are ignored in the search
					var options = RegexOptions.Singleline | startTagRegex.Options;

					//combine open and end tag regex and match all in between
					var completeRegex = new Regex(startTagRegex +
						@".*?" + endTagRegex, options);

					var completeMatch = completeRegex.Match(plainString);

					//matches full tag pair, otherwise we could have orphaned start or end tags
					while (completeMatch.Success)
					{
						var startTagMatch = startTagRegex.Match(completeMatch.Value);
						Debug.Assert(startTagMatch.Success, "Unable to find start tag match in tag pair search!");

						var endTagMatch = endTagRegex.Match(completeMatch.Value);
						Debug.Assert(endTagMatch.Success, "Unable to find end tag match in tag pair search!");

						var endIndex = (completeMatch.Index + completeMatch.Length) - endTagMatch.Value.Length;

						//only add matches at indexes that have not already been added
						if (!matchedIndexes.Contains(completeMatch.Index) && !matchedIndexes.Contains(endIndex))
						{
							//have to use index from original match when adding to search results or won't be valid in the document
							searchResults.Add(CreateRegexMatch(rule, startTagMatch, completeMatch.Index, RegexMatch.TagType.TagPairOpening));
							searchResults.Add(CreateRegexMatch(rule, endTagMatch, endIndex, RegexMatch.TagType.TagPairClosing));

							matchedIndexes.Add(completeMatch.Index);
							matchedIndexes.Add(endIndex);
						}

						completeMatch = completeMatch.NextMatch();
					}
				}
			}
		}

		private static void ProcessPlaceholderRules(string plainString, List<IMatchRule> matchRules,
		   ICollection<RegexMatch> searchResults, ICollection<int> matchedIndexes)
		{
			//we need to process placeholders before tag pairs
			matchRules.Sort(new MatchRulesTypeComparer());

			foreach (var rule in matchRules)
			{
				var startTagRegex = rule.BuildStartTagRegex();
				var placeholderMatch = startTagRegex.Match(plainString);

				while (placeholderMatch.Success)
				{
					if (!matchedIndexes.Contains(placeholderMatch.Index))
					{
						searchResults.Add(CreateRegexMatch(rule, placeholderMatch, placeholderMatch.Index, RegexMatch.TagType.Placeholder));
						matchedIndexes.Add(placeholderMatch.Index);
					}

					placeholderMatch = placeholderMatch.NextMatch();
				}

				//process tag pair end tags
				if (rule.TagType == TagTypeOption.TagPair)
				{
					var endTagRegex = rule.BuildEndTagRegex();
					var endTagMatch = endTagRegex.Match(plainString);

					while (endTagMatch.Success)
					{
						if (!matchedIndexes.Contains(endTagMatch.Index))
						{
							searchResults.Add(CreateRegexMatch(rule, endTagMatch, endTagMatch.Index, RegexMatch.TagType.Placeholder));
							matchedIndexes.Add(endTagMatch.Index);
						}

						endTagMatch = endTagMatch.NextMatch();
					}
				}
			}
		}

		private static void ApplyInlineTagProperties(IAbstractInlineTagProperties tagProperties, IMatchRule rule)
		{
			tagProperties.CanHide = rule.CanHide;
			tagProperties.IsSoftBreak = rule.IsSoftBreak;
			tagProperties.IsWordStop = rule.IsWordStop;
		}

		private static RegexMatch CreateRegexMatch(IMatchRule rule, Capture tagMatch, int index, RegexMatch.TagType tagType)
		{
			var regexMatch = new RegexMatch
			{
				Type = tagType,
				Value = tagMatch.Value,
				Index = index,
				Rule = rule
			};
			return regexMatch;
		}
	}
}

