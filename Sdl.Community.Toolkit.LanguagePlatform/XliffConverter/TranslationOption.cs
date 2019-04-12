using System.Xml.Serialization;

namespace Sdl.Community.Toolkit.LanguagePlatform.XliffConverter
{
	public class TranslationOption
	{
		public TranslationOption(string toolId, TargetTranslation translation)
		{
			ToolId = toolId;
			Translation = translation;
		}

		/// <summary>
		/// Required constructor for XML serialization
		/// </summary>
		private TranslationOption()
		{
			
		}

		[XmlAttribute("tool-id")]
		public string ToolId { get; set; }

		[XmlAttribute("date")]
		public string Date { get; set; }

		[XmlElement("target")]
		public TargetTranslation Translation { get; set; }
	}
}
