#!/bin/bash
# Quick fix: Create and push tag to GitHub
# Run this from your BruteGamingMacros folder

echo "🏷️  Creating release tag..."

# Make sure we're in a git repo
if ! git rev-parse --git-dir > /dev/null 2>&1; then
    echo "❌ Not in a git repository!"
    exit 1
fi

# Fetch latest
echo "📥 Fetching from GitHub..."
git fetch origin

# Checkout the branch with workflows
echo "🔀 Checking out branch..."
git checkout claude/review-status-011CUKoJjdez1skkTJBaoAmJ

# Create tag
echo "🏷️  Creating tag v2.0.0..."
git tag -a v2.0.0 -m "Release v2.0.0 - Production Ready"

# Push tag
echo "⬆️  Pushing tag to GitHub..."
git push origin v2.0.0

echo ""
echo "✅ Tag created successfully!"
echo ""
echo "Now go to GitHub and create the release:"
echo "1. Visit: https://github.com/epicseo/BruteGamingMacros/releases/new"
echo "2. Select tag: v2.0.0 (should now appear in dropdown)"
echo "3. Fill in title and description"
echo "4. Click 'Publish release'"
echo ""
echo "Or the tag push alone should trigger the workflow!"
echo "Check: https://github.com/epicseo/BruteGamingMacros/actions"
