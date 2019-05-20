using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;

namespace ITI.Roomies.DAL
{
    public class CollRoomGateway
    {
        readonly string _connectionString;

        public CollRoomGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public async Task<Result<CollRoomData>> FindById( int collocId, int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                CollRoomData CR = await con.QueryFirstOrDefaultAsync<CollRoomData>(
                    @"select i.CollocId,
                             i.RoomieId
                        from rm.tiCollRoom i
                        where i.CollocId = @CollocId and i.RoomieId = @RoomieId;",
                new { CollocId = collocId } );

                if( CR == null ) return Result.Failure<CollRoomData>( Status.NotFound, "Not found." );
                return Result.Success( CR );
            }
        }

        public async Task<Result<int>> AddCollRoom( int collocId, int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CollocId", collocId );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCollRoomAdd", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@CollocId" ) );
            }
        }

        public async Task<Result> Delete( int collocId, int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@RoomieId", roomieId );
                p.Add( "@CollocId", collocId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCollRoomDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Roomie not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result<int>> FindCollocByRoomieId( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                int colloc = await con.QueryFirstOrDefaultAsync<int>(
                    @"select i.CollocId
                        from rm.tiCollRoom i
                        where i.RoomieId = @RoomieId;",
                    new { RoomieId = roomieId } );

                // Return et procédure correctes
                //if( task == null ) return Result.Failure<int>( Status.NotFound, "No collocation was found for this Roomie." );
                return Result.Success( colloc );
            }
        }

        public async Task<Result<CollocData>> FindCollocNameByRoomieId( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                CollocData collocName = await con.QueryFirstOrDefaultAsync<CollocData>(
                    @"select CollocName, CollocId
                        from rm.vCollocInfo 
                        where RoomieId = @RoomieId;",
                    new { RoomieId = roomieId } );
                // Return et procédure correctes
                //if( task == null ) return Result.Failure<int>( Status.NotFound, "No collocation was found for this Roomie." );
                return Result.Success( collocName );
            }
        }

        // TO DO : ajouter type Result proprement
        public async Task<IEnumerable<RoomiesData>> FindRoomiesIdNamesByCollocId( int collocId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                IEnumerable<RoomiesData> CR = await con.QueryAsync<RoomiesData>(
                    @"select RoomieId, FirstName, LastName
                      from rm.vRoomieInfo 
                      where CollocId = @CollocId;",
                new { CollocId = collocId } );

                // Return et procédure correctes
                //if( CR == null ) return Result.Failure<RoomiesData>( Status.NotFound, "Not found." );
                return CR;
            }
        }
        public async Task<Result> LeaveCollocation( int collocId, int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CollocId", collocId );
                p.Add( "@RoomieId", roomieId );


                await con.ExecuteAsync( "rm.sCollRoomDelete", p, commandType: CommandType.StoredProcedure );

                return Result.Success(  );

            }


        }
    }
}
