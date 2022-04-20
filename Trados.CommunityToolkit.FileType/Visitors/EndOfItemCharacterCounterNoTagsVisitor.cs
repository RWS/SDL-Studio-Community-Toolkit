using Sdl.FileTypeSupport.Framework.BilingualApi;
using static Sdl.FileTypeSupport.Framework.Core.Utilities.BilingualApi.CharacterCountingIterator;

namespace Trados.Community.Toolkit.FileType.Visitors
{
	public class EndOfItemCharacterCounterNoTagsVisitor : ICharacterCountingVisitor
    {
        private int _count;

        public int Count
        {
            get => _count;

	        set => _count = value;
        }

        public void VisitCommentMarker(ICommentMarker commentMarker)
        {
            //Nothing to add
        }

        public void VisitLocationMarker(ILocationMarker location)
        {
            //Nothing to add
        }

        public void VisitLockedContent(ILockedContent lockedContent)
        {
            //Nothing to add
        }

        public void VisitOtherMarker(IOtherMarker marker)
        {
            //Nothing to add
        }

        public void VisitPlaceholderTag(IPlaceholderTag tag)
        {
            //Nothing to add
        }

        public void VisitRevisionMarker(IRevisionMarker revisionMarker)
        {
            //Nothing to add
        }

        public void VisitSegment(ISegment segment)
        {
            //Nothing to add
        }

        public void VisitTagPair(ITagPair tagPair)
        {
            //Nothing to add
        }

        public void VisitText(IText text)
        {
            //Nothing to add
        }
    }
}
