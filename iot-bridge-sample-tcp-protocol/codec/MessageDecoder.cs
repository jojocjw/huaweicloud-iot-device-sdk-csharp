using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Transport.Channels;
using IoT.Bridge.Sample.Tcp.Constant;
using IoT.Bridge.Sample.Tcp.Dto;
using NLog;

namespace IoT.Bridge.Sample.Tcp.Codec
{
    class MessageDecoder : SimpleChannelInboundHandler<string>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        {
            Log.Info("MessageDecoder msg={0}", msg);
            if (!CheckInComingMsg(msg))
            {
                return;
            }
            int startIndex = ((string)msg).IndexOf(Constants.MESSAGE_START_DELIMITER);
            if (startIndex < 0)
            {
                return;
            }

            BaseMessage message = DecodeMessage(((string)msg).Substring(startIndex + 1));
            if (message == null)
            {
                Log.Warn("decode message failed");
                return;
            }
            ctx.FireChannelRead(message);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, string msg) { }
        private bool CheckInComingMsg(object msg) { return msg is string && ((string)msg).Length != 0; }

        private BaseMessage DecodeMessage(string message)
        {
            MsgHeader header = DecodeHeader(message);
            if (header == null)
            {
                return null;
            }
            BaseMessage baseMessage = DecodeBody(header, message.Substring(message.LastIndexOf(",") + 1));
            if (baseMessage == null)
            {
                return null;
            }
            baseMessage.msgHeader = header;
            return baseMessage;
        }

        private MsgHeader DecodeHeader(string message)
        {
            string[] splits = message.Split(Constants.HEADER_PARS_DELIMITER);
            if (splits.Length <= 4)
            {
                return null;
            }

            MsgHeader msgHeader = new MsgHeader();
            msgHeader.deviceId = splits[0];
            msgHeader.flowNo = splits[1];
            msgHeader.msgType = splits[2];
            msgHeader.direct = int.Parse(splits[3]);
            return msgHeader;
        }

        private BaseMessage DecodeBody(MsgHeader header, string body)
        {
            switch (header.msgType)
            {
                case Constants.MSG_TYPE_DEVICE_LOGIN:
                    return DecodeLoginMessage(body);

                case Constants.MSG_TYPE_REPORT_LOCATION_INFO:
                    return DecodeLocationMessage(body);

                case Constants.MSG_TYPE_FREQUENCY_LOCATION_SET:
                    return DecodeLocationSetMessage(body);

                default:
                    Log.Warn("invalid msgType");
                    return null;
            }
        }

        private BaseMessage DecodeLoginMessage(string body)
        {
            DeviceLoginMessage loginMessage = new DeviceLoginMessage();
            loginMessage.secret = body;
            return loginMessage;
        }

        private BaseMessage DecodeLocationMessage(string body)
        {
            string[] splits = body.Split(Constants.BODY_PARS_DELIMITER);
            DeviceLocationMessage deviceLocationMessage = new DeviceLocationMessage();
            deviceLocationMessage.longitude = splits[0];
            deviceLocationMessage.latitude = splits[1];
            return deviceLocationMessage;
        }

        private BaseMessage DecodeLocationSetMessage(string body)
        {
            CommonResponse commonResponse = new CommonResponse();
            commonResponse.resultCode = int.Parse(body);
            return commonResponse;
        }
    }
}
