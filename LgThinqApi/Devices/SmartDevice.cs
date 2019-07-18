using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LGThingApi.Structures;

namespace LGThingApi.Devices
{
    public class SmartDevice : Structures.Device, IDisposable
    {
        public delegate void MonitorPollDelegate(object sender, byte[] polledData);
        public event MonitorPollDelegate MonitorPoll;

        private bool monitorRunning = false;

        public SmartDevice(Structures.Device device) : base(device)
        {
            MonitorPoll += PollMonitor;
        }
        public virtual void PollMonitor(object sender, byte[] polledData)
        {
        }
        public async Task StartMonitor(int frequency)
        {
            await Communication.LGGateway.StartMonitoring(this);
            monitorRunning = true;
            _poll(frequency);
        }
        public async Task StopMonitor()
        {
            await Communication.LGGateway.StopMonitoring(this);
            monitorRunning = false;
        }
        private async void _poll(int sleepLenght)
        {
            byte[] res;
            while (monitorRunning)
            {
                res = await Communication.LGGateway.PollMonitor(this);
                if (res != null)
                MonitorPoll(this, res);
                Thread.Sleep(sleepLenght);
            }
        }
        public async void Dispose()
        {
            await StopMonitor();
        }
    }
}
