using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Transport.Channels;

namespace IoT.Bridge.Sample.Tcp.Session
{
    class DeviceSession
    {
        private static readonly int MAX_FLOW_NO = 0xFFFF;

        public string deviceId { get; set; }

        public IChannel channel { get; set; }

        private int seqId;

        public int GetAndUpdateSeqId()
        {
            if (seqId >= MAX_FLOW_NO - 1)
            {
                seqId = 0;
            }
            else
            {
                seqId++;
            }
            return seqId;
        }
    }
}
