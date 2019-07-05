create proc rm.sTransacBudgetUpdate
(
	@TBudgetId int,
	@Price int, 
	@Date datetime2,
	@BudgetId int,
	@RoomieId int
)
as
begin
    set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tTransacBudget tb where tb.TBudgetId = @TBudgetId)
	begin
		rollback;
		return 1;
	end;

	if exists( select * from rm.tTransacBudget tb where tb.BudgetId <> @TBudgetId
		and tb.RoomieId = @RoomieId
		and tb.[Date] = @Date
		and tb.BudgetId = @BudgetId )
	begin 
		rollback;
		return 2;
	end;

update rm.tTransacBudget
set 
	Price = @Price,
	[Date] = @Date,
	BudgetId = @BudgetId,
	RoomieId = @RoomieId

where TBudgetId = @TBudgetId;
	commit;
    return 0;
end;