using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BruteGamingMacros.Core.Utils
{
    /// <summary>
    /// Checks for application updates from GitHub releases with security validation.
    /// </summary>
    public class UpdateChecker
    {
        private const string GITHUB_API_URL = "https://api.github.com/repos/epicseo/BruteGamingMacros/releases/latest";
        private static readonly HttpClient _httpClient;

        static UpdateChecker()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", $"BruteGamingMacros/{ProductionLogger.GetAppVersion()}");
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Check for updates asynchronously.
        /// </summary>
        /// <returns>UpdateInfo object if successful, null on error</returns>
        public static async Task<UpdateInfo> CheckForUpdatesAsync()
        {
            try
            {
                Log.Information("Checking for updates from GitHub...");

                var response = await _httpClient.GetStringAsync(GITHUB_API_URL);
                var json = JObject.Parse(response);

                // Extract version info
                string tagName = json["tag_name"]?.ToString();
                if (string.IsNullOrEmpty(tagName))
                {
                    Log.Warning("No tag_name in GitHub API response");
                    return null;
                }

                // Remove 'v' prefix if present
                string latestVersion = tagName.TrimStart('v');

                // Parse current and latest versions
                Version currentVersion;
                Version remoteVersion;

                try
                {
                    currentVersion = new Version(ProductionLogger.GetAppVersion());
                    remoteVersion = new Version(latestVersion);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Failed to parse version numbers");
                    return null;
                }

                bool isUpdateAvailable = remoteVersion > currentVersion;

                // Extract download info from assets
                string downloadUrl = null;
                string fileName = null;
                long fileSize = 0;

                var assets = json["assets"];
                if (assets != null && assets.HasValues)
                {
                    // Look for the installer executable
                    foreach (var asset in assets)
                    {
                        string assetName = asset["name"]?.ToString();
                        if (!string.IsNullOrEmpty(assetName) && assetName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                        {
                            downloadUrl = asset["browser_download_url"]?.ToString();
                            fileName = assetName;
                            fileSize = asset["size"]?.ToObject<long>() ?? 0;
                            break;
                        }
                    }
                }

                string releaseNotes = json["body"]?.ToString() ?? "";
                string publishedAt = json["published_at"]?.ToString() ?? "";

                var updateInfo = new UpdateInfo
                {
                    IsUpdateAvailable = isUpdateAvailable,
                    CurrentVersion = currentVersion.ToString(),
                    LatestVersion = latestVersion,
                    DownloadUrl = downloadUrl,
                    FileName = fileName,
                    FileSize = fileSize,
                    ReleaseNotes = releaseNotes,
                    PublishedAt = publishedAt,
                    CheckedAt = DateTime.Now
                };

                if (isUpdateAvailable)
                {
                    Log.Information("Update available: v{LatestVersion} (current: v{CurrentVersion})",
                        latestVersion, currentVersion);
                }
                else
                {
                    Log.Information("No updates available (current: v{CurrentVersion})", currentVersion);
                }

                return updateInfo;
            }
            catch (HttpRequestException ex)
            {
                Log.Warning(ex, "Network error while checking for updates");
                return null;
            }
            catch (TaskCanceledException ex)
            {
                Log.Warning(ex, "Update check timed out");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error while checking for updates");
                return null;
            }
        }

        /// <summary>
        /// Verify the SHA256 hash of a downloaded file.
        /// </summary>
        /// <param name="filePath">Path to the file to verify</param>
        /// <param name="expectedHash">Expected SHA256 hash (hex string)</param>
        /// <returns>True if hash matches, false otherwise</returns>
        public static bool VerifyFileHash(string filePath, string expectedHash)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    using (var stream = System.IO.File.OpenRead(filePath))
                    {
                        var hashBytes = sha256.ComputeHash(stream);
                        var actualHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                        var expectedHashNormalized = expectedHash.Replace("-", "").ToLowerInvariant();

                        bool matches = actualHash.Equals(expectedHashNormalized, StringComparison.OrdinalIgnoreCase);

                        if (matches)
                        {
                            Log.Information("File hash verification passed: {Hash}", actualHash);
                        }
                        else
                        {
                            Log.Warning("File hash mismatch! Expected: {Expected}, Actual: {Actual}",
                                expectedHashNormalized, actualHash);
                        }

                        return matches;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to verify file hash");
                return false;
            }
        }
    }

    /// <summary>
    /// Information about an available update.
    /// </summary>
    public class UpdateInfo
    {
        public bool IsUpdateAvailable { get; set; }
        public string CurrentVersion { get; set; }
        public string LatestVersion { get; set; }
        public string DownloadUrl { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string ReleaseNotes { get; set; }
        public string PublishedAt { get; set; }
        public DateTime CheckedAt { get; set; }

        public string FileSizeFormatted
        {
            get
            {
                if (FileSize < 1024)
                    return $"{FileSize} bytes";
                if (FileSize < 1024 * 1024)
                    return $"{FileSize / 1024.0:F2} KB";
                return $"{FileSize / (1024.0 * 1024.0):F2} MB";
            }
        }
    }
}
