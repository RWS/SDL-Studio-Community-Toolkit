using System.Xml.Serialization;

namespace Trados.Community.Toolkit.LanguagePlatform.Models.Xliff
{
	public class Header
	{
		[XmlElement("tool")]
		public Tool[] Tools { get; set; }

		/// <summary>
		/// Required constructor for XML serialization
		/// </summary>
		private Header()
		{
			
		}
	}
}
