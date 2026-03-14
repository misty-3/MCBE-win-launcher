using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace MCLauncher {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            
            // Setup error handling
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = args.ExceptionObject as Exception;
                MessageBox.Show(
                    $"A critical error occurred:\n\n{ex?.Message}\n\nCheck Log.txt for details.",
                    "Startup Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            };

            DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show(
                    $"An error occurred:\n\n{args.Exception.Message}\n\nCheck Log.txt for details.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                args.Handled = true;
            };

            try
            {
                // Setup AppData directory - using Local instead of Roaming for game files
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "MinecraftVersionLauncher");
                
                if (!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }

                // Change working directory to AppData
                Directory.SetCurrentDirectory(appDataPath);

                Debug.Listeners.Add(new TextWriterTraceListener("Log.txt"));
                Debug.AutoFlush = true;
                Debug.WriteLine("=== Application Starting ===");
                Debug.WriteLine($"Time: {DateTime.Now}");
                Debug.WriteLine($"OS: {Environment.OSVersion}");
                Debug.WriteLine($"AppData: {appDataPath}");

                // Launch main window directly (no wizard)
                Debug.WriteLine("Creating main window");
                var mainWindow = new ModernMainWindow();
                Debug.WriteLine("Showing main window");
                mainWindow.Show();
                Debug.WriteLine("Main window shown successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FATAL ERROR: {ex}");
                MessageBox.Show(
                    $"Failed to start the application:\n\n{ex.Message}\n\nDetails have been written to Log.txt",
                    "Startup Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown();
            }
        }

    }
}
