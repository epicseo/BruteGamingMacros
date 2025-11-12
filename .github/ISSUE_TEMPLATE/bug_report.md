---
name: Bug Report
about: Report a bug or issue with Brute Gaming Macros
title: '[BUG] '
labels: bug
assignees: ''
---

## Bug Description
<!-- A clear and concise description of what the bug is -->

## Steps to Reproduce
1. Go to '...'
2. Click on '...'
3. Enable '...'
4. See error

## Expected Behavior
<!-- What you expected to happen -->

## Actual Behavior
<!-- What actually happened -->

## Screenshots
<!-- If applicable, add screenshots to help explain your problem -->

## Environment
**BruteGamingMacros Version:** (e.g., 2.0.1)
**Windows Version:** (e.g., Windows 11 22H2)
**. NET Framework Version:** (e.g., 4.8.1)
**Game Server:** (e.g., OsRO MR)
**Installation Method:** (Installer / Portable)

**To find your versions:**
- BGM Version: Check About dialog or title bar
- Windows: Run `winver` in command prompt
- .NET: Run this in PowerShell:
  ```powershell
  Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full' | Get-ItemProperty -Name Release
  ```

## Additional Information

### Antivirus Software
<!-- List any antivirus software you have installed -->
- [ ] Windows Defender
- [ ] Avast
- [ ] Norton
- [ ] Other: _______

### Logs
<!-- Please attach logs from %LOCALAPPDATA%\BruteGamingMacros\Logs\ -->
<!-- Drag and drop log files here or paste relevant excerpts -->

```
[Paste log contents here if short]
```

### Configuration
<!-- If relevant, describe your configuration -->
- Features enabled: (e.g., Auto-Pot, Auto-Buff, Spam Engine)
- Hotkeys configured: (e.g., F1, F2, Ctrl+Space)
- Number of profiles: _______

## Frequency
- [ ] This happens every time
- [ ] This happens sometimes (specify: _____% of the time)
- [ ] This happened once

## Impact
- [ ] **Critical** - Application crashes or data loss
- [ ] **High** - Feature completely broken
- [ ] **Medium** - Feature partially works
- [ ] **Low** - Minor inconvenience

## Workaround
<!-- If you found a workaround, describe it here -->

## Additional Context
<!-- Add any other context about the problem here -->

---

**Checklist before submitting:**
- [ ] I have searched existing issues to make sure this isn't a duplicate
- [ ] I am using the latest version of BruteGamingMacros
- [ ] I have included logs (if applicable)
- [ ] I have provided steps to reproduce
- [ ] I have checked the [FAQ](https://github.com/epicseo/BruteGamingMacros/blob/main/docs/FAQ.md)
