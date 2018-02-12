

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


