$ToolkitPath = Resolve-Path "..\Dependencies\Toolkit*\lib\Tasks.ps1"
. $ToolkitPath

# Debug: The default configuration, typically used for development. Includes debug symbols and disables optimizations.
# Release: Used for production builds. Optimizations are enabled, and debug symbols are excluded.
$configuration = "Debug"
$SolutionPath = Resolve-Path ("..\src\sports-booker-auth.sln")
$OutputPath = Resolve-Path ("..\Output")
$ArtifactsPath = Resolve-Path ("..\Artifacts")

$SourceFolder = $OutputPath
$OutputFolder = $ArtifactsPath
$TestsLogOutput = $ArtifactsPath

$ArtifactsFolder = $ArtifactsPath 
$DestinationFolder = "$ArtifactsFolder\Application.zip"
$ProjectArtifact = $DestinationFolder