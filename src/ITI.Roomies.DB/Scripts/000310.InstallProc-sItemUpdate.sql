create procedure rm.sItemUpdate
(
	@ItemId		int,
	@CollocId	int,
    @ItemName   nvarchar(32),
    @ItemPrice	int
)

as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tItem t where t.ItemId = @ItemId)
	begin
		rollback;
		return 1;
	end;

	if exists(select * from rm.tItem t where t.ItemId <> @ItemId and t.ItemName = @ItemName and t.CollocId = @CollocId)
	begin
		rollback;
		return 2;
	end;

	update rm.tItem
	set ItemName = @ItemName, ItemPrice = @ItemPrice, CollocId = @CollocId
	where ItemId = @ItemId;

	commit;
    return 0;
end;
