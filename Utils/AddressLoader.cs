using BruteGamingMacros.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BruteGamingMacros.Core.Utils
{
    /// <summary>
    /// Loads and manages memory address configurations from external JSON files.
    /// Enables runtime address updates without recompilation.
    /// </summary>
    public static class AddressLoader
    {
        private static AddressConfiguration _configuration;
        private static readonly object _lock = new object();
        private static bool _initialized = false;
        private static string _configPath;

        /// <summary>
        /// Gets the path to the addresses configuration file.
        /// </summary>
        public static string ConfigPath
        {
            get
            {
                if (string.IsNullOrEmpty(_configPath))
                {
                    _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "addresses.json");
                }
                return _configPath;
            }
        }

        /// <summary>
        /// Gets the loaded configuration, initializing if necessary.
        /// </summary>
        public static AddressConfiguration Configuration
        {
            get
            {
                if (!_initialized)
                {
                    Initialize();
                }
                return _configuration;
            }
        }

        /// <summary>
        /// Initializes the address loader, loading configuration from file.
        /// Falls back to hardcoded defaults if file is missing or invalid.
        /// </summary>
        public static void Initialize()
        {
            lock (_lock)
            {
                if (_initialized)
                    return;

                try
                {
                    EnsureConfigDirectoryExists();

                    if (File.Exists(ConfigPath))
                    {
                        LoadFromFile();
                    }
                    else
                    {
                        DebugLogger.Warning($"Address config not found at {ConfigPath}, creating default");
                        CreateDefaultConfigFile();
                        LoadFromFile();
                    }

                    _initialized = true;
                    DebugLogger.Info($"AddressLoader initialized. Config version: {_configuration?.Version ?? "unknown"}");
                }
                catch (Exception ex)
                {
                    DebugLogger.Error(ex, "Failed to initialize AddressLoader, using hardcoded fallback");
                    _configuration = CreateHardcodedFallback();
                    _initialized = true;
                }
            }
        }

        /// <summary>
        /// Reloads the configuration from file.
        /// </summary>
        public static void Reload()
        {
            lock (_lock)
            {
                _initialized = false;
                _configuration = null;
                Initialize();
            }
        }

        /// <summary>
        /// Gets the server configuration for the current server mode.
        /// </summary>
        /// <param name="serverMode">0=MR, 1=HR, 2=LR</param>
        /// <returns>Server configuration with addresses</returns>
        public static ServerAddressConfig GetServerConfig(int serverMode)
        {
            var config = Configuration?.GetServerConfig(serverMode);

            if (config == null)
            {
                DebugLogger.Warning($"No config found for server mode {serverMode}, using fallback");
                return GetHardcodedServerConfig(serverMode);
            }

            return config;
        }

        /// <summary>
        /// Gets server addresses as a dynamic object for backward compatibility with AppConfig.
        /// </summary>
        public static List<dynamic> GetServerAddresses(int serverMode)
        {
            var config = GetServerConfig(serverMode);

            return new List<dynamic>
            {
                new
                {
                    name = config.Name,
                    description = config.Description,
                    hpAddress = config.Addresses.Hp,
                    nameAddress = config.Addresses.Name,
                    mapAddress = config.Addresses.Map,
                    onlineAddress = config.Addresses.Online
                }
            };
        }

        /// <summary>
        /// Updates addresses for a specific server and saves to file.
        /// </summary>
        public static bool UpdateServerAddresses(int serverMode, MemoryAddresses addresses)
        {
            lock (_lock)
            {
                try
                {
                    if (_configuration == null)
                        Initialize();

                    string key = AddressConfiguration.GetServerKey(serverMode);
                    if (_configuration.Servers.TryGetValue(key, out var serverConfig))
                    {
                        serverConfig.Addresses = addresses;
                        serverConfig.VerifiedDate = DateTime.Now.ToString("yyyy-MM-dd");
                        serverConfig.Verified = true;

                        SaveToFile();
                        DebugLogger.Info($"Updated addresses for server {key}");
                        return true;
                    }

                    DebugLogger.Warning($"Server {key} not found in configuration");
                    return false;
                }
                catch (Exception ex)
                {
                    DebugLogger.Error(ex, "Failed to update server addresses");
                    return false;
                }
            }
        }

        /// <summary>
        /// Checks if the current server configuration has valid addresses.
        /// </summary>
        public static bool HasValidAddresses(int serverMode)
        {
            var config = GetServerConfig(serverMode);
            return config?.HasValidAddresses() ?? false;
        }

        private static void LoadFromFile()
        {
            string json = File.ReadAllText(ConfigPath);
            _configuration = JsonConvert.DeserializeObject<AddressConfiguration>(json);

            if (_configuration == null)
            {
                throw new InvalidOperationException("Failed to deserialize address configuration");
            }
        }

        private static void SaveToFile()
        {
            string json = JsonConvert.SerializeObject(_configuration, Formatting.Indented);
            File.WriteAllText(ConfigPath, json);
        }

        private static void EnsureConfigDirectoryExists()
        {
            string configDir = Path.GetDirectoryName(ConfigPath);
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
                DebugLogger.Info($"Created config directory: {configDir}");
            }
        }

        private static void CreateDefaultConfigFile()
        {
            var defaultConfig = CreateHardcodedFallback();
            string json = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
            File.WriteAllText(ConfigPath, json);
            DebugLogger.Info($"Created default config file: {ConfigPath}");
        }

        /// <summary>
        /// Creates a hardcoded fallback configuration matching original AppConfig values.
        /// </summary>
        private static AddressConfiguration CreateHardcodedFallback()
        {
            return new AddressConfiguration
            {
                Version = "2.1.0",
                LastUpdated = DateTime.Now.ToString("yyyy-MM-dd"),
                Description = "Hardcoded fallback configuration",
                Servers = new Dictionary<string, ServerAddressConfig>
                {
                    ["MR"] = new ServerAddressConfig
                    {
                        Name = "OsRO Midrate",
                        Description = "OsRO Midrate Server",
                        Website = "https://osro.mr",
                        WindowClass = "Oldschool RO - Midrate | www.osro.mr",
                        Addresses = new MemoryAddresses
                        {
                            Hp = "00E8F434",
                            Name = "00E91C00",
                            Map = "00E8ABD4",
                            Online = "00E8A928"
                        },
                        Verified = true,
                        VerifiedDate = "2025-12-09"
                    },
                    ["HR"] = new ServerAddressConfig
                    {
                        Name = "OsRO Highrate",
                        Description = "OsRO Highrate Server",
                        Website = "https://osro.gg",
                        WindowClass = "Oldschool RO | www.osro.gg",
                        Addresses = new MemoryAddresses
                        {
                            Hp = "010DCE10",
                            Name = "010DF5D8",
                            Map = "010D856C",
                            Online = "010D83C7"
                        },
                        Verified = true,
                        VerifiedDate = "2025-12-09"
                    },
                    ["LR"] = new ServerAddressConfig
                    {
                        Name = "OsRO Revo",
                        Description = "OsRO Revo (Lowrate) - Addresses not configured",
                        Website = "https://osro-revo.gg",
                        WindowClass = "Oldschool RO | Revo",
                        Addresses = new MemoryAddresses
                        {
                            Hp = "00000000",
                            Name = "00000000",
                            Map = "00000000",
                            Online = "00000000"
                        },
                        Verified = false,
                        Notes = "Memory addresses need to be discovered. See docs/CONTRIBUTING.md"
                    }
                }
            };
        }

        /// <summary>
        /// Gets hardcoded server config as fallback.
        /// </summary>
        private static ServerAddressConfig GetHardcodedServerConfig(int serverMode)
        {
            var fallback = CreateHardcodedFallback();
            string key = AddressConfiguration.GetServerKey(serverMode);
            return fallback.Servers.TryGetValue(key, out var config) ? config : fallback.Servers["MR"];
        }
    }
}
