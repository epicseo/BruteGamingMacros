# Deployment Guide - Brute Gaming Macros

This document describes how to build, test, and release Brute Gaming Macros.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Local Development Build](#local-development-build)
- [Building Release Versions](#building-release-versions)
- [Automated Builds (GitHub Actions)](#automated-builds-github-actions)
- [Creating a Release](#creating-a-release)
- [Distribution](#distribution)
- [Troubleshooting](#troubleshooting)

---

## Prerequisites

### Required Software
- **Visual Studio 2017 or later** (recommended: VS 2022)
  - Workload: .NET desktop development
  - Component: .NET Framework 4.8.1 SDK
- **Git** for version control
- **NuGet** (included with Visual Studio)

### Optional Tools
- **7-Zip** or **WinRAR** for manual packaging
- **Visual Studio Code** for lightweight editing

---

## Local Development Build

### Step 1: Clone Repository
```bash
git clone https://github.com/epicseo/BruteGamingMacros.git
cd BruteGamingMacros
```

### Step 2: Restore NuGet Packages
```bash
# Using Visual Studio
# Build > Restore NuGet Packages

# OR using command line
nuget restore BruteGamingMacros.sln
```

### Step 3: Build Solution
```bash
# Using Visual Studio
# Build > Build Solution (Ctrl+Shift+B)

# OR using MSBuild
msbuild BruteGamingMacros.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

### Step 4: Run Application
```bash
# Debug build output
.\bin\Debug\BruteGamingMacros.exe

# Must run as Administrator
# Right-click > Run as Administrator
```

---

## Building Release Versions

The project supports multiple build configurations:

### Standard Release Build
```bash
msbuild BruteGamingMacros.sln /p:Configuration=Release /p:Platform="Any CPU"
```

Output: `bin\Release\BruteGamingMacros.exe`

### Server-Specific Builds

#### Midrate (MR) Build
```bash
msbuild BruteGamingMacros.sln /p:Configuration=Release-MR /p:Platform="Any CPU"
```
Output: `bin\Release-MR\BruteGamingMacros.exe`

#### Highrate (HR) Build
```bash
msbuild BruteGamingMacros.sln /p:Configuration=Release-HR /p:Platform="Any CPU"
```
Output: `bin\Release-HR\BruteGamingMacros.exe`

#### Lowrate (LR) Build
```bash
msbuild BruteGamingMacros.sln /p:Configuration=Release-LR /p:Platform="Any CPU"
```
Output: `bin\Release-LR\BruteGamingMacros.exe`

### Build Configuration Details

Each build sets different preprocessor directives:
- **Release-MR**: Defines `MR_BUILD`, sets `ServerMode = 0`
- **Release-HR**: Defines `HR_BUILD`, sets `ServerMode = 1`
- **Release-LR**: Defines `LR_BUILD`, sets `ServerMode = 2`

---

## Automated Builds (GitHub Actions)

### CI/CD Pipeline

The repository includes GitHub Actions workflows for:
1. **Continuous Integration** (`.github/workflows/build.yml`)
   - Triggers on every push and pull request
   - Builds all configurations
   - Runs tests
   - Validates code quality

2. **Release Automation** (`.github/workflows/release.yml`)
   - Triggers on version tags (e.g., `v2.0.1`)
   - Builds all server variants
   - Creates release packages
   - Publishes to GitHub Releases

### Triggering Automated Builds

#### Push to Branch
```bash
git add .
git commit -m "Your changes"
git push origin your-branch
```
→ Triggers CI build (test only)

#### Create Release Tag
```bash
# Tag the commit
git tag -a v2.0.1 -m "Release version 2.0.1"

# Push tag to GitHub
git push origin v2.0.1
```
→ Triggers full release build with artifacts

---

## Creating a Release

### Manual Release Process

#### 1. Update Version Numbers
Update version in:
- `Utils/AppConfig.cs`: `Version = "v2.0.1"`
- `Properties/AssemblyInfo.cs`: `[assembly: AssemblyVersion("2.0.1.0")]`
- `CHANGELOG.md`: Add release notes

#### 2. Build All Configurations
```bash
# Build all variants
msbuild BruteGamingMacros.sln /p:Configuration=Release /t:Rebuild
msbuild BruteGamingMacros.sln /p:Configuration=Release-MR /t:Rebuild
msbuild BruteGamingMacros.sln /p:Configuration=Release-HR /t:Rebuild
msbuild BruteGamingMacros.sln /p:Configuration=Release-LR /t:Rebuild
```

#### 3. Run Tests
```bash
# Run NUnit tests
nunit3-console.exe BruteGamingMacros.Tests\bin\Release\BruteGamingMacros.Tests.dll
```

#### 4. Package Releases
```powershell
# Create release packages
$version = "2.0.1"

# Standard release
Compress-Archive -Path "bin\Release\*" -DestinationPath "BruteGamingMacros-v$version.zip"

# Server-specific releases
Compress-Archive -Path "bin\Release-MR\*" -DestinationPath "BruteGamingMacros-MR-v$version.zip"
Compress-Archive -Path "bin\Release-HR\*" -DestinationPath "BruteGamingMacros-HR-v$version.zip"
Compress-Archive -Path "bin\Release-LR\*" -DestinationPath "BruteGamingMacros-LR-v$version.zip"
```

#### 5. Create GitHub Release
1. Go to: https://github.com/epicseo/BruteGamingMacros/releases/new
2. Tag: `v2.0.1`
3. Title: `Brute Gaming Macros v2.0.1`
4. Description: Copy from CHANGELOG.md
5. Upload `.zip` files
6. Publish release

### Automated Release Process

Using GitHub Actions (recommended):

```bash
# 1. Update version numbers (as above)
git add .
git commit -m "Release v2.0.1"
git push origin main

# 2. Create and push tag
git tag -a v2.0.1 -m "Release version 2.0.1"
git push origin v2.0.1

# 3. GitHub Actions automatically:
#    - Builds all configurations
#    - Runs tests
#    - Creates release packages
#    - Publishes to GitHub Releases
```

---

## Distribution

### Release Artifacts

Each release includes:
1. **BruteGamingMacros-v{version}.zip** - Standard release
2. **BruteGamingMacros-MR-v{version}.zip** - Midrate server build
3. **BruteGamingMacros-HR-v{version}.zip** - Highrate server build
4. **BruteGamingMacros-LR-v{version}.zip** - Lowrate server build

### Package Contents

Each `.zip` contains:
```
BruteGamingMacros.exe       # Main executable
Config/                      # Configuration folder (empty initially)
Profile/                     # Profile folder (empty initially)
README.md                    # User documentation
```

### Installation for End Users

1. Download appropriate `.zip` for your server
2. Extract to desired folder
3. Run `BruteGamingMacros.exe` as Administrator
4. Configure settings

---

## Troubleshooting

### Build Errors

#### "Could not find package Aspose.Zip"
```bash
# Solution: Restore NuGet packages
nuget restore BruteGamingMacros.sln
```

#### "The type or namespace name 'Fody' could not be found"
```bash
# Solution: Update NuGet packages
Update-Package -Reinstall
```

#### "MSB4019: The imported project was not found"
```bash
# Solution: Ensure Visual Studio is installed with .NET desktop development workload
```

### Runtime Errors

#### "Application requires Administrator privileges"
```bash
# Solution: Run as Administrator
# Right-click BruteGamingMacros.exe > Run as Administrator
```

#### "Could not load file or assembly Newtonsoft.Json"
```bash
# Solution: Ensure all DLLs are in same folder as .exe
# Costura.Fody should embed dependencies automatically
```

### GitHub Actions Errors

#### "Restore NuGet packages failed"
```yaml
# Solution: Check packages.config for invalid versions
# Ensure all package versions are available on nuget.org
```

#### "MSBuild failed with exit code 1"
```bash
# Solution: Check build logs for specific errors
# Common: Missing SDK, incorrect configuration names
```

---

## Build Environment Requirements

### Minimal Requirements
- **OS**: Windows 10 (64-bit) or later
- **RAM**: 4 GB minimum, 8 GB recommended
- **.NET Framework**: 4.8.1 installed
- **Disk Space**: 500 MB for build output

### GitHub Actions Environment
- **Runner**: `windows-latest` (Windows Server 2022)
- **MSBuild**: Included with Visual Studio Build Tools
- **NuGet**: Pre-installed on GitHub-hosted runners

---

## Security Considerations

### Code Signing
For production releases, consider:
1. Obtaining a code signing certificate
2. Signing the `.exe` with `signtool.exe`
3. Distributing via HTTPS only

### Antivirus False Positives
Due to memory reading capabilities:
- Some antivirus software may flag the application
- Documented in README.md
- Consider submitting to antivirus vendors for whitelisting

---

## Version Numbering

Follow [Semantic Versioning](https://semver.org/):
- **MAJOR** (2.x.x): Breaking changes, major rewrites
- **MINOR** (x.1.x): New features, backwards-compatible
- **PATCH** (x.x.1): Bug fixes, backwards-compatible

Examples:
- `v2.0.0` → Initial release
- `v2.0.1` → Bug fix
- `v2.1.0` → New feature
- `v3.0.0` → Breaking change

---

## Rollback Procedure

If a release has critical issues:

### 1. Identify Problem Version
```bash
# Example: v2.0.1 is broken
git tag -l
```

### 2. Revert to Previous Version
```bash
# Checkout previous tag
git checkout v2.0.0

# Create hotfix branch
git checkout -b hotfix/v2.0.2
```

### 3. Create Hotfix Release
```bash
# Fix issue
git add .
git commit -m "Hotfix: Critical bug"

# Tag hotfix
git tag -a v2.0.2 -m "Hotfix release"
git push origin v2.0.2
```

### 4. Mark Broken Release
Edit the broken release on GitHub:
- Add warning in description
- Mark as "pre-release"
- Link to working version

---

## Continuous Delivery Checklist

Before each release:
- [ ] All tests pass locally
- [ ] Version numbers updated
- [ ] CHANGELOG.md updated
- [ ] README.md updated (if needed)
- [ ] Build all configurations successfully
- [ ] Test each .exe variant
- [ ] GitHub Actions CI passes
- [ ] Code reviewed (if team project)
- [ ] Security scan completed
- [ ] Release notes prepared

---

## Support

For build/deployment issues:
- GitHub Issues: https://github.com/epicseo/BruteGamingMacros/issues
- Check TROUBLESHOOTING.md
- Review GitHub Actions logs

---

**Last Updated**: 2025-10-21
**Document Version**: 1.0
