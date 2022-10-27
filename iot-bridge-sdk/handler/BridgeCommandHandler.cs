using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Transport;
using IoT.SDK.Bridge.Clent;
using IoT.SDK.Device.Utils;
using IoT.SDK.Device.Client.Requests;
using IoT.SDK.Bridge.Request;
using NLog;

namespace IoT.SDK.Bridge.Handler
{
    class BridgeCommandHandler : RawMessageListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private BridgeClient bridgeClient;

        public BridgeCommandHandler(BridgeClient bridgeClient)
        {
            this.bridgeClient = bridgeClient;
        }

        public void OnMessageReceived(RawMessage message)
        {
            string topic = message.Topic;
            string requestId = IotUtil.GetRequestId(topic);
            Command command = JsonUtil.ConvertJsonStringToObject<Command>(message.ToString());
            if (command == null)
            {
                Log.Error("invalid command");
                return;
            }
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
            if (bridgeClient.bridgeCommandListener != null)
            {
                BridgeCommand cmd = new BridgeCommand();
                cmd.command = command;
                bridgeClient.bridgeCommandListener.OnCommand(deviceId, requestId, cmd);
            }

            return;
        }

        public void OnMessagePublished(RawMessage message) { return; }
        public void OnMessageUnPublished(RawMessage message) { return; }
    }
}
