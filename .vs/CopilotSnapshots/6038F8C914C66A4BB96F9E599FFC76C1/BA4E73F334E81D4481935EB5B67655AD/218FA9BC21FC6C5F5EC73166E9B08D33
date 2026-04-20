using NModbus;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EquipmentMonitor.Services
{
    public class ModbusSensorService : ISensorService
    {
        private TcpClient? client;
        private IModbusMaster? master;

        public bool IsConnected => master != null;

        public ModbusSensorService()
        {
        }

        public async Task ConnectAsync(string host, int port)
        {
            // TCP connect can be done asynchronously
            client = new TcpClient();
            await client.ConnectAsync(host, port).ConfigureAwait(false);
            var factory = new ModbusFactory();
            master = factory.CreateMaster(client);
        }

        public void Disconnect()
        {
            try { client?.Close(); } catch { }
            master = null;
        }

        public Task<double> ReadValueAsync()
        {
            if (master == null) throw new InvalidOperationException("Not connected");
            // Modbus master API is synchronous; run on threadpool to avoid blocking UI
            return Task.Run(() =>
            {
                ushort[] registers = master.ReadHoldingRegisters(1, 0, 1);
                return (double)registers[0];
            });
        }
    }
}
