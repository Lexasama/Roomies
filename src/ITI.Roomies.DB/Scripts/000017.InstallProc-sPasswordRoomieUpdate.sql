create procedure rm.sPasswordRoomieUpdate
(
    @RoomieId   int,
    @Password varbinary(128)
)
as
begin
    update rm.tPasswordRoomie set [Password] = @Password where RoomieId = @RoomieId;
    return 0;
end;