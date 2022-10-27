using System;
using System.Collections.Generic;
using System.Text;
using IoT.SDK.Device.Service;
using IoT.SDK.Device.Filemanager.Request;
using IoT.SDK.Device.Client.Requests;
using IoT.SDK.Device.Filemanager.Response;
using IoT.SDK.Device.Utils;
using NLog;

namespace IoT.SDK.Device.Filemanager
{
    public class FileManagerService : AbstractService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly string FILE_NAME = "file_name";

        private static readonly string FILE_ATTRIBUTES = "file_attributes";

        private static readonly string FILE_MANAGER = "$file_manager";

        private static readonly string GET_UPLOAD_URL = "get_upload_url";

        private static readonly string GET_UPLOAD_URL_RESPONSE = "get_upload_url_response";

        private static readonly string OBJECT_NAME = "object_name";

        private static readonly string RESULT_CODE = "result_code";

        private static readonly string STATUS_CODE = "status_code";

        private static readonly string STATUS_DESCRIPTION = "status_description";

        private static readonly string UPLOAD_RESULT_REPORT = "upload_result_report";

        private static readonly string GET_DOWNLOAD_URL = "get_download_url";

        private static readonly string GET_DOWNLOAD_URL_RESPONSE = "get_download_url_response";

        private static readonly string DOWNLOAD_RESULT_REPORT = "download_result_report";

        // 设备获取文件上传URL监听器
        public FileMangerListener fileMangerListener { get; set; }

        // 网桥获取文件上传URL监听器
        public BridgeFileMangerListener bridgeFileMangerListener { get;  set; }

        // 直连设备获取文件上传url
        // gettingUpLoadUrlDTO 请求体
        public void GetUploadUrl(UrlRequest gettingUpLoadUrlDTO)
        {
            DeviceEvent deviceEvent = GenerateUpOrDownUrlDeviceEvent(gettingUpLoadUrlDTO, GET_UPLOAD_URL);
            iotDevice.GetClient().ReportEvent(deviceEvent);
        }

        // 网桥获取文件上传url
        // deviceId            设备Id
        // gettingUpLoadUrlDTO 请求体
        public void GetUploadUrlOfBridge(string deviceId, UrlRequest gettingUpLoadUrlDTO)
        {
            DeviceEvent deviceEvent = GenerateUpOrDownUrlDeviceEvent(gettingUpLoadUrlDTO, GET_UPLOAD_URL);
            iotDevice.GetClient().ReportEvent(deviceId, deviceEvent);
        }

        private DeviceEvent GenerateUpOrDownUrlDeviceEvent(UrlRequest gettingUpLoadUrlDTO, string eventType)
        {
            Dictionary<string, object> node = new Dictionary<string, object>();
            node.Add(FILE_NAME, gettingUpLoadUrlDTO.fileName);
            node.Add(FILE_ATTRIBUTES, gettingUpLoadUrlDTO.fileAttributes);

            DeviceEvent deviceEvent = new DeviceEvent();
            deviceEvent.serviceId = FILE_MANAGER;
            deviceEvent.eventType = eventType;
            deviceEvent.eventTime = IotUtil.GetTimeStamp();
            deviceEvent.paras = node;
            return deviceEvent;
        }

        private DeviceEvent GenerateUploadFileStatusEvent(OpFileStatusRequest uploadFileStatusRequest, string eventType)
        {
            Dictionary<string, object> node = new Dictionary<string, object>();
            node.Add(OBJECT_NAME, uploadFileStatusRequest.objectName);
            node.Add(RESULT_CODE, uploadFileStatusRequest.resultCode);
            node.Add(STATUS_CODE, uploadFileStatusRequest.statusCode);
            node.Add(STATUS_DESCRIPTION, uploadFileStatusRequest.statusDescription);

            DeviceEvent deviceEvent = new DeviceEvent();
            deviceEvent.serviceId = FILE_MANAGER;
            deviceEvent.eventType = eventType;
            deviceEvent.eventTime = IotUtil.GetTimeStamp();
            deviceEvent.paras = node;
            return deviceEvent;
        }

