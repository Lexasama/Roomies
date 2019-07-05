using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;
using System;

namespace ITI.Roomies.DAL
{
    public class TaskRoomGateway
    {
        readonly string _connectionString;

        public TaskRoomGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public async Task<Result<TaskRoomData>> FindByTaskId( int taskId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                TaskRoomData TR = await con.QueryFirstOrDefaultAsync<TaskRoomData>(
                    @"",
                    new { TaskId = taskId } );

                if( TR == null ) return Result.Failure<TaskRoomData>( Status.NotFound, "Not found" );
                return Result.Success(TR);
                   
            }
        }

        public async Task<Result<int>> AddTaskRoom (int taskId, int roomieId)
        {
            using( SqlConnection con = new SqlConnection ( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TaskId", taskId );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTaskRoomAdd", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@TaskId" ) );
            }
        }


        public async Task<Result> Delete( int taskId, int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TaskId", taskId );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTaskRoomDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Roomie not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> DeleteTaskRoomByTaskId( int taskId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TaskId", taskId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTaskRoomDeleteByTaskId", p, commandType: CommandType.StoredProcedure );

                return Result.Success();
            }
        }
      

    }
}
