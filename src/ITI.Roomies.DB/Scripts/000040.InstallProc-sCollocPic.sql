create procedure rm.sCollocPic
(
	@CollocId int out,
	@CollocPic int
	
)

as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tColloc c where c.CollocId = @CollocId)
	begin
		rollback;
		return 1;
	end;

	if exists(select * from rm.tColloc c where c.CollocId <> @CollocId)
	begin
		rollback;
		return 2;
	end;

	update rm.tColloc
	set CollocPic = @CollocPic
	where CollocId = @CollocId;

	commit;
    return 0;
end;