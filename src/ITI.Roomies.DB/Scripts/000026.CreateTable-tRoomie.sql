create table rm.tRoomie
(
    StudentId int identity(0, 1),
    FirstName nvarchar(32) collate Latin1_General_CI_AI not null,
    LastName  nvarchar(32) collate Latin1_General_CI_AI not null,
    BirthDate datetime2 not null,
    Phone	  char(10),
	[Description] nvarchar(200),
	RoomiePic nvarchar(max),

    constraint PK_tRoomie primary key(RoomieId),
    constraint FK_tRoomie_tClass foreign key(ClassId) references iti.tClass(ClassId),
    constraint UK_tRoomie_FirstName_LastName unique(FirstName, LastName),
    constraint CK_tRoomie_FirstName check(FirstName <> N''),
    constraint CK_tRoomie_LastName check(LastName <> N'')
);

insert into iti.tStudent(FirstName,                                LastName,                                 BirthDate,  ClassId)
                  values(left(convert(nvarchar(36), newid()), 32), left(convert(nvarchar(36), newid()), 32), '00010101', 0);
