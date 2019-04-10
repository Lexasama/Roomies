create procedure rm.sGoogleUserCreateOrUpdate
(
    @Email        nvarchar(64),
    @GoogleId     varchar(32),
    @RefreshToken varchar(64)
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if exists(select * from rm.tGoogleUser u where u.GoogleId = @GoogleId)
	begin
		update rm.tGoogleUser set RefreshToken = @RefreshToken where GoogleId = @GoogleId;
		commit;
		return 0;
	end;

    declare @userId int;
	select @userId = u.UserId from rm.tUser u where u.Email = @Email;

	if @userId is null
	begin
		insert into rm.tUser(Email) values(@Email);
		set @userId = scope_identity();
	end;
    
    insert into rm.tGoogleUser(UserId,  GoogleId,  RefreshToken)
                         values(@userId, @GoogleId, @RefreshToken);
	commit;
    return 0;
end;