create Procedure rm.sTransacDelete
(
    @TransacId int
)
as
begin
   
    delete from rm.tTransaction where TransacId = TransacId;
    return 0;
end;