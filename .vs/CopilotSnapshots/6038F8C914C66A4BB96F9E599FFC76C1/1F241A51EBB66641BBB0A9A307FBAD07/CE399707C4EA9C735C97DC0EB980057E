using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using EquipmentMonitor.Services;
using EquipmentMonitor.ViewModels;

namespace EquipmentMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // register services
                    services.AddSingleton<ISensorService, ModbusSensorService>();
                    services.AddSingleton<IStorageService, SqliteStorageService>();

                    // register viewmodels
                    services.AddSingleton<MainViewModel>();

                    // register MainWindow and let DI inject DataContext if needed
                    services.AddSingleton<MainWindow>(sp =>
                    {
                        var window = new MainWindow();
                        window.DataContext = sp.GetRequiredService<MainViewModel>();
                        return window;
                    });
                })
                .Build();

            // Start host
            _host.Start();

            // Show MainWindow
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }
            base.OnExit(e);
        }
    }
}
