using System.Collections.Generic;

namespace Trados.Community.Toolkit.FileType.Processors.Internal.Comparers
{
	internal class RegexSearchResultsComparer : IComparer<RegexMatch>
	{
		public int Compare(RegexMatch x, RegexMatch y)
		{
			if (x == null || y == null)
			{
				return 0;
			}

			return x.Index.CompareTo(y.Index);
		}
	}
}
