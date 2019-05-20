create table rm.tiCollRoom
(
	CollocId int not null,
	RoomieId int not null,

	constraint PK_rm_tiCollRoom primary key(CollocId,RoomieId),
	constraint FK_rm_tiCollRoom_CollocId foreign key (CollocId) references rm.tColloc(CollocId),
	constraint FK_rm_tiCollRoom_RoomieId foreign key (RoomieId) references rm.tRoomie(RoomieId)
);

insert into rm.tiCollRoom(CollocId, RoomieId) values(0,0);