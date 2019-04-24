create procedure rm.sRoomiePic
(
	@RoomieId int out,
	@RoomiePic int
	
)

as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tRoomies r  where r.RoomieId = @RoomieId)
	begin
		rollback;
		return 1;
	end;

	if exists(select * from rm.tRoomies r where r.RoomieId <> @RoomieId)
	begin
		rollback;
		return 2;
	end;

	update rm.tRoomies
	set RoomiePic = @RoomiePic
	where RoomieId = @RoomieId;

	commit;
    return 0;
end;