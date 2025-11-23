# GitHub Actions Workflows Documentation

**Last Updated:** 2025-11-17
**Project:** BruteGamingMacros

---

## Overview

This directory contains the GitHub Actions CI/CD workflows for automated building, testing, quality checks, and releases.

---

## Active Workflows

### 1. `build-ci.yml` - Continuous Integration Build

**Purpose:** Automated builds on every push and pull request
**Triggers:**
- Push to: `main`, `develop`, `claude/**`
- Pull requests to: `main`, `develop`
- Manual dispatch

**What it does:**
1. Builds all 4 configurations in parallel (fail-fast: false)
   - `Release` (required - must succeed)
   - `Release-MR` (optional - can fail)
   - `Release-HR` (optional - can fail)
   - `Release-LR` (optional - can fail)
2. Uploads Release build artifacts (7 day retention)
3. Verifies executables were created
4. Provides summary of build results

**Success criteria:** Main `Release` build must succeed. Optional builds can fail.

**Duration:** ~5-7 minutes

---

### 2. `code-quality.yml` - Code Quality & Security

**Purpose:** Static analysis, security scanning, and quality checks
**Triggers:**
- Push to: `main`, `develop`, `claude/**`
- Pull requests to: `main`, `develop`
- Manual dispatch

**What it does:**
1. **Build & Analyze Job:**
   - Builds in Debug and Release configurations
   - Checks for compiler warnings
   - Counts source files and lines of code
   - Generates code quality metrics

2. **Security Scan Job:**
   - Scans for sensitive data patterns (API keys, passwords, tokens)
   - Checks dependencies for known vulnerabilities
   - Reviews packages for security issues

