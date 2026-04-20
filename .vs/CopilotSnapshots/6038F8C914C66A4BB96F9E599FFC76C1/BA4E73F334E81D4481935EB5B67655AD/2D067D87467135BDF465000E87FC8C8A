using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EquipmentMonitor.Models;
using EquipmentMonitor.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace EquipmentMonitor.ViewModels
{
    public class MainViewModel : ObservableObject, IDisposable
    {
        private ObservableCollection<SensorData> _dataList = new ObservableCollection<SensorData>();
        private string _currentValue = "0";
        private bool _isConnected;
        private bool _isCollecting;

        private readonly ISensorService _sensorService;
        private readonly IStorageService _storageService;
        private DispatcherTimer? timer;

        public ObservableCollection<SensorData> DataList
        {
            get => _dataList;
            set => SetProperty(ref _dataList, value);
        }

        public string CurrentValue
        {
            get => _currentValue;
            set => SetProperty(ref _currentValue, value);
        }

        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        public bool IsCollecting
        {
            get => _isCollecting;
            set
            {
                if (SetProperty(ref _isCollecting, value))
                {
                    OnPropertyChanged(nameof(StartButtonText));
                }
            }
        }

        public string StartButtonText => IsCollecting ? "停止采集" : "开始采集";

        public IRelayCommand ConnectCommand { get; }
        public IRelayCommand StartStopCommand { get; }

        // Provide a public parameterless constructor for XAML usage that creates default services
        public MainViewModel() : this(new ModbusSensorService(), new SqliteStorageService()) { }

        public MainViewModel(ISensorService sensorService, IStorageService storageService)
        {
            _sensorService = sensorService ?? throw new ArgumentNullException(nameof(sensorService));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

            ConnectCommand = new RelayCommand(async () => await ConnectAsync());
            StartStopCommand = new RelayCommand(ToggleStartStop);

            // Initialize storage (create DB/table)
            try
            {
                _storageService.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"数据库初始化失败: {ex.Message}");
            }

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += async (s, e) => await Timer_Tick(s, e);
        }

        private async Task ConnectAsync()
        {
            try
            {
                await _sensorService.ConnectAsync("127.0.0.1", 502);
                IsConnected = _sensorService.IsConnected;
                MessageBox.Show("连接成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                IsConnected = false;
            }
        }

        private void ToggleStartStop()
        {
            if (!IsCollecting)
            {
                if (!IsConnected)
                {
                    MessageBox.Show("请先连接设备！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                timer?.Start();
                IsCollecting = true;
            }
            else
            {
                timer?.Stop();
                IsCollecting = false;
            }
        }

        private async Task Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (!_sensorService.IsConnected)
                {
                    MessageBox.Show("请先连接设备！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    timer?.Stop();
                    IsCollecting = false;
                    return;
                }

                double value = await _sensorService.ReadValueAsync();

                CurrentValue = value.ToString("F2");

                var sensorData = new SensorData(value);
                DataList.Insert(0, sensorData);

                try
                {
                    _storageService.Save(sensorData);
                }
                catch (Exception ex)
                {
                    // log or notify
                    Console.WriteLine($"保存到数据库失败: {ex.Message}");
                }

                if (DataList.Count > 50)
                    DataList.RemoveAt(DataList.Count - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                timer?.Stop();
                IsCollecting = false;
            }
        }

        public void Dispose()
        {
            timer?.Stop();
            try { _sensorService.Disconnect(); } catch { }
        }
    }
}
