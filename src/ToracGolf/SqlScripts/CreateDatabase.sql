USE [ToracGolf]
GO
/****** Object:  User [ToracGolfV2]    Script Date: 11/24/2015 9:02:14 PM ******/
CREATE USER [ToracGolfV2] FOR LOGIN [ToracGolfV2] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [ToracGolfV2]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [ToracGolfV2]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 11/24/2015 9:02:14 PM ******/
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
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CourseImages]    Script Date: 11/24/2015 9:02:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CourseImages](
	[CourseId] [int] NOT NULL,
	[CourseImage] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_CourseImages] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CourseTeeLocations]    Script Date: 11/24/2015 9:02:14 PM ******/
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
/****** Object:  Table [dbo].[Ref_Season]    Script Date: 11/24/2015 9:02:14 PM ******/
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
/****** Object:  Table [dbo].[Ref_State]    Script Date: 11/24/2015 9:02:14 PM ******/
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
/****** Object:  Table [dbo].[Round]    Script Date: 11/24/2015 9:02:14 PM ******/
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
	[GreensInRegulation] [int] NULL,
	[FairwaysHit] [int] NULL,
	[Putts] [int] NULL,
 CONSTRAINT [PK_Round] PRIMARY KEY CLUSTERED 
(
	[RoundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RoundHandicap]    Script Date: 11/24/2015 9:02:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoundHandicap](
	[RoundId] [int] NOT NULL,
	[HandicapBeforeRound] [float] NOT NULL,
	[HandicapAfterRound] [float] NOT NULL,
 CONSTRAINT [PK_RoundHandicap] PRIMARY KEY CLUSTERED 
(
	[RoundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserAccounts]    Script Date: 11/24/2015 9:02:14 PM ******/
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
/****** Object:  Table [dbo].[UserSeason]    Script Date: 11/24/2015 9:02:14 PM ******/
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
ALTER TABLE [dbo].[CourseImages]  WITH CHECK ADD  CONSTRAINT [FK_CourseImages_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CourseImages] CHECK CONSTRAINT [FK_CourseImages_Course]
GO
ALTER TABLE [dbo].[CourseTeeLocations]  WITH CHECK ADD  CONSTRAINT [FK_CourseTeeLocations_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CourseTeeLocations] CHECK CONSTRAINT [FK_CourseTeeLocations_Course]
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
ALTER TABLE [dbo].[RoundHandicap]  WITH CHECK ADD  CONSTRAINT [FK_RoundHandicap_Round] FOREIGN KEY([RoundId])
REFERENCES [dbo].[Round] ([RoundId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoundHandicap] CHECK CONSTRAINT [FK_RoundHandicap_Round]
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
