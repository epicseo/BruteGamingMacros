# Build Script for Brute Gaming Macros
# Usage: .\build.ps1 -Configuration Release -BuildInstaller -Sign

param(
    [ValidateSet("Debug", "Release", "ReleaseMR", "ReleaseHR", "ReleaseLR")]
    [string]$Configuration = "Release",

    [switch]$Clean,
    [switch]$BuildInstaller,
    [switch]$Sign,
    [switch]$RunTests
)

$ErrorActionPreference = "Stop"
$ProjectDir = Split-Path $PSScriptRoot -Parent
$SolutionFile = Join-Path $ProjectDir "BruteGamingMacros.sln"
$ProjectFile = Join-Path $ProjectDir "BruteGamingMacros.csproj"

Write-Host "======================================" -ForegroundColor Cyan
Write-Host "Brute Gaming Macros - Build Script" -ForegroundColor Cyan
Write-Host "Configuration: $Configuration" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

# Get version from version.txt
$VersionFile = Join-Path $PSScriptRoot "version.txt"
if (Test-Path $VersionFile) {
    $Version = (Get-Content $VersionFile).Trim()
    Write-Host "Building version: $Version" -ForegroundColor Yellow
} else {
    $Version = "2.0.0"
    Write-Warning "version.txt not found, using default: $Version"
}

# Clean
if ($Clean) {
    Write-Host "[1/6] Cleaning solution..." -ForegroundColor Yellow
    & msbuild $SolutionFile /t:Clean /p:Configuration=$Configuration /nologo /v:minimal
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Clean failed!"
        exit 1
    }
    Write-Host "✓ Clean completed" -ForegroundColor Green
    Write-Host ""
}

# Restore NuGet packages
Write-Host "[2/6] Restoring NuGet packages..." -ForegroundColor Yellow
$NugetPath = "nuget.exe"
if (-not (Get-Command nuget -ErrorAction SilentlyContinue)) {
    Write-Host "Downloading NuGet.exe..." -ForegroundColor Yellow
    $NugetPath = Join-Path $PSScriptRoot "nuget.exe"
    if (-not (Test-Path $NugetPath)) {
        Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile $NugetPath
    }
}

& $NugetPath restore $SolutionFile
if ($LASTEXITCODE -ne 0) {
    Write-Error "NuGet restore failed!"
    exit 1
}
Write-Host "✓ NuGet restore completed" -ForegroundColor Green
Write-Host ""

# Build
Write-Host "[3/6] Building solution..." -ForegroundColor Yellow
& msbuild $SolutionFile /t:Build /p:Configuration=$Configuration /p:Platform="Any CPU" /m /nologo /v:minimal
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed!"
    exit 1
}
Write-Host "✓ Build completed successfully" -ForegroundColor Green
Write-Host ""

# Determine output directory
$BinPath = Join-Path $ProjectDir "bin\$Configuration"
$ExePath = Join-Path $BinPath "BruteGamingMacros.exe"

if (-not (Test-Path $ExePath)) {
    Write-Error "Executable not found at: $ExePath"
    exit 1
}

# Run tests (if test project exists)
if ($RunTests) {
    Write-Host "[4/6] Running tests..." -ForegroundColor Yellow
    $TestProject = Join-Path $ProjectDir "tests\BruteGamingMacros.Tests\BruteGamingMacros.Tests.csproj"
    if (Test-Path $TestProject) {
        & dotnet test $TestProject --configuration $Configuration --no-build
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Tests failed!"
            exit 1
        }
        Write-Host "✓ Tests passed" -ForegroundColor Green
    } else {
        Write-Warning "Test project not found, skipping tests"
    }
    Write-Host ""
}

# Sign executable
if ($Sign) {
    Write-Host "[5/6] Signing executable..." -ForegroundColor Yellow
    $SignScript = Join-Path $PSScriptRoot "sign.ps1"
    & $SignScript -FilePath $ExePath
    Write-Host ""
} else {
    Write-Host "[5/6] Skipping code signing (use -Sign to enable)" -ForegroundColor Yellow
    Write-Host ""
}

# Build installer
if ($BuildInstaller) {
    Write-Host "[6/6] Building installer..." -ForegroundColor Yellow
    $NsisPath = "C:\Program Files (x86)\NSIS\makensis.exe"
    if (Test-Path $NsisPath) {
        $InstallerScript = Join-Path $ProjectDir "installer\installer.nsi"
        if (Test-Path $InstallerScript) {
            & $NsisPath /DVERSION=$Version $InstallerScript
            if ($LASTEXITCODE -ne 0) {
                Write-Error "Installer build failed!"
                exit 1
            }
            Write-Host "✓ Installer created" -ForegroundColor Green
        } else {
            Write-Warning "Installer script not found at: $InstallerScript"
        }
    } else {
        Write-Warning "NSIS not found. Install from https://nsis.sourceforge.io/"
    }
    Write-Host ""
}

