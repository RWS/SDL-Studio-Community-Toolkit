using System;
using System.IO;
using Sdl.Community.Toolkit.Core;

namespace Sdl.Community.Toolkit.LanguagePlatform.Models
{
	/// <summary>
	/// Path info
	/// </summary>
	public class PathInfo
	{
		private string _productPath;
		private string _temporaryResourcesPath;
		private readonly string _productName;

		/// <summary>
		/// Path Info
		/// </summary>
		public PathInfo() : this(null) { }

		/// <summary>
		/// Path Info
		/// </summary>
		/// <param name="productName">The default product name is taken from the version of Studio that is identified from the current app domain</param>
		public PathInfo(string productName)
		{
			if (string.IsNullOrEmpty(productName))
			{
				var studio = new Studio();
				var studioVersion = studio.GetStudioVersion();
				if (studioVersion != null)
				{
					productName = studioVersion.PublicVersion.Substring(4);					
				}
			}

			if (string.IsNullOrEmpty(productName))
			{
				throw new Exception("The product name cannot be null; unable to locate the version of studio from the current app domain");
			}

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
				{
					return _productPath;
				}

				_productPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _productName);
				if (!Directory.Exists(_productPath))
				{
					Directory.CreateDirectory(_productPath);
				}

				return _productPath;
			}
		}

		// Summary:
		//     The default temporary resources path
		public virtual string TemporaryResourcesPath
		{
			get
			{
				if (!string.IsNullOrEmpty(_temporaryResourcesPath))
				{
					return _temporaryResourcesPath;
				}

				_temporaryResourcesPath = Path.Combine(ProductPath, "Temporary Resources");
				if (!Directory.Exists(_temporaryResourcesPath))
				{
					Directory.CreateDirectory(_temporaryResourcesPath);
				}

				return _temporaryResourcesPath;
			}
		}

		//
		// Summary:
		//     The default TM name
		public virtual string TmName => "TM.[SourceLanguageName]-[TargetLanguageName].sdltm";
	}
}
