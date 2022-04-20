using System;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Trados.Community.Toolkit.FileType.Extensions
{
	public static class TranslatorOriginExtensions
    {
        public static OriginType GetOriginType(this ITranslationOrigin translationOrigin)
        {
            if (translationOrigin == null) return OriginType.None;
            if (string.Compare(translationOrigin.OriginType, "auto-propagated", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return OriginType.AutoPropagated;
            }

	        if (string.Compare(translationOrigin.OriginType, "interactive", StringComparison.OrdinalIgnoreCase) == 0)
	        {
		        return OriginType.Interactive;
	        }

	        if (translationOrigin.MatchPercent >= 100)
	        {
		        if (string.Compare(translationOrigin.OriginType, "document-match", StringComparison.OrdinalIgnoreCase) == 0)
		        {
			        return OriginType.PM;
		        }

		        if (translationOrigin.TextContextMatchLevel == TextContextMatchLevel.SourceAndTarget)
		        {
			        return OriginType.CM;
		        }

		        if (string.Compare(translationOrigin.OriginType, "mt", StringComparison.OrdinalIgnoreCase) == 0)
		        {
			        return OriginType.AT;
		        }

		        if (string.Compare(translationOrigin.OriginType, "nmt", StringComparison.OrdinalIgnoreCase) == 0)
		        {
			        return OriginType.NMT;
		        }

		        return OriginType.Exact;
	        }

	        if (string.Compare(translationOrigin.OriginType, "nmt", StringComparison.OrdinalIgnoreCase) == 0)
	        {
		        return OriginType.NMT;
	        }

	        if (string.Compare(translationOrigin.OriginType, "mt", StringComparison.OrdinalIgnoreCase) == 0)
	        {
		        return OriginType.AT;
	        }

	        if (translationOrigin.MatchPercent > 0)
	        {
		        return OriginType.Fuzzy;
	        }

	        if (string.Compare(translationOrigin.OriginType, "source", StringComparison.OrdinalIgnoreCase) == 0)
	        {
		        return OriginType.Source;
	        }

	        return OriginType.None;
        }
    }
}
