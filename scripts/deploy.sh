#!/bin/bash
# Deployment script - creates branch, commits, and pushes
# Usage:
#   ./scripts/deploy.sh feature my-feature "Add new feature"
#   ./scripts/deploy.sh bugfix issue-123 "Fix crash"
#   ./scripts/deploy.sh hotfix critical "Fix critical bug"

set -e

TYPE=${1}
NAME=${2}
MESSAGE=${3}

# Validate arguments
if [ -z "$TYPE" ] || [ -z "$NAME" ]; then
  echo "Usage: ./scripts/deploy.sh <type> <name> [message]"
  echo ""
  echo "Types:"
  echo "  feature    - New feature development"
  echo "  bugfix     - Bug fix"
  echo "  hotfix     - Emergency production fix"
  echo "  refactor   - Code refactoring"
  echo "  docs       - Documentation updates"
  echo ""
  echo "Examples:"
  echo "  ./scripts/deploy.sh feature auto-targeting \"Add auto-targeting system\""
  echo "  ./scripts/deploy.sh bugfix issue-123 \"Fix memory leak\""
  echo "  ./scripts/deploy.sh hotfix critical \"Fix crash on startup\""
  exit 1
fi

# Validate type
case $TYPE in
  feature|bugfix|hotfix|refactor|docs)
    ;;
  *)
    echo "✗ Invalid type: $TYPE"
    echo "Valid types: feature, bugfix, hotfix, refactor, docs"
    exit 1
    ;;
esac

BRANCH_NAME="${TYPE}/${NAME}"

echo "================================"
echo "Deploy: $BRANCH_NAME"
echo "================================"
echo ""

# Check for uncommitted changes
if [[ -n $(git status --porcelain) ]]; then
  echo "⚠ You have uncommitted changes:"
  git status --short
  echo ""
  read -p "Commit these changes first? (y/n) " -n 1 -r
  echo
  if [[ $REPLY =~ ^[Yy]$ ]]; then
    if [ -z "$MESSAGE" ]; then
      read -p "Commit message: " MESSAGE
    fi
    git add .
    git commit -m "wip: $MESSAGE"
    echo "✓ Changes committed"
  else
    echo "Please commit or stash changes before deploying"
    exit 1
  fi
fi

# Determine base branch
if [ "$TYPE" == "hotfix" ]; then
  BASE_BRANCH="main"
else
  BASE_BRANCH="develop"
fi

echo "[1/5] Updating $BASE_BRANCH..."
git fetch origin
git checkout "$BASE_BRANCH"
git pull origin "$BASE_BRANCH"
echo "✓ $BASE_BRANCH updated"
echo ""

# Create branch
echo "[2/5] Creating branch: $BRANCH_NAME..."
if git show-ref --verify --quiet "refs/heads/$BRANCH_NAME"; then
  echo "⚠ Branch $BRANCH_NAME already exists"
  read -p "Switch to existing branch? (y/n) " -n 1 -r
  echo
  if [[ $REPLY =~ ^[Yy]$ ]]; then
    git checkout "$BRANCH_NAME"
  else
    echo "✗ Aborted"
    exit 1
  fi
else
  git checkout -b "$BRANCH_NAME"
  echo "✓ Branch created"
fi
echo ""

# Push to remote
echo "[3/5] Pushing to remote..."
git push -u origin "$BRANCH_NAME"
echo "✓ Pushed to origin/$BRANCH_NAME"
echo ""

# Create initial commit if message provided and no changes
if [ -n "$MESSAGE" ] && [[ -z $(git status --porcelain) ]]; then
  echo "[4/5] Creating initial commit..."
  # Create a placeholder file to commit
  mkdir -p .github/workflows
  echo "# $MESSAGE" > ".github/BRANCH_${TYPE}_${NAME}.md"
  git add ".github/BRANCH_${TYPE}_${NAME}.md"
  git commit -m "chore: initialize $TYPE branch for $NAME"
  git push origin "$BRANCH_NAME"
  echo "✓ Initial commit created"
else
  echo "[4/5] Skipping initial commit (no message or uncommitted changes)"
fi
echo ""

# Summary
echo "[5/5] Deployment complete!"
echo ""
echo "================================"
echo "DEPLOYMENT SUMMARY"
echo "================================"
echo "Type:   $TYPE"
echo "Name:   $NAME"
echo "Branch: $BRANCH_NAME"
echo "Base:   $BASE_BRANCH"
echo "Remote: origin/$BRANCH_NAME"
echo ""
echo "Next steps:"
echo "1. Make your changes"
echo "2. Run: ./scripts/push.sh \"commit message\""
echo "3. Run: ./scripts/create-pr.sh \"$MESSAGE\""
echo ""
