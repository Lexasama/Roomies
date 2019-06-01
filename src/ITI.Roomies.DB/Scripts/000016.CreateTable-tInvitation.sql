create table rm.tInvitation
(
    InvitationKey nvarchar (64),
    IdColloc int,
    IdSender int,
    IdReceiver int,
    InvitationDate datetime2 default getdate(),
    InvitationState int,


	constraint PK_rm_tInvitation primary key( InvitationKey ),
	constraint FK_rm_tTasks_IdColloc foreign key(IdColloc) references rm.tColloc(CollocId), 
	constraint FK_rm_tTasks_IdSender foreign key(IdSender) references rm.tRoomie(RoomieId),
	constraint FK_rm_tTasks_IdReceiver foreign key(IdReceiver) references rm.tRoomie(RoomieId)

);


insert into rm.tInvitation(InvitationKey, IdColloc, IdSender, Idreceiver, InvitationState )
      values(left(convert(nvarchar(36), newid()), 32), 0, 0,  0, 0);
