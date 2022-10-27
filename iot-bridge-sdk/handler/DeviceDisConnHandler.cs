using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Transport;
using IoT.SDK.Bridge.Clent;
using IoT.SDK.Device.Utils;
using NLog;

namespace IoT.SDK.Bridge.Handler
{
    class DeviceDisConnHandler : RawMessageListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private BridgeClient bridgeClient;

        public DeviceDisConnHandler(BridgeClient bridgeClient)
        {
            this.bridgeClient = bridgeClient;
        }

        public void OnMessageReceived(RawMessage message)
        {
            Log.Debug("received the message of the device under one bridge disconnects, the  message is {0}", message);
            string deviceId = IotUtil.GetDeviceId(message.Topic);
            if (string.IsNullOrEmpty(deviceId))
            {
                Log.Error("invalid deviceId");
                return;
            }

            if (bridgeClient.bridgeDeviceDisConnListener != null)
            {
                bridgeClient.bridgeDeviceDisConnListener.OnDisConnect(deviceId);
            }

            return;
        }

        public void OnMessagePublished(RawMessage message) { return; }
        public void OnMessageUnPublished(RawMessage message) { return; }
    }
}
