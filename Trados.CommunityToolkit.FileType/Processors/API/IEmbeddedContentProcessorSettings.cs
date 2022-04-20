using System;
using System.Collections.Generic;

namespace Trados.Community.Toolkit.FileType.Processors.API
{
	public interface IEmbeddedContentProcessorSettings: ICloneable
	{
		bool Enabled { get; set; }

		List<string> StructureInfos { get; set; }

		List<IMatchRule> MatchRules { get; set; }
	}
}
