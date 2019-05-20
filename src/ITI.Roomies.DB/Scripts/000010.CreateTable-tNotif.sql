create table rm.tNotifs
(
	NotifId int identity(0,1),
	RoomieId int not null,
	[Message] nvarchar(max) not null,
	Seen bit default 0,

	constraint Pk_rm_tNotif primary key(NotifId),
	constraint FK_rm_tNotif_RoomieId foreign key(RoomieId) references rm.tRoomie(RoomieId)
);
	insert into rm.tNotifs(RoomieId, [Message]) 
					values(0,      N''); 