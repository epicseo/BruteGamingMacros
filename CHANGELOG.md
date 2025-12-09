# Changelog

All notable changes to Brute Gaming Macros will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Planned
- OsRO LR (Revo) server support (pending memory address configuration)
- Unit test framework setup
- API documentation generation

---

## [2.1.0] - 2025-12-09

### Added
- GitHub Actions CI/CD pipeline for automated builds
- Automated release workflow with portable ZIP and installer artifacts
- Code quality workflow with security scanning
- `.editorconfig` for consistent code formatting
- Comprehensive 360-degree production readiness audit
- SHA256 checksums for all release artifacts
- Structured logging with Serilog (daily rotation, 7-day retention)
- CrashReporter for detailed crash dumps and user-friendly error dialogs
- ProductionLogger for production-grade logging

### Changed
- **BREAKING**: OsRO LR (Revo) build now throws `NotSupportedException` instead of silently failing
- Updated version numbering to be consistent across all files
- Improved README with accurate installation instructions
- Updated installer copyright to 2025
- Updated Aspose.Zip from v22.10.0 to v25.5.0 (security & features)
- Updated Newtonsoft.Json from v13.0.1 to v13.0.3 (security patches)
- Updated Costura.Fody from v5.7.0 to v6.0.0
- Updated Fody from v6.5.5 to v6.8.0
- Updated NETStandard.Library from v1.6.1 to v2.0.3
- Updated Microsoft.NETCore.Platforms from v1.1.0 to v7.0.4

### Fixed
- Version mismatch across AppConfig.cs, AssemblyInfo.cs, README.md, and installer
- Broken image path in README.md (XX -> applogo.png)
- LR build silently failing with all-zero memory addresses (now fails explicitly)

### Security
- Fixed known vulnerabilities in Newtonsoft.Json
- Updated all dependencies to latest stable versions
- Added sensitive data scanning in CI/CD pipeline
- Added dependency vulnerability checking

### Documentation
- Updated README with v2.1.0 references
- Added clear note about LR server support status
- Comprehensive CHANGELOG following Keep a Changelog format

---

## [2.0.0] - 2025-10-21

### Added
- Complete rebrand from 4RTools to Brute Gaming Macros
- SuperiorInputEngine with hardware-level input simulation
  - Ultra mode: 1ms delay (1000+ APS)
  - Turbo mode: 5ms delay (200 APS)
  - Standard mode: 10ms delay (100 APS)
- SuperiorMemoryEngine with batch operations and caching
  - 10-100x faster batch memory reads
  - Smart caching system with TTL
  - 95%+ reduction in P/Invoke calls
- SuperiorSkillSpammer for high-performance skill execution
- Thread-safe Profile singleton pattern
- Comprehensive debug logging system
- Real-time Actions Per Second (APS) tracking
- Performance benchmarking tools
- Sub-millisecond precision timing with SpinWait
- Smart Auto-Buff (detects Quagmire and Decrease AGI)
- Weight-based auto-disable (50%/90% overweight detection)
- Support for OsRO MR, HR, and LR servers
- Padawan (Jedi/Sith) skill support with icons
- Clean namespace architecture (BruteGamingMacros.Core.*)
- System tray integration
- Debug log window for development
- Profile management (create, delete, rename, copy)
- Auto-potion system with HP/SP thresholds
- Skill-based and item-based auto-buffing
- Macro chains for complex ability sequences
- ATK/DEF equipment switching
- Skill cooldown timer display
- Item transfer helper
- Debuff recovery system
- Status effect tracking and visualization

### Changed
- Migrated to .NET Framework 4.8.1
- Reorganized all namespaces from _4RTools to BruteGamingMacros.Core
- Updated all resource paths (Icons, Sounds)
- Modernized project structure
- Improved error handling throughout
- Enhanced UI with light theme
- Optimized memory reading patterns
- Improved thread safety across components

### Fixed
- AutoBuff logic race conditions
- IsOnline() method reliability
- Profile loading thread safety issues
- Memory leak in ProcessMemoryReader (added IDisposable)
- Resource path inconsistencies in Designer files
- AutoPatcher executable name references
- Legacy profile migration from "Custom" to "TransferHelper"
- Various null reference exceptions

### Security
- Implemented proper IDisposable pattern for unmanaged resources
- Added thread-safe locking mechanisms
- Proper exception handling in critical paths
- Administrator privilege requirement properly declared

### Documentation
- README.md with features and installation
- DEEP_AUDIT_REPORT.md with code quality findings
- FEATURE_VERIFICATION.md for testing checklist
- TRANSFORMATION_*.md files documenting rebrand process
- BACKUP_SYSTEM.md and RESTORE_INSTRUCTIONS.md
- Inline code documentation and XML comments

## [1.x] - Legacy (4RTools)

Previous versions under the 4RTools name. See original repository for history.

---

## Version Number Guidelines

- **MAJOR** version: Incompatible API changes or major rewrites
- **MINOR** version: New features in a backwards-compatible manner
- **PATCH** version: Backwards-compatible bug fixes

## Release Types

- **[Unreleased]**: Changes in development, not yet released
- **[X.Y.Z]**: Released version with date
- **[X.Y.Z-alpha.N]**: Pre-release alpha version
- **[X.Y.Z-beta.N]**: Pre-release beta version
- **[X.Y.Z-rc.N]**: Release candidate

## Links

[Unreleased]: https://github.com/epicseo/BruteGamingMacros/compare/v2.1.0...HEAD
[2.1.0]: https://github.com/epicseo/BruteGamingMacros/releases/tag/v2.1.0
[2.0.0]: https://github.com/epicseo/BruteGamingMacros/releases/tag/v2.0.0
