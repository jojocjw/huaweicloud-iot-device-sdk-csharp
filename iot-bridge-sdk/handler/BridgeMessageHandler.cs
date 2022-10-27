using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Transport;
using IoT.SDK.Bridge.Clent;
using IoT.SDK.Device.Utils;
using IoT.SDK.Device.Client.Requests;
using NLog;

namespace IoT.SDK.Bridge.Handler
{
    class BridgeMessageHandler : RawMessageListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private BridgeClient bridgeClient;

        public BridgeMessageHandler(BridgeClient bridgeClient)
        {
            this.bridgeClient = bridgeClient;
        }

        public void OnMessageReceived(RawMessage message)
        {
            string topic = message.Topic;
            DeviceMessage deviceMsg = new DeviceMessage();
            deviceMsg.content = message.ToString();
            if (!topic.Contains("$oc/bridges/"))
            {
                Log.Error("invalid topic");
                return;
            }

            string deviceId = IotUtil.GetDeviceId(topic);
            if (string.IsNullOrEmpty(deviceId))
            {
                return;
            }

            if (bridgeClient.bridgeDeviceMessageListener != null)
            {
                bridgeClient.bridgeDeviceMessageListener.OnDeviceMessage(deviceId, deviceMsg);
            }

            return;
        }

        public void OnMessagePublished(RawMessage message) { return; }
        public void OnMessageUnPublished(RawMessage message) { return; }
    }
}
