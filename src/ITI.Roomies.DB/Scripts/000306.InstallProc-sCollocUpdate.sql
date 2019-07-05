create proc rm.sCollocUpdate
(
	@CollocId int,
	@CollocName nvarchar(32),
	@CollocPic nvarchar(max)
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	update rm.tColloc
	set @CollocName = @CollocName, CollocPic = @CollocPic
	where CollocId = @CollocId;

	commit;
    return 0;
end;