#!/bin/bash
# Quick Release Script for Brute Gaming Macros
# Run this script locally to publish v2.0.0 with automated .exe builds

set -e  # Exit on error

echo "🚀 Brute Gaming Macros - Release Script"
echo "========================================"
echo ""

# Check if we're in a git repo
if ! git rev-parse --git-dir > /dev/null 2>&1; then
    echo "❌ Error: Not in a git repository"
    echo "   Please run this from the BruteGamingMacros folder"
    exit 1
fi

echo "📥 Step 1: Fetching latest changes from GitHub..."
git fetch origin

echo ""
echo "🔀 Step 2: Checking out the prepared branch..."
git checkout claude/review-status-011CUKoJjdez1skkTJBaoAmJ

echo ""
echo "🌿 Step 3: Creating main branch..."
if git show-ref --quiet refs/heads/main; then
    echo "   Main branch already exists, switching to it..."
    git checkout main
    git merge claude/review-status-011CUKoJjdez1skkTJBaoAmJ --no-edit
else
    echo "   Creating new main branch..."
    git checkout -b main
fi

echo ""
echo "⬆️  Step 4: Pushing main branch to GitHub..."
git push -u origin main

echo ""
echo "🏷️  Step 5: Creating release tag v2.0.0..."
if git tag -l | grep -q "^v2.0.0$"; then
    echo "   Tag v2.0.0 already exists, skipping creation..."
else
    git tag -a v2.0.0 -m "Release v2.0.0 - Production Ready

Production-ready release with:
- Superior performance engines (1000+ APS capability)
- Automated CI/CD with GitHub Actions
- Comprehensive documentation
- Security hardening and input validation
- Updated dependencies (all security patches applied)
- Multi-server support (MR/HR/LR)

See CHANGELOG.md for full details."
fi

echo ""
echo "⬆️  Step 6: Pushing release tag to GitHub..."
git push origin v2.0.0

echo ""
echo "✅ Release process initiated!"
echo ""
echo "GitHub Actions is now building your .exe files..."
echo "This will take approximately 5-10 minutes."
echo ""
echo "📊 Monitor build progress:"
echo "   👉 https://github.com/epicseo/BruteGamingMacros/actions"
echo ""
echo "📦 Download releases when ready:"
echo "   👉 https://github.com/epicseo/BruteGamingMacros/releases/tag/v2.0.0"
echo ""
echo "The following .zip files will be created:"
echo "   • BruteGamingMacros-v2.0.0.zip (Standard)"
echo "   • BruteGamingMacros-MR-v2.0.0.zip (Midrate)"
echo "   • BruteGamingMacros-HR-v2.0.0.zip (Highrate)"
echo "   • BruteGamingMacros-LR-v2.0.0.zip (Lowrate)"
echo ""
echo "🎉 Done! Check GitHub in a few minutes!"
