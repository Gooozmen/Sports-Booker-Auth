function Add-PackageSource([string] $Command){
    $Username = $env:NUGET_USERNAME
    $Password = $env:NUGET_PASSWORD

    if (-not $Username -or -not $Password) {
        Write-Error "Environment variables NUGET_USERNAME or NUGET_PASSWORD are not set."
        return
    }

    nuget sources $Command -Name "github" -Source "https://nuget.pkg.github.com/Gooozmen/index.json" -username $Username -password $Password
}

function Set-PackageSource{
    $sources = nuget sources list
    if($sources -like "*https://nuget.pkg.github.com/Gooozmen/index.json*"){
        Add-PackageSource -Command "update"
        Write-Host "Github source was updated"
    }
    else{
        Add-PackageSource -Command "add"
        Write-Host "Github source was added"
    }
}

function Install-Toolkit{
    nuget install Toolkit -Source "github" -OutputDirectory "..\Dependencies" -NoCache
}

function Import-ToolkitSetup{
    $ToolkitPath = Resolve-Path "..\Dependencies\Toolkit*\lib\setup.ps1"
    . $ToolkitPath
}

function Remove-Folder([string[]] $FolderArray){
    foreach($_ in $FolderArray){
        If(Test-Path("$_")) {Remove-Item $_ -Recurse }
    }
}

function Clear-NugetCache{
    nuget locals all -clear
}

function Set-EnviromentVariables{
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    $EnvsPath = Resolve-Path "..\Env\$env:ASPNETCORE_ENVIRONMENT.env.ps1"
    . $EnvsPath
}

function Set-DotnetSecrets {
    $CurrentPath = Get-Location
    $StartingProjectLocation = Resolve-Path "..\src\Presentation"

    try {
        Set-Location $StartingProjectLocation

        & dotnet user-secrets clear
        & dotnet user-secrets set "Jwt:Key" "$env:JWT_KEY"
        & dotnet user-secrets set "Jwt:Issuer" "$env:JWT_ISSUER"
        & dotnet user-secrets set "Jwt:Audience" "$env:JWT_AUDIENCE"
        & dotnet user-secrets set "ConnectionStrings:AuthDb" "$env:AUTH_DB"
    }
    finally {
        Set-Location $CurrentPath
    }
}

function Verify-EnviromentVariables{

    if ([string]::IsNullOrEmpty($env:NUGET_USERNAME)) 
    {
        Write-Host "NUGET_USERNAME is not set"
        exit 1
    }
    if ([string]::IsNullOrEmpty($env:NUGET_PASSWORD)) 
    {
        Write-Host "NUGET_PASSWORD is not set"
        exit 1
    }
}
# Set-EnviromentVariables
# Set-DotnetSecrets
Verify-EnviromentVariables
Clear-NugetCache
Set-PackageSource
Remove-Folder -FolderArray @("..\Dependencies\psake*","..\Dependencies\Toolkit*","..\Artifacts\**")
Install-Toolkit
Import-ToolkitSetup
