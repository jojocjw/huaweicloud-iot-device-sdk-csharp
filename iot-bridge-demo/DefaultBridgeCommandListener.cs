using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Client.Listener;
using IoT.SDK.Device;
using DotNetty.Transport.Channels;
using IoT.SDK.Device.Client.Requests;
using NLog;

namespace IoT.Bridge.Demo
{
    class DefaultBridgeCommandListener : CommandListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private IChannel channel;

        private IoTDevice ioTDevice;

        public DefaultBridgeCommandListener(IChannel channel, IoTDevice ioTDevice)
        {
            this.channel = channel;
            this.ioTDevice = ioTDevice;
        }

        public void OnCommand(string requestId, string serviceId, string commandName, Dictionary<string, object> paras)
        {
            // 这里可以根据需要进行消息格式转换
            channel.WriteAndFlushAsync(paras);

            // 为了简化处理，我们在这里直接回命令响应。更合理做法是在设备处理完后再回响应
            ioTDevice.GetClient().RespondCommand(requestId, new CommandRsp(0));
        }
    }
}
