using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApiClientServiceUpdater
{
    public class Worker : BackgroundService
    {
        private readonly string serviceName = "WebApiClientService";
        private readonly string exePath = @"C:\QaTools\WebApiClientService\WebApiClientService.exe";
        private readonly string updatePath = @"C:\QaTools\WebApiClientServiceUpdates";
        private readonly string triggerFile = "updateNow.txt";
        private readonly string triggerFilePath;
        private readonly string newExePath;
        private readonly ILogger<Worker> _logger;
        private FileSystemWatcher watcher;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            triggerFilePath = Path.Combine(updatePath, triggerFile);
            newExePath = Path.Combine(updatePath, Path.GetFileName(exePath));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started. Monitoring update directory.");
            watcher = new FileSystemWatcher(updatePath)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                Filter = triggerFile
            };
            watcher.Created += OnUpdateTriggered; // watch if the file is created only (ignore the change for now) : watcher.Changed += OnUpdateTriggered;  
            watcher.EnableRaisingEvents = true;

            await Task.CompletedTask;
        }

        private void OnUpdateTriggered(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("Update trigger file detected. Initiating update process.");
            Thread.Sleep(5000); // Allow file write operations to complete
            UpdateService();
            EventLog.WriteEntry("WebApiClientServiceUpdater", $"Updates detected for {serviceName}");
        }

        private void UpdateService()
        {
            try
            {
                _logger.LogInformation("Stopping service {ServiceName} for update.", serviceName);
                ServiceController sc = new ServiceController(serviceName);
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                }
                EventLog.WriteEntry("WebApiClientServiceUpdater", $"Stopping {serviceName}");

                if (File.Exists(newExePath))
                {
                    _logger.LogInformation("Replacing old executable with new version.");
                    File.Copy(newExePath, exePath, true);
                }
                EventLog.WriteEntry("WebApiClientServiceUpdater", $"Updating {serviceName}");

                sc.Start();
                _logger.LogInformation("Service {ServiceName} restarted successfully.", serviceName);
                EventLog.WriteEntry("WebApiClientServiceUpdater", $"Starting {serviceName}");

                CleanupUpdateFolder(updatePath);
                EventLog.WriteEntry("WebApiClientServiceUpdater", $"Cleaning updates folder");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during the update process.");
                EventLog.WriteEntry("WebApiClientServiceUpdater", $"Error: {ex.Message}", EventLogEntryType.Error);
            }
        }

        private void CleanupUpdateFolder(string updateFolder)
        {
            try
            {
                if (Directory.Exists(updateFolder))
                {
                    foreach (string file in Directory.GetFiles(updateFolder))
                    {
                        File.Delete(file);
                    }
                    foreach (string dir in Directory.GetDirectories(updateFolder))
                    {
                        Directory.Delete(dir, true);
                    }
                }
                _logger.LogInformation("Update folder cleaned up successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cleanup error: {Message}", ex.Message);
                EventLog.WriteEntry("WebApiClientServiceUpdater", $"Cleanup error: {ex.Message}", EventLogEntryType.Warning);
            }
        }
    }
}
