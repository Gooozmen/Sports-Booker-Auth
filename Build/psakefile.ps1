$ToolkitPath = Resolve-Path "..\Dependencies\Toolkit*\lib\Tasks.ps1"
$EnvFile = Resolve-Path "..\.env"

.$ToolkitPath

$configuration = "Debug"

#PATHS
$SolutionPath = Resolve-Path ("..\src\CourtBooker-Auth.sln")
$OutputPath = Resolve-Path ("..\Output")

#Artifacts
$ArtifactsPath = Resolve-Path ("..\Artifacts")
$ArtifactsFolder = $ArtifactsPath 

#Tests
$TestsLogOutput = $ArtifactsPath
$TestDllPath = "$OutputPath\Tests.dll"
$TestsLogOutput = $ArtifactsPath

#Application
$ApplicationName = "courtbooker-auth"
$ContainerServiceName = $ApplicationName 

#Docker
$DockerFilePath = Split-Path (Resolve-Path ("..\dockerfile")) -Parent 
$DockerComposePath = $DockerFilePath
$ImageVersion = "1.0.$env:DOCKER_IMAGE_VERSION"

$Username = $env:NUGET_USERNAME
$Token = $env:NUGET_PASSWORD