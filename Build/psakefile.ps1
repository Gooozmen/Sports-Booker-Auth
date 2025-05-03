$ToolkitPath = Resolve-Path "..\Dependencies\Toolkit*\lib\Tasks.ps1"
$EnvFile = Resolve-Path "..\.env"

.$ToolkitPath

$configuration = "Debug"

#PATHS
$SolutionPath = Resolve-Path ("..\src\sports-booker-auth.sln")
$OutputPath = Resolve-Path ("..\Output")

#Docker
$DockerFilePath = Split-Path (Resolve-Path ("..\dockerfile")) -Parent 
$ImageVersion = $Version
$Username = $env:NUGET_USERNAME
$Token = $env:NUGET_PASSWORD
$Port= 8080

#Artifacts
$ArtifactsPath = Resolve-Path ("..\Artifacts")
$ArtifactsFolder = $ArtifactsPath 

#Tests
$TestsLogOutput = $ArtifactsPath
$TestDllPath = "$OutputPath\Tests.dll"
$TestsLogOutput = $ArtifactsPath

#Others
$ApplicationName = "sports-booker-auth"
$Version = "1.0.0.0"
$Identifier = "$ApplicationName"

task Deploy-ApplicationContainer -depends Build-DockerContainer{
    $currentDir = Get-Location
    Set-Location $DockerFilePath
    docker-compose --env-file $EnvFile up -d
    Set-Location $currentDir
}