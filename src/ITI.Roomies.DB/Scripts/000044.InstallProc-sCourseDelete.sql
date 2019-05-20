create proc rm.sCourseDelete
(
	@CourseId int
)
as
begin
	delete from rm.tCourse where CourseId = @CourseId;
	return 0;
	
end;