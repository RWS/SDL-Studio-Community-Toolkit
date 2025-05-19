using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;

namespace Trados.Community.Toolkit.Core.Services
{
	public class StudioVersionService
	{
		private const string InstallLocation64Bit = @"SOFTWARE\Wow6432Node\SDL\";
		private const string InstallLocation32Bit = @"SOFTWARE\SDL";

		private readonly Dictionary<string, string> _supportedStudioVersions = new Dictionary<string, string>
		{
			{"Studio2", "SDL Trados Studio 2011"},
			{"Studio3", "SDL Trados Studio 2014"},
			{"Studio4", "SDL Trados Studio 2015"},
			{"Studio5", "SDL Trados Studio 2017"},
			{"Studio15", "SDL Trados Studio 2019"},
			{"Studio16", "SDL Trados Studio 2021"},
			{"Studio17", "Trados Studio 2022"},
            {"Studio18", "Trados Studio 2024"}
        };

		private readonly Dictionary<string, string> _supportedStudioShortVersions = new Dictionary<string, string>
		{
			{"Studio2", "2011"},
			{"Studio3", "2014"},
			{"Studio4", "2015"},
			{"Studio5", "2017"},
			{"Studio15", "2019"},
			{"Studio16", "2021"},
			{"Studio17", "2022"},
            {"Studio18", "2024"}
        };

		private readonly List<StudioVersion> _installedStudioVersions;

		public StudioVersionService()
		{
			_installedStudioVersions = new List<StudioVersion>();
			Initialize();
		}

		public List<StudioVersion> GetInstalledStudioVersions()
		{
			return _installedStudioVersions;
		}

		public StudioVersion GetStudioVersion()
		{
			var assembly = Assembly.LoadFile(string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), "SDLTradosStudio.exe"));
			var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			var currentVersion = new Version(versionInfo.FileVersion);
			var installedStudioVersion = _installedStudioVersions.Find(x => x.ExecutableVersion.Major.Equals(currentVersion.Major));
			var studioVersion = new StudioVersion
			{
				InstallPath = assembly.Location,
				Version = installedStudioVersion.Version,
				PublicVersion = installedStudioVersion.PublicVersion,
				ShortVersion = installedStudioVersion.ShortVersion,
				ExecutableVersion = currentVersion
			};

			return studioVersion;
		}

		private void Initialize()
		{
			var registryPath = Environment.Is64BitOperatingSystem ? InstallLocation64Bit : InstallLocation32Bit;
			var sdlRegistryKey = Registry.LocalMachine.OpenSubKey(registryPath);

			if (sdlRegistryKey == null) return;
			foreach (var supportedStudioVersion in _supportedStudioVersions)
			{
				FindAndCreateStudioVersion(registryPath, supportedStudioVersion.Key, supportedStudioVersion.Value);
			}
		}

		private void FindAndCreateStudioVersion(string registryPath, string studioVersion, string studioPublicVersion)
		{
			var studioKey = Registry.LocalMachine.OpenSubKey(string.Format(@"{0}\{1}", registryPath, studioVersion));
			if (studioKey != null)
			{
				CreateStudioVersion(studioKey, studioVersion, studioPublicVersion);
			}
		}

		private void CreateStudioVersion(RegistryKey studioKey, string version, string publicVersion)
		{
			if (studioKey.GetValue("InstallLocation") != null)
			{
				var installLocation = studioKey.GetValue("InstallLocation").ToString();
				var fullVersion = GetStudioFullVersion(installLocation);
				var shortVersion = GetShortVersion(version);
				_installedStudioVersions.Add(new StudioVersion()
				{
					Version = version,
					PublicVersion = publicVersion,
					InstallPath = installLocation,
					ShortVersion = shortVersion,
					ExecutableVersion = new Version(fullVersion)
				});
			}
		}

		private static string GetStudioFullVersion(string installLocation)
		{
			var assembly = Assembly.LoadFile(string.Format(@"{0}\{1}", installLocation.TrimEnd('\\'), "SDLTradosStudio.exe"));
			var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			var fullVersion = versionInfo.FileVersion;
			return fullVersion;
		}

		private string GetShortVersion(string version)
		{
			foreach (var supportedVersion in _supportedStudioShortVersions)
			{
				if (supportedVersion.Key.Equals(version))
				{
					return supportedVersion.Value;
				}
			}
			return string.Empty;
		}
	}
}