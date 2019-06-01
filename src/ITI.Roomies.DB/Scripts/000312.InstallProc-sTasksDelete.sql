	create procedure rm.sTasksDelete
	(
		@TaskId int
	)
	as
	begin

		delete from rm.tTasks where TaskId = @TaskId;
		return 0;
	end;
