using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Client.Requests;

namespace IoT.SDK.Bridge.Listener
{
    public interface BridgeDeviceMessageListener
    {
        /// <summary>
        /// 处理平台给网桥下发的消息
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <param name="deviceMessage">消息体</param>
        void OnDeviceMessage(string deviceId, DeviceMessage deviceMessage);
    }
}
