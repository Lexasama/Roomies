create procedure rm.sTaskRoomDelete
(
    @TaskId int,
	@RoomieId int
)
as
begin
   
    delete from rm.tiTaskRoom where TaskId = @TaskId and RoomieId = @RoomieId;
    return 0;
end;