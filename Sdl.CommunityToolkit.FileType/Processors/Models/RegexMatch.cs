using Sdl.Community.Toolkit.FileType.Processors.API;

namespace Sdl.Community.Toolkit.FileType.Processors.Models
{
	public class RegexMatch: IRegexMatch
	{
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

		public object Clone()
		{
			return new RegexMatch
			{
				Rule = Rule.Clone() as IMatchRule,
				Index = Index,
				Value = Value,
				Type = Type
			};
		}
	}
}