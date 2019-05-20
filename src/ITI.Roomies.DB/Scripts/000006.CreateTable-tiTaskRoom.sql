create table rm.tiTaskRoom
(
	TaskId int not null,
	RoomieId int not null,

	constraint PK_rm_tiTaskRoom primary key (TaskId, RoomieId),
	constraint FK_rm_tiTaskRoom_RoomieId foreign key(RoomieId) references rm.tRoomie(RoomieId) ,
	constraint FK_rm_tiTaskRoom_TaskId foreign key(TaskId) references rm.tTasks(TaskId)
); 

insert into rm.tiTaskRoom(TaskId, RoomieId) values(0,0);