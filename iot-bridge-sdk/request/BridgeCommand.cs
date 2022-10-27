using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Client.Requests;

// 网桥设备命令
namespace IoT.SDK.Bridge.Request
{
    public class BridgeCommand
    {
        private string deviceId { get; set; }

        public Command command { get; set; }

        // Override
        public override string ToString()
        {
            return "BridgeCommand{" + "deviceId='" + deviceId + '\'' + ", command=" + command + '}';
        }
    }
}
