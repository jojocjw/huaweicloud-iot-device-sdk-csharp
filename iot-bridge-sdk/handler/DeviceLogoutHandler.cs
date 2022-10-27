using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Transport;
using IoT.SDK.Bridge.Clent;
using IoT.SDK.Device.Utils;
using NLog;

namespace IoT.SDK.Bridge.Handler
{
    class DeviceLogoutHandler : RawMessageListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private BridgeClient bridgeClient;

        public DeviceLogoutHandler(BridgeClient bridgeClient)
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
            Dictionary<string, object> dict = JsonUtil.ConvertJsonStringToObject<Dictionary<string, object>>(message.ToString());
            if (dict == null)
            {
                Log.Warn("the response of device logout is invalid.");
                return;
            }
            int resultCode = (int)dict["result_code"];

            if (bridgeClient.logoutListener != null)
            {
                bridgeClient.logoutListener.OnLogout(deviceId, requestId, dict);
                return;
            }

            var future = bridgeClient.requestIdCache.GetFuture(requestId);
            if (future != null)
            {
                future.SetResult(resultCode);
            }
        }
    
        public void OnMessagePublished(RawMessage message) { return; }
        public void OnMessageUnPublished(RawMessage message) { return; }
    }
}
