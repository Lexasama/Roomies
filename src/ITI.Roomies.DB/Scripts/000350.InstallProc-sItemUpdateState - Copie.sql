create proc rm.sItemUpdateState
(
	@ItemId	 int,
	@ItemSaved		bit
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

	update rm.tItem
	set ItemSaved = @itemSaved
	where ItemId = @ItemId;

	commit;
    return 0;
end;
