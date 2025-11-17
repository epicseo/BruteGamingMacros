# WSL Development & Deployment Guide

**Project:** BruteGamingMacros
**Version:** 1.0
**Last Updated:** 2025-11-17

---

## Overview

Yes! You can **completely control** this repository from WSL (Windows Subsystem for Linux) with automated scripts. This guide shows you how to set up WSL, use the included deployment scripts, and manage the entire development workflow from Linux.

---

## Why Use WSL?

### Advantages

✅ **Native Linux tools** - Git, bash, ssh work perfectly
✅ **Faster Git operations** - Often 2-3x faster than Windows Git
✅ **Better scripting** - Bash/shell scripts more powerful than batch/PowerShell
✅ **Direct filesystem access** - Can access Windows files from `/mnt/c/`
✅ **SSH key management** - Linux-style SSH agent works great
✅ **Automation friendly** - Easy to write deployment scripts
✅ **GitHub CLI native** - Better integration in Linux environment
✅ **Docker support** - If you need containers later

### What Works

✅ Git push/pull/commit
✅ GitHub CLI operations
✅ SSH authentication
✅ Deployment scripts
✅ File editing (VS Code Remote-WSL)
✅ Building C# projects (via MSBuild on Windows)
✅ Running workflows
✅ Branch management
✅ Release tagging

---

## Initial WSL Setup

### 1. Install WSL 2 (if not already installed)

**PowerShell (as Administrator):**
```powershell
# Install WSL 2
wsl --install

# Or if already installed, update to WSL 2
wsl --set-default-version 2

# Install Ubuntu (recommended)
wsl --install -d Ubuntu-22.04

# Verify installation
wsl --list --verbose
```

**Expected output:**
```
  NAME            STATE           VERSION
* Ubuntu-22.04    Running         2
```

### 2. Launch WSL

```powershell
# From Windows Terminal or PowerShell
wsl

# Or from Windows search, run "Ubuntu"
```

### 3. Update Ubuntu

```bash
# Update package lists
sudo apt update && sudo apt upgrade -y

# Install essential tools
sudo apt install -y git curl wget build-essential
```

---

## Git Setup in WSL

### 1. Configure Git Identity

```bash
# Set your name and email
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"

# Set default branch name
git config --global init.defaultBranch main

# Set default editor
git config --global core.editor "vim"

# Enable colored output
git config --global color.ui auto

# Set pull strategy
git config --global pull.rebase false
```

### 2. Configure Line Endings (IMPORTANT for Windows/WSL)

```bash
# For cross-platform compatibility
git config --global core.autocrlf input
git config --global core.eol lf

# Ignore file mode changes (WSL/Windows difference)
git config --global core.fileMode false
```

---

## Authentication Methods

### Method 1: SSH Keys (Recommended)

**Advantages:** No password prompts, most secure, works with all Git operations

```bash
# 1. Generate SSH key
ssh-keygen -t ed25519 -C "your.email@example.com"
# Press Enter for default location: /home/username/.ssh/id_ed25519
# Set a passphrase (recommended) or leave empty

# 2. Start SSH agent
eval "$(ssh-agent -s)"

# 3. Add key to agent
ssh-add ~/.ssh/id_ed25519

# 4. Copy public key to clipboard
cat ~/.ssh/id_ed25519.pub
# Copy the output

# 5. Add to GitHub
# Go to: https://github.com/settings/keys
# Click "New SSH key"
# Paste your public key
# Title: "WSL Ubuntu"

# 6. Test connection
ssh -T git@github.com
# Should see: "Hi username! You've successfully authenticated..."

# 7. Clone repository with SSH
git clone git@github.com:epicseo/BruteGamingMacros.git
cd BruteGamingMacros
```

**Auto-start SSH agent on login:**
```bash
# Add to ~/.bashrc
echo '
# Auto-start SSH agent
if [ -z "$SSH_AUTH_SOCK" ]; then
   eval "$(ssh-agent -s)" > /dev/null
   ssh-add ~/.ssh/id_ed25519 2>/dev/null
fi
' >> ~/.bashrc
```

### Method 2: GitHub CLI (Easiest)

