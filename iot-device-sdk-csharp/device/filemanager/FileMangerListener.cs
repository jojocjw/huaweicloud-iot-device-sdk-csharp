using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Filemanager.Response;

namespace IoT.SDK.Device.Filemanager
{
    public interface FileMangerListener
    {
        // 接收文件上传url
        void OnUploadUrl(UrlResponse param);

        // 接收文件下载url
        void OnDownloadUrl(UrlResponse param);
    }
}
