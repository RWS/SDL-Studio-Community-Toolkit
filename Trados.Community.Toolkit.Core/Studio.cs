using System.Collections.Generic;
using Trados.Community.Toolkit.Core.Services;

namespace Trados.Community.Toolkit.Core
{
    public class Studio
    {
        public List<StudioVersion> GetInstalledStudioVersion()
        {
            var studioVersionService = new StudioVersionService();
            return studioVersionService.GetInstalledStudioVersions();
        }

        public StudioVersion GetStudioVersion()
        {
            var studioVersionService = new StudioVersionService();
            return studioVersionService.GetStudioVersion();
        }
    }
}