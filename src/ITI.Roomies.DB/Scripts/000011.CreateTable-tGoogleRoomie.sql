create table rm.tGoogleRoomie
(
    RoomieId       int,
    GoogleId     varchar(32) collate Latin1_General_BIN2 not null,
    RefreshToken varchar(64) collate Latin1_General_BIN2 not null,

    constraint PK_tGoogleRoomie primary key(RoomieId),
    constraint FK_tGoogleRoomie_RoomieId foreign key(RoomieId) references rm.tRoomie(RoomieId),
    constraint UK_tGoogleRoomie_GoogleId unique(GoogleId)
);

insert into rm.tGoogleRoomie(RoomieId, GoogleId, RefreshToken) values(0, 0, '');