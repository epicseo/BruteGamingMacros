# Memory Address Discovery Guide

This guide explains how to find and update memory addresses for Ragnarok Online private servers when game client updates break functionality.

## Overview

BruteGamingMacros reads memory addresses from the game client to detect:
- Current HP/SP values
- Max HP/SP values
- Character name
- Current map
- Online/connected status

When the game client updates, these addresses may change and need to be rediscovered.

## Prerequisites

1. **Cheat Engine** - Download from https://www.cheatengine.org/
2. **Administrator privileges** - Required to read game memory
3. **Running game client** - Log into a character first

## Step-by-Step Address Discovery

### 1. Finding HP Address

1. Open Cheat Engine and attach to the RO process (`ragexe.exe` or similar)
2. Set **Value Type** to `4 Bytes`
3. Note your current HP (e.g., 5000)
4. Enter this value and click **First Scan**
5. Get hit by a monster to change your HP
6. Enter your new HP value and click **Next Scan**
7. Repeat steps 5-6 until you have 1-5 results
8. The correct address should:
   - Update in real-time as your HP changes
   - Be stable across game sessions (same address after restarting client)

### 2. Finding SP Address

Follow the same process as HP:
1. Search for current SP value
2. Use a skill to reduce SP
3. Search for new SP value
4. Repeat until narrowed down

**Tip:** SP address is often near the HP address (within a few bytes).

### 3. Finding Max HP/SP Addresses

1. Search for your max HP/SP value
2. Level up or use equipment that changes max HP/SP
3. Search for new value
4. The addresses are often 4-8 bytes after the current HP/SP addresses

### 4. Finding Character Name Address

1. Set **Value Type** to `String`
2. Search for your character name
3. Filter by looking for addresses in the game's memory region
4. The correct address will update when you switch characters

### 5. Finding Map Name Address

1. Set **Value Type** to `String`
2. Search for current map name (e.g., `prontera`)
3. Teleport to another map
4. Search for new map name
5. Repeat to narrow down

### 6. Finding Online Status Address

1. Set **Value Type** to `Byte`
2. Search for `1` when logged into a character
3. Disconnect from server
4. Search for `0`
5. The address that changes correctly is the online status

## Updating addresses.json

Once you've found the addresses, update `Config/addresses.json`:

```json
{
  "servers": {
    "MR": {
      "addresses": {
        "hp": "00E8F434",
        "sp": "00E8F438",
        "maxHp": "00E8F43C",
        "maxSp": "00E8F440",
        "name": "00E91C00",
        "map": "00E8ABD4",
        "online": "00E8A928"
      },
      "verified": true,
      "verifiedDate": "2025-01-15"
    }
  }
}
```

**Address Format:** Use hexadecimal without `0x` prefix (e.g., `00E8F434`)

## Creating AOB Patterns

For more resilient address detection, create AOB (Array of Bytes) patterns:

### Why Use Patterns?

- Addresses may change with each client update
- Patterns find addresses automatically by searching for unique byte sequences
- Patterns are more portable across different client versions

### How to Create a Pattern

1. In Cheat Engine, find the address you want
2. Right-click the address and select **Browse this memory region**
3. Look at the bytes before and after the address
4. Find a unique sequence (10-20 bytes) that includes the address
5. Note which bytes change between sessions (use `??` as wildcards)

### Pattern Format

```
89 45 ?? 8B 4D ?? 89 01 8B 45
```

- Each byte is 2 hex characters separated by spaces
- `??` indicates a wildcard (any byte matches)

### Testing Patterns

Use the PatternScanner class to verify your pattern finds the correct address:

```csharp
using (var scanner = new PatternScanner())
{
    scanner.Attach(gameProcess);

    if (PatternScanner.ParsePattern("89 45 ?? 8B 4D", out byte[] pattern, out string mask))
    {
        IntPtr address = scanner.FindPattern(pattern, mask);
        Console.WriteLine($"Found at: 0x{address.ToInt64():X8}");
    }
}
```

## Troubleshooting

### Address Changes Every Session
- The address you found might be a pointer
- Look for a stable base address that points to your value
- Create an AOB pattern instead

### Can't Find the Value
- Make sure you're searching the correct value type
- Try searching in different memory regions
- The value might be stored differently (float, double, etc.)

### Cheat Engine Crashes Game
- Run Cheat Engine as Administrator
- Disable any anti-cheat software
- Try using a older version of Cheat Engine

### Value Found But Doesn't Work in App
- Verify the address format (hex without 0x prefix)
- Check if the address is 32-bit or 64-bit
- Ensure you have the correct server mode selected

## Server-Specific Notes

### OsRO Midrate (MR)
- Uses standard RO memory layout
- Addresses verified as of December 2024

### OsRO Highrate (HR)
- Different client version from MR
- Addresses are in different memory regions

### OsRO Revo/Lowrate (LR)
- **Addresses not yet discovered**
- Follow this guide to find them
- Submit addresses via GitHub issue or PR

## Contributing

Found working addresses for a server? Please contribute:

1. Fork the repository
2. Update `Config/addresses.json` with new addresses
3. Set `verified: true` and `verifiedDate` to today
4. Submit a Pull Request

Include in your PR:
- Server name and version
- Date tested
- Any special notes about the client
