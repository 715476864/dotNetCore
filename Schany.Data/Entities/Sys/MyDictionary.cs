using Schany.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Schany.Data.Entities.Sys
{
    public class MyDictionary : BaseEntity
    {
        /// <summary>
        /// 字典类型
        /// </summary>
        [Display(Name = "字典类型")]
        [Required]
        public DicType DicType { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [Display(Name = "字典代码")]
        [Required]
        public int Code { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        [Display(Name = "字典文本")]
        [Required]
        [StringLength(20)]
        public string Text { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Display(Name = "最后修改时间")]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 最后修改用户
        /// </summary>
        [Display(Name = "最后修改用户Id")]
        public Guid? LastUpdatedUserId { get; set; }

        
        #region 导航属性
        public virtual Customer CreateUser { get; set; }  
        #endregion

    }
}