# Create portable ZIP
Write-Host "Creating portable ZIP package..." -ForegroundColor Yellow
$ZipPath = Join-Path $ProjectDir "BruteGamingMacros-v$Version-portable.zip"
if (Test-Path $ZipPath) {
    Remove-Item $ZipPath -Force
}

$FilesToZip = @(
    "BruteGamingMacros.exe",
    "BruteGamingMacros.exe.config",
    "Newtonsoft.Json.dll"
)

$TempDir = Join-Path $env:TEMP "BGM_Portable_$(Get-Date -Format 'yyyyMMddHHmmss')"
New-Item -ItemType Directory -Path $TempDir -Force | Out-Null

foreach ($file in $FilesToZip) {
    $sourcePath = Join-Path $BinPath $file
    if (Test-Path $sourcePath) {
        Copy-Item $sourcePath -Destination $TempDir
    }
}

# Create README for portable
$PortableReadme = @"
Brute Gaming Macros v$Version - Portable Edition

INSTALLATION:
1. Extract all files to a folder of your choice
2. Run BruteGamingMacros.exe as Administrator
3. Configure your game profiles and hotkeys

REQUIREMENTS:
- Windows 10/11 (64-bit)
- .NET Framework 4.8.1
- Administrator privileges

FIRST TIME SETUP:
- The application will create configuration files in:
  %LocalAppData%\BruteGamingMacros\

ANTIVIRUS WARNING:
This application uses legitimate Windows APIs that may trigger
false positives. Please whitelist the application folder.

See: https://github.com/epicseo/BruteGamingMacros/blob/main/docs/ANTIVIRUS.md

SUPPORT:
- GitHub: https://github.com/epicseo/BruteGamingMacros
- Issues: https://github.com/epicseo/BruteGamingMacros/issues

LICENSE: MIT License
"@

Set-Content -Path (Join-Path $TempDir "README.txt") -Value $PortableReadme

Compress-Archive -Path "$TempDir\*" -DestinationPath $ZipPath -Force
Remove-Item $TempDir -Recurse -Force

Write-Host "✓ Portable ZIP created: $ZipPath" -ForegroundColor Green
Write-Host ""

# Generate checksums
Write-Host "Generating SHA256 checksums..." -ForegroundColor Yellow
$ChecksumFile = Join-Path $ProjectDir "checksums.txt"
$ChecksumOutput = @()

$ChecksumOutput += "Brute Gaming Macros v$Version - SHA256 Checksums"
$ChecksumOutput += "Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
$ChecksumOutput += ""

# Checksum for executable
$ExeHash = (Get-FileHash $ExePath -Algorithm SHA256).Hash
$ChecksumOutput += "BruteGamingMacros.exe"
$ChecksumOutput += "  $ExeHash"
$ChecksumOutput += ""

# Checksum for portable ZIP
$ZipHash = (Get-FileHash $ZipPath -Algorithm SHA256).Hash
$ChecksumOutput += "BruteGamingMacros-v$Version-portable.zip"
$ChecksumOutput += "  $ZipHash"
$ChecksumOutput += ""

# Checksum for installer (if exists)
$InstallerPath = Join-Path $ProjectDir "installer\BruteGamingMacros-Setup-v$Version.exe"
if (Test-Path $InstallerPath) {
    $InstallerHash = (Get-FileHash $InstallerPath -Algorithm SHA256).Hash
    $ChecksumOutput += "BruteGamingMacros-Setup-v$Version.exe"
    $ChecksumOutput += "  $InstallerHash"
    $ChecksumOutput += ""
}

Set-Content -Path $ChecksumFile -Value $ChecksumOutput
Write-Host "✓ Checksums saved to: $ChecksumFile" -ForegroundColor Green
Write-Host ""

# Summary
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "Build Summary" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "Version:       $Version" -ForegroundColor White
Write-Host "Configuration: $Configuration" -ForegroundColor White
Write-Host "Executable:    $ExePath" -ForegroundColor White
Write-Host "Portable ZIP:  $ZipPath" -ForegroundColor White
if (Test-Path $InstallerPath) {
    Write-Host "Installer:     $InstallerPath" -ForegroundColor White
}
Write-Host "Checksums:     $ChecksumFile" -ForegroundColor White
Write-Host ""
Write-Host "✓ BUILD COMPLETED SUCCESSFULLY!" -ForegroundColor Green
Write-Host ""

# Exit with success
exit 0
