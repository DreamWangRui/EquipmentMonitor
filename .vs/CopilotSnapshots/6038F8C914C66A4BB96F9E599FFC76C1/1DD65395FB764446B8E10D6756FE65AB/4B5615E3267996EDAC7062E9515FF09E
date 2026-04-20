using NModbus;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Threading;

namespace EquipmentMonitor
{
    public partial class MainWindow : Window
    {
        // 数据列表
        private ObservableCollection<SensorData> dataList = new ObservableCollection<SensorData>();

        // Modbus相关 - 添加 ? 表示可为空
        private TcpClient? client;
        private IModbusMaster? master;

        // 定时器
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            // 绑定数据
            listData.ItemsSource = dataList;

            // 初始化定时器
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            // 初始化数据库
            InitializeDatabase();
        }

        // 连接设备按钮点击事件
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 创建TCP连接
                client = new TcpClient("127.0.0.1", 502);
                var factory = new ModbusFactory();
                master = factory.CreateMaster(client);

                MessageBox.Show("连接成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                btnStart.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 开始采集按钮点击事件
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
                btnStart.Content = "停止采集";
            }
            else
            {
                timer.Stop();
                btnStart.Content = "开始采集";
            }
        }

        // 定时器触发
        private void Timer_Tick(object? sender, EventArgs e)  // 添加 ? 表示参数可为空
        {
            try
            {
                // 检查master是否已初始化
                if (master == null)
                {
                    MessageBox.Show("请先连接设备！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    timer.Stop();
                    btnStart.Content = "开始采集";
                    return;
                }

                // 读取保持寄存器
                ushort[] registers = master.ReadHoldingRegisters(1, 0, 1);
                double value = registers[0];

                // 更新界面
                txtCurrentValue.Text = value.ToString("F2");

                // 添加到数据列表
                var sensorData = new SensorData(value);
                dataList.Insert(0, sensorData);

                // 保存到数据库
                SaveToDatabase(sensorData);

                // 限制显示数量
                if (dataList.Count > 50)
                    dataList.RemoveAt(dataList.Count - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                timer.Stop();
                btnStart.Content = "开始采集";
            }
        }

        // 初始化数据库
        private void InitializeDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection("Data Source=sensor.db"))
                {
                    connection.Open();
                    string createTable = @"
                        CREATE TABLE IF NOT EXISTS SensorData (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Value REAL NOT NULL,
                            Time DATETIME NOT NULL
                        )";
                    new SQLiteCommand(createTable, connection).ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"数据库初始化失败: {ex.Message}");
            }
        }

        // 保存数据到数据库
        private void SaveToDatabase(SensorData data)
        {
            try
            {
                using (var connection = new SQLiteConnection("Data Source=sensor.db"))
                {
                    connection.Open();
                    string insert = "INSERT INTO SensorData (Value, Time) VALUES (@value, @time)";
                    var command = new SQLiteCommand(insert, connection);
                    command.Parameters.AddWithValue("@value", data.Value);
                    command.Parameters.AddWithValue("@time", data.Time);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存到数据库失败: {ex.Message}");
            }
        }

        // 窗口关闭时清理资源
        protected override void OnClosed(EventArgs e)
        {
            timer?.Stop();
            client?.Close();
            base.OnClosed(e);
        }
    }
}