        // 直连设备上报文件上传结果
        // uploadFileStatusRequest 文件上传结果
        public void ReportUploadFileStatus(OpFileStatusRequest uploadFileStatusRequest)
        {
            DeviceEvent deviceEvent = GenerateUploadFileStatusEvent(uploadFileStatusRequest, UPLOAD_RESULT_REPORT);
            iotDevice.GetClient().ReportEvent(deviceEvent);
        }

        // 网桥上报文件上传结果
        // deviceId                设备Id
        // uploadFileStatusRequest 监听器
        public void ReportUploadFileStatusOfBridge(string deviceId, OpFileStatusRequest uploadFileStatusRequest)
        {
            DeviceEvent deviceEvent = GenerateUploadFileStatusEvent(uploadFileStatusRequest, UPLOAD_RESULT_REPORT);
            iotDevice.GetClient().ReportEvent(deviceId, deviceEvent);
        }

        // 直连设备获取文件下载URL
        // urlRequest 请求体
        public void GetDownloadUrl(UrlRequest urlRequest)
        {
            DeviceEvent deviceEvent = GenerateUpOrDownUrlDeviceEvent(urlRequest, GET_DOWNLOAD_URL);
            iotDevice.GetClient().ReportEvent(deviceEvent);
        }

        // 网桥设备获取文件下载URL
        // deviceId   设备Id
        // urlRequest 请求体
        public void GetDownloadUrlOfBridge(string deviceId, UrlRequest urlRequest)
        {
            DeviceEvent deviceEvent = GenerateUpOrDownUrlDeviceEvent(urlRequest, GET_DOWNLOAD_URL);
            iotDevice.GetClient().ReportEvent(deviceId, deviceEvent);
        }

        // 直连设备上报文件下载结果
        // uploadFileStatusRequest 请求体
        public void ReportDownloadFileStatus(OpFileStatusRequest uploadFileStatusRequest)
        {
            DeviceEvent deviceEvent = GenerateUploadFileStatusEvent(uploadFileStatusRequest, DOWNLOAD_RESULT_REPORT);
            iotDevice.GetClient().ReportEvent(deviceEvent);
        }

        // 网桥设备上报文件下载结果
        // deviceId   设备Id
        // uploadFileStatusRequest 请求体
        public void ReportDownloadFileStatusOfBridge(string deviceId, OpFileStatusRequest uploadFileStatusRequest)
        {
            DeviceEvent deviceEvent = GenerateUploadFileStatusEvent(uploadFileStatusRequest, DOWNLOAD_RESULT_REPORT);
            iotDevice.GetClient().ReportEvent(deviceId, deviceEvent);
        }

        // 接收文件处理事件
        public override void OnEvent(DeviceEvent deviceEvent)
        {
            if (fileMangerListener == null)
            {
                Log.Info("fileMangerListener is null");
                return;
            }

            if (deviceEvent.eventType == GET_UPLOAD_URL_RESPONSE)
            {
                UrlResponse urlParam = JsonUtil.ConvertDicToObject<UrlResponse>(deviceEvent.paras);
                fileMangerListener.OnUploadUrl(urlParam);
            } else if (deviceEvent.eventType == GET_DOWNLOAD_URL_RESPONSE)
            {
                UrlResponse urlParam = JsonUtil.ConvertDicToObject<UrlResponse>(deviceEvent.paras);
                fileMangerListener.OnDownloadUrl(urlParam);
            } else {
                Log.Error("invalid event type.");
            }
        }

        // 网桥场景下接受文件处理事件
        public void OnBridgeEvent(string deviceId, DeviceEvent deviceEvent)
        {
            if (bridgeFileMangerListener == null)
            {
                Log.Info("bridgeFileMangerListener is null");
                return;
            }

            if (deviceEvent.eventType == GET_UPLOAD_URL_RESPONSE)
            {
                UrlResponse urlParam = JsonUtil.ConvertDicToObject<UrlResponse>(deviceEvent.paras);
                bridgeFileMangerListener.OnUploadUrl(urlParam, deviceId);
            } else if (deviceEvent.eventType == GET_DOWNLOAD_URL_RESPONSE)
            {
                UrlResponse urlParam = JsonUtil.ConvertDicToObject<UrlResponse>(deviceEvent.paras);
                bridgeFileMangerListener.OnDownloadUrl(urlParam, deviceId);
            } else {
                Log.Error("invalid event type.");
            }
        }
    }
}
