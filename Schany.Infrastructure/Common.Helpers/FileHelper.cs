using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Schany.Infrastructure.Common.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="files"></param>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        public static async Task<Notification> FileUpload(string baseUrl, IFormCollection files, IHostingEnvironment hostingEnvironment)
        {
            string rootPath = hostingEnvironment.WebRootPath + "\\Content\\UploadFiles\\";
            string fileUrl = "/Content/UploadFiles/";

            var uploadFile = files.Files[0];
            string fileName = uploadFile.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();
            if (fileExt.IndexOf(".") >= 0)
            {
                fileExt = fileExt.Substring(fileExt.IndexOf(".") + 1, fileExt.Length - 1);
            }
            //上传文件分类存放
            string dirName = string.Empty;
            //定义允许上传的文件分类及其扩展名
            var imageExt = ".gif,.jpg,.jpeg,.png,.bmp";
            var mediaExt = ".swf,.flv,.mp3,.mp4,.wav,.wma,.wmv,.mid,.avi,.mpg,.asf,.rm,.rmvb";
            var FileExt = ".doc,.docx,.xls,.xlsx,.ppt,.txt,.zip,.rar,.apk";

            if (imageExt.IndexOf(fileExt) >= 0)
            {
                dirName = "image";
            }
            else if (mediaExt.IndexOf(fileExt) >= 0)
            {
                dirName = "media";
            }
            else if (FileExt.IndexOf(fileExt) >= 0)
            {
                dirName = "file";
            }
            else
            {
                return new Notification(NotifyType.Error, string.Format("文件【{0}】是不允许的上传类型。\n", fileName));
            }

            //文件大小限制
            int maxSize = 0;
            if (dirName == "image")
            {
                //图片最大5M
                maxSize = 1024 * 1024 * 5;
            }
            if (dirName == "file")
            {
                //附件最大30M
                maxSize = 1024 * 1024 * 30;
            }
            if (dirName == "media")
            {
                //媒体文件最大50M
                maxSize = 1024 * 1024 * 50;
            }

            //检查文件物理路径是否存在
            if (!Directory.Exists(rootPath))
            {
                //目录不存在就创建这个目录
                Directory.CreateDirectory(rootPath);
            }

            if (uploadFile.Length <= 0)
            {
                return new Notification(NotifyType.Error, string.Format("文件【{0}】为空。\n", fileName));
            }

            if (uploadFile.Length > maxSize)
            {
                return new Notification(NotifyType.Error,
                    string.Format("文件【{0}】{1}M,超过最大值{2}M限制。", fileName, Convert.ToString(uploadFile.Length / (1024 * 1024.00)), Convert.ToString((maxSize / (1024 * 1024)))));
            }

            //创建文件夹
            rootPath += dirName + "/";
            fileUrl += dirName + "/";
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            //按上传日期创建文件夹
            string ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            rootPath += ymd + "/";
            fileUrl += ymd + "/";
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            //按上传日期重命名文件
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + "." + fileExt;
            string filePath = rootPath + newFileName;
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    await uploadFile.CopyToAsync(stream);
                    fileUrl += newFileName;
                    return new Notification(NotifyType.Success, string.Format("文件【{0}】上传成功。", fileName), new
                    {
                        Name = fileName,
                        Url = baseUrl + fileUrl,
                        _Url = fileUrl
                    });
                }
                catch (Exception ex)
                {
                    return new Notification(NotifyType.Error, string.Format("文件【{0}】上传失败：{1}。", fileName, ex.Message));
                }
            }
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="request"></param>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        public static async Task<Notification> FileUpload(string baseUrl, HttpRequest request, IHostingEnvironment hostingEnvironment)
        {
            string rootPath = hostingEnvironment.WebRootPath + "\\Content\\UploadFiles\\";
            string fileUrl = "/Content/UploadFiles/";

            var uploadFile = request.Form.Files[0];
            string fileName = uploadFile.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();
            if (fileExt.IndexOf(".") >= 0)
            {
                fileExt = fileExt.Substring(fileExt.IndexOf(".") + 1, fileExt.Length - 1);
            }
            //上传文件分类存放
            string dirName = string.Empty;
            //定义允许上传的文件分类及其扩展名
            var imageExt = ".gif,.jpg,.jpeg,.png,.bmp";
            var mediaExt = ".swf,.flv,.mp3,.mp4,.wav,.wma,.wmv,.mid,.avi,.mpg,.asf,.rm,.rmvb";
            var FileExt = ".doc,.docx,.xls,.xlsx,.ppt,.txt,.zip,.rar,.apk";

            if (imageExt.IndexOf(fileExt) >= 0)
            {
                dirName = "image";
            }
            else if (mediaExt.IndexOf(fileExt) >= 0)
            {
                dirName = "media";
            }
            else if (FileExt.IndexOf(fileExt) >= 0)
            {
                dirName = "file";
            }
            else
            {
                return new Notification(NotifyType.Error, string.Format("文件【{0}】是不允许的上传类型。\n", fileName));
            }

            //文件大小限制
            int maxSize = 0;
            if (dirName == "image")
            {
                //图片最大5M
                maxSize = 1024 * 1024 * 5;
            }
            if (dirName == "file")
            {
                //附件最大30M
                maxSize = 1024 * 1024 * 30;
            }
            if (dirName == "media")
            {
                //媒体文件最大50M
                maxSize = 1024 * 1024 * 50;
            }

            //检查文件物理路径是否存在
            if (!Directory.Exists(rootPath))
            {
                //目录不存在就创建这个目录
                Directory.CreateDirectory(rootPath);
            }

            if (uploadFile.Length <= 0)
            {
                return new Notification(NotifyType.Error, string.Format("文件【{0}】为空。\n", fileName));
            }

            if (uploadFile.Length > maxSize)
            {
                return new Notification(NotifyType.Error,
                    string.Format("文件【{0}】{1}M,超过最大值{2}M限制。", fileName, Convert.ToString(uploadFile.Length / (1024 * 1024.00)), Convert.ToString((maxSize / (1024 * 1024)))));
            }

            //创建文件夹
            rootPath += dirName + "/";
            fileUrl += dirName + "/";
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            //按上传日期创建文件夹
            string ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            rootPath += ymd + "/";
            fileUrl += ymd + "/";
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            //按上传日期重命名文件
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + "." + fileExt;
            string filePath = rootPath + newFileName;
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    await uploadFile.CopyToAsync(stream);
                    fileUrl += newFileName;
                    return new Notification(NotifyType.Success, string.Format("文件【{0}】上传成功。", fileName), new
                    {
                        Name = fileName,
                        Url = baseUrl + fileUrl,
                        _Url = fileUrl
                    });
                }
                catch (Exception ex)
                {
                    return new Notification(NotifyType.Error, string.Format("文件【{0}】上传失败：{1}。", fileName, ex.Message));
                }
            }
        }
    }
}
