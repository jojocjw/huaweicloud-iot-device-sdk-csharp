using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using DotNetty.Transport.Channels;
using IoT.SDK.Device;
using IoT.SDK.Device.Client.Listener;
using IoT.SDK.Device.Client.Requests;
using NLog;

/**
 * 此例子用来演示如何使用协议网桥来实现TCP协议设备接入。网桥为每个TCP设备创建一个客户端（IotClient），使用设备的身份
 * 和平台进行通讯。本例子TCP server传输简单的字符串，并且首条消息会发送设备标识来鉴权。用户可以自行扩展StringTcpServer类
 * 来实现更复杂的TCP server。
 */

namespace IoT.Bridge.Demo
{
    class Bridge : DeviceMessageListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static Bridge instance;

        private DeviceIdentityRegistry deviceIdentityRegistry;

        private string serverUri;

        private ConcurrentDictionary<string, Session> deviceIdToSessionMap;

        private ConcurrentDictionary<string, Session> channelIdToSessionMap;

        private Bridge(string serverUri, DeviceIdentityRegistry deviceIdentityRegistry)
        {
            this.serverUri = serverUri;

            if (deviceIdentityRegistry == null)
            {
                deviceIdentityRegistry = new DefaultDeviceIdentityRegistry();
            }
            this.deviceIdentityRegistry = deviceIdentityRegistry;
            deviceIdToSessionMap = new ConcurrentDictionary<string, Session>();
            channelIdToSessionMap = new ConcurrentDictionary<string, Session>();
        }

        public static Bridge GetInstance()
        {
            return instance;
        }

        public static void CreateBridge(string serverUri, DeviceIdentityRegistry deviceIdentityRegistry)
        {
            instance = new Bridge(serverUri, deviceIdentityRegistry);
        }

        public void BridgeTest()
        {
            // 默认使用北京4的接入地址，其他region的用户请修改
            string serverUri = "ssl://iot-mqtts.cn-north-4.myhuaweicloud.com:8883";
            int port = 8080;

            CreateBridge(serverUri, null);
            new TcpServer(port).Run();
        }

        public Session GetSessionByChannel(string channelId)
        {
            return channelIdToSessionMap[channelId];
        }

        public void RemoveSession(string channelId)
        {
            Session session = channelIdToSessionMap[channelId];
            Session tmp = null;
            if (session != null)
            {
                session.deviceClient.Close();
                deviceIdToSessionMap.Remove(session.deviceId, out tmp);
                Log.Info("session removed : {0}", session.ToString());
            }
            channelIdToSessionMap.Remove(channelId, out tmp);

        }

        public void CreateSession(string nodeId, IChannel channel)
        {

            // 根据设备识别码获取设备标识信息
            DeviceIdentity deviceIdentity = deviceIdentityRegistry.GetDeviceIdentity(nodeId);
            if (deviceIdentity == null)
            {
                Log.Warn("deviceIdentity is null");
                return;
            }

            string deviceId = deviceIdentity.deviceId;
            IoTDevice ioTDevice = new IoTDevice(serverUri, 8883, deviceId, deviceIdentity.secret);
            int ret = ioTDevice.Init();
            if (ret != 0) {
                return;
            }

            // 创建会话
            Session session = new Session();
            session.channel = channel;
            session.nodeId = nodeId;
            session.deviceId = deviceId;
            session.deviceClient = ioTDevice.GetClient();

            deviceIdToSessionMap.TryAdd(deviceId, session);
            channelIdToSessionMap.TryAdd(channel.Id.AsLongText(), session);

            // 设置下行回调
            ioTDevice.GetClient().deviceMessageListener = this;

            ioTDevice.GetClient().commandListener = new DefaultBridgeCommandListener(channel, ioTDevice);

            ioTDevice.GetClient().propertyListener = new DefaultBridgePropertyListener(channel, ioTDevice);

            // 保存会话
            Log.Info("create new session : {0}", session.ToString());

        }

        public void OnDeviceMessage(DeviceMessage deviceMessage)
        {
            Session session = deviceIdToSessionMap[deviceMessage.deviceId];
            if (session == null)
            {
                Log.Error("session is null ,deviceId:" + deviceMessage.deviceId);
                return;
            }
            session.channel.WriteAndFlushAsync(deviceMessage.content);
        }
    }
}
