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
    /// Stock
    /// </summary>
    public partial class Stock
    {
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string Id { get; set; }
    	/// <summary>
        /// 日期
        /// </summary>
    	[DisplayName( "日期" )]
        public System.DateTime Date { get; set; }
    	/// <summary>
        /// 总库存
        /// </summary>
    	[DisplayName( "总库存" )]
        public short Total { get; set; }
    	/// <summary>
        /// 剩余库存
        /// </summary>
    	[DisplayName( "剩余库存" )]
        public short SurplusStock { get; set; }
    	/// <summary>
        /// 状态
        /// </summary>
    	[DisplayName( "状态" )]
        public short Status { get; set; }
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string RoomId { get; set; }
    
        public virtual Room Room { get; set; }
    }
}
