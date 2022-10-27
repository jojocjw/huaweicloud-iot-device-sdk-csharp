using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Bridge.Sample.Tcp.Dto
{
    class DeviceLoginMessage : BaseMessage
    {
        // 设备鉴权码
        public string secret { get; set; }
    }
}
