create procedure rm.sDeleteItemFromCourse
(
    @ItemId int,
	@CourseId int,
	@RoomieId int
)
as
begin
   
    delete from rm.tiItemCourse where ItemId = @ItemId and CourseId = @CourseId and RoomieId = @RoomieId;
    return 0;
end;