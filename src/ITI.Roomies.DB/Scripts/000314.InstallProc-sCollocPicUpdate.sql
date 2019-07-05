create proc rm.sCollocPicUpdate
(
	@CollocPic nvarchar(max),
	@CollocId int
)
as begin 
	set transaction isolation level serializable;
	begin tran; 

	if not exists( 
		select * from rm.tColloc c where c.CollocId = @CollocId)
	begin 
		rollback;
		return 1;
	end;

	update rm.tColloc
		set CollocPic = @CollocPic
		where CollocPic = @CollocPic;

	commit;
	return 0;

end;