create procedure rm.sUserUpdate
(
    @UserId int,
    @Email  nvarchar(64)
)
as
begin
    update rm.tUser
    set Email = @Email
    where UserId = @UserId;
    return 0;
end;