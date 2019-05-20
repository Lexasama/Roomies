using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ITI.Roomies.DAL
{
    public class TasksGateway
    {
        readonly string _connectionString;

        public TasksGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        // TO DO : Type et return avec Result
        public async Task<IEnumerable<TasksData>> FindTaskByCollocId( int collocId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                IEnumerable<TasksData> tasks = await con.QueryAsync<TasksData>(
                //@"select t.TaskId,
                //         t.TaskName,
                //         t.TaskDes,
                //         t.TaskDate,
                //         t.State,
                //         t.CollocId
                //  from rm.tTasks t
                //  where t.CollocId = @CollocId;",
                //new { CollocId = collocId } );

                @"select t.TaskId, t.TaskName, t.TaskDes, t.TaskDate, t.State, t.CollocId, tr.RoomieId, r.FirstName, r.LastName
                  from rm.tTasks t inner join rm.tiTaskRoom tr on t.TaskId = tr.TaskId
                                   inner join rm.tRoomie r on tr.RoomieId = r.RoomieId
                  where t.CollocId = @CollocId
                  order by t.TaskId, r.FirstName;"
                , new { CollocId = collocId } );

                return tasks;
            }
        }

        // TO DO : Type et return avec Result
        public async Task<IEnumerable<TasksData>> FindTaskByRoomieId( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                IEnumerable<TasksData> tasks = await con.QueryAsync<TasksData>(
                    //              @"select t.TaskId,
                    //                      t.TaskName,
                    //                      t.TaskDes,
                    //                      t.TaskDate,
                    //                      t.State,
                    //                      t.CollocId,
                    //	r.RoomieId,
                    //	r.FirstName,
                    //	r.LastName
                    //                from rm.tTasks t
                    //inner join rm.tiTaskRoom tr on t.TaskId = tr.TaskId
                    //inner join rm.tRoomie r on tr.RoomieId = r.RoomieId
                    //                where tr.RoomieId = @RoomieId;",
                    //              new { RoomieId = roomieId } );
                    @"select t.TaskId,                             
					        t.TaskName,
                            t.TaskDes,                             
							t.TaskDate,
                            t.State,                            
							t.CollocId,
							r.RoomieId,							
							r.FirstName,
							r.LastName
                      from rm.tiTaskRoom tr
						inner join rm.tTasks t on t.TaskId = tr.TaskId
						inner join rm.tRoomie r on tr.RoomieId = r.RoomieId
					where t.TaskId in (select ti.TaskId from rm.tiTaskRoom ti where ti.RoomieId = @RoomieId)
					ORDER BY t.CollocId, t.TaskId, r.FirstName;",
                    new { RoomieId = roomieId } );

                return tasks;
            }
        }

        // TO DO : Type et return avec Result
        // TO DO : Return avec un tableau de Roomie plutôt que plusieurs requêtes
        public async Task<IEnumerable<TasksData>> FindTaskByTaskId( int taskId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                IEnumerable<TasksData> task = await con.QueryAsync<TasksData>(
                  @"select t.TaskId, t.TaskName, t.TaskDes, t.TaskDate, t.State, t.CollocId, tr.RoomieId, r.FirstName, r.LastName
                  from rm.tTasks t inner join rm.tiTaskRoom tr on t.TaskId = tr.TaskId
                                   inner join rm.tRoomie r on tr.RoomieId = r.RoomieId
                  where t.TaskId = @TaskId
                  order by r.FirstName",
                    new { TaskId = taskId } );

                //if( task == null ) return Result.Failure<TasksData>( Status.NotFound, "Task not found." );
                //return Result.Success( task );
                return task;
            }
        }

        public async Task<Result<int>> CreateTask( string taskName, string taskDes, DateTime taskDate, int collocId, int roomieId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TaskName", taskName );
                p.Add( "@TaskDes", taskDes );
                p.Add( "@TaskDate", taskDate );
                p.Add( "@RoomieId", roomieId);
                p.Add( "@CollocId", collocId );
                p.Add( "@TaskId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTasksCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "A Task with this name already exists." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@TaskId" ) );
            }
        }


        public async Task<Result> UpdateTask( int taskId, string taskName, DateTime taskDate, string taskDes )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TaskId", taskId );
                p.Add( "@TaskName", taskName );
                p.Add( "@TaskDate", taskDate );
                p.Add( "@TaskDes", taskDes );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTasksUpdate", p, commandType: CommandType.StoredProcedure );
                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Task not found." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
            }
        }

        public async Task<Result> DeleteTaskById( int taskId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TaskId", taskId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTasksDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Task not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> UpdateTaskState( int taskId, bool taskState )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TaskId", taskId );
                p.Add( "@State", taskState );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTasksUpdateState", p, commandType: CommandType.StoredProcedure );
                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Task not found." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
            }
        }
    }
}
