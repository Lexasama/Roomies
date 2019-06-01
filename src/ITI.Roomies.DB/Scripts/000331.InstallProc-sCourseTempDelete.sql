create proc rm.sCourseTempDelete
(
	@CourseTempId int
)
as
begin
	delete from rm.tCourseTemp where CourseTempId = @CourseTempId;
	return 0;
	
end;