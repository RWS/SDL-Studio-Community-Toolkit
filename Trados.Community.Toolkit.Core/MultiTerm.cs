using System.Collections.Generic;
using Trados.Community.Toolkit.Core.Services;

namespace Trados.Community.Toolkit.Core
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