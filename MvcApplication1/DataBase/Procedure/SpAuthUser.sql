 

Create Procedure [dbo].[spAuthenticateUser]
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


