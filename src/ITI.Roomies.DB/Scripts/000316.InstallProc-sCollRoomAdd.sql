create Procedure rm.sCollRoomAdd
(
	@CollocId int,
	@RoomieId int 
)
as 
begin


	insert into rm.tiCollRoom(CollocId, RoomieId, AdminColloc)
	                  values(@CollocId, @RoomieId, 0 );
	return 0;
end;

