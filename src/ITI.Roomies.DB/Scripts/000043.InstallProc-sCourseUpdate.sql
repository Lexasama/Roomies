create proc rm.sCourseUpdate
(
	@CourseId int, 
	@CourseName nvarchar(32),
	@CourseDate datetime2
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tCourse c where c.CourseId = @CourseId)
	begin
		rollback;
		return 1;
	end;

	update rm.tCourse
	set CourseName = @CourseName,
		CourseDate = @CourseDate
	where CourseId = @CourseId;

	commit;
    return 0;


end;
