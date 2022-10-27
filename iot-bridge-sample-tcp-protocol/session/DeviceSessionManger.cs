using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace IoT.Bridge.Sample.Tcp.Session
{
    class DeviceSessionManger
    {
        private static readonly DeviceSessionManger INSTANCE = new DeviceSessionManger();

        private ConcurrentDictionary<string, DeviceSession> sessions = new ConcurrentDictionary<string, DeviceSession>();

        public static DeviceSessionManger GetInstance()
        {
            return INSTANCE;
        }

        public void CreateSession(string deviceId, DeviceSession session)
        {
            sessions.TryAdd(deviceId, session);
        }

        public DeviceSession GetSession(string deviceId)
        {
            return sessions[deviceId];
        }

        public void DeleteSession(string deviceId)
        {
            DeviceSession tmp = null;
            sessions.TryRemove(deviceId, out tmp);
        }
    }
}
