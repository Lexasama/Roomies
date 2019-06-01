create procedure rm.sBudgetCreate
(	
	
	@CategoryId int,
	@Date1 datetime2,
	@Date2 datetime2,
	@CollocId int,
	@Amount int,
	@BudgetId int out
)
as
begin

	set transaction isolation level serializable
	begin tran;

	insert into rm.tBudget( CategoryId, Date1, Date2, Amount, CollocId )
				values( @CategoryId, @Date1, @Date2, @Amount, @CollocId)
	set @BudgetId = SCOPE_IDENTITY();
	commit;
	return 0;

end;