using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Client.Listener;
using IoT.SDK.Device;
using DotNetty.Transport.Channels;
using IoT.SDK.Device.Client.Requests;
using IoT.SDK.Device.Client;
using NLog;

namespace IoT.Bridge.Demo
{
    class DefaultBridgePropertyListener : PropertyListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private IChannel channel;

        private IoTDevice ioTDevice;

        public DefaultBridgePropertyListener(IChannel channel, IoTDevice ioTDevice)
        {
            this.channel = channel;
            this.ioTDevice = ioTDevice;
        }

        public void OnPropertiesSet(string requestId, List<ServiceProperty> services)
        {
            //这里可以根据需要进行消息格式转换
            channel.WriteAndFlushAsync(services);
            ioTDevice.GetClient().RespondPropsSet(requestId, IotResult.SUCCESS);
        }

        public void OnPropertiesGet(string requestId, string serviceId)
        {

            Log.Error("not supporte onSubdevPropertiesGet");
            ioTDevice.GetClient().RespondPropsSet(requestId, IotResult.FAIL);
        }
    }
}
