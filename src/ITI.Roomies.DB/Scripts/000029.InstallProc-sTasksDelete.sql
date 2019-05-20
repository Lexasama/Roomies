create procedure rm.sTasksDelete
(
    @TaskId int
)
as
begin
   
	delete from rm.tiTaskRoom where TaskId = @TaskId;
    delete from rm.tTasks where TaskId = @TaskId
    return 0;
end;