create procedure rm.sNotifDelete
(
    @NotifId int
)
as
begin
   
    delete from rm.tNotif where NotifId = @NotifId;
    return 0;
end;