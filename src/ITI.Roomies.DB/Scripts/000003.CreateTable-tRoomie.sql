create table rm.tRoomie
(
	RoomieId int identity(0, 1),
	FirstName  nvarchar(32) collate Latin1_General_100_CI_AS,
	LastName   nvarchar(32) collate Latin1_General_100_CI_AS,
    Email  nvarchar(64) collate Latin1_General_CI_AI not null,
	BirthDate Datetime2,
	Phone	  char(10),
	[Description] nvarchar(200),
	RoomiePic nvarchar(max),

    constraint PK_tRoomie primary key(RoomieId),
    constraint UK_tRoomie_Email unique(Email),
    --constraint UK_tUser_Phone unique(Phone),
);

insert into rm.tRoomie(FirstName,                                LastName,                                 Email, BirthDate, Phone )
              values(left(convert(nvarchar(36), newid()), 32), left(convert(nvarchar(36), newid()), 32), N'',   '20190101','0000000000');