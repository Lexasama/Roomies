create procedure rm.sRoomieDelete
(
    @RoomieId int
)
as
begin
    delete from rm.tPasswordRoomie where RoomieId = @RoomieId;
    delete from rm.tGoogleRoomie where RoomieId = @RoomieId;
    delete from rm.tRoomie where RoomieId = @RoomieId;
    return 0;
end;