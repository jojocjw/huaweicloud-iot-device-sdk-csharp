using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IoT.SDK.Bridge.Bootstrap
{
    public class BridgeClientConf
    {
        // 平台接入地址变量名称
        private static readonly string ENV_NET_BRIDGE_SERVER_IP = "iot-mqtts.cn-north-5.myhuaweicloud.com";

        // 平台接入端口变量名称
        private static readonly int ENV_NET_BRIDGE_SERVER_PORT = 8884;

        // 网桥ID环境变量名称
        private static readonly string ENV_NET_BRIDGE_ID = "bridge004";

        // 网桥密码环境变量名称
        private static readonly string ENV_NET_BRIDGE_SECRET = "bridge004";

        // 连接IoT平台的地址 样例：xxxxxx.iot-mqtts.cn-north-4.myhuaweicloud.com
        public string serverIp { get; set; }

        // 连接IoT平台的端口
        public int serverPort { get; set; }

        // 连接IoT平台的网桥ID.
        public string bridgeId { get; set; }

        // 连接IoT平台的网桥密码
        public string bridgeSecret { get; set; }

        public static BridgeClientConf Config()
        {
            BridgeClientConf conf = new BridgeClientConf();
            conf.serverIp = ENV_NET_BRIDGE_SERVER_IP;
            conf.serverPort = ENV_NET_BRIDGE_SERVER_PORT;
            conf.bridgeId = ENV_NET_BRIDGE_ID;
            conf.bridgeSecret = ENV_NET_BRIDGE_SECRET;

            return conf;
        }
    }
}
