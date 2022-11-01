using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Client;
using IoT.SDK.Bridge.Device;
using IoT.SDK.Device.Client.Requests;
using IoT.SDK.Bridge.Listener;
using IoT.SDK.Bridge.Request;
using IoT.SDK.Device.Filemanager;
using IoT.SDK.Device.Filemanager.Request;
using System.Threading;
using NLog;

namespace IoT.Bridge.Demo
{
    class BridgeSample : BridgeCommandListener, BridgeDeviceMessageListener, ResetDeviceSecretListener, LogoutListener, BridgeDeviceDisConnListener, BridgePropertyListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // iot平台连接地址
        private static readonly string SERVER_URI = "iot-mqtts.cn-north-5.myhuaweicloud.com";

        // 网桥设备Id（需要提前在iot平台上注册）
        private static readonly string BRIDGE_ID = "bridge004";

        // 设备密钥
        private static readonly string BRIDGE_SECRET = "bridge004";

        // 网桥下的设备Id
        private static readonly string DEVICE_ID = "635b8da293d99934f4be3836_csharpStudent";

        // 网桥下设备的密钥
        private static readonly string DEVICE_SECRET = "123456789";

        // 设备新的密钥，重置设备密钥时使用
        private static readonly string NEW_DEVICE_SECRET = "123456789";

        // 服务Id，需要跟物模型中设置的一致
        private static readonly string SERVICE_ID = "Location";

        // 属性，需要跟物模型中设置的一致。
        private static readonly string PROPERTY = "longitude";

        // 消息名称， 建议每条消息不一致，用于消息上报。
        private static readonly string MESSAGE_NAME = "messageName";

        // 消息Id， 建议每条消息不一致，用于消息上报。
        private static readonly string MESSAGE_ID = " messageId";

        // 消息内容， 建议每条消息不一致，用于消息上报。
        private static readonly string MESSAGE_CONTENT = "messageContent";

        // 文件哈希值
        private static readonly string HASH_CODE = "58059181f378062f9b446e884362a526";

        // 文件名称，此处样例填写a.jpg。
        private static readonly string FILE_NAME = "a.jpg";

        // 文件大小，此处样例填写1024。
        private static readonly int SIZE = 1024;

        private BridgeDevice bridgeDevice;
        public BridgeSample()
        {
            ClientConf clientConf = new ClientConf();
            clientConf.ServerUri = SERVER_URI;
            clientConf.Port = 8884;
            clientConf.DeviceId = BRIDGE_ID;
            clientConf.Secret = BRIDGE_SECRET;
            clientConf.Mode = ClientConf.CONNECT_OF_BRIDGE_MODE;

            bridgeDevice = BridgeDevice.GetInstance(clientConf);
            if (bridgeDevice.Init() < 0)
            {
                Log.Warn("the bridge connect error");
                return;
            }

            // 网桥设备同步登录接口。
            int result = bridgeDevice.bridgeClient.LoginSync(DEVICE_ID, DEVICE_SECRET, 1000);
            if (result != 0)
            {
                Log.Warn("bridge device login failed. the result is {0}", result);
                return;
            }

            // 上报属性
            ReportProperty();

            // 上报设备消息
            ReportDeviceMessage();

            // 处理命令下发
            HandleCommand();

            // 处理消息下发
            HandlerMessageDown();

            // 重置设备密钥
            ResetDeviceSecret();

            // 设备登出
            Logout();

            // 设备断链
            Disconnect();

            // 文件上传/下载功能
            UploadAndDownloadFile();

            // 属性设置/查询功能
            HandlePropSetOrGet();

            // 网桥设备同步登出接口。
            result = bridgeDevice.bridgeClient.LogoutSync(DEVICE_ID, 1000);
            if (result != 0)
            {
                Log.Warn("bridge device logout failed. the result is {0}", result);
            }
        }

        public void OnPropertiesSet(string deviceId, string requestId, List<ServiceProperty> services)
        {
            Log.Info("the requestId is {0}", requestId);
            Log.Info("the deviceId is {0}", deviceId);

            if (services == null)
            {
                Log.Warn("the services is null");
            }

            // 遍历service
            foreach (ServiceProperty serviceProperty in services)
            {
                Log.Info("OnPropertiesSet, serviceId is {0}", serviceProperty.serviceId);
                // 遍历属性
                foreach (string name in serviceProperty.properties.Keys)
                {
                    Log.Info("property name is {0}", name);
                    Log.Info("set property value is {0}", serviceProperty.properties[name]);
                }

            }

            // 修改本地的属性值
            bridgeDevice.bridgeClient.RespondPropsSet(deviceId, requestId, IotResult.SUCCESS);
        }

