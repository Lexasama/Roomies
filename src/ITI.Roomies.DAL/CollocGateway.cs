using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ITI.Roomies.DAL
{

    public class CollocGateway
    {
        readonly string _connectionString;

        public CollocGateway( string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Result<CollocData>> FindById( int collocId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                CollocData colloc = await con.QueryFirstOrDefaultAsync<CollocData>(
                    @"select c.CollocId,
                             c.CollocName,
                             c.CreationDate
                      from rm.tColloc c
                      where c.CollocId = @CollocId;",
                    new { CollocId = collocId } );

                if( colloc == null ) return Result.Failure<CollocData>( Status.NotFound, "Colloc not found." );
                return Result.Success( colloc );
            }
        }


        public async Task<Result<int>> CreateColloc(string collocName )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CollocName", collocName );
                p.Add( "@CollocId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCollocCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "A Flatsharing with this name already exists." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@CollocId" ) );
            }
        }

        public async Task<Result> Update (int collocId, string collocName, string collocPic)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CollocId", collocId );
                p.Add( "@CollocName", collocName );
                p.Add( "@CollocPic", collocPic );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCollocUpdate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Flatsharing not found." );

                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
            }
        }

        public async Task<int> getRoomieIdByEmail( string email )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                int result = await con.QueryFirstOrDefaultAsync<int>(
                     "select r.RoomieId, r.FirstName from rm.tRoomie r where r.Email = @Email",
                     new { Email = email } );
                
                return result;

            }
        }

        public async Task<Result> Invitation( string guid, int idReceiver, int idSender, int idColloc)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@InvitationKey", guid );
                p.Add( "IdReceiver", idReceiver );
                p.Add( "IdSender", idSender );
                p.Add( "IdColloc", idColloc );

                await con.ExecuteAsync( "rm.sInvitation", p, commandType: CommandType.StoredProcedure );
                
                return Result.Success( Status.Ok );


            }
            
        }

        public async Task<int> CheckInvitaion(string invitationKey)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@InvitationKey", invitationKey );
                int result = await con.QueryFirstOrDefault("rm.sCheckInvite",p,commandType:CommandType.StoredProcedure);


                return result;
            }

        }

        public async Task<Result> DeleteInvite(string invitationKey )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@InvitationKey", invitationKey );
                int result = await con.QueryFirstOrDefault( "rm.sDeleteInvite", p, commandType: CommandType.StoredProcedure );

                return Result.Success( Status.Ok );
            }
            
        }

    }
}
