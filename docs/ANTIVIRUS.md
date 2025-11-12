# Anti-Virus and Windows Defender Guide

## Why is Brute Gaming Macros flagged by antivirus software?

Brute Gaming Macros uses **legitimate Windows APIs** for game automation that are also commonly used by malware, causing false positives from heuristic detection systems.

### APIs Used (and Why They're Flagged)

| API Function | Purpose in BGM | Why AV Flags It | Legitimacy |
|--------------|----------------|-----------------|------------|
| **ReadProcessMemory** | Reading game data (HP, mana, buffs) | Used by game cheats and info stealers | ✅ Same API used by debuggers, monitoring tools |
| **WriteProcessMemory** | Reserved for future features | Code injection technique | ✅ Used by development tools, accessibility software |
| **OpenProcess** | Getting handle to game process | Process manipulation | ✅ Used by task managers, system utilities |
| **SetWindowsHookEx** | Detecting hotkey combinations | Keylogger signature | ✅ Used by productivity tools, macro recorders |
| **SendInput** | Simulating keyboard/mouse input | Automation/botting | ✅ Used by remote desktop, accessibility tools |

**These are the SAME APIs used by:**
- Visual Studio Debugger
- Process Explorer (Sysinternals)
- ShareX (screenshot tool)
- AutoHotkey
- Remote Desktop applications
- Accessibility software for disabled users

## Is Brute Gaming Macros safe?

**YES.** Here's why you can trust it:

### 🔓 100% Open Source
- **Every line of code is visible** on GitHub
- Anyone can audit the source code
- No hidden functionality
- No obfuscation or packing

### 🔍 Community Reviewed
- Public issue tracker
- Active community contributions
- Transparent development process
- All changes are version controlled

### 📦 Reproducible Builds
- You can build from source yourself
- Compare your build's SHA256 hash with official releases
- Instructions provided in `/docs/BUILD.md`

### 🛡️ Security Best Practices
- No network communication (except update checks)
- No data collection or telemetry
- No auto-execution on startup
- Requires explicit user action

## How to whitelist in Windows Defender

### Method 1: Exclude the Installation Folder (Recommended)

1. Press **Win + I** to open Windows Settings
2. Go to **Privacy & Security** → **Windows Security**
3. Click **Virus & threat protection**
4. Under "Virus & threat protection settings," click **Manage settings**
5. Scroll down to **Exclusions**
6. Click **Add or remove exclusions**
7. Click **Add an exclusion** → **Folder**
8. Navigate to your BruteGamingMacros installation folder:
   - Default installer: `C:\Program Files\BruteGamingMacros`
   - Portable: Wherever you extracted it
9. Click **Select Folder**

### Method 2: Exclude the Executable

Follow steps 1-7 above, but:
- Choose **File** instead of **Folder**
- Navigate to `BruteGamingMacros.exe`
- Click **Open**

### Method 3: Allow the file when Windows Defender blocks it

When Windows Defender blocks the file:
1. Click on the notification
2. Click **Actions**
3. Select **Allow on device**
4. Confirm by clicking **Yes**

## Other Antivirus Software

### Avast / AVG
1. Open Avast/AVG
2. Go to **Menu** → **Settings**
3. Select **General** → **Exceptions**
4. Click **Add Exception**
5. Browse to your BruteGamingMacros folder
6. Click **Add Exception**

### Kaspersky
1. Open Kaspersky
2. Click **Settings** (gear icon)
3. Go to **Additional** → **Threats and Exclusions**
4. Click **Specify Trusted Applications**
5. Click **Add**
6. Browse to `BruteGamingMacros.exe`
7. Click **Add**

### Norton
1. Open Norton
2. Click **Settings**
3. Select **Antivirus**
4. Click **Scans and Risks** tab
5. Under **Exclusions / Low Risks**, click **Configure**
6. Click **Add** next to "Items to Exclude from Scans"
7. Browse to your BruteGamingMacros folder
8. Click **OK**

