using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;

namespace XIVPlug
{
    public class ToyConnection
    {
        private static ButtplugWebsocketConnector connector = null;
        private static ButtplugClient client = null;
        public static string SelectedDeviceName { get; set; }

        public static ButtplugClient GetInstance()
        {
            if (connector == null)
            {
                connector = new ButtplugWebsocketConnector(new Uri("ws://localhost:12345"));
            }

            if (client == null)
            {
                client = new ButtplugClient("XIVPlug", connector);
            }

            return client;
        }

        public static bool IsDeviceSelected => SelectedDevice != null;

        public static ButtplugClientDevice SelectedDevice =>
            client.Devices.FirstOrDefault(d => string.Equals(d.Name, SelectedDeviceName));
    }
}