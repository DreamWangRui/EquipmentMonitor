using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentMonitor
{
    public class SensorData
    {
        public DateTime Time { get; set; }
        public double Value { get; set; }

        public SensorData(double value)
        {
            Time = DateTime.Now;
            Value = value;
        }
    }
}