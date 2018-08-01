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
    /// Company
    /// </summary>
    public partial class Company
    {
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string Id { get; set; }
    	/// <summary>
        /// 公司
        /// </summary>
    	[DisplayName( "公司" )]
        public string Name { get; set; }
    	/// <summary>
        /// 电话
        /// </summary>
    	[DisplayName( "电话" )]
        public string Tel { get; set; }
    	/// <summary>
        /// 地址
        /// </summary>
    	[DisplayName( "地址" )]
        public string Address { get; set; }
    	/// <summary>
        /// 联系人
        /// </summary>
    	[DisplayName( "联系人" )]
        public string Contact { get; set; }
    	/// <summary>
        /// 邮箱
        /// </summary>
    	[DisplayName( "邮箱" )]
        public string Email { get; set; }
    	/// <summary>
        /// 手机
        /// </summary>
    	[DisplayName( "手机" )]
        public string Phone { get; set; }
    	/// <summary>
        /// 营业执照
        /// </summary>
    	[DisplayName( "营业执照" )]
        public string BusinessLicense { get; set; }
    	/// <summary>
        /// 法人身份证正面
        /// </summary>
    	[DisplayName( "法人身份证正面" )]
        public string CardPositive { get; set; }
    	/// <summary>
        /// 法人身份证反面
        /// </summary>
    	[DisplayName( "法人身份证反面" )]
        public string CardOpposite { get; set; }
    	/// <summary>
        /// 创建时间
        /// </summary>
    	[DisplayName( "创建时间" )]
        public System.DateTime CreateTime { get; set; }
    	/// <summary>
        /// 省份ID
        /// </summary>
    	[DisplayName( "省份ID" )]
        public long ProvinceId { get; set; }
    	/// <summary>
        /// 城市ID
        /// </summary>
    	[DisplayName( "城市ID" )]
        public long CityId { get; set; }
    	/// <summary>
        /// 区县ID
        /// </summary>
    	[DisplayName( "区县ID" )]
        public long DistrictId { get; set; }
    	/// <summary>
        /// 状态
        /// </summary>
    	[DisplayName( "状态" )]
        public short Status { get; set; }
    	/// <summary>
        /// 审核人
        /// </summary>
    	[DisplayName( "审核人" )]
        public string ExamineUser { get; set; }
    	/// <summary>
        /// 审核时间
        /// </summary>
    	[DisplayName( "审核时间" )]
        public System.DateTime ExamineTime { get; set; }
    	/// <summary>
        /// 审核说明意见
        /// </summary>
    	[DisplayName( "审核说明意见" )]
        public string Opinion { get; set; }
    
        public virtual Province Province { get; set; }
        public virtual City City { get; set; }
        public virtual District District { get; set; }
        public virtual GuestUser GuestUser { get; set; }
    }
}
