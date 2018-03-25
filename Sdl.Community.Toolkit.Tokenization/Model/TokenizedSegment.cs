using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;

namespace Sdl.Community.Toolkit.Tokenization.Model
{
    public class TokenizedSegment
    {
        public WordCounts SourceWordCounts { get; set; }

        public Segment SourceSegment { get; set; }

        public Segment TargetSegment { get; set; }
    }
}
