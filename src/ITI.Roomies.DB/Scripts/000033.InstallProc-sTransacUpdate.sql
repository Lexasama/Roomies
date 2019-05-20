create procedure rm.sTransacUpdate
(
	@TransacId int out,
	@TransacDesc nvarchar(32),
	@TransacPrice int,
	@CollocId int,
	@RoomieId int
)

as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tTransaction t where t.TransacId = @TransacId)
	begin
		rollback;
		return 1;
	end;

	update rm.tTransaction
	set @TransacDesc = @TransacDesc, TransacPrice = @TransacPrice, RoomieId = @RoomieId, CollocId = @CollocId
	where TransacId = @TransacId;

	commit;
    return 0;
end;
