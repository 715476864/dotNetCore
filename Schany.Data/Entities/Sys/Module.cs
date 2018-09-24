
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Schany.Data.Entities.Sys
{
    [Description("系统模块")]
    public class Module : BaseEntity
    {
        public Module()
        {
            this.PresetDataPermissions = new List<PresetDataPermission>();
        }

        [Display(Name = "名称")]
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "文本")]
        [StringLength(50, MinimumLength = 2)]
        public string Text { get; set; }

        [Display(Name = "显示顺序")]
        public int DisplayNo { get; set; }

        [Display(Name = "附加样式")]
        [MaxLength(100)]
        public string AppendClass { get; set; }

        [Display(Name = "备注")]
        [MaxLength(100)]
        public string Remark { get; set; }

        [Display(Name = "父模块Id")]
        public Guid? ParentModuleId { get; set; }

        [Display(Name = "是否有数据权限")]
        public bool HasPermission { get; set; }

        #region 导航属性

        /// <summary>
        /// 父模块
        /// </summary>
        public virtual Module ParentModule { get; set; }
        /// <summary>
        /// 子模块
        /// </summary>
        public virtual ICollection<Module> Children { get; set; }

        /// <summary>
        /// 模块关联的功能
        /// </summary>
        //public virtual ICollection<Function> Functions { get; set; }

        /// <summary>
        /// 角色-模块 n-n
        /// </summary>
        //public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// 该模块包括的预置数据权限项
        /// </summary>
        public virtual ICollection<PresetDataPermission> PresetDataPermissions { get; set; }
        #endregion
    }
}
