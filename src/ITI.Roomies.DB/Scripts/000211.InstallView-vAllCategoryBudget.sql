Create view rm.vAllCategoryBudget
as
	select 
	cb.CollocId,
		cb.CategoryName, 
		Amount= sum(cb.Amount)
	 from  rm.vCategoryBudget cb
	 group by CategoryName, Amount, CollocId
	