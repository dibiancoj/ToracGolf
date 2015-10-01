USE [ToracGolf]
GO
/****** Object:  User [ToracGolfV2]    Script Date: 9/30/2015 8:07:29 PM ******/
CREATE USER [ToracGolfV2] FOR LOGIN [ToracGolfV2] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [ToracGolfV2]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [ToracGolfV2]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 9/30/2015 8:07:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Course](
	[CourseId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](75) NOT NULL,
	[Location] [varchar](100) NOT NULL,
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
/****** Object:  Table [dbo].[CourseTeeLocations]    Script Date: 9/30/2015 8:07:29 PM ******/
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
 CONSTRAINT [PK_CourseTeeLocations_1] PRIMARY KEY CLUSTERED 
(
	[CourseTeeLocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ref_Season]    Script Date: 9/30/2015 8:07:29 PM ******/
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
/****** Object:  Table [dbo].[Ref_State]    Script Date: 9/30/2015 8:07:29 PM ******/
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
/****** Object:  Table [dbo].[UserAccounts]    Script Date: 9/30/2015 8:07:29 PM ******/
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
/****** Object:  Table [dbo].[UserSeason]    Script Date: 9/30/2015 8:07:29 PM ******/
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
INSERT [dbo].[Course] ([CourseId], [Name], [Location], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive]) VALUES (1, N'sdfds', N'dsfds', 32, N'dsfds', 1, 21, 1, 1)
GO
INSERT [dbo].[Course] ([CourseId], [Name], [Location], [StateId], [Description], [OnlyAllow18Holes], [UserIdThatCreatedCourse], [Pending], [IsActive]) VALUES (2, N'dsfds', N'fsdfds', 32, N'fdsfds', 0, 21, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Course] OFF
GO
SET IDENTITY_INSERT [dbo].[Ref_Season] ON 

GO
INSERT [dbo].[Ref_Season] ([SeasonId], [SeasonText], [CreatedDate]) VALUES (29, N'2015', CAST(N'2015-09-12 00:14:13.807' AS DateTime))
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
/****** Object:  Index [UQ_TeeLocationPerCourse]    Script Date: 9/30/2015 8:07:29 PM ******/
ALTER TABLE [dbo].[CourseTeeLocations] ADD  CONSTRAINT [UQ_TeeLocationPerCourse] UNIQUE NONCLUSTERED 
(
	[CourseId] ASC,
	[TeeLocationSortOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_SeasonText]    Script Date: 9/30/2015 8:07:29 PM ******/
ALTER TABLE [dbo].[Ref_Season] ADD  CONSTRAINT [UQ_SeasonText] UNIQUE NONCLUSTERED 
(
	[SeasonText] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Email]    Script Date: 9/30/2015 8:07:29 PM ******/
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
