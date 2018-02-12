Create Procedure SpUserEnableDisable
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