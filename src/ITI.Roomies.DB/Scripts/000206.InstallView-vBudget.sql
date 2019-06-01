create view rm.vBudget
as 
	select * from rm.tBudget b
    where b.BudgetId <> 0;