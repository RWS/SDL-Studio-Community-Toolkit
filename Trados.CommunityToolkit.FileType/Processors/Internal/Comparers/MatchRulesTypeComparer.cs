using System.Collections.Generic;
using Trados.Community.Toolkit.FileType.Processors.API;

namespace Trados.Community.Toolkit.FileType.Processors.Internal.Comparers
{
	internal class MatchRulesTypeComparer : IComparer<IMatchRule>
	{
		public int Compare(IMatchRule x, IMatchRule y)
		{
			if (x == null || y == null)
			{
				return 0;
			}

			if (x.TagType == TagTypeOption.TagPair && y.TagType == TagTypeOption.Placeholder)
			{
				return 1;
			}

			if (x.TagType == TagTypeOption.Placeholder && y.TagType == TagTypeOption.TagPair)
			{
				return -1;
			}

			return 0;
		}
	}
}
