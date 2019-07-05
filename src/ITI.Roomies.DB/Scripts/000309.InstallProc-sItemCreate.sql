create procedure rm.sItemCreate
(
    @ItemId int out,
	@CollocId	int,
	@ItemName   nvarchar(32),
    @ItemPrice	int,
    @ItemSaved  bit
)
as
begin
    set transaction isolation level serializable;
	begin tran;

	insert into rm.tItem(ItemName, ItemPrice, CollocId, ItemSaved)
	                  values(@ItemName, @ItemPrice, @CollocId, @ItemSaved);
		set @ItemId = scope_identity();
	
	commit;
	return 0;
end;
