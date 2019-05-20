create procedure rm.sCollRoomDelete
(
    @CollocId int,
	@RoomieId int
)
as
begin
   
    delete from rm.tiCollRoom where CollocId = @CollocId and RoomieId = @RoomieId;
    
    if (select count(rm.tiCollRoom.RoomieId) from rm.tiCollRoom where rm.tiCollRoom.CollocId =@CollocId) = 0
        begin
        delete from rm.tColloc where rm.tColloc.CollocId=@CollocId
    end;

    return 0;
end;
