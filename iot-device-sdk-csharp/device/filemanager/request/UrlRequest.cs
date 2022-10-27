using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace IoT.SDK.Device.Filemanager.Request
{
    public class UrlRequest
    {
        [JsonProperty("file_name")]
        public string fileName { get; set; }

        [JsonProperty("file_attributes")]
        public Dictionary<string, object> fileAttributes { get; set; }

        public override string ToString()
        {
            return "UrlRequest{" + "fileName='" + fileName + '\'' + ", fileAttributes=" + fileAttributes + '}';
        }
    }
}
