using System.Threading.Tasks;

namespace EquipmentMonitor.Services
{
    public interface ISensorService
    {
        Task ConnectAsync(string host, int port);
        void Disconnect();
        Task<double> ReadValueAsync();
        bool IsConnected { get; }
    }
}
