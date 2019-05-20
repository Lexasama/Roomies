create procedure rm.sGoogleRoomieCreateOrUpdate
(
    @Email        nvarchar(64),
    @GoogleId     varchar(32),
    @RefreshToken varchar(64)
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if exists(select * from rm.tGoogleRoomie u where u.GoogleId = @GoogleId)
	begin
		update rm.tGoogleRoomie set RefreshToken = @RefreshToken where GoogleId = @GoogleId;
		commit;
		return 0;
	end;

    declare @roomieId int;
	select @roomieId = r.RoomieId from rm.tRoomie r where r.Email = @Email;

	if @roomieId is null
	begin
		insert into rm.tRoomie(Email) values(@Email);
		set @roomieId = scope_identity();
	end;
    
    insert into rm.tGoogleRoomie(RoomieId,  GoogleId,  RefreshToken)
                         values(@roomieId, @GoogleId, @RefreshToken);
	commit;
    return 0;
end;