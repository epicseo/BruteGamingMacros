using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BruteGamingMacros.Core.Utils
{
    /// <summary>
    /// Handles unhandled exceptions and provides crash reporting functionality.
    /// </summary>
    public static class CrashReporter
    {
        private static bool _initialized = false;
        private static readonly object _crashLock = new object();

        /// <summary>
        /// Initialize crash reporting. Call this early in application startup.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized)
                return;

            // Handle unhandled exceptions on UI thread
            Application.ThreadException += OnThreadException;

            // Handle unhandled exceptions on non-UI threads
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            // Handle Task exceptions
            AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;

            _initialized = true;
            Log.Information("CrashReporter initialized");
        }

        private static void OnFirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            // Log first-chance exceptions at debug level only (noisy)
            // These happen frequently and are often caught
            Log.Debug("First-chance exception: {ExceptionType} - {Message}",
                e.Exception.GetType().Name, e.Exception.Message);
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleCrash(e.Exception, "UI Thread Exception", isTerminating: false);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            HandleCrash(exception, "Unhandled Exception", e.IsTerminating);
        }

        /// <summary>
        /// Handle a crash by logging it and optionally showing a crash dialog.
        /// </summary>
        private static void HandleCrash(Exception ex, string type, bool isTerminating)
        {
            if (ex == null)
                return;

            lock (_crashLock)
            {
                try
                {
                    // Log to Serilog
                    Log.Fatal(ex, "{Type} - Application {Status}",
                        type, isTerminating ? "will terminate" : "may continue");

                    // Create crash dump file
                    WriteCrashDump(ex, type);

                    // Show user-friendly crash dialog
                    if (isTerminating)
                    {
                        ShowCrashDialog(ex);
                    }
                }
                catch (Exception crashEx)
                {
                    // Last-resort fallback
                    Console.WriteLine($"[CRITICAL] Crash reporter failed: {crashEx.Message}");

                    try
                    {
                        // Try to at least write to a file
                        File.WriteAllText(
                            Path.Combine(Path.GetTempPath(), "BGM_EMERGENCY_CRASH.txt"),
                            $"Original Crash:\n{ex}\n\nCrash Reporter Error:\n{crashEx}"
                        );
                    }
                    catch
                    {
                        // Give up
                    }
                }
            }
        }

        /// <summary>
        /// Write detailed crash information to a crash dump file.
        /// </summary>
        private static void WriteCrashDump(Exception ex, string type)
        {
            try
            {
                string crashDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "BruteGamingMacros",
                    "Crashes"
                );

                Directory.CreateDirectory(crashDir);

                string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                string crashFile = Path.Combine(crashDir, $"crash-{timestamp}.txt");

                var sb = new StringBuilder();
                sb.AppendLine("=== BRUTE GAMING MACROS CRASH REPORT ===");
                sb.AppendLine();
                sb.AppendLine($"Crash Type: {type}");
                sb.AppendLine($"Crash Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Version: {ProductionLogger.GetAppVersion()}");
                sb.AppendLine();
                sb.AppendLine("=== SYSTEM INFORMATION ===");
                sb.AppendLine($"OS: {Environment.OSVersion}");
                sb.AppendLine($"CLR: {Environment.Version}");
                sb.AppendLine($"64-bit Process: {Environment.Is64BitProcess}");
                sb.AppendLine($"Processor Count: {Environment.ProcessorCount}");
                sb.AppendLine($"Working Set: {Environment.WorkingSet:N0} bytes");
                sb.AppendLine($"Machine Name: {Environment.MachineName}");
                sb.AppendLine();
                sb.AppendLine("=== EXCEPTION DETAILS ===");
                sb.AppendLine($"Type: {ex.GetType().FullName}");
                sb.AppendLine($"Message: {ex.Message}");
                sb.AppendLine();
                sb.AppendLine("Stack Trace:");
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine();

                // Include inner exceptions
                if (ex.InnerException != null)
                {
                    sb.AppendLine("=== INNER EXCEPTION ===");
                    var inner = ex.InnerException;
                    int depth = 1;
                    while (inner != null && depth <= 5)
                    {
                        sb.AppendLine($"[{depth}] {inner.GetType().FullName}: {inner.Message}");
                        sb.AppendLine(inner.StackTrace);
                        sb.AppendLine();
                        inner = inner.InnerException;
                        depth++;
                    }
                }

                // Additional debug info
                sb.AppendLine("=== ADDITIONAL INFO ===");
                sb.AppendLine($"Current Directory: {Environment.CurrentDirectory}");
                sb.AppendLine($"Command Line: {Environment.CommandLine}");
                sb.AppendLine($"Tick Count: {Environment.TickCount}");
                sb.AppendLine();
                sb.AppendLine("=== END OF CRASH REPORT ===");

                File.WriteAllText(crashFile, sb.ToString(), Encoding.UTF8);

                Log.Information("Crash dump written to: {CrashFile}", crashFile);
            }
            catch (Exception dumpEx)
            {
                Log.Error(dumpEx, "Failed to write crash dump");
            }
        }

        /// <summary>
        /// Show a user-friendly crash dialog.
        /// </summary>
        private static void ShowCrashDialog(Exception ex)
        {
            try
            {
                string message = $"Brute Gaming Macros has encountered a critical error and needs to close.\n\n" +
                                $"Error Type: {ex.GetType().Name}\n" +
                                $"Error Message: {ex.Message}\n\n" +
                                $"A crash report has been saved to:\n" +
                                $"%LOCALAPPDATA%\\BruteGamingMacros\\Crashes\\\n\n" +
                                $"Please report this issue on GitHub:\n" +
                                $"https://github.com/epicseo/BruteGamingMacros/issues\n\n" +
                                $"Include the crash report file and steps to reproduce.";

                MessageBox.Show(
                    message,
                    "Application Crash - Brute Gaming Macros",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch
            {
                // If even the crash dialog fails, give up gracefully
            }
        }
    }
}
