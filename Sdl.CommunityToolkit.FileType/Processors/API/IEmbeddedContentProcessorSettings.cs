using System;
using System.Collections.Generic;

namespace Sdl.Community.Toolkit.FileType.Processors.API
{
	public interface IEmbeddedContentProcessorSettings: ICloneable
	{
		bool Enabled { get; set; }

		List<string> StructureInfos { get; set; }

		List<IMatchRule> MatchRules { get; set; }
	}
}
