Create procedure rm.sTransacDepenseCreate
(	
	@TDepenseId int out,
	@Price int,
	@Date datetime2,
	@SRoomieId int,
	@RRoomieId int
)
as 
begin
	set transaction isolation level serializable;
	begin tran;

	insert into rm.tTransacDepense( Price, [Date], SRoomieId, RRoomieId)
	values( @Price, @Date, @SRoomieId, @RRoomieId)
	set @TDepenseId = SCOPE_IDENTITY();

	commit;
	return 0;
end;