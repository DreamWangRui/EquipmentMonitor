using EquipmentMonitor.Models;
using System.Threading.Tasks;

namespace EquipmentMonitor.Services
{
    public interface IDataService
    {
        Task SaveAsync(SensorData data);
    }
}
