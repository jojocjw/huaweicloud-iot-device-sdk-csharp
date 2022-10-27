using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Bridge.Sample.Tcp.Dto
{
    // 设备位置信息
    class DeviceLocationMessage : BaseMessage
    {
        public string longitude { get; set; }

        public string latitude { get; set; }
    }
}