**Advantages:** Automatic authentication, works with gh commands

```bash
# 1. Install GitHub CLI
curl -fsSL https://cli.github.com/packages/githubcli-archive-keyring.gpg | sudo dd of=/usr/share/keyrings/githubcli-archive-keyring.gpg
sudo chmod go+r /usr/share/keyrings/githubcli-archive-keyring.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/githubcli-archive-keyring.gpg] https://cli.github.com/packages stable main" | sudo tee /etc/apt/sources.list.d/github-cli.list > /dev/null
sudo apt update
sudo apt install gh -y

# 2. Authenticate
gh auth login
# Choose: GitHub.com
# Choose: HTTPS or SSH
# Choose: Login with browser
# Follow browser prompts

# 3. Configure Git to use GitHub CLI
gh auth setup-git

# 4. Clone repository
gh repo clone epicseo/BruteGamingMacros
cd BruteGamingMacros

# 5. Verify
gh auth status
```

### Method 3: Personal Access Token (PAT)

**Advantages:** Simple, works from anywhere

```bash
# 1. Generate PAT on GitHub
# Go to: https://github.com/settings/tokens
# Click "Generate new token (classic)"
# Scopes: repo, workflow
# Copy the token (save it securely!)

# 2. Clone with HTTPS
git clone https://github.com/epicseo/BruteGamingMacros.git
cd BruteGamingMacros

# 3. Configure credential helper
git config --global credential.helper store

# 4. On first push, enter credentials
git push
# Username: your-github-username
# Password: ghp_xxxxxxxxxxxxxxxxxxxx (your PAT)
# Credentials are saved for future use
```

---

## Repository Location Options

### Option 1: Clone in WSL Home (Recommended)

**Location:** `/home/username/BruteGamingMacros`

**Advantages:**
- ✅ Fastest Git operations
- ✅ Native Linux permissions
- ✅ Better performance

```bash
# Clone to WSL home
cd ~
git clone git@github.com:epicseo/BruteGamingMacros.git
cd BruteGamingMacros
```

**Access from Windows:**
- File Explorer: `\\wsl$\Ubuntu-22.04\home\username\BruteGamingMacros`
- VS Code: `code .` from WSL terminal (installs Remote-WSL extension)

### Option 2: Use Existing Windows Repository

**Location:** `/mnt/c/Users/YourName/source/repos/BruteGamingMacros`

**Advantages:**
- ✅ Shared with Windows tools (Visual Studio)
- ✅ No duplication

```bash
# Navigate to Windows directory
cd /mnt/c/Users/YourName/source/repos/BruteGamingMacros

# Verify Git works
git status

# Important: Set file mode to false (Windows/WSL compatibility)
git config core.fileMode false
```

**Note:** Git operations may be slightly slower on `/mnt/c/` but still work perfectly.

---

## Using the Deployment Scripts

All scripts are located in: `scripts/`

### 1. Quick Push Script

**File:** `scripts/push.sh`

```bash
# Quick push current branch
./scripts/push.sh

# Push with commit message
./scripts/push.sh "feat: add new feature"

# Force push (use carefully!)
./scripts/push.sh --force
```

### 2. Deploy Script

**File:** `scripts/deploy.sh`

```bash
# Create feature branch, commit, and push
./scripts/deploy.sh feature my-new-feature "Add auto-targeting system"

# Create bugfix branch
./scripts/deploy.sh bugfix issue-123 "Fix crash in ThreadRunner"

# Create hotfix branch
./scripts/deploy.sh hotfix critical-bug "Fix memory leak"
```

### 3. Release Script

**File:** `scripts/release.sh`

```bash
# Create and push release tag
./scripts/release.sh 2.0.2

# Create prerelease tag
./scripts/release.sh 2.0.3-beta

# Create release with custom message
./scripts/release.sh 2.0.2 "Major update with new features"
```

### 4. PR Creation Script

**File:** `scripts/create-pr.sh`

```bash
# Create PR from current branch to develop
./scripts/create-pr.sh

# Create PR with custom title
./scripts/create-pr.sh "Add auto-targeting feature"

# Create PR to main (instead of develop)
./scripts/create-pr.sh --base main "Hotfix: critical bug"
```

