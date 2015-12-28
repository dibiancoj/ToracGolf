USE [master]
GO
/****** Object:  Database [ToracGolf]    Script Date: 12/27/2015 8:49:11 PM ******/
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
/****** Object:  User [ToracGolfV2User]    Script Date: 12/27/2015 8:49:12 PM ******/
CREATE USER [ToracGolfV2User] FOR LOGIN [ToracGolfV2User] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [ToracGolfV2]    Script Date: 12/27/2015 8:49:12 PM ******/
CREATE USER [ToracGolfV2] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [sql_dependency_role]    Script Date: 12/27/2015 8:49:12 PM ******/
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
/****** Object:  Schema [sql_dependency_role]    Script Date: 12/27/2015 8:49:12 PM ******/
CREATE SCHEMA [sql_dependency_role]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 12/27/2015 8:49:12 PM ******/
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
/****** Object:  Table [dbo].[CourseTeeLocations]    Script Date: 12/27/2015 8:49:12 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_TeeLocationPerCourse] UNIQUE NONCLUSTERED 
(
	[CourseId] ASC,
	[TeeLocationSortOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Handicap]    Script Date: 12/27/2015 8:49:12 PM ******/
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
/****** Object:  Table [dbo].[NewsFeedComment]    Script Date: 12/27/2015 8:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NewsFeedComment](
	[NewsFeedTypeId] [int] NOT NULL,
	[AreaId] [int] NOT NULL,
	[UserIdThatCommented] [int] NOT NULL,
	[Comment] [varchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_NewsFeedComment] PRIMARY KEY CLUSTERED 
(
	[NewsFeedTypeId] ASC,
	[AreaId] ASC,
	[UserIdThatCommented] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NewsFeedLike]    Script Date: 12/27/2015 8:49:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsFeedLike](
	[NewsFeedTypeId] [int] IDENTITY(1,1) NOT NULL,
	[AreaId] [int] NOT NULL,
	[UserIdThatLikedItem] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_NewsFeedLike] PRIMARY KEY CLUSTERED 
(
	[NewsFeedTypeId] ASC,
	[AreaId] ASC,
	[UserIdThatLikedItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ref_NewsFeedTypeId]    Script Date: 12/27/2015 8:49:12 PM ******/
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
/****** Object:  Table [dbo].[Ref_Season]    Script Date: 12/27/2015 8:49:12 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_SeasonText] UNIQUE NONCLUSTERED 
(
	[SeasonText] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ref_State]    Script Date: 12/27/2015 8:49:12 PM ******/
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
/****** Object:  Table [dbo].[Round]    Script Date: 12/27/2015 8:49:12 PM ******/
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
/****** Object:  Table [dbo].[UserAccounts]    Script Date: 12/27/2015 8:49:12 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Email] UNIQUE NONCLUSTERED 
(
	[EmailAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserSeason]    Script Date: 12/27/2015 8:49:12 PM ******/
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
ALTER TABLE [dbo].[NewsFeedLike] ADD  CONSTRAINT [DF_NewsFeedLike_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
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
