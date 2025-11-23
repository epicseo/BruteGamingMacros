#!/bin/bash
# Cleanup merged branches
# Usage:
#   ./scripts/cleanup.sh --list       (list merged branches)
#   ./scripts/cleanup.sh              (delete all merged branches)
#   ./scripts/cleanup.sh branch-name  (delete specific branch)

set -e

MODE="delete"
SPECIFIC_BRANCH=""

# Parse arguments
if [ "$1" == "--list" ] || [ "$1" == "-l" ]; then
  MODE="list"
elif [ -n "$1" ]; then
  SPECIFIC_BRANCH="$1"
  MODE="specific"
fi

echo "================================"
echo "Branch Cleanup"
echo "================================"
echo ""

# Update from remote
echo "Fetching latest from remote..."
git fetch origin --prune
echo "✓ Fetched"
echo ""

if [ "$MODE" == "specific" ]; then
  # Delete specific branch
  echo "Deleting branch: $SPECIFIC_BRANCH"
  echo ""

  # Check if branch exists locally
  if git show-ref --verify --quiet "refs/heads/$SPECIFIC_BRANCH"; then
    git branch -d "$SPECIFIC_BRANCH" 2>/dev/null || {
      echo "⚠ Branch not fully merged"
      read -p "Force delete? (y/n) " -n 1 -r
      echo
      if [[ $REPLY =~ ^[Yy]$ ]]; then
        git branch -D "$SPECIFIC_BRANCH"
        echo "✓ Branch force deleted locally"
      else
        echo "✗ Aborted"
        exit 1
      fi
    }
    echo "✓ Branch deleted locally"
  else
    echo "Branch does not exist locally"
  fi

  # Check if branch exists remotely
  if git ls-remote --exit-code --heads origin "$SPECIFIC_BRANCH" &> /dev/null; then
    read -p "Delete remote branch? (y/n) " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
      git push origin --delete "$SPECIFIC_BRANCH"
      echo "✓ Branch deleted remotely"
    fi
  else
    echo "Branch does not exist remotely"
  fi

  echo ""
  echo "✓ Cleanup complete"
  exit 0
fi

# Find merged branches
echo "Finding merged branches..."
MERGED_BRANCHES=$(git branch --merged develop | grep -v "\*\|main\|develop" | xargs 2>/dev/null || echo "")

if [ -z "$MERGED_BRANCHES" ]; then
  echo "✓ No merged branches to clean up"
  exit 0
fi

echo ""
echo "Merged branches:"
echo "$MERGED_BRANCHES" | sed 's/^/  /'
echo ""

COUNT=$(echo "$MERGED_BRANCHES" | wc -w)
echo "Total: $COUNT branch(es)"
echo ""

if [ "$MODE" == "list" ]; then
  echo "✓ List complete"
  echo ""
  echo "To delete these branches, run:"
  echo "  ./scripts/cleanup.sh"
  exit 0
fi

# Confirm deletion
read -p "Delete these $COUNT branch(es)? (y/n) " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
  echo "✗ Aborted"
  exit 0
fi

# Delete local branches
echo ""
echo "Deleting local branches..."
DELETED=0
for branch in $MERGED_BRANCHES; do
  if git branch -d "$branch" 2>/dev/null; then
    echo "✓ Deleted: $branch"
    ((DELETED++))
  else
    echo "⚠ Failed: $branch"
  fi
done

echo ""
echo "Deleted $DELETED local branch(es)"
echo ""

# Ask about remote branches
read -p "Check for merged remote branches? (y/n) " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
  echo ""
  echo "Finding merged remote branches..."

  # Get remote branches merged into origin/develop
  REMOTE_MERGED=$(git branch -r --merged origin/develop | \
    grep "origin/" | \
    grep -v "origin/main\|origin/develop\|origin/HEAD" | \
    sed 's/origin\///' | \
    xargs 2>/dev/null || echo "")

  if [ -z "$REMOTE_MERGED" ]; then
    echo "✓ No merged remote branches found"
  else
    echo ""
    echo "Merged remote branches:"
    echo "$REMOTE_MERGED" | sed 's/^/  /'
    echo ""

    REMOTE_COUNT=$(echo "$REMOTE_MERGED" | wc -w)
    echo "Total: $REMOTE_COUNT remote branch(es)"
    echo ""

    read -p "Delete these remote branches? (y/n) " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
      echo ""
      echo "Deleting remote branches..."
      REMOTE_DELETED=0
      for branch in $REMOTE_MERGED; do
        if git push origin --delete "$branch" 2>/dev/null; then
          echo "✓ Deleted remote: $branch"
          ((REMOTE_DELETED++))
        else
          echo "⚠ Failed: $branch"
        fi
      done
      echo ""
      echo "Deleted $REMOTE_DELETED remote branch(es)"
    fi
  fi
fi

echo ""
echo "================================"
echo "✓ CLEANUP COMPLETE"
echo "================================"
echo "Local branches deleted:  $DELETED"
echo ""
echo "Remaining branches:"
git branch | sed 's/^/  /'
echo ""
