create table rm.tCourse
(
	CourseId int identity(0,1),
	CourseName nvarchar(32) not null,
	CourseDate datetime2,
	CoursePrice int,
	CollocId int not null,

	constraint PK_rm_tCourse primary key(CourseId),
	constraint FK_rm_tCourse_CollocId foreign key (CollocId) references rm.tColloc(CollocId)
);

insert into rm.tCourse( CourseName, CourseDate, CoursePrice, CollocId) 
			   values ( N'', '20190101', 0, 0);
