USE [InzNetwork]
GO

 

 
ALTER procedure [dbo].[spInsertPostDetails]
 
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

 