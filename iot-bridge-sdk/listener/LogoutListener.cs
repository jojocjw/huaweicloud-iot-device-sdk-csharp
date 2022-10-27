using System.Collections.Generic;
using System.Text;

namespace IoT.SDK.Bridge.Listener
{
    public interface LogoutListener
    {
        /// <summary>
        /// 网桥下设备登出监听器
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <param name="requestId">请求Id</param>
        /// <param name="map">响应体</param>
        void OnLogout(string deviceId, string requestId, Dictionary<string, object> map);
    }
}
