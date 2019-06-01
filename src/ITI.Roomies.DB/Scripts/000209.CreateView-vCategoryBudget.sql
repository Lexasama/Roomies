create view rm.vCategoryBudget
as 
	select	
		CategoryId = c.CategoryId,
		Icon = c.IconName,
		CategoryName = c.CategoryName,
		CollocId = c.CollocId,
		BudgetId = b.BudgetId,
		Debut = b.Date1,
		fin = b.Date2,
		Amount = b.Amount
	from rm.tCategory c
		left outer join rm.tBudget b on b.CategoryId = c.CategoryId
	where c.CategoryId <> 0;