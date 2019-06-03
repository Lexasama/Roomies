create table rm.tCourseTemp
(
	CourseTempId int identity(0,1),
	CourseTempName nvarchar(32) not null,
	Price int,
	CollocId int not null,

	constraint PK_rm_tCourseTemp primary key(CourseTempId),
	constraint FK_rm_tCourseTemp_CollocId foreign key (CollocId) references rm.tColloc(CollocId)
);

insert into rm.tCourseTemp( CourseTempName, CollocId) 
			   values ( N'', 0);
