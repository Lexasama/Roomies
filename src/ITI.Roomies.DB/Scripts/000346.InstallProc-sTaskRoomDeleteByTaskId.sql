create procedure rm.sTaskRoomDeleteByTaskId
(
	@TaskId int
)
as
begin
   
    delete from rm.tiTaskRoom where TaskId = @TaskId;
    return 0;
end;