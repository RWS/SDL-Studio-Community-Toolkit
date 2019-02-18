using System.Collections.Generic;
using Sdl.Community.Toolkit.FileType.Processors.API;

namespace Sdl.Community.Toolkit.FileType.Processors.Comparers
{
	internal class RegexSearchResultsComparer : IComparer<IRegexMatch>
	{
		public int Compare(IRegexMatch x, IRegexMatch y)
		{
			if (x == null || y == null)
			{
				return 0;
			}

			return x.Index.CompareTo(y.Index);
		}
	}
}
