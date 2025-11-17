# Local Development Quick Start Guide

**Project:** BruteGamingMacros
**Version:** 1.0
**Last Updated:** 2025-11-17

---

## Prerequisites

### Required Software

1. **Visual Studio 2019/2022**
   - Workload: .NET desktop development
   - Component: .NET Framework 4.8.1 SDK

2. **Git for Windows**
   - Download: https://git-scm.com/download/win
   - Version: 2.40+

3. **GitHub CLI** (Recommended)
   ```powershell
   winget install GitHub.cli
   ```

4. **.NET Framework 4.8.1**
   - Download: https://dotnet.microsoft.com/download/dotnet-framework/net481
   - Required for building and running

### Optional Tools

- **NuGet CLI**: `winget install Microsoft.NuGet`
- **MSBuild Structured Log Viewer**: For debugging build issues
- **GitHub Desktop**: Visual Git interface (alternative to CLI)

---

## Initial Setup

### 1. Clone Repository

#### Option A: GitHub CLI (Easiest)
```powershell
# Authenticate
gh auth login

# Clone repository
gh repo clone epicseo/BruteGamingMacros
cd BruteGamingMacros
```

#### Option B: HTTPS with Personal Access Token
```powershell
# Clone
git clone https://github.com/epicseo/BruteGamingMacros.git
cd BruteGamingMacros

# Configure credential storage
git config --global credential.helper wincred
```

#### Option C: SSH
```powershell
# Generate SSH key (if needed)
ssh-keygen -t ed25519 -C "your.email@example.com"

# Add to GitHub: Settings → SSH and GPG keys

# Clone
git clone git@github.com:epicseo/BruteGamingMacros.git
cd BruteGamingMacros
```

### 2. Configure Git

```powershell
# Set your identity
git config user.name "Your Name"
git config user.email "your.email@example.com"

# Set default branch behavior
git config pull.rebase false  # Use merge strategy

# Enable colored output
git config --global color.ui auto

# Set default editor (optional)
git config --global core.editor "code --wait"  # VS Code
# OR
git config --global core.editor "notepad"      # Notepad
```

### 3. Install Dependencies

```powershell
# Restore NuGet packages
nuget restore BruteGamingMacros.sln

# OR using MSBuild
msbuild BruteGamingMacros.sln /t:Restore
```

### 4. Build Project

```powershell
# Build in Release configuration
msbuild BruteGamingMacros.sln /p:Configuration=Release /p:Platform="Any CPU" /t:Rebuild

# OR using build script
.\build\build.ps1 -Configuration Release

# Verify executable exists
Test-Path "bin\Release\BruteGamingMacros.exe"
```

---

## Development Workflow

### Step 1: Create Feature Branch

```powershell
# Update develop branch
git checkout develop
git pull origin develop

# Create your feature branch
git checkout -b feature/your-feature-name

# OR for bug fixes
git checkout -b bugfix/issue-123-description
```

**Branch Naming Conventions:**
- `feature/short-description` - New features
- `bugfix/issue-number-description` - Bug fixes
- `hotfix/critical-issue` - Emergency production fixes
- `refactor/component-name` - Code refactoring
- `docs/section-name` - Documentation updates

### Step 2: Make Changes

Open project in Visual Studio:

```powershell
# Open solution
start BruteGamingMacros.sln
```

**Development Guidelines:**
1. Follow existing code style
2. Add XML comments for public APIs
3. Update CHANGELOG.md for user-facing changes
4. Test with actual game if possible

### Step 3: Test Your Changes

```powershell
# Build and test
msbuild BruteGamingMacros.sln /p:Configuration=Release /t:Rebuild

# Check for warnings
msbuild BruteGamingMacros.sln /p:Configuration=Release /v:detailed 2>&1 | Select-String "warning"

# Run executable
& ".\bin\Release\BruteGamingMacros.exe"
```

**Testing Checklist:**
- [ ] Builds without errors
- [ ] No new compiler warnings
- [ ] Application starts successfully
- [ ] Features work as expected
- [ ] No crashes during operation
- [ ] Memory usage is reasonable
- [ ] Antivirus doesn't block (Windows Defender test)

### Step 4: Commit Changes

