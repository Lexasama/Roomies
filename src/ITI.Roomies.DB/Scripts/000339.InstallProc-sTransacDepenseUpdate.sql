create Proc rm.sTransacDepenseUpdate
(
	@TDepenseId int,
	@Price int,
	@Date datetime2,
	@SRoomieId int,
	@RRoomieId int
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists ( select* from rm.tTransacDepense td where td.TDepenseId = @TDepenseId)
	begin
		rollback;
		return 1;
	end;

	update rm.tTransacDepense
	set Price = @Price,
		[Date] = @Date,
		SRoomieId = @SRoomieId,
		RRoomieId = @RRoomieId
	where TDepenseId = @TDepenseId;

	commit;
	return 0;
end;