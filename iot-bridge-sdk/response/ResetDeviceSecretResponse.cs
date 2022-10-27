using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using IoT.SDK.Device.Utils;

namespace IoT.SDK.Bridge.Response
{
    class ResetDeviceSecretResponse
    {
        [JsonProperty("result_code")]
        public int resultCode { set; get; }

        public Dictionary<string, object> paras { set; get; }

        public override string ToString()
        {
            return JsonUtil.ConvertObjectToJsonString(this);
        }
    }
}
