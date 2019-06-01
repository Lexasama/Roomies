create procedure rm.sNotifCreate
(
	@NotifId int out,
	@RoomieId int ,
	@Message nvarchar(max)
)
as
begin
	set transaction isolation level serializable;
	begin tran;
	insert into rm.tNotif(RoomieId, [Message]) 
				   values(@RoomieId, @Message)
	set @NotifId = SCOPE_IDENTITY();
	
	commit;
	return 0;
end;