# .NET API Development & Automation Toolkit

This repository provides an automated environment for building, testing, and deploying a .NET 9 API, including local development configuration and Docker support. PowerShell and Psake are used as the foundation for workflow automation.

---

## 📦 Prerequisites

Make sure the following tools are installed **before running any scripts**:

| Tool            | Version / Notes                                     |
|------------------|-----------------------------------------------------|
| [Chocolatey](https://chocolatey.org/install) | Package manager for Windows |
| NuGet CLI       | Install via `choco install nuget.commandline`       |
| PowerShell Core | Recommended version: latest                         |
| .NET SDK        | `9.0.205` or higher                                 |
| GitHub Secrets  | Required to push Docker image — ask the repo owner |

---

## 🛠️ First Time Setup

> ⚠️ Before running any build or deployment script, run the setup script first.

In your **development `appsettings.json`**, go to the `EntityFramework` section and set:

"ExecuteReBuild": true


This will:

* Recreate the local database
* Apply migrations
* Seed initial data

---

## ⚙️ Toolkit Overview

This project includes a **custom PowerShell-based toolkit** that uses [Psake](https://github.com/psake/psake) to automate common dev tasks.

### Toolkit Features

* Restore and build .NET solutions
* Run tests
* Build and push Docker images
* Deploy containers locally

---

## 📁 Environment Configuration Files

### `.nuget`

Used for restoring packages via NuGet CLI. A global `nuget.config` file can be included to point to private/public sources.

---

### `.env` (Root Level)

Used for Docker builds and local container configuration.

```env
ASPNETCORE_ENVIRONMENT=Docker
JWT__KEY=
JWT__ISSUER=http://
JWT__AUDIENCE=http://
CONNECTIONSTRINGS__AUTHDB="Host=;Port=;Database=;Username=;Password=;"
IMAGE_IDENTIFIER=ghcr.io/username/repo-name
IMAGE_TAG= integer
CONTAINER_NAME=
```

> ⚠️ This file should be listed in `.gitignore` and **never committed** with real secrets.

---

### `Env/development.env.ps1`

Used by `setup.ps1` to inject variables into the local development environment.

```env

$env:NUGET_PASSWORD = ""
$env:NUGET_USERNAME = ""
$env:AUTH_DB = "Host=;Port=;Database=;Username=;Password=;" 
$env:ELASTIC = "http://:"
$env:KEYVAULT = "https://XXXXX.vault.azure.net/"
$env:JWT_KEY = ""
$env:JWT_ISSUER = "http://"
$env:JWT_AUDIENCE = "http://"
$env:ASPNETCORE_ENVIRONMENT = ""
```

> Add this file manually. Do not commit to the repository.

---

## 📁 Build Folder Scripts

These PowerShell scripts live in the `/build` directory and must be executed **after running `setup.ps1`**.

| Script                 | Description                                                                                             |
| ---------------------- | ------------------------------------------------------------------------------------------------------- |
| `setup.ps1`            | - Sets environment variables for local dev  <br> - Installs dependencies <br> - Loads toolkit and Psake |
| `build-solution.ps1`   | Restores and builds the .NET solution                                                                   |
| `build-image.ps1`      | Builds the Docker image                                                                                 |
| `start-container.ps1` | Deploys the image to the local Docker engine                                                            |
| `publish-image.ps1`    | Pushes the image to GitHub Container Registry                                                           |
| `run-tests.ps1`        | Runs the test project in the solution                                                                   |

---

## ✅ Usage Example

```powershell
# Initial setup (only needed once)
./build/setup.ps1

# Build the solution
./build/build-solution.ps1

# Run tests
./build/run-tests.ps1

# Build Docker image
./build/build-image.ps1

# Deploy locally
./build/deploy-container.ps1

# Publish image to GitHub Container Registry
./build/publish-image.ps1
```

---

## 📌 Notes

* The toolkit is designed to reduce repetitive DevOps tasks during development.
* GitHub Actions workflows can integrate these scripts for CI/CD pipelines.

---