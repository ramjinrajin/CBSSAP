
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