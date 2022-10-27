using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using IoT.SDK.Device.Utils;
using NLog;

// 默认的设备标识管理器，从文件中读取设备标识信息。用于可以自定义DeviceIdentityRegistry类进行替换
namespace IoT.Bridge.Demo
{
    class DefaultDeviceIdentityRegistry : DeviceIdentityRegistry
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private Dictionary<string, Dictionary<string, string>> deviceIdentityMap;

        public DefaultDeviceIdentityRegistry()
        {
            string content = null;
            try {
                string contents = File.ReadAllText("deviceIdentity.json", Encoding.UTF8);
            } catch (Exception ex) {
                Log.Error("get the file of DeviceIdIdentity failed : " + ex.ToString());
            }
            deviceIdentityMap = JsonUtil.ConvertJsonStringToObject<Dictionary<string, Dictionary<string, string>>>(content);
        }

        public DeviceIdentity GetDeviceIdentity(string nodeId)
        {

            Dictionary<string, string> map = deviceIdentityMap[nodeId];

            string json = JsonUtil.ConvertObjectToJsonString(map);

            return JsonUtil.ConvertJsonStringToObject<DeviceIdentity>(json);
    }
}
}
