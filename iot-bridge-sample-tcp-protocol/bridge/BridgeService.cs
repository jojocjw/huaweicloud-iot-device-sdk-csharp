using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Bridge.Clent;
using IoT.SDK.Bridge.Bootstrap;
using IoT.Bridge.Sample.Tcp.Handler;

namespace IoT.Bridge.Sample.Tcp.Bridge
{
    class BridgeService
    {
        private static BridgeClient bridgeClient;

        public void Init()
        {
            // 网桥启动初始化
            BridgeBootstrap bridgeBootstrap = new BridgeBootstrap();

            // 从环境变量获取配置进行初始化
            bridgeBootstrap.InitBridge();

            bridgeClient = bridgeBootstrap.GetBridgeDevice().bridgeClient;

            // 设置平台下行数据监听器
            DownLinkHandler downLinkHandler = new DownLinkHandler(bridgeClient);
            bridgeClient.bridgeCommandListener = downLinkHandler;   // 设置平台命令下发监听器
            bridgeClient.bridgeDeviceMessageListener = downLinkHandler;    // 设置平台消息下发监听器
            bridgeClient.bridgeDeviceDisConnListener = downLinkHandler;   // 设置平台通知网桥主动断开设备连接的监听器
        }

        public static BridgeClient GetBridgeClient()
        {
            return bridgeClient;
        }
    }
}
