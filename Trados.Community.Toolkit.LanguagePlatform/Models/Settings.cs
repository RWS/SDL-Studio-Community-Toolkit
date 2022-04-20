using System.Globalization;
using Sdl.LanguagePlatform.Core.Tokenization;
using Sdl.LanguagePlatform.TranslationMemory;

namespace Trados.Community.Toolkit.LanguagePlatform.Models
{
	/// <summary>
	/// Settings
	/// </summary>
	public class Settings
	{
		public Settings(CultureInfo sourceLanguage, CultureInfo targetLanguage)
		{
			SourceLanguage = sourceLanguage;
			TargetLanguage = targetLanguage;

			Recognizers = BuiltinRecognizers.RecognizeAll;
			TokenizerFlags = TokenizerFlags.DefaultFlags;
			WordCountFlags = WordCountFlags.DefaultFlags;
		}

		//
		// Summary:
		//     The source language.
		public CultureInfo SourceLanguage { get; set; }

		//
		// Summary:
		//     The target language.
		public CultureInfo TargetLanguage { get; set; }

		//
		// Summary:
		//     Enumerates the known types of special token recognizers.
		public BuiltinRecognizers Recognizers { get; set; }

		//
		// Summary:
		//     Flags controlling tokenizer behaviour
		public TokenizerFlags TokenizerFlags { get; set; }

		//
		// Summary:
		//     Flags controlling word count behaviour
		public WordCountFlags WordCountFlags { get; set; }
	}
}
