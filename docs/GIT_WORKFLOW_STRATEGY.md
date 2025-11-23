# Git Workflow Strategy & Improvement Plan

**Version:** 1.0
**Date:** 2025-11-17
**Project:** BruteGamingMacros

---

## Executive Summary

This document provides a comprehensive analysis of the current Git repository structure and proposes strategic improvements for continuous integration, branch management, and local development workflows. The recommendations follow industry best practices while accommodating the unique constraints of this project.

---

## 1. Current State Analysis

### 1.1 Repository Overview

```
Repository: epicseo/BruteGamingMacros
Technology: C# .NET Framework 4.8.1
Main Branch: main
Current Version: v2.0.1
```

### 1.2 Current Branch Structure

| Branch | Purpose | Status | Last Commit |
|--------|---------|--------|-------------|
| `main` | Production-ready code | Active | 80a0631 (Merge PR #3) |
| `claude/production-ready-deployment-*` | Feature development by AI | Active | a5fb367 (Latest fixes) |
| *(No develop branch)* | Integration branch | Missing | N/A |

**Analysis:**
- ✅ Clean branch structure (only 2 active branches)
- ⚠️ Missing `develop` branch for integration
- ⚠️ Claude branches accumulate without automatic cleanup
- ✅ Main branch is protected and stable

### 1.3 GitHub Actions Workflows

| Workflow | Trigger | Purpose | Status |
|----------|---------|---------|--------|
| `build.yml` | Push to main/develop/claude/* | Multi-config builds | ✅ Active |
| `code-quality.yml` | Push/PR to main/develop | Security & quality checks | ✅ Active |
| `release.yml` | Version tags (v*.*.*) | Create GitHub releases | ✅ Active |
| `build-release.yml` | Version tags (v*) | Build & release automation | ✅ Recently fixed |

**Analysis:**
- ⚠️ **Workflow overlap**: Both `release.yml` and `build-release.yml` trigger on tags
- ⚠️ **Matrix inconsistency**: `build.yml` uses different config names than actual builds
- ✅ **Comprehensive coverage**: Build, test, quality, and release automation
- ⚠️ **Missing**: Auto-cleanup workflow for stale branches

### 1.4 Git Configuration

```ini
User: Claude <noreply@anthropic.com>
Remote: http://local_proxy@127.0.0.1:21010/git/epicseo/BruteGamingMacros
Branch Protection: Can only push to 'claude/*' branches
```

**Constraints:**
- Cannot push directly to `main` (requires PR)
- Cannot delete remote tags (403 errors)
- All development must happen on `claude/*` branches

### 1.5 Documentation Structure

```
docs/
├── ANTIVIRUS.md          - AV whitelisting guide
├── CHANGELOG.md          - Version history
├── CONTRIBUTING.md       - Contribution guidelines
├── DEPLOYMENT_SUMMARY.md - Production deployment docs
├── FAQ.md                - Frequently asked questions
├── INSTALL.md            - Installation instructions
├── PRODUCTION_AUDIT.md   - Security audit
└── QA_CHECKLIST.md       - Quality assurance
```

✅ **Excellent documentation coverage**

---

## 2. Identified Issues & Risks

### 2.1 Critical Issues

#### Issue 1: Duplicate Release Workflows
**Severity:** High
**Impact:** Tag-triggered builds may run twice, wasting resources

**Evidence:**
- `release.yml` triggers on `v*.*.*` tags
- `build-release.yml` triggers on `v*` tags (includes v*.*.*)
- Both create GitHub releases with different package structures

**Recommendation:** Consolidate into single workflow

#### Issue 2: No Branch Cleanup Strategy
**Severity:** Medium
**Impact:** Claude branches accumulate indefinitely

**Evidence:**
- Claude branches persist after PR merge
- No automatic or manual cleanup process
- Repository pollution over time

**Recommendation:** Implement automated cleanup workflow

#### Issue 3: Missing Development Branch
**Severity:** Medium
**Impact:** No integration testing before main branch merge

**Evidence:**
- Workflows reference `develop` branch that doesn't exist
- All features merge directly to main
- No pre-production staging environment

**Recommendation:** Create `develop` branch for integration

### 2.2 Minor Issues

- `.gitignore` includes `*.zip` and `*.7z` - may prevent artifact commits
- Build matrix in `build.yml` uses hyphenated names (Release-MR) but actual configs may differ
- No automated version bumping mechanism
- Missing pull request template
- No commit message linting

---

## 3. Recommended Git Workflow (GitFlow Lite)

### 3.1 Branch Strategy

```
main (production)
│
├── develop (integration)
│   │
│   ├── claude/feature-xyz (AI development)
│   ├── feature/user-auth (manual development)
│   └── bugfix/crash-fix (bug fixes)
│
└── hotfix/critical-bug (emergency fixes)
```

### 3.2 Branch Types & Naming

| Type | Pattern | Lifetime | Merges To | Auto-Delete |
|------|---------|----------|-----------|-------------|
| `main` | `main` | Permanent | N/A | Never |
| `develop` | `develop` | Permanent | `main` | Never |
| `claude/*` | `claude/{description}-{session-id}` | Temporary | `develop` or `main` | After merge |
| `feature/*` | `feature/{short-description}` | Temporary | `develop` | After merge |
| `bugfix/*` | `bugfix/{issue-number}-{description}` | Temporary | `develop` | After merge |
| `hotfix/*` | `hotfix/{issue-number}-{description}` | Temporary | `main` + `develop` | After merge |
| `release/*` | `release/v{version}` | Temporary | `main` + `develop` | After tag |

### 3.3 Workflow Rules

#### Rule 1: Main Branch Protection
```yaml
main:
  - Requires pull request reviews
  - Requires status checks to pass
  - No direct pushes
  - No force pushes
  - Only release/* and hotfix/* can merge
```

#### Rule 2: Develop Branch
```yaml
develop:
  - Integration branch for all features
  - Continuous deployment to staging environment
  - Merges to main only through release/* branches
  - Can receive direct pushes from trusted contributors
```

#### Rule 3: Branch Lifecycle
```
1. Create branch from develop
2. Develop feature with commits
3. Open PR to develop
4. CI/CD runs automated tests
5. Code review approval
6. Merge to develop (squash or merge commit)
7. Auto-delete branch after merge
8. Periodically create release/* from develop
9. Tag release and merge to main
```

---

## 4. Workflow Consolidation Plan

### 4.1 Consolidate Duplicate Workflows

**Current Problem:** Two release workflows compete

**Solution:** Create unified release workflow

```yaml
# .github/workflows/release.yml (CONSOLIDATED)
name: Release

on:
  push:
    tags:
      - 'v*.*.*'  # Semantic versioning only
  workflow_dispatch:
    inputs:
      version:
        description: 'Version (e.g., 2.0.2)'
        required: true

permissions:
  contents: write

jobs:
  # ... consolidated build and release logic
```

**Action Items:**
1. Merge `build-release.yml` into `release.yml`
2. Delete `build-release.yml`
3. Update tag patterns to semantic versioning only
4. Add NSIS installer build from `build-release.yml`
5. Use portable package creation from `build-release.yml`

### 4.2 Fix Build Configuration Matrix

**Current Problem:** Matrix uses `Release-MR` but project may use `ReleaseMR`

**Investigation Needed:**
```powershell
# Check actual build configurations in .csproj
Select-String -Path "*.csproj" -Pattern "Configuration\|Platform"
```

**Solution:** Ensure matrix matches actual MSBuild configurations

### 4.3 Add Branch Cleanup Workflow

**New File:** `.github/workflows/branch-cleanup.yml`

```yaml
name: Branch Cleanup

on:
  pull_request:
    types: [closed]
  schedule:
    - cron: '0 0 * * 0'  # Weekly on Sunday
  workflow_dispatch:

jobs:
  cleanup-merged-branches:
    runs-on: ubuntu-latest
    if: github.event.pull_request.merged == true || github.event_name == 'schedule' || github.event_name == 'workflow_dispatch'

    steps:
      - name: Delete merged branch
        if: github.event_name == 'pull_request'
        run: |
          gh api \
            --method DELETE \
            -H "Accept: application/vnd.github+json" \
            "/repos/${{ github.repository }}/git/refs/heads/${{ github.head_ref }}"
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}

      - name: Checkout
        if: github.event_name != 'pull_request'
        uses: actions/checkout@v4
        with:
          fetch-depth: 0

      - name: Cleanup stale claude branches
        if: github.event_name != 'pull_request'
        run: |
          # Delete claude/* branches older than 30 days with no associated open PR
          git for-each-ref --format='%(refname:short) %(committerdate:unix)' refs/remotes/origin/claude/ |
          while read branch timestamp; do
            days_old=$(( ( $(date +%s) - $timestamp ) / 86400 ))
            if [ $days_old -gt 30 ]; then
              branch_name=${branch#origin/}
              echo "Checking $branch_name (${days_old} days old)"

              # Check if there's an open PR
              open_prs=$(gh pr list --head "$branch_name" --state open --json number --jq length)

              if [ "$open_prs" -eq 0 ]; then
                echo "Deleting stale branch: $branch_name"
                gh api --method DELETE "/repos/${{ github.repository }}/git/refs/heads/$branch_name" || true
              fi
            fi
          done
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

### 4.4 Add PR Template

**New File:** `.github/pull_request_template.md`

```markdown
## Description
<!-- Describe your changes in detail -->

## Type of Change
- [ ] Bug fix (non-breaking change which fixes an issue)
- [ ] New feature (non-breaking change which adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update
- [ ] Refactoring (no functional changes)
- [ ] Build/CI changes

## Testing
<!-- Describe the tests you ran to verify your changes -->
- [ ] Built successfully in Release configuration
- [ ] Tested manually with game [specify which]
- [ ] No new compiler warnings
- [ ] Antivirus false positive check completed

## Checklist
- [ ] My code follows the project's coding standards
- [ ] I have commented my code, particularly in hard-to-understand areas
- [ ] I have updated the CHANGELOG.md
- [ ] My changes generate no new warnings
- [ ] I have updated documentation if needed

## Related Issues
<!-- Link to related issues: Fixes #123, Closes #456 -->

## Screenshots (if applicable)
<!-- Add screenshots to help explain your changes -->
```

---

## 5. Local Development Setup

### 5.1 Initial Repository Clone

```powershell
# Clone repository
git clone https://github.com/epicseo/BruteGamingMacros.git
cd BruteGamingMacros

# Set up user identity
git config user.name "Your Name"
git config user.email "your.email@example.com"

# Set default branch
git config init.defaultBranch main

# Set up pull strategy
git config pull.rebase false  # Merge strategy
```

### 5.2 Create Development Branch

```powershell
# Ensure you're on latest main
git checkout main
git pull origin main

# Create feature branch
git checkout -b feature/your-feature-name

# Or create from develop (recommended)
git checkout develop
git pull origin develop
git checkout -b feature/your-feature-name
```

### 5.3 Development Workflow

```powershell
# 1. Make your changes in Visual Studio

# 2. Stage changes
git add .

# 3. Commit with descriptive message
git commit -m "feat: add new macro detection logic

- Implement advanced pattern matching
- Add unit tests for edge cases
- Update documentation

Closes #123"

# 4. Push to remote
git push -u origin feature/your-feature-name

# 5. Create pull request via GitHub UI or CLI
gh pr create --base develop --title "Add new macro detection logic" --body "Description of changes"

# 6. After PR approval and merge, delete local branch
git checkout develop
git pull origin develop
git branch -d feature/your-feature-name
```

### 5.4 Commit Message Convention

Follow **Conventional Commits** specification:

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation only
- `style`: Code style changes (formatting)
- `refactor`: Code refactoring
- `perf`: Performance improvements
- `test`: Adding tests
- `build`: Build system changes
- `ci`: CI/CD changes
- `chore`: Maintenance tasks

**Examples:**
```
feat(macros): add auto-targeting system

Implement automatic target acquisition using memory scanning.
Includes configuration UI and hotkey bindings.

Closes #45

---

fix(crash): resolve null reference in ThreadRunner.Terminate()

Add null check before accessing thread instance.
Improve termination timeout handling.

Fixes #67

---

docs: update INSTALL.md with Windows 11 requirements

Add note about .NET Framework 4.8.1 dependency.
Include troubleshooting section for antivirus issues.
```

### 5.5 Pushing from Local Windows Desktop

#### Option A: HTTPS with Personal Access Token (PAT)

```powershell
# 1. Generate PAT on GitHub
# Settings → Developer settings → Personal access tokens → Fine-grained tokens
# Permissions: Contents (Read/Write), Pull Requests (Read/Write)

# 2. Clone with HTTPS
git clone https://github.com/epicseo/BruteGamingMacros.git

# 3. Configure credential helper (Windows)
git config --global credential.helper wincred

# 4. On first push, enter PAT when prompted
git push -u origin feature/your-branch
# Username: your-github-username
# Password: ghp_xxxxxxxxxxxxxxxxxxxx (your PAT)
```

#### Option B: SSH with SSH Key

```powershell
# 1. Generate SSH key (if you don't have one)
ssh-keygen -t ed25519 -C "your.email@example.com"
# Save to: C:\Users\YourName\.ssh\id_ed25519

# 2. Add SSH key to ssh-agent
# Start ssh-agent
Start-Service ssh-agent

# Add key
ssh-add C:\Users\YourName\.ssh\id_ed25519

# 3. Copy public key
Get-Content C:\Users\YourName\.ssh\id_ed25519.pub | clip

# 4. Add to GitHub
# Settings → SSH and GPG keys → New SSH key
# Paste the public key

# 5. Clone with SSH
git clone git@github.com:epicseo/BruteGamingMacros.git

# 6. Push works without password
git push -u origin feature/your-branch
```

#### Option C: GitHub CLI (Recommended)

```powershell
# 1. Install GitHub CLI
winget install GitHub.cli

# 2. Authenticate
gh auth login
# Choose: GitHub.com → HTTPS → Yes (authenticate Git) → Login with browser

# 3. Clone repository
gh repo clone epicseo/BruteGamingMacros

# 4. Create and push branches
gh pr create  # Automatically creates branch and PR

# 5. All git operations authenticated automatically
git push origin feature/your-branch
```

### 5.6 Local Build and Test

```powershell
# Navigate to project directory
cd BruteGamingMacros

# Restore NuGet packages
nuget restore BruteGamingMacros.sln

# Build in Release configuration
msbuild BruteGamingMacros.sln /p:Configuration=Release /p:Platform="Any CPU" /t:Rebuild

# Run custom build script (if needed)
.\build\build.ps1 -Configuration Release -BuildInstaller

# Generate portable package
.\build\build.ps1 -Configuration Release

# Check for warnings
msbuild BruteGamingMacros.sln /p:Configuration=Release /v:detailed | Select-String "warning"
```

---

## 6. Automated Version Management

### 6.1 Semantic Versioning Strategy

```
v{MAJOR}.{MINOR}.{PATCH}[-{PRERELEASE}][+{BUILD}]

MAJOR: Breaking changes (incompatible API changes)
MINOR: New features (backward-compatible)
PATCH: Bug fixes (backward-compatible)
PRERELEASE: alpha, beta, rc1, rc2
BUILD: Build metadata
```

**Examples:**
- `v2.0.0` - Major release
- `v2.1.0` - Minor update with new features
- `v2.1.1` - Patch/bugfix release
- `v2.2.0-beta` - Beta release
- `v2.2.0-rc1` - Release candidate

### 6.2 Version Bumping Workflow

**New File:** `.github/workflows/version-bump.yml`

```yaml
name: Version Bump

on:
  workflow_dispatch:
    inputs:
      bump_type:
        description: 'Version bump type'
        required: true
        type: choice
        options:
          - patch
          - minor
          - major
          - prerelease

jobs:
  bump-version:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4
        with:
          token: ${{ secrets.GITHUB_TOKEN }}
          fetch-depth: 0

      - name: Get current version
        id: current_version
        run: |
          VERSION=$(git describe --tags --abbrev=0 2>/dev/null || echo "v0.0.0")
          echo "version=${VERSION#v}" >> $GITHUB_OUTPUT

      - name: Bump version
        id: bump
        run: |
          current="${{ steps.current_version.outputs.version }}"
          IFS='.' read -r major minor patch <<< "$current"

          case "${{ inputs.bump_type }}" in
            major)
              major=$((major + 1))
              minor=0
              patch=0
              ;;
            minor)
              minor=$((minor + 1))
              patch=0
              ;;
            patch)
              patch=$((patch + 1))
              ;;
            prerelease)
              patch=$((patch + 1))
              new_version="${major}.${minor}.${patch}-beta"
              echo "new_version=${new_version}" >> $GITHUB_OUTPUT
              exit 0
              ;;
          esac

          new_version="${major}.${minor}.${patch}"
          echo "new_version=${new_version}" >> $GITHUB_OUTPUT

      - name: Update CHANGELOG
        run: |
          version="${{ steps.bump.outputs.new_version }}"
          date=$(date +%Y-%m-%d)

          # Add new version section to CHANGELOG
          sed -i "s/## \[Unreleased\]/## [Unreleased]\n\n## [$version] - $date/" docs/CHANGELOG.md

      - name: Commit version bump
        run: |
          version="${{ steps.bump.outputs.new_version }}"

          git config user.name "GitHub Actions"
          git config user.email "actions@github.com"

          # Update version file
          echo "$version" > build/version.txt

          git add docs/CHANGELOG.md build/version.txt
          git commit -m "chore: bump version to v$version"
          git tag -a "v$version" -m "Release v$version"
          git push origin develop
          git push origin "v$version"
```

---

## 7. Continuous Deployment Strategy

### 7.1 Deployment Environments

| Environment | Branch | Trigger | Purpose |
|-------------|--------|---------|---------|
| **Development** | `develop` | Every push | Integration testing |
| **Staging** | `release/*` | Release branch creation | Pre-production validation |
| **Production** | `main` | Version tag | Public releases |

### 7.2 Deployment Workflow

```yaml
# .github/workflows/deploy.yml
name: Deploy

on:
  push:
    branches:
      - develop
      - main
    tags:
      - 'v*.*.*'

jobs:
  deploy-dev:
    if: github.ref == 'refs/heads/develop'
    runs-on: windows-latest
    steps:
      # Build and deploy to staging environment
      - name: Deploy to development
        run: echo "Deploy to dev environment"

  deploy-staging:
    if: startsWith(github.ref, 'refs/heads/release/')
    runs-on: windows-latest
    steps:
      # Build and deploy to staging
      - name: Deploy to staging
        run: echo "Deploy to staging environment"

  deploy-production:
    if: startsWith(github.ref, 'refs/tags/v')
    runs-on: windows-latest
    steps:
      # Build and create GitHub release (existing workflow)
      - name: Deploy to production
        uses: ./.github/workflows/release.yml
```

---

## 8. Advanced Git Configuration

### 8.1 Recommended Git Aliases

Add to `.git/config` or `~/.gitconfig`:

```ini
[alias]
    # Short status
    st = status -sb

    # Pretty log
    lg = log --graph --pretty=format:'%Cred%h%Creset -%C(yellow)%d%Creset %s %Cgreen(%cr) %C(bold blue)<%an>%Creset' --abbrev-commit

    # Show last commit
    last = log -1 HEAD --stat

    # Amend last commit
    amend = commit --amend --no-edit

    # Undo last commit (keep changes)
    undo = reset HEAD~1 --soft

    # Discard all changes
    nuke = reset --hard HEAD

    # List branches sorted by last commit
    branches = branch --sort=-committerdate

    # Delete merged branches
    cleanup = "!git branch --merged | grep -v '\\*\\|main\\|develop' | xargs -n 1 git branch -d"

    # Sync with upstream
    sync = "!git checkout main && git pull origin main && git checkout - && git rebase main"

    # Create feature branch
    feature = "!f() { git checkout develop && git pull && git checkout -b feature/$1; }; f"

    # Create bugfix branch
    bugfix = "!f() { git checkout develop && git pull && git checkout -b bugfix/$1; }; f"

    # Quick commit
    cm = commit -m

    # Add all and commit
    ac = "!git add -A && git commit -m"

    # Push current branch
    pushup = "!git push -u origin $(git branch --show-current)"
```

**Usage:**
```powershell
git st                           # Status
git lg                           # Pretty log
git feature my-new-feature       # Create feature branch
git ac "feat: add new feature"   # Add all and commit
git pushup                       # Push current branch
git cleanup                      # Delete merged branches
```

### 8.2 Git Hooks (Local Development)

**Pre-Commit Hook** - Prevent commits with common issues

Create `.git/hooks/pre-commit`:

```bash
#!/bin/sh

# Prevent commits to main/develop
branch=$(git symbolic-ref HEAD | sed -e 's,.*/\(.*\),\1,')
if [ "$branch" = "main" ] || [ "$branch" = "develop" ]; then
  echo "❌ Direct commits to $branch are not allowed."
  echo "   Create a feature branch instead."
  exit 1
fi

# Check for debugging code
if git diff --cached | grep -E "Console.WriteLine|Debugger.Break|TODO:|FIXME:" > /dev/null; then
  echo "⚠️  Warning: Debugging code detected in staged files:"
  git diff --cached --name-only | while read file; do
    if grep -n "Console.WriteLine\|Debugger.Break\|TODO:\|FIXME:" "$file" 2>/dev/null; then
      echo "   $file"
    fi
  done
  echo ""
  read -p "Continue anyway? (y/N) " choice
  case "$choice" in
    y|Y ) exit 0;;
    * ) exit 1;;
  esac
