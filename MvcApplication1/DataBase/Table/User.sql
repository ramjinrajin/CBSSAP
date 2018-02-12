 
 
 CREATE TABLE [dbo].[inz_USERS](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](100) NULL,
	[EmailID] [varchar](50) NULL,
	[mobilenno] [varchar](100) NULL,
	[Rowstatus] [char](1) NULL,
	[createdate] [datetime] NULL,
	[modifieddate] [datetime] NULL,
	[Password] [varchar](100) NULL,
	[isadmin][varchar](50) NULL,
	[Category] [varchar](50) NULL )
 