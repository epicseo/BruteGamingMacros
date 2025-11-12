# Frequently Asked Questions (FAQ)

## Installation & Setup

### Q: What are the system requirements?
**A:**
- Windows 10/11 (64-bit)
- .NET Framework 4.8.1
- Administrator privileges
- 50-100 MB disk space

See [INSTALL.md](INSTALL.md) for full requirements.

---

### Q: Do I need administrator rights?
**A:** Yes. Windows requires elevated privileges for:
- `ReadProcessMemory` - Reading game data
- `OpenProcess` - Accessing game process
- `SetWindowsHookEx` - Global keyboard hooks

Without admin rights, BGM cannot function.

---

### Q: Which .NET version do I need?
**A:** .NET Framework 4.8.1 (minimum)

**Check your version:**
```powershell
Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full' | Get-ItemProperty -Name Release
```
Result should be ≥ 533320

**Download:** https://dotnet.microsoft.com/download/dotnet-framework/net481

---

### Q: Can I install on Windows 7/8?
**A:** No. Windows 10 or later is required for .NET Framework 4.8.1 support.

---

### Q: Installer vs Portable - which should I choose?
**A:**

| Feature | Installer | Portable |
|---------|-----------|----------|
| Installation required | Yes | No |
| Admin rights to install | Yes | No |
| Start Menu integration | Yes | No |
| Automatic uninstall | Yes | Manual |
| USB drive compatible | No | Yes |
| Updates | In-place | Manual overwrite |

**Recommendation:** Installer for most users, Portable for USB/testing.

---

## Anti-Virus & Security

### Q: Why does my antivirus flag this as malware?
**A:** BGM uses legitimate Windows APIs that are also used by malware, causing false positives.

**APIs used:**
- ReadProcessMemory (game data reading)
- SetWindowsHookEx (hotkey detection)
- SendInput (keyboard simulation)

See [ANTIVIRUS.md](ANTIVIRUS.md) for detailed explanation.

---

### Q: Is this safe to use?
**A:** Yes. BGM is:
- ✅ 100% open source
- ✅ Community reviewed
- ✅ No network communication (except update checks)
- ✅ No data collection
- ✅ No code obfuscation
- ✅ Reproducible builds

