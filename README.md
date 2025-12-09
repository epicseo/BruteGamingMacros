<p align="center">
  <img src="assets/image/logos/applogo.png" alt="Brute Gaming Macros" width="256">
</p>

<p align="center">
  <a href="https://github.com/epicseo/BruteGamingMacros/releases">
    <img src="https://img.shields.io/github/v/release/epicseo/BruteGamingMacros?style=for-the-badge&logo=github&color=blue" alt="Latest Release">
  </a>
  <a href="https://github.com/epicseo/BruteGamingMacros/releases">
    <img src="https://img.shields.io/github/downloads/epicseo/BruteGamingMacros/total?style=for-the-badge&logo=github&color=green" alt="Downloads">
  </a>
  <a href="https://github.com/epicseo/BruteGamingMacros/blob/main/LICENSE">
    <img src="https://img.shields.io/github/license/epicseo/BruteGamingMacros?style=for-the-badge&color=orange" alt="License">
  </a>
  <a href="https://github.com/epicseo/BruteGamingMacros/actions">
    <img src="https://img.shields.io/github/actions/workflow/status/epicseo/BruteGamingMacros/build-release.yml?style=for-the-badge&logo=githubactions&label=Build" alt="Build Status">
  </a>
</p>

<p align="center">
  <a href="https://dotnet.microsoft.com/download/dotnet-framework/net481">
    <img src="https://img.shields.io/badge/.NET%20Framework-4.8.1-blue?style=for-the-badge&logo=.net" alt=".NET 4.8.1">
  </a>
  <a href="https://github.com/epicseo/BruteGamingMacros/issues">
    <img src="https://img.shields.io/github/issues/epicseo/BruteGamingMacros?style=for-the-badge&logo=github&color=red" alt="Issues">
  </a>
  <a href="https://github.com/epicseo/BruteGamingMacros/stargazers">
    <img src="https://img.shields.io/github/stars/epicseo/BruteGamingMacros?style=for-the-badge&logo=github&color=yellow" alt="Stars">
  </a>
</p>

# Brute Gaming Macros v2.1.0
**Production-Ready Gaming Automation Suite**

## 🔥 Overview
**Brute Gaming Macros** is a production-ready, high-performance gaming macro tool - the evolution of automation for Ragnarok Online. Originally inspired by the community tools TalesTools and related projects, this represents a complete reimagining focused on maximum performance, reliability, and professional deployment.

