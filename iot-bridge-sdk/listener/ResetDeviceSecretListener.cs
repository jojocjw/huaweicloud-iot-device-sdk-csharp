using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.SDK.Bridge.Listener
{
    public interface ResetDeviceSecretListener
    {
        /// <summary>
        /// 网桥设置设备密钥监听器
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <param name="requestId">请求Id</param>
        /// <param name="resultCode">结果码，0表示成功，其他表示失败。不带默认认为成功</param>
        /// <param name="newSecret">设备新secret</param>
        void OnResetDeviceSecret(string deviceId, string requestId, int resultCode, string newSecret);
    }
}
