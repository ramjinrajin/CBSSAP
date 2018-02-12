

CREATE TABLE PostShared
(
	ShareId int Primary key identity(1,1),
	PostId int,
	UserId int,
	IsShared int,
	IsDeclined int,
	CreateDate DateTime Default GETDATE(),
	IsDeleted int Default 0
)
	