using System;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using Sdl.Community.Toolkit.LanguagePlatform.Models.Xliff;
using Sdl.LanguagePlatform.Core;

namespace Sdl.Community.Toolkit.LanguagePlatform.XliffConverter
{
	[XmlRoot(Namespace = "urn:oasis:names:tc:xliff:document:1.2")]
	public class Xliff
	{
		[XmlAttribute]
		public string Version { get; set; }

		[XmlElement("file")]
		public File File { get; set; }

		public Xliff(CultureInfo sourceCulture, CultureInfo targetCulture)
		{
			File = new File(sourceCulture, targetCulture);
		}

		/// <summary>
		/// Required constructor for XML serialization
		/// </summary>
		private Xliff()
		{

		}

		public void AddTranslationUnit(Sdl.LanguagePlatform.TranslationMemory.TranslationUnit translationUnit, string toolId)
		{
			File?.Body?.Add(translationUnit?.SourceSegment, translationUnit?.TargetSegment, toolId);
		}

		public void AddSourceText(string sourceText)
		{
			if (sourceText == null)
			{
				throw new NullReferenceException("Source text cannot be null");
			}

			var seg = new Segment();
			seg.Add(sourceText);
			File?.Body?.Add(seg);
		}

		public void AddSourceSegment(Segment sourceSegment)
		{
			File?.Body?.Add(sourceSegment);
		}

		public void AddTranslation(Segment sourceSegment, Segment targetSegment, string toolId)
		{
			File?.Body?.Add(sourceSegment, targetSegment, toolId);
		}

		public Segment[] GetTargetSegments()
		{
			return File.Body.TranslationUnits.Select(x =>
			{
				var option = x.TranslationList?.FirstOrDefault();
				var segment = option?.Translation?.TargetSegment;

				if (segment == null)
				{
					return null;
				}

				segment.Culture = File.TargetCulture ?? File.SourceCulture;

				return segment;
			}).ToArray();
		}

		public override string ToString()
		{
			return Converter.PrintXliff(this);
		}
	}
}
