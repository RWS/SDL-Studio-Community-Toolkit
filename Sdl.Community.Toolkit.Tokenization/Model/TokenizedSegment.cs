using Sdl.LanguagePlatform.Core;

namespace Sdl.Community.Toolkit.Tokenization.Model
{
    public class TokenizedSegment
    {
        public WordCounts SourceWordCounts { get; set; }

        public Segment SourceSegment { get; set; }

        public Segment TargetSegment { get; set; }
    }
}
