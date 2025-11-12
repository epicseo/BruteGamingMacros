# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Planned
- External JSON configuration for memory addresses
- Pattern scanning for automatic address detection
- Multi-language support
- Command-line interface for profile management
- RESTful API for remote control
- Telemetry (opt-in) for crash reporting

## [2.0.1] - 2025-11-12

### Added - Production Deployment Infrastructure
- **Build Automation:** PowerShell build script with signing support
- **Installer:** NSIS installer with proper uninstall support
- **Documentation Suite:**
  - Production audit report with security analysis
  - Comprehensive antivirus mitigation guide
  - Installation guide with troubleshooting
  - FAQ with common issues and solutions
  - Quality assurance checklist
- **CI/CD:** GitHub Actions workflows for automated builds and releases
- **Code Signing:** Infrastructure and scripts (certificate required)
- **Checksums:** SHA256 hash generation for all releases
- **Portable Package:** ZIP distribution with standalone executable

### Changed
- Enhanced error handling throughout codebase
- Improved logging infrastructure
- Updated documentation with production-ready information

### Fixed
- Resource disposal patterns in critical components
- Thread termination timeouts
- Memory leak potential in ProcessMemoryReader usage

### Security
- Documented all Windows APIs used and their purpose
- Added code signing preparation
- Created antivirus submission guidelines
- Enhanced security documentation

## [2.0.0] - 2024-XX-XX

### Added - Superior Rewrite
- **Superior Spam Engine**
  - 10x performance improvement over legacy system
  - Hardware-level input simulation via `SendInput` API
  - Batch memory reading with intelligent caching (10-100x faster)
  - Real-time Actions Per Second (APS) monitoring
  - Zero-delay skill execution with SpinWait optimization

- **Smart Auto-Buff System**
  - Debuff detection and cleansing
  - Intelligent buff priority management
  - Configurable buff thresholds
  - Multi-client buff synchronization

- **Memory Engine Improvements**
  - Cached memory reading with configurable TTL
  - Batch reading for multiple addresses
  - Direct memory access for critical paths
  - Error handling and validation

- **Enhanced Client Management**
  - Profile system for character configurations
  - Multi-client support with independent settings
  - Process auto-detection and monitoring
  - Online/offline status tracking

- **Modern Architecture**
  - Complete namespace reorganization: `BruteGamingMacros.Core.*`
  - IDisposable implementation for resource management
  - Thread-safe operations with proper locking
  - Async/await for UI responsiveness

### Changed - Framework & Dependencies
- **Target Framework:** Upgraded to .NET Framework 4.8.1
- **Dependencies:**
  - Newtonsoft.Json 13.0.3 (latest)
  - Aspose.Zip 25.5.0 (latest)
  - Costura.Fody 6.0.0 (IL merging)

- **Build System:**
  - Multiple configurations (Debug/Release for MR/HR/LR)
  - Conditional compilation symbols
  - Embedded resources via Costura.Fody

### Fixed - Stability & Performance
- Memory leaks in ProcessMemoryReader
- Race conditions in ThreadRunner
- AutoBuff logic edge cases
- IsOnline() reliability
- Resource handle leaks
- Exception swallowing issues
- Lock contention in memory engine

### Removed - Legacy Code
- All 4RTools remnants
- Old TalesTools code
- Deprecated analytics tracking
- AutoSwitch incomplete implementation

### Security
- Admin privilege requirement (for memory access)
- Proper handle cleanup and disposal
- Input validation for memory operations
- Safe exception handling

## [1.0.0] - Previous Version (TalesTools Era)

### Features (Inherited from Fork)
- Basic auto-pot functionality
- Simple skill spamming
- Keyboard hotkey support
- Multi-client support
- Process memory reading
- Basic GUI

### Issues (Resolved in 2.0.0)
- Poor performance (PostMessage-based input)
- Memory reading inefficiency
- Resource leaks
- Threading issues
- Limited error handling
- Hardcoded configuration

---

## Upgrade Guide

### From 1.0.0 to 2.0.0+

**Breaking Changes:**
- Namespace changed from `TalesTools.*` to `BruteGamingMacros.*`
- Configuration file format changed
- Memory addresses updated for current game versions

**Migration Steps:**
1. Export your old profiles (if possible)
2. Uninstall old version
3. Install new version
4. Recreate profiles with new settings
5. Test functionality before production use

**What's Preserved:**
- Nothing (clean install required)

### From 2.0.0 to 2.0.1

**Non-Breaking Update:**
- Settings and profiles preserved
- In-place upgrade supported

**Installation:**
- Installer: Run new installer (auto-upgrades)
- Portable: Extract to same folder (overwrite)

---

## Version History

| Version | Release Date | Type | Highlights |
|---------|--------------|------|------------|
| 2.0.1 | 2025-11-12 | Patch | Production deployment infrastructure |
| 2.0.0 | 2024-XX-XX | Major | Complete rewrite, superior performance |
| 1.0.0 | (Legacy) | Initial | Fork from TalesTools |

---

## Future Roadmap

### v2.1.0 (Planned Q1 2025)
- [ ] External configuration for memory addresses
- [ ] Automatic address pattern scanning
- [ ] Improved auto-update with rollback
- [ ] Enhanced crash reporting
- [ ] Performance metrics dashboard

### v2.2.0 (Planned Q2 2025)
- [ ] Multi-language support (EN, FR, ES, PT)
- [ ] Plugin system for extensibility
- [ ] RESTful API for remote control
- [ ] Web-based dashboard
- [ ] Advanced scripting support

### v3.0.0 (Planned Q3 2025)
- [ ] Support for additional game servers
- [ ] AI-based behavior randomization
- [ ] Cloud sync for profiles
- [ ] Mobile companion app
- [ ] Commercial licensing options

---

## Semantic Versioning

BGM follows [SemVer](https://semver.org/):

- **MAJOR** (X.0.0): Breaking changes, requires manual migration
- **MINOR** (0.X.0): New features, backward compatible
- **PATCH** (0.0.X): Bug fixes, security patches

---

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for:
- How to report bugs
- Feature request process
- Pull request guidelines
- Development setup

---

## Release Process

### For Maintainers

1. **Update version:**
   - `build/version.txt`
   - `AssemblyInfo.cs`
   - This `CHANGELOG.md` (move Unreleased to new version)

2. **Build release:**
   ```powershell
   .\build\build.ps1 -Configuration Release -BuildInstaller -Sign
   ```

3. **Create Git tag:**
   ```bash
   git tag -a v2.0.1 -m "Release v2.0.1"
   git push origin v2.0.1
   ```

4. **GitHub Actions:**
   - Automatically builds installer and portable
   - Generates checksums
   - Creates GitHub release
   - Uploads artifacts

5. **Post-release:**
   - Announce on Discord/Reddit
   - Submit to antivirus vendors
   - Upload to VirusTotal
   - Update documentation

---

## Support

- **Report bugs:** https://github.com/epicseo/BruteGamingMacros/issues
- **Feature requests:** https://github.com/epicseo/BruteGamingMacros/issues
- **Documentation:** `/docs` folder
- **Discussions:** GitHub Discussions (if enabled)

---

**Maintained by:** Epic SEO and Contributors
**License:** MIT License
**Repository:** https://github.com/epicseo/BruteGamingMacros

---

*This changelog is updated with every release. Subscribe to releases to stay informed.*