```powershell
# Check what changed
git status
git diff

# Stage changes
git add .

# Commit with descriptive message
git commit -m "feat: add auto-targeting system

Implement automatic target selection using memory scanning.
Includes:
- Target prioritization logic
- Configuration UI panel
- Hotkey bindings (F5 toggle)
- Safety checks for invalid targets

Closes #45"
```

**Commit Message Format:**
```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation
- `style`: Formatting changes
- `refactor`: Code restructuring
- `perf`: Performance improvement
- `test`: Adding tests
- `build`: Build system changes
- `ci`: CI/CD changes
- `chore`: Maintenance

**Examples:**
```powershell
git commit -m "fix: resolve crash in ThreadRunner termination"
git commit -m "docs: update README with Windows 11 compatibility"
git commit -m "refactor: extract macro detection into separate class"
```

### Step 5: Push to GitHub

```powershell
# Push your branch
git push -u origin feature/your-feature-name

# OR use GitHub CLI
gh pr create --base develop --fill
```

### Step 6: Create Pull Request

#### Option A: GitHub CLI
```powershell
gh pr create --base develop --title "Add auto-targeting system" --body "
## Description
Implements automatic target selection for enhanced macro performance.

## Changes
- Added TargetSelector class
- Updated UI with target configuration panel
- Implemented F5 hotkey for toggle

## Testing
- Tested with World of Warcraft (retail)
- No memory leaks detected
- Antivirus scan: clean

## Checklist
- [x] Builds successfully
- [x] No compiler warnings
- [x] Tested manually
- [x] Documentation updated
"
```

#### Option B: GitHub Web UI
1. Visit: https://github.com/epicseo/BruteGamingMacros/pulls
2. Click "New pull request"
3. Base: `develop` ← Compare: `feature/your-feature-name`
4. Fill in template
5. Click "Create pull request"

### Step 7: Address Review Comments

```powershell
# Make requested changes
# ... edit files in Visual Studio ...

# Commit additional changes
git add .
git commit -m "refactor: address PR review feedback"
git push origin feature/your-feature-name

# OR amend previous commit
git add .
git commit --amend --no-edit
git push --force origin feature/your-feature-name  # Use carefully!
```

### Step 8: After Merge

```powershell
# Switch to develop
git checkout develop

# Pull latest changes (includes your merged PR)
git pull origin develop

# Delete local feature branch
git branch -d feature/your-feature-name

# Remote branch is auto-deleted by GitHub
```

---

## Common Tasks

### Update Your Branch with Latest Changes

```powershell
# Method 1: Merge (preserves branch history)
git checkout feature/your-feature
git fetch origin
git merge origin/develop

# Method 2: Rebase (cleaner history, preferred)
git checkout feature/your-feature
git fetch origin
git rebase origin/develop

# If conflicts occur during rebase:
# 1. Fix conflicts in Visual Studio
# 2. Stage resolved files: git add .
# 3. Continue rebase: git rebase --continue
```

### Discard All Local Changes

```powershell
# Discard unstaged changes
git restore .

# Discard all changes (including staged)
git reset --hard HEAD

# Clean untracked files
git clean -fd
```

### Undo Last Commit (Keep Changes)

```powershell
git reset --soft HEAD~1

# Changes are now unstaged, make adjustments and recommit
```

### View Commit History

```powershell
# Pretty log
git log --graph --oneline --decorate -20

# File-specific history
git log --follow -- path/to/file.cs

# Search commits
git log --grep="auto-target"

# View specific commit
git show abc123
```

### Create Release

```powershell
# Checkout develop
git checkout develop
git pull origin develop

# Create release branch
git checkout -b release/v2.1.0

# Update version in files
# - build/version.txt → 2.1.0
# - docs/CHANGELOG.md → Add release notes

# Commit version bump
git add .
git commit -m "chore: bump version to v2.1.0"

# Build and test thoroughly
.\build\build.ps1 -Configuration Release -BuildInstaller

# Merge to main
git checkout main
git pull origin main
git merge release/v2.1.0

# Create tag
git tag -a v2.1.0 -m "Release v2.1.0

Production-ready release with:
- Auto-targeting system
- Performance improvements
- Bug fixes

See CHANGELOG.md for full details"

# Push everything
git push origin main
git push origin v2.1.0

# Merge back to develop
git checkout develop
git merge release/v2.1.0
git push origin develop

