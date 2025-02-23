﻿using System;
using IoT.Bridge.Sample.Tcp.Bridge;
using IoT.Bridge.Sample.Tcp.Sever;
using System.Threading.Tasks;
using System.Threading;

namespace IoT.Bridge.Sample.Tcp
{

    class Program
    {
        private static ManualResetEvent mre = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            // 启动时初始化网桥连接
            BridgeService bridgeService = new BridgeService();
            bridgeService.Init();

            // 启动TCP服务
            TcpServer tcpServer = new TcpServer();
            Task.Run(async () => { await tcpServer.Start("localhost", 8080); });
            
            mre.WaitOne();
        }
    }
}
