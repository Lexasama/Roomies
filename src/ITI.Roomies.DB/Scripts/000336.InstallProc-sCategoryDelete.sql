create proc rm.sCategoryDelete
(
	@CategoryId int
)
as 
begin

	delete from rm.tCategory where CategoryId = @CategoryId
	return 0; 
end;