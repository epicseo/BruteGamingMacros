using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;

namespace BruteGamingMacros.Core.Utils
{
    /// <summary>
    /// Production-grade logging using Serilog with file rotation and structured logging.
    /// This replaces the basic DebugLogger for production deployments.
    /// </summary>
    public static class ProductionLogger
    {
        private static bool _initialized = false;
        private static readonly object _initLock = new object();

        /// <summary>
        /// Initialize the production logger. Call this once at application startup.
        /// </summary>
        public static void Initialize()
        {
            lock (_initLock)
            {
                if (_initialized)
                    return;

                try
                {
                    // Get application version
                    string version = GetAppVersion();

                    // Configure log directory
                    string logDir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "BruteGamingMacros",
                        "Logs"
                    );

                    // Ensure directory exists
                    Directory.CreateDirectory(logDir);

                    string logPath = Path.Combine(logDir, "app-.log");

                    // Configure Serilog
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .Enrich.WithThreadId()
                        .Enrich.WithMachineName()
                        .Enrich.WithProperty("AppVersion", version)
                        .WriteTo.File(
                            logPath,
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 7,
                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [T{ThreadId}] {Message:lj}{NewLine}{Exception}",
                            shared: true
                        )
                        .CreateLogger();

                    _initialized = true;

                    Log.Information("=== Brute Gaming Macros v{Version} Started ===", version);
                    Log.Information("Operating System: {OS}", Environment.OSVersion);
                    Log.Information("CLR Version: {CLR}", Environment.Version);
                    Log.Information("Working Directory: {WorkDir}", Environment.CurrentDirectory);
                    Log.Information("Machine Name: {Machine}", Environment.MachineName);
                    Log.Information("User: {User}", Environment.UserName);
                    Log.Information("64-bit Process: {Is64Bit}", Environment.Is64BitProcess);
                    Log.Information("Log directory: {LogDir}", logDir);
                }
                catch (Exception ex)
                {
                    // Fallback to console if Serilog fails
                    Console.WriteLine($"[CRITICAL] Failed to initialize ProductionLogger: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);

                    // Keep DebugLogger as backup
                    DebugLogger.Error(ex, "ProductionLogger initialization failed");
                }
            }
        }

        /// <summary>
        /// Get the application version from assembly.
        /// </summary>
        public static string GetAppVersion()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Shutdown the logger and flush all pending logs.
        /// Call this before application exit.
        /// </summary>
        public static void Shutdown()
        {
            if (!_initialized)
                return;

            try
            {
                Log.Information("=== Application Shutdown ===");
                Log.CloseAndFlush();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to shutdown ProductionLogger: {ex.Message}");
            }
        }

        /// <summary>
        /// Enable or disable debug-level logging at runtime.
        /// </summary>
        public static void SetDebugMode(bool enabled)
        {
            if (!_initialized)
                return;

            var minLevel = enabled ? LogEventLevel.Debug : LogEventLevel.Information;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(minLevel)
                .WriteTo.Logger(Log.Logger)
                .CreateLogger();

            Log.Information("Debug mode {Status}", enabled ? "enabled" : "disabled");
        }

        // Convenience methods for backward compatibility with DebugLogger
        public static void Info(string message) => Log.Information(message);
        public static void Warning(string message) => Log.Warning(message);
        public static void Error(string message) => Log.Error(message);
        public static void Error(Exception ex, string message) => Log.Error(ex, message);
        public static void Debug(string message) => Log.Debug(message);
        public static void Fatal(Exception ex, string message) => Log.Fatal(ex, message);
    }
}
