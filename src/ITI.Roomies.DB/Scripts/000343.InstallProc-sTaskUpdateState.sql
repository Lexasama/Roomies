create proc rm.sTasksUpdateState
(
	@TaskId	 int,
	@State		bit
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tTasks t where t.TaskId = @TaskId)
	begin
		rollback;
		return 1;
	end;

	update rm.tTasks
	set [State] = @State
	where TaskId = @TaskId;

	commit;
    return 0;
end;
