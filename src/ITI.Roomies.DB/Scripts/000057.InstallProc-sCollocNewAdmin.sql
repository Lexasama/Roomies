create proc rm.sCollocNewAdmin
(
	@CollocId int
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	update rm.tiCollRoom
	set AdminColloc = 1
	where CollocId = @CollocId;

	commit;
    return 0;
end;
