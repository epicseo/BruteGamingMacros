#!/bin/bash
# Release script - creates and pushes version tags
# Usage:
#   ./scripts/release.sh 2.0.2
#   ./scripts/release.sh 2.0.3-beta
#   ./scripts/release.sh 2.0.2 "Custom release message"

set -e

VERSION=${1}
MESSAGE=${2}

if [ -z "$VERSION" ]; then
  echo "Usage: ./scripts/release.sh <version> [message]"
  echo ""
  echo "Examples:"
  echo "  ./scripts/release.sh 2.0.2"
  echo "  ./scripts/release.sh 2.0.3-beta"
  echo "  ./scripts/release.sh 2.0.2 \"Major release with new features\""
  echo ""
  echo "Version format: MAJOR.MINOR.PATCH[-PRERELEASE]"
  echo "  2.0.0        - Major release"
  echo "  2.1.0        - Minor release"
  echo "  2.0.1        - Patch release"
  echo "  2.1.0-beta   - Prerelease"
  echo "  2.1.0-rc1    - Release candidate"
  exit 1
fi

# Validate version format
if [[ ! $VERSION =~ ^[0-9]+\.[0-9]+\.[0-9]+(-[a-zA-Z0-9]+)?$ ]]; then
  echo "✗ Invalid version format: $VERSION"
  echo "Expected format: MAJOR.MINOR.PATCH[-PRERELEASE]"
  echo "Example: 2.0.1 or 2.1.0-beta"
  exit 1
fi

TAG="v$VERSION"

echo "================================"
echo "Create Release: $TAG"
echo "================================"
echo ""

# Check if tag already exists
if git rev-parse "$TAG" >/dev/null 2>&1; then
  echo "✗ Tag $TAG already exists!"
  echo ""
  echo "Existing tags:"
  git tag -l "v*" | tail -5
  echo ""
  read -p "Delete existing tag and recreate? (y/n) " -n 1 -r
  echo
  if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo "Deleting local tag..."
    git tag -d "$TAG"
    echo "Deleting remote tag (if exists)..."
    git push origin ":refs/tags/$TAG" 2>/dev/null || true
    echo "✓ Existing tag deleted"
  else
    echo "✗ Aborted"
    exit 1
  fi
fi

# Ensure we're on main branch
CURRENT_BRANCH=$(git branch --show-current)
if [ "$CURRENT_BRANCH" != "main" ]; then
  echo "⚠ Warning: Not on main branch (currently on $CURRENT_BRANCH)"
  read -p "Switch to main? (y/n) " -n 1 -r
  echo
  if [[ $REPLY =~ ^[Yy]$ ]]; then
    git checkout main
  else
    echo "⚠ Continuing on $CURRENT_BRANCH"
  fi
fi

# Update from remote
echo "[1/6] Updating from remote..."
git fetch origin
git pull origin main
echo "✓ Updated from origin/main"
echo ""

# Check for uncommitted changes
if [[ -n $(git status --porcelain) ]]; then
  echo "✗ You have uncommitted changes:"
  git status --short
  echo ""
  echo "Please commit or stash changes before creating a release"
  exit 1
fi

# Update version file
echo "[2/6] Updating version file..."
echo "$VERSION" > build/version.txt
git add build/version.txt
git commit -m "chore: bump version to $VERSION" || echo "Version file already up to date"
echo "✓ Version file updated"
echo ""

# Extract changelog
echo "[3/6] Extracting changelog..."
CHANGELOG_FILE="docs/CHANGELOG.md"
if [ -f "$CHANGELOG_FILE" ]; then
  # Try to extract version-specific changelog
  CHANGELOG=$(sed -n "/## \[$VERSION\]/,/## \[/p" "$CHANGELOG_FILE" | sed '$d' | sed '1d')

  if [ -z "$CHANGELOG" ]; then
    CHANGELOG="Release v$VERSION\n\nSee CHANGELOG.md for full details."
  fi
else
  CHANGELOG="Release v$VERSION"
fi
echo "✓ Changelog extracted"
echo ""

# Set release message
if [ -z "$MESSAGE" ]; then
  MESSAGE="Release v$VERSION

$CHANGELOG"
fi

# Create annotated tag
echo "[4/6] Creating tag: $TAG..."
git tag -a "$TAG" -m "$MESSAGE"
echo "✓ Tag created"
echo ""

# Show tag info
echo "Tag details:"
git show "$TAG" --no-patch
echo ""

# Push changes and tag
echo "[5/6] Pushing to remote..."
git push origin main
git push origin "$TAG"
echo "✓ Pushed to remote"
echo ""

# Trigger release workflow
echo "[6/6] Triggering release workflow..."
echo "GitHub Actions will automatically build and create release"
sleep 2
echo "✓ Workflow triggered"
echo ""

echo "================================"
echo "✓ RELEASE CREATED"
echo "================================"
echo "Version:  $VERSION"
echo "Tag:      $TAG"
echo "Branch:   main"
echo ""
echo "GitHub Release will be created automatically by GitHub Actions"
echo "Monitor progress: https://github.com/epicseo/BruteGamingMacros/actions"
echo "View release:     https://github.com/epicseo/BruteGamingMacros/releases/tag/$TAG"
echo ""
echo "Artifacts will include:"
echo "  - BruteGamingMacros-v$VERSION-portable.zip"
echo "  - BruteGamingMacros-Setup-v$VERSION.exe (if NSIS succeeds)"
echo "  - checksums.txt (SHA256 hashes)"
echo ""
