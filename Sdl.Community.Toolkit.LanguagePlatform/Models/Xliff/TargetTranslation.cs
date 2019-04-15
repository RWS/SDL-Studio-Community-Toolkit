using System.Globalization;
using System.Xml.Serialization;
using Sdl.Community.Toolkit.LanguagePlatform.XliffConverter;
using Sdl.LanguagePlatform.Core;

namespace Sdl.Community.Toolkit.LanguagePlatform.Models.Xliff
{
	public class TargetTranslation
	{
		[XmlIgnore]
		private CultureInfo _targetCulture;
		[XmlIgnore]
		private string _targetLanguage;

		public TargetTranslation(CultureInfo targetCulture, string text)
		{
			TargetCulture = targetCulture;
			Text = text;
		}
		private TargetTranslation() { }

		[XmlAttribute("xml:lang")]
		public string TargetLanguage
		{
			get => _targetLanguage;
			set
			{
				_targetLanguage = value;

				// verify TargetCulture isn't the desired value before setting,
				// else you run the risk of infinite loop
				var newTargetCulture = CultureInfo.GetCultureInfo(value);
				if (!Equals(TargetCulture, newTargetCulture))
				{
					TargetCulture = newTargetCulture;
				}
			}
		}

		[XmlIgnore]
		public CultureInfo TargetCulture
		{
			get => _targetCulture;
			set
			{
				_targetCulture = value;

				// verify targetLanguage isn't the desired value before setting,
				// else you run the risk of infinite loop
				var newTargetLanguage = value.ToString();
				if (TargetLanguage != newTargetLanguage)
				{
					TargetLanguage = newTargetLanguage;
				}
			}
		}

		[XmlText]
		public string Text { get; set; }

		[XmlIgnore]
		public Segment TargetSegment => SegmentParser.Parser.ParseLine(Converter.RemoveXliffTags(Text));
	}
}
