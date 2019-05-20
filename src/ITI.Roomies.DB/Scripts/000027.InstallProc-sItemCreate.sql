create procedure rm.sItemCreate
(
	@CourseId	int,
	@RoomieId	int,
    @ItemName   nvarchar(32),
    @ItemPrice	int
)
as
begin
    set transaction isolation level serializable;
	begin tran;

	insert into rm.tItem(ItemName, ItemPrice, RoomieId, CourseId)
	                  values(@ItemName, @ItemPrice, @RoomieId, @CourseId);
	
	commit;
	return 0;
end;
