create table rm.tGoogleRoomie
(
    RoomieId       int,
    GoogleId     varchar(32) collate Latin1_General_BIN2 not null,
    RefreshToken varchar(64) collate Latin1_General_BIN2 not null,

    constraint PK_tGoogleUser primary key(RoomieId),
    constraint FK_tGoogleUser_UserId foreign key(RoomieId) references rm.tRoomie(RoomieId),
    constraint UK_tGoogleUser_GoogleId unique(GoogleId)
);

insert into rm.tGoogleRoomie(RoomieId, GoogleId, RefreshToken) values(0, 0, '');
