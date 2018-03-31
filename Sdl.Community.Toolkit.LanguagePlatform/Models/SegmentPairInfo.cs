using Sdl.LanguagePlatform.Core;

namespace Sdl.Community.Toolkit.LanguagePlatform.Models
{
	/// <summary>
	/// SegmentPair info
	/// </summary>
	public class SegmentPairInfo
    {
	    //
	    // Summary:
	    //     Source word counts
		public WordCounts SourceWordCounts { get; set; }

	    //
	    // Summary:
	    //     Source segment, which is a sequence of Sdl.LanguagePlatform.Core.SegmentElements
		public Segment SourceSegment { get; set; }

	    //
	    // Summary:
	    //     Target segment, which is a sequence of Sdl.LanguagePlatform.Core.SegmentElements
		public Segment TargetSegment { get; set; }
    }
}
