using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ITI.Roomies.DAL
{
    public class TransactionGateway
    {
        readonly string _connectionString;

        public TransactionGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        #region TransacBudget
        public async Task<IEnumerable<TransacBudgetData>> GetAllTransacBudget( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<TransacBudgetData>(
                    @"select * from rm.tTransacBudget where RoomieId = @RoomieId;",
                    new { RoomieId = roomieId } );
            }
        }

        public async Task<Result<TransacBudgetData>> FindTBudgetById( int tBudgetId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                TransacBudgetData tBudget = await con.QueryFirstOrDefaultAsync<TransacBudgetData>(
                    @"select 
                             tb.TBudgetId,
                             tb.Price,
                             tb.Date,
                             tb.BudgetId,
                             tb.RoomieId
                     from rm.tTransacBudget tb
                     where tb.TBudgetId = @TBudgetId;",
                    new { TBudgetId = tBudgetId } );
                if( tBudget == null ) return Result.Failure<TransacBudgetData>( Status.NotFound, "Transaction not found" );
                return Result.Success( tBudget );
            }
        }

        public async Task<Result<int>> CreateTransacBudget( int price, DateTime date, int budgetId, int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@Price", price );
                p.Add( "@Date", date );
                p.Add( "@BudgetId", budgetId );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@TBudgetId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTransacBudgetCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@TBudgetId" ) );
            }
        }

        public Task<Result> UpdateTransacBudget( int transacBudget, int price, DateTime date, int budgetId )
        {
            //using( SqlConnection con = new SqlConnection( _connectionString ) )
            //{
            //    var p = new DynamicParameters();
            //    p.Add( "@");
            //    p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
            //    await con.ExecuteAsync( "rm.sBudgetUpdate", p, commandType: CommandType.StoredProcedure );

            //    int status = p.Get<int>( "@Status" );
            //    if( status == 1 ) return Result.Failure( Status.NotFound, "Budeget not found." );

            //    Debug.Assert( status == 0 );
            //    return Result.Success();
            //}
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteTransacBudget( int tBudgetId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TBudgetId", tBudgetId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTransacBudgetDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Transaction not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        #endregion

        #region TransacDepense
        public async Task<IEnumerable<TransacDepenseData>> GetAllTransacDepense( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<TransacDepenseData>(
                @"select * from rm.tTransacDepense where SRoomieId = @RoomieId",
                new { RoomieId = roomieId } );
            }
        }

        public Task<Result> UpdateTransacDepense( int transacDepense )
        {
            throw new NotImplementedException();
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

        public async Task<Result> UpdateTransacBudget( int tBudgetId, int price, DateTime date, int budgetId, int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TBudgetId", tBudgetId );
                p.Add( "@Price", price );
                p.Add( "@Date", date );
                p.Add( "@BudgetId", budgetId );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTransacBudgetUpdate", p, commandType: CommandType.StoredProcedure );
                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Transaction not found." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
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
#endregion
    }
}
