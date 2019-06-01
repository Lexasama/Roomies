create table rm.tBudget
(
	BudgetId int identity(0,1) not null,
	CategoryId int not null,
	Date1 datetime2 not null,
	Date2 datetime2 not null,
	CollocId int not null,
	Amount int not null,

	constraint PK_rm_tBudget primary key (BudgetId),
	constraint FK_rm_tBudget_CollocId foreign key (CollocId)  references rm.tColloc(CollocId),
	constraint FK_rm_tBudget_CategoryId foreign key (CategoryId) references rm.tCategory(CategoryId)
);

insert into rm.tBudget(CategoryId, date1, date2, CollocId, Amount) 
					values(0,'20000101', '20000101', 0, 0);