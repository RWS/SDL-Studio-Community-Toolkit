using Sdl.Community.Toolkit.FileType.Internal;
using Sdl.Community.Toolkit.FileType.Visitors;
using Sdl.Community.Toolkit.Integration.Visitors;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.Core.Utilities.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdl.Community.Toolkit.FileType
{
    public static class SegmentExtensions
    {
        /// <summary>
        /// Verifies if the text qualifies as a valid floating number.
        /// </summary>
        /// <param name="text">text to verify</param>
        /// <returns>returns true if the text has qualified as a valid floating number</returns>
        public static bool IsValidFloatingNumber(this ISegment segment, params char[] separators)
        {
            var text = segment.GetString(false);
            if (string.IsNullOrEmpty(text)) return true;

            return NumberTokenHelper.IsValidFloating(text);


        }
        public static string GetString(this ISegment segment, bool includeSegments = false)
        {
            var textVisitor = new TextCollectionVisitor(includeSegments);

            foreach (var item in segment)
            {
                IStructureTag stag = item as IStructureTag;
                if (stag != null) continue;

                item.AcceptVisitor(textVisitor);
            }

            return textVisitor.CollectedText;
        }

        public static List<IComment> GetComments(this ISegment segment)
        {
            var commentVisitor = new CommentDataVisitor();

            return commentVisitor.GetComments(segment);
        }

        public static IText GetTextAtLocation(this ISegment segment, int startIndex)
        {
            var counter = segment.GetCharacterCountingIterator(startIndex);
            if (counter.CharacterCount < startIndex)
            {
                // the actual count is between this location and the next
                //  - if this is a text node we can point to the exact location inside the text
                IText text = counter.CurrentLocation.ItemAtLocation as IText;
                return text;
            }
            return null;
        }

        public static string Substring(this ISegment segment, int startIndex, int endPosition)
        {
            var counter = segment.GetCharacterCountingIterator(startIndex);
            if (counter.CharacterCount < startIndex)
            {
                // the actual count is between this location and the next
                //  - if this is a text node we can point to the exact location inside the text
                IText text = counter.CurrentLocation.ItemAtLocation as IText;
                if (text != null)
                {
                    var startLocationInsideTextItem = new TextLocation(counter.CurrentLocation, startIndex - counter.CharacterCount);
                    var segmentText = text.Properties.Text;
                    return segmentText.Substring(startLocationInsideTextItem.TextOffset,
                        endPosition - startLocationInsideTextItem.TextOffset);

                }
            }

            return string.Empty;
        }

        public static void Replace(this ISegment segment, int startIndex, int endPosition, string replacementText)
        {
            var textVisitor = new CustomTextCollectionVisitor(segment, startIndex, endPosition);
            foreach (var item in segment)
                item.AcceptVisitor(textVisitor);

            textVisitor.ReplaceText(replacementText);
        }

        private static CharacterCountingIterator GetCharacterCountingIterator(this ISegment segment, int startIndex)
        {
            Location startLocation = new Location(segment, true);


            CharacterCountingIterator counter = new CharacterCountingIterator(startLocation,
                ()=>new StartOfItemCharacterCounterNoTagsVisitor(),
                ()=>new EndOfItemCharacterCounterNoTagsVisitor());

            while (counter.CharacterCount <= startIndex)
            {
                if (!counter.MoveNext())
                {
                    break;
                }
            }
            counter.MovePrevious();
            return counter;
        }

        public static int GetSegmentEditDistance(this ISegment segment, string replacementText)
        {
            var textSegment = segment.GetString();
            return LevenshteinDistance.Calculate(textSegment, replacementText);
        }
    }
}
