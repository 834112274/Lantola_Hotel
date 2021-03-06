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
    /// GuestUser
    /// </summary>
    public partial class GuestUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GuestUser()
        {
            this.Order = new HashSet<Order>();
            this.Comment = new HashSet<Comment>();
            this.Collection = new HashSet<Collection>();
            this.GuestInvoice = new HashSet<GuestInvoice>();
            this.Invoice = new HashSet<Invoice>();
        }
    
    	/// <summary>
        /// 
        /// </summary>
    	[DisplayName( "" )]
        public string Id { get; set; }
    	/// <summary>
        /// 用户名
        /// </summary>
    	[DisplayName( "用户名" )]
        public string Name { get; set; }
    	/// <summary>
        /// 密码
        /// </summary>
    	[DisplayName( "密码" )]
        public string Password { get; set; }
    	/// <summary>
        /// 昵称
        /// </summary>
    	[DisplayName( "昵称" )]
        public string NickName { get; set; }
    	/// <summary>
        /// 姓名
        /// </summary>
    	[DisplayName( "姓名" )]
        public string RealName { get; set; }
    	/// <summary>
        /// 最后登陆
        /// </summary>
    	[DisplayName( "最后登陆" )]
        public System.DateTime LastLogin { get; set; }
    	/// <summary>
        /// 手机
        /// </summary>
    	[DisplayName( "手机" )]
        public string Phone { get; set; }
    	/// <summary>
        /// QQ
        /// </summary>
    	[DisplayName( "QQ" )]
        public string QQ { get; set; }
    	/// <summary>
        /// 微信
        /// </summary>
    	[DisplayName( "微信" )]
        public string WeChat { get; set; }
    	/// <summary>
        /// 邮箱
        /// </summary>
    	[DisplayName( "邮箱" )]
        public string Email { get; set; }
    	/// <summary>
        /// 性别
        /// </summary>
    	[DisplayName( "性别" )]
        public short Sex { get; set; }
    	/// <summary>
        /// 城市
        /// </summary>
    	[DisplayName( "城市" )]
        public string City { get; set; }
    	/// <summary>
        /// 用户类型
        /// </summary>
    	[DisplayName( "用户类型" )]
        public short Type { get; set; }
    	/// <summary>
        /// 头像
        /// </summary>
    	[DisplayName( "头像" )]
        public string Head { get; set; }
    	/// <summary>
        /// 个人说明
        /// </summary>
    	[DisplayName( "个人说明" )]
        public string Summary { get; set; }
    	/// <summary>
        /// 注册方式
        /// </summary>
    	[DisplayName( "注册方式" )]
        public string RegisterType { get; set; }
    	/// <summary>
        /// 注册IP
        /// </summary>
    	[DisplayName( "注册IP" )]
        public string RegisterIp { get; set; }
    	/// <summary>
        /// 注册时间
        /// </summary>
    	[DisplayName( "注册时间" )]
        public System.DateTime RegisterTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Collection> Collection { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GuestInvoice> GuestInvoice { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual Company Company { get; set; }
    }
}
