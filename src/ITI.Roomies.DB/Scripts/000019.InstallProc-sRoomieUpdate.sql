create procedure rm.sUserUpdate
(
    @RoomieId int,
    @Email  nvarchar(64)
)
as
begin
    update rm.tRoomie
    set Email = @Email
    where RoomieId = @RoomieId;
    return 0;
end;