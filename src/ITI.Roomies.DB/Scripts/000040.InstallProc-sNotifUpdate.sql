Create procedure rm.sNotifUpdate
(
	@NotifId	int,
	@RoomieId	int,
	@Message	nvarchar(max),
	@Seen		bit
)
as
begin 
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tNotif n where n.NotifId = @NotifId)
	begin
		rollback;
		return 1;
	end;

	if exists( select * from rm.tNotif n where n.NotifId <> @NotifId)
	begin
		rollback;
		return 2;
	end;

	update rm.tNotif
	set RoomieId = @RoomieId, [Message]= @Message, Seen = @Seen
	where NotifId = @NotifId;

	commit;
    return 0;
end;