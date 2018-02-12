USE [master]
GO
/****** Object:  Database [InzNetwork]    Script Date: 10/02/2018 12:25:10 AM ******/
CREATE DATABASE [InzNetwork]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'InzNetwork', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\InzNetwork.mdf' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'InzNetwork_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\InzNetwork_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [InzNetwork] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [InzNetwork].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [InzNetwork] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [InzNetwork] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [InzNetwork] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [InzNetwork] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [InzNetwork] SET ARITHABORT OFF 
GO
ALTER DATABASE [InzNetwork] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [InzNetwork] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [InzNetwork] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [InzNetwork] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [InzNetwork] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [InzNetwork] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [InzNetwork] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [InzNetwork] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [InzNetwork] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [InzNetwork] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [InzNetwork] SET  ENABLE_BROKER 
GO
ALTER DATABASE [InzNetwork] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [InzNetwork] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [InzNetwork] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [InzNetwork] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [InzNetwork] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [InzNetwork] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [InzNetwork] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [InzNetwork] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [InzNetwork] SET  MULTI_USER 
GO
ALTER DATABASE [InzNetwork] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [InzNetwork] SET DB_CHAINING OFF 
GO
ALTER DATABASE [InzNetwork] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [InzNetwork] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [InzNetwork]
GO
/****** Object:  StoredProcedure [dbo].[spAuthenticateUser]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 

CREATE Procedure [dbo].[spAuthenticateUser]
@UserName nvarchar(100),
@Password nvarchar(100)
as
Begin
 Declare @Count int
 
 Select @Count = COUNT(UserName) from inz_USERS
 where [UserName] = @UserName and [Password] = @Password AND Rowstatus='A'
 
 if(@Count = 1)
 Begin
  Select 1 as ReturnCode
 End
 Else
 Begin
  Select -1 as ReturnCode
 End
End 

GO
/****** Object:  StoredProcedure [dbo].[spInsertPostDetails]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 

 
CREATE procedure [dbo].[spInsertPostDetails]
 
(

@UserId int,
@Title varchar(50),
@SubDescription varchar(100),
@Category varchar(100),
@IsPublic int,
@Description nvarchar(max),
@ReferenceURL varchar(50),
@FileName varchar(50)
)

AS BEGIN

 DECLARE @Count INT
 DECLARE @RETURNCODE INT 
  DECLARE @PostId int
 
 SELECT @Count=COUNT(*) FROM [inz_post] WHERE Title=@Title 

IF(@Count=0)
BEGIN
INSERT INTO [inz_post]
           ( 
				[UserID],
				[Title],
				[SubDescription],
				[Description],
				[ReferenceURL],
				[IsPublic],
				[Category],
				[FileName]
		   )
     SELECT 
		@UserId,@Title,@SubDescription,@Description,@ReferenceURL,@IsPublic,@Category,@FileName 

 
SET @PostId = SCOPE_IDENTITY()
IF(@PostId=null)
SET @PostId=0

INSERT INTO [dbo].[PostShared]
           ([PostId]
           ,[UserId]
           ,[IsShared]
           ,[IsDeclined])

		   SELECT @PostId,@UserId,0,0



	SET @RETURNCODE=@PostId
END
ELSE
BEGIN
SET @RETURNCODE=-1
END
 SELECT @PostId;
END


GO
/****** Object:  StoredProcedure [dbo].[spRegisterUser]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[spRegisterUser]

 @UserName NVARCHAR(100)

,@Password NVARCHAR(200)

,@Email NVARCHAR(200)

,@Category varchar(50)


AS

BEGIN

DECLARE @Count INT

DECLARE @ReturnCode INT



SELECT @Count = COUNT(UserName)

FROM inz_USERS

WHERE UserName = @UserName and EmailID=@Email



IF @Count > 0

BEGIN

SET @ReturnCode = - 1

END

ELSE

BEGIN

SET @ReturnCode = 1



INSERT INTO inz_USERS (Username,Password,EmailID,Rowstatus,createdate,Category)

VALUES (

@UserName

,@Password

,@Email

,'A'

,GetDATE()

,@Category

)

END



SELECT @ReturnCode AS ReturnValue

END
GO
/****** Object:  StoredProcedure [dbo].[spSharePost]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[spSharePost]

 

(



@UserId int,

@PostId int,

@isShared int,

@isDeclined int

)



AS BEGIN


 DECLARE @RETURNCODE INT,@Count INT

SET @PostId = SCOPE_IDENTITY()

SELECT @Count=COUNT(*) FROM [PostShared] WHERE  PostId=@PostId and UserId=@UserId

IF(@Count=0)
BEGIN
INSERT INTO [dbo].[PostShared]

           ([PostId]

           ,[UserId]

           ,[IsShared]

           ,[IsDeclined])



		   SELECT @PostId,@UserId,0,0
	SET @RETURNCODE=1

END

ELSE

BEGIN

SET @RETURNCODE=-1

END

 SELECT @RETURNCODE;

END



GO
/****** Object:  StoredProcedure [dbo].[spSuggestFriend]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spSuggestFriend]
(
@UserId int,
@PostId int 

)

AS BEGIN


DECLARE  @Category varchar(50),@PostCategory varchar(50),@RETURNCODE INT,@OwnPost int 


SET @Category=(Select top  1 Category From inz_USERS Where UserID=@UserId)
SET @PostCategory=(select top 1 Category from inz_Post Where PostID=@PostId)
SET @OwnPost=(select count(*) from inz_Post Where PostID=@PostId and UserID=@UserId)

if(@OwnPost=1)
BEGIN
SET @RETURNCODE=-2
 SELECT @RETURNCODE;
RETURN;
END

IF(@Category=@PostCategory)
BEGIN
INSERT INTO [dbo].[FriendSuggest]

           ([PostId]

           ,[UserId]

           ,IsAccepted )

		   SELECT @PostId,@UserId,0 
	SET @RETURNCODE=1

END

ELSE

BEGIN

SET @RETURNCODE=-1

END

 SELECT @RETURNCODE;

END
GO
/****** Object:  StoredProcedure [dbo].[spUpdatePostDetails]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 

CREATE procedure [dbo].[spUpdatePostDetails]
 
(
@PostId int,
@Title varchar(50),
@SubDescription varchar(100),
@Category varchar(100),
@IsPublic int,
@Description nvarchar(max),
@ReferenceURL varchar(50),
@FileName varchar(50)
)

AS BEGIN

 DECLARE @Count INT
 DECLARE @RETURNCODE INT 
 
 SELECT @Count=COUNT(*) FROM [inz_post] WHERE Title=@Title AND PostID!=@PostId
 


IF(@Count=0)
BEGIN

UPDATE inz_post
SET [Title]=@Title,[SubDescription]=@SubDescription,[Description]=@Description,[ReferenceURL]=@ReferenceURL,[IsPublic]=@IsPublic,[Category]=@Category,[FileName]=@FileName
WHERE PostID=@PostId

SET @RETURNCODE=1
END
ELSE
BEGIN
SET @RETURNCODE=-1
END
 SELECT @RETURNCODE;
END



GO
/****** Object:  StoredProcedure [dbo].[SpUserEnableDisable]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SpUserEnableDisable]
(
@Email Varchar(50)
)
As 
BEGIN 
UPDATE    inz_USERS
SET              
Rowstatus = CASE Rowstatus WHEN 'A' THEN 'D' WHEN 'D' THEN 'A' 
ELSE Rowstatus 
END
Where EmailID=@Email
END

GO
/****** Object:  Table [dbo].[FriendSuggest]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FriendSuggest](
	[POstId] [int] NULL,
	[UserId] [int] NULL,
	[IsAccepted] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[INZ_CATEGORY]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[INZ_CATEGORY](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CATEGORY] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[inz_Post]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[inz_Post](
	[PostID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Title] [varchar](50) NOT NULL,
	[SubDescription] [varchar](50) NOT NULL,
	[Category] [varchar](50) NOT NULL,
	[IsPublic] [bit] NULL,
	[IsApproved] [bit] NULL,
	[Description] [nvarchar](max) NULL,
	[ReferenceURL] [varchar](50) NULL,
	[IsDeleted] [int] NULL,
	[createdate] [datetime] NULL,
	[modifieddate] [datetime] NULL,
	[FileName] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[inz_USERS]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[inz_USERS](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](100) NULL,
	[EmailID] [varchar](50) NULL,
	[mobilenno] [varchar](100) NULL,
	[Rowstatus] [char](1) NULL,
	[createdate] [datetime] NULL,
	[modifieddate] [datetime] NULL,
	[Password] [varchar](100) NULL,
	[isadmin] [varchar](50) NULL,
	[Category] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PostShared]    Script Date: 10/02/2018 12:25:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostShared](
	[ShareId] [int] IDENTITY(1,1) NOT NULL,
	[PostId] [int] NULL,
	[UserId] [int] NULL,
	[IsShared] [int] NULL,
	[IsDeclined] [int] NULL,
	[CreateDate] [datetime] NULL,
	[IsDeleted] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ShareId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[inz_Post] ADD  DEFAULT ((0)) FOR [IsPublic]
GO
ALTER TABLE [dbo].[inz_Post] ADD  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[inz_Post] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[PostShared] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[PostShared] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
USE [master]
GO
ALTER DATABASE [InzNetwork] SET  READ_WRITE 
GO