# Delete release branch
git branch -d release/v2.1.0
```

---

## Building Installer

### Prerequisites

Install NSIS (Nullsoft Scriptable Install System):

```powershell
# Using Chocolatey
choco install nsis -y

# OR download manually
# https://nsis.sourceforge.io/Download
```

### Build Process

```powershell
# Build with installer
.\build\build.ps1 -Configuration Release -BuildInstaller

# Installer output
# → installer\BruteGamingMacros-Setup-v{VERSION}.exe

# Portable package output
# → BruteGamingMacros-v{VERSION}-portable.zip
```

### Test Installer

```powershell
# Run installer (as Administrator)
Start-Process "installer\BruteGamingMacros-Setup-v2.0.1.exe" -Verb RunAs

# Verify installation
Test-Path "$env:ProgramFiles\BruteGamingMacros\BruteGamingMacros.exe"

# Test uninstaller
Start-Process "$env:ProgramFiles\BruteGamingMacros\uninstall.exe" -Verb RunAs
```

---

## Troubleshooting

### Build Errors

#### Error: NuGet packages not restored
```powershell
# Solution 1: Restore manually
nuget restore BruteGamingMacros.sln

# Solution 2: Delete packages and restore
Remove-Item packages -Recurse -Force
nuget restore BruteGamingMacros.sln
```

#### Error: "The type or namespace name 'X' could not be found"
```powershell
# Check .csproj has <Compile Include> for the file
Select-String -Path "*.csproj" -Pattern "Compile Include"

# Rebuild solution
msbuild BruteGamingMacros.sln /t:Clean
msbuild BruteGamingMacros.sln /t:Rebuild
```

#### Error: "Could not load file or assembly 'X'"
```powershell
# Verify DLL is in bin folder
Get-ChildItem bin\Release -Filter "*.dll"

# Check binding redirects in App.config
# Ensure NuGet package versions match
```

### Git Errors

#### Error: "Permission denied (publickey)"
```powershell
# Check SSH key
ssh -T git@github.com

# Add SSH key to agent
ssh-add ~/.ssh/id_ed25519

# Or switch to HTTPS
git remote set-url origin https://github.com/epicseo/BruteGamingMacros.git
```

#### Error: "failed to push some refs"
```powershell
# Pull latest changes first
git pull origin feature/your-branch --rebase

# Then push
git push origin feature/your-branch
```

#### Error: "Your branch is behind"
```powershell
# Pull and merge
git pull origin develop

# OR pull and rebase
git pull origin develop --rebase
```

### Merge Conflicts

```powershell
# 1. See conflicted files
git status

# 2. Open in Visual Studio
#    - Look for conflict markers:
#      <<<<<<< HEAD
#      Your changes
#      =======
#      Their changes
#      >>>>>>> branch-name

# 3. Resolve manually or use VS merge tool

# 4. Mark as resolved
git add path/to/resolved/file.cs

# 5. Complete merge
git commit  # Or `git rebase --continue` if rebasing
```

### Antivirus Issues During Development

```powershell
# Add exclusions in Windows Defender
Add-MpPreference -ExclusionPath "C:\Path\To\BruteGamingMacros"
Add-MpPreference -ExclusionPath "$env:USERPROFILE\source\repos\BruteGamingMacros"

# Temporarily disable real-time protection (as Administrator)
Set-MpPreference -DisableRealtimeMonitoring $true

# Re-enable when done
Set-MpPreference -DisableRealtimeMonitoring $false
```

---

## Code Style Guidelines

### C# Conventions

```csharp
// ✅ Good: PascalCase for classes, methods, properties
public class MacroEngine
{
    public void StartExecution() { }
    public string TargetName { get; set; }
}

// ✅ Good: camelCase for local variables, parameters
public void ProcessTarget(string targetName)
{
    int currentHealth = GetHealth();
}

// ✅ Good: _camelCase for private fields
private readonly ILogger _logger;
private int _retryCount;

// ✅ Good: Clear, descriptive names
public bool IsTargetValid(Entity target)

// ❌ Bad: Abbreviations, unclear names
public bool IsTV(Entity t)
```

### File Organization

```csharp
// File order
using System;              // System namespaces
using System.Collections;
using Newtonsoft.Json;     // External packages
using BruteGamingMacros.Utils;  // Project namespaces

namespace BruteGamingMacros.Features
{
    /// <summary>
    /// Handles automatic target selection and tracking.
    /// </summary>
    public class TargetSelector
    {
        // Constants
        private const int MAX_TARGETS = 10;

