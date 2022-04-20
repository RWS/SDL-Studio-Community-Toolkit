using Trados.Community.Toolkit.FileType.Processors.API;

namespace Trados.Community.Toolkit.FileType.Processors.Internal
{
	internal class RegexMatch
	{
		public enum TagType
		{
			Placeholder, TagPairOpening, TagPairClosing
		}

		public IMatchRule Rule
		{
			get;
			set;
		}

		public int Index
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public TagType Type
		{
			get;
			set;
		}		
	}
}