**Success criteria:** All checks pass (warnings don't fail build)

**Duration:** ~6-8 minutes

---

### 3. `release-consolidated.yml` - Production Release

**Purpose:** Build and publish production releases
**Triggers:**
- Tags matching `v*.*.*` (semantic versioning only)
- Manual dispatch with version input

**What it does:**
1. Extracts version from tag or input
2. Builds all configurations (Release required, others optional)
3. Installs NSIS via Chocolatey
4. Builds Windows installer (.exe)
5. Creates portable ZIP packages for all successful builds
6. Generates SHA256 checksums for all artifacts
7. Extracts changelog from `docs/CHANGELOG.md`
8. Creates GitHub Release with all artifacts
9. Marks as prerelease if version contains `alpha`, `beta`, or `rc`

**Artifacts created:**
- `BruteGamingMacros-v{VERSION}-portable.zip` (main)
- `BruteGamingMacros-v{VERSION}-MR-portable.zip` (if MR build succeeds)
- `BruteGamingMacros-v{VERSION}-HR-portable.zip` (if HR build succeeds)
- `BruteGamingMacros-v{VERSION}-LR-portable.zip` (if LR build succeeds)
- `BruteGamingMacros-Setup-v{VERSION}.exe` (NSIS installer)
- `checksums.txt` (SHA256 hashes)

**Success criteria:** Main Release build and at least one portable package created

**Duration:** ~10-15 minutes

**Permissions:** Requires `contents: write` for creating releases

---

## Deprecated Workflows

### ❌ `build.yml` (REPLACED)
**Status:** Replaced by `build-ci.yml`
**Reason:** Improved error handling, clearer output, better artifact management

### ❌ `release.yml` (REPLACED)
**Status:** Replaced by `release-consolidated.yml`
**Reason:** Duplicate functionality with build-release.yml, incomplete features

### ❌ `build-release.yml` (REPLACED)
**Status:** Replaced by `release-consolidated.yml`
**Reason:** Consolidated with release.yml to eliminate duplication

---

## Workflow Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    GitHub Actions Workflows                  │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  CONTINUOUS INTEGRATION (Every Push/PR)                     │
│  ┌────────────────────┐  ┌────────────────────┐            │
│  │   build-ci.yml     │  │ code-quality.yml   │            │
│  │   - Build all      │  │ - Static analysis  │            │
│  │   - Verify builds  │  │ - Security scan    │            │
│  │   - Upload Release │  │ - Quality metrics  │            │
│  └────────────────────┘  └────────────────────┘            │
│                                                              │
│  RELEASE DEPLOYMENT (Tags Only)                             │
│  ┌────────────────────────────────────────────┐            │
│  │       release-consolidated.yml              │            │
│  │       - Build all configurations            │            │
│  │       - Create NSIS installer               │            │
│  │       - Package portable ZIPs               │            │
│  │       - Generate checksums                  │            │
│  │       - Create GitHub Release               │            │
│  └────────────────────────────────────────────┘            │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## Configuration Matrix

All build workflows use the same configuration matrix:

| Configuration | Purpose | Required | Build Output |
|---------------|---------|----------|--------------|
| `Release` | Standard production build | ✅ Yes | `bin/Release/` |
| `Release-MR` | Midrate server build | ⚠️ Optional | `bin/Release-MR/` |
| `Release-HR` | Highrate server build | ⚠️ Optional | `bin/Release-HR/` |
| `Release-LR` | Lowrate server build | ⚠️ Optional | `bin/Release-LR/` |

**Notes:**
- Only `Release` is required to succeed
- Optional builds use `continue-on-error: true`
- Matrix uses `fail-fast: false` to run all builds even if some fail

---

## Permissions Required

### Repository Permissions
```yaml
Contents: Write  # For creating releases and tags
Actions: Read    # For workflow execution
```

### Secrets Required
```yaml
GITHUB_TOKEN  # Automatically provided by GitHub Actions
```

**Optional Secrets (for future features):**
```yaml
CODE_SIGNING_CERT      # Code signing certificate
CODE_SIGNING_PASSWORD  # Certificate password
```

---

## Triggering Workflows

### Automatic Triggers

**On every push to branches:**
```bash
git push origin main           # Triggers: build-ci, code-quality
git push origin develop        # Triggers: build-ci, code-quality
git push origin claude/feature # Triggers: build-ci, code-quality
```

**On pull requests:**
```bash
gh pr create --base main       # Triggers: code-quality
gh pr create --base develop    # Triggers: code-quality
```

**On version tags:**
```bash
git tag -a v2.0.2 -m "Release v2.0.2"
git push origin v2.0.2         # Triggers: release-consolidated
```

### Manual Triggers

**Build workflow:**
```bash
gh workflow run build-ci.yml
```

**Release workflow:**
```bash
gh workflow run release-consolidated.yml -f version=2.0.2
```

**Code quality workflow:**
```bash
gh workflow run code-quality.yml
```

---

## Workflow Status Badges

Add to README.md:

```markdown
![Build Status](https://github.com/epicseo/BruteGamingMacros/actions/workflows/build-ci.yml/badge.svg)
![Code Quality](https://github.com/epicseo/BruteGamingMacros/actions/workflows/code-quality.yml/badge.svg)
![Release](https://github.com/epicseo/BruteGamingMacros/actions/workflows/release-consolidated.yml/badge.svg)
```

---

## Troubleshooting

### Build Failures

**Symptom:** Build-ci.yml fails with "executable not found"
**Cause:** Main Release build failed to compile
**Solution:**
1. Check compilation errors in workflow logs
2. Verify all files are in .csproj `<Compile Include>`
3. Ensure NuGet packages restored correctly
4. Test build locally: `msbuild BruteGamingMacros.sln /p:Configuration=Release`

**Symptom:** Optional builds (MR/HR/LR) fail
**Cause:** Expected behavior - these are optional
**Solution:** No action needed if Release build succeeds

### Release Failures

**Symptom:** release-consolidated.yml fails to create release
**Cause:** Missing `contents: write` permission
**Solution:** Verify `permissions:` block at top of workflow file

**Symptom:** No installer created
**Cause:** NSIS installation failed or installer.nsi missing
**Solution:**
1. Check NSIS installation step logs
2. Verify `installer/installer.nsi` exists
3. Test locally: `choco install nsis -y`

**Symptom:** Checksums file empty
**Cause:** No artifacts found to hash
**Solution:** Check previous build steps for failures

### Code Quality Failures

**Symptom:** Security scan finds sensitive data
**Cause:** Hardcoded API keys, passwords, or tokens
**Solution:**
1. Review flagged files and lines
2. Move secrets to environment variables
3. Use GitHub Secrets for CI/CD
4. Add patterns to .gitignore

**Symptom:** Compiler warnings detected
**Cause:** Code quality issues
**Solution:**
1. Review warning messages in logs
2. Fix warnings in source code
3. Note: Warnings don't fail the build (informational only)

---

## Maintenance

### Updating Workflows

1. **Test changes locally first:**
   ```bash
   # Validate YAML syntax
   yamllint .github/workflows/*.yml

   # Test workflow logic
   act -j build  # Using nektos/act
   ```

2. **Make changes on feature branch:**
   ```bash
   git checkout -b update-workflows
   # Edit workflow files
   git add .github/workflows/
   git commit -m "ci: update workflow XYZ"
   git push origin update-workflows
   ```

3. **Test on pull request:**
   - Create PR to trigger workflows
   - Verify all checks pass
   - Review workflow run logs

4. **Merge when validated:**
   - Merge PR to deploy changes
   - Monitor first run on main branch

### Adding New Workflows

1. Create new file: `.github/workflows/new-workflow.yml`
2. Define triggers, jobs, and steps
3. Set appropriate permissions
4. Test with manual dispatch first
5. Document in this README

### Deprecating Workflows

1. Rename to `.github/workflows/DEPRECATED-old-workflow.yml`
2. Add deprecation notice at top of file
3. Update this README
4. Monitor for 2 weeks
5. Delete if no issues

---

## Best Practices

### Workflow Design

✅ **DO:**
- Use `fail-fast: false` for matrix builds
- Set `continue-on-error` for optional steps
- Provide clear, colored output messages
- Upload artifacts with reasonable retention
- Use semantic versioning for releases
- Cache NuGet packages when possible
- Keep workflows DRY (Don't Repeat Yourself)

❌ **DON'T:**
- Hardcode version numbers
- Use `if-no-files-found: error` for optional artifacts
- Fail builds on warnings
- Store secrets in workflow files
- Duplicate workflow logic
- Use wildcards in critical paths

### Security

✅ **DO:**
- Pin action versions (`uses: actions/checkout@v4`)
- Use `permissions:` blocks with minimal scope
- Validate inputs on `workflow_dispatch`
- Review third-party actions before use
- Use GitHub Secrets for sensitive data
- Audit workflow changes in PRs

❌ **DON'T:**
- Use `permissions: write-all`
- Trust user inputs without validation
- Expose secrets in logs
- Use unverified third-party actions
- Skip security reviews

### Performance

✅ **DO:**
- Run jobs in parallel when possible
- Cache dependencies (NuGet, npm)
- Use matrix builds efficiently
- Clean up artifacts regularly
- Set appropriate retention periods
- Use `if:` conditions to skip unnecessary steps

❌ **DON'T:**
- Run unnecessary builds
- Keep artifacts forever
- Download entire repo if not needed
- Install tools multiple times
- Use excessive logging

---

## Migration Guide

### From Old Workflows to New

#### Replacing build.yml with build-ci.yml

**Changes:**
- Better error handling for optional builds
- Clearer output with colored messages
- Improved artifact naming (includes SHA)
- Only uploads Release artifacts (saves space)

**Migration:**
```bash
# 1. Rename old workflow
mv .github/workflows/build.yml .github/workflows/DEPRECATED-build.yml

# 2. Activate new workflow
# (build-ci.yml is already active)

# 3. Test with push
git push origin develop

# 4. Verify new workflow runs successfully

# 5. Delete old workflow after 2 weeks
rm .github/workflows/DEPRECATED-build.yml
```

#### Replacing release.yml and build-release.yml with release-consolidated.yml

**Changes:**
- Single source of truth for releases
- Comprehensive artifact creation
- Better error messages
- Improved changelog extraction
- Includes NSIS installer from build-release.yml
- Maintains all features from both workflows

**Migration:**
```bash
# 1. Rename old workflows
mv .github/workflows/release.yml .github/workflows/DEPRECATED-release.yml
mv .github/workflows/build-release.yml .github/workflows/DEPRECATED-build-release.yml

# 2. Activate new workflow
# (release-consolidated.yml is already active)

# 3. Test with manual dispatch
gh workflow run release-consolidated.yml -f version=2.0.2-test

# 4. Verify artifacts created correctly

# 5. Test with actual tag
git tag -a v2.0.2 -m "Test release"
git push origin v2.0.2

# 6. Delete old workflows after successful release
rm .github/workflows/DEPRECATED-release.yml
rm .github/workflows/DEPRECATED-build-release.yml
```

---

## Future Enhancements

### Planned Improvements

- [ ] Add code signing step to release workflow
- [ ] Implement automatic version bumping
- [ ] Add unit test execution (when tests are created)
- [ ] Deploy to staging environment on develop branch
- [ ] Add performance benchmarking
- [ ] Integrate SonarQube analysis
- [ ] Add automated changelog generation
- [ ] Implement branch cleanup workflow
- [ ] Add PR template validation
- [ ] Create workflow health dashboard

### Ideas for Consideration

- Deploy preview builds for PRs
- Automated dependency updates (Dependabot)
- Nightly builds with extended testing
- Multi-platform builds (if Linux/Mac support added)
- Container builds (Docker)
- Automated documentation deployment
- Integration test suite
- Load testing workflow

---

## Support

### Getting Help

**Workflow Issues:**
1. Check workflow run logs in GitHub Actions tab
2. Review this documentation
3. Search existing issues: https://github.com/epicseo/BruteGamingMacros/issues
4. Create new issue with workflow run URL

**Questions:**
- Open discussion: https://github.com/epicseo/BruteGamingMacros/discussions
- Tag with `ci/cd` or `github-actions`

**Contributing:**
- See `docs/CONTRIBUTING.md`
- Workflow changes require testing before merge
- Document all changes in this README

---

## Changelog

### 2025-11-17 - Major Refactor
- Created `build-ci.yml` to replace `build.yml`
- Created `release-consolidated.yml` to replace `release.yml` and `build-release.yml`
- Improved error handling for optional build configurations
- Added comprehensive documentation (this file)
- Standardized output formatting with colors
- Enhanced artifact management
- Fixed duplicate release workflow issue

### 2025-11-12 - Production Infrastructure
- Added `build-release.yml` with NSIS installer support
- Added `code-quality.yml` with security scanning
- Updated `build.yml` with matrix strategy
- Added `release.yml` for tag-based releases

### 2025-11-10 - Initial Setup
- Created basic build and release workflows
- Set up NuGet restore and MSBuild steps
- Configured artifact uploads

---

**Document Version:** 1.0
**Maintained By:** BruteGamingMacros Development Team
**Last Review:** 2025-11-17
