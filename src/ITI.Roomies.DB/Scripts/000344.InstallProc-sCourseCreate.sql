create procedure rm.sCourseCreate
(
	@CourseId int out,
	@CourseName nvarchar(32),
	@CourseDate datetime2,
	@CollocId int
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	insert into rm.tCourse( CourseName, CourseDate, CollocId )
					values(@CourseName, @CourseDate, @CollocId);
	set @CourseId = SCOPE_IDENTITY();

	commit;
	return 0;
end;

