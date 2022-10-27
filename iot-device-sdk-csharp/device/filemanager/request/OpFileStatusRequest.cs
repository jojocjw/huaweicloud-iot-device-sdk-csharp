using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace IoT.SDK.Device.Filemanager.Request
{
    public class OpFileStatusRequest
    {
        [JsonProperty("object_name")]
        public string objectName { get; set; }

        [JsonProperty("result_code")]
        public int resultCode { get; set; }

        [JsonProperty("status_code")]
        public int statusCode { get; set; }

        [JsonProperty("status_description")]
        public string statusDescription { get; set; }

        public override string ToString()
        {
            return "UploadFileStatusRequest{" + "objectName='" + objectName + '\'' + ", resultCode=" + resultCode
                + ", statusCode=" + statusCode + ", statusDescription='" + statusDescription + '\'' + '}';
        }
    }
}
