using System;

namespace Sdl.Community.Toolkit.FileType.Processors.API
{
	public enum TagType
	{
		Placeholder, TagPairOpening, TagPairClosing
	}

	public interface IRegexMatch: ICloneable
	{
		IMatchRule Rule
		{
			get;
			set;
		}

		int Index
		{
			get;
			set;
		}

		string Value
		{
			get;
			set;
		}

		TagType Type
		{
			get;
			set;
		}

		
	}
}
