create Procedure rm.sTransacCreate
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


	if exists(select * from rm.tTransaction t where t.TransacDesc= @TransacDesc and t.TransacPrice = @TransacPrice)
	begin
		rollback;
		return 1;
	end;

	insert into rm.tTransaction(TransacDesc, TransacPrice, RoomieId, CollocId)
	                  values(@TransacDesc, @TransacPrice, @RoomieId, @CollocId);
	set @TransacId = scope_identity();
	
	commit;
	return 0;
end;
