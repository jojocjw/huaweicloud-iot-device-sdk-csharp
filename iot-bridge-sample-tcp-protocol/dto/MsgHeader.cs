using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Bridge.Sample.Tcp.Dto
{
    // 消息头
    class MsgHeader
    {
        // 设备号
        public string deviceId { get; set; }

        // 流水号
        public string flowNo { get; set; }

        // 接口消息类型
        public string msgType { get; set; }

        // 通信方向 1、平台下发请求 2、设备返回平台的响应 3、设备上报请求 4、平台返回设备上报的响应
        public int direct { get; set; }

    }
}
