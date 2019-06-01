create proc rm.sCourseTempUpdate
(
	@CourseTempId int, 
	@CourseName nvarchar(32),
	@CollocId int

)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tCourseTemp c where c.CourseTempId = @CourseTempId)
	begin
		rollback;
		return 1;
	end;

	update rm.tCourseTemp
	set CourseName = @CourseName,
		CollocId = @CollocId
	where CourseTempId = @CourseTempId;

	commit;
    return 0;

end;
