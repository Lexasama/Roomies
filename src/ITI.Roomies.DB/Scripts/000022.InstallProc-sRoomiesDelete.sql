create proc rm.sRoomiesDelete
(
    @RoomieId int
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tRoomies r where r.RoomieId = @RoomieId)
	begin
		rollback;
		return 1;
	end;

  
    delete from rm.tRoomies where RoomieId = @RoomieId;

	commit;
    return 0;
end;