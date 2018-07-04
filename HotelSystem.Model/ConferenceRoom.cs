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
    /// 会议室
    /// </summary>
    public partial class ConferenceRoom
    {
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string Id { get; set; }
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string ConferenceId { get; set; }
    	/// <summary>
        /// 会议室名称
        /// </summary>
    	[DisplayName( "会议室名称" )]
        public string Name { get; set; }
    	/// <summary>
        /// 面积
        /// </summary>
    	[DisplayName( "面积" )]
        public string Area { get; set; }
    	/// <summary>
        /// 容纳人数
        /// </summary>
    	[DisplayName( "容纳人数" )]
        public int Capacity { get; set; }
    	/// <summary>
        /// 层高
        /// </summary>
    	[DisplayName( "层高" )]
        public int Storey { get; set; }
    	/// <summary>
        /// 长宽
        /// </summary>
    	[DisplayName( "长宽" )]
        public string Size { get; set; }
    	/// <summary>
        /// 立柱
        /// </summary>
    	[DisplayName( "立柱" )]
        public short Column { get; set; }
    	/// <summary>
        /// 会议室布局
        /// </summary>
    	[DisplayName( "会议室布局" )]
        public string Layout { get; set; }
    	/// <summary>
        /// 营业时间
        /// </summary>
    	[DisplayName( "营业时间" )]
        public string BusinessHours { get; set; }
    	/// <summary>
        /// 图片
        /// </summary>
    	[DisplayName( "图片" )]
        public string Images { get; set; }
    	/// <summary>
        /// 场价
        /// </summary>
    	[DisplayName( "场价" )]
        public string Price { get; set; }
    	/// <summary>
        /// 半场价
        /// </summary>
    	[DisplayName( "半场价" )]
        public string HalfPrice { get; set; }
    
        public virtual Conference Conference { get; set; }
    }
}
