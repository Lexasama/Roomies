create view rm.vCategory
as
	select
		CategoryId = c.CategoryId,
		 IconName = c.IconName,
		CategoryName = c.CategoryName,
		CollocId = c.CollocId,
		BudgetId = b.BudgetId,
		Debut = b.Date1,
		fin = b.Date2,
		Amount = b.Amount,
		TransactionPrice = tb.Price,
		TransactionDate = tb.[Date],
		DepensePrice = td.Price,
		DepenseDate = td.[Date],
		SenderRoomie = td.SRoomieId,
		RecieverRoomie = td.RRoomieId 
	from rm.tCategory c
		left outer join rm.tBudget b on b.CategoryId = c.CategoryId  
		left outer join rm.tTransacBudget tb on tb.BudgetId = b.BudgetId 
		left outer join rm.tTransacDepense td on td.SRoomieId = tb.RoomieId
	where c.CategoryId <> 0 ;