### 5. Sync Script

**File:** `scripts/sync.sh`

```bash
# Sync current branch with develop
./scripts/sync.sh

# Sync with main
./scripts/sync.sh main

# Force sync (reset to upstream)
./scripts/sync.sh --force
```

### 6. Cleanup Script

**File:** `scripts/cleanup.sh`

```bash
# List merged branches
./scripts/cleanup.sh --list

# Delete merged branches
./scripts/cleanup.sh

# Delete specific branch
./scripts/cleanup.sh feature/old-feature
```

---

## Complete Workflow Examples

### Example 1: Create Feature, Push, and Open PR

```bash
# 1. Start new feature
./scripts/deploy.sh feature auto-targeting "Implement auto-targeting system"

# 2. Make your changes (edit files in VS Code)
code .

# 3. Quick commit and push
./scripts/push.sh "feat: add target selection logic"

# 4. Make more changes
# ... edit more files ...

# 5. Push again
./scripts/push.sh "feat: add configuration UI"

# 6. Create pull request
./scripts/create-pr.sh "Add auto-targeting system"

# 7. After PR is merged, cleanup
./scripts/cleanup.sh feature/auto-targeting
```

### Example 2: Create Production Release

```bash
# 1. Ensure you're on latest main
git checkout main
git pull origin main

# 2. Create release tag (triggers GitHub Actions)
./scripts/release.sh 2.0.2 "Production release with bug fixes and improvements"

# 3. Verify release created
gh release list

# 4. View release URL
gh release view v2.0.2 --web
```

### Example 3: Quick Hotfix

```bash
# 1. Create hotfix branch from main
git checkout main
git pull origin main
./scripts/deploy.sh hotfix memory-leak "Fix critical memory leak"

# 2. Make the fix
# ... edit files ...

# 3. Push fix
./scripts/push.sh "hotfix: resolve memory leak in macro execution"

# 4. Create PR to main (hotfixes go directly to main)
./scripts/create-pr.sh --base main "Hotfix: memory leak in macro execution"

# 5. After merge, also merge to develop
git checkout develop
git pull origin develop
git merge main
git push origin develop

# 6. Cleanup
./scripts/cleanup.sh hotfix/memory-leak
```

### Example 4: Daily Development Workflow

```bash
# Morning: Start work
cd ~/BruteGamingMacros
git checkout develop
git pull origin develop
./scripts/deploy.sh feature improved-logging "Improve production logging"

# During day: Regular commits
# ... make changes ...
./scripts/push.sh "feat: add structured logging"

# ... more changes ...
./scripts/push.sh "feat: add log rotation"

# ... more changes ...
./scripts/push.sh "docs: update logging documentation"

# End of day: Create PR
./scripts/create-pr.sh "Improve production logging system"

# Next day: After PR review, make requested changes
# ... edit files ...
./scripts/push.sh "refactor: address PR feedback"

# After merge: Cleanup
git checkout develop
git pull origin develop
./scripts/cleanup.sh feature/improved-logging
```

---

## Building from WSL

While you can't run MSBuild directly in WSL (it's a Windows tool), you can still trigger Windows builds:

### Option 1: Call Windows MSBuild from WSL

```bash
# Create build script
cat > scripts/build.sh << 'EOF'
#!/bin/bash
# Build via Windows MSBuild from WSL

CONFIG=${1:-Release}
PROJECT_DIR=$(pwd)

# Convert WSL path to Windows path
WIN_PATH=$(wslpath -w "$PROJECT_DIR")

echo "Building BruteGamingMacros ($CONFIG)..."
echo "Project: $WIN_PATH"

# Call Windows MSBuild
/mnt/c/Program\ Files/Microsoft\ Visual\ Studio/2022/Community/MSBuild/Current/Bin/MSBuild.exe \
  "$WIN_PATH\\BruteGamingMacros.sln" \
  //p:Configuration=$CONFIG \
  //p:Platform="Any CPU" \
  //t:Rebuild \
  //v:minimal \
  //nologo

if [ $? -eq 0 ]; then
  echo "✓ Build successful!"
  ls -lh "bin/$CONFIG/BruteGamingMacros.exe"
else
  echo "✗ Build failed!"
  exit 1
fi
EOF

chmod +x scripts/build.sh

# Usage
./scripts/build.sh Release
```

