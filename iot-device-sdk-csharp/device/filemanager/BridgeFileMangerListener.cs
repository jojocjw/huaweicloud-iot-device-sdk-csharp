using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Filemanager.Response;

// 监听文件上传下载事件
namespace IoT.SDK.Device.Filemanager
{
    public interface BridgeFileMangerListener
    {
        // 接收文件上传url
        // param    上传参数
        // deviceId 设备Id
        void OnUploadUrl(UrlResponse param, string deviceId);

        // 接收文件下载url
        // param    下载参数`
        // deviceId 设备Id
        void OnDownloadUrl(UrlResponse param, string deviceId);
    }
}
