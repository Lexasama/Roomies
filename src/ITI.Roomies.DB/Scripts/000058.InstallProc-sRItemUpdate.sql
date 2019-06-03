create procedure rm.sRItemUpdate
(
	@RItemId	int,
	@RItemName	nvarchar(32),
	@CollocId	int,
	@CourseTempId	int
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.RItem ri where ri.RItemId = @RItemId )
	begin
		rollback;
		return 1;
	end;

	if exists(select * from rm.tRItem ri. where ri.RItemId <> @RItemId and ri.CollocId = @Colloc
		ri.ItemName = @RItemName and ri.CourseTempId = @CourseTempId)
	begin
		rollback;
		return 2;
	end;
	
	update rm.tRItem
	set RItemName = @RItemName, RItemPrice = @RItemPrice,
		CollocId = @CollocId,	CouseTempId = @CourseTempId 
	where RItemId = @RItemId.
	
	commit;
	return 0;
end; 