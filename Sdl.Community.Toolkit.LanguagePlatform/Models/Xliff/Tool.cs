﻿using System.Xml.Serialization;

namespace Sdl.Community.Toolkit.LanguagePlatform.Models.Xliff
{
	public class Tool
	{
		[XmlAttribute("tool-name")]
		public string ToolName { get; set; }

		[XmlAttribute("tool-version")]
		public string ToolVersion { get; set; }

		[XmlAttribute("tool-id")]
		public string ToolId { get; set; }
	}
}
