using EquipmentMonitor.Models;

namespace EquipmentMonitor.Services
{
    public interface IStorageService
    {
        void Initialize();
        void Save(SensorData data);
    }
}