### Option 2: Use GitHub Actions for Builds

```bash
# Trigger workflow manually
gh workflow run build-ci.yml

# Watch workflow run
gh run watch

# Download artifacts when complete
gh run download <run-id>
```

### Option 3: Use Docker (Advanced)

```bash
# Create Dockerfile with Windows SDK
# Then build in container
docker build -t bgm-builder .
docker run --rm -v $(pwd):/src bgm-builder
```

---

## VS Code Integration

### Setup VS Code Remote-WSL

```bash
# 1. Install VS Code on Windows if not already installed
# Download from: https://code.visualstudio.com/

# 2. From WSL terminal, open project
cd ~/BruteGamingMacros
code .

# 3. VS Code installs Remote-WSL extension automatically
# 4. Now you can edit files with full Linux environment
```

**Advantages:**
- ✅ IntelliSense works with C# extension
- ✅ Git integration built-in
- ✅ Terminal is WSL bash
- ✅ Extensions run in WSL context
- ✅ Fast file operations

**Recommended Extensions:**
- C# (ms-dotnettools.csharp)
- GitLens
- GitHub Pull Requests
- Remote - WSL (ms-vscode-remote.remote-wsl)

---

## Advanced Git Aliases in WSL

Add to `~/.gitconfig` or `~/.bashrc`:

```bash
# ~/.bashrc additions
cat >> ~/.bashrc << 'EOF'

# Git aliases
alias gs='git status -sb'
alias ga='git add'
alias gc='git commit -m'
alias gp='git push'
alias gpl='git pull'
alias gco='git checkout'
alias gb='git branch'
alias glog='git log --graph --oneline --decorate -20'
alias gclean='git branch --merged | grep -v "\*\|main\|develop" | xargs -n 1 git branch -d'

# Quick commit and push
function gcp() {
  git add .
  git commit -m "$1"
  git push
}

# Create and checkout branch
function gcb() {
  git checkout -b "$1"
}

# Show current branch
function current_branch() {
  git branch --show-current
}

# Push current branch
alias gpush='git push -u origin $(current_branch)'

# Project shortcuts
alias bgm='cd ~/BruteGamingMacros'
alias bgm-logs='cd ~/BruteGamingMacros && git log --oneline -10'
alias bgm-status='cd ~/BruteGamingMacros && git status'

EOF

# Reload bashrc
source ~/.bashrc
```

**Usage:**
```bash
gs                          # Git status
ga .                        # Git add all
gc "feat: add feature"      # Git commit
gp                          # Git push
gcb feature/new-feature     # Create and checkout branch
gcp "quick commit"          # Add, commit, and push in one command
bgm                         # Jump to project directory
```

---

## Automated Deployment Pipeline

### Full Automation Example

Create `scripts/auto-deploy.sh`:

