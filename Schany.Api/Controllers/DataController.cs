
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schany.Core.Dto.Sys;
using Schany.Core.Service.Core.Sys;
using Schany.Data.Enums;
using Schany.Infrastructure;
using Schany.Infrastructure.Common.Extensions;
using Schany.Infrastructure.Common.Helpers;

namespace Schany.Api.Controllers
{
    public class DataController : BaseController
    {
        #region Ioc
        private readonly ICustomerService _customerService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ILogger<DataController> _logger { get; } //注意日志类别的创建方式。
        #endregion

        public DataController(ICustomerService customerService,
            ILogger<DataController> logger,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment)
        {
            _customerService = customerService;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("[action]")]
        public string GetString()
        {
            return "这是一个数据接口";
        }

        [HttpGet]
        [Route("[action]")]
        public List<CustomerListModel> GetCustomerList()
        {
            _logger.LogInformation(string.Format("用户请求了GetCustomerList接口,当前登录用户：{0}", loginUser.TrueName));
            return this._customerService.GetList();
        }

        #region 文件上传相关
        [HttpPost]
        [Route("[action]")]
        [RequestSizeLimit(100_000_000)]//允许上传100M左右文件      
        public async Task<Notification> UploadFile([FromForm]IFormCollection files)
        {
            return await FileHelper.FileUpload(this.baseUri, files, _hostingEnvironment);
        }

        [HttpPost]
        [Route("[action]")]
        [RequestSizeLimit(100_000_000)]//允许上传100M左右文件  
        public async Task<Notification> UploadBinaryFile()
        {
            return await FileHelper.FileUpload(this.baseUri, Request, _hostingEnvironment);
        }
        #endregion

        #region DES加密类
        /// <summary>
        /// DES加密类
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public Notification Des(string str)
        {
            //加密
            var des_str = DESEncrypt.Encrypt(str);
            //解密
            var _str = DESEncrypt.Decrypt(des_str);

            if (_str.Equals(str))
            {
                return new Notification(NotifyType.Success, "success");
            }
            else
            {
                return new Notification(NotifyType.Error, "error");
            }
        }
        #endregion

        #region 枚举扩展
        [HttpGet]
        [Route("[action]")]
        public string GetEnumDes(DicType dic)
        {
            return dic.ToDescription();
        } 
        #endregion
    }
}