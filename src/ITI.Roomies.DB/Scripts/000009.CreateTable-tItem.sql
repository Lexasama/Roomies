create table rm.tItem
( 
	ItemId int identity(0,1),
	ItemPrice int,
	ItemName nvarchar(32) not null,
    ItemSaved bit not null default 0,
	CollocId int not null,

	constraint PK_rm_tItem primary key (ItemId),
	constraint FK_rm_tItem_CollocId foreign key (CollocId) references rm.tColloc(CollocId)

);

insert into rm.tItem( ItemPrice, ItemName, CollocId) values( 0, left(convert(nvarchar(36), newid()), 32), 0);
