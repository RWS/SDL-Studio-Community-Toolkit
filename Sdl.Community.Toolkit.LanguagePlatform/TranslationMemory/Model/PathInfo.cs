using System;
using System.IO;

namespace Sdl.Community.Toolkit.LanguagePlatform.TranslationMemory.Model
{
	/// <summary>
	/// Environment path info
	/// </summary>
	public class PathInfo
	{
		private string _productPath;
		private string _tmPath;
		private readonly string _productName;

		/// <summary>
		/// Path Info
		/// </summary>
		/// <param name="productName"></param>
		public PathInfo(string productName)
		{
			_productName = productName;
		}
		//

		// Summary:
		//     The default product path, created in the my documents folder
		public virtual string ProductPath
		{
			get
			{
				if (!string.IsNullOrEmpty(_productPath))
					return _productPath;

				_productPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _productName);
				if (!Directory.Exists(_productPath))
					Directory.CreateDirectory(_productPath);

				return _productPath;
			}
		}

		// Summary:
		//     The default TM path
		public virtual string TmPath
		{
			get
			{
				if (!string.IsNullOrEmpty(_tmPath))
					return _tmPath;

				_tmPath = Path.Combine(ProductPath, "TM");
				if (!Directory.Exists(_tmPath))
					Directory.CreateDirectory(_tmPath);

				return _tmPath;
			}
		}

		//
		// Summary:
		//     The default TM name
		public virtual string TmName => "TM.[SourceLanguageName]-[TargetLanguageName].sdltm";	
	}
}
