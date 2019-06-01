create procedure rm.sBudgetDelete
(
    @BudgetId int
)
as
begin
   
    delete from rm.tBudget where BudgetId = @BudgetId;
    return 0;
end;