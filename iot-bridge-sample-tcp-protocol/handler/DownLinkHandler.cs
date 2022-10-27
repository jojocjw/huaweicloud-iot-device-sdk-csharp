using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Bridge.Listener;
using IoT.SDK.Device.Client.Requests;
using IoT.SDK.Bridge.Request;
using IoT.Bridge.Sample.Tcp.Session;
using IoT.Bridge.Sample.Tcp.Dto;
using IoT.Bridge.Sample.Tcp.Constant;
using NLog;

namespace IoT.Bridge.Sample.Tcp.Handler
{
    class DownLinkHandler : BridgeDeviceMessageListener, BridgeCommandListener, BridgeDeviceDisConnListener
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void OnDeviceMessage(string deviceId, DeviceMessage deviceMessage) {}

        public void OnCommand(string deviceId, string requestId, BridgeCommand bridgeCommand)
        {
            Log.Info("onCommand deviceId={0}, requestId={1}, bridgeCommand={2}", deviceId, requestId, bridgeCommand);
            DeviceSession session = DeviceSessionManger.GetInstance().GetSession(deviceId);
            if (session == null)
            {
                Log.Warn("device={0} session is null", deviceId);
                return;
            }

            // 设置位置上报的周期
            if (bridgeCommand.command.commandName == "FREQUENCY_LOCATION_SET")
            {
                processLocationSetCommand(session, requestId, bridgeCommand);
            }
        }

        private void processLocationSetCommand(DeviceSession session, string requestId, BridgeCommand bridgeCommand)
        {
            int flowNo = session.GetAndUpdateSeqId();

            // 构造消息头
            MsgHeader msgHeader = new MsgHeader();
            msgHeader.deviceId = session.deviceId;
            msgHeader.flowNo = flowNo.ToString();
            msgHeader.direct = Constants.DIRECT_CLOUD_REQ;
            msgHeader.msgType = bridgeCommand.command.commandName;

            // 根据参数内容构造消息体
            Dictionary<string, Object> paras = bridgeCommand.command.paras;
            DeviceLocationFrequencySet locationFrequencySet = new DeviceLocationFrequencySet();
            locationFrequencySet.period = (int)paras["period"];
            locationFrequencySet.msgHeader = msgHeader;

            // 发下消息到设备
            session.channel.WriteAndFlushAsync(locationFrequencySet);

            // 记录平台requestId和设备流水号的关联关系，用于关联命令的响应
            Session.RequestIdCache.GetInstance().SetRequestId(session.deviceId, flowNo.ToString(), requestId);
        }
        public void OnDisConnect(string deviceId)
        {
            // 关闭session
            DeviceSession session = DeviceSessionManger.GetInstance().GetSession(deviceId);
            if(session != null && session.channel != null)
            {
                session.channel.CloseAsync();
            }

            // 删除会话
            DeviceSessionManger.GetInstance().DeleteSession(deviceId);
        }
    }
}
