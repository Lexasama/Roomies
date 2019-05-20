create table rm.tItem
( 
	ItemId int identity(0,1),
	ItemPrice int not null,
	ItemName nvarchar(32) not null,
	CourseId int not null,
	RoomieId int,

	constraint PK_rm_tItem primary key (ItemId),
	constraint FK_rm_tCourse_CourseId foreign key (CourseId) references rm.tCourse(CourseId),
	constraint FK_rm_tCourse_RoomieId foreign key (RoomieId) references rm.tRoomie(RoomieId)

);

insert into rm.tItem( ItemPrice, ItemName, CourseId, RoomieId) values( 0, left(convert(nvarchar(36), newid()), 32), 0, 0);