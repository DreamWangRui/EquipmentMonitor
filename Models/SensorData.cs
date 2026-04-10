using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EquipmentMonitor.Models
{
    /// <summary>
    /// 传感器数据模型
    /// </summary>
    public class SensorData : INotifyPropertyChanged
    {
        private int _id;
        private DateTime _timestamp;
        private double _value;
        private string _tagName = "Temperature";
        private string _unit = "°C";

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp
        {
            get => _timestamp;
            set
            {
                if (_timestamp != value)
                {
                    _timestamp = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 数值
        /// </summary>
        public double Value
        {
            get => _value;
            set
            {
                if (Math.Abs(_value - value) > 0.001) // 避免浮点数精度问题
                {
                    _value = value;
                    OnPropertyChanged();
                    // 同时更新格式化字符串
                    OnPropertyChanged(nameof(ValueFormatted));
                }
            }
        }

        /// <summary>
        /// 格式化后的数值（保留2位小数）
        /// </summary>
        public string ValueFormatted => Value.ToString("F2");

        /// <summary>
        /// 数据点标签名称
        /// </summary>
        public string TagName
        {
            get => _tagName;
            set
            {
                if (_tagName != value)
                {
                    _tagName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get => _unit;
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SensorData()
        {
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 原有构造函数（保持兼容）
        /// </summary>
        public SensorData(double value) : this()
        {
            Value = value;
        }

        /// <summary>
        /// 新构造函数
        /// </summary>
        public SensorData(double value, string tagName, string unit = "°C") : this()
        {
            Value = value;
            TagName = tagName;
            Unit = unit;
        }

        /// <summary>
        /// 完整的构造函数
        /// </summary>
        public SensorData(int id, DateTime timestamp, double value, string tagName, string unit)
        {
            Id = id;
            Timestamp = timestamp;
            Value = value;
            TagName = tagName;
            Unit = unit;
        }

        /// <summary>
        /// 属性变更通知
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 重写ToString，便于调试
        /// </summary>
        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] {TagName}: {Value:F2} {Unit}";
        }
    }
}