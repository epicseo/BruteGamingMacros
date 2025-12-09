using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BruteGamingMacros.Core.Utils
{
    /// <summary>
    /// AOB (Array of Bytes) pattern scanner for auto-detecting memory addresses.
    /// Scans process memory regions to find patterns that identify game data structures.
    /// </summary>
    public class PatternScanner : IDisposable
    {
        #region Win32 API

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        private const uint PROCESS_VM_READ = 0x0010;
        private const uint PROCESS_QUERY_INFORMATION = 0x0400;
        private const uint MEM_COMMIT = 0x1000;
        private const uint PAGE_READABLE = 0x02 | 0x04 | 0x20 | 0x40; // PAGE_READONLY | PAGE_READWRITE | PAGE_EXECUTE_READ | PAGE_EXECUTE_READWRITE

        #endregion

        private IntPtr _processHandle = IntPtr.Zero;
        private Process _process;
        private bool _disposed = false;
        private readonly object _lock = new object();

        /// <summary>
        /// Gets the attached process.
        /// </summary>
        public Process AttachedProcess => _process;

        /// <summary>
        /// Gets whether a process is currently attached.
        /// </summary>
        public bool IsAttached => _processHandle != IntPtr.Zero;

        /// <summary>
        /// Creates a new PatternScanner instance.
        /// </summary>
        public PatternScanner()
        {
        }

        /// <summary>
        /// Attaches to a process for scanning.
        /// </summary>
        /// <param name="process">The process to attach to</param>
        /// <returns>True if attachment succeeded</returns>
        public bool Attach(Process process)
        {
            lock (_lock)
            {
                try
                {
                    Detach();

                    _process = process;
                    _processHandle = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, process.Id);

                    if (_processHandle == IntPtr.Zero)
                    {
                        DebugLogger.Error($"PatternScanner: Failed to open process {process.ProcessName} (PID: {process.Id})");
                        return false;
                    }

                    DebugLogger.Info($"PatternScanner: Attached to {process.ProcessName} (PID: {process.Id})");
                    return true;
                }
                catch (Exception ex)
                {
                    DebugLogger.Error(ex, "PatternScanner: Failed to attach to process");
                    return false;
                }
            }
        }

        /// <summary>
        /// Detaches from the current process.
        /// </summary>
        public void Detach()
        {
            lock (_lock)
            {
                if (_processHandle != IntPtr.Zero)
                {
                    CloseHandle(_processHandle);
                    _processHandle = IntPtr.Zero;
                    _process = null;
                    DebugLogger.Info("PatternScanner: Detached from process");
                }
            }
        }

        /// <summary>
        /// Scans for a pattern in process memory.
        /// </summary>
        /// <param name="pattern">The byte pattern to search for (use 0xFF as wildcard)</param>
        /// <param name="mask">The mask string where 'x' is exact match and '?' is wildcard</param>
        /// <param name="startAddress">Start address for scanning (0 for beginning)</param>
        /// <param name="endAddress">End address for scanning (0 for end of process memory)</param>
        /// <returns>The address where pattern was found, or IntPtr.Zero if not found</returns>
        public IntPtr FindPattern(byte[] pattern, string mask, long startAddress = 0, long endAddress = 0)
        {
            if (!IsAttached)
            {
                DebugLogger.Warning("PatternScanner: Not attached to any process");
                return IntPtr.Zero;
            }

            if (pattern == null || mask == null || pattern.Length != mask.Length)
            {
                DebugLogger.Error("PatternScanner: Invalid pattern or mask");
                return IntPtr.Zero;
            }

            try
            {
                var results = ScanMemoryRegions(pattern, mask, startAddress, endAddress, maxResults: 1);
                return results.Count > 0 ? results[0] : IntPtr.Zero;
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "PatternScanner: Error during pattern scan");
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Scans for all occurrences of a pattern in process memory.
        /// </summary>
        /// <param name="pattern">The byte pattern to search for</param>
        /// <param name="mask">The mask string where 'x' is exact match and '?' is wildcard</param>
        /// <param name="startAddress">Start address for scanning</param>
        /// <param name="endAddress">End address for scanning</param>
        /// <param name="maxResults">Maximum number of results to return</param>
        /// <returns>List of addresses where pattern was found</returns>
        public List<IntPtr> FindAllPatterns(byte[] pattern, string mask, long startAddress = 0, long endAddress = 0, int maxResults = 100)
        {
            if (!IsAttached)
            {
                DebugLogger.Warning("PatternScanner: Not attached to any process");
                return new List<IntPtr>();
            }

            return ScanMemoryRegions(pattern, mask, startAddress, endAddress, maxResults);
        }

        /// <summary>
        /// Parses an AOB pattern string into byte array and mask.
        /// Format: "89 45 ?? 8B 4D" where ?? is wildcard
        /// </summary>
        /// <param name="patternString">The pattern string to parse</param>
        /// <param name="pattern">Output byte array</param>
        /// <param name="mask">Output mask string</param>
        /// <returns>True if parsing succeeded</returns>
        public static bool ParsePattern(string patternString, out byte[] pattern, out string mask)
        {
            pattern = null;
            mask = null;

            if (string.IsNullOrWhiteSpace(patternString))
                return false;

            try
            {
                var parts = patternString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var patternList = new List<byte>();
                var maskBuilder = new System.Text.StringBuilder();

                foreach (var part in parts)
                {
                    if (part == "??" || part == "?")
                    {
                        patternList.Add(0x00);
                        maskBuilder.Append('?');
                    }
                    else
                    {
                        patternList.Add(Convert.ToByte(part, 16));
                        maskBuilder.Append('x');
                    }
                }

                pattern = patternList.ToArray();
                mask = maskBuilder.ToString();
                return true;
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"PatternScanner: Failed to parse pattern: {patternString}");
                return false;
            }
        }

        /// <summary>
        /// Scans a specific memory region for a pattern.
        /// </summary>
        private List<IntPtr> ScanMemoryRegions(byte[] pattern, string mask, long startAddress, long endAddress, int maxResults)
        {
            var results = new List<IntPtr>();
            var address = new IntPtr(startAddress);

            // Get process memory bounds
            if (endAddress == 0)
            {
                endAddress = _process.MainModule?.ModuleMemorySize ?? 0x7FFFFFFF;
                if (startAddress == 0)
                {
                    startAddress = (long)(_process.MainModule?.BaseAddress ?? IntPtr.Zero);
                    address = new IntPtr(startAddress);
                }
            }

            const int chunkSize = 65536; // 64KB chunks
            var buffer = new byte[chunkSize];

            while (address.ToInt64() < endAddress && results.Count < maxResults)
            {
                MEMORY_BASIC_INFORMATION memInfo;
                if (VirtualQueryEx(_processHandle, address, out memInfo, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) == 0)
                    break;

                // Check if region is committed and readable
                if (memInfo.State == MEM_COMMIT && (memInfo.Protect & PAGE_READABLE) != 0)
                {
                    var regionSize = (long)memInfo.RegionSize;
                    var regionStart = memInfo.BaseAddress;

                    // Scan this region in chunks
                    for (long offset = 0; offset < regionSize && results.Count < maxResults; offset += chunkSize)
                    {
                        var readAddress = new IntPtr(regionStart.ToInt64() + offset);
                        var bytesToRead = (int)Math.Min(chunkSize, regionSize - offset);

                        int bytesRead;
                        if (ReadProcessMemory(_processHandle, readAddress, buffer, bytesToRead, out bytesRead) && bytesRead > 0)
                        {
                            // Search for pattern in buffer
                            for (int i = 0; i <= bytesRead - pattern.Length && results.Count < maxResults; i++)
                            {
                                if (MatchPattern(buffer, i, pattern, mask))
                                {
                                    var foundAddress = new IntPtr(readAddress.ToInt64() + i);
                                    results.Add(foundAddress);
                                    DebugLogger.Info($"PatternScanner: Found pattern at 0x{foundAddress.ToInt64():X8}");
                                }
                            }
                        }
                    }
                }

                // Move to next region
                address = new IntPtr(memInfo.BaseAddress.ToInt64() + (long)memInfo.RegionSize);
            }

            return results;
        }

        /// <summary>
        /// Checks if buffer at given offset matches the pattern with mask.
        /// </summary>
        private static bool MatchPattern(byte[] buffer, int offset, byte[] pattern, string mask)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (mask[i] == 'x' && buffer[offset + i] != pattern[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Reads memory at specified address and returns as hex string.
        /// </summary>
        public string ReadAddressHex(IntPtr address)
        {
            if (!IsAttached)
                return "00000000";

            var buffer = new byte[4];
            int bytesRead;
            if (ReadProcessMemory(_processHandle, address, buffer, 4, out bytesRead) && bytesRead == 4)
            {
                return BitConverter.ToInt32(buffer, 0).ToString("X8");
            }
            return "00000000";
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _process = null;
                }

                Detach();
                _disposed = true;
            }
        }

        ~PatternScanner()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    /// <summary>
    /// Known pattern signatures for Ragnarok Online clients.
    /// These may need to be updated when game client updates.
    /// </summary>
    public static class ROPatterns
    {
        /// <summary>
        /// Pattern hints for finding HP/SP addresses.
        /// These are reference patterns - actual patterns may vary by client version.
        /// </summary>
        public static class AddressPatterns
        {
            // Note: These patterns are placeholders and need to be discovered
            // using tools like Cheat Engine or x64dbg for each game version.

            /// <summary>
            /// Pattern for HP address (placeholder - needs discovery)
            /// </summary>
            public const string HP_PATTERN = "";

            /// <summary>
            /// Pattern for character name address (placeholder - needs discovery)
            /// </summary>
            public const string NAME_PATTERN = "";

            /// <summary>
            /// Pattern for map name address (placeholder - needs discovery)
            /// </summary>
            public const string MAP_PATTERN = "";

            /// <summary>
            /// Pattern for online status address (placeholder - needs discovery)
            /// </summary>
            public const string ONLINE_PATTERN = "";
        }

        /// <summary>
        /// Tips for discovering patterns using Cheat Engine:
        /// 1. HP: Search for your current HP value, take damage, search again
        /// 2. Name: Search for your character name as text (UTF-8 or ASCII)
        /// 3. Map: Search for current map name (e.g., "prontera")
        /// 4. Online: Search for 1 when connected, 0 when disconnected
        ///
        /// Once found, look at surrounding bytes to create a unique pattern.
        /// Use ?? for bytes that change between sessions.
        /// Example: "89 45 ?? 8B 4D ?? 89"
        /// </summary>
        public static string GetPatternDiscoveryGuide()
        {
            return @"
Pattern Discovery Guide for Ragnarok Online:

1. HP/SP Addresses:
   - Open Cheat Engine, attach to RO process
   - Search for your current HP as '4 Bytes' exact value
   - Get hit by a monster, search for new HP value
   - Repeat until you have 1-3 results
   - The address should be stable across sessions

2. Character Name:
   - Search for your character name as 'String' (UTF-8 or UTF-16)
   - Filter results by checking which updates when you relog

3. Map Name:
   - Search for current map name (e.g., 'prontera')
   - Teleport to another map, filter for new map name

4. Online Status:
   - Search for '1' as byte when logged in
   - Disconnect, search for '0'
   - Filter for address that changes correctly

5. Creating Pattern:
   - Once you find the address, view memory region
   - Look for unique byte sequences before the address
   - Use ?? for bytes that vary (e.g., pointer values)
   - Test pattern to ensure it finds the same address

6. Update Config/addresses.json with found addresses
";
        }
    }
}
