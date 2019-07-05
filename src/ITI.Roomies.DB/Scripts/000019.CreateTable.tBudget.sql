create table rm.tBudget
(
	BudgetId int identity(0,1) not null,
	CategoryId int not null,
	Date1 datetime2 not null,
	Date2 datetime2 not null,
	Amount int not null,

	constraint PK_rm_tBudget primary key (BudgetId),
	constraint UK_rm_tBudget_Date check (Date1 <= Date2),
	constraint FK_rm_tBudget_CategoryId foreign key (CategoryId) references rm.tCategory(CategoryId)
);

insert into rm.tBudget(CategoryId, date1, date2 , Amount) 
					values(0,'20000101', '20000101', 0);