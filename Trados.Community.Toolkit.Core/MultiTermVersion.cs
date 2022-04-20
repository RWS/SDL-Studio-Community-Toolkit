using System;

namespace Trados.Community.Toolkit.Core
{
    public class MultiTermVersion
    {
        public string Version { get; set; }
        public string PublicVersion { get; set; }
        public string InstallPath { get; set; }
        public Version ExecutableVersion { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0} - {1}", PublicVersion, ExecutableVersion);
        }
    }
}