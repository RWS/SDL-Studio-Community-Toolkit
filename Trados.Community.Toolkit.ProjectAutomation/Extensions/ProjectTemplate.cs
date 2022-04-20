using System.Linq;
using System.Xml.Linq;
using Sdl.ProjectAutomation.Core;

namespace Trados.Community.Toolkit.ProjectAutomation.Extensions
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
