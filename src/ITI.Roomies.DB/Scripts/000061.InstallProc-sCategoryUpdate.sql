create proc rm.sCategoryUpdate
(	
	@CategoryId int,
	@CategoryName nvarchar(32),
	@Icon nvarchar(max),
	@CollocId int
)
as
begin 
	 set transaction isolation level serializable;
	 begin tran;

	 if not exists( select * from rm.tCategory c where c.CategoryId = @CategoryId)
	begin
		rollback;
		return 1;
	end;
	
	if exists( select * from rm.tCategory c where CategoryId <> @CategoryId
		and c.CategoryName = @CategoryName
		and c.Icon = @Icon
		and c.CollocId = CollocId)
	begin
		rollback;
		return 2;
	end
	
	update rm.tCategory 
	set CategoryName = @CategoryName,
		Icon = @Icon,
		CollocId = @CollocId
	where CategoryId = @CategoryId;
	
	commit;
	return 0;
end;			