create table rm.tTransacBudget
(	
	TBudgetId int identity(0,1),
	[Desc] nvarchar(200),
	Price int not null,
	[Date] datetime2 not null,
	BudgetId int not null,
	RoomieId int not null
	
	constraint PK_rm_tTbudget primary key (TBudgetId),
	constraint FK_rm_tTBudget_BudgetId foreign key (BudgetId) references rm.tBudget(BudgetId),
	constraint FK_rm_tTBudget_RoomieId foreign key (RoomieId) references rm.tRoomie(RoomieId) 
);

insert into rm.tTransacBudget([Desc], Price, [Date], BudgetId, RoomieId)
					values(N'', 0, '20000101', 0, 0);