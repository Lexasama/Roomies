create proc rm.sRoomiesUpdate
(
	@RoomieId	 int, 
	@Description nvarchar(200),
	@Phone		 varchar(10)
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

	if exists(select * from rm.tRoomies r where r.RoomieId <> @RoomieId)
	begin
		rollback;
		return 2;
	end;

	update rm.tRoomies
	set [Description] = @Description, Phone = @Phone
	where RoomieId = @RoomieId;

	commit;
    return 0;
end;