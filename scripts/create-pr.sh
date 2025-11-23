#!/bin/bash
# Create pull request script
# Requires GitHub CLI (gh) to be installed and authenticated
# Usage:
#   ./scripts/create-pr.sh
#   ./scripts/create-pr.sh "PR title"
#   ./scripts/create-pr.sh --base main "Hotfix title"

set -e

BASE_BRANCH="develop"
TITLE=""

# Parse arguments
while [[ $# -gt 0 ]]; do
  case $1 in
    --base|-b)
      BASE_BRANCH="$2"
      shift 2
      ;;
    *)
      TITLE="$1"
      shift
      ;;
  esac
done

# Check if gh is installed
if ! command -v gh &> /dev/null; then
  echo "✗ GitHub CLI (gh) is not installed"
  echo ""
  echo "Install with:"
  echo "  Ubuntu/WSL: See docs/WSL_DEVELOPMENT_GUIDE.md"
  echo "  Or visit: https://cli.github.com/"
  exit 1
fi

# Check authentication
if ! gh auth status &> /dev/null; then
  echo "✗ Not authenticated with GitHub"
  echo ""
  echo "Run: gh auth login"
  exit 1
fi

CURRENT_BRANCH=$(git branch --show-current)

if [ -z "$CURRENT_BRANCH" ]; then
  echo "✗ Not on any branch"
  exit 1
fi

if [ "$CURRENT_BRANCH" == "main" ] || [ "$CURRENT_BRANCH" == "develop" ]; then
  echo "✗ Cannot create PR from $CURRENT_BRANCH"
  echo "Create a feature/bugfix branch first"
  exit 1
fi

echo "================================"
echo "Create Pull Request"
echo "================================"
echo ""
echo "Branch: $CURRENT_BRANCH → $BASE_BRANCH"
echo ""

# Check if changes are pushed
if ! git ls-remote --exit-code --heads origin "$CURRENT_BRANCH" &> /dev/null; then
  echo "⚠ Branch not pushed to remote"
  read -p "Push now? (y/n) " -n 1 -r
  echo
  if [[ $REPLY =~ ^[Yy]$ ]]; then
    git push -u origin "$CURRENT_BRANCH"
    echo "✓ Pushed to remote"
  else
    echo "✗ Aborted - push branch first"
    exit 1
  fi
fi

# Check if PR already exists
EXISTING_PR=$(gh pr list --head "$CURRENT_BRANCH" --json number --jq '.[0].number' 2>/dev/null || echo "")

if [ -n "$EXISTING_PR" ]; then
  echo "⚠ Pull request already exists: #$EXISTING_PR"
  PR_URL=$(gh pr view "$EXISTING_PR" --json url -q .url)
  echo "URL: $PR_URL"
  echo ""
  read -p "Open in browser? (y/n) " -n 1 -r
  echo
  if [[ $REPLY =~ ^[Yy]$ ]]; then
    gh pr view "$EXISTING_PR" --web
  fi
  exit 0
fi

# Generate title if not provided
if [ -z "$TITLE" ]; then
  # Extract from branch name
  BRANCH_PART=$(echo "$CURRENT_BRANCH" | sed 's/^[^\/]*\///' | tr '-' ' ' | sed 's/\b\(.\)/\u\1/g')
  TITLE="$BRANCH_PART"
fi

# Get commit messages for PR body
COMMITS=$(git log --oneline "$BASE_BRANCH..$CURRENT_BRANCH" --pretty=format:"- %s" | head -10)

# Create PR body
PR_BODY="## Description
<!-- Describe your changes in detail -->

## Changes
$COMMITS

## Type of Change
- [ ] Bug fix (non-breaking change which fixes an issue)
- [ ] New feature (non-breaking change which adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update
- [ ] Refactoring (no functional changes)

## Testing
- [ ] Built successfully in Release configuration
- [ ] Tested manually
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
"

# Create PR
echo "Creating pull request..."
echo "Title: $TITLE"
echo "Base:  $BASE_BRANCH"
echo "Head:  $CURRENT_BRANCH"
echo ""

PR_URL=$(gh pr create \
  --base "$BASE_BRANCH" \
  --head "$CURRENT_BRANCH" \
  --title "$TITLE" \
  --body "$PR_BODY" \
  2>&1 | grep -o 'https://[^ ]*')

echo ""
echo "================================"
echo "✓ PULL REQUEST CREATED"
echo "================================"
echo "Title:  $TITLE"
echo "Branch: $CURRENT_BRANCH → $BASE_BRANCH"
echo "URL:    $PR_URL"
echo ""
echo "Next steps:"
echo "1. Review the PR description and update if needed"
echo "2. Request reviews from team members"
echo "3. Wait for CI checks to pass"
echo "4. Merge when approved"
echo ""

# Ask to open in browser
read -p "Open PR in browser? (y/n) " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
  gh pr view --web
fi
