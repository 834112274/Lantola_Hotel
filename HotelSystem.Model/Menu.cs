//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelSystem.Model
{
    using System;
    using System.Collections.Generic;
    
    using System.ComponentModel;
    /// <summary>
    /// Menu
    /// </summary>
    public partial class Menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menu()
        {
            this.UserRole = new HashSet<UserRole>();
        }
    
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string Id { get; set; }
    	/// <summary>
        /// 菜单名称
        /// </summary>
    	[DisplayName( "菜单名称" )]
        public string Name { get; set; }
    	/// <summary>
        /// 菜单图标
        /// </summary>
    	[DisplayName( "菜单图标" )]
        public string IcoClass { get; set; }
    	/// <summary>
        /// 地址
        /// </summary>
    	[DisplayName( "地址" )]
        public string Url { get; set; }
    	/// <summary>
        /// 父级
        /// </summary>
    	[DisplayName( "父级" )]
        public string ParentId { get; set; }
    	/// <summary>
        /// 层级
        /// </summary>
    	[DisplayName( "层级" )]
        public short Level { get; set; }
    	/// <summary>
        /// 类型
        /// </summary>
    	[DisplayName( "类型" )]
        public string Type { get; set; }
    	/// <summary>
        /// 排序
        /// </summary>
    	[DisplayName( "排序" )]
        public short Sort { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
