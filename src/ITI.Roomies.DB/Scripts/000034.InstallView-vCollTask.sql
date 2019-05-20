create view rm.vCollTask
as 
	select
                CollocId = c.CollocId,
                TaskId = t.TaskId,
                TaskName = t.TaskName,
				[State] = t.State
            from rm.tColloc c
                inner join rm.tTasks t on c.CollocId = t.CollocId
            where c.CollocId <> 0;