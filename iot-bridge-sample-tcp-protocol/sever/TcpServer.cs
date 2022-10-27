using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Codecs;
using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using IoT.Bridge.Sample.Tcp.Codec;
using IoT.Bridge.Sample.Tcp.Handler;
using System.Threading.Tasks;
using NLog;

namespace IoT.Bridge.Sample.Tcp.Sever
{
    /**
     * 模拟定时进行位置上报的电子学生卡服务端，设备的协议数据如下：
     * <p>
     * 1、客户端同平台建链连接后，发起登录请求，格式如下：
     * [867082058798193,0,DEVICE_LOGIN,3,12345678]
     * 2、平台鉴权通过后，给设备返回登录成功的响应，格式如下：
     * [867082058798193,0,DEVICE_LOGIN,4,0]
     * <p>
     * 3、设备鉴权成功后，定时上报位置信息，格式如下：
     * [867082058798193,1,REPORT_LOCATION_INFO,3,116.307629@40.058359]
     * 4、平台返回位置上报的响应
     * [867082058798193,1,REPORT_LOCATION_INFO,4,0]
     * <p>
     * 5、平台下发设置位置上报的周期：
     * [867082058798193,2,FREQUENCY_LOCATION_SET,1,5]
     * 5、设备返回设置设备上报周期的响应：
     * [867082058798193,2,FREQUENCY_LOCATION_SET,2,0]
     */
    class TcpServer
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly int MAX_FRAME_LENGTH = 1024;

        private static readonly int DEFAULT_BUF_VALUE = 1024 * 1024;

        private static readonly int DEFAULT_IDLE_TIME = 300;

        private static readonly char DELIMITER_CHAR = ']';

        private MultithreadEventLoopGroup bossGroup;

        private MultithreadEventLoopGroup workerGroup;

        public async Task Start(string host, int port)
        {
            MultithreadEventLoopGroup bossGroup = new MultithreadEventLoopGroup();
            MultithreadEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
            try
            {
                ServerBootstrap b = new ServerBootstrap();
                b.Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ServerChannelHandler())
                    .Option(ChannelOption.SoBacklog, 128)
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.SoRcvbuf, 1024 * 1024)
                    .ChildOption(ChannelOption.SoKeepalive, true)
                    .ChildOption(ChannelOption.SoSndbuf, 1024 * 1024);

                Log.Info("tcp server start......");

                IChannel f = await b.BindAsync(port);
            }
            catch (Exception ex)
            {
                Log.Warn("Binding server host={0}, port={1} exception {2}", host, port, ex.StackTrace);
            }
        }

        class ServerChannelHandler : ChannelInitializer<IChannel>
        {
            protected override void InitChannel(IChannel channel)
            {
                IChannelPipeline pipeline = channel.Pipeline;
                pipeline.AddFirst("readTimeoutHandler", new ReadTimeoutHandler(DEFAULT_IDLE_TIME));
                pipeline.AddLast("delimiterDecoder", new DelimiterBasedFrameDecoder(MAX_FRAME_LENGTH, Unpooled.CopiedBuffer(new byte[] { (byte)DELIMITER_CHAR })));
                pipeline.AddLast("decoder", new StringDecoder());
                pipeline.AddLast("messageDecoder", new MessageDecoder());
                pipeline.AddLast("encoder", new StringEncoder());
                pipeline.AddLast("messageEncoder", new MessageEncoder());
                pipeline.AddLast("handler", new UpLinkHandler());
            }
        }
    }
}
