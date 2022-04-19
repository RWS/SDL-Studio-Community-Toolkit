using Sdl.FileTypeSupport.Framework.BilingualApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdl.Community.Toolkit.FileType
{
    public static class SegmentPairExtensions
    {
        public static OriginType GetOriginType(this ISegmentPair segmentPair)
        {
            if (segmentPair == null ||
                segmentPair.Properties == null||
                segmentPair.Properties.TranslationOrigin == null) return OriginType.None;
            return segmentPair.Properties.TranslationOrigin.GetOriginType();
        }

        public static OriginType GetPreviousTranslationOriginType(this ISegmentPair segmentPair)
        {
            if (segmentPair == null ||
                segmentPair.Properties == null ||
                segmentPair.Properties.TranslationOrigin == null||
                segmentPair.Properties.TranslationOrigin.OriginBeforeAdaptation== null) return OriginType.None;
            return segmentPair.Properties.TranslationOrigin.OriginBeforeAdaptation.GetOriginType();
        }
    }
}
