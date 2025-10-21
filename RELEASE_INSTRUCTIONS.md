# 🚀 Release Instructions - Get Your .exe on GitHub!

All the code is ready! Here's exactly what you need to do to publish your .exe files on GitHub:

## Option 1: Quick Release (Recommended - 2 minutes)

**On your local machine** (not in Claude Code), run these commands:

```bash
# 1. Navigate to your project folder
cd /path/to/BruteGamingMacros

# 2. Fetch the latest changes
git fetch origin

# 3. Checkout the prepared branch
git checkout claude/review-status-011CUKoJjdez1skkTJBaoAmJ

# 4. Create main branch (if it doesn't exist)
git checkout -b main

# 5. Push to GitHub
git push -u origin main

# 6. Create and push the release tag
git tag -a v2.0.0 -m "Release v2.0.0 - Production Ready"
git push origin v2.0.0
```

**That's it!** GitHub Actions will automatically:
✅ Build all 4 .exe versions (Standard, MR, HR, LR)
✅ Create .zip packages
✅ Publish to: https://github.com/epicseo/BruteGamingMacros/releases

⏱️ **Wait 5-10 minutes** for the build to complete, then check:
👉 https://github.com/epicseo/BruteGamingMacros/releases

---

## Option 2: Via GitHub Web Interface (3 minutes)

If you prefer using GitHub's website:

### Step 1: Create Pull Request
1. Go to: https://github.com/epicseo/BruteGamingMacros/pull/new/claude/review-status-011CUKoJjdez1skkTJBaoAmJ
2. Click "Create Pull Request"
3. Click "Merge Pull Request"
4. Click "Confirm Merge"

### Step 2: Create Release
1. Go to: https://github.com/epicseo/BruteGamingMacros/releases/new
2. Click "Choose a tag" → type `v2.0.0` → "Create new tag: v2.0.0"
3. **Target**: Select `main` branch
4. **Release title**: `Brute Gaming Macros v2.0.0`
5. **Description**: Copy from CHANGELOG.md (under [2.0.0])
6. Click "Publish release"

**Done!** GitHub Actions will automatically build and attach the .exe files.

---

## Option 3: Using GitHub CLI (1 minute)

If you have `gh` installed:

```bash
# Login to GitHub
gh auth login

# Create release (triggers automated build)
gh release create v2.0.0 \
  --title "Brute Gaming Macros v2.0.0" \
  --notes-file CHANGELOG.md \
  --target claude/review-status-011CUKoJjdez1skkTJBaoAmJ
```

---

## What Happens Next?

### GitHub Actions Will:
1. ✅ Restore NuGet packages
2. ✅ Build `Release` configuration → `BruteGamingMacros-v2.0.0.zip`
3. ✅ Build `Release-MR` configuration → `BruteGamingMacros-MR-v2.0.0.zip`
4. ✅ Build `Release-HR` configuration → `BruteGamingMacros-HR-v2.0.0.zip`
5. ✅ Build `Release-LR` configuration → `BruteGamingMacros-LR-v2.0.0.zip`
6. ✅ Create GitHub Release with all .zip files attached

### Each .zip Contains:
- ✅ `BruteGamingMacros.exe` (ready to run)
- ✅ All embedded dependencies (via Costura.Fody)
- ✅ Config/ folder (for settings)
- ✅ Profile/ folder (for character profiles)

---

## Verify the Release

### Check Build Status:
1. Go to: https://github.com/epicseo/BruteGamingMacros/actions
2. Look for "Release" workflow
3. Wait for green checkmark ✅ (~5-10 minutes)

### Download Your .exe:
1. Go to: https://github.com/epicseo/BruteGamingMacros/releases
2. Click on `v2.0.0`
3. Download the appropriate .zip for your server:
   - **BruteGamingMacros-v2.0.0.zip** - Standard/Default (MR)
   - **BruteGamingMacros-MR-v2.0.0.zip** - OsRO Midrate
   - **BruteGamingMacros-HR-v2.0.0.zip** - OsRO Highrate
   - **BruteGamingMacros-LR-v2.0.0.zip** - OsRO Lowrate

---

## Troubleshooting

### If Build Fails:
1. Check: https://github.com/epicseo/BruteGamingMacros/actions
2. Click on the failed workflow
3. Look at the error logs
4. Common issues:
   - Missing NuGet packages → Check packages.config
   - MSBuild errors → Check .csproj file
   - Missing files → Check git status

### If No Release Appears:
1. Make sure you pushed the tag: `git push origin v2.0.0`
2. Check Actions tab for running workflows
3. Wait 10 minutes (builds take time)

### If You Get 403 Errors:
- You might be using a restricted git context
- Use Option 2 (GitHub Web Interface) instead
- Or clone fresh repo locally and follow Option 1

---

## Future Releases

For your next release (e.g., v2.0.1):

```bash
# 1. Update version numbers in code
# - Utils/AppConfig.cs: Version = "v2.0.1"
# - Properties/AssemblyInfo.cs: AssemblyVersion("2.0.1.0")

# 2. Update CHANGELOG.md
# - Add release notes

# 3. Commit and tag
git add .
git commit -m "chore: bump version to 2.0.1"
git tag -a v2.0.1 -m "Release v2.0.1"
git push origin main
git push origin v2.0.1

# 4. Done! Check releases page in 5-10 minutes
```

---

## Summary

**Everything is ready!** The code, workflows, and documentation are all in place.

**You just need to**:
1. Choose your preferred method above
2. Push the tag or create release on GitHub
3. Wait 5-10 minutes
4. Download your .exe files!

**Release URL** (will be live after you complete above):
👉 https://github.com/epicseo/BruteGamingMacros/releases/tag/v2.0.0

**Questions?** Check the logs at:
👉 https://github.com/epicseo/BruteGamingMacros/actions

---

**Good luck! Your automated .exe builds are ready to go! 🚀**
