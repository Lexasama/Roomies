create proc rm.sCheckInvite
(
    @InvitationKey nvarchar(64),
    @IdColloc int out
)
as
begin
    set transaction isolation level serializable;
	begin tran;
    select IdColloc from rm.tInvitation where InvitationKey = @InvitationKey;
    set @IdColloc = scope_identity();
    commit;
	return 0;
end;
