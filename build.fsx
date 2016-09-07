#r @"tools/FAKE.Core/tools/FakeLib.dll"

open Fake
open System

let authors = ["Sdl Community"]

//project details
let projectName = "Sdl Studio Community"
let projectDescription="The SDL Studio Community Toolkit is a collection of helper functions. It simplifies and demonstrates common developer tasks building SDL Studio plugins."
let projectSummary = projectDescription

//directories
let buildDir = "./build"
let packagingRoot = "./packaging"
let packagingDir = packagingRoot @@ "studiotoolkit"

let releaseNotes = 
    ReadFile "ReleaseNotes.md"
    |> ReleaseNotesHelper.parseReleaseNotes

let buildMode = getBuildParamOrDefault "buildMode" "Release"

MSBuildDefaults <-{
    MSBuildDefaults with 
        ToolsVersion = Some "14.0"
        Verbosity = Some MSBuildVerbosity.Minimal
}

Target "Clean"(fun _ ->
    CleanDirs[buildDir;packagingRoot;packagingDir]
)

open Fake.AssemblyInfoFile

Target "AssemblyInfo" (fun _ ->
    CreateCSharpAssemblyInfo "./SolutionInfo.cs"
      [ Attribute.Product projectName
        Attribute.Version releaseNotes.AssemblyVersion
        Attribute.FileVersion releaseNotes.AssemblyVersion
        Attribute.ComVisible false ]
)

let setParams defaults = {
    defaults with
        ToolsVersion = Some("14.0")
        Targets = ["Build"]
        Properties =
            [
                "Configuration", buildMode
            ]
        
    }

Target "BuildApp" (fun _ ->
    build setParams "./SDL Studio Community Toolkit.sln"
        |> DoNothing
)

"Clean"
   ==> "AssemblyInfo"
   ==> "BuildApp"

RunTargetOrDefault "Clean"