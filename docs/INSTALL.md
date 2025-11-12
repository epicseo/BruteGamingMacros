# Installation Guide

## System Requirements

### Minimum Requirements
- **Operating System:** Windows 10 (64-bit) or Windows 11
- **Framework:** .NET Framework 4.8.1
- **RAM:** 2 GB
- **Disk Space:** 50 MB
- **Privileges:** Administrator rights (for memory reading)

### Recommended Requirements
- **Operating System:** Windows 11 (64-bit)
- **RAM:** 4 GB or more
- **Disk Space:** 100 MB (for logs and profiles)
- **Display:** 1920x1080 or higher

### Supported Game Servers
- OsRO MR (Medium Rate)
- OsRO HR (High Rate)
- OsRO LR (Low Rate)

## Installation Methods

### Method 1: Installer (Recommended)

**Best for:** Most users, automatic setup, Start Menu integration

1. **Download the installer:**
   - Go to [Releases](https://github.com/epicseo/BruteGamingMacros/releases)
   - Download `BruteGamingMacros-Setup-v[VERSION].exe`

2. **Run the installer:**
   - Double-click the downloaded file
   - Windows may show "Windows protected your PC" warning:
     - Click **More info**
     - Click **Run anyway**
   - Accept UAC prompt (requires Administrator)

3. **Follow the setup wizard:**
   - Accept the license agreement
   - Choose installation directory (default: `C:\Program Files\BruteGamingMacros`)
   - Select components:
     - ✅ Brute Gaming Macros (required)
     - ⬜ Desktop Shortcut (optional)
     - ⬜ Start Menu Shortcuts (optional)
   - Click **Install**

4. **Launch the application:**
   - From Start Menu: **Brute Gaming Macros**
   - From Desktop: Double-click shortcut (if created)
   - From installation folder: Right-click `BruteGamingMacros.exe` → **Run as administrator**

### Method 2: Portable (No Installation)

**Best for:** Users who want to run from USB drive or don't have admin rights for installation

1. **Download the portable package:**
   - Go to [Releases](https://github.com/epicseo/BruteGamingMacros/releases)
   - Download `BruteGamingMacros-v[VERSION]-portable.zip`

2. **Extract the archive:**
   - Right-click the ZIP file
   - Select **Extract All...**
   - Choose a destination folder (e.g., `C:\BGM` or USB drive)
   - Click **Extract**

3. **Run the application:**
   - Navigate to the extracted folder
   - Right-click `BruteGamingMacros.exe`
   - Select **Run as administrator**

4. **Create a shortcut (optional):**
   - Right-click `BruteGamingMacros.exe`
   - Select **Create shortcut**
   - Drag shortcut to Desktop or taskbar

### Method 3: Build from Source

**Best for:** Developers, security-conscious users who want to verify the code

See [BUILD.md](BUILD.md) for detailed instructions.

## First-Time Setup

### 1. Run as Administrator

**Why?** Memory reading APIs require elevated privileges.

To always run as administrator:
1. Right-click `BruteGamingMacros.exe` or shortcut
2. Select **Properties**
3. Go to **Compatibility** tab
4. Check ✅ **Run this program as an administrator**
5. Click **OK**

### 2. Whitelist in Antivirus

**Important:** See [ANTIVIRUS.md](ANTIVIRUS.md) for detailed instructions.

Quick steps for Windows Defender:
1. Open **Windows Security**
2. Go to **Virus & threat protection**
3. Click **Manage settings**
4. Under **Exclusions**, click **Add or remove exclusions**
5. Add your BGM folder

### 3. Configure .NET Framework 4.8.1

The installer checks for .NET Framework 4.8.1 automatically.

**If not installed:**
1. Installer will prompt to download
2. OR manually download from: https://dotnet.microsoft.com/download/dotnet-framework/net481
3. Install and restart your computer

**To verify installed version:**
```powershell
# Run in PowerShell
Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full' | Get-ItemProperty -Name Release | Select-Object -ExpandProperty Release
```
- **Result should be ≥ 533320** (4.8.1)

### 4. Initial Application Setup

When you first launch BGM:

1. **Select Game Process:**
   - Click **Process** dropdown
   - Select your Ragnarok Online process (e.g., `ragexe.exe`)
   - If not visible, make sure RO is running

2. **Configure Server Mode:**
   - MR (Medium Rate)
   - HR (High Rate)
   - LR (Low Rate)
   - **Must match your server type** for correct memory offsets

3. **Set Up Profiles:**
   - Click **Add Client** to create a new profile
   - Name it after your character
   - Configure hotkeys and features

4. **Test Functionality:**
   - Enable **Auto-Pot** or **Auto-Buff**
   - Check if the application reads your character stats
   - Test hotkeys

## Troubleshooting

### Issue: "Application failed to start"

**Solution 1: Install .NET Framework 4.8.1**
```
Error: This application requires .NET Framework 4.8.1
```
- Download from: https://dotnet.microsoft.com/download/dotnet-framework/net481

**Solution 2: Run as Administrator**
```
Error: Access denied / Privilege required
```
- Right-click → **Run as administrator**

**Solution 3: Repair Visual C++ Redistributables**
```
Error: VCRUNTIME140.dll not found
```
- Download VC++ Redist from: https://aka.ms/vs/17/release/vc_redist.x64.exe

### Issue: "Antivirus deleted/quarantined the file"

**Solution:** Whitelist the application (see [ANTIVIRUS.md](ANTIVIRUS.md))

1. Restore file from quarantine
2. Add exclusion for BGM folder
3. Re-extract or re-install

### Issue: "Cannot detect game process"

**Checklist:**
- ✅ Is Ragnarok Online running?
- ✅ Are you running BGM as administrator?
- ✅ Is the game process visible in Task Manager?
- ✅ Is the game 32-bit? (BGM doesn't support 64-bit clients yet)
- ✅ Are you on a supported server (OsRO)?

**Solution:**
1. Start Ragnarok Online first
2. Then start BGM as administrator
3. Click **Refresh** on process dropdown
4. Select correct process

### Issue: "Hotkeys not working"

**Causes:**
- Another application using the same hotkeys
- Keyboard hook not installed
- Running without administrator rights

**Solution:**
1. Check for conflicting applications (AutoHotkey, macro recorders)
2. Change hotkeys in BGM settings
3. Restart BGM as administrator

### Issue: "Application crashes on startup"

**Solution 1: Delete configuration**
```powershell
# Delete corrupted config
Remove-Item "$env:LOCALAPPDATA\BruteGamingMacros\config.json"
```

**Solution 2: Check logs**
```powershell
# View crash logs
Get-Content "$env:LOCALAPPDATA\BruteGamingMacros\Logs\app-*.log" | Select-Object -Last 50
```

**Solution 3: Reinstall**
1. Uninstall BGM
2. Delete `%LOCALAPPDATA%\BruteGamingMacros`
3. Reinstall from latest release

### Issue: "High CPU/Memory usage"

**Normal usage:**
- **CPU:** 5-15% (depends on enabled features)
- **Memory:** 50-150 MB (depends on cache size)

**If abnormal:**
1. Disable unused features
2. Reduce polling intervals
3. Clear memory cache
4. Restart application

### Issue: "Getting banned/detected"

**Important:** This is NOT an antivirus issue - it's a game server policy violation.

**Understanding:**
- BGM is for **offline/private servers only**
- Check your server's automation policy
- Use at your own risk
- We are NOT responsible for bans

**Recommendations:**
- Use conservative automation speeds
- Don't automate 24/7
- Follow server rules
- Consider asking admins about tool policies

## Uninstallation

### If Installed via Installer:

1. **Windows 10/11:**
   - Open **Settings** → **Apps** → **Apps & features**
   - Search for "Brute Gaming Macros"
   - Click **Uninstall**

2. **Control Panel method:**
   - Open **Control Panel** → **Programs and Features**
   - Find "Brute Gaming Macros"
   - Right-click → **Uninstall**

3. **Remove user data (optional):**
   - Uninstaller will ask if you want to remove settings
   - OR manually delete: `%LOCALAPPDATA%\BruteGamingMacros`

### If Portable:

1. Close the application
2. Delete the extracted folder
3. Remove shortcuts (if created)
4. Delete user data: `%LOCALAPPDATA%\BruteGamingMacros` (optional)

## Advanced Configuration

### Configuration File Location

```
%LOCALAPPDATA%\BruteGamingMacros\
├── config.json          # Application settings
├── profiles\            # Character profiles
│   ├── character1.json
│   └── character2.json
└── Logs\                # Application logs
    └── app-*.log
```

### Editing Configuration Manually

**Warning:** Only for advanced users. Backup first!

```powershell
# Open config in notepad
notepad "$env:LOCALAPPDATA\BruteGamingMacros\config.json"
```

### Command-Line Arguments

```batch
BruteGamingMacros.exe [options]

Options:
  --no-update-check    Skip checking for updates
  --debug              Enable debug logging
  --profile <name>     Load specific profile on startup
  --minimized          Start minimized to tray
```

Example:
```batch
BruteGamingMacros.exe --profile "MyCharacter" --minimized
```

## Updating

### Automatic Updates (Recommended)

1. BGM checks for updates on startup
2. If available, you'll see a notification
3. Click **Update** to download and install
4. Application will restart automatically

### Manual Updates

1. Download latest release from GitHub
2. Close BGM
3. **Installer:** Run new installer (will upgrade in place)
4. **Portable:** Extract to same folder (overwrite files)
5. Launch BGM
6. Your settings and profiles are preserved

### Rollback to Previous Version

If an update causes issues:

1. Download previous version from [Releases](https://github.com/epicseo/BruteGamingMacros/releases)
2. Uninstall current version
3. Install previous version
4. Report the issue on GitHub

## Data Backup

### Backup Your Profiles

```powershell
# Backup all settings and profiles
$backup = "$env:USERPROFILE\Desktop\BGM_Backup_$(Get-Date -Format 'yyyyMMdd').zip"
Compress-Archive -Path "$env:LOCALAPPDATA\BruteGamingMacros" -DestinationPath $backup
```

### Restore from Backup

```powershell
# Extract backup
Expand-Archive -Path "C:\path\to\BGM_Backup.zip" -DestinationPath "$env:LOCALAPPDATA\BruteGamingMacros" -Force
```

## Portable USB Installation

To run BGM from a USB drive:

1. Extract portable version to USB drive
2. Create a batch file on USB: `RunBGM.bat`
   ```batch
   @echo off
   cd /d "%~dp0"
   powershell -Command "Start-Process 'BruteGamingMacros.exe' -Verb RunAs"
   ```
3. Double-click `RunBGM.bat` to launch with admin rights

**Note:** User data will still be stored in `%LOCALAPPDATA%` on the host PC.

## Multi-User Setup

If multiple Windows users want to use BGM:

1. Each user needs their own installation OR
2. Use portable version with separate config folders:
   ```batch
   BruteGamingMacros.exe --config-dir "D:\BGM\UserConfigs\User1"
   ```

## Network/Server Deployment

**NOT RECOMMENDED** for the following reasons:
- Requires admin rights (security risk)
- Keyboard hooks are per-session
- Memory reading is local only
- No remote control features

## Getting Help

### Resources
- **Documentation:** `/docs` folder in installation
- **FAQ:** [FAQ.md](FAQ.md)
- **GitHub Issues:** https://github.com/epicseo/BruteGamingMacros/issues
- **Releases:** https://github.com/epicseo/BruteGamingMacros/releases

### Reporting Installation Issues

When reporting installation problems, include:
1. Windows version (`winver`)
2. .NET version (see above PowerShell command)
3. Installation method (installer/portable)
4. Error messages (screenshot or text)
5. Antivirus software installed
6. Logs from `%LOCALAPPDATA%\BruteGamingMacros\Logs`

**Create an issue:** https://github.com/epicseo/BruteGamingMacros/issues/new

---

**Last Updated:** 2025-11-12
**Version:** 2.0.1

*For build instructions, see [BUILD.md](BUILD.md)*
*For troubleshooting, see [FAQ.md](FAQ.md)*
