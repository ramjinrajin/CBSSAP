 

ALTER procedure [dbo].[spUpdatePostDetails]
 
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


