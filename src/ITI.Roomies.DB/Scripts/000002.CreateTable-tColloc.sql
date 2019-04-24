create table rm.tColloc
(
	CollocId int identity(0,1),
	CollocName nvarchar(32) not null,
	CreationDate datetime2 default getdate(),
	CollocPic varchar(max),

	constraint PK_rm_tColloc primary key( CollocId ),
	constraint CK_rm_tColloc_CollocName check( CollocName <> N'') 
);
insert into rm.tColloc( CollocName ) 
values                ( left(convert(nvarchar(36), newid()), 32));	
