using Sdl.FileTypeSupport.Framework.NativeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdl.Community.Toolkit.FileType
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
            else if (string.Compare(translationOrigin.OriginType, "interactive", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return OriginType.Interactive;
            }
            else
            {
                if (translationOrigin.MatchPercent >= 100)
                {
                    if (string.Compare(translationOrigin.OriginType, "document-match", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return OriginType.PM;
                    }
                    else if (translationOrigin.TextContextMatchLevel == TextContextMatchLevel.SourceAndTarget)
                    {
                        return OriginType.CM;
                    }
                    else if (string.Compare(translationOrigin.OriginType, "mt", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return OriginType.AT;
                    }
                    else
                    {
                        return OriginType.Exact;
                    }
                }
                else if (string.Compare(translationOrigin.OriginType, "mt", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return OriginType.AT;
                }
                else if (translationOrigin.MatchPercent > 0)
                {
                    return OriginType.Fuzzy;
                }
                else if (string.Compare(translationOrigin.OriginType, "source", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return OriginType.Source;
                }
            }
            return OriginType.None;
        }

        
    }
}
