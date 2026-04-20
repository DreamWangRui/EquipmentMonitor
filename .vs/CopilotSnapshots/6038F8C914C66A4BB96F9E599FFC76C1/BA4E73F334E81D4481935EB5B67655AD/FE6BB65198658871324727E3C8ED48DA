using NModbus;
using Serilog;
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
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(host, port).ConfigureAwait(false);
                var factory = new ModbusFactory();
                master = factory.CreateMaster(client);
                Log.Information("Connected to Modbus device {Host}:{Port}", host, port);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to connect to Modbus device {Host}:{Port}", host, port);
                throw;
            }
        }

        public void Disconnect()
        {
            try
            {
                client?.Close();
                master = null;
                Log.Information("Disconnected from Modbus device");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error while disconnecting");
            }
        }

        public Task<double> ReadValueAsync()
        {
            if (master == null) throw new InvalidOperationException("Not connected");
            return Task.Run(() =>
            {
                try
                {
                    ushort[] registers = master.ReadHoldingRegisters(1, 0, 1);
                    double value = registers[0];
                    Log.Debug("Read value {Value} from Modbus", value);
                    return value;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to read from Modbus");
                    throw;
                }
            });
        }
    }
}
