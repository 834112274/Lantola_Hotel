
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/10/2018 16:19:19
-- Generated from EDMX file: F:\Project\github\Lantola_Hotel\HotelSystem.Model\DBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Lantola];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MenuUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_MenuUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_City_Province]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[City] DROP CONSTRAINT [FK_City_Province];
GO
IF OBJECT_ID(N'[dbo].[FK_District_City]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[District] DROP CONSTRAINT [FK_District_City];
GO
IF OBJECT_ID(N'[dbo].[FK_CityHotelInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HotelInfo] DROP CONSTRAINT [FK_CityHotelInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_DistrictHotelInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HotelInfo] DROP CONSTRAINT [FK_DistrictHotelInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoHotelImages]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HotelImages] DROP CONSTRAINT [FK_HotelInfoHotelImages];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoHotelPolicy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HotelPolicy] DROP CONSTRAINT [FK_HotelInfoHotelPolicy];
GO
IF OBJECT_ID(N'[dbo].[FK_PolicyHotelPolicy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HotelPolicy] DROP CONSTRAINT [FK_PolicyHotelPolicy];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoRestaurant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Restaurant] DROP CONSTRAINT [FK_HotelInfoRestaurant];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoGuide]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Guide] DROP CONSTRAINT [FK_HotelInfoGuide];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_HotelInfoActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoConference]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Conference] DROP CONSTRAINT [FK_HotelInfoConference];
GO
IF OBJECT_ID(N'[dbo].[FK_ConferenceConferenceRoom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ConferenceRoom] DROP CONSTRAINT [FK_ConferenceConferenceRoom];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoRoom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Room] DROP CONSTRAINT [FK_HotelInfoRoom];
GO
IF OBJECT_ID(N'[dbo].[FK_RoomRoomImages]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoomImages] DROP CONSTRAINT [FK_RoomRoomImages];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoHotelUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HotelUsers] DROP CONSTRAINT [FK_HotelInfoHotelUser];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoHotelService]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HotelService] DROP CONSTRAINT [FK_HotelInfoHotelService];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoFeatureHotel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FeatureHotel] DROP CONSTRAINT [FK_HotelInfoFeatureHotel];
GO
IF OBJECT_ID(N'[dbo].[FK_FeatureFeatureHotel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FeatureHotel] DROP CONSTRAINT [FK_FeatureFeatureHotel];
GO
IF OBJECT_ID(N'[dbo].[FK_GuestUserOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_GuestUserOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_HotelInfoOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comment] DROP CONSTRAINT [FK_OrderComment];
GO
IF OBJECT_ID(N'[dbo].[FK_GuestUserComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comment] DROP CONSTRAINT [FK_GuestUserComment];
GO
IF OBJECT_ID(N'[dbo].[FK_CollectionGuestUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Collection] DROP CONSTRAINT [FK_CollectionGuestUser];
GO
IF OBJECT_ID(N'[dbo].[FK_RoomPriceType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PriceType] DROP CONSTRAINT [FK_RoomPriceType];
GO
IF OBJECT_ID(N'[dbo].[FK_PriceTypePrice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK_PriceTypePrice];
GO
IF OBJECT_ID(N'[dbo].[FK_PriceTypeOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_PriceTypeOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_PriceTypeDaysPrice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DaysPrice] DROP CONSTRAINT [FK_PriceTypeDaysPrice];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderDaysPrice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DaysPrice] DROP CONSTRAINT [FK_OrderDaysPrice];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderOccupant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Occupant] DROP CONSTRAINT [FK_OrderOccupant];
GO
IF OBJECT_ID(N'[dbo].[FK_GuestUserGuestInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GuestInvoice] DROP CONSTRAINT [FK_GuestUserGuestInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_ProvinceInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [FK_ProvinceInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_CityInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [FK_CityInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_DistrictInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [FK_DistrictInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoCollection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Collection] DROP CONSTRAINT [FK_HotelInfoCollection];
GO
IF OBJECT_ID(N'[dbo].[FK_Order_transaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[transaction] DROP CONSTRAINT [FK_Order_transaction];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoScore]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Score] DROP CONSTRAINT [FK_HotelInfoScore];
GO
IF OBJECT_ID(N'[dbo].[FK_ScoreTypeScore]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Score] DROP CONSTRAINT [FK_ScoreTypeScore];
GO
IF OBJECT_ID(N'[dbo].[FK_CommentScore]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Score] DROP CONSTRAINT [FK_CommentScore];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelUsersReply]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reply] DROP CONSTRAINT [FK_HotelUsersReply];
GO
IF OBJECT_ID(N'[dbo].[FK_CommentReply]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reply] DROP CONSTRAINT [FK_CommentReply];
GO
IF OBJECT_ID(N'[dbo].[FK_RoomStock]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stock] DROP CONSTRAINT [FK_RoomStock];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_InvoiceOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_GuestUserInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [FK_GuestUserInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersSettlement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Settlement] DROP CONSTRAINT [FK_UsersSettlement];
GO
IF OBJECT_ID(N'[dbo].[FK_HotelInfoSettlement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Settlement] DROP CONSTRAINT [FK_HotelInfoSettlement];
GO
IF OBJECT_ID(N'[dbo].[FK_SettlementOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_SettlementOrder];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Menu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Menu];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[UserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRole];
GO
IF OBJECT_ID(N'[dbo].[City]', 'U') IS NOT NULL
    DROP TABLE [dbo].[City];
GO
IF OBJECT_ID(N'[dbo].[District]', 'U') IS NOT NULL
    DROP TABLE [dbo].[District];
GO
IF OBJECT_ID(N'[dbo].[Province]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Province];
GO
IF OBJECT_ID(N'[dbo].[HotelInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HotelInfo];
GO
IF OBJECT_ID(N'[dbo].[HotelImages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HotelImages];
GO
IF OBJECT_ID(N'[dbo].[Policy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Policy];
GO
IF OBJECT_ID(N'[dbo].[HotelPolicy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HotelPolicy];
GO
IF OBJECT_ID(N'[dbo].[Restaurant]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Restaurant];
GO
IF OBJECT_ID(N'[dbo].[Guide]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Guide];
GO
IF OBJECT_ID(N'[dbo].[Activity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Activity];
GO
IF OBJECT_ID(N'[dbo].[Conference]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Conference];
GO
IF OBJECT_ID(N'[dbo].[ConferenceRoom]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConferenceRoom];
GO
IF OBJECT_ID(N'[dbo].[Room]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Room];
GO
IF OBJECT_ID(N'[dbo].[RoomImages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoomImages];
GO
IF OBJECT_ID(N'[dbo].[HotelUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HotelUsers];
GO
IF OBJECT_ID(N'[dbo].[HotelService]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HotelService];
GO
IF OBJECT_ID(N'[dbo].[HomeConfig]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HomeConfig];
GO
IF OBJECT_ID(N'[dbo].[Feature]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Feature];
GO
IF OBJECT_ID(N'[dbo].[FeatureHotel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FeatureHotel];
GO
IF OBJECT_ID(N'[dbo].[Cooperative]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cooperative];
GO
IF OBJECT_ID(N'[dbo].[GuestUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GuestUser];
GO
IF OBJECT_ID(N'[dbo].[Order]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Order];
GO
IF OBJECT_ID(N'[dbo].[Comment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comment];
GO
IF OBJECT_ID(N'[dbo].[Collection]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Collection];
GO
IF OBJECT_ID(N'[dbo].[Message]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Message];
GO
IF OBJECT_ID(N'[dbo].[Article]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Article];
GO
IF OBJECT_ID(N'[dbo].[PriceType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PriceType];
GO
IF OBJECT_ID(N'[dbo].[Price]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Price];
GO
IF OBJECT_ID(N'[dbo].[Occupant]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Occupant];
GO
IF OBJECT_ID(N'[dbo].[Invoice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invoice];
GO
IF OBJECT_ID(N'[dbo].[DaysPrice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DaysPrice];
GO
IF OBJECT_ID(N'[dbo].[GuestInvoice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GuestInvoice];
GO
IF OBJECT_ID(N'[dbo].[transaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[transaction];
GO
IF OBJECT_ID(N'[dbo].[Score]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Score];
GO
IF OBJECT_ID(N'[dbo].[ScoreType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ScoreType];
GO
IF OBJECT_ID(N'[dbo].[Reply]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reply];
GO
IF OBJECT_ID(N'[dbo].[Stock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stock];
GO
IF OBJECT_ID(N'[dbo].[UserLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserLog];
GO
IF OBJECT_ID(N'[dbo].[Settlement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Settlement];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Menu'
CREATE TABLE [dbo].[Menu] (
    [Id] nvarchar(40)  NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [IcoClass] nvarchar(60)  NOT NULL,
    [Url] nvarchar(100)  NOT NULL,
    [ParentId] nvarchar(40)  NULL,
    [Level] smallint  NOT NULL,
    [Type] nvarchar(20)  NOT NULL,
    [Sort] smallint  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] nvarchar(40)  NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Password] nvarchar(40)  NOT NULL,
    [Role] nvarchar(10)  NOT NULL,
    [RegisterTime] datetime  NULL,
    [RegisterIP] nvarchar(20)  NOT NULL,
    [ParentId] nvarchar(40)  NULL,
    [Vail] bit  NOT NULL,
    [Summary] nvarchar(100)  NULL,
    [Status] smallint  NOT NULL
);
GO

-- Creating table 'UserRole'
CREATE TABLE [dbo].[UserRole] (
    [Id] nvarchar(40)  NOT NULL,
    [UsersId] nvarchar(40)  NOT NULL,
    [MenuId] nvarchar(40)  NOT NULL,
    [UserType] nvarchar(30)  NOT NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'City'
CREATE TABLE [dbo].[City] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [CityName] nvarchar(50)  NULL,
    [ZipCode] nvarchar(50)  NULL,
    [ProvinceID] bigint  NOT NULL,
    [DateCreated] datetime  NULL,
    [DateUpdated] datetime  NULL
);
GO

-- Creating table 'District'
CREATE TABLE [dbo].[District] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [DistrictName] nvarchar(50)  NULL,
    [CityID] bigint  NOT NULL,
    [DateCreated] datetime  NULL,
    [DateUpdated] datetime  NULL
);
GO

-- Creating table 'Province'
CREATE TABLE [dbo].[Province] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [ProvinceName] nvarchar(50)  NULL,
    [DateCreated] datetime  NULL,
    [DateUpdated] datetime  NULL
);
GO

-- Creating table 'HotelInfo'
CREATE TABLE [dbo].[HotelInfo] (
    [Id] nvarchar(40)  NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [EnglishName] nvarchar(80)  NULL,
    [Type] smallint  NOT NULL,
    [Adress] nvarchar(80)  NOT NULL,
    [Tel] nvarchar(20)  NULL,
    [Star] smallint  NOT NULL,
    [Contact] nvarchar(20)  NOT NULL,
    [Position] nvarchar(40)  NULL,
    [Phone] nvarchar(20)  NOT NULL,
    [Email] nvarchar(40)  NULL,
    [BusinessLicense] nvarchar(150)  NULL,
    [Franchise] nvarchar(150)  NULL,
    [Card] nvarchar(150)  NULL,
    [Apply] datetime  NOT NULL,
    [ExamineTime] datetime  NULL,
    [ExamineUser] nvarchar(40)  NULL,
    [ExamineStatus] int  NOT NULL,
    [DistrictId] bigint  NOT NULL,
    [CityId] bigint  NOT NULL,
    [OpeningTime] datetime  NULL,
    [Synopsis] nvarchar(2000)  NULL,
    [RenovationTime] datetime  NULL,
    [RoomCount] smallint  NULL,
    [DomainName] nvarchar(30)  NULL,
    [Valid] smallint  NULL
);
GO

-- Creating table 'HotelImages'
CREATE TABLE [dbo].[HotelImages] (
    [Id] nvarchar(40)  NOT NULL,
    [Url] nvarchar(200)  NOT NULL,
    [Summary] nvarchar(200)  NULL,
    [Type] int  NOT NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL,
    [Sort] int  NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'Policy'
CREATE TABLE [dbo].[Policy] (
    [Id] nvarchar(40)  NOT NULL,
    [Type] int  NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Values] nvarchar(200)  NULL,
    [Icon] nvarchar(150)  NULL,
    [CreateTime] datetime  NULL
);
GO

-- Creating table 'HotelPolicy'
CREATE TABLE [dbo].[HotelPolicy] (
    [Id] nvarchar(40)  NOT NULL,
    [Value] nvarchar(40)  NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL,
    [PolicyId] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'Restaurant'
CREATE TABLE [dbo].[Restaurant] (
    [Id] nvarchar(40)  NOT NULL,
    [Describe] nvarchar(300)  NULL,
    [BreakfastTime] nvarchar(60)  NULL,
    [LunchTime] nvarchar(60)  NULL,
    [DinnerTime] nvarchar(60)  NULL,
    [Seat] nvarchar(50)  NULL,
    [Clothing] nvarchar(100)  NULL,
    [Place] nvarchar(90)  NULL,
    [Reserve] nvarchar(200)  NULL,
    [Image1] nvarchar(150)  NULL,
    [Image2] nvarchar(150)  NULL,
    [Image3] nvarchar(150)  NULL,
    [HotelInfo_Id] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'Guide'
CREATE TABLE [dbo].[Guide] (
    [Id] nvarchar(40)  NOT NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL,
    [Type] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Describe] nvarchar(100)  NULL
);
GO

-- Creating table 'Activity'
CREATE TABLE [dbo].[Activity] (
    [Id] nvarchar(40)  NOT NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL,
    [Title] nvarchar(60)  NOT NULL,
    [Describe] nvarchar(400)  NULL,
    [Images] nvarchar(200)  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NULL,
    [Place] nvarchar(100)  NULL,
    [CreateTime] datetime  NULL,
    [Valid] bit  NOT NULL
);
GO

-- Creating table 'Conference'
CREATE TABLE [dbo].[Conference] (
    [Id] nvarchar(40)  NOT NULL,
    [RoomCount] int  NOT NULL,
    [MaxArea] nvarchar(40)  NULL,
    [OpenYear] nvarchar(10)  NULL,
    [NewestRenovation] nvarchar(10)  NULL,
    [GuestRoom] int  NOT NULL,
    [TeaBreak] nvarchar(50)  NULL,
    [Buffet] nvarchar(50)  NULL,
    [Banquet] nvarchar(50)  NULL,
    [CreateTime] datetime  NOT NULL,
    [HotelInfo_Id] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'ConferenceRoom'
CREATE TABLE [dbo].[ConferenceRoom] (
    [Id] nvarchar(40)  NOT NULL,
    [ConferenceId] nvarchar(40)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Area] nvarchar(40)  NULL,
    [Capacity] int  NOT NULL,
    [Storey] int  NOT NULL,
    [Size] nvarchar(20)  NULL,
    [Column] smallint  NOT NULL,
    [Layout] nvarchar(50)  NULL,
    [BusinessHours] nvarchar(60)  NULL,
    [Images] nvarchar(150)  NULL,
    [Price] nvarchar(20)  NULL,
    [HalfPrice] nvarchar(20)  NULL
);
GO

-- Creating table 'Room'
CREATE TABLE [dbo].[Room] (
    [Id] nvarchar(40)  NOT NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [EnglishName] nvarchar(40)  NULL,
    [Area] nvarchar(20)  NULL,
    [Storey] smallint  NOT NULL,
    [BedType] nvarchar(20)  NOT NULL,
    [BedDescribe] nvarchar(200)  NULL,
    [OccupancyNumber] smallint  NOT NULL,
    [Scenery] nvarchar(20)  NULL,
    [AddBed] nvarchar(20)  NOT NULL,
    [Number] smallint  NOT NULL,
    [Smokeless] nvarchar(20)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL,
    [Shower] bit  NOT NULL,
    [Bathtub] bit  NOT NULL,
    [HotSpring] bit  NOT NULL,
    [RoundBathtub] bit  NOT NULL,
    [WindowBathtub] bit  NOT NULL
);
GO

-- Creating table 'RoomImages'
CREATE TABLE [dbo].[RoomImages] (
    [Id] nvarchar(40)  NOT NULL,
    [RoomId] nvarchar(40)  NOT NULL,
    [Url] nvarchar(150)  NOT NULL,
    [Sort] smallint  NOT NULL,
    [Summary] nvarchar(200)  NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'HotelUsers'
CREATE TABLE [dbo].[HotelUsers] (
    [Id] nvarchar(40)  NOT NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Password] nvarchar(40)  NOT NULL,
    [Role] nvarchar(10)  NULL,
    [CreateTime] datetime  NOT NULL,
    [LastLogin] datetime  NOT NULL,
    [ParentId] nvarchar(40)  NULL,
    [Vail] bit  NOT NULL,
    [Summary] nvarchar(100)  NULL,
    [Status] smallint  NOT NULL
);
GO

-- Creating table 'HotelService'
CREATE TABLE [dbo].[HotelService] (
    [Id] nvarchar(40)  NOT NULL,
    [CheckInTime] nvarchar(20)  NULL,
    [LeaveTime] nvarchar(20)  NULL,
    [Pet] nvarchar(20)  NULL,
    [Buffet] nvarchar(30)  NULL,
    [CreditCard] nvarchar(300)  NULL,
    [HotelInfo_Id] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'HomeConfig'
CREATE TABLE [dbo].[HomeConfig] (
    [Id] nvarchar(40)  NOT NULL,
    [Title] nvarchar(50)  NOT NULL,
    [Summary] nvarchar(200)  NOT NULL,
    [ImgUrl] nvarchar(150)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL,
    [Valid] bit  NOT NULL
);
GO

-- Creating table 'Feature'
CREATE TABLE [dbo].[Feature] (
    [Id] nvarchar(40)  NOT NULL,
    [Name] nvarchar(60)  NOT NULL,
    [Summary] nvarchar(120)  NULL,
    [Ico] nvarchar(150)  NULL,
    [Type] smallint  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL,
    [Valid] bit  NOT NULL
);
GO

-- Creating table 'FeatureHotel'
CREATE TABLE [dbo].[FeatureHotel] (
    [Id] nvarchar(40)  NOT NULL,
    [Title] nvarchar(60)  NOT NULL,
    [Summary] nvarchar(120)  NULL,
    [ImgUrl] nvarchar(150)  NOT NULL,
    [Valid] bit  NOT NULL,
    [Sort] smallint  NOT NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL,
    [FeatureId] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'Cooperative'
CREATE TABLE [dbo].[Cooperative] (
    [Id] nvarchar(40)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [LogoUrl] nvarchar(150)  NOT NULL,
    [Summary] nvarchar(200)  NULL,
    [Url] nvarchar(150)  NULL,
    [CreateTime] datetime  NOT NULL
);
GO

-- Creating table 'GuestUser'
CREATE TABLE [dbo].[GuestUser] (
    [Id] nvarchar(40)  NOT NULL,
    [Name] nvarchar(24)  NOT NULL,
    [Password] nvarchar(40)  NOT NULL,
    [NickName] nvarchar(24)  NULL,
    [RealName] nvarchar(20)  NULL,
    [LastLogin] datetime  NOT NULL,
    [Phone] nvarchar(18)  NULL,
    [QQ] nvarchar(20)  NULL,
    [WeChat] nvarchar(40)  NULL,
    [Email] nvarchar(60)  NULL,
    [Sex] smallint  NOT NULL,
    [City] nvarchar(10)  NULL,
    [Type] smallint  NOT NULL,
    [Head] nvarchar(150)  NULL,
    [Summary] nvarchar(400)  NULL,
    [RegisterType] nvarchar(40)  NOT NULL,
    [RegisterIp] nvarchar(20)  NOT NULL,
    [RegisterTime] datetime  NOT NULL
);
GO

-- Creating table 'Order'
CREATE TABLE [dbo].[Order] (
    [Id] nvarchar(40)  NOT NULL,
    [Number] nvarchar(30)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [State] nvarchar(10)  NOT NULL,
    [IsValid] bit  NOT NULL,
    [Source] nvarchar(30)  NULL,
    [ApplyUser] nvarchar(40)  NOT NULL,
    [ApplyPhone] nvarchar(18)  NULL,
    [ApplyEmail] nvarchar(60)  NULL,
    [Remarks] nvarchar(80)  NULL,
    [Payment] bit  NOT NULL,
    [Total] float  NOT NULL,
    [BedType] nvarchar(20)  NULL,
    [Breakfast] nvarchar(20)  NULL,
    [Broadband] nvarchar(20)  NULL,
    [PriceName] nvarchar(40)  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [RoomCount] int  NOT NULL,
    [AvgPrice] float  NOT NULL,
    [RoomName] nvarchar(40)  NOT NULL,
    [HotelName] nvarchar(80)  NOT NULL,
    [GuestUserId] nvarchar(40)  NOT NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL,
    [PriceTypeId] nvarchar(40)  NOT NULL,
    [PaymentMethod] smallint  NOT NULL,
    [IsInvoice] smallint  NOT NULL,
    [HousingPrice] float  NOT NULL,
    [UpdateUser] nvarchar(50)  NULL,
    [UpdateTime] datetime  NULL,
    [CancelRemarks] nvarchar(200)  NULL,
    [PayTime] datetime  NULL,
    [InvoiceId] nvarchar(40)  NULL,
    [SettlementId] nvarchar(40)  NULL
);
GO

-- Creating table 'Comment'
CREATE TABLE [dbo].[Comment] (
    [Id] nvarchar(40)  NOT NULL,
    [Content] nvarchar(2000)  NOT NULL,
    [Images] nvarchar(600)  NULL,
    [ParentCommentId] nvarchar(40)  NULL,
    [CreateTime] datetime  NOT NULL,
    [OrderId] nvarchar(40)  NOT NULL,
    [GuestUserId] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'Collection'
CREATE TABLE [dbo].[Collection] (
    [Id] nvarchar(40)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [Remarks] nvarchar(200)  NULL,
    [GuestUserId] nvarchar(40)  NOT NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'Message'
CREATE TABLE [dbo].[Message] (
    [Id] nvarchar(40)  NOT NULL,
    [ToUser] nvarchar(40)  NOT NULL,
    [FromUser] nvarchar(40)  NOT NULL,
    [Title] nvarchar(100)  NOT NULL,
    [Content] nvarchar(1000)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [Send] bit  NOT NULL,
    [Read] bit  NOT NULL,
    [ParentMessageId] nvarchar(40)  NULL,
    [SendTime] datetime  NOT NULL,
    [RendTime] datetime  NOT NULL
);
GO

-- Creating table 'Article'
CREATE TABLE [dbo].[Article] (
    [Id] nvarchar(40)  NOT NULL,
    [Type] smallint  NOT NULL,
    [IsVaid] bit  NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [CreateUser] nvarchar(40)  NOT NULL,
    [UpdateUser] nvarchar(40)  NOT NULL,
    [UpdateTime] datetime  NOT NULL
);
GO

-- Creating table 'PriceType'
CREATE TABLE [dbo].[PriceType] (
    [Id] nvarchar(40)  NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Breakfast] nvarchar(40)  NULL,
    [Broadband] nvarchar(40)  NULL,
    [PaymentMethod] smallint  NOT NULL,
    [Number] nvarchar(20)  NOT NULL,
    [Remarks] nvarchar(50)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL,
    [CreateUser] nvarchar(40)  NOT NULL,
    [UpdateUser] nvarchar(40)  NOT NULL,
    [RoomId] nvarchar(40)  NOT NULL,
    [ExtraBedPrice] nvarchar(20)  NULL
);
GO

-- Creating table 'Price'
CREATE TABLE [dbo].[Price] (
    [Id] nvarchar(40)  NOT NULL,
    [Date] datetime  NOT NULL,
    [UnitPrice] float  NOT NULL,
    [PriceTypeId] nvarchar(40)  NOT NULL,
    [ConfigId] nvarchar(40)  NOT NULL,
    [Stock] smallint  NOT NULL,
    [SurplusStock] smallint  NOT NULL,
    [Status] smallint  NOT NULL
);
GO

-- Creating table 'Occupant'
CREATE TABLE [dbo].[Occupant] (
    [Id] nvarchar(40)  NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Sex] smallint  NULL,
    [Phone] nvarchar(18)  NULL,
    [OrderId] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'Invoice'
CREATE TABLE [dbo].[Invoice] (
    [Id] nvarchar(40)  NOT NULL,
    [Company] nvarchar(120)  NOT NULL,
    [Address] nvarchar(100)  NULL,
    [Tel] nvarchar(20)  NULL,
    [Bank] nvarchar(30)  NULL,
    [BankAccount] nvarchar(40)  NULL,
    [TaxpayerNumber] nvarchar(30)  NULL,
    [Addressee] nvarchar(20)  NOT NULL,
    [Phone] nvarchar(20)  NOT NULL,
    [ReceivingAddress] nvarchar(100)  NOT NULL,
    [Amount] float  NULL,
    [Express] nvarchar(40)  NULL,
    [ExpressNumber] nvarchar(20)  NULL,
    [ProvinceId] bigint  NOT NULL,
    [CityId] bigint  NOT NULL,
    [DistrictId] bigint  NOT NULL,
    [GuestUserId] nvarchar(40)  NOT NULL,
    [Status] smallint  NOT NULL,
    [Type] smallint  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL
);
GO

-- Creating table 'DaysPrice'
CREATE TABLE [dbo].[DaysPrice] (
    [Id] nvarchar(40)  NOT NULL,
    [Date] datetime  NOT NULL,
    [UnitPrice] float  NOT NULL,
    [ConfigId] nvarchar(40)  NOT NULL,
    [PriceTypeId] nvarchar(40)  NOT NULL,
    [OrderId] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'GuestInvoice'
CREATE TABLE [dbo].[GuestInvoice] (
    [Id] nvarchar(40)  NOT NULL,
    [Company] nvarchar(120)  NOT NULL,
    [Address] nvarchar(100)  NULL,
    [Tel] nvarchar(20)  NULL,
    [Bank] nvarchar(30)  NULL,
    [BankAccount] nvarchar(40)  NULL,
    [TaxpayerNumber] nvarchar(30)  NULL,
    [GuestUserId] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'transaction'
CREATE TABLE [dbo].[transaction] (
    [id] nvarchar(40)  NOT NULL,
    [data] nvarchar(1024)  NOT NULL,
    [Order_Id] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'Score'
CREATE TABLE [dbo].[Score] (
    [Id] nvarchar(40)  NOT NULL,
    [Value] smallint  NOT NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL,
    [ScoreTypeId] nvarchar(40)  NOT NULL,
    [CommentId] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'ScoreType'
CREATE TABLE [dbo].[ScoreType] (
    [Id] nvarchar(40)  NOT NULL,
    [Name] nvarchar(30)  NOT NULL,
    [Valid] bit  NOT NULL
);
GO

-- Creating table 'Reply'
CREATE TABLE [dbo].[Reply] (
    [Id] nvarchar(40)  NOT NULL,
    [Content] nvarchar(2000)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [HotelUsersId] nvarchar(40)  NOT NULL,
    [CommentId] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'Stock'
CREATE TABLE [dbo].[Stock] (
    [Id] nvarchar(40)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Total] smallint  NOT NULL,
    [SurplusStock] smallint  NOT NULL,
    [Status] smallint  NOT NULL,
    [RoomId] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'UserLog'
CREATE TABLE [dbo].[UserLog] (
    [Id] nvarchar(40)  NOT NULL,
    [Level] nvarchar(10)  NOT NULL,
    [TypeName] nvarchar(max)  NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [UserType] nvarchar(max)  NOT NULL,
    [UserName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Settlement'
CREATE TABLE [dbo].[Settlement] (
    [Id] nvarchar(40)  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [Amount] float  NOT NULL,
    [OrderCount] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UsersId] nvarchar(40)  NOT NULL,
    [HotelInfoId] nvarchar(40)  NOT NULL,
    [HotelName] nvarchar(80)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Menu'
ALTER TABLE [dbo].[Menu]
ADD CONSTRAINT [PK_Menu]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [PK_UserRole]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'City'
ALTER TABLE [dbo].[City]
ADD CONSTRAINT [PK_City]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'District'
ALTER TABLE [dbo].[District]
ADD CONSTRAINT [PK_District]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Province'
ALTER TABLE [dbo].[Province]
ADD CONSTRAINT [PK_Province]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HotelInfo'
ALTER TABLE [dbo].[HotelInfo]
ADD CONSTRAINT [PK_HotelInfo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HotelImages'
ALTER TABLE [dbo].[HotelImages]
ADD CONSTRAINT [PK_HotelImages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Policy'
ALTER TABLE [dbo].[Policy]
ADD CONSTRAINT [PK_Policy]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HotelPolicy'
ALTER TABLE [dbo].[HotelPolicy]
ADD CONSTRAINT [PK_HotelPolicy]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Restaurant'
ALTER TABLE [dbo].[Restaurant]
ADD CONSTRAINT [PK_Restaurant]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Guide'
ALTER TABLE [dbo].[Guide]
ADD CONSTRAINT [PK_Guide]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Activity'
ALTER TABLE [dbo].[Activity]
ADD CONSTRAINT [PK_Activity]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Conference'
ALTER TABLE [dbo].[Conference]
ADD CONSTRAINT [PK_Conference]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ConferenceRoom'
ALTER TABLE [dbo].[ConferenceRoom]
ADD CONSTRAINT [PK_ConferenceRoom]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Room'
ALTER TABLE [dbo].[Room]
ADD CONSTRAINT [PK_Room]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoomImages'
ALTER TABLE [dbo].[RoomImages]
ADD CONSTRAINT [PK_RoomImages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HotelUsers'
ALTER TABLE [dbo].[HotelUsers]
ADD CONSTRAINT [PK_HotelUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HotelService'
ALTER TABLE [dbo].[HotelService]
ADD CONSTRAINT [PK_HotelService]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HomeConfig'
ALTER TABLE [dbo].[HomeConfig]
ADD CONSTRAINT [PK_HomeConfig]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Feature'
ALTER TABLE [dbo].[Feature]
ADD CONSTRAINT [PK_Feature]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FeatureHotel'
ALTER TABLE [dbo].[FeatureHotel]
ADD CONSTRAINT [PK_FeatureHotel]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cooperative'
ALTER TABLE [dbo].[Cooperative]
ADD CONSTRAINT [PK_Cooperative]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GuestUser'
ALTER TABLE [dbo].[GuestUser]
ADD CONSTRAINT [PK_GuestUser]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Order'
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [PK_Order]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Comment'
ALTER TABLE [dbo].[Comment]
ADD CONSTRAINT [PK_Comment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Collection'
ALTER TABLE [dbo].[Collection]
ADD CONSTRAINT [PK_Collection]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Message'
ALTER TABLE [dbo].[Message]
ADD CONSTRAINT [PK_Message]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Article'
ALTER TABLE [dbo].[Article]
ADD CONSTRAINT [PK_Article]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PriceType'
ALTER TABLE [dbo].[PriceType]
ADD CONSTRAINT [PK_PriceType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Price'
ALTER TABLE [dbo].[Price]
ADD CONSTRAINT [PK_Price]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Occupant'
ALTER TABLE [dbo].[Occupant]
ADD CONSTRAINT [PK_Occupant]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [PK_Invoice]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DaysPrice'
ALTER TABLE [dbo].[DaysPrice]
ADD CONSTRAINT [PK_DaysPrice]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GuestInvoice'
ALTER TABLE [dbo].[GuestInvoice]
ADD CONSTRAINT [PK_GuestInvoice]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'transaction'
ALTER TABLE [dbo].[transaction]
ADD CONSTRAINT [PK_transaction]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'Score'
ALTER TABLE [dbo].[Score]
ADD CONSTRAINT [PK_Score]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ScoreType'
ALTER TABLE [dbo].[ScoreType]
ADD CONSTRAINT [PK_ScoreType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reply'
ALTER TABLE [dbo].[Reply]
ADD CONSTRAINT [PK_Reply]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Stock'
ALTER TABLE [dbo].[Stock]
ADD CONSTRAINT [PK_Stock]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserLog'
ALTER TABLE [dbo].[UserLog]
ADD CONSTRAINT [PK_UserLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Settlement'
ALTER TABLE [dbo].[Settlement]
ADD CONSTRAINT [PK_Settlement]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MenuId] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [FK_MenuUserRole]
    FOREIGN KEY ([MenuId])
    REFERENCES [dbo].[Menu]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MenuUserRole'
CREATE INDEX [IX_FK_MenuUserRole]
ON [dbo].[UserRole]
    ([MenuId]);
GO

-- Creating foreign key on [ProvinceID] in table 'City'
ALTER TABLE [dbo].[City]
ADD CONSTRAINT [FK_City_Province]
    FOREIGN KEY ([ProvinceID])
    REFERENCES [dbo].[Province]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_City_Province'
CREATE INDEX [IX_FK_City_Province]
ON [dbo].[City]
    ([ProvinceID]);
GO

-- Creating foreign key on [CityID] in table 'District'
ALTER TABLE [dbo].[District]
ADD CONSTRAINT [FK_District_City]
    FOREIGN KEY ([CityID])
    REFERENCES [dbo].[City]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_District_City'
CREATE INDEX [IX_FK_District_City]
ON [dbo].[District]
    ([CityID]);
GO

-- Creating foreign key on [CityId] in table 'HotelInfo'
ALTER TABLE [dbo].[HotelInfo]
ADD CONSTRAINT [FK_CityHotelInfo]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[City]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CityHotelInfo'
CREATE INDEX [IX_FK_CityHotelInfo]
ON [dbo].[HotelInfo]
    ([CityId]);
GO

-- Creating foreign key on [DistrictId] in table 'HotelInfo'
ALTER TABLE [dbo].[HotelInfo]
ADD CONSTRAINT [FK_DistrictHotelInfo]
    FOREIGN KEY ([DistrictId])
    REFERENCES [dbo].[District]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DistrictHotelInfo'
CREATE INDEX [IX_FK_DistrictHotelInfo]
ON [dbo].[HotelInfo]
    ([DistrictId]);
GO

-- Creating foreign key on [HotelInfoId] in table 'HotelImages'
ALTER TABLE [dbo].[HotelImages]
ADD CONSTRAINT [FK_HotelInfoHotelImages]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoHotelImages'
CREATE INDEX [IX_FK_HotelInfoHotelImages]
ON [dbo].[HotelImages]
    ([HotelInfoId]);
GO

-- Creating foreign key on [HotelInfoId] in table 'HotelPolicy'
ALTER TABLE [dbo].[HotelPolicy]
ADD CONSTRAINT [FK_HotelInfoHotelPolicy]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoHotelPolicy'
CREATE INDEX [IX_FK_HotelInfoHotelPolicy]
ON [dbo].[HotelPolicy]
    ([HotelInfoId]);
GO

-- Creating foreign key on [PolicyId] in table 'HotelPolicy'
ALTER TABLE [dbo].[HotelPolicy]
ADD CONSTRAINT [FK_PolicyHotelPolicy]
    FOREIGN KEY ([PolicyId])
    REFERENCES [dbo].[Policy]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PolicyHotelPolicy'
CREATE INDEX [IX_FK_PolicyHotelPolicy]
ON [dbo].[HotelPolicy]
    ([PolicyId]);
GO

-- Creating foreign key on [HotelInfo_Id] in table 'Restaurant'
ALTER TABLE [dbo].[Restaurant]
ADD CONSTRAINT [FK_HotelInfoRestaurant]
    FOREIGN KEY ([HotelInfo_Id])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoRestaurant'
CREATE INDEX [IX_FK_HotelInfoRestaurant]
ON [dbo].[Restaurant]
    ([HotelInfo_Id]);
GO

-- Creating foreign key on [HotelInfoId] in table 'Guide'
ALTER TABLE [dbo].[Guide]
ADD CONSTRAINT [FK_HotelInfoGuide]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoGuide'
CREATE INDEX [IX_FK_HotelInfoGuide]
ON [dbo].[Guide]
    ([HotelInfoId]);
GO

-- Creating foreign key on [HotelInfoId] in table 'Activity'
ALTER TABLE [dbo].[Activity]
ADD CONSTRAINT [FK_HotelInfoActivity]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoActivity'
CREATE INDEX [IX_FK_HotelInfoActivity]
ON [dbo].[Activity]
    ([HotelInfoId]);
GO

-- Creating foreign key on [HotelInfo_Id] in table 'Conference'
ALTER TABLE [dbo].[Conference]
ADD CONSTRAINT [FK_HotelInfoConference]
    FOREIGN KEY ([HotelInfo_Id])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoConference'
CREATE INDEX [IX_FK_HotelInfoConference]
ON [dbo].[Conference]
    ([HotelInfo_Id]);
GO

-- Creating foreign key on [ConferenceId] in table 'ConferenceRoom'
ALTER TABLE [dbo].[ConferenceRoom]
ADD CONSTRAINT [FK_ConferenceConferenceRoom]
    FOREIGN KEY ([ConferenceId])
    REFERENCES [dbo].[Conference]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ConferenceConferenceRoom'
CREATE INDEX [IX_FK_ConferenceConferenceRoom]
ON [dbo].[ConferenceRoom]
    ([ConferenceId]);
GO

-- Creating foreign key on [HotelInfoId] in table 'Room'
ALTER TABLE [dbo].[Room]
ADD CONSTRAINT [FK_HotelInfoRoom]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoRoom'
CREATE INDEX [IX_FK_HotelInfoRoom]
ON [dbo].[Room]
    ([HotelInfoId]);
GO

-- Creating foreign key on [RoomId] in table 'RoomImages'
ALTER TABLE [dbo].[RoomImages]
ADD CONSTRAINT [FK_RoomRoomImages]
    FOREIGN KEY ([RoomId])
    REFERENCES [dbo].[Room]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoomRoomImages'
CREATE INDEX [IX_FK_RoomRoomImages]
ON [dbo].[RoomImages]
    ([RoomId]);
GO

-- Creating foreign key on [HotelInfoId] in table 'HotelUsers'
ALTER TABLE [dbo].[HotelUsers]
ADD CONSTRAINT [FK_HotelInfoHotelUser]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoHotelUser'
CREATE INDEX [IX_FK_HotelInfoHotelUser]
ON [dbo].[HotelUsers]
    ([HotelInfoId]);
GO

-- Creating foreign key on [HotelInfo_Id] in table 'HotelService'
ALTER TABLE [dbo].[HotelService]
ADD CONSTRAINT [FK_HotelInfoHotelService]
    FOREIGN KEY ([HotelInfo_Id])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoHotelService'
CREATE INDEX [IX_FK_HotelInfoHotelService]
ON [dbo].[HotelService]
    ([HotelInfo_Id]);
GO

-- Creating foreign key on [HotelInfoId] in table 'FeatureHotel'
ALTER TABLE [dbo].[FeatureHotel]
ADD CONSTRAINT [FK_HotelInfoFeatureHotel]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoFeatureHotel'
CREATE INDEX [IX_FK_HotelInfoFeatureHotel]
ON [dbo].[FeatureHotel]
    ([HotelInfoId]);
GO

-- Creating foreign key on [FeatureId] in table 'FeatureHotel'
ALTER TABLE [dbo].[FeatureHotel]
ADD CONSTRAINT [FK_FeatureFeatureHotel]
    FOREIGN KEY ([FeatureId])
    REFERENCES [dbo].[Feature]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FeatureFeatureHotel'
CREATE INDEX [IX_FK_FeatureFeatureHotel]
ON [dbo].[FeatureHotel]
    ([FeatureId]);
GO

-- Creating foreign key on [GuestUserId] in table 'Order'
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [FK_GuestUserOrder]
    FOREIGN KEY ([GuestUserId])
    REFERENCES [dbo].[GuestUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GuestUserOrder'
CREATE INDEX [IX_FK_GuestUserOrder]
ON [dbo].[Order]
    ([GuestUserId]);
GO

-- Creating foreign key on [HotelInfoId] in table 'Order'
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [FK_HotelInfoOrder]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoOrder'
CREATE INDEX [IX_FK_HotelInfoOrder]
ON [dbo].[Order]
    ([HotelInfoId]);
GO

-- Creating foreign key on [OrderId] in table 'Comment'
ALTER TABLE [dbo].[Comment]
ADD CONSTRAINT [FK_OrderComment]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Order]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderComment'
CREATE INDEX [IX_FK_OrderComment]
ON [dbo].[Comment]
    ([OrderId]);
GO

-- Creating foreign key on [GuestUserId] in table 'Comment'
ALTER TABLE [dbo].[Comment]
ADD CONSTRAINT [FK_GuestUserComment]
    FOREIGN KEY ([GuestUserId])
    REFERENCES [dbo].[GuestUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GuestUserComment'
CREATE INDEX [IX_FK_GuestUserComment]
ON [dbo].[Comment]
    ([GuestUserId]);
GO

-- Creating foreign key on [GuestUserId] in table 'Collection'
ALTER TABLE [dbo].[Collection]
ADD CONSTRAINT [FK_CollectionGuestUser]
    FOREIGN KEY ([GuestUserId])
    REFERENCES [dbo].[GuestUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CollectionGuestUser'
CREATE INDEX [IX_FK_CollectionGuestUser]
ON [dbo].[Collection]
    ([GuestUserId]);
GO

-- Creating foreign key on [RoomId] in table 'PriceType'
ALTER TABLE [dbo].[PriceType]
ADD CONSTRAINT [FK_RoomPriceType]
    FOREIGN KEY ([RoomId])
    REFERENCES [dbo].[Room]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoomPriceType'
CREATE INDEX [IX_FK_RoomPriceType]
ON [dbo].[PriceType]
    ([RoomId]);
GO

-- Creating foreign key on [PriceTypeId] in table 'Price'
ALTER TABLE [dbo].[Price]
ADD CONSTRAINT [FK_PriceTypePrice]
    FOREIGN KEY ([PriceTypeId])
    REFERENCES [dbo].[PriceType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PriceTypePrice'
CREATE INDEX [IX_FK_PriceTypePrice]
ON [dbo].[Price]
    ([PriceTypeId]);
GO

-- Creating foreign key on [PriceTypeId] in table 'Order'
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [FK_PriceTypeOrder]
    FOREIGN KEY ([PriceTypeId])
    REFERENCES [dbo].[PriceType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PriceTypeOrder'
CREATE INDEX [IX_FK_PriceTypeOrder]
ON [dbo].[Order]
    ([PriceTypeId]);
GO

-- Creating foreign key on [PriceTypeId] in table 'DaysPrice'
ALTER TABLE [dbo].[DaysPrice]
ADD CONSTRAINT [FK_PriceTypeDaysPrice]
    FOREIGN KEY ([PriceTypeId])
    REFERENCES [dbo].[PriceType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PriceTypeDaysPrice'
CREATE INDEX [IX_FK_PriceTypeDaysPrice]
ON [dbo].[DaysPrice]
    ([PriceTypeId]);
GO

-- Creating foreign key on [OrderId] in table 'DaysPrice'
ALTER TABLE [dbo].[DaysPrice]
ADD CONSTRAINT [FK_OrderDaysPrice]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Order]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderDaysPrice'
CREATE INDEX [IX_FK_OrderDaysPrice]
ON [dbo].[DaysPrice]
    ([OrderId]);
GO

-- Creating foreign key on [OrderId] in table 'Occupant'
ALTER TABLE [dbo].[Occupant]
ADD CONSTRAINT [FK_OrderOccupant]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Order]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderOccupant'
CREATE INDEX [IX_FK_OrderOccupant]
ON [dbo].[Occupant]
    ([OrderId]);
GO

-- Creating foreign key on [GuestUserId] in table 'GuestInvoice'
ALTER TABLE [dbo].[GuestInvoice]
ADD CONSTRAINT [FK_GuestUserGuestInvoice]
    FOREIGN KEY ([GuestUserId])
    REFERENCES [dbo].[GuestUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GuestUserGuestInvoice'
CREATE INDEX [IX_FK_GuestUserGuestInvoice]
ON [dbo].[GuestInvoice]
    ([GuestUserId]);
GO

-- Creating foreign key on [ProvinceId] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [FK_ProvinceInvoice]
    FOREIGN KEY ([ProvinceId])
    REFERENCES [dbo].[Province]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProvinceInvoice'
CREATE INDEX [IX_FK_ProvinceInvoice]
ON [dbo].[Invoice]
    ([ProvinceId]);
GO

-- Creating foreign key on [CityId] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [FK_CityInvoice]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[City]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CityInvoice'
CREATE INDEX [IX_FK_CityInvoice]
ON [dbo].[Invoice]
    ([CityId]);
GO

-- Creating foreign key on [DistrictId] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [FK_DistrictInvoice]
    FOREIGN KEY ([DistrictId])
    REFERENCES [dbo].[District]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DistrictInvoice'
CREATE INDEX [IX_FK_DistrictInvoice]
ON [dbo].[Invoice]
    ([DistrictId]);
GO

-- Creating foreign key on [HotelInfoId] in table 'Collection'
ALTER TABLE [dbo].[Collection]
ADD CONSTRAINT [FK_HotelInfoCollection]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoCollection'
CREATE INDEX [IX_FK_HotelInfoCollection]
ON [dbo].[Collection]
    ([HotelInfoId]);
GO

-- Creating foreign key on [Order_Id] in table 'transaction'
ALTER TABLE [dbo].[transaction]
ADD CONSTRAINT [FK_Order_transaction]
    FOREIGN KEY ([Order_Id])
    REFERENCES [dbo].[Order]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Order_transaction'
CREATE INDEX [IX_FK_Order_transaction]
ON [dbo].[transaction]
    ([Order_Id]);
GO

-- Creating foreign key on [HotelInfoId] in table 'Score'
ALTER TABLE [dbo].[Score]
ADD CONSTRAINT [FK_HotelInfoScore]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoScore'
CREATE INDEX [IX_FK_HotelInfoScore]
ON [dbo].[Score]
    ([HotelInfoId]);
GO

-- Creating foreign key on [ScoreTypeId] in table 'Score'
ALTER TABLE [dbo].[Score]
ADD CONSTRAINT [FK_ScoreTypeScore]
    FOREIGN KEY ([ScoreTypeId])
    REFERENCES [dbo].[ScoreType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ScoreTypeScore'
CREATE INDEX [IX_FK_ScoreTypeScore]
ON [dbo].[Score]
    ([ScoreTypeId]);
GO

-- Creating foreign key on [CommentId] in table 'Score'
ALTER TABLE [dbo].[Score]
ADD CONSTRAINT [FK_CommentScore]
    FOREIGN KEY ([CommentId])
    REFERENCES [dbo].[Comment]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommentScore'
CREATE INDEX [IX_FK_CommentScore]
ON [dbo].[Score]
    ([CommentId]);
GO

-- Creating foreign key on [HotelUsersId] in table 'Reply'
ALTER TABLE [dbo].[Reply]
ADD CONSTRAINT [FK_HotelUsersReply]
    FOREIGN KEY ([HotelUsersId])
    REFERENCES [dbo].[HotelUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelUsersReply'
CREATE INDEX [IX_FK_HotelUsersReply]
ON [dbo].[Reply]
    ([HotelUsersId]);
GO

-- Creating foreign key on [CommentId] in table 'Reply'
ALTER TABLE [dbo].[Reply]
ADD CONSTRAINT [FK_CommentReply]
    FOREIGN KEY ([CommentId])
    REFERENCES [dbo].[Comment]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommentReply'
CREATE INDEX [IX_FK_CommentReply]
ON [dbo].[Reply]
    ([CommentId]);
GO

-- Creating foreign key on [RoomId] in table 'Stock'
ALTER TABLE [dbo].[Stock]
ADD CONSTRAINT [FK_RoomStock]
    FOREIGN KEY ([RoomId])
    REFERENCES [dbo].[Room]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoomStock'
CREATE INDEX [IX_FK_RoomStock]
ON [dbo].[Stock]
    ([RoomId]);
GO

-- Creating foreign key on [InvoiceId] in table 'Order'
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [FK_InvoiceOrder]
    FOREIGN KEY ([InvoiceId])
    REFERENCES [dbo].[Invoice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceOrder'
CREATE INDEX [IX_FK_InvoiceOrder]
ON [dbo].[Order]
    ([InvoiceId]);
GO

-- Creating foreign key on [GuestUserId] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [FK_GuestUserInvoice]
    FOREIGN KEY ([GuestUserId])
    REFERENCES [dbo].[GuestUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GuestUserInvoice'
CREATE INDEX [IX_FK_GuestUserInvoice]
ON [dbo].[Invoice]
    ([GuestUserId]);
GO

-- Creating foreign key on [UsersId] in table 'Settlement'
ALTER TABLE [dbo].[Settlement]
ADD CONSTRAINT [FK_UsersSettlement]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersSettlement'
CREATE INDEX [IX_FK_UsersSettlement]
ON [dbo].[Settlement]
    ([UsersId]);
GO

-- Creating foreign key on [HotelInfoId] in table 'Settlement'
ALTER TABLE [dbo].[Settlement]
ADD CONSTRAINT [FK_HotelInfoSettlement]
    FOREIGN KEY ([HotelInfoId])
    REFERENCES [dbo].[HotelInfo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelInfoSettlement'
CREATE INDEX [IX_FK_HotelInfoSettlement]
ON [dbo].[Settlement]
    ([HotelInfoId]);
GO

-- Creating foreign key on [SettlementId] in table 'Order'
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [FK_SettlementOrder]
    FOREIGN KEY ([SettlementId])
    REFERENCES [dbo].[Settlement]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SettlementOrder'
CREATE INDEX [IX_FK_SettlementOrder]
ON [dbo].[Order]
    ([SettlementId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------