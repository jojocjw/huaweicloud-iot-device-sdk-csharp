using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Bridge.Sample.Tcp.Constant
{
    public static class Constants
    {
        // 登录消息
        public const string MSG_TYPE_DEVICE_LOGIN = "DEVICE_LOGIN";

        // 位置上报
        public const string MSG_TYPE_REPORT_LOCATION_INFO = "REPORT_LOCATION_INFO";

        // 设备位置上报周期
        public const string MSG_TYPE_FREQUENCY_LOCATION_SET = "FREQUENCY_LOCATION_SET";


        // 设备请求
        public const int DIRECT_DEVICE_REQ = 3;

        // 云端响应消息
        public const int DIRECT_CLOUD_RSP = 4;

        // 云端发给设备的消息
        public const int DIRECT_CLOUD_REQ = 1;

        // 设备返回的响应消息
        public const int DIRECT_DEVICE_RSP = 2;

        // 消息头分隔符
        public const string HEADER_PARS_DELIMITER  = ",";

        // 消息体分隔符
        public const string BODY_PARS_DELIMITER = "@";

        // 消息开始标志
        public const string MESSAGE_START_DELIMITER = "[";

        // 消息结束标志
        public const string MESSAGE_END_DELIMITER = "]";
    }
}
