using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Transport.Channels;
using IoT.Bridge.Sample.Tcp.Constant;
using IoT.Bridge.Sample.Tcp.Dto;
using IoT.SDK.Device.Utils;
using System.Threading.Tasks;
using NLog;

namespace IoT.Bridge.Sample.Tcp.Codec
{
    class MessageEncoder : ChannelHandlerAdapter
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public override Task WriteAsync(IChannelHandlerContext ctx, object msg)
        {
            Log.Info("MessageEncoder msg={}", JsonUtil.ConvertObjectToJsonString(msg));
            BaseMessage baseMessage = (BaseMessage)msg;
            MsgHeader msgHeader = baseMessage.msgHeader;

            StringBuilder stringBuilder = new StringBuilder();
            EncodeHeader(msgHeader, stringBuilder);

            // 根据消息类型编码消息
            switch (msgHeader.msgType)
            {
                case Constants.MSG_TYPE_DEVICE_LOGIN:
                case Constants.MSG_TYPE_REPORT_LOCATION_INFO:
                    stringBuilder.Append(((CommonResponse)msg).resultCode);
                    break;
                case Constants.MSG_TYPE_FREQUENCY_LOCATION_SET:
                    stringBuilder.Append(((DeviceLocationFrequencySet)msg).period);
                    break;
                default:
                    Log.Warn("invalid msgType");
                    return new Task(()=> { });
            }

            // 添加结束符
            stringBuilder.Append(Constants.MESSAGE_END_DELIMITER);

            return ctx.WriteAsync(stringBuilder.ToString());
        }

        private void EncodeHeader(MsgHeader msgHeader, StringBuilder sb)
        {
            sb.Append(Constants.MESSAGE_START_DELIMITER)
                .Append(msgHeader.deviceId)
                .Append(Constants.HEADER_PARS_DELIMITER)
                .Append(msgHeader.flowNo)
                .Append(Constants.HEADER_PARS_DELIMITER)
                .Append(msgHeader.msgType)
                .Append(Constants.HEADER_PARS_DELIMITER)
                .Append(msgHeader.direct)
                .Append(Constants.HEADER_PARS_DELIMITER);
        }
    }
}