fi

# Check for large files
max_size=5242880  # 5MB
files=$(git diff --cached --name-only --diff-filter=ACM)
for file in $files; do
  if [ -f "$file" ]; then
    size=$(wc -c < "$file")
    if [ "$size" -gt "$max_size" ]; then
      echo "❌ File too large (>5MB): $file ($(numfmt --to=iec-i --suffix=B $size))"
      echo "   Consider using Git LFS for large files."
      exit 1
    fi
  fi
done

echo "✓ Pre-commit checks passed"
exit 0
```

Make executable:
```powershell
chmod +x .git/hooks/pre-commit
```

---

## 9. Branch Minimization Strategy

### 9.1 Current State
- **Active Branches:** 2 (main, claude/production-ready-deployment-*)
- **Stale Branches:** 0 (currently clean)
- **Tags:** 1 (v2.0.1)

### 9.2 Target State
- **Permanent Branches:** 2 (main, develop)
- **Temporary Branches:** Max 5 concurrent feature/bugfix branches
- **Auto-cleanup:** Merged branches deleted within 1 day
- **Manual cleanup:** Stale branches (30+ days, no PR) deleted weekly

### 9.3 Implementation

#### Step 1: Create `develop` branch
```bash
git checkout main
git pull origin main
git checkout -b develop
git push -u origin develop
```

#### Step 2: Set branch protection rules (GitHub UI)

**Main Branch:**
```yaml
Settings → Branches → Add rule → main
  ✓ Require pull request reviews before merging (1 reviewer)
  ✓ Require status checks to pass before merging
    - Build and Test
    - Code Quality
  ✓ Require branches to be up to date before merging
  ✓ Include administrators
  ✗ Allow force pushes
  ✗ Allow deletions
