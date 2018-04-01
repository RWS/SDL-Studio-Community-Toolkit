SDL-Studio-Community-Toolkit
===========

The SDL Studio Community Toolkit is a collection of helper functions. It simplifies and demonstrates common developer tasks building SDL Studio plugins.
## Supported platforms

* .NET 4.5 (Desktop / Server)
* Windows 7 / 8 / 8.1 /10 Store Apps
* SDL Studio 2017 or later

## Getting started

This libraries are build on top of SDL Studio APIs and thus using them also requires having SDL Studio installed.

1. Install Visual Studio 2013 or 2015. The community edition is available for free [here](https://www.visualstudio.com/).

2. Open the solution for an existing SDL Studio plugin or create a new one.

3. In Solution Explorer panel, right click on your project name and select **Manage NuGet packages**. Search for **Sdl.Community.Toolkit**, and choose your desired [NuGet Packages](https://www.nuget.org/packages?q=Sdl.Community.Toolkit) from the list.

![Manage Nuget packages](https://github.com/sdl/SDL-Studio-Community-Toolkit/blob/master/Resources/ManageNugetPackages.png)

4. In your C# class, add the namespaces to the toolkit, for example:

```c#
using Sdl.Community.Toolkit.Integration
```

5. Use the extensions for existing API classes.

## Nuget Packages

NuGet is a standard package manager for .NET applications that is built into Visual Studio. From your open solution choose the *Tools* menu, *NuGet Package Manager*, *Manage NuGet packages for solution...* to open the UI.  Enter one of the package names below to search for it online.

![Community Toolkit Nuget packages](https://github.com/sdl/SDL-Studio-Community-Toolkit/blob/master/Resources/nuget-packages.png)

| NuGet Package Name | description |
| --- | --- |
| [Sdl.Community.Toolkit.Core](https://www.nuget.org/packages/Sdl.Community.Toolkit.Core/) | NuGet package includes helper methods for Studio versions. |
| [Sdl.Community.Toolkit.FileType](https://www.nuget.org/packages/Sdl.Community.Toolkit.FileType/) | NuGet package that includes helper metods for [FileType Support Framework](http://producthelp.sdl.com/SDK/FileTypeSupport/4.0/html/1f5584af-9763-46ff-894b-08127a2421a7.htm) classes, like ISegment, ISegmentPairs or ITranslationOrigin. |
| [Sdl.Community.Toolkit.ProjectAutomation](https://www.nuget.org/packages/Sdl.Community.Toolkit.ProjectAutomation/) | NuGet package that includes helper methods for [ProjectAutomation API](http://producthelp.sdl.com/SDK/ProjectAutomationApi/4.0/html/b986e77a-82d2-4049-8610-5159c55fddd3.htm) classes, like ProjectTemplateInfo. |
| [Sdl.Community.Toolkit.LanguagePlatform](https://www.nuget.org/packages/Sdl.Community.Toolkit.LanguagePlatform/) | NuGet package that includes a helper methods for the LanguagePlatform. Recover additional information from the ISegmentPair (FileTypeSupport.Framework) through the LanguagePlatform, that includes the tokenized segments and word counts |
| [Sdl.Community.Toolkit.Integration](https://www.nuget.org/packages/Sdl.Community.Toolkit.Integration/) |  NuGet package that includes helper methods for [Integration API](http://producthelp.sdl.com/SDK/StudioIntegrationApi/4.1/html/135dcb1c-535b-46a9-8063-b83be4a06d82.htm) classes, like DisplayFilterRowInfo or Document. |

## Features
* Helper functions for working with FileTypeSupport Framework
* Helper functions for working with ProjectAutomation API
* Helper functions for working with LanguagePlatform
* Helper functions for working with Integration API
* Helper functions for handling SDL Studio versioning

## Feedback and Requests

Please use [GitHub issues](https://github.com/sdl/SDL-Studio-Community-Toolkit/issues) for questions or comments.

## Contributing
Do you want to contribute? Here are our [contribution guidelines](https://github.com/sdl/SDL-Studio-Community-Toolkit/blob/master/contributing.md).

## Principles

 - Principle #1: The toolkit will be kept simple.
 - Principle #2: As soon as a comparable feature is available in the SDL Studio, it will be marked as deprecated.
 - Principle #3: All features will be supported for the latest SDL Studio release cycles or until another principle supersedes it.
