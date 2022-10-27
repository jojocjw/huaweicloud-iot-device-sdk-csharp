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
    class BridgePropertySetHandler : RawMessageListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private BridgeClient bridgeClient;

        public BridgePropertySetHandler(BridgeClient bridgeClient)
        {
            this.bridgeClient = bridgeClient;
        }

        public void OnMessageReceived(RawMessage message)
        {
            PropsSet propsSet = JsonUtil.ConvertJsonStringToObject<PropsSet>(message.ToString());
            if (propsSet == null)
            {
                Log.Error("invalid property setting");
                return;
            }
            string topic = message.Topic;
            if (!topic.Contains("$oc/bridges/"))
            {
                Log.Error("invalid topic");
                return;
            }

            string deviceId = IotUtil.GetDeviceId(topic);
            string requestId = IotUtil.GetRequestId(topic);
            if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(requestId))
            {
                Log.Error("invalid para");
                return;
            }

            if (bridgeClient.bridgePropertyListener != null)
            {
                bridgeClient.bridgePropertyListener.OnPropertiesSet(deviceId, requestId, propsSet.services);
            }

            return;
        }

        public void OnMessagePublished(RawMessage message) { return; }
        public void OnMessageUnPublished(RawMessage message) { return; }
    }
}
