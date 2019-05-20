create proc rm.sDeleteInvite
(
    @InvitationKey nvarchar(64)
)
as
begin
    delete from rm.tInvitation where InvitationKey = @InvitationKey;
    return 0;
end;
