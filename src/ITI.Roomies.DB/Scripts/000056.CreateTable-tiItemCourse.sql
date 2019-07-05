create table rm.tiItemCourse
(
	ItemId int not null,
	CourseId int not null,
	RoomieId int default 0 not null,
	ItemQuantite nvarchar(32),
	ItemBought bit default 0,

	constraint PK_rm_tiItemCourse primary key (ItemId, CourseId),
	constraint FK_rm_tiItemCourse_ItemId foreign key (ItemId) references rm.tItem(ItemId),
	constraint FK_rm_tiItemCourse_CourseId foreign key (CourseId) references rm.tCourse(CourseId),
	constraint FK_rm_tiItemCourse_RoomieId foreign key (RoomieId) references rm.tRoomie(RoomieId)
);
insert into rm.tiItemCourse( ItemId, CourseId, RoomieId, ItemQuantite, ItemBought)
	values (0, 0, 0, left(convert(nvarchar(36), newid()), 32), 0);
