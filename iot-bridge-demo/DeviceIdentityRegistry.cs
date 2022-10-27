using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Bridge.Demo
{
    interface DeviceIdentityRegistry
    {
        DeviceIdentity GetDeviceIdentity(string nodeId);
    }
}
