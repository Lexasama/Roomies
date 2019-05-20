using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;

namespace ITI.Roomies.DAL
{
    public class RoomiesGateway
    {
        readonly string _connectionString;

        public RoomiesGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public async Task<RoomiesData> FindById( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<RoomiesData>(
                    "select r.RoomieId, r.Email, r.[Password], r.GoogleRefreshToken, r.GoogleId from rm.vRoomie r where r.RoomieId = @RoomieId",
                    new { RoomieId = roomieId } );
            }
        }

        public async Task<Result<RoomieProfileData>> GetProfile(int roomieId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                RoomieProfileData roomie =  await con.QueryFirstAsync<RoomieProfileData>(
                    "select * from rm.tRoomie where RoomieId = @RoomieId",
                    new { RoomieId = roomieId } );

                Console.WriteLine(Result.Success(roomie));
                return Result.Success( roomie );
            }
            
        }

        public async Task<RoomiesData> FindByEmail( string email )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<RoomiesData>(
                    "select r.RoomieId, r.Email, r.[Password], r.GoogleRefreshToken, r.GoogleId from rm.vRoomie r where r.Email = @Email",
                    new { Email = email } );
            }
        }

        public async Task<RoomiesData> FindByGoogleId( string googleId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {

                return await con.QueryFirstOrDefaultAsync<RoomiesData>(
                    "select r.RoomieId, r.Email, r.[Password], r.GoogleRefreshToken, r.GoogleId from rm.vRoomie r where r.GoogleId = @GoogleId",
                    new { GoogleId = googleId } );
            }
        }

        public async Task<Result<string>> GetRoomiePicAsync( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                string result = await con.QueryFirstAsync<string>(
                    "select p.RoomiePic from rm.vRoomiesPic p where p.RoomieId = @RoomieId",
                new { RoomieId = roomieId });

                if( result == null ) return Result.Failure<string>( Status.NotFound, "Roomie has no pictures" );

                return Result.Success(result);
            }
        }

        public async Task<Result<RoomiesData>> getRoomieIdByEmail( string email )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                RoomiesData result = await con.QueryFirstOrDefaultAsync<RoomiesData>(
                     "select r.RoomieId, r.FirstName from rm.tRoomie r where r.Email = @Email",
                     new { Email = email } );

                if( result == null ) return Result.Failure<RoomiesData>( Status.NotFound, "Roomie not found." );
                return Result.Success( result );

            }
        }

        public async Task<Result<int>> CreatePasswordUser(string firstName, string lastName,  string email, DateTime birthDate, byte[] password, string phone )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@FirstName", firstName);
                p.Add( "@LastName", lastName );
                p.Add( "@Email", email );
                p.Add( "@BirthDate", birthDate);
                p.Add( "@Password", password );
                p.Add( "@Phone", phone );
                p.Add( "@RoomieId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sPasswordRoomieCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "An account with this email already exists." );

                Debug.Assert( status == 0 );
                return Result.Success( p.Get<int>( "@RoomieId" ) );
            }
        }

        public async Task CreateOrUpdateGoogleUser( string email, string googleId, string refreshToken )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "rm.sGoogleRoomieCreateOrUpdate",
                    new { Email = email, GoogleId = googleId, RefreshToken = refreshToken },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task<IEnumerable<string>> GetAuthenticationProviders( string roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<string>(
                    "select p.ProviderName from rm.vAuthenticationProvider p where p.RoomieId = @RoomieId",
                    new { RoomieId = roomieId } );
            }
        }

        public async Task Delete( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync( "rm.sRoomieDelete", new { RoomieId = roomieId }, commandType: CommandType.StoredProcedure );
            }
        }

        public async Task UpdateEmail( int roomieId, string email )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "rm.sRoomieUpdate",
                    new { RoomieId = roomieId, Email = email },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task UpdatePassword( int roomieId, byte[] password )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "rm.sPasswordRoomieUpdate",
                    new { RoomieId = roomieId, Password = password },
                    commandType: CommandType.StoredProcedure );
            }
        }

        public async Task<Result<int>> CreateUpdateRoomie( string firstName, string lastName, DateTime birthDate, string Phone, int userId )
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<int>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<int>( Status.BadRequest, "The last name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@FirstName", firstName );
                p.Add( "@LastName", lastName );
                p.Add( "@BirthDate", birthDate );
                p.Add( "@Phone", Phone ?? string.Empty );
                p.Add( "@RoomieId", userId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sRoomiesUpdateCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "A roomie with this name already exists." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, userId );
            }

        }
        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );

        public async Task<Result<RoomiesData>> FindById2( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                RoomiesData roomie = await con.QueryFirstOrDefaultAsync<RoomiesData>(
                      @"select s.RoomieId,
                             s.FirstName,
                             s.LastName,
                             s.BirthDate,
                             s.Phone,
                             s.Email
                      from rm.tRoomie s
                      where s.RoomieId = @RoomieId;",
                     new { RoomieId = roomieId } );

                if( roomie == null ) return Result.Failure<RoomiesData>( Status.NotFound, "Roomie not found." );
                return Result.Success( roomie );
            }
        }

        public async Task AddImageOfRoomie( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                await con.ExecuteAsync(
                    "rm.sRoomieImage",
                    new { RoomieId = roomieId },
                    commandType: CommandType.StoredProcedure );
            }

        }
    }
}
