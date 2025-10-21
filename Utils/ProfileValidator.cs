using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace BruteGamingMacros.Core.Utils
{
    /// <summary>
    /// Validates profile JSON files for security and integrity
    /// </summary>
    public static class ProfileValidator
    {
        /// <summary>
        /// Validates a profile JSON file before loading
        /// </summary>
        /// <param name="profilePath">Path to the profile JSON file</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool ValidateProfileFile(string profilePath)
        {
            try
            {
                // Check file exists
                if (!File.Exists(profilePath))
                {
                    DebugLogger.Error($"Profile file does not exist: {profilePath}");
                    return false;
                }

                // Check file size (profiles should be < 1MB)
                var fileInfo = new FileInfo(profilePath);
                if (fileInfo.Length > 1048576) // 1MB
                {
                    DebugLogger.Error($"Profile file too large: {fileInfo.Length} bytes (max: 1MB)");
                    return false;
                }

                // Read and parse JSON
                string jsonContent = File.ReadAllText(profilePath);

                // Validate JSON structure
                return ValidateProfileJson(jsonContent);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Profile validation failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Validates profile JSON content
        /// </summary>
        /// <param name="jsonContent">JSON string to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool ValidateProfileJson(string jsonContent)
        {
            try
            {
                // Check for null or empty
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    DebugLogger.Error("Profile JSON is empty");
                    return false;
                }

                // Parse JSON
                JObject profileObj = JObject.Parse(jsonContent);

                // Validate required properties exist
                if (!ValidateRequiredProperties(profileObj))
                {
                    return false;
                }

                // Validate property types
                if (!ValidatePropertyTypes(profileObj))
                {
                    return false;
                }

                // Validate value ranges
                if (!ValidateValueRanges(profileObj))
                {
                    return false;
                }

                DebugLogger.Debug("Profile JSON validation passed");
                return true;
            }
            catch (JsonReaderException ex)
            {
                DebugLogger.Error($"Invalid JSON format: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"JSON validation error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Validates that required properties exist
        /// </summary>
        private static bool ValidateRequiredProperties(JObject profileObj)
        {
            string[] requiredProperties = new string[]
            {
                "Name",
                "UserPreferences",
                "SkillSpammer",
                "Autopot",
                "AutopotYgg"
            };

            foreach (string property in requiredProperties)
            {
                if (profileObj[property] == null)
                {
                    DebugLogger.Warning($"Missing required property: {property}");
                    // Don't fail - older profiles may miss some properties
                }
            }

            return true; // Allow loading even with missing properties (defaults will be used)
        }

        /// <summary>
        /// Validates property types
        /// </summary>
        private static bool ValidatePropertyTypes(JObject profileObj)
        {
            try
            {
                // Name should be a string
                if (profileObj["Name"] != null && profileObj["Name"].Type != JTokenType.String)
                {
                    DebugLogger.Error("Property 'Name' must be a string");
                    return false;
                }

                // Check that objects are objects, not primitives
                string[] objectProperties = new string[]
                {
                    "UserPreferences", "SkillSpammer", "Autopot", "AutopotYgg",
                    "SkillTimer", "AutobuffSkill", "AutobuffItem", "StatusRecovery",
                    "SongMacro", "MacroSwitch", "AtkDefMode", "DebuffsRecovery",
                    "WeightDebuffsRecovery", "TransferHelper"
                };

                foreach (string property in objectProperties)
                {
                    if (profileObj[property] != null &&
                        profileObj[property].Type != JTokenType.Object &&
                        profileObj[property].Type != JTokenType.Null)
                    {
                        DebugLogger.Error($"Property '{property}' must be an object");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Property type validation failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Validates value ranges for numeric properties
        /// </summary>
        private static bool ValidateValueRanges(JObject profileObj)
        {
            try
            {
                // Validate delays are within reasonable ranges
                var components = new[] { "Autopot", "AutopotYgg", "SkillSpammer", "AutobuffSkill", "AutobuffItem" };

                foreach (var component in components)
                {
                    if (profileObj[component] is JObject obj && obj["Delay"] != null)
                    {
                        int delay = obj["Delay"].Value<int>();
                        if (delay < 0 || delay > 10000) // 0-10 seconds
                        {
                            DebugLogger.Warning($"{component}.Delay out of range: {delay} (allowed: 0-10000ms)");
                            // Don't fail, just warn - app will clamp values
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Value range validation failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Sanitizes profile name to prevent path traversal
        /// </summary>
        /// <param name="profileName">Profile name to sanitize</param>
        /// <returns>Sanitized profile name</returns>
        public static string SanitizeProfileName(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                return "Default";
            }

            // Remove path traversal attempts
            profileName = profileName.Replace("..", "");
            profileName = profileName.Replace("/", "");
            profileName = profileName.Replace("\\", "");
            profileName = profileName.Replace(":", "");

            // Remove invalid filename characters
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                profileName = profileName.Replace(c.ToString(), "");
            }

            // Limit length
            if (profileName.Length > 50)
            {
                profileName = profileName.Substring(0, 50);
            }

            // Ensure not empty after sanitization
            if (string.IsNullOrWhiteSpace(profileName))
            {
                return "Default";
            }

            return profileName.Trim();
        }

        /// <summary>
        /// Validates and sanitizes a full profile path
        /// </summary>
        /// <param name="profilePath">Profile path to validate</param>
        /// <returns>Sanitized path or null if invalid</returns>
        public static string ValidateProfilePath(string profilePath)
        {
            try
            {
                // Get the filename without extension
                string fileName = Path.GetFileNameWithoutExtension(profilePath);

                // Sanitize
                string sanitized = SanitizeProfileName(fileName);

                // Reconstruct safe path
                string safePath = Path.Combine(AppConfig.ProfileFolder, sanitized + ".json");

                // Ensure it's within the profile folder (prevent traversal)
                string fullSafePath = Path.GetFullPath(safePath);
                string profileFolderPath = Path.GetFullPath(AppConfig.ProfileFolder);

                if (!fullSafePath.StartsWith(profileFolderPath))
                {
                    DebugLogger.Error($"Path traversal attempt detected: {profilePath}");
                    return null;
                }

                return safePath;
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Path validation failed: {ex.Message}");
                return null;
            }
        }
    }
}
