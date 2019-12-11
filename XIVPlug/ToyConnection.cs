using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Web;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using XIVPlug.Models;

namespace XIVPlug
{
    public class ToyConnection
    {
        private ButtplugWebsocketConnector connector = null;
        private ButtplugClient client = null;

        private string selectedDeviceName;

        public string SelectedDeviceName
        {
            get => selectedDeviceName;
            set
            {
                selectedDeviceName = value;

                // Clear the queue so the old device doesn't keep going
                ClearQueue();
            }
        }

        private readonly BackgroundWorker worker;
        private readonly ConcurrentQueue<Command> commandQueue;

        private double baselineVibrationValue { get; set; }

        public double BaselineVibrationValue
        {
            get => baselineVibrationValue;
            set
            {
                baselineVibrationValue = value;

                if (!worker.IsBusy)
                {
                    SelectedDevice?.SendVibrateCmd(value);
                }
            }
        }

        public ButtplugClient GetClientInstance()
        {
            if (connector == null)
            {
                connector = new ButtplugWebsocketConnector(new Uri("ws://localhost:12345"));
            }

            return client ?? (client = new ButtplugClient("XIVPlug", connector));
        }

        public bool IsDeviceSelected => SelectedDevice != null;

        public ButtplugClientDevice SelectedDevice =>
            client.Devices.FirstOrDefault(d => string.Equals(d.Name, SelectedDeviceName));

        private ToyConnection()
        {
            worker = new BackgroundWorker();
            commandQueue = new ConcurrentQueue<Command>();

            worker.WorkerSupportsCancellation = true;
            worker.DoWork += ProcessQueueEvents;
        }

        public void Enqueue(Command command)
        {
            try
            {
                commandQueue.Enqueue(command);

                if (!worker.IsBusy)
                {
                    worker.RunWorkerAsync();
                }
            }
            catch (Exception)
            {
            }
        }

        public void ClearQueue()
        {
            try
            {
                worker.CancelAsync();
            }
            catch (Exception)
            {
            }
        }

        private void ProcessQueueEvents(object sender, DoWorkEventArgs args)
        {
            try
            {
                while (commandQueue.TryDequeue(out var command))
                {
                    switch (command.Type)
                    {
                        case CommandType.Vibrate:
                            SelectedDevice?.SendVibrateCmd(command.Value.Clamp(0.0, 1.0));
                            Thread.Sleep(command.Duration);
                            SelectedDevice?.SendVibrateCmd(0.0);
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private static ToyConnection instance;

        public static ToyConnection GetInstance()
        {
            if (instance == null)
            {
                instance = new ToyConnection();
            }

            return instance;
        }
    }
}