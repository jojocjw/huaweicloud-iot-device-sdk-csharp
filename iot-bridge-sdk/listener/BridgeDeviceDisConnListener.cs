using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.SDK.Bridge.Listener
{
    public interface BridgeDeviceDisConnListener
    {
        /// <summary>
        /// 网桥设备断链通知处理
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        void OnDisConnect(string deviceId);
    }
}
