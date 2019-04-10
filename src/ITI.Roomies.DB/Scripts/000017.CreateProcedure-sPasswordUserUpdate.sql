create procedure rm.sPasswordUserUpdate
(
    @UserId   int,
    @Password varbinary(128)
)
as
begin
    update rm.tPasswordUser set [Password] = @Password where UserId = @UserId;
    return 0;
end;