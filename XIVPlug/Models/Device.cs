using Buttplug.Client;

namespace XIVPlug.Models
{
    public class Device
    {
        public string Name { get; set; }

        /// <inheritdoc />
        public Device(string name)
        {
            Name = name;
        }

        public Device()
        {
        }

        public Device(ButtplugClientDevice device) : this(device.Name)
        {
        }
    }
}