

CREATE DATABASE InzNetwork
--------------------------------------------
USE InzNetwork
CREATE TABLE [dbo].[inz_Post](
	[PostID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Title] [varchar](50) NOT NULL,
	[SubDescription] [varchar](50) NOT NULL,
	[Category] [varchar](50) NOT NULL,
	[IsPublic] BIT DEFAULT 0,
	[IsApproved] BIT DEFAULT 0,
	[Description] [nvarchar](max) NULL,
	[ReferenceURL] [varchar](50),
	[IsDeleted] [int] default 0,
	[createdate] [datetime] NULL,
	[modifieddate] [datetime] NULL
 )

 ALTER TABLE inz_Post
 ADD [FileName] [varchar](50) 