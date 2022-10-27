using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Bridge.Request;

namespace IoT.SDK.Bridge.Listener
{
    public interface BridgeCommandListener
    {
        /// <summary>
        /// 网桥命令处理
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <param name="requestId">请求Id</param>
        /// <param name="bridgeCommand">网桥命令</param>
        void OnCommand(string deviceId, string requestId, BridgeCommand bridgeCommand);
    }
}
