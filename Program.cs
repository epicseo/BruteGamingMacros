using BruteGamingMacros.Core.Utils;
using BruteGamingMacros.UI.Forms;
using System;
using System.Windows.Forms;

namespace BruteGamingMacros.Core
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Initialize production logging and crash reporting first
            ProductionLogger.Initialize();
            CrashReporter.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                using (Container app = new Container())
                {
                    Application.Run(app);
                }
            }
            catch (Exception ex)
            {
                // Log to both systems for redundancy
                DebugLogger.Error("Unhandled exception:\n" + ex.ToString());
                ProductionLogger.Fatal(ex, "Unhandled exception in Main");

                MessageBox.Show("An unexpected error occurred. Please check the logs.", "Application Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DebugLogger.Info("Application exiting...");
                ProductionLogger.Info("Application exiting...");
                ProductionLogger.Shutdown();
                Application.Exit();
            }
        }
    }
}
