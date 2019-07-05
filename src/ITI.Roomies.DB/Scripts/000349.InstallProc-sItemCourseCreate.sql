create procedure rm.sItemCourseCreate
(
	@ItemId	int,
	@CourseId   int,
    @RoomieId	int,
    @ItemQuantite  nvarchar(32)
)
as
begin
    set transaction isolation level serializable;
	begin tran;

	insert into rm.tiItemCourse(ItemId, CourseId, RoomieId, ItemQuantite)
	                  values(@ItemId, @CourseId, @RoomieId, @ItemQuantite);
	
	commit;
	return 0;
end;