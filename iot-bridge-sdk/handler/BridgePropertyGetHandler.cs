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
    class BridgePropertyGetHandler : RawMessageListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private BridgeClient bridgeClient;

        public BridgePropertyGetHandler(BridgeClient bridgeClient)
        {
            this.bridgeClient = bridgeClient;
        }

        public void OnMessageReceived(RawMessage message)
        {
            PropsGet propsGet = JsonUtil.ConvertJsonStringToObject<PropsGet>(message.ToString());
            if (propsGet == null)
            {
                Log.Error("invalid property getting");
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
            string serviceId = propsGet.serviceId;
            if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(requestId) || string.IsNullOrEmpty(serviceId))
            {
                Log.Error("invalid para");
                return;
            }

            if (bridgeClient.bridgePropertyListener != null)
            {
                bridgeClient.bridgePropertyListener.OnPropertiesGet(deviceId, requestId, serviceId);
            }

            return;
        }

        public void OnMessagePublished(RawMessage message) { return; }
        public void OnMessageUnPublished(RawMessage message) { return; }
    }
}
