#!/bin/bash
# Quick push script for BruteGamingMacros
# Usage:
#   ./scripts/push.sh "commit message"
#   ./scripts/push.sh  (commits with generic message)
#   ./scripts/push.sh --force "message"

set -e

FORCE=false
MESSAGE=""

# Parse arguments
while [[ $# -gt 0 ]]; do
  case $1 in
    --force|-f)
      FORCE=true
      shift
      ;;
    *)
      MESSAGE="$1"
      shift
      ;;
  esac
done

# Get current branch
CURRENT_BRANCH=$(git branch --show-current)

if [ -z "$CURRENT_BRANCH" ]; then
  echo "✗ Error: Not on any branch"
  exit 1
fi

echo "================================"
echo "Quick Push to $CURRENT_BRANCH"
echo "================================"
echo ""

# Check if there are changes
if [[ -z $(git status --porcelain) ]]; then
  echo "⚠ No changes to commit"
  echo "Pushing anyway..."
else
  # Set default message if not provided
  if [ -z "$MESSAGE" ]; then
    MESSAGE="chore: update $(date +%Y-%m-%d)"
  fi

  echo "Changes detected:"
  git status --short
  echo ""

  # Stage all changes
  echo "Staging all changes..."
  git add .

  # Commit
  echo "Committing: $MESSAGE"
  git commit -m "$MESSAGE"
  echo "✓ Committed"
fi

# Push
echo ""
echo "Pushing to origin/$CURRENT_BRANCH..."

if [ "$FORCE" = true ]; then
  echo "⚠ FORCE PUSH ENABLED"
  read -p "Are you sure? (y/n) " -n 1 -r
  echo
  if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo "✗ Aborted"
    exit 1
  fi
  git push --force-with-lease origin "$CURRENT_BRANCH"
else
  git push -u origin "$CURRENT_BRANCH"
fi

echo "✓ Pushed successfully"
echo ""
echo "================================"
echo "✓ PUSH COMPLETE"
echo "================================"
echo "Branch: $CURRENT_BRANCH"
echo "Remote: origin/$CURRENT_BRANCH"
echo ""