        public void OnPropertiesGet(string deviceId, string requestId, string serviceId)
        {
            Log.Info("the requestId is {0}", requestId);
            Log.Info("the deviceId is {0}", deviceId);
            Log.Info("OnPropertiesGet, the serviceId is {0}", serviceId);

            // 读取本地的属性值并上报
            ServiceProperty serviceProperty = new ServiceProperty();
            serviceProperty.serviceId = serviceId;

            List<ServiceProperty> services = new List<ServiceProperty>();
            services.Add(serviceProperty);
            // 上报本地的属性值
            bridgeDevice.bridgeClient.RespondPropsGet(deviceId, requestId, services);
        }

        private void HandlePropSetOrGet()
        {
            bridgeDevice.bridgeClient.bridgePropertyListener = this;
        }

        public void OnDisConnect(string deviceId)
        {
            // 打印断链的返回体
            Log.Info("the disconnected device is {0}", deviceId);
        }

        private void Disconnect()
        {
            bridgeDevice.bridgeClient.bridgeDeviceDisConnListener = this;
        }

        // 设备登出操作
        public void OnLogout(string deviceId, string requestId, Dictionary<string, object> map)
        {
            Log.Info("the requestId is {0}", requestId);
            Log.Info("the deviceId is {0}", deviceId);
            Log.Info("the response of login is {0}", map["result_code"]);
        }

        private void Logout()
        {
            string requestId = Guid.NewGuid().ToString();
            bridgeDevice.bridgeClient.logoutListener = this;
            bridgeDevice.bridgeClient.LogoutAsync(DEVICE_ID, requestId);
        }


        // 密码重置操作
        public void OnResetDeviceSecret(string deviceId, string requestId, int resultCode, string newSecret)
        {
            // 打印重置密钥的返回体
            Log.Info("the requestId is {0}", requestId);
            Log.Info("the deviceId is {0}", deviceId);
            Log.Info("the resultCode is {0}", resultCode);
        }

        private void ResetDeviceSecret()
        {
            string requestId = Guid.NewGuid().ToString();
            bridgeDevice.bridgeClient.resetDeviceSecretListener = this;   
            bridgeDevice.bridgeClient.ResetSecret(DEVICE_ID, requestId, new DeviceSecret(DEVICE_SECRET, NEW_DEVICE_SECRET));
        }

        // 消息下行操作
        public void OnDeviceMessage(string deviceId, DeviceMessage deviceMsg)
        {
            // 打印网桥消息下发的body体
            Log.Info("the deviceId is {0}", deviceId);
            Log.Info("the message of device is {0}", deviceMsg.ToString());
        }

        private void HandlerMessageDown()
        {
            bridgeDevice.bridgeClient.bridgeDeviceMessageListener = this;
        }

        // 命令响应操作
        public void OnCommand(string deviceId, string requestId, BridgeCommand bridgeCommand)
        {
            Log.Info("the requestId is {0}", requestId);
            Log.Info("the deviceId is {0}", deviceId);
            Log.Info("the command of device is {0}", bridgeCommand);
            bridgeDevice.bridgeClient.RespondCommand(deviceId, requestId, new CommandRsp(0));
        }

        private void HandleCommand()
        {
            bridgeDevice.bridgeClient.bridgeCommandListener = this;
        }

        private void ReportDeviceMessage()
        {
            DeviceMessage deviceMessage = new DeviceMessage();
            deviceMessage.name = MESSAGE_NAME;
            deviceMessage.id = MESSAGE_ID;
            deviceMessage.content = MESSAGE_CONTENT;
            bridgeDevice.bridgeClient.ReportDeviceMessage(DEVICE_ID, deviceMessage);
        }

        private void ReportProperty()
        {
            Dictionary<string, object> json = new Dictionary<string, object>();
            Random rand = new Random();
            json.Add(PROPERTY, rand.NextDouble() * 100.0f);
            ServiceProperty serviceProperty = new ServiceProperty();
            serviceProperty.properties = json;
            serviceProperty.serviceId = SERVICE_ID;

            List<ServiceProperty> listProperties = new List<ServiceProperty>();
            listProperties.Add(serviceProperty);

            bridgeDevice.bridgeClient.ReportProperties(DEVICE_ID, listProperties);
        }

        private void UploadAndDownloadFile()
        {
            FileManagerService fileManagerService = bridgeDevice.fileManagerService;
            fileManagerService.bridgeFileMangerListener = new DefaultBridgeFileManagerListener(fileManagerService);

            UrlRequest urlRequest = new UrlRequest();
            urlRequest.fileName = FILE_NAME;
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("hash_code", HASH_CODE);
            map.Add("size", SIZE);
            urlRequest.fileAttributes = map;
            fileManagerService.GetUploadUrlOfBridge(DEVICE_ID, urlRequest);

            Thread.Sleep(3000);
            fileManagerService.GetDownloadUrlOfBridge(DEVICE_ID, urlRequest);
        }
    }
}
