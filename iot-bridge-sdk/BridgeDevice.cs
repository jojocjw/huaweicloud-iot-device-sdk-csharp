using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device;
using IoT.SDK.Device.Client;
using IoT.SDK.Bridge.Clent;
using NLog;

namespace IoT.SDK.Bridge.Device
{
    public class BridgeDevice : IoTDevice {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static BridgeDevice instance;

        public BridgeClient bridgeClient { get; }

        private BridgeDevice(ClientConf clientConf) : base(clientConf.ServerUri, clientConf.Port, clientConf.DeviceId, clientConf.Secret)
        {
            if (clientConf.Mode != ClientConf.CONNECT_OF_BRIDGE_MODE) {
                throw new Exception("the bridge mode is invalid which the value should be 3.");
            }
            bridgeClient = new BridgeClient(clientConf, this);
        }

        // 此处采用单例模式，默认一个网桥服务，只会启动一个网桥，且网桥参数一致
        public static BridgeDevice GetInstance(ClientConf clientConf)
        {
            if (instance == null)
            {
                instance = new BridgeDevice(clientConf);
            }
            return instance;
        }

        public new int Init()
        {
            Log.Debug("the bridge client starts to init.");
            return bridgeClient.Connect();
        }
    
    }
}
