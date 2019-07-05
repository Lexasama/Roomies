create table rm.tCategory
(
	CategoryId int identity(0,1),
	CategoryName nvarchar(32) not null,
	IconName nvarchar(32) not null,
	CollocId int not null,

	constraint PK_rm_tCategory primary key(CategoryId),
	constraint FK_rm_tCategory_IconName foreign key (IconName) references rm.tIcon(IconName), 
	constraint FK_rm_tCategory_CollocId foreign key (CollocId) references rm.tColloc(CollocId),
);

insert into rm.tCategory( CategoryName, IconName, CollocId) 
				 values ( N'', 'Icon0', 0);
