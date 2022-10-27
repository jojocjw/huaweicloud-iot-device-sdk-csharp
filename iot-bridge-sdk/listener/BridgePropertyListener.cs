using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Client.Requests;

namespace IoT.SDK.Bridge.Listener
{
    public interface BridgePropertyListener
    {
        /// <summary>
        /// 处理写属性操作
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <param name="requestId">请求Id</param>
        /// <param name="services">服务属性列表</param>
        void OnPropertiesSet(string deviceId, string requestId, List<ServiceProperty> services);

        /// <summary>
        /// 处理读属性操作
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <param name="requestId">请求Id</param>
        /// <param name="serviceId">服务id，可选</param>
        void OnPropertiesGet(string deviceId, string requestId, string serviceId);
    }
}
