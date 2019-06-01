create table rm.tTransacDepense
(	
	TDepenseId int identity(0,1) not null,
	[Desc] nvarchar(200),
	Price int not null,
	[Date] datetime2,
	SRoomieId int not null, 
	RRoomieId int not null,

	constraint PK_rm_tTransacDepense primary key (TDepenseId),
	constraint FK_rm_tTransacDepense_SRoomieId foreign key (SRoomieId) references rm.tRoomie(RoomieId),
	constraint FK_rm_tTransacDepense_RRoomieId foreign key (RRoomieId) references rm.tRoomie(RoomieId),
);

insert into rm.tTransacDepense([Desc], Price, [Date], SRoomieId, RRoomieId )
							values(N'',0, '20000101', 0, 0);