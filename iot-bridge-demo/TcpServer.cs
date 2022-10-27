using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Codecs;
using IoT.SDK.Device.Client.Requests;
using NLog;

namespace IoT.Bridge.Demo
{
    // 一个传输字符串数据的tcp server，客户端建链后，首条消息是鉴权消息，携带设备标识nodeId。server将收到的消息通过bridge转发给平台
    class TcpServer
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private int port;

        public TcpServer(int port)
        {
            this.port = port;
        }

        public async Task Run()
        {
            MultithreadEventLoopGroup bossGroup = new MultithreadEventLoopGroup();
            MultithreadEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
            try
            {
                ServerBootstrap b = new ServerBootstrap();
                b.Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;

                        pipeline.AddLast("decoder", new StringDecoder());
                        pipeline.AddLast("encoder", new StringEncoder());
                        pipeline.AddLast("handler", new StringHandler());

                        Log.Info("initChannel:" + channel.RemoteAddress);
                    }))
                    .Option(ChannelOption.SoBacklog, 128)
                    .ChildOption(ChannelOption.SoKeepalive, true);

                Log.Info("tcp server start......");

                IChannel f = await b.BindAsync(port);

                await f.CloseCompletion;
            }
            catch (Exception ex)
            {
                Log.Error("tcp server start error.");
            }
            finally
            {
                await workerGroup.ShutdownGracefullyAsync();
                await bossGroup.ShutdownGracefullyAsync();

                Log.Info("tcp server close");
            }
        }

        public class StringHandler : SimpleChannelInboundHandler<string>
        {
            protected override void ChannelRead0(IChannelHandlerContext ctx, string s)
            {
                IChannel incoming = ctx.Channel;
                Log.Info("channelRead0" + incoming.RemoteAddress + " msg :" + s);

                // 如果是首条消息,创建session
                Session session = Bridge.GetInstance().GetSessionByChannel(incoming.Id.AsLongText());
                if (session == null)
                {
                    Bridge.GetInstance().CreateSession(s, incoming);
                }
                else
                {
                    session.deviceClient.ReportDeviceMessage(new DeviceMessage(s));
                }
            }
        }
    }
}
