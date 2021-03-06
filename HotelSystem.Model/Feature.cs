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
    /// Feature
    /// </summary>
    public partial class Feature
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Feature()
        {
            this.FeatureHotel = new HashSet<FeatureHotel>();
        }
    
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string Id { get; set; }
    	/// <summary>
        /// 标签名
        /// </summary>
    	[DisplayName( "标签名" )]
        public string Name { get; set; }
    	/// <summary>
        /// 说明
        /// </summary>
    	[DisplayName( "说明" )]
        public string Summary { get; set; }
    	/// <summary>
        /// 图标
        /// </summary>
    	[DisplayName( "图标" )]
        public string Ico { get; set; }
    	/// <summary>
        /// 类型
        /// </summary>
    	[DisplayName( "类型" )]
        public short Type { get; set; }
    	/// <summary>
        /// 创建时间
        /// </summary>
    	[DisplayName( "创建时间" )]
        public System.DateTime CreateTime { get; set; }
    	/// <summary>
        /// 更新时间
        /// </summary>
    	[DisplayName( "更新时间" )]
        public System.DateTime UpdateTime { get; set; }
    	/// <summary>
        /// 是否有效
        /// </summary>
    	[DisplayName( "是否有效" )]
        public bool Valid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeatureHotel> FeatureHotel { get; set; }
    }
}
