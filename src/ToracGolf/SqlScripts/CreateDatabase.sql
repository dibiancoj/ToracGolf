USE [ToracGolf]
GO
/****** Object:  User [ToracGolfV2]    Script Date: 12/1/2015 8:10:29 PM ******/
CREATE USER [ToracGolfV2] FOR LOGIN [ToracGolfV2] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [ToracGolfV2]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [ToracGolfV2]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 12/1/2015 8:10:29 PM ******/
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
/****** Object:  Table [dbo].[CourseTeeLocations]    Script Date: 12/1/2015 8:10:29 PM ******/
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
/****** Object:  Table [dbo].[Ref_Season]    Script Date: 12/1/2015 8:10:29 PM ******/
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
/****** Object:  Table [dbo].[Ref_State]    Script Date: 12/1/2015 8:10:29 PM ******/
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
/****** Object:  Table [dbo].[Round]    Script Date: 12/1/2015 8:10:29 PM ******/
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
/****** Object:  Table [dbo].[RoundHandicap]    Script Date: 12/1/2015 8:10:29 PM ******/
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
/****** Object:  Table [dbo].[UserAccounts]    Script Date: 12/1/2015 8:10:29 PM ******/
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
/****** Object:  Table [dbo].[UserSeason]    Script Date: 12/1/2015 8:10:29 PM ******/
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
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive]) VALUES (15, N'Saxon Woods Golf Course', N'White Plains', 35, N'County Course. Slow on the weekends', 0, 21, 1, 1)
GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive]) VALUES (16, N'West Point Golf Course', N'West Point', 35, N'Short course but you need to be accurate', 0, 21, 1, 1)
GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive]) VALUES (17, N'Hudson Hills', N'Ossining', 35, N'Hard course with many blind shots', 0, 21, 1, 1)
GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive]) VALUES (18, N'Richter Park', N'Danbury', 35, N'Great Course Owned By Town', 0, 21, 1, 1)
GO
INSERT [dbo].[Course] ([CourseId], [Name], [City], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive]) VALUES (19, N'River Vale Country Club', N'Jersey', 35, N'Over priced course', 0, 21, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Course] OFF
GO
SET IDENTITY_INSERT [dbo].[CourseTeeLocations] ON 

GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (14, 15, N'Middle', 0, 6400, 36, 36, 70, 115, 3)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (15, 15, N'Back', 1, 6700, 36, 36, 72, 118, 3)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (16, 16, N'Back', 0, 6900, 36, 36, 72, 125, 3)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (17, 16, N'Middle', 1, 6250, 36, 36, 70, 120, 3)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (18, 17, N'Front', 0, 5755, 36, 35, 68, 126, 4)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (19, 17, N'Middle', 1, 6323, 36, 35, 71, 129, 4)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (20, 17, N'Back', 2, 6935, 36, 35, 73.7, 139, 4)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (21, 18, N'Middle', 0, 6304, 36, 36, 71.6, 136, 4)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (22, 18, N'Back', 1, 6744, 36, 36, 73.6, 139, 4)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (23, 18, N'Front', 2, 5531, 36, 36, 72.9, 129, 4)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (24, 19, N'Front', 0, 5892, 36, 36, 68.7, 126, 4)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (25, 19, N'Middle', 1, 6168, 36, 36, 69.7, 129, 4)
GO
INSERT [dbo].[CourseTeeLocations] ([CourseTeeLocationId], [CourseId], [Description], [TeeLocationSortOrderId], [Yardage], [Front9Par], [Back9Par], [Rating], [Slope], [FairwaysOnCourse]) VALUES (26, 19, N'Back', 2, 6504, 36, 36, 71.4, 132, 4)
GO
SET IDENTITY_INSERT [dbo].[CourseTeeLocations] OFF
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
INSERT [dbo].[Round] ([RoundId], [CourseId], [CourseTeeLocationId], [UserId], [SeasonId], [RoundDate], [Score], [Is9HoleScore], [RoundHandicap], [GreensInRegulation], [FairwaysHit], [Putts]) VALUES (1, 16, 16, 21, 29, CAST(N'2015-11-30 00:00:00.000' AS DateTime), 93, 0, 18.2, NULL, NULL, NULL)
GO
INSERT [dbo].[Round] ([RoundId], [CourseId], [CourseTeeLocationId], [UserId], [SeasonId], [RoundDate], [Score], [Is9HoleScore], [RoundHandicap], [GreensInRegulation], [FairwaysHit], [Putts]) VALUES (2, 15, 14, 21, 29, CAST(N'2015-11-04 00:00:00.000' AS DateTime), 92, 0, 20.8, NULL, 13, 29)
GO
SET IDENTITY_INSERT [dbo].[Round] OFF
GO
INSERT [dbo].[RoundHandicap] ([RoundId], [HandicapBeforeRound], [HandicapAfterRound]) VALUES (1, 20.8, 18.2)
GO
INSERT [dbo].[RoundHandicap] ([RoundId], [HandicapBeforeRound], [HandicapAfterRound]) VALUES (2, 20.8, 20.8)
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
/****** Object:  Index [UQ_TeeLocationPerCourse]    Script Date: 12/1/2015 8:10:29 PM ******/
ALTER TABLE [dbo].[CourseTeeLocations] ADD  CONSTRAINT [UQ_TeeLocationPerCourse] UNIQUE NONCLUSTERED 
(
	[CourseId] ASC,
	[TeeLocationSortOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_SeasonText]    Script Date: 12/1/2015 8:10:29 PM ******/
ALTER TABLE [dbo].[Ref_Season] ADD  CONSTRAINT [UQ_SeasonText] UNIQUE NONCLUSTERED 
(
	[SeasonText] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Email]    Script Date: 12/1/2015 8:10:29 PM ******/
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
