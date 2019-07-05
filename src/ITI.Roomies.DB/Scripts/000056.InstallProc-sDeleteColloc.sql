create proc rm.sDeleteColloc
(
	@CollocId int
)
as
begin
	delete from rm.tiCollRoom where CollocId = @CollocId;
    delete from rm.tColloc where CollocId = @CollocId;
	return 0;
	
end;