```bash
#!/bin/bash
# Automated deployment pipeline
# Usage: ./scripts/auto-deploy.sh feature my-feature "Add new feature"

set -e

TYPE=${1:-feature}
NAME=${2:-temp}
MESSAGE=${3:-"Work in progress"}

echo "================================"
echo "Automated Deployment Pipeline"
echo "================================"
echo ""

# Step 1: Ensure clean state
echo "[1/7] Checking repository state..."
if [[ -n $(git status --porcelain) ]]; then
  echo "⚠ Uncommitted changes detected"
  read -p "Commit all changes? (y/n) " -n 1 -r
  echo
  if [[ $REPLY =~ ^[Yy]$ ]]; then
    git add .
    git commit -m "wip: $MESSAGE"
  else
    echo "✗ Aborted - commit changes first"
    exit 1
  fi
fi
echo "✓ Repository clean"

# Step 2: Update base branch
echo "[2/7] Updating develop branch..."
git checkout develop
git pull origin develop
echo "✓ Develop updated"

# Step 3: Create feature branch
BRANCH_NAME="${TYPE}/${NAME}"
echo "[3/7] Creating branch: $BRANCH_NAME..."
git checkout -b "$BRANCH_NAME"
echo "✓ Branch created"

# Step 4: Push to remote
echo "[4/7] Pushing to remote..."
git push -u origin "$BRANCH_NAME"
echo "✓ Pushed to remote"

# Step 5: Trigger CI build
echo "[5/7] Triggering CI build..."
sleep 2  # Wait for GitHub to register push
echo "✓ CI build triggered (check GitHub Actions)"

# Step 6: Create pull request
echo "[6/7] Creating pull request..."
gh pr create --base develop --head "$BRANCH_NAME" \
  --title "$MESSAGE" \
  --body "## Description
$MESSAGE

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Builds successfully
- [ ] Tested manually
- [ ] No compiler warnings

## Checklist
- [ ] Code follows project style
- [ ] CHANGELOG.md updated
- [ ] Documentation updated
"

echo "✓ Pull request created"

# Step 7: Summary
echo "[7/7] Deployment complete!"
echo ""
echo "================================"
echo "Summary"
echo "================================"
echo "Branch: $BRANCH_NAME"
echo "Remote: origin/$BRANCH_NAME"
PR_URL=$(gh pr view --json url -q .url)
echo "PR URL: $PR_URL"
echo ""
echo "Next steps:"
echo "1. Make your changes"
echo "2. Run: ./scripts/push.sh \"commit message\""
echo "3. Request review on PR"
echo "4. Merge when approved"
echo ""
```

Make executable:
```bash
chmod +x scripts/auto-deploy.sh
```

Usage:
```bash
./scripts/auto-deploy.sh feature auto-targeting "Add auto-targeting system"
```

---

## Monitoring & Notifications

### Watch GitHub Actions from WSL

```bash
# Install jq for JSON parsing
sudo apt install jq -y

# Create workflow watcher script
cat > scripts/watch-ci.sh << 'EOF'
#!/bin/bash
# Watch GitHub Actions workflow runs

echo "Watching GitHub Actions workflows..."
echo ""

while true; do
  clear
  echo "================================"
  echo "GitHub Actions Status"
  echo "================================"
  echo ""

  gh run list --limit 5 --json status,conclusion,name,createdAt,url \
    --jq '.[] | "\(.name) - \(.status) - \(.conclusion // "In Progress") - \(.createdAt) - \(.url)"' \
    | column -t -s '-'

  echo ""
  echo "Refreshing in 10 seconds... (Ctrl+C to stop)"
  sleep 10
done
EOF

chmod +x scripts/watch-ci.sh

# Usage
./scripts/watch-ci.sh
```

### Desktop Notifications (Optional)

```bash
# Install notify-send (if not already installed)
sudo apt install libnotify-bin -y

# Create notification script
cat > scripts/notify-build.sh << 'EOF'
#!/bin/bash
# Send desktop notification when build completes

WORKFLOW_NAME=${1:-"Build"}
STATUS=$(gh run list --workflow="$WORKFLOW_NAME" --limit 1 --json conclusion -q '.[0].conclusion')

if [ "$STATUS" == "success" ]; then
  notify-send "✓ Build Success" "$WORKFLOW_NAME completed successfully"
  echo "✓ Build passed!"
  exit 0
elif [ "$STATUS" == "failure" ]; then
  notify-send "✗ Build Failed" "$WORKFLOW_NAME failed - check logs"
  echo "✗ Build failed!"
  exit 1
else
  echo "Build status: $STATUS"
fi
EOF

chmod +x scripts/notify-build.sh
```

---

## Troubleshooting

### Permission Errors

```bash
# If you get permission denied errors
chmod +x scripts/*.sh

# If Git shows too many modified files (file mode changes)
git config core.fileMode false
git diff --stat  # Should show no changes now
```

### Line Ending Issues

```bash
# Convert CRLF to LF for shell scripts
find scripts -name "*.sh" -exec dos2unix {} \;

# Or install dos2unix
sudo apt install dos2unix -y
dos2unix scripts/*.sh
```

