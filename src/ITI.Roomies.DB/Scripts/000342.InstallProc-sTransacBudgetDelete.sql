create procedure rm.sTransacBudgetDelete
(
    @TBudgetId int
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists( select * from rm.tTransacBudget tb where tb.TBudgetId = @TBudgetId)
	begin
		rollback;
		return 1;
	end;

    delete from rm.tTransacBudget where TBudgetId = @TBudgetId;
	commit;
    return 0;
end;