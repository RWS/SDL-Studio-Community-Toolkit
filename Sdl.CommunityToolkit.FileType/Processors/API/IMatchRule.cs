using System;
using System.Text.RegularExpressions;
using Sdl.FileTypeSupport.Framework.Core.Settings;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdl.Community.Toolkit.FileType.Processors.API
{
	public enum TagTypeOption
	{
		Placeholder,
		TagPair
	}

	public interface IMatchRule: ICloneable
	{
		SegmentationHint SegmentationHint { get; set; }

		TagTypeOption TagType { get; set; }

		string StartTagRegexValue { get; set; }

		string EndTagRegexValue { get; set; }

		bool IgnoreCase { get; set; }

		bool IsContentTranslatable { get; set; }

		bool IsWordStop { get; set; }

		bool IsSoftBreak { get; set; }

		bool CanHide { get; set; }

		string TextEquivalent { get; set; }

		FormattingGroupSettings Formatting { get; set; }

		Regex BuildStartTagRegex();

		Regex BuildEndTagRegex();
	}
}
