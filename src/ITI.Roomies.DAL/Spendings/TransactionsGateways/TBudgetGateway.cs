using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ITI.Roomies.DAL.Spendings.TransactionsGateways
{
    public class TBudgetGateway
    {
        readonly string _connectionString;

        public TBudgetGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TransacBudgetData>> GetAllTransacBudget( int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<TransacBudgetData>(
                    @"select * from rm.tTransacBudget where RoomieId = @RoomieId;",
                    new { RoomieId = roomieId } );
            }
        }
        public async Task<IEnumerable<TransacBudgetData>> GetDepensesOfBudget(int budgetId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<TransacBudgetData>(
                    @"select * from rm.tTransacBudget where BudgetId = @BudgetId;",
                    new { BudgetId = budgetId} );
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

        public async Task<BudgetData> GetActualBudget( int categoryId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                BudgetData budget = await con.QueryFirstOrDefaultAsync<BudgetData>(
                    @"select
                        BudgetId,
                        CategoryId,
                        Date2 = MAX(Date2),
                        Amount
                    from rm.tBudget
                    WHERE BudgetId <> 0 and CategoryId = @CategoryId
                    GROUP BY BudgetId, CategoryId, Date2, Amount
                    ORDER BY Date2 DESC;",
                new { CategoryId = categoryId } );
                return budget;
            }
        }

        public async Task<Result<int>> CreateTransacBudget( int price, DateTime date, int categoryId, int roomieId )
        {
            BudgetData budget = await this.GetActualBudget( categoryId );
            int budgetId = budget.BudgetId;

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


    }
}