        // Fields
        private readonly ILogger _logger;
        private List<Entity> _targets;

        // Constructor
        public TargetSelector(ILogger logger)
        {
            _logger = logger;
        }

        // Properties
        public bool IsEnabled { get; set; }

        // Public methods
        public void SelectBestTarget() { }

        // Private methods
        private bool ValidateTarget() { }
    }
}
```

### XML Documentation

```csharp
/// <summary>
/// Selects the optimal target based on priority rules.
/// </summary>
/// <param name="candidates">List of potential targets to evaluate.</param>
/// <param name="maxDistance">Maximum distance in game units (default: 40).</param>
/// <returns>The selected target entity, or null if none qualify.</returns>
/// <exception cref="ArgumentNullException">Thrown when candidates is null.</exception>
/// <remarks>
/// Target selection prioritizes:
/// 1. Lowest health percentage
/// 2. Closest distance
/// 3. Facing direction
/// </remarks>
public Entity SelectTarget(List<Entity> candidates, float maxDistance = 40.0f)
{
    // Implementation
}
```

---

## Useful Git Aliases

Add to `~/.gitconfig`:

```ini
[alias]
    # Status
    st = status -sb

    # Log
    lg = log --graph --pretty=format:'%C(red)%h%C(reset) -%C(yellow)%d%C(reset) %s %C(green)(%cr) %C(blue)<%an>%C(reset)' --abbrev-commit

    # Quick commit
    ac = "!git add -A && git commit -m"

    # Push current branch
    pushup = "!git push -u origin $(git branch --show-current)"

    # Create feature branch
    feature = "!f() { git checkout develop && git pull && git checkout -b feature/$1; }; f"

    # Sync with develop
    sync = "!git fetch origin && git rebase origin/develop"

    # Undo last commit (keep changes)
    undo = reset HEAD~1 --soft

    # List branches by date
    branches = branch --sort=-committerdate
```

**Usage:**
```powershell
git st                              # Status
git ac "feat: add new feature"      # Add all and commit
git pushup                          # Push current branch
git feature auto-targeting          # Create feature/auto-targeting branch
git sync                            # Sync with develop
```

---

## Resources

### Documentation
- **Project Docs:** `docs/` directory
- **CHANGELOG:** `docs/CHANGELOG.md`
- **Contributing:** `docs/CONTRIBUTING.md`
- **Git Workflow:** `docs/GIT_WORKFLOW_STRATEGY.md`

### External Resources
- **Git Documentation:** https://git-scm.com/doc
- **GitHub Guides:** https://guides.github.com/
- **C# Coding Conventions:** https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
- **.NET Framework 4.8.1:** https://dotnet.microsoft.com/download/dotnet-framework/net481

### Support
- **Issues:** https://github.com/epicseo/BruteGamingMacros/issues
- **Discussions:** https://github.com/epicseo/BruteGamingMacros/discussions

---

## Quick Reference Card

```
┌─────────────────────────────────────────────────────────┐
│ BRUTE GAMING MACROS - DEVELOPMENT CHEAT SHEET          │
├─────────────────────────────────────────────────────────┤
│ Setup                                                   │
│   gh repo clone epicseo/BruteGamingMacros              │
│   cd BruteGamingMacros                                  │
│   nuget restore BruteGamingMacros.sln                   │
│                                                         │
│ Create Feature                                          │
│   git checkout develop                                  │
│   git pull origin develop                               │
│   git checkout -b feature/my-feature                    │
│                                                         │
│ Build & Test                                            │
│   msbuild BruteGamingMacros.sln /p:Configuration=Release│
│   .\build\build.ps1 -Configuration Release              │
│                                                         │
│ Commit & Push                                           │
│   git add .                                             │
│   git commit -m "feat: description"                     │
│   git push -u origin feature/my-feature                 │
│                                                         │
│ Create PR                                               │
│   gh pr create --base develop --fill                    │
│                                                         │
│ After Merge                                             │
│   git checkout develop                                  │
│   git pull origin develop                               │
│   git branch -d feature/my-feature                      │
└─────────────────────────────────────────────────────────┘
```

---

**Version:** 1.0
**Last Updated:** 2025-11-17
**Maintained By:** BruteGamingMacros Development Team
