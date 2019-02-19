using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.Buffer;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdl.Community.Toolkit.FileType.Processors
{
	public class FeedbackRemovalProcessor : INativeGenerationContentProcessor, INativeContentCycleAware, INativeFileTypeComponent
	{
		private readonly NativeBuffer _buffer = new NativeBuffer();
		private Mode _processingMode;
		private bool _bufferContent = true;
		private bool _tqaDeletion = false;

		private enum Mode
		{
			None,
			Remove,
			Add
		}

		public void StructureTag(IStructureTagProperties tagInfo)
		{
			if (!_tqaDeletion)
			{
				_buffer.StructureTag(tagInfo);
			}
		}

		public void InlineStartTag(IStartTagProperties tagInfo)
		{
			if (!_tqaDeletion)
			{
				_buffer.InlineStartTag(tagInfo);
			}
		}

		public void InlineEndTag(IEndTagProperties tagInfo)
		{
			if (!_tqaDeletion)
				_buffer.InlineEndTag(tagInfo);
		}

		public void InlinePlaceholderTag(IPlaceholderTagProperties tagInfo)
		{
			if (!_tqaDeletion)
			{
				_buffer.InlinePlaceholderTag(tagInfo);
			}
		}

		public void Text(ITextProperties textInfo)
		{
			if (!_tqaDeletion)
			{
				_buffer.Text(textInfo);
			}
		}

		public void ChangeContext(IContextProperties newContexts)
		{
			_buffer.ChangeContext(newContexts);
		}

		public void CustomInfo(ICustomInfoProperties info)
		{
			if (!_tqaDeletion)
			{
				_buffer.CustomInfo(info);
			}
		}

		public void LocationMark(LocationMarkerId markerId)
		{
			if (!_tqaDeletion)
			{
				_buffer.LocationMark(markerId);
			}
		}

		public void LockedContentStart(ILockedContentProperties lockedContentInfo)
		{
			if (!_tqaDeletion)
			{
				_buffer.LockedContentStart(lockedContentInfo);
			}
		}

		public void LockedContentEnd()
		{
			if (!_tqaDeletion)
			{
				_buffer.LockedContentEnd();
			}
		}

		public void RevisionStart(IRevisionProperties revisionInfo)
		{
			if (revisionInfo.RevisionType == RevisionType.FeedbackAdded ||
				revisionInfo.RevisionType == RevisionType.FeedbackDeleted ||
				revisionInfo.RevisionType == RevisionType.FeedbackComment)
			{
				_bufferContent = false;
				if (revisionInfo.RevisionType == RevisionType.FeedbackDeleted)
				{
					_tqaDeletion = true;
				}
			}

			if (_bufferContent)
			{
				_buffer.RevisionStart(revisionInfo);
			}
		}

		public void RevisionEnd()
		{
			if (_bufferContent)
				_buffer.RevisionEnd();
			else
			{
				_bufferContent = true;
				_tqaDeletion = false;
			}
		}

		public void CommentStart(ICommentProperties commentInfo)
		{
			if (!_tqaDeletion)
			{
				_buffer.CommentStart(commentInfo);
			}
		}

		public void CommentEnd()
		{
			if (!_tqaDeletion)
			{
				_buffer.CommentEnd();
			}
		}

		public void ParagraphComments(ICommentProperties commentInfo)
		{
			if (!_tqaDeletion)
			{
				_buffer.ParagraphComments(commentInfo);
			}
		}

		public void ParagraphUnitStart(IParagraphUnitProperties properties)
		{
			if (_processingMode != Mode.None)
			{
				_buffer.Hold();
			}

			_buffer.ParagraphUnitStart(properties);
		}

		public void ParagraphUnitEnd()
		{
			if (_processingMode != Mode.None)
			{
				_buffer.Release();
			}

			_buffer.ParagraphUnitEnd();
		}

		public void SegmentStart(ISegmentPairProperties properties)
		{
			if (!_tqaDeletion)
			{
				_buffer.SegmentStart(properties);
			}
		}

		public void SegmentEnd()
		{
			if (!_tqaDeletion)
			{
				_buffer.SegmentEnd();
			}
		}

		public INativeGenerationContentHandler Output
		{
			get => _buffer.GenerationOutput;
			set => _buffer.GenerationOutput = value;
		}
		
		public void SetFileProperties(IFileProperties properties)
		{
		}

		public void StartOfInput()
		{
		}

		public void EndOfInput()
		{
			_buffer.Release();
		}

	
		public IPropertiesFactory PropertiesFactory
		{
			get; set;
		}

		public INativeContentStreamMessageReporter MessageReporter
		{
			get; set;
		}

		public void SetOutputProperties(INativeOutputFileProperties properties)
		{
			if (properties.ContentRestriction == ContentRestriction.Source)
			{
				// Override the processing if we are outputting the source document - no need to try and make any modifications
				_processingMode = Mode.None;
			}
		}

		public void GetProposedOutputFileInfo(IPersistentFileConversionProperties fileProperties, IOutputFileInfo proposedFileInfo)
		{
		}
	}
}
