using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Schany.Data.Entities.Sys
{
    [Description("用户")]
    public class Customer : BaseEntity
    {
        public Customer()
        {
            this.CustomerOpenPlateAccounts = new List<CustomerOpenPlateAccount>();
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MinLength(2, ErrorMessage = "用户名最少2个字符")]
        [MaxLength(50, ErrorMessage = "用户名最长50个字符")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [MinLength(6, ErrorMessage = "密码最少6位")]
        [MaxLength(50, ErrorMessage = "密码最长50位")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required]
        [MinLength(2, ErrorMessage = "真实姓名最少2个字符")]
        [MaxLength(50, ErrorMessage = "真实姓名最长50个字符")]
        [Display(Name = "真实姓名")]
        public string TrueName { get; set; }

        /// <summary>
        /// 用户照片
        /// </summary>
        [Display(Name = "用户照片")]
        [MaxLength(200, ErrorMessage = "用户照片路径最长200个字符")]
        public string Pic { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        [Display(Name = "最后登陆时间")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 登陆错误次数
        /// </summary>
        [Display(Name = "登陆错误次数")]
        public int LoginErrorTimes { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>    
        [Display(Name = "是否锁定")]
        public bool IsLocked { get; set; }


        #region 导航属性
        public ICollection<CustomerOpenPlateAccount> CustomerOpenPlateAccounts { get; set; }
        #endregion
    }
}