### Bitdefender
1. Open Bitdefender
2. Go to **Protection** → **Antivirus**
3. Click **Settings**
4. Select **Manage Exceptions**
5. Click **Add an Exception**
6. Enter the file path or browse to it
7. Click **Save**

### McAfee
1. Open McAfee
2. Click **PC Security**
3. Click **Real-Time Scanning**
4. Click **Excluded Files**
5. Click **Add File**
6. Browse to `BruteGamingMacros.exe`
7. Click **Add**

### Malwarebytes
1. Open Malwarebytes
2. Go to **Settings** → **Exclusions**
3. Click **Add Exclusion**
4. Select **Exclude a File or Folder**
5. Browse to your BruteGamingMacros folder
6. Click **Exclude**

## VirusTotal Results

You can verify the application's safety by uploading it to [VirusTotal](https://www.virustotal.com):

1. Go to https://www.virustotal.com
2. Click **Choose file**
3. Select `BruteGamingMacros.exe`
4. Wait for analysis

### Expected Results:
- **Detection ratio:** 0-5 out of 70+ engines
- **Flagged by:** Usually smaller, less-known engines using aggressive heuristics
- **NOT flagged by:** Major vendors (Microsoft, Kaspersky, Bitdefender, Norton, etc.)

### Verify SHA256 Hash:
Compare the hash shown on VirusTotal with the official hash in our [release notes](https://github.com/epicseo/BruteGamingMacros/releases):

```
1. Check VirusTotal's "Details" tab for SHA-256 hash
2. Compare with checksums.txt in the GitHub release
3. Hashes must match exactly (all 64 characters)
```

## Building Trust Over Time

### Code Signing Certificate (In Progress)
We are working on obtaining an **Extended Validation (EV) Code Signing Certificate**:
- **Cost:** ~$300-400/year
- **Benefit:** Immediate Windows SmartScreen reputation
- **Timeline:** Expected in Q1 2025

### SmartScreen Reputation
For standard code signing certificates:
- **Reputation builds over time** with download volume
- **Timeline:** 3-6 months for established reputation
- **You may see:** "Windows protected your PC" warning initially
- **To proceed:** Click "More info" → "Run anyway"

### Community Trust Indicators
- ⭐ GitHub stars and forks
- 📝 Issue resolution responsiveness
- 🔄 Regular updates and maintenance
- 👥 Active community discussions
- 📊 Transparent version history

## Submitting to Antivirus Vendors

We regularly submit new releases to major antivirus vendors for whitelisting:

### Already Submitted To:
- ✅ Microsoft Defender (via Microsoft Security Intelligence)
- ✅ VirusTotal (automatically shared with 70+ engines)

### Submission URLs (if you want to help):
- **Microsoft Defender:** https://www.microsoft.com/en-us/wdsi/filesubmission
- **Norton:** https://submit.norton.com
- **McAfee:** https://www.mcafee.com/enterprise/en-us/threat-center/submit-sample.html
- **Kaspersky:** https://support.kaspersky.com/viruses/disinfection/16448
- **Avast:** https://www.avast.com/false-positive-file-form.php
- **Bitdefender:** https://www.bitdefender.com/consumer/support/answer/29358/

### How to Submit:
1. Download the latest release
2. Fill out the vendor's false positive form
3. Attach `BruteGamingMacros.exe`
4. Explain: "Legitimate game automation tool using Windows APIs"
5. Provide GitHub repository link

## Technical Explanation (For Security Researchers)

### Why These APIs Are Necessary

**ReadProcessMemory:**
```csharp
// Reading player stats from game memory
uint currentHP = ReadUInt32(baseAddress + 0x470);
uint maxHP = ReadUInt32(baseAddress + 0x474);
```
- **Purpose:** Non-invasive reading of game state
- **No modification** of game code or data
- **Read-only** operation

**SendInput:**
```csharp
// Simulating keyboard press for skill activation
SendInput(1, &input, Marshal.SizeOf(input));
```
- **Purpose:** Hardware-level input simulation
- **Safer than** message-based input (PostMessage)
- **Detectable** by games (not a stealth bot)

**SetWindowsHookEx:**
```csharp
// Global hotkey detection (e.g., F1 to toggle automation)
SetWindowsHookEx(WH_KEYBOARD_LL, callback, hInstance, 0);
```
- **Purpose:** Hotkey registration without keeping app in foreground
- **Alternative:** RegisterHotKey (less flexible)

### What We DON'T Do

❌ **No code injection** - We don't inject DLLs or modify game code
❌ **No rootkit techniques** - No kernel drivers or system hooks
❌ **No network sniffing** - No packet interception or manipulation
❌ **No data exfiltration** - No transmission of user data
❌ **No persistence** - No auto-start or system modification
❌ **No obfuscation** - Code is readable and auditable

### Behavioral Analysis

**What antivirus behavioral analysis sees:**
1. ✅ Application requests admin privileges (for memory access)
2. ✅ Opens handle to another process (game)
3. ✅ Reads memory from that process
4. ✅ Installs keyboard hook
5. ✅ Sends simulated input

**Why this is legitimate:**
- All operations are **user-initiated**
- Target process is **user-selected** (not random)
- No **persistence mechanisms**
- No **network communication** (except update checks)
- Operations are **visible** (not hidden)

## Comparison with Known Malware

| Behavior | BGM | Malware |
|----------|-----|---------|
| Open Source | ✅ Yes | ❌ No |
| Code Signing | 🟡 In Progress | ❌ No (usually) |
| Requests Admin | ✅ Explicit | ⚠️ Often hidden |
| Network Activity | ✅ Update checks only | ❌ C&C communication |
| Persistence | ❌ None | ✅ Auto-start, registry |
| Obfuscation | ❌ None | ✅ Heavy packing/encryption |
| Keylogging | ❌ Hotkeys only | ✅ All keystrokes |
| Data Theft | ❌ None | ✅ Credentials, files |

## FAQ

### Q: Will I get banned from games for using this?
**A:** Depends on the game's Terms of Service. BGM is designed for **offline/private servers only**. Check your server's automation policy.

### Q: Does BGM contain any viruses or malware?
**A:** No. It's 100% open source. You can verify every line of code on GitHub.

### Q: Why not just submit to Microsoft for whitelisting?
**A:** We do! But it takes time (weeks to months) for them to process submissions and build reputation.

### Q: Can I use BGM without antivirus warnings?
**A:** Yes, with an **EV code signing certificate** (in progress). Standard certificates take 3-6 months to build reputation.

### Q: Is there any telemetry or data collection?
**A:** No. BGM does not collect or transmit any user data. You can verify this in the source code.

### Q: Why does Task Manager show high memory usage?
**A:** BGM caches game memory to improve performance. This is normal and by design.

### Q: Can I run BGM without admin rights?
**A:** No. Memory reading APIs require elevated privileges. This is a Windows security requirement.

## Still Concerned?

### Build from Source
The ultimate verification:

```powershell
# 1. Clone the repository
git clone https://github.com/epicseo/BruteGamingMacros.git
cd BruteGamingMacros

# 2. Review the source code
# (examine all .cs files)

# 3. Build it yourself
.\build\build.ps1 -Configuration Release

# 4. Your build = your trust
```

### Contact Us
- **GitHub Issues:** https://github.com/epicseo/BruteGamingMacros/issues
- **Security Concerns:** Open a GitHub issue with [SECURITY] prefix
- **Email:** (if available) security@yourdomain.com

## Resources

- **GitHub Repository:** https://github.com/epicseo/BruteGamingMacros
- **Build Instructions:** /docs/BUILD.md
- **Architecture:** /docs/ARCHITECTURE.md
- **FAQ:** /docs/FAQ.md

---

**Last Updated:** 2025-11-12
**Version:** 2.0.1

*This document is maintained by the BGM development team and is updated with each release.*
