create procedure rm.sUserDelete
(
    @UserId int
)
as
begin
    delete from rm.tPasswordUser where UserId = @UserId;
    delete from rm.tGoogleUser where UserId = @UserId;
    delete from rm.tUser where UserId = @UserId;
    return 0;
end;