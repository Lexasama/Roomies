create Procedure rm.sTaskRoomAdd
(
	@TaskId int out,
	@RoomieId int out
)
as 
begin
	set transaction isolation level serializable;
	begin tran;


	if exists(select * from rm.tiTaskRoom t where t.TaskId = @TaskId and t.RoomieId = @RoomieId)
	begin
		rollback;
		return 1;
	end;

	insert into rm.tiTaskRoom(TaskId, RoomieId)
	                  values(@TaskId, @RoomieId );
	commit;
	return 0;
end;