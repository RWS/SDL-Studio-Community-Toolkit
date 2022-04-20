using Sdl.LanguagePlatform.Core;

namespace Trados.Community.Toolkit.LanguagePlatform.Models
{
	/// <summary>
	/// SegmentPair info
	/// </summary>
	public class SegmentPairInfo
	{
		//
		// Summary:
		//     Source word counts
		public WordCounts SourceWordCounts { get; internal set; }

		//
		// Summary:
		//     Source segment, which is a sequence of Sdl.LanguagePlatform.Core.SegmentElements
		public Segment SourceSegment { get; internal set; }

		//
		// Summary:
		//     Target segment, which is a sequence of Sdl.LanguagePlatform.Core.SegmentElements
		public Segment TargetSegment { get; internal set; }
	}
}
