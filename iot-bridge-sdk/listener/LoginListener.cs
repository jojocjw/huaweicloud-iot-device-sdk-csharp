using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.SDK.Bridge.Listener
{
    public interface LoginListener
    {
        /// <summary>
        /// 网桥下设备登录监听器
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <param name="requestId">请求Id</param>
        /// <param name="resultCode">登录响应码</param>
        void OnLogin(string deviceId, string requestId, int resultCode);
    }
}
