using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Transport;
using IoT.SDK.Bridge.Clent;
using IoT.SDK.Device.Utils;
using IoT.SDK.Bridge.Response;
using NLog;

namespace IoT.SDK.Bridge.Handler
{
    class SecretResetHandler : RawMessageListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private BridgeClient bridgeClient;

        public SecretResetHandler(BridgeClient bridgeClient)
        {
            this.bridgeClient = bridgeClient;
        }

        public void OnMessageReceived(RawMessage message)
        {
            Log.Debug("received the response of the bridge resets device secret, the  message is {0}", message);
            string requestId = IotUtil.GetRequestId(message.Topic);
            string deviceId = IotUtil.GetDeviceId(message.Topic);
            if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(requestId))
            {
                Log.Error("invalid para");
                return;
            }

            ResetDeviceSecretResponse rsp = JsonUtil.ConvertJsonStringToObject<ResetDeviceSecretResponse>(message.ToString());
            if (rsp == null)
            {
                Log.Warn("invalid response of resetting the device secret.");
                return;
            }

            string newSecret = rsp.paras == null ? null : (string)rsp.paras["new_secret"];
            if (string.IsNullOrEmpty(newSecret))
            {
                Log.Warn("new secret is null.");
                return;
            }
            if (bridgeClient.resetDeviceSecretListener != null)
            {
                bridgeClient.resetDeviceSecretListener.OnResetDeviceSecret(deviceId, requestId, rsp.resultCode, newSecret);
            }

            return;
        }

        public void OnMessagePublished(RawMessage message) { return; }
        public void OnMessageUnPublished(RawMessage message) { return; }
    }
}
