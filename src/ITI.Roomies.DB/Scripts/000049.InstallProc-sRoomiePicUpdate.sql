create proc rm.sRoomiePicUpdate
(	
	@RoomiePic nvarchar(max),
	@RoomieId int
)
as 
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tRoomie r where r.RoomieId = @RoomieId)
	begin
		rollback;
		return 1;
	end;

	update rm.tRoomie 
		set RoomiePic = @RoomiePic
	where RoomieId = @RoomieId;

	commit; 
	return 0;

end;
