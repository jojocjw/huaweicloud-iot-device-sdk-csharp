using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace IoT.SDK.Device.Filemanager.Response
{
    public class UrlResponse
    {
        public string url { get; set; }

        [JsonProperty("bucket_name")]
        public string bucketName { get; set; }

        [JsonProperty("object_name")]
        public string objectName { get; set; }

        [JsonProperty("file_attributes")]
        public Dictionary<string, object> fileAttributes { get; set; }

        public int expire { get; set; }

        public override string ToString()
        {
            return "UrlParam{" + "url='" + url + '\'' + ", bucketName='" + bucketName + '\'' + ", objectName='" + objectName
                + '\'' + ", expire=" + expire + ", fileAttributes='" + fileAttributes + '\'' + '}';
        }
    }
}
