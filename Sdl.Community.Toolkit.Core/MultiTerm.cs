using Sdl.Community.Toolkit.Core.Services;
using System.Collections.Generic;

namespace Sdl.Community.Toolkit.Core
{
    public class MultiTerm
    {
        public List<MultiTermVersion> GetInstalledMultiTermVersion()
        {
            var multiTermVersionService = new MultiTermVersionService();
            return multiTermVersionService.GetInstalledMultiTermVersions();
        }

        public MultiTermVersion GetMultiTermVersion()
        {
            var multiTermVersionService = new MultiTermVersionService();
            return multiTermVersionService.GetMultiTermVersion();
        }
    }
}