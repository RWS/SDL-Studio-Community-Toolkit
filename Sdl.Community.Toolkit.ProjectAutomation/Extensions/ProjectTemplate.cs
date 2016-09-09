using Sdl.ProjectAutomation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sdl.Community.Toolkit.ProjectAutomation
{
    public static class ProjectTemplate
    {

        public static string GetProjectLocation(this ProjectTemplateInfo projectTemplate)
        {
            var pathToTemplate = projectTemplate.Uri.LocalPath;
            var templateXml = XElement.Load(pathToTemplate);
            var settingsGroup =
               templateXml.Descendants("SettingsGroup")
                   .Where(s => s.Attribute("Id").Value.Equals("ProjectTemplateSettings"));
            var location =
                settingsGroup.Descendants("Setting")
                    .Where(id => id.Attribute("Id").Value.Equals("ProjectLocation"))
                    .Select(l => l.Value).FirstOrDefault();
            return location;
        }
    }
}
