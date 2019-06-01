	create procedure rm.sTasksCreate
	(
		@TaskId int out,
		@TaskName   nvarchar(32),
		@TaskDes	nvarchar(200) = null,
		@TaskDate	datetime2,
		@CollocId int
	)
	as
	begin
		set transaction isolation level serializable;
		begin tran;

		insert into rm.tTasks ( TaskName, TaskDate, TaskDes, CollocId)
			values(@TaskName, @TaskDate, @TaskDes, @CollocId);
		set @TaskId = scope_identity();
			
		commit;
			return 0;
	end;
