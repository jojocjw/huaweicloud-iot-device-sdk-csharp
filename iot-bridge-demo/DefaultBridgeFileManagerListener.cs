using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Filemanager;
using IoT.SDK.Device.Filemanager.Response;
using IoT.SDK.Device.Filemanager.Request;
using NLog;

namespace IoT.Bridge.Demo
{
    class DefaultBridgeFileManagerListener : BridgeFileMangerListener
    {
        // 上传文件结果码，此处样例填写0。
        private static readonly int RESULT_CODE_OF_FILE_UP = 0;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private FileManagerService fileManagerService;

        public DefaultBridgeFileManagerListener(FileManagerService fileManagerService)
        {
            this.fileManagerService = fileManagerService;
        }

        public void OnUploadUrl(UrlResponse param, string deviceId)
        {
            Log.Info("the upload url is {0}", param.url);
            Log.Info("the upload bucketName is {0}", param.bucketName);
            Log.Info("the upload objectName  is {0}", param.objectName);
            Log.Info("the upload expire is {0}", param.expire);
            Log.Info("the fileAttributes is {0}", param.fileAttributes);

            // 收到文件上传的URL后，用户实现向URL上传文件

            // 上报文件上传结果
            OpFileStatusRequest opFileStatusRequest = new OpFileStatusRequest();
            opFileStatusRequest.objectName = "objectName";
            opFileStatusRequest.resultCode = RESULT_CODE_OF_FILE_UP;
            opFileStatusRequest.statusCode = 200;
            opFileStatusRequest.statusDescription = "upload file success";
            fileManagerService.ReportUploadFileStatus(opFileStatusRequest);
        }

        public void OnDownloadUrl(UrlResponse param, string deviceId)
        {
            if (param == null)
            {
                Log.Error("the response of url is null");
            }

            Log.Info("the download url is {0}", param.url);
            Log.Info("the download bucketName is {0}", param.bucketName);
            Log.Info("the download objectName  is {0}", param.objectName);
            Log.Info("the download expire is {0}", param.expire);
            Log.Info("the download fileAttributes is {0}", param.fileAttributes);

            // 收到文件上传的URL后，从URL下载文件

            // 上报文件下载结果
            OpFileStatusRequest opFileStatusRequest = new OpFileStatusRequest();
            opFileStatusRequest.objectName = "objectName";
            opFileStatusRequest.resultCode = 0;
            opFileStatusRequest.statusCode = 200;
            opFileStatusRequest.statusDescription = "download file success";
            fileManagerService.ReportUploadFileStatus(opFileStatusRequest);
        }
    }
}
