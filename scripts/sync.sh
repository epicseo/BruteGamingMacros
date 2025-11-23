#!/bin/bash
# Sync current branch with upstream
# Usage:
#   ./scripts/sync.sh          (sync with develop)
#   ./scripts/sync.sh main     (sync with main)
#   ./scripts/sync.sh --force  (force reset to upstream)

set -e

UPSTREAM_BRANCH=${1:-develop}
FORCE=false

# Parse arguments
if [ "$1" == "--force" ] || [ "$2" == "--force" ]; then
  FORCE=true
  if [ "$1" == "--force" ]; then
    UPSTREAM_BRANCH=${2:-develop}
  fi
fi

CURRENT_BRANCH=$(git branch --show-current)

if [ -z "$CURRENT_BRANCH" ]; then
  echo "✗ Not on any branch"
  exit 1
fi

echo "================================"
echo "Sync: $CURRENT_BRANCH with $UPSTREAM_BRANCH"
echo "================================"
echo ""

# Check for uncommitted changes
if [[ -n $(git status --porcelain) ]] && [ "$FORCE" = false ]; then
  echo "⚠ You have uncommitted changes:"
  git status --short
  echo ""
  read -p "Stash changes before syncing? (y/n) " -n 1 -r
  echo
  if [[ $REPLY =~ ^[Yy]$ ]]; then
    git stash push -m "Auto-stash before sync at $(date)"
    echo "✓ Changes stashed"
    STASHED=true
  else
    echo "Please commit or stash changes before syncing"
    exit 1
  fi
else
  STASHED=false
fi

# Fetch latest
echo "[1/4] Fetching from remote..."
git fetch origin
echo "✓ Fetched"
echo ""

# Update upstream branch
echo "[2/4] Updating $UPSTREAM_BRANCH..."
git checkout "$UPSTREAM_BRANCH"
git pull origin "$UPSTREAM_BRANCH"
echo "✓ $UPSTREAM_BRANCH updated"
echo ""

# Return to original branch
echo "[3/4] Returning to $CURRENT_BRANCH..."
git checkout "$CURRENT_BRANCH"

# Sync
if [ "$FORCE" = true ]; then
  echo "[4/4] Force resetting to origin/$UPSTREAM_BRANCH..."
  echo "⚠ WARNING: This will discard all local changes!"
  read -p "Are you absolutely sure? (y/n) " -n 1 -r
  echo
  if [[ $REPLY =~ ^[Yy]$ ]]; then
    git reset --hard "origin/$UPSTREAM_BRANCH"
    echo "✓ Force reset complete"
  else
    echo "✗ Aborted"
    exit 1
  fi
else
  echo "[4/4] Rebasing onto $UPSTREAM_BRANCH..."
  if git rebase "$UPSTREAM_BRANCH"; then
    echo "✓ Rebase successful"
  else
    echo "✗ Rebase failed with conflicts"
    echo ""
    echo "To resolve:"
    echo "1. Fix conflicts in the listed files"
    echo "2. git add <resolved-files>"
    echo "3. git rebase --continue"
    echo ""
    echo "Or abort:"
    echo "  git rebase --abort"
    exit 1
  fi
fi
echo ""

# Restore stashed changes if any
if [ "$STASHED" = true ] && [ "$FORCE" = false ]; then
  echo "Restoring stashed changes..."
  if git stash pop; then
    echo "✓ Stashed changes restored"
  else
    echo "⚠ Stash conflicts - resolve manually"
    echo "Run: git stash list"
  fi
  echo ""
fi

echo "================================"
echo "✓ SYNC COMPLETE"
echo "================================"
echo "Branch:   $CURRENT_BRANCH"
echo "Synced with: $UPSTREAM_BRANCH"
echo ""

# Check if ahead/behind
AHEAD=$(git rev-list --count "origin/$CURRENT_BRANCH..$CURRENT_BRANCH" 2>/dev/null || echo "0")
BEHIND=$(git rev-list --count "$CURRENT_BRANCH..origin/$CURRENT_BRANCH" 2>/dev/null || echo "0")

if [ "$AHEAD" -gt 0 ]; then
  echo "⚠ Your branch is ahead of origin by $AHEAD commit(s)"
  echo "Run: git push origin $CURRENT_BRANCH"
fi

if [ "$BEHIND" -gt 0 ]; then
  echo "⚠ Your branch is behind origin by $BEHIND commit(s)"
  echo "Run: git pull origin $CURRENT_BRANCH"
fi

if [ "$AHEAD" -eq 0 ] && [ "$BEHIND" -eq 0 ]; then
  echo "✓ Your branch is up to date with origin"
fi
echo ""
