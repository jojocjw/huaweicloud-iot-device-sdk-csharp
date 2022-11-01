using System;
using System.Threading.Tasks;
using System.Threading;

namespace IoT.Bridge.Demo
{
    class Program
    {
        private static ManualResetEvent mre = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            // 默认使用北京4的接入地址，其他region的用户请修改
            string serverUri = "iot-mqtts.cn-north-5.myhuaweicloud.com";
            int port = 8080;

            // ================================================
            /*
            Bridge.CreateBridge(serverUri, null);
            Bridge bridge = Bridge.GetInstance();
            TcpServer tcpServer = new TcpServer(port);

            Task.Run(async () => { await tcpServer.Run(); });
            */
            // ================================================
            BridgeSample bridgeSample = new BridgeSample();
            mre.WaitOne();
        }
    }
}
