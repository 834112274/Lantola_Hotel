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
    /// Settlement
    /// </summary>
    public partial class Settlement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Settlement()
        {
            this.Order = new HashSet<Order>();
        }
    
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string Id { get; set; }
    	/// <summary>
        /// 开始时间
        /// </summary>
    	[DisplayName( "开始时间" )]
        public System.DateTime StartTime { get; set; }
    	/// <summary>
        /// 结束时间
        /// </summary>
    	[DisplayName( "结束时间" )]
        public System.DateTime EndTime { get; set; }
    	/// <summary>
        /// 结算金额
        /// </summary>
    	[DisplayName( "结算金额" )]
        public double Amount { get; set; }
    	/// <summary>
        /// 订单数
        /// </summary>
    	[DisplayName( "订单数" )]
        public int OrderCount { get; set; }
    	/// <summary>
        /// 结算时间
        /// </summary>
    	[DisplayName( "结算时间" )]
        public System.DateTime CreateTime { get; set; }
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string UsersId { get; set; }
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string HotelInfoId { get; set; }
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string HotelName { get; set; }
    	/// <summary>
        /// 是否转账
        /// </summary>
    	[DisplayName( "是否转账" )]
        public short IsPay { get; set; }
    	/// <summary>
        /// 转账时间
        /// </summary>
    	[DisplayName( "转账时间" )]
        public System.DateTime PayTime { get; set; }
    
        public virtual Users Users { get; set; }
        public virtual HotelInfo HotelInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
    }
}
