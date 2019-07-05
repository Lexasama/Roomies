create procedure rm.sItemDelete
(
    @ItemId int
)
as
begin
   
    delete from rm.tItem where ItemId = @ItemId;
    return 0;
end;