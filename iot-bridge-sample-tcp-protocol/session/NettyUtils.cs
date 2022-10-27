using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Transport.Channels;
using DotNetty.Common.Utilities;

namespace IoT.Bridge.Sample.Tcp.Session
{
    class NettyUtils
    {
        private static readonly string ATTR_DEVICE_ID = "deviceId";

        private static readonly AttributeKey<object> ATTR_KEY_DEVICE_ID = AttributeKey<object>.ValueOf(ATTR_DEVICE_ID);

        public static void SetDeviceId(IChannel channel, string deviceId)
        {
            channel.GetAttribute(NettyUtils.ATTR_KEY_DEVICE_ID).Set(deviceId);
        }

        public static string GetDeviceId(IChannel channel)
        {
            return (string)channel.GetAttribute(NettyUtils.ATTR_KEY_DEVICE_ID).Get();
        }
    }
}
