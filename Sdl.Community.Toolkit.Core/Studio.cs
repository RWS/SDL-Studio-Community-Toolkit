using Sdl.Community.Toolkit.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdl.Community.Toolkit.Core
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
