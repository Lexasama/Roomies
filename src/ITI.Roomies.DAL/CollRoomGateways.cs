using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;

namespace ITI.Roomies.DAL
{
    public class CollRoomGateways
    {
        readonly string _connectionString;

        public CollRoomGateways( string connectionString )
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
                            inner join rm.tColloc c on i.CollocId = c.CollocId
                            left outer join rm.tRoomies r on i.RoomieId = r.RoomieId
                        where i.CollocId = @CollocId and i.RoomieId = @RoomieId;",
                new { CollocId = collocId } );

                if( CR == null ) return Result.Failure<CollRoomData>( Status.NotFound, "Not found." );
                return Result.Success( CR );
            }
        }

        public async Task<Result<int>> AddCollRoom(int collocId, int roomieId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CollocId", collocId);
                p.Add( "@RoomieId", roomieId );
                p.Add( "@CollocId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@RoomieId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCollRoomAdd", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@CollocId" ) );
            }
        }

        public async Task<Result> Delete( int collocId ,int roomieId)
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
    }
}