```

**Develop Branch:**
```yaml
Settings → Branches → Add rule → develop
  ✓ Require status checks to pass before merging
    - Build and Test
  ✓ Require branches to be up to date before merging
  ✗ Allow force pushes
  ✗ Allow deletions
```

#### Step 3: Enable auto-delete of head branches

```yaml
Settings → General → Pull Requests
  ✓ Automatically delete head branches
```

#### Step 4: Deploy branch cleanup workflow

Create `.github/workflows/branch-cleanup.yml` (see section 4.3)

#### Step 5: Set up branch lifecycle rules

**Immediate deletion after merge:**
- Handled automatically by GitHub if "auto-delete head branches" is enabled
- Backup: Weekly cleanup workflow deletes missed branches

**Stale branch cleanup criteria:**
```yaml
Age: > 30 days
Status: No open PR
Exceptions: main, develop
Action: Delete remote branch
Notification: Comment on last commit before deletion
```

---

## 10. Security & Best Practices

### 10.1 Secrets Management

**Never commit:**
- API keys
- Passwords
- Private keys
- Connection strings with credentials
- Code signing certificates

**Use GitHub Secrets:**
```yaml
Settings → Secrets and variables → Actions → New repository secret
```

**Required Secrets:**
- `GITHUB_TOKEN` (automatic, no setup needed)
- `CODE_SIGNING_CERT` (optional, for future code signing)
- `CODE_SIGNING_PASSWORD` (optional, for future code signing)

### 10.2 Signed Commits

Enable commit signing for verification:

```powershell
# Generate GPG key
gpg --full-generate-key
# Select: RSA and RSA, 4096 bits, no expiration

