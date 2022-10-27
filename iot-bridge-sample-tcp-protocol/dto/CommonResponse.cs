using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Bridge.Sample.Tcp.Dto
{
    class CommonResponse : BaseMessage
    {
        // 响应码
        public int resultCode { get; set; }
    }
}
