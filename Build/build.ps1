$psakeFilePath = ".\psakefile.ps1"
& (Resolve-Path "..\Dependencies\psake*\tools\psake\psake.ps1") $psakeFilePath Build