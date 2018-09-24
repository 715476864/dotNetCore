using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Schany.Data.Entities.Sys
{
    [Description("预置数据权限项")]
    public class PresetDataPermission : BaseEntity
    {       
        [StringLength(50, ErrorMessage = "预置数据权限项名称(Name)最长50个字符")]
        [Display(Name = "预置数据权限项名称")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "预置数据权限项文本(Text)最长50个字符")]
        [Display(Name = "预置数据权限项文本")]
        public string Text { get; set; }

        #region 导航属性
        public virtual Customer CreateUser { get; set; } 
        #endregion
    }
}