**Version 2.1.0** brings production deployment infrastructure, comprehensive documentation, automated CI/CD, and professional-grade features while maintaining full compatibility with [OsRO MR](https://osro.mr/) and [OsRO HR](https://osro.gg/).

> **Note:** OsRO LR (Revo) support is planned but not yet available. Memory addresses need to be configured.

> 🚀 **Production Ready:** Professional installer, automated builds, crash reporting, structured logging, and comprehensive documentation.

## ⚡ What's New in v2.1.0

### 🏗️ Production Infrastructure
- **Professional Installer:** NSIS-based installer with .NET Framework detection
- **Automated CI/CD:** GitHub Actions for automated builds and releases
- **Comprehensive Documentation:** Installation guide, FAQ, antivirus guide, and more
- **Build Automation:** PowerShell scripts for reproducible builds
- **Code Quality:** Automated testing and security scanning

### 📊 Production Features
- **Structured Logging:** Serilog-based logging with daily rotation
- **Crash Reporting:** Detailed crash dumps for debugging
- **Update Checker:** Secure GitHub release integration with SHA256 verification
- **Critical Fixes:** Thread termination improvements and resource leak fixes

### 📚 Documentation Suite
- [Installation Guide](docs/INSTALL.md) - Step-by-step setup instructions
- [FAQ](docs/FAQ.md) - Common questions and troubleshooting
- [Antivirus Guide](docs/ANTIVIRUS.md) - Whitelisting and security explanations
- [Contributing](docs/CONTRIBUTING.md) - Development guidelines
- [Changelog](docs/CHANGELOG.md) - Version history and roadmap

## ⚡ What Was New in v2.0.0

### Superior Performance
- **10x Faster Input:** Ultra-fast spam engine with 1000+ actions/second capability
- **Hardware Simulation:** SendInput API for more reliable input injection
- **Optimized Memory Reading:** 10-100x faster batch memory reads with caching
- **Sub-millisecond Latency:** <1ms input response time

### Modern Architecture
- **Professional Rebrand:** Brute Gaming Macros - premium gaming tool identity
- **Clean Codebase:** All namespaces reorganized (BruteGamingMacros.Core.*)
- **Better Performance:** Advanced threading and memory optimization
- **Future-Ready:** Plugin architecture and scripting engine foundation

### Enhanced Reliability
- **Thread Safety:** Fixed all race conditions and concurrency issues
- **Memory Management:** Proper resource disposal, zero memory leaks
- **Bug Fixes:** Critical fixes including AutoBuff logic and IsOnline() method
- **Error Handling:** Comprehensive exception handling throughout

## 🎯 Key Features

### Core Features
- **Language & Adaptation:** English interface aligned with OsRO's status set
- **Weight-Based Auto-Disable:** Disable when overweight (50%/90%) with Alt-# macro trigger
- **Expanded Skill Support:** Padawan (Jedi/Sith) skills with icons
- **Pre-Renewal Focus:** Optimized for pre-renewal mechanics
- **Single Toggle:** Simplified macro control
- **Light Theme:** UI matching RO client aesthetic
- **.NET 4.8.1:** Modern framework support

### Enhanced in v2.0
- **Superior Spam Engine:** Choose between Ultra (1ms), Turbo (5ms), or Standard (10ms) modes
- **Smart Auto-Buff:** Detects Quagmire and Decrease AGI to avoid wasting buffs
- **Performance Monitoring:** Real-time Actions Per Second (APS) tracking
- **Optimized Memory:** Batch reading for 95%+ reduction in memory calls

## 📦 Installation

### Option 1: Installer (Recommended)
1. Download `BruteGamingMacros-Setup-v2.1.0.exe` from [latest release](https://github.com/epicseo/BruteGamingMacros/releases/latest)
2. Run the installer (requires Administrator)
3. Follow the setup wizard
4. Launch from Start Menu or Desktop shortcut

### Option 2: Portable
1. Download `BruteGamingMacros-v2.1.0-portable.zip` from [releases](https://github.com/epicseo/BruteGamingMacros/releases)
2. Extract to any folder
3. Run `BruteGamingMacros.exe` as Administrator

### System Requirements
- **OS:** Windows 10/11 (64-bit)
- **Framework:** .NET Framework 4.8.1 or higher
- **Privileges:** Administrator (required for memory reading)
- **Game:** Ragnarok Online (OsRO MR/HR)
- **Disk Space:** 50-100 MB

> **⚠️ Antivirus Warning:** This application uses legitimate Windows APIs (ReadProcessMemory, SendInput) that may trigger false positives.
> **Solution:** See our comprehensive [Antivirus Guide](docs/ANTIVIRUS.md) for whitelisting instructions.

For detailed installation instructions, see [INSTALL.md](docs/INSTALL.md).

## 📸 Screenshots

![4rt_screens_1](https://github.com/user-attachments/assets/2cabbec4-1072-4dbf-b4bb-8681663394ee)
![4rt_screens_2](https://github.com/user-attachments/assets/4c7493a5-8ca7-441f-88e7-c1bf3b73e9f1)

---

## 🛠️ Building from Source

### Prerequisites
- Visual Studio 2019+ or Visual Studio Code
- .NET Framework 4.8.1 SDK
- Git

### Build Steps
```powershell
# Clone the repository
git clone https://github.com/epicseo/BruteGamingMacros.git
cd BruteGamingMacros

# Restore NuGet packages
nuget restore BruteGamingMacros.sln

# Build using PowerShell script
.\build\build.ps1 -Configuration Release

# Or build using MSBuild
msbuild BruteGamingMacros.sln /p:Configuration=Release
```

For detailed build instructions, see [docs/CONTRIBUTING.md](docs/CONTRIBUTING.md)

---

## 🤝 Contributing

We welcome contributions! Here's how you can help:

1. **Report Bugs:** [Open an issue](https://github.com/epicseo/BruteGamingMacros/issues/new?template=bug_report.md)
2. **Request Features:** [Submit a feature request](https://github.com/epicseo/BruteGamingMacros/issues/new?template=feature_request.md)
3. **Submit Code:** Fork the repo, make changes, and submit a pull request
4. **Improve Documentation:** Documentation improvements are always welcome

Please read our [Contributing Guidelines](docs/CONTRIBUTING.md) before submitting.

---

## 📋 Support

### Need Help?
- **📖 Documentation:** Check [docs/](docs/) folder for comprehensive guides
- **❓ FAQ:** See [docs/FAQ.md](docs/FAQ.md) for common questions
- **🐛 Bug Report:** [Open an issue](https://github.com/epicseo/BruteGamingMacros/issues)
- **💬 Discussions:** Use GitHub Discussions (if enabled)

### Troubleshooting
1. **Installation Issues:** [INSTALL.md](docs/INSTALL.md)
2. **Antivirus Blocking:** [ANTIVIRUS.md](docs/ANTIVIRUS.md)
3. **Common Errors:** [FAQ.md](docs/FAQ.md)
4. **Build Problems:** [CONTRIBUTING.md](docs/CONTRIBUTING.md)

---

## 📜 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

### What This Means
- ✅ Free to use for personal and commercial purposes
- ✅ Modify and distribute as you wish
- ✅ No warranty or liability
- ✅ Must include original license and copyright notice

---

## ⚖️ Disclaimer

**Important:** This tool is designed for educational and private server use only.

- 🚫 **Not for official servers** - Using automation tools on official servers violates Terms of Service
- 🚫 **No responsibility for bans** - Use at your own risk
- ✅ **Private servers only** - Check your server's automation policy
- ✅ **Educational purposes** - Learn about Windows APIs and game automation

**We do not condone or support:**
- Cheating in online games
- Violating game Terms of Service
- Using this tool on official/commercial game servers

---

## 🙏 Acknowledgments

- **Original Inspiration:** TalesTools and community contributors
- **Community:** OsRO players and testers
- **Contributors:** Everyone who has contributed to this project
- **Libraries:** Serilog, Newtonsoft.Json, Aspose.Zip, Costura.Fody

---

## 🔗 Links

- **Repository:** [github.com/epicseo/BruteGamingMacros](https://github.com/epicseo/BruteGamingMacros)
- **Releases:** [Latest Release](https://github.com/epicseo/BruteGamingMacros/releases/latest)
- **Issues:** [Report a Bug](https://github.com/epicseo/BruteGamingMacros/issues)
- **Documentation:** [docs/](docs/)

---

<p align="center">
  Made with ❤️ for the Ragnarok Online community
</p>

<p align="center">
  <sub>If you find this project useful, please consider giving it a ⭐ on GitHub!</sub>
</p>