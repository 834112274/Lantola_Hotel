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
    /// 合作伙伴
    /// </summary>
    public partial class Cooperative
    {
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string Id { get; set; }
    	/// <summary>
        /// 名称
        /// </summary>
    	[DisplayName( "名称" )]
        public string Name { get; set; }
    	/// <summary>
        /// Logo
        /// </summary>
    	[DisplayName( "Logo" )]
        public string LogoUrl { get; set; }
    	/// <summary>
        /// 描述
        /// </summary>
    	[DisplayName( "描述" )]
        public string Summary { get; set; }
    	/// <summary>
        /// 官网
        /// </summary>
    	[DisplayName( "官网" )]
        public string Url { get; set; }
    	/// <summary>
        /// 创建时间
        /// </summary>
    	[DisplayName( "创建时间" )]
        public System.DateTime CreateTime { get; set; }
    }
}
