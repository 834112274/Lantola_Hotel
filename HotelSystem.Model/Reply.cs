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
    /// Reply
    /// </summary>
    public partial class Reply
    {
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string Id { get; set; }
    	/// <summary>
        /// 回复内容
        /// </summary>
    	[DisplayName( "回复内容" )]
        public string Content { get; set; }
    	/// <summary>
        /// 回复时间
        /// </summary>
    	[DisplayName( "回复时间" )]
        public System.DateTime CreateTime { get; set; }
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string HotelUsersId { get; set; }
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string CommentId { get; set; }
    
        public virtual HotelUsers HotelUsers { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
