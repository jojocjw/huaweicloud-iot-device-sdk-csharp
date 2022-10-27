using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Bridge.Device;
using IoT.SDK.Device.Client;
using NLog;

namespace IoT.SDK.Bridge.Bootstrap
{
    // 网桥启动初始化
    public class BridgeBootstrap
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // 网桥模式
        private static readonly int CONNECT_OF_BRIDGE_MODE = 3;

        private BridgeDevice bridgeDevice;

        // 从环境变量获取网桥配置信息，初始化网桥。
        public void InitBridge()
        {
            BridgeClientConf conf = BridgeClientConf.fromEnv();
            InitBridge(conf);
        }

        // 根据网桥配置信息，初始化网桥
        public void InitBridge(BridgeClientConf conf)
        {
            if (conf == null)
            {
                conf = BridgeClientConf.fromEnv();
            }
            BridgeOnline(conf);
        }

        private void BridgeOnline(BridgeClientConf conf)
        {
            ClientConf clientConf = new ClientConf();
            if (conf.serverIp != null && conf.serverPort != null)
            {
                clientConf.ServerUri = ("ssl://" + conf.serverIp + ":" + conf.serverPort);
            }
            clientConf.DeviceId = conf.bridgeId;
            clientConf.Secret = conf.bridgeSecret;
            clientConf.Mode = CONNECT_OF_BRIDGE_MODE;

            BridgeDevice bridgeDev = BridgeDevice.GetInstance(clientConf);
            if (bridgeDev.Init() != 0)
            {
                Log.Error("Bridge can't login. please check!");
            }
            this.bridgeDevice = bridgeDev;
        }

        public BridgeDevice GetBridgeDevice()
        {
            return bridgeDevice;
        }
    }
}
