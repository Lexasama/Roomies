Create procedure rm.sTransacBudgetCreate
(	
	@TBudgetId int out,
	@Price int,
	@Date datetime2,
	@BudgetId int,
	@RoomieId int
)
as 
begin
	set transaction isolation level serializable;
	begin tran;

	insert into rm.tTransacBudget( Price, [Date], BudgetId, RoomieId)
					values( @Price, @Date, @BudgetId, @RoomieId)
	set @TBudgetId = SCOPE_IDENTITY();

	commit;
	return 0;
end;