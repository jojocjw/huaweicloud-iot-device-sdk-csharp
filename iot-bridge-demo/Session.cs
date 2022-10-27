using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Client;
using DotNetty.Transport.Channels;
using IoT.SDK.Device.Client.Listener;

namespace IoT.Bridge.Demo
{
    class Session
    {
        public string deviceId { get; set; }

        public string nodeId { get; set; }

        public IChannel channel { get; set; }

        public DeviceClient deviceClient { get; set; }

        public PropertyListener downlinkListener { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Session{");
            sb.Append("nodeId='" + nodeId + '\'');
            sb.Append(", channel=" + channel);
            sb.Append(", deviceId='" + deviceId + '\'');
            sb.Append('}');
            return sb.ToString();
        }
    }
}
