using Sdl.Community.Toolkit.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
