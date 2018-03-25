using System.Globalization;
using System.IO;
using Sdl.Community.Toolkit.Tokenization.Model;
using Sdl.Community.Toolkit.Tokenization.Reader;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.Core.Tokenization;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace Sdl.Community.Toolkit.Tokenization
{
    public class Tokenizer
    {
        public FileBasedTranslationMemory MyTemporaryTm { get; private set; }

        private readonly EnvironmentPaths _environmentPaths;

        private CultureInfo SourceCultureInfo { get; }
        private CultureInfo TargetCultureInfo { get; }

        public Tokenizer(CultureInfo sourceCultureInfo, CultureInfo targetCultureInfo)
        {
            _environmentPaths = new EnvironmentPaths(EnvironmentConstants.ProductName);
            SourceCultureInfo = sourceCultureInfo;
            TargetCultureInfo = targetCultureInfo;
        }

        public TokenizedSegment TokenizeSegment(ISegmentPair segmentPair)
        {
            return TokenizeSegment(SegmentVisitor(segmentPair.Source, MyTemporaryTm.LanguageDirection.SourceLanguage).Segment,
                SegmentVisitor(segmentPair.Target, MyTemporaryTm.LanguageDirection.TargetLanguage).Segment);
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

            var searchResults = MyTemporaryTm?.LanguageDirection.SearchSegment(GetSearchSettings(), sourceSegment);

            MyTemporaryTm?.LanguageDirection.DeleteTranslationUnit(tuImport.TuId);

            return GeTokenizedSegmentResult(searchResults);
        }

        public bool CreateTranslationMemory()
        {
            var tmName = "TM." + SourceCultureInfo.Name + "-" + TargetCultureInfo.Name + ".sdltm";
            var tmPath = Path.Combine(_environmentPaths.MyTmPath, tmName);

            if (File.Exists(tmPath))
            {
                MyTemporaryTm = new FileBasedTranslationMemory(tmPath);
                return false;
            }

            MyTemporaryTm = new FileBasedTranslationMemory(tmPath
                , "Temporary TM"
                , SourceCultureInfo
                , TargetCultureInfo
                , FuzzyIndexes.SourceWordBased
                , BuiltinRecognizers.RecognizeAll
                , TokenizerFlags.DefaultFlags
                , WordCountFlags.DefaultFlags
                , false);
            MyTemporaryTm.FieldDefinitions.Add(new FieldDefinition("FileIndex", FieldValueType.Integer));
            MyTemporaryTm.Save();

            return true;
        }

        public void DeleteTranslationMemory()
        {
            try
            {
                if (File.Exists(MyTemporaryTm.FilePath))
                    File.Delete(MyTemporaryTm.FilePath);
            }
            catch
            {
                // catch all
            }
        }

        private static SegmentVisitor SegmentVisitor(ISegment seg, CultureInfo culture)
        {
            var segment = new Segment(culture);

            var visitor = new SegmentVisitor(segment, false);

            visitor.VisitSegment(seg);

            return visitor;
        }

        private TokenizedSegment GeTokenizedSegmentResult(SearchResults searchResults)
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

        private ImportResult AddTranslationUnit(Segment sourceSegment, Segment targetSegment)
        {
            var unit = new TranslationUnit(sourceSegment, targetSegment);

            var tuResult = MyTemporaryTm.LanguageDirection.AddTranslationUnit(
                unit, GetImportSettings());

            return tuResult;
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