You can verify every line of code on [GitHub](https://github.com/epicseo/BruteGamingMacros).

---

### Q: How do I whitelist BGM in Windows Defender?
**A:**
1. Open **Windows Security**
2. Go to **Virus & threat protection** → **Manage settings**
3. Under **Exclusions**, click **Add or remove exclusions**
4. Add the BGM folder

Full guide: [ANTIVIRUS.md](ANTIVIRUS.md)

---

### Q: What data does BGM collect?
**A:** **None.** BGM does not collect or transmit any user data.

**The only network activity is:**
- Checking for updates (GitHub API)
- Downloading updates (if user accepts)

No telemetry, no analytics, no tracking.

---

### Q: Can I verify the download is legitimate?
**A:** Yes. Every release includes SHA256 checksums:

1. Download `checksums.txt` from the release
2. Calculate hash of your download:
   ```powershell
   Get-FileHash BruteGamingMacros.exe -Algorithm SHA256
   ```
3. Compare with checksum in `checksums.txt`

Hashes must match exactly.

---

## Usage & Features

### Q: Which servers are supported?
**A:** Currently OsRO (Old School Ragnarok Online):
- MR (Medium Rate)
- HR (High Rate)
- LR (Low Rate)

Memory addresses are hardcoded for these servers. Other servers may not work correctly.

---

### Q: Can I use multiple profiles?
**A:** Yes. Unlimited profiles are supported.

**To create a profile:**
1. Click **Add Client**
2. Configure settings
3. Save with a name

**Profiles are stored in:**
```
%LOCALAPPDATA%\BruteGamingMacros\profiles\
```

---

### Q: What features are available?
**A:**
- **Auto-Pot:** Automatic HP/SP potion usage
- **Auto-Buff:** Skill-based buff automation
- **Auto-Skill:** Skill rotation automation
- **Spam Engine:** High-speed skill spamming
- **Hotkeys:** Global hotkey support
- **Multi-Client:** Control multiple game instances

See [README.md](../README.md) for full feature list.

---

### Q: How do I set up hotkeys?
**A:**
1. Select a profile
2. Go to hotkey settings
3. Click on hotkey field
4. Press desired key combination
5. Save

**Supported modifiers:** Ctrl, Alt, Shift
**Supported keys:** F1-F12, A-Z, 0-9

---

### Q: Can I use BGM with multiple game windows?
**A:** Yes. Add multiple profiles, each pointing to a different game process.

---

### Q: Does BGM work with all Ragnarok Online servers?
**A:** No. Memory addresses are specific to:
- OsRO MR/HR/LR (tested and supported)
- Other servers may have different memory layouts

**To add support for other servers:**
- Find memory addresses for your server
- Edit `AppConfig.cs` (requires rebuilding)
- OR wait for configurable address support (planned)

---

## Technical Issues

### Q: Application won't start
**A:** Common causes:

**1. Missing .NET Framework 4.8.1**
- Download from: https://dotnet.microsoft.com/download/dotnet-framework/net481

**2. Not running as administrator**
- Right-click → Run as administrator

**3. Antivirus quarantined files**
- Restore from quarantine
- Whitelist BGM folder

**4. Corrupted configuration**
```powershell
Remove-Item "$env:LOCALAPPDATA\BruteGamingMacros\config.json"
```

---

### Q: Can't detect game process
**A:** Checklist:
- ✅ Is Ragnarok Online running?
- ✅ Is BGM running as administrator?
- ✅ Is the game process visible in Task Manager?
- ✅ Is the game 32-bit? (64-bit not supported)
- ✅ Click **Refresh** on process dropdown

**Solution:**
1. Start game first
2. Then start BGM as admin
3. Select process from dropdown

---

### Q: Hotkeys not working
**A:** Possible causes:

**1. Conflicting applications**
- AutoHotkey, macro recorders, Discord overlay
- Close conflicting apps or change BGM hotkeys

**2. Not running as administrator**
- Global keyboard hooks require admin rights

**3. Game in focus**
- Some games block global hotkeys
- Try windowed mode

---

### Q: Memory reading not working
**A:** Troubleshooting:

**1. Verify server mode matches**
- MR/HR/LR must match your server

**2. Check process architecture**
```powershell
Get-Process ragexe | Select-Object ProcessName, @{Name="Architecture";Expression={if ([System.Environment]::Is64BitProcess) {"64-bit"} else {"32-bit"}}}
```
BGM only supports 32-bit game clients.

**3. Game updated (memory addresses changed)**
- Wait for BGM update with new addresses
- OR manually update addresses (advanced)

---

### Q: Application crashes frequently
**A:** Steps to diagnose:

**1. Check crash logs**
```powershell
Get-Content "$env:LOCALAPPDATA\BruteGamingMacros\Logs\app-*.log" | Select-Object -Last 100
```

**2. Run in debug mode**
```batch
BruteGamingMacros.exe --debug
```

**3. Report issue on GitHub**
- Include crash logs
- Describe what you were doing
- Windows version
- .NET version

**Create issue:** https://github.com/epicseo/BruteGamingMacros/issues

---

### Q: High CPU/Memory usage
**A:** Expected usage:
- **CPU:** 5-15% (depending on features enabled)
- **Memory:** 50-150 MB (depends on cache size)

**If higher:**
1. Disable unused features
2. Reduce polling frequency
3. Clear memory cache (restart BGM)
4. Check for memory leaks (long-running sessions)

**To monitor:**
```powershell
Get-Process BruteGamingMacros | Select-Object CPU, WorkingSet
```

---

### Q: Updates failing
**A:** Solutions:

**1. Firewall blocking**
- Allow BGM through Windows Firewall
- Check corporate/antivirus firewall

**2. GitHub API rate limiting**
- Wait 1 hour and try again

**3. Manual update**
- Download from [Releases](https://github.com/epicseo/BruteGamingMacros/releases)
- Install/extract manually

---

## Game Server & Bans

### Q: Will I get banned for using this?
**A:** **Depends on your server's policy.**

**Important:**
- BGM is designed for **private/offline servers**
- Check your server's Terms of Service
- Some servers allow automation, others don't
- **Use at your own risk**

**We are NOT responsible for:**
- Account bans
- Server violations
- Loss of progress

---

### Q: Is BGM detectable by game anti-cheat?
**A:** Yes, potentially.

**How it can be detected:**
- Memory reading patterns
- Input timing analysis
- Process enumeration
- Keyboard hook detection

**Recommendation:**
- Only use on servers that allow automation
- Use conservative speeds
- Don't automate 24/7
- Follow server rules

---

### Q: Can I use this on official servers?
**A:** **NOT RECOMMENDED.**

Official servers typically:
- Have anti-cheat systems
- Prohibit third-party tools
- Ban accounts using automation
- Monitor suspicious activity

**Use BGM only on:**
- Private servers that allow it
- Offline single-player
- Test/development environments

---

### Q: How to avoid detection?
**A:** **We do NOT condone violating server rules.**

If you have permission to automate:
- Use human-like delays
- Randomize action timing
- Don't run 24/7
- Take breaks
- Avoid obvious patterns

---

## Advanced Usage

### Q: Can I run BGM from a USB drive?
**A:** Yes, use the portable version.

**Note:** User data is still stored in `%LOCALAPPDATA%` on the host PC.

---

### Q: Can I use BGM over Remote Desktop?
**A:** Partially. Limitations:
- Keyboard hooks may not work properly
- SendInput may be blocked
- Performance may be degraded

**Workarounds:**
- Use local installation
- Enable input passthrough in RDP settings

---

### Q: Can I automate BGM itself (scripting)?
**A:** Currently no. Planned for future releases:
- Command-line control
- RESTful API
- Profile import/export
- Scripting support

---

### Q: Can I contribute to development?
**A:** Yes! BGM is open source.

**Ways to contribute:**
1. Report bugs on GitHub Issues
2. Submit pull requests
3. Add support for new servers
4. Improve documentation
5. Test pre-release versions

See [CONTRIBUTING.md](CONTRIBUTING.md)

---

### Q: How do I find memory addresses for other servers?
**A:** Advanced topic. Tools needed:
- Cheat Engine
- OllyDbg or x64dbg
- Process Hacker

**General steps:**
1. Open game in Cheat Engine
2. Search for known values (current HP, max HP, etc.)
3. Repeat searches until unique address found
4. Calculate offset from base address
5. Update `AppConfig.cs`

**Tutorial:** (link to guide, if available)

---

### Q: Can I modify the source code for personal use?
**A:** Yes. BGM is MIT licensed.

**You can:**
- ✅ Modify for personal use
- ✅ Fork the repository
- ✅ Distribute modified versions
- ✅ Use in commercial projects

**You must:**
- ✅ Include original license
- ✅ State changes made
- ✅ Not hold authors liable

See [LICENSE](../LICENSE)

---

## Troubleshooting Checklist

### Before Reporting an Issue

Run through this checklist:

- [ ] Windows 10/11 (64-bit)?
- [ ] .NET Framework 4.8.1 installed?
- [ ] Running as administrator?
- [ ] Antivirus exclusion added?
- [ ] Game process is 32-bit?
- [ ] Correct server mode selected?
- [ ] Logs checked for errors?
- [ ] Latest BGM version installed?
- [ ] Configuration file not corrupted?
- [ ] No conflicting applications?

**If all checked and still not working:**
- Create GitHub issue with details
- Include logs, screenshots, error messages

---

## Getting More Help

### Resources
- **Documentation:** `/docs` folder
- **GitHub Repository:** https://github.com/epicseo/BruteGamingMacros
- **Issue Tracker:** https://github.com/epicseo/BruteGamingMacros/issues
- **Releases:** https://github.com/epicseo/BruteGamingMacros/releases

### Reporting Issues
**Include:**
1. BGM version
2. Windows version (`winver`)
3. .NET version (PowerShell command from above)
4. Steps to reproduce
5. Expected vs actual behavior
6. Logs from `%LOCALAPPDATA%\BruteGamingMacros\Logs`
7. Screenshots (if applicable)

**Create issue:** https://github.com/epicseo/BruteGamingMacros/issues/new

---

### Community
- **GitHub Discussions:** (if enabled)
- **Discord:** (if available)
- **Reddit:** (if applicable)

---

**Last Updated:** 2025-11-12
**Version:** 2.0.1

*Can't find your question? [Open an issue](https://github.com/epicseo/BruteGamingMacros/issues/new) or check [INSTALL.md](INSTALL.md)*
