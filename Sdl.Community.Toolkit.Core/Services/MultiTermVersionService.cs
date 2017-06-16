using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sdl.Community.Toolkit.Core.Services
{
    public class MultiTermVersionService
    {
        private const string InstallLocation64Bit = @"SOFTWARE\Wow6432Node\SDL\";
        private const string InstallLocation32Bit = @"SOFTWARE\SDL";


        private readonly Dictionary<string, string> _supportedMultiTermVersions = new Dictionary<string, string>
        {
            {"MultiTerm11", "SDL MultiTerm 2014"},
            {"MultiTerm12", "SDL MultiTerm 2015"},
            {"MultiTerm14", "SDL MultiTerm 2017"},
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
            var multiTermKey = Registry.LocalMachine.OpenSubKey(string.Format(@"{0}\{1}", registryPath, multiTermVersion));
            if (multiTermKey != null)
            {
                CreateMultiTermVersion(multiTermKey, multiTermVersion, multiTermPublicVersion);
            }
        }

        private void CreateMultiTermVersion(RegistryKey multiTermKey, string version, string publicVersion)
        {
            var installLocation = multiTermKey.GetValue("InstallLocation").ToString();
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
            var assembly = Assembly.LoadFile(string.Format(@"{0}\{1}", installLocation, "MultiTerm.exe"));
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
            var assembly = Assembly.
                LoadFile(string.Format(@"{0}\{1}",
                        AppDomain.CurrentDomain.BaseDirectory,
                        "MultiTerm.exe"));
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var currentVersion = new Version(versionInfo.FileVersion);
            var installedMultiTermVersion = _installedMultiTermVersions.
                    Find(x => x.ExecutableVersion.MajorRevision.Equals(currentVersion.MajorRevision));

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
