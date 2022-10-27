using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace IoT.SDK.Bridge.Request
{
    public class DeviceSecret
    {
        [JsonProperty("old_secret")]
        private string oldSecret { get; set; }

        [JsonProperty("new_secret")]
        private string newSecret { get; set; }

        public DeviceSecret(string oldSecret, string newSecret)
        {
            this.oldSecret = oldSecret;
            this.newSecret = newSecret;
        }
    }
}