### Slow Git Operations on /mnt/c/

```bash
# Move repository to WSL home for better performance
mv /mnt/c/Users/YourName/source/repos/BruteGamingMacros ~/
cd ~/BruteGamingMacros

# Access from Windows: \\wsl$\Ubuntu-22.04\home\username\BruteGamingMacros
```

### SSH Agent Not Persisting

```bash
# Add to ~/.bashrc for persistent SSH agent
echo '
# Persistent SSH agent
SSH_ENV="$HOME/.ssh/agent-environment"

function start_agent {
    echo "Initializing new SSH agent..."
    /usr/bin/ssh-agent | sed '\''s/^echo/#echo/'\'' > "${SSH_ENV}"
    echo succeeded
    chmod 600 "${SSH_ENV}"
    . "${SSH_ENV}" > /dev/null
    /usr/bin/ssh-add ~/.ssh/id_ed25519 2>/dev/null
}

if [ -f "${SSH_ENV}" ]; then
    . "${SSH_ENV}" > /dev/null
    ps -ef | grep ${SSH_AGENT_PID} | grep ssh-agent$ > /dev/null || {
        start_agent;
    }
else
    start_agent;
fi
' >> ~/.bashrc

source ~/.bashrc
```

---

## Best Practices

### DO ✅

- ✅ Use SSH keys for authentication (most reliable)
- ✅ Clone repository in WSL home for best performance
- ✅ Use provided scripts for common tasks
- ✅ Configure `core.fileMode false` for Windows compatibility
- ✅ Use VS Code Remote-WSL for editing
- ✅ Commit from WSL, build from Windows (or GitHub Actions)
- ✅ Keep scripts in `scripts/` directory
- ✅ Use `#!/bin/bash` shebang in all scripts
- ✅ Test scripts before pushing

### DON'T ❌

- ❌ Edit the same files in both Windows and WSL simultaneously
- ❌ Mix Windows Git and WSL Git on same repo
- ❌ Use Windows paths in WSL scripts (`C:\` won't work)
- ❌ Forget to set execute permissions on scripts
- ❌ Use CRLF line endings in shell scripts
- ❌ Commit scripts without testing them first
- ❌ Hardcode absolute paths in scripts

---

## Quick Reference

### Common Commands

```bash
# Navigate to project
cd ~/BruteGamingMacros

# Check status
git status

# Create feature branch
./scripts/deploy.sh feature my-feature "Description"

# Quick push
./scripts/push.sh "commit message"

# Create PR
./scripts/create-pr.sh

# Create release
./scripts/release.sh 2.0.2

# Sync with develop
./scripts/sync.sh

# Cleanup merged branches
./scripts/cleanup.sh

# Build (via Windows MSBuild)
./scripts/build.sh Release

# Watch CI
./scripts/watch-ci.sh
```

### File Paths

```
WSL Home: ~/BruteGamingMacros
Windows Access: \\wsl$\Ubuntu-22.04\home\username\BruteGamingMacros
Windows C Drive from WSL: /mnt/c/
Scripts Directory: ~/BruteGamingMacros/scripts/
SSH Keys: ~/.ssh/
Git Config: ~/.gitconfig
Bash Config: ~/.bashrc
```

---

## Summary

✅ **Yes, you can completely control this repository from WSL!**

**What you can do:**
- ✅ Clone, push, pull, commit - all Git operations
- ✅ Create branches, tags, releases
- ✅ Open pull requests
- ✅ Run deployment scripts
- ✅ Automate workflows
- ✅ Monitor CI/CD
- ✅ Edit files with VS Code Remote-WSL
- ✅ Trigger Windows builds from WSL

**Recommended setup:**
1. Use WSL 2 with Ubuntu 22.04
2. Authenticate with SSH keys
3. Clone repository in WSL home (`~/`)
4. Use provided scripts in `scripts/` directory
5. Edit with VS Code Remote-WSL
6. Build via GitHub Actions or Windows MSBuild

**Next:** See `scripts/` directory for all automation tools!

---

**Document Version:** 1.0
**Maintained By:** BruteGamingMacros Development Team
