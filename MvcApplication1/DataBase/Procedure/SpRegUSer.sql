USE [InzNetwork]
GO

/****** Object:  StoredProcedure [dbo].[spRegisterUser]    Script Date: 29-Sep-17 5:34:43 PM ******/
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


