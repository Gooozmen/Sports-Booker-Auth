$ToolkitPath = Resolve-Path "..\Dependencies\Toolkit*\lib\Tasks.ps1"
$EnvFile = Resolve-Path "..\.env"

.$ToolkitPath

$configuration = "Debug"

#PATHS
$SolutionPath = Resolve-Path ("..\src\sports-booker-auth.sln")
$OutputPath = Resolve-Path ("..\Output")
$ArtifactsPath = Resolve-Path ("..\Artifacts")
$DockerFilePath = Split-Path (Resolve-Path ("..\dockerfile")) -Parent 
$SourceFolder = $OutputPath
$OutputFolder = $ArtifactsPath
$TestsLogOutput = $ArtifactsPath
$ArtifactsFolder = $ArtifactsPath 
$DestinationFolder = "$ArtifactsFolder\Application.zip"
$ProjectArtifact = $DestinationFolder

#Others
$ApplicationName = "sports-booker-auth"
$Version = "1.0.0.0"
$Identifier = "$ApplicationName"

$Port= 8080

$ImageVersion = $Version
$Username = $env:NUGET_USERNAME
$Token = $env:NUGET_PASSWORD