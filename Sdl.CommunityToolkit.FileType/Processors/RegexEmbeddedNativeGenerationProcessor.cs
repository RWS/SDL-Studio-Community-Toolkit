using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdl.Community.Toolkit.FileType.Processors
{
	/// <summary>
	/// Native Generation Processor that checks tags for content processed by the <see cref="RegexEmbeddedNativeProcessor"/> when
	/// the file was extracted and reverts the tags generated back to text so that they will be processed by the filter correctly.
	/// </summary>
	public class RegexEmbeddedNativeGenerationProcessor : AbstractNativeGenerationContentProcessor
	{
		public override void InlineStartTag(IStartTagProperties tagInfo)
		{
			var originalText = tagInfo.GetMetaData(EmbeddedContentConstants.EmbeddedContentMetaKey);

			if (originalText != null)
			{
				var textInfo = PropertiesFactory.CreateTextProperties(originalText);
				Text(textInfo);
			}
			else
			{
				base.InlineStartTag(tagInfo);
			}
		}

		public override void InlineEndTag(IEndTagProperties tagInfo)
		{
			var originalText = tagInfo.GetMetaData(EmbeddedContentConstants.EmbeddedContentMetaKey);

			if (originalText != null)
			{
				var textInfo = PropertiesFactory.CreateTextProperties(originalText);
				Text(textInfo);
			}
			else
			{
				base.InlineEndTag(tagInfo);
			}
		}

		public override void InlinePlaceholderTag(IPlaceholderTagProperties tagInfo)
		{
			var originalText = tagInfo.GetMetaData(EmbeddedContentConstants.EmbeddedContentMetaKey);

			if (originalText != null)
			{
				var textInfo = PropertiesFactory.CreateTextProperties(originalText);
				Text(textInfo);
			}
			else
			{
				base.InlinePlaceholderTag(tagInfo);
			}
		}
	}
}
