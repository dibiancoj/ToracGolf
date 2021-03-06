USE [master]
GO
/****** Object:  Database [ToracGolf]    Script Date: 1/17/2016 12:10:53 PM ******/
CREATE DATABASE [ToracGolf]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ToracGolf', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\ToracGolf.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ToracGolf_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\ToracGolf_log.ldf' , SIZE = 5696KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ToracGolf] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ToracGolf].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ToracGolf] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ToracGolf] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ToracGolf] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ToracGolf] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ToracGolf] SET ARITHABORT OFF 
GO
ALTER DATABASE [ToracGolf] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ToracGolf] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ToracGolf] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ToracGolf] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ToracGolf] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ToracGolf] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ToracGolf] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ToracGolf] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ToracGolf] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ToracGolf] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ToracGolf] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ToracGolf] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ToracGolf] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ToracGolf] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ToracGolf] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ToracGolf] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ToracGolf] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ToracGolf] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ToracGolf] SET  MULTI_USER 
GO
ALTER DATABASE [ToracGolf] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ToracGolf] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ToracGolf] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ToracGolf] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ToracGolf] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ToracGolf', N'ON'
GO
USE [ToracGolf]
GO
/****** Object:  User [ToracGolfV2User]    Script Date: 1/17/2016 12:10:54 PM ******/
CREATE USER [ToracGolfV2User] FOR LOGIN [ToracGolfV2User] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [ToracGolfV2]    Script Date: 1/17/2016 12:10:54 PM ******/
CREATE USER [ToracGolfV2] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [sql_dependency_role]    Script Date: 1/17/2016 12:10:54 PM ******/
CREATE ROLE [sql_dependency_role]
GO
ALTER ROLE [db_datareader] ADD MEMBER [ToracGolfV2User]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [ToracGolfV2User]
GO
ALTER ROLE [db_owner] ADD MEMBER [ToracGolfV2]
GO
ALTER ROLE [db_datareader] ADD MEMBER [ToracGolfV2]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [ToracGolfV2]
GO
/****** Object:  Schema [sql_dependency_role]    Script Date: 1/17/2016 12:10:54 PM ******/
CREATE SCHEMA [sql_dependency_role]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Course](
	[CourseId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](75) NOT NULL,
	[City] [varchar](100) NOT NULL,
	[StateId] [int] NOT NULL,
	[Description] [varchar](200) NOT NULL,
	[OnlyAllow18Holes] [bit] NOT NULL,
	[UserIdThatCreatedCourse] [int] NOT NULL,
	[Pending] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Course_CreatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CourseTeeLocations]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CourseTeeLocations](
	[CourseTeeLocationId] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[TeeLocationSortOrderId] [int] NOT NULL,
	[Yardage] [int] NOT NULL,
	[Front9Par] [int] NOT NULL,
	[Back9Par] [int] NOT NULL,
	[CoursePar]  AS ([Front9Par]+[Front9Par]),
	[Rating] [float] NOT NULL,
	[Slope] [float] NOT NULL,
	[FairwaysOnCourse] [int] NOT NULL,
 CONSTRAINT [PK_CourseTeeLocations_1] PRIMARY KEY CLUSTERED 
(
	[CourseTeeLocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Handicap]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Handicap](
	[RoundId] [int] NOT NULL,
	[HandicapBeforeRound] [float] NOT NULL,
	[HandicapAfterRound] [float] NOT NULL,
 CONSTRAINT [PK_RoundHandicap] PRIMARY KEY CLUSTERED 
(
	[RoundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NewsFeedComment]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NewsFeedComment](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[NewsFeedTypeId] [int] NOT NULL,
	[AreaId] [int] NOT NULL,
	[UserIdThatCommented] [int] NOT NULL,
	[Comment] [varchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_NewsFeedComment] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NewsFeedLike]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsFeedLike](
	[NewsFeedTypeId] [int] NOT NULL,
	[AreaId] [int] NOT NULL,
	[UserIdThatLikedItem] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_NewsFeedLike_CreatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_NewsFeedLike] PRIMARY KEY CLUSTERED 
(
	[NewsFeedTypeId] ASC,
	[AreaId] ASC,
	[UserIdThatLikedItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ref_NewsFeedTypeId]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ref_NewsFeedTypeId](
	[NewsFeedTypeId] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[NewsFeedTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ref_Season]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ref_Season](
	[SeasonId] [int] IDENTITY(1,1) NOT NULL,
	[SeasonText] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Seasons_CreatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Seasons] PRIMARY KEY CLUSTERED 
(
	[SeasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ref_State]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ref_State](
	[StateId] [int] NOT NULL,
	[Description] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Ref_States] PRIMARY KEY CLUSTERED 
(
	[StateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Round]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Round](
	[RoundId] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NOT NULL,
	[CourseTeeLocationId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[SeasonId] [int] NOT NULL,
	[RoundDate] [datetime] NOT NULL,
	[Score] [int] NOT NULL,
	[Is9HoleScore] [bit] NOT NULL,
	[RoundHandicap] [float] NOT NULL,
	[GreensInRegulation] [int] NULL,
	[FairwaysHit] [int] NULL,
	[Putts] [int] NULL,
 CONSTRAINT [PK_Round] PRIMARY KEY CLUSTERED 
(
	[RoundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserAccounts]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAccounts](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[EmailAddress] [varchar](100) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[CurrentSeasonId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Users_CreatedDate]  DEFAULT (getdate()),
	[StateId] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserSeason]    Script Date: 1/17/2016 12:10:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSeason](
	[UserId] [int] NOT NULL,
	[SeasonId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_UserSeason_CreateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_UserSeason] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[SeasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Course] ON 

GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive], [CreatedDate]) VALUES (1, N'Richter Park', N'Danbury', 7, N'Town owned course which is very nice. Course is long and on the harder side. Majority of the fairways are lined with trees where driving accuracy becomes crucial to score well.', 0, 21, 1, 1, CAST(N'2015-12-27 18:55:03.007' AS DateTime))
GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive], [CreatedDate]) VALUES (2, N'Hudson Hills', N'Ossining', 35, N'Toughest of the Westchester County golf courses. Many blind tee shots force precise and accurate tee shots.', 0, 21, 1, 1, CAST(N'2015-11-27 18:55:03.007' AS DateTime))
GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive], [CreatedDate]) VALUES (3, N'Patriot Hills Golf Club', N'Stony Point', 35, N'Hilly course with many elevated greens. Many shots come on uneven lies. Course has great views of the mountain ranges of Rockland County', 0, 21, 1, 1, CAST(N'2015-12-15 18:55:03.007' AS DateTime))
GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive], [CreatedDate]) VALUES (4, N'River Vale Country Club', N'River Vale', 32, N'Nice course but at over $100 this is way overpriced. Greens are usually firmed and fast which gives the course some grit.', 0, 21, 1, 1, CAST(N'2015-12-01 18:55:03.007' AS DateTime))
GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive], [CreatedDate]) VALUES (5, N'E. Gaynor Brennan Golf Course', N'Stamford', 7, N'Town owned coursed. Nothing really special. Good rates. Course is on the easier side.', 0, 21, 1, 1, CAST(N'2015-11-29 18:55:03.007' AS DateTime))
GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive], [CreatedDate]) VALUES (6, N'Sprain Lake Golf Course', N'Yonkers', 35, N'County owned course where water comes into play on many of the holes. Course is a little run down but the views over the lake are great. Course is on the short and easy side. Fun course though.', 0, 21, 1, 1, CAST(N'2015-12-05 18:55:03.007' AS DateTime))
GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive], [CreatedDate]) VALUES (7, N'Dunwoodie Golf Course', N'Yonkers', 35, N'County course which plays really slow on the weekend. Course is great for beginners as it plays easy and short.', 0, 21, 1, 1, CAST(N'2015-12-06 18:55:03.007' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Course] OFF
GO
SET IDENTITY_INSERT [dbo].[CourseTeeLocations] ON 

GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (39, 1, N'Blue (Back)', 0, 6744, 36, 36, 73.6, 139, 14)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (40, 1, N'White (Middle)', 1, 6304, 36, 36, 71.6, 136, 14)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (41, 1, N'Gold (Front)', 2, 5114, 35, 37, 72.9, 129, 14)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (42, 2, N'Black (Back)', 0, 6935, 35, 36, 73.7, 139, 13)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (43, 2, N'Green (Middle)', 1, 6323, 35, 36, 71, 129, 13)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (44, 2, N'Blue (Front)', 2, 5755, 35, 36, 68, 126, 13)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (45, 2, N'Gold (Ladies)', 3, 5102, 35, 36, 66.7, 113, 13)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (46, 3, N'Gold (Back)', 0, 6485, 36, 35, 72, 136, 13)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (47, 3, N'Blue (Middle)', 1, 6111, 36, 35, 70.4, 134, 13)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (48, 3, N'White (Front)', 2, 5599, 36, 35, 68, 128, 13)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (49, 4, N'Blue (Back)', 0, 6504, 36, 36, 71.4, 132, 14)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (50, 4, N'White (Middle)', 1, 6168, 36, 36, 69.7, 129, 14)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (51, 4, N'Gold (Front)', 2, 5892, 36, 36, 68.7, 126, 14)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (52, 5, N'Blue (Back)', 0, 5931, 36, 35, 69.5, 121, 14)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (53, 5, N'White (Middle)', 1, 5577, 36, 35, 68, 118, 14)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (54, 6, N'Middle', 0, 5631, 34, 36, 67.2, 120, 14)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (55, 6, N'Back', 1, 6110, 34, 36, 69.3, 124, 14)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (56, 7, N'Black (Back)', 0, 5830, 36, 34, 68.1, 123, 13)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (57, 7, N'Blue (Middle)', 1, 5441, 36, 34, 65.9, 115, 13)
GO
SET IDENTITY_INSERT [dbo].[CourseTeeLocations] OFF
GO
INSERT [dbo].[Handicap] ([RoundId], [HandicapBeforeRound], [HandicapAfterRound]) VALUES (14, 16.2, 16.2)
GO
INSERT [dbo].[Handicap] ([RoundId], [HandicapBeforeRound], [HandicapAfterRound]) VALUES (16, 14.7, 15.5)
GO
INSERT [dbo].[Handicap] ([RoundId], [HandicapBeforeRound], [HandicapAfterRound]) VALUES (17, 16.2, 14.7)
GO
INSERT [dbo].[Handicap] ([RoundId], [HandicapBeforeRound], [HandicapAfterRound]) VALUES (19, 20.7, 20.7)
GO
INSERT [dbo].[Handicap] ([RoundId], [HandicapBeforeRound], [HandicapAfterRound]) VALUES (20, 20.7, 20.7)
GO
INSERT [dbo].[Handicap] ([RoundId], [HandicapBeforeRound], [HandicapAfterRound]) VALUES (21, 20.7, 16.2)
GO
INSERT [dbo].[Handicap] ([RoundId], [HandicapBeforeRound], [HandicapAfterRound]) VALUES (22, 16.2, 16.2)
GO
INSERT [dbo].[Handicap] ([RoundId], [HandicapBeforeRound], [HandicapAfterRound]) VALUES (23, 15.5, 14)
GO
SET IDENTITY_INSERT [dbo].[NewsFeedComment] ON 

GO
INSERT [dbo].[NewsFeedComment] ([CommentId], [NewsFeedTypeId], [AreaId], [UserIdThatCommented], [Comment], [CreatedDate]) VALUES (1, 1, 1, 21, N'test 123', CAST(N'2016-01-17 12:09:18.750' AS DateTime))
GO
INSERT [dbo].[NewsFeedComment] ([CommentId], [NewsFeedTypeId], [AreaId], [UserIdThatCommented], [Comment], [CreatedDate]) VALUES (2, 1, 1, 21, N'test 123 456', CAST(N'2016-01-17 12:09:25.517' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[NewsFeedComment] OFF
GO
INSERT [dbo].[NewsFeedLike] ([NewsFeedTypeId], [AreaId], [UserIdThatLikedItem], [CreatedDate]) VALUES (0, 23, 21, CAST(N'2016-01-09 11:24:31.763' AS DateTime))
GO
INSERT [dbo].[Ref_NewsFeedTypeId] ([NewsFeedTypeId], [Description]) VALUES (0, N'NewRound')
GO
INSERT [dbo].[Ref_NewsFeedTypeId] ([NewsFeedTypeId], [Description]) VALUES (1, N'NewCourse')
GO
SET IDENTITY_INSERT [dbo].[Ref_Season] ON 

GO
INSERT [dbo].[Ref_Season] ([SeasonId], [SeasonText], [CreatedDate]) VALUES (29, N'2015', CAST(N'2015-09-12 00:14:13.807' AS DateTime))
GO
INSERT [dbo].[Ref_Season] ([SeasonId], [SeasonText], [CreatedDate]) VALUES (30, N'bla', CAST(N'2015-10-19 19:59:23.553' AS DateTime))
GO
INSERT [dbo].[Ref_Season] ([SeasonId], [SeasonText], [CreatedDate]) VALUES (31, N'bla2', CAST(N'2015-10-19 20:00:15.617' AS DateTime))
GO
INSERT [dbo].[Ref_Season] ([SeasonId], [SeasonText], [CreatedDate]) VALUES (32, N'sdfdsfds', CAST(N'2015-10-19 20:23:31.930' AS DateTime))
GO
INSERT [dbo].[Ref_Season] ([SeasonId], [SeasonText], [CreatedDate]) VALUES (33, N'dsfdsfds', CAST(N'2015-10-19 20:35:35.390' AS DateTime))
GO
INSERT [dbo].[Ref_Season] ([SeasonId], [SeasonText], [CreatedDate]) VALUES (34, N'sdfdsfs', CAST(N'2015-10-19 20:53:33.157' AS DateTime))
GO
INSERT [dbo].[Ref_Season] ([SeasonId], [SeasonText], [CreatedDate]) VALUES (35, N'test 123', CAST(N'2015-11-01 15:00:08.667' AS DateTime))
GO
INSERT [dbo].[Ref_Season] ([SeasonId], [SeasonText], [CreatedDate]) VALUES (36, N'2016', CAST(N'2015-11-08 00:44:21.603' AS DateTime))
GO
INSERT [dbo].[Ref_Season] ([SeasonId], [SeasonText], [CreatedDate]) VALUES (37, N'2014', CAST(N'2015-12-06 18:15:42.607' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Ref_Season] OFF
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (1, N'Alaska')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (2, N'Alabama')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (3, N'Arkansas')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (4, N'Arizona')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (5, N'California')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (6, N'Colorado')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (7, N'Connecticut')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (8, N'District Of Columbia')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (9, N'Delaware')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (10, N'Florida')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (11, N'Georgia')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (12, N'Hawaii')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (13, N'Iowa')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (14, N'Idaho')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (15, N'Illinois')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (16, N'Indiana')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (17, N'Kansas')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (18, N'Kentucky')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (19, N'Louisiana')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (20, N'Massachusetts')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (21, N'Maryland')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (22, N'Maine')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (23, N'Michigan')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (24, N'Minnesota')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (25, N'Missouri')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (26, N'Mississippi')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (27, N'Montana')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (28, N'North Carolina')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (29, N'North Dakota')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (30, N'Nebraska')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (31, N'New Hampshire')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (32, N'New Jersey')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (33, N'New Mexico')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (34, N'Nevada')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (35, N'New York')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (36, N'Ohio')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (37, N'Oklahoma')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (38, N'Oregon')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (39, N'Pennsylvania')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (40, N'Puerto Rico')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (41, N'Rhode Island')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (42, N'South Carolina')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (43, N'South Dakota')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (44, N'Tennessee')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (45, N'Texas')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (46, N'Utah')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (47, N'Virginia')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (48, N'Vermont')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (49, N'Washington')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (50, N'Wisconsin')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (51, N'West Virginia')
GO
INSERT [dbo].[Ref_State] ([StateId], [Description]) VALUES (52, N'Wyoming')
GO
SET IDENTITY_INSERT [dbo].[Round] ON 

GO
INSERT [dbo].[Round] ([RoundId], [CourseId], [CourseTeeLocationId], [UserId], [SeasonId], [RoundDate], [Score], [Is9HoleScore], [RoundHandicap], [GreensInRegulation], [FairwaysHit], [Putts]) VALUES (14, 2, 43, 21, 29, CAST(N'2015-04-25 00:00:00.000' AS DateTime), 94, 0, 19.3, NULL, NULL, NULL)
GO
INSERT [dbo].[Round] ([RoundId], [CourseId], [CourseTeeLocationId], [UserId], [SeasonId], [RoundDate], [Score], [Is9HoleScore], [RoundHandicap], [GreensInRegulation], [FairwaysHit], [Putts]) VALUES (16, 2, 43, 21, 29, CAST(N'2015-10-24 00:00:00.000' AS DateTime), 92, 0, 17.7, NULL, NULL, NULL)
GO
INSERT [dbo].[Round] ([RoundId], [CourseId], [CourseTeeLocationId], [UserId], [SeasonId], [RoundDate], [Score], [Is9HoleScore], [RoundHandicap], [GreensInRegulation], [FairwaysHit], [Putts]) VALUES (17, 1, 40, 21, 29, CAST(N'2015-07-11 00:00:00.000' AS DateTime), 90, 0, 14.7, NULL, NULL, NULL)
GO
INSERT [dbo].[Round] ([RoundId], [CourseId], [CourseTeeLocationId], [UserId], [SeasonId], [RoundDate], [Score], [Is9HoleScore], [RoundHandicap], [GreensInRegulation], [FairwaysHit], [Putts]) VALUES (19, 3, 47, 21, 37, CAST(N'2014-04-18 00:00:00.000' AS DateTime), 96, 0, 20.7, NULL, NULL, NULL)
GO
INSERT [dbo].[Round] ([RoundId], [CourseId], [CourseTeeLocationId], [UserId], [SeasonId], [RoundDate], [Score], [Is9HoleScore], [RoundHandicap], [GreensInRegulation], [FairwaysHit], [Putts]) VALUES (20, 6, 54, 21, 37, CAST(N'2014-04-26 00:00:00.000' AS DateTime), 94, 0, 24.2, NULL, NULL, NULL)
GO
INSERT [dbo].[Round] ([RoundId], [CourseId], [CourseTeeLocationId], [UserId], [SeasonId], [RoundDate], [Score], [Is9HoleScore], [RoundHandicap], [GreensInRegulation], [FairwaysHit], [Putts]) VALUES (21, 4, 50, 21, 37, CAST(N'2014-08-30 00:00:00.000' AS DateTime), 89, 0, 16.2, NULL, NULL, NULL)
GO
INSERT [dbo].[Round] ([RoundId], [CourseId], [CourseTeeLocationId], [UserId], [SeasonId], [RoundDate], [Score], [Is9HoleScore], [RoundHandicap], [GreensInRegulation], [FairwaysHit], [Putts]) VALUES (22, 5, 53, 21, 37, CAST(N'2014-10-25 00:00:00.000' AS DateTime), 90, 0, 20.2, NULL, NULL, NULL)
GO
INSERT [dbo].[Round] ([RoundId], [CourseId], [CourseTeeLocationId], [UserId], [SeasonId], [RoundDate], [Score], [Is9HoleScore], [RoundHandicap], [GreensInRegulation], [FairwaysHit], [Putts]) VALUES (23, 7, 57, 21, 29, CAST(N'2015-12-12 00:00:00.000' AS DateTime), 80, 0, 13.3, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Round] OFF
GO
SET IDENTITY_INSERT [dbo].[UserAccounts] ON 

GO
INSERT [dbo].[UserAccounts] ([UserId], [EmailAddress], [FirstName], [LastName], [Password], [CurrentSeasonId], [CreatedDate], [StateId]) VALUES (20, N'dfgfd000gfd@gmail.com', N'dfdfd', N'dibiancoj@gmail.com', N'0', 29, CAST(N'2015-09-12 00:14:15.507' AS DateTime), 2)
GO
INSERT [dbo].[UserAccounts] ([UserId], [EmailAddress], [FirstName], [LastName], [Password], [CurrentSeasonId], [CreatedDate], [StateId]) VALUES (21, N'dibiancoj@gmail.com', N'Jason', N'DiBianco', N'0', 29, CAST(N'2015-09-12 09:10:23.370' AS DateTime), 35)
GO
SET IDENTITY_INSERT [dbo].[UserAccounts] OFF
GO
INSERT [dbo].[UserSeason] ([UserId], [SeasonId], [CreatedDate]) VALUES (20, 29, CAST(N'2015-09-12 00:14:16.407' AS DateTime))
GO
INSERT [dbo].[UserSeason] ([UserId], [SeasonId], [CreatedDate]) VALUES (21, 29, CAST(N'2015-09-12 09:10:23.653' AS DateTime))
GO
INSERT [dbo].[UserSeason] ([UserId], [SeasonId], [CreatedDate]) VALUES (21, 37, CAST(N'2015-12-06 18:43:19.977' AS DateTime))
GO
/****** Object:  Index [UQ_TeeLocationPerCourse]    Script Date: 1/17/2016 12:10:54 PM ******/
ALTER TABLE [dbo].[CourseTeeLocations] ADD  CONSTRAINT [UQ_TeeLocationPerCourse] UNIQUE NONCLUSTERED 
(
	[CourseId] ASC,
	[TeeLocationSortOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_SeasonText]    Script Date: 1/17/2016 12:10:54 PM ******/
ALTER TABLE [dbo].[Ref_Season] ADD  CONSTRAINT [UQ_SeasonText] UNIQUE NONCLUSTERED 
(
	[SeasonText] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Email]    Script Date: 1/17/2016 12:10:54 PM ******/
ALTER TABLE [dbo].[UserAccounts] ADD  CONSTRAINT [UQ_Email] UNIQUE NONCLUSTERED 
(
	[EmailAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Ref_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[Ref_State] ([StateId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Ref_State]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_UserAccounts] FOREIGN KEY([UserIdThatCreatedCourse])
REFERENCES [dbo].[UserAccounts] ([UserId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_UserAccounts]
GO
ALTER TABLE [dbo].[CourseTeeLocations]  WITH CHECK ADD  CONSTRAINT [FK_CourseTeeLocations_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CourseTeeLocations] CHECK CONSTRAINT [FK_CourseTeeLocations_Course]
GO
ALTER TABLE [dbo].[Handicap]  WITH CHECK ADD  CONSTRAINT [FK_RoundHandicap_Round] FOREIGN KEY([RoundId])
REFERENCES [dbo].[Round] ([RoundId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Handicap] CHECK CONSTRAINT [FK_RoundHandicap_Round]
GO
ALTER TABLE [dbo].[NewsFeedComment]  WITH CHECK ADD  CONSTRAINT [FK_NewsFeedComment_UserAccounts] FOREIGN KEY([UserIdThatCommented])
REFERENCES [dbo].[UserAccounts] ([UserId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsFeedComment] CHECK CONSTRAINT [FK_NewsFeedComment_UserAccounts]
GO
ALTER TABLE [dbo].[NewsFeedLike]  WITH CHECK ADD  CONSTRAINT [FK_NewsFeedLike_Ref_NewsFeedTypeId] FOREIGN KEY([NewsFeedTypeId])
REFERENCES [dbo].[Ref_NewsFeedTypeId] ([NewsFeedTypeId])
GO
ALTER TABLE [dbo].[NewsFeedLike] CHECK CONSTRAINT [FK_NewsFeedLike_Ref_NewsFeedTypeId]
GO
ALTER TABLE [dbo].[NewsFeedLike]  WITH CHECK ADD  CONSTRAINT [FK_NewsFeedLike_UserAccounts] FOREIGN KEY([UserIdThatLikedItem])
REFERENCES [dbo].[UserAccounts] ([UserId])
GO
ALTER TABLE [dbo].[NewsFeedLike] CHECK CONSTRAINT [FK_NewsFeedLike_UserAccounts]
GO
ALTER TABLE [dbo].[Round]  WITH CHECK ADD  CONSTRAINT [FK_Round_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Round] CHECK CONSTRAINT [FK_Round_Course]
GO
ALTER TABLE [dbo].[Round]  WITH CHECK ADD  CONSTRAINT [FK_Round_CourseTeeLocations] FOREIGN KEY([CourseTeeLocationId])
REFERENCES [dbo].[CourseTeeLocations] ([CourseTeeLocationId])
GO
ALTER TABLE [dbo].[Round] CHECK CONSTRAINT [FK_Round_CourseTeeLocations]
GO
ALTER TABLE [dbo].[Round]  WITH CHECK ADD  CONSTRAINT [FK_Round_Ref_Season] FOREIGN KEY([SeasonId])
REFERENCES [dbo].[Ref_Season] ([SeasonId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Round] CHECK CONSTRAINT [FK_Round_Ref_Season]
GO
ALTER TABLE [dbo].[Round]  WITH CHECK ADD  CONSTRAINT [FK_Round_UserAccounts] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserAccounts] ([UserId])
GO
ALTER TABLE [dbo].[Round] CHECK CONSTRAINT [FK_Round_UserAccounts]
GO
ALTER TABLE [dbo].[UserAccounts]  WITH CHECK ADD  CONSTRAINT [FK_User_Ref_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[Ref_State] ([StateId])
GO
ALTER TABLE [dbo].[UserAccounts] CHECK CONSTRAINT [FK_User_Ref_State]
GO
ALTER TABLE [dbo].[UserAccounts]  WITH CHECK ADD  CONSTRAINT [FK_Users_Seasons] FOREIGN KEY([CurrentSeasonId])
REFERENCES [dbo].[Ref_Season] ([SeasonId])
GO
ALTER TABLE [dbo].[UserAccounts] CHECK CONSTRAINT [FK_Users_Seasons]
GO
ALTER TABLE [dbo].[UserSeason]  WITH CHECK ADD  CONSTRAINT [FK_UserSeason_Ref_Season] FOREIGN KEY([SeasonId])
REFERENCES [dbo].[Ref_Season] ([SeasonId])
GO
ALTER TABLE [dbo].[UserSeason] CHECK CONSTRAINT [FK_UserSeason_Ref_Season]
GO
ALTER TABLE [dbo].[UserSeason]  WITH CHECK ADD  CONSTRAINT [FK_UserSeason_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserAccounts] ([UserId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserSeason] CHECK CONSTRAINT [FK_UserSeason_Users]
GO
USE [master]
GO
ALTER DATABASE [ToracGolf] SET  READ_WRITE 
GO