# List keys
gpg --list-secret-keys --keyid-format LONG

# Export public key
gpg --armor --export YOUR_KEY_ID

# Add to GitHub: Settings → SSH and GPG keys → New GPG key

# Configure Git
git config --global user.signingkey YOUR_KEY_ID
git config --global commit.gpgsign true

# Commits now automatically signed
git commit -m "feat: signed commit"
```

### 10.3 `.gitignore` Improvements

Add to `.gitignore`:

```gitignore
# Build artifacts that were accidentally allowed
build/version.txt
build/nuget.exe
checksums.txt
release-notes.md

# Local development
.env
.env.local
*.local.config

# Deployment artifacts
installer/*.exe
releases/

# OS-specific
.DS_Store
Thumbs.db
desktop.ini

# IDE
.vs/
.vscode/
.idea/
*.swp
*.swo
*~
```

---

## 11. Monitoring & Metrics

### 11.1 Repository Health Metrics

Track monthly:
- **Branch Count:** Target < 5 active branches
- **PR Merge Time:** Target < 2 days
- **Build Success Rate:** Target > 95%
- **Code Quality Score:** No regressions
- **Documentation Coverage:** All public APIs documented

### 11.2 GitHub Insights

Monitor via **Insights** tab:
- Pulse: Recent activity summary
- Contributors: Contribution statistics
- Traffic: Clone and visitor statistics
- Commits: Commit frequency
- Code frequency: Lines added/removed over time
- Network: Branch and fork visualization

### 11.3 Workflow Efficiency

Track in Actions tab:
- Average workflow duration
- Workflow success rate
- Failed workflow root causes
- Resource usage (minutes)

---

## 12. Migration Checklist

### Phase 1: Preparation (Week 1)
- [ ] Review this document with team
- [ ] Create `develop` branch
- [ ] Set up branch protection rules
- [ ] Configure auto-delete for merged branches
- [ ] Create PR template

### Phase 2: Workflow Consolidation (Week 2)
- [ ] Consolidate `release.yml` and `build-release.yml`
- [ ] Test consolidated workflow with manual dispatch
- [ ] Fix build configuration matrix
- [ ] Deploy branch cleanup workflow
- [ ] Add version bump workflow

### Phase 3: Documentation (Week 3)
- [ ] Update CONTRIBUTING.md with new workflow
- [ ] Add local development guide
- [ ] Create video walkthrough (optional)
- [ ] Update README.md with badges and workflow info

### Phase 4: Team Training (Week 4)
- [ ] Train team on new Git workflow
- [ ] Practice creating feature branches
- [ ] Practice PR process
- [ ] Test automated cleanup

### Phase 5: Monitoring (Ongoing)
- [ ] Monitor branch count weekly
- [ ] Review workflow efficiency monthly
- [ ] Update documentation as needed
- [ ] Iterate on improvements

---

## 13. Quick Reference

### Common Commands

```powershell
# Start new feature
git checkout develop
git pull origin develop
git checkout -b feature/my-feature

# Commit changes
git add .
git commit -m "feat: add my feature"
git push -u origin feature/my-feature

# Create PR
gh pr create --base develop --fill

# Update from develop
git checkout develop
git pull origin develop
git checkout feature/my-feature
git rebase develop

# Clean up after merge
git checkout develop
git pull origin develop
git branch -d feature/my-feature

# Create release
git checkout develop
git pull origin develop
git checkout -b release/v2.1.0
# Test and stabilize
git checkout main
git merge release/v2.1.0
git tag -a v2.1.0 -m "Release v2.1.0"
git push origin main --tags
```

### Workflow Triggers

```yaml
Push to main:     build.yml, code-quality.yml
Push to develop:  build.yml, code-quality.yml
Push to claude/*: build.yml, code-quality.yml
PR to main:       code-quality.yml
PR to develop:    code-quality.yml
Tag v*.*.*:       release.yml (consolidated)
PR merged:        branch-cleanup.yml
Weekly Sunday:    branch-cleanup.yml
```

---

## 14. Conclusion

This comprehensive Git workflow strategy provides:

✅ **Minimal Branch Count** - Auto-cleanup keeps repository clean
✅ **Continuous Integration** - Automated build, test, and deployment
✅ **Quality Assurance** - Code quality and security checks
✅ **Local Development** - Clear guidance for all contributors
✅ **Scalability** - Workflow supports team growth
✅ **Automation** - Reduces manual effort and errors

**Next Steps:**
1. Review and approve this strategy
2. Create `develop` branch
3. Implement branch protection
4. Deploy consolidated workflows
5. Train team members

**Questions or Feedback:**
Open an issue or discussion on GitHub repository.

---

**Document Version:** 1.0
**Last Updated:** 2025-11-17
**Author:** Claude (AI Assistant)
**Status:** Ready for Implementation
