using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ITI.Roomies.DAL
{

    public class CollocGateway
    {
        readonly string _connectionString;

        public CollocGateway( string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Find a colloc by it Id
        /// </summary>
        /// <param name="collocId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a collocation
        /// </summary>
        /// <param name="collocName"></param>
        /// <param name="roomieId"></param>
        /// <returns></returns>
        public async Task<Result<int>> CreateColloc(string collocName, int roomieId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CollocName", collocName );
                p.Add( "@RoomieId", roomieId);
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

        public async Task<int> CheckInvitation(string invitationKey)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                int result = await con.QueryFirstOrDefaultAsync<int>(
                    @"select IdColloc from rm.tInvitation where InvitationKey = @InvitationKey;",
                    new { InvitationKey = invitationKey } );
                return result;
            }
        }

        public async Task<Result> DeleteInvite(string invitationKey )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@InvitationKey", invitationKey );
                int result = await con.ExecuteAsync( "rm.sDeleteInvite", p, commandType: CommandType.StoredProcedure );

                return Result.Success( Status.Ok );
            }
        }
        public async Task<IEnumerable<CollocData>> getCollocInformation( int collocId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                IEnumerable<CollocData> colloc = await con.QueryAsync<CollocData>(
                @"select * from rm.vCollocInfo where CollocId=@CollocId"
                , new { CollocId = collocId } );

                return colloc;
            }
        }

        public async Task<int> IsAdminAsync( int collocId, int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                int result = await con.QueryFirstOrDefaultAsync<int>(
                     @"select AdminColloc from rm.tiCollRoom where CollocId=@CollocId and RoomieId=@RoomieId",
                     new { CollocId = collocId, RoomieId= roomieId } );

                return result;
            }
        }

        public async Task<Result<string>> GetCollocPic( int collocId )
        {
           using( SqlConnection con = new SqlConnection( _connectionString))
           {
                string result = await con.QueryFirstAsync<string>(
                    @"select p.CollocPic from rm.tColloc c where c.CollocId = @CollocId",
                    new { CollocId = collocId } );
                if( result == null ) return Result.Failure<string>( Status.NotFound, "FlatSharing has no pictures" );

                return Result.Success( result );
            }
        }

        public async Task<Result> DestroyCollocAsync( int collocId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CollocId", collocId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sDeleteColloc", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Roomie not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

    }
}
