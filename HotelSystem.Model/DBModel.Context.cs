﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBModelContainer : DbContext
    {
        public DBModelContainer()
            : base("name=DBModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<HotelInfo> HotelInfo { get; set; }
        public virtual DbSet<HotelImages> HotelImages { get; set; }
        public virtual DbSet<Policy> Policy { get; set; }
        public virtual DbSet<HotelPolicy> HotelPolicy { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<Guide> Guide { get; set; }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Conference> Conference { get; set; }
        public virtual DbSet<ConferenceRoom> ConferenceRoom { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<RoomImages> RoomImages { get; set; }
        public virtual DbSet<HotelUsers> HotelUsers { get; set; }
        public virtual DbSet<HotelService> HotelService { get; set; }
        public virtual DbSet<HomeConfig> HomeConfig { get; set; }
        public virtual DbSet<Feature> Feature { get; set; }
        public virtual DbSet<FeatureHotel> FeatureHotel { get; set; }
        public virtual DbSet<Cooperative> Cooperative { get; set; }
        public virtual DbSet<GuestUser> GuestUser { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<PriceType> PriceType { get; set; }
        public virtual DbSet<Price> Price { get; set; }
        public virtual DbSet<Occupant> Occupant { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<DaysPrice> DaysPrice { get; set; }
        public virtual DbSet<GuestInvoice> GuestInvoice { get; set; }
        public virtual DbSet<transaction> transaction { get; set; }
        public virtual DbSet<Score> Score { get; set; }
        public virtual DbSet<ScoreType> ScoreType { get; set; }
        public virtual DbSet<Reply> Reply { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<UserLog> UserLog { get; set; }
        public virtual DbSet<Settlement> Settlement { get; set; }
        public virtual DbSet<Company> Company { get; set; }
    }
}
