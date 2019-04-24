using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;

namespace ITI.Roomies.DAL
{
    public class ImageGateway
    {
        readonly string _connectionString;

        public ImageGateway( string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Result> AddImageOfRoomie(int userId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add("@RoomieId", userId);
                p.Add("@RoomiePic", userId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sRoomiePic", p, commandType: CommandType.StoredProcedure );
                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Roomie not found." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );

            }
        }

        public async Task<Result> AddImageOfColloc( int collocId, string imageName )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@RoomieId", collocId );
                p.Add( "@RoomiePic", imageName );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCollocPic", p, commandType: CommandType.StoredProcedure );
                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "colloc not found." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );

            }
        }
    }
}
