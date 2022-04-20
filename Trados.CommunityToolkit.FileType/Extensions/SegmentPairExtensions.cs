using Sdl.FileTypeSupport.Framework.BilingualApi;

namespace Trados.Community.Toolkit.FileType.Extensions
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
