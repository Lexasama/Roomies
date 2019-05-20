create Procedure rm.sCollRoomAdd
(
	@CollocId int,
	@RoomieId int 
)
as 
begin


	insert into rm.tiCollRoom(CollocId, RoomieId)
	                  values(@CollocId, @RoomieId );
	return 0;
end;

