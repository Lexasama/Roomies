create procedure rm.sTransacDepenseDelete
(
    @TDepenseId int
)
as
begin
   
    delete from rm.tTransacDepense where TDepenseId = @TDepenseId;
    return 0;
end;