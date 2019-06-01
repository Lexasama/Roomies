create proc rm.sInvitation
(
    @InvitationKey nvarchar(64),
    @IdReceiver int,
    @IdSender int,
    @IdColloc int
)
as
begin

    insert into rm.tInvitation (InvitationKey,IdColloc,IdReceiver,IdSender)
    values (@InvitationKey,@IdColloc,@IdReceiver,@IdSender);
    return 0;
end;
