using System.Xml.Serialization;

namespace Sdl.Community.Toolkit.LanguagePlatform.XliffConverter
{
	public class Header
	{
		[XmlElement("tool")]
		public Tool[] Tools { get; set; }
	}
}
