using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ITI.Roomies.DAL.Spendings.TransactionsGateways
{
    public class TDepenseGateway
    {
        readonly string _connectionString;

        public TDepenseGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TransacDepenseData>> GetAllTransacDepense( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<TransacDepenseData>(
                @"select * from rm.tTransacDepense where SRoomieId = @RoomieId",
                new { RoomieId = roomieId } );
            }
        }

        public async Task<Result<TransacDepenseData>> FindTDepenseById( int tDepenseId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                TransacDepenseData tBudget = await con.QueryFirstOrDefaultAsync<TransacDepenseData>(
                    @"select 
                                     td.TDepenseId,
                                     td.Price,
                                     td.Date,
                                     td.SRoomieId,
                                     td.RRoomieId
                             from rm.tTransacDepense td
                             where td.TDepenseId = @TDepenseId;",
                    new { TDepenseId = tDepenseId } );
                if( tBudget == null ) return Result.Failure<TransacDepenseData>( Status.NotFound, "Transaction not found" );
                return Result.Success( tBudget );
            }
        }

        public async Task<Result<int>> CreateTransacDepense( int price, DateTime date, int sRoomieId, int rRoomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( @"Price", price );
                p.Add( "@Date", date );
                p.Add( "@SRoomieId", sRoomieId );
                p.Add( "@RRoomieId", rRoomieId );
                p.Add( "@TDepenseId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTransacDepenseCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@TDepenseId" ) );
            }
        }
        public async Task<Result> DeleteTransacDepense( int tDepenseId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TDepenseId", tDepenseId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTransacDepenseDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Transaction not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> UpdateTransacDepense( int tDepenseId, int price, DateTime date, int sRoomieId, int rRoomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TDepenseId", tDepenseId );
                p.Add( "@Price", price );
                p.Add( "@Date", date );
                p.Add( "@SRoomieId", sRoomieId );
                p.Add( "@RRoomieId", rRoomieId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTransacDepenseUpdate", p, commandType: CommandType.StoredProcedure );
                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Transaction not found." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
            }

        }
    }
}
