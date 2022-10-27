using System;
using System.Collections.Generic;
using System.Text;

// 位置上报周期
namespace IoT.Bridge.Sample.Tcp.Dto
{
    class DeviceLocationFrequencySet : BaseMessage
    {
        public int period { get; set; }
    }
}
