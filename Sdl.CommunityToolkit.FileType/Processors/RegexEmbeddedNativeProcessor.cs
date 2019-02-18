using System.Collections.Generic;
using System.Diagnostics;
using Sdl.Community.Toolkit.FileType.Processors.API;
using Sdl.Community.Toolkit.FileType.Processors.Services;
using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.Buffer;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdl.Community.Toolkit.FileType.Processors
{
	public class RegexEmbeddedNativeProcessor: AbstractNativeFileTypeComponent, INativeExtractionContentProcessor, ISettingsAware
	{
		private readonly NativeBuffer _buffer = new NativeBuffer();
		private readonly IEmbeddedContentProcessorSettings _settings;
		private readonly RegexProcessorService _regexProcessorService;

		private bool _isEnabled;
		private List<string> _structureInfos;
		private List<IMatchRule> _matchRules;
		private ISettingsAware _settingsAwareImplementation;

		public RegexEmbeddedNativeProcessor(IEmbeddedContentProcessorSettings settings)
		{			
			_settings = settings;
			_isEnabled = false;
			_structureInfos = new List<string>();
			_matchRules = new List<IMatchRule>();

			_regexProcessorService = new RegexProcessorService();
		}		

		public List<string> StructureInfo
		{
			get => _structureInfos;
			set => _structureInfos = value;
		}

		public List<IMatchRule> MatchRules
		{
			get => _matchRules;
			set => _matchRules = value;
		}

		public bool IsEnabled
		{
			get => _isEnabled;
			set => _isEnabled = value;
		}

		public IEmbeddedContentProcessorSettings Settings => _settings;

		public INativeExtractionContentHandler Output
		{
			get => _buffer.ExtractionOutput;
			set => _buffer.Output = value;
		}

		public void ChangeContext(IContextProperties newContexts)
		{
			_buffer.ChangeContext(newContexts);

			if (_isEnabled && _regexProcessorService.HasEmbeddedContext(_structureInfos, newContexts))
			{
				//we hold the buffer when processing embedded content
				if (!_buffer.IsHolding)
				{
					_buffer.Hold();
				}
			}
			else
			{
				//no longer in embedded content, release the buffer!
				if (_buffer.IsHolding)
				{
					_buffer.Release();
				}
			}
		}

		public void CustomInfo(ICustomInfoProperties info)
		{
			_buffer.CustomInfo(info);
		}

		public void InlineEndTag(IEndTagProperties tagInfo)
		{
			_buffer.InlineEndTag(tagInfo);
		}

		public void InlinePlaceholderTag(IPlaceholderTagProperties tagInfo)
		{
			_buffer.InlinePlaceholderTag(tagInfo);
		}

		public void InlineStartTag(IStartTagProperties tagInfo)
		{
			_buffer.InlineStartTag(tagInfo);
		}

		public void LocationMark(LocationMarkerId markerId)
		{
			_buffer.LocationMark(markerId);
		}

		public void LockedContentEnd()
		{
			_buffer.LockedContentEnd();
		}

		public void LockedContentStart(ILockedContentProperties lockedContentInfo)
		{
			_buffer.LockedContentStart(lockedContentInfo);
		}

		public void RevisionStart(IRevisionProperties revisionInfo)
		{
			_buffer.RevisionStart(revisionInfo);
		}

		public void RevisionEnd()
		{
			_buffer.RevisionEnd();
		}

		public void CommentStart(ICommentProperties commentInfo)
		{
			_buffer.CommentStart(commentInfo);
		}

		public void CommentEnd()
		{
			_buffer.CommentEnd();
		}

		public void ParagraphComments(ICommentProperties commentInfo)
		{
			_buffer.ParagraphComments(commentInfo);
		}

		public void StructureTag(IStructureTagProperties tagInfo)
		{
			_buffer.StructureTag(tagInfo);
		}

		public void Text(ITextProperties textInfo)
		{
			if (_buffer.IsHolding && _matchRules.Count > 0)			
			{
				//process embedded content
				ProcessEmbeddedContent(textInfo);
			}
			else
			{
				//process normal text
				_buffer.Text(textInfo);
			}
		}

		public void InitializeSettings(ISettingsBundle settingsBundle, string configurationId)
		{
			ApplySettings();
		}

		/// <summary>
		/// Process embedded text content with defined inline rules
		/// </summary>
		/// <param name="textInfo"></param>
		private void ProcessEmbeddedContent(ITextProperties textInfo)
		{
			var inputText = textInfo.Text;

			//first iterate whole string and store object for later use
			var matches = _regexProcessorService.ApplyRegexRules(inputText, _matchRules);

			var lastMatchIndex = 0;
			foreach (var match in matches)
			{
				if (match.Index > lastMatchIndex)
				{
					_buffer.Text(PropertiesFactory.CreateTextProperties(inputText.Substring(lastMatchIndex, match.Index - lastMatchIndex)));
				}
				else if (lastMatchIndex > match.Index)
				{
					//there are multiple matches found in one string, if already applied match index is behind current, just skip it
					continue;
				}

				switch (match.Type)
				{
					case TagType.Placeholder:
						WritePlaceholderTag(match.Value, match.Rule);
						break;
					case TagType.TagPairOpening:
						WriteStartTag(match.Value, match.Rule);
						break;
					case TagType.TagPairClosing:
						WriteEndTag(match.Value, match.Rule);
						break;
					default:
						Debug.Assert(false, "Tag match type not defined!");
						break;
				}

				lastMatchIndex = match.Index + match.Value.Length;
			}

			//output anything that comes after input
			if (lastMatchIndex < inputText.Length)
			{
				_buffer.Text(PropertiesFactory.CreateTextProperties(inputText.Substring(lastMatchIndex, inputText.Length - lastMatchIndex)));
			}
		}

		private void WriteStartTag(string tagContent, IMatchRule rule)
		{
			//treat non-translatable content as locked
			if (!rule.IsContentTranslatable)
			{
				var lockedPoperties =
					PropertiesFactory.CreateLockedContentProperties(LockTypeFlags.Manual);

				_buffer.LockedContentStart(lockedPoperties);
			}

			if (!string.IsNullOrEmpty(tagContent))
			{
				var startProperties =
					_regexProcessorService.CreateStartTagProperties(PropertiesFactory, tagContent, rule);
				startProperties.SetMetaData(EmbeddedContentConstants.EmbeddedContentMetaKey, tagContent);

				_buffer.InlineStartTag(startProperties);
			}
		}

		private void WriteEndTag(string tagContent, IMatchRule rule)
		{
			if (!string.IsNullOrEmpty(tagContent))
			{
				var endProperties =
					_regexProcessorService.CreateEndTagProperties(PropertiesFactory, tagContent, rule);
				endProperties.SetMetaData(EmbeddedContentConstants.EmbeddedContentMetaKey, tagContent);

				_buffer.InlineEndTag(endProperties);
			}

			//end of locked content
			if (!rule.IsContentTranslatable)
			{
				_buffer.LockedContentEnd();
			}
		}

		private void WritePlaceholderTag(string tagContent, IMatchRule rule)
		{
			if (!string.IsNullOrEmpty(tagContent))
			{
				var placeholderProps =
					_regexProcessorService.CreatePlaceholderTagProperties(PropertiesFactory, tagContent, rule);
				placeholderProps.SetMetaData(EmbeddedContentConstants.EmbeddedContentMetaKey, tagContent);

				_buffer.InlinePlaceholderTag(placeholderProps);
			}
		}

		private void ApplySettings()
		{
			_isEnabled = _settings.Enabled;

			_structureInfos.Clear();
			foreach (var item in _settings.StructureInfos)
			{
				_structureInfos.Add(item);
			}

			_matchRules.Clear();
			foreach (var rule in _settings.MatchRules)
			{
				_matchRules.Add(rule);
			}
		}	
	}
}
