create procedure rm.sBudgetCreate
(	
	
	@BudgetId int out,
	@Date1 datetime2,
	@Date2 datetime2,
	@Amount int,
	@CategoryId int
	
)
as
begin

	set transaction isolation level serializable
	begin tran;

	insert into rm.tBudget( CategoryId, Date1, Date2, Amount)
				values( @CategoryId, @Date1, @Date2, @Amount)
	set @BudgetId = SCOPE_IDENTITY();
	commit;
	return 0;

end;