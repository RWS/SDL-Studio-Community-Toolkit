using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Sdl.Community.Toolkit.Core.Services
{
	public class MultiTermVersionService
	{
		private const string InstallLocation64Bit = @"SOFTWARE\Wow6432Node\SDL\";
		private const string InstallLocation32Bit = @"SOFTWARE\SDL\";

		private readonly Dictionary<string, string> _supportedMultiTermVersions = new Dictionary<string, string>
		{
			{"MTCore14", "SDL MultiTerm 2017"},
			{"MTCore15", "SDL MultiTerm 2019"}
		};

		private readonly List<MultiTermVersion> _installedMultiTermVersions;

		public MultiTermVersionService()
		{
			_installedMultiTermVersions = new List<MultiTermVersion>();
			Initialize();
		}

		private void Initialize()
		{
			var registryPath = Environment.Is64BitOperatingSystem ? InstallLocation64Bit : InstallLocation32Bit;
			var sdlRegistryKey = Registry.LocalMachine.OpenSubKey(registryPath);

			if (sdlRegistryKey == null) return;
			foreach (var supportedMultiTermVersion in _supportedMultiTermVersions)
			{
				FindAndCreateMultiTermVersion(registryPath, supportedMultiTermVersion.Key, supportedMultiTermVersion.Value);
			}
		}

		private void FindAndCreateMultiTermVersion(string registryPath, string multiTermVersion, string multiTermPublicVersion)
		{
			var multiTermKey = Registry.LocalMachine.OpenSubKey($@"{registryPath}\{multiTermVersion}\{"Installer"}\{"PersistedProperties"}");
			if (multiTermKey != null)
			{
				CreateMultiTermVersion(multiTermKey, multiTermVersion, multiTermPublicVersion);
			}
		}

		private void CreateMultiTermVersion(RegistryKey multiTermKey, string version, string publicVersion)
		{
			var installLocation = multiTermKey.GetValue("CoreINSTALLDIR").ToString();
			var fullVersion = GetMultiTermFullVersion(installLocation);

			_installedMultiTermVersions.Add(new MultiTermVersion()
			{
				Version = version,
				PublicVersion = publicVersion,
				InstallPath = installLocation,
				ExecutableVersion = new Version(fullVersion)
			});
		}

		private static string GetMultiTermFullVersion(string installLocation)
		{
			var assembly = Assembly.LoadFile($@"{installLocation}\{"MultiTerm.exe"}");
			var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			var fullVersion = versionInfo.FileVersion;
			return fullVersion;
		}

		public List<MultiTermVersion> GetInstalledMultiTermVersions()
		{
			return _installedMultiTermVersions;
		}

		public MultiTermVersion GetMultiTermVersion()
		{
			var assembly = Assembly.LoadFile($@"{AppDomain.CurrentDomain.BaseDirectory}\{"MultiTerm.exe"}");
			var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			var currentVersion = new Version(versionInfo.FileVersion);
			var installedMultiTermVersion = _installedMultiTermVersions.Find(x => x.ExecutableVersion.MajorRevision.Equals(currentVersion.MajorRevision));

			var multiTermVersion = new MultiTermVersion
			{
				InstallPath = assembly.Location,
				Version = installedMultiTermVersion.Version,
				PublicVersion = installedMultiTermVersion.PublicVersion,
				ExecutableVersion = currentVersion
			};

			return multiTermVersion;
		}
	}
}
