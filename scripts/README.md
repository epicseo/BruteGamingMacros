# Deployment Scripts

**WSL/Linux Bash Scripts for BruteGamingMacros**

---

## Quick Reference

| Script | Purpose | Example |
|--------|---------|---------|
| `push.sh` | Quick commit and push | `./scripts/push.sh "feat: add feature"` |
| `deploy.sh` | Create branch and deploy | `./scripts/deploy.sh feature my-feature "Description"` |
| `release.sh` | Create version tag and release | `./scripts/release.sh 2.0.2` |
| `create-pr.sh` | Create pull request | `./scripts/create-pr.sh "PR title"` |
| `sync.sh` | Sync with upstream branch | `./scripts/sync.sh` |
| `cleanup.sh` | Delete merged branches | `./scripts/cleanup.sh --list` |

---

## Installation

### Make Scripts Executable

```bash
cd ~/BruteGamingMacros
chmod +x scripts/*.sh
```

### Add to PATH (Optional)

```bash
# Add to ~/.bashrc
echo 'export PATH="$HOME/BruteGamingMacros/scripts:$PATH"' >> ~/.bashrc
source ~/.bashrc

# Now you can run from anywhere
push.sh "commit message"
```

---

## Script Details

### 1. push.sh - Quick Push

**Purpose:** Commit all changes and push to current branch

**Usage:**
```bash
# Basic usage
./scripts/push.sh "commit message"

# Auto-commit with timestamp
./scripts/push.sh

# Force push (use carefully!)
./scripts/push.sh --force "message"
```

**What it does:**
1. Checks for uncommitted changes
2. Stages all changes (`git add .`)
3. Commits with your message
4. Pushes to origin

---

### 2. deploy.sh - Deploy Branch

**Purpose:** Create feature/bugfix branch and deploy

**Usage:**
```bash
# Feature branch
./scripts/deploy.sh feature auto-targeting "Add auto-targeting system"

# Bugfix branch
./scripts/deploy.sh bugfix issue-123 "Fix memory leak"

# Hotfix branch (from main)
./scripts/deploy.sh hotfix critical "Fix critical bug"

# Other types
./scripts/deploy.sh refactor cleanup "Refactor code"
./scripts/deploy.sh docs api "Update API docs"
```

**What it does:**
1. Checks for uncommitted changes
2. Updates base branch (develop or main for hotfixes)
3. Creates new branch
4. Pushes to remote
5. Creates initial commit if message provided

---

### 3. release.sh - Create Release

**Purpose:** Create version tag and trigger release workflow

**Usage:**
```bash
# Production release
./scripts/release.sh 2.0.2

# Prerelease
./scripts/release.sh 2.0.3-beta
./scripts/release.sh 2.1.0-rc1

# With custom message
./scripts/release.sh 2.0.2 "Major update with new features"
```

**What it does:**
1. Validates version format
2. Checks if tag exists
3. Ensures on main branch
4. Updates version file
5. Extracts changelog
6. Creates annotated tag
7. Pushes to remote
8. Triggers GitHub Actions release workflow

**Version Format:**
- `MAJOR.MINOR.PATCH` - Production release (e.g., 2.0.1)
- `MAJOR.MINOR.PATCH-PRERELEASE` - Prerelease (e.g., 2.1.0-beta)

---

### 4. create-pr.sh - Create Pull Request

**Purpose:** Create pull request on GitHub

**Requirements:** GitHub CLI (`gh`) must be installed and authenticated

**Usage:**
```bash
# Basic usage (PR to develop)
./scripts/create-pr.sh

# With title
./scripts/create-pr.sh "Add auto-targeting feature"

# PR to main (for hotfixes)
./scripts/create-pr.sh --base main "Hotfix: critical bug"
```

**What it does:**
1. Checks if branch is pushed
2. Checks if PR already exists
3. Generates title from branch name if not provided
4. Extracts commit messages
5. Creates PR with template
6. Opens in browser (optional)

---

### 5. sync.sh - Sync Branch

**Purpose:** Sync current branch with upstream

**Usage:**
```bash
# Sync with develop
./scripts/sync.sh

# Sync with main
./scripts/sync.sh main

# Force reset to upstream (DESTRUCTIVE!)
./scripts/sync.sh --force
```

**What it does:**
1. Stashes uncommitted changes (optional)
2. Fetches from remote
3. Updates upstream branch
4. Rebases current branch
5. Restores stashed changes

---

### 6. cleanup.sh - Branch Cleanup

**Purpose:** Delete merged branches

**Usage:**
```bash
# List merged branches
./scripts/cleanup.sh --list

# Delete all merged branches
./scripts/cleanup.sh

# Delete specific branch
./scripts/cleanup.sh feature/old-feature
```

