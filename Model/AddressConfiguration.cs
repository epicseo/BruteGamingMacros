using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BruteGamingMacros.Core.Model
{
    /// <summary>
    /// Root configuration for memory addresses loaded from Config/addresses.json.
    /// Enables runtime configuration updates without recompilation.
    /// </summary>
    public class AddressConfiguration
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("lastUpdated")]
        public string LastUpdated { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("servers")]
        public Dictionary<string, ServerAddressConfig> Servers { get; set; }

        [JsonProperty("patternSignatures")]
        public PatternSignatures PatternSignatures { get; set; }

        public AddressConfiguration()
        {
            Servers = new Dictionary<string, ServerAddressConfig>();
        }

        /// <summary>
        /// Gets the server configuration for the specified server mode.
        /// </summary>
        /// <param name="serverMode">0=MR, 1=HR, 2=LR</param>
        /// <returns>Server configuration or null if not found</returns>
        public ServerAddressConfig GetServerConfig(int serverMode)
        {
            string key = GetServerKey(serverMode);
            return Servers.TryGetValue(key, out var config) ? config : null;
        }

        /// <summary>
        /// Converts server mode int to config key.
        /// </summary>
        public static string GetServerKey(int serverMode)
        {
            switch (serverMode)
            {
                case 0: return "MR";
                case 1: return "HR";
                case 2: return "LR";
                default: return "MR";
            }
        }
    }

    /// <summary>
    /// Configuration for a specific game server.
    /// </summary>
    public class ServerAddressConfig
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("windowClass")]
        public string WindowClass { get; set; }

        [JsonProperty("addresses")]
        public MemoryAddresses Addresses { get; set; }

        [JsonProperty("offsets")]
        public MemoryOffsets Offsets { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("verifiedDate")]
        public string VerifiedDate { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        public ServerAddressConfig()
        {
            Addresses = new MemoryAddresses();
            Offsets = new MemoryOffsets();
        }

        /// <summary>
        /// Checks if this server configuration has valid (non-zero) addresses.
        /// </summary>
        public bool HasValidAddresses()
        {
            return Addresses != null &&
                   !string.IsNullOrEmpty(Addresses.Hp) &&
                   Addresses.Hp != "00000000" &&
                   !string.IsNullOrEmpty(Addresses.Name) &&
                   Addresses.Name != "00000000";
        }
    }

    /// <summary>
    /// Memory addresses for game data.
    /// </summary>
    public class MemoryAddresses
    {
        [JsonProperty("hp")]
        public string Hp { get; set; } = "00000000";

        [JsonProperty("sp")]
        public string Sp { get; set; } = "00000000";

        [JsonProperty("maxHp")]
        public string MaxHp { get; set; } = "00000000";

        [JsonProperty("maxSp")]
        public string MaxSp { get; set; } = "00000000";

        [JsonProperty("name")]
        public string Name { get; set; } = "00000000";

        [JsonProperty("map")]
        public string Map { get; set; } = "00000000";

        [JsonProperty("online")]
        public string Online { get; set; } = "00000000";

        [JsonProperty("baseLevel")]
        public string BaseLevel { get; set; } = "00000000";

        [JsonProperty("jobLevel")]
        public string JobLevel { get; set; } = "00000000";

        [JsonProperty("statusBuffer")]
        public string StatusBuffer { get; set; } = "00000000";

        /// <summary>
        /// Converts a hex address string to int pointer.
        /// </summary>
        public static int ToPointer(string hexAddress)
        {
            if (string.IsNullOrEmpty(hexAddress) || hexAddress == "00000000")
                return 0;

            try
            {
                return Convert.ToInt32(hexAddress, 16);
            }
            catch
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// Memory offsets for complex data structures.
    /// </summary>
    public class MemoryOffsets
    {
        [JsonProperty("statusBufferOffset")]
        public int StatusBufferOffset { get; set; }

        [JsonProperty("statusBufferSize")]
        public int StatusBufferSize { get; set; } = 108;
    }

    /// <summary>
    /// AOB pattern signatures for auto-detection.
    /// </summary>
    public class PatternSignatures
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("hp")]
        public PatternSignature Hp { get; set; }

        [JsonProperty("name")]
        public PatternSignature Name { get; set; }
    }

    /// <summary>
    /// Single pattern signature for memory scanning.
    /// </summary>
    public class PatternSignature
    {
        [JsonProperty("pattern")]
        public string Pattern { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }
}
