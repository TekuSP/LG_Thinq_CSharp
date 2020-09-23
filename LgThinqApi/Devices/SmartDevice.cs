using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LGThingApi.Structures;

namespace LGThingApi.Devices
{
    /// <summary>
    /// SmartDevice base class from which should be all devices derived, inherits Device
    /// </summary>
    public class SmartDevice : Device
    {
        public delegate void MonitorPollDelegate(object sender, byte[] polledData);
        public event MonitorPollDelegate MonitorPoll;

        private bool monitorRunning = false;
        /// <summary>
        /// Creates SmartDevice based on JSON parsed Device
        /// </summary>
        /// <param name="device">Device to copy information from</param>
        public SmartDevice(Device device) : base(device)
        {
            MonitorPoll += PollMonitor;
        }
        /// <summary>
        /// Destructs this object and makes sure to stop Monitor
        /// </summary>
        ~SmartDevice()
        {
            var task = StopMonitor();
            task.Wait();
        }

        /// <summary>
        /// This method is called every time Monitor has data and is polled
        /// Should be overriden for each device
        /// </summary>
        /// <param name="sender">Sender who called</param>
        /// <param name="polledData">Data which were polled</param>
        public virtual void PollMonitor(object sender, byte[] polledData)
        {
        }
        /// <summary>
        /// Starts monitoring and polls data based on frequency in milliseconds
        /// </summary>
        /// <param name="frequency">Milliseconds how often to poll</param>
        /// <returns></returns>
        public async Task StartMonitor(int frequency)
        {
            await Communication.LGGateway.StartMonitoring(this);
            monitorRunning = true;
            _ = Task.Run(new System.Action(() => { Poll(frequency); }));
        }
        /// <summary>
        /// Stops monitoring and ends task
        /// </summary>
        /// <returns></returns>
        public async Task StopMonitor()
        {
            monitorRunning = false;
            await Communication.LGGateway.StopMonitoring(this);
        }
        /// <summary>
        /// Asnychronous task to Poll data from LG
        /// </summary>
        /// <param name="sleepLenght">How often to poll in milliseconds</param>
        private async void Poll(int sleepLenght)
        {
            byte[] res;
            for (int i = 0; i < 10; i++) //Initial data polling
            {
                if (monitorRunning)
                {
                    res = await Communication.LGGateway.PollMonitor(this);
                    if (res != null)
                        MonitorPoll(this, res);
                    Thread.Sleep(300);
                }
            }
            while (monitorRunning)
            {
                res = await Communication.LGGateway.PollMonitor(this);
                if (res != null)
                MonitorPoll(this, res);
                Thread.Sleep(sleepLenght);
            }
        }
    }
}