**What it does:**
1. Fetches latest from remote
2. Finds branches merged into develop
3. Deletes local merged branches
4. Optionally deletes remote branches

**Protected branches:** main, develop (never deleted)

---

## Workflows

### Daily Development

```bash
# 1. Start new feature
./scripts/deploy.sh feature my-feature "Add new feature"

# 2. Make changes
# ... edit files ...

# 3. Push changes
./scripts/push.sh "feat: implement core logic"

# 4. More changes
# ... edit more ...

# 5. Push again
./scripts/push.sh "feat: add UI components"

# 6. Create PR
./scripts/create-pr.sh "Add my feature"

# 7. After merge - cleanup
./scripts/cleanup.sh feature/my-feature
```

### Hotfix Workflow

```bash
# 1. Create hotfix from main
git checkout main
git pull origin main
./scripts/deploy.sh hotfix critical-bug "Fix critical bug"

# 2. Make fix
# ... fix the bug ...

# 3. Push
./scripts/push.sh "hotfix: resolve critical issue"

# 4. PR to main
./scripts/create-pr.sh --base main "Hotfix: critical bug"

# 5. After merge - sync develop
git checkout develop
git pull origin develop
git merge main
git push origin develop
```

### Release Workflow

```bash
# 1. Ensure on latest main
git checkout main
git pull origin main

# 2. Create release
./scripts/release.sh 2.0.2

# 3. GitHub Actions automatically:
#    - Builds all configurations
#    - Creates NSIS installer
#    - Packages portable ZIPs
#    - Generates checksums
#    - Creates GitHub Release

# 4. Verify release
gh release view v2.0.2 --web
```

---

## Troubleshooting

### Script Not Found

```bash
# Make executable
chmod +x scripts/*.sh

# Or run with bash
bash scripts/push.sh "message"
```

### Permission Denied

```bash
# Check file permissions
ls -la scripts/

# Fix permissions
chmod +x scripts/*.sh
```

### Line Ending Issues

```bash
# Install dos2unix
sudo apt install dos2unix -y

# Convert line endings
dos2unix scripts/*.sh
```

### GitHub CLI Not Found

```bash
# Install gh (see docs/WSL_DEVELOPMENT_GUIDE.md)
# Ubuntu/WSL:
curl -fsSL https://cli.github.com/packages/githubcli-archive-keyring.gpg | \
  sudo dd of=/usr/share/keyrings/githubcli-archive-keyring.gpg
# ... follow installation steps

# Authenticate
gh auth login
```

---

## Advanced Usage

### Custom Aliases

Add to `~/.bashrc`:

```bash
# Quick shortcuts
alias bgm-push='~/BruteGamingMacros/scripts/push.sh'
alias bgm-deploy='~/BruteGamingMacros/scripts/deploy.sh'
alias bgm-release='~/BruteGamingMacros/scripts/release.sh'
alias bgm-pr='~/BruteGamingMacros/scripts/create-pr.sh'
alias bgm-sync='~/BruteGamingMacros/scripts/sync.sh'
alias bgm-clean='~/BruteGamingMacros/scripts/cleanup.sh'

# Reload
source ~/.bashrc
```

Usage:
```bash
bgm-push "feat: add feature"
bgm-deploy feature my-feature "Add feature"
bgm-release 2.0.2
```

### Automated Workflows

Chain scripts together:

```bash
# Deploy, commit, and create PR in one go
./scripts/deploy.sh feature my-feature "Add feature" && \
./scripts/push.sh "feat: initial implementation" && \
./scripts/create-pr.sh "Add my feature"
```

Create custom workflow script:

```bash
#!/bin/bash
# custom-workflow.sh
./scripts/deploy.sh "$@"
# ... make changes ...
./scripts/push.sh "wip: ${3}"
./scripts/create-pr.sh "${3}"
```

---

## Best Practices

### DO ✅

- ✅ Use descriptive commit messages
- ✅ Run scripts from repository root
- ✅ Review changes before pushing
- ✅ Test locally before creating PR
- ✅ Keep scripts updated
- ✅ Use conventional commit format

### DON'T ❌

- ❌ Force push without good reason
- ❌ Skip testing before release
- ❌ Delete branches before merge
- ❌ Modify scripts without testing
- ❌ Commit without reviewing changes

---

## Documentation

**Full Guide:** `docs/WSL_DEVELOPMENT_GUIDE.md`
**Git Workflow:** `docs/GIT_WORKFLOW_STRATEGY.md`
**Local Development:** `docs/LOCAL_DEVELOPMENT_GUIDE.md`

---

**Version:** 1.0
**Last Updated:** 2025-11-17
