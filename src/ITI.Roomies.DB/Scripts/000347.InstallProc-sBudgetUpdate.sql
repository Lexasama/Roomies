create proc rm.sBudgetUpdate
(
	@BudgetId int,
	@CategoryId int,
	@Date1 datetime2,
	@Date2 datetime2,
	@Amount int
)
as 
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tBudget b where b.BudgetId = @BudgetId)
	begin
		rollback;
		return 1;
	end;

	if exists(select * from rm.tBudget b where b.Amount <> @BudgetId
		and b.CategoryId = @CategoryId
		and b.Date1 = @Date1
		and b.Date2 = @Date2
		and b.Amount = @Amount)
	begin
		rollback;
		return 2;
	end

	update rm.tBudget
	set CategoryId = @CategoryId,
		Date1 = @Date1,
		Date2 = @Date2,
		Amount = @Amount
	where BudgetId = @BudgetId;

	commit;
    return 0;
end;