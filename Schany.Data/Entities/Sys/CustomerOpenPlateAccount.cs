using Schany.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Schany.Data.Entities.Sys
{
    [Description("用户开放平台账号")]
    public class CustomerOpenPlateAccount : BaseEntity
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [Display(Name = "开放平台类型")]
        public OpenPlateType OpenPlateType { get; set; }

        [Required]
        [Display(Name = "开放平台账户")]
        [StringLength(50, ErrorMessage = "开放平台账户最长50个字符")]
        public string Account { get; set; }

        #region 导航属性
        public virtual Customer Customer { get; set; } 
        #endregion
    }
}
