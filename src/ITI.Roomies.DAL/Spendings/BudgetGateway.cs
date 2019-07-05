using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;
using ITI.Roomies.DAL.Spendings;


namespace ITI.Roomies.DAL
{
    public class BudgetGateway
    {
        readonly string _connectionString;


        public BudgetGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public async Task<Result<BudgetData>> FindBudgetById( int budgetId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                BudgetData budget = await con.QueryFirstOrDefaultAsync<BudgetData>(
                    @"select * from rm.vBudget where BudgetId = @BudgetId;",
                    new { BudgetId = budgetId } );
                if( budget == null ) return Result.Failure<BudgetData>( Status.NotFound, "Budget not found." );
                return Result.Success( budget );
            }
        }

        public async Task<IEnumerable<BudgetCatData>> GetChartDataByTime( int collocId, DateTime date3 )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<BudgetCatData>(
                    @"select * from rm.vCategoryBudget where CollocId = @CollocId and Date1 <= @date3 and @Date3<= Date2 ",
                    new { CollocId = collocId, date3 = date3 } );
            }
        }

        public async Task<BudgetData> GetCurrentBudget( int categoryId )
        {
           using(SqlConnection con = new SqlConnection( _connectionString ) )
           {
                return  await con.QueryFirstOrDefaultAsync<BudgetData>(
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
           }
        }

        public async Task<IEnumerable<BudgetCatData>> GetAllChartDataByCollocId( int collocId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<BudgetCatData>(
                    @"select * from rm.vAllCategoryBudget where CollocId = @CollocId",
                    new { CollocId = collocId } );
            }
        }

        public async Task<IEnumerable<BudgetCatData>> GetDailyBudget( int collocId, DateTime day )
        {
            //IEnumerable<BudgetCatData> allBudget = await this.GetAllChartDataByCollocId( collocId );
            List<BudgetCatData> dailyBudget = new List<BudgetCatData>();
            IEnumerable<BudgetCatData> allBudget = await this.GetChartDataByTime( collocId, day );

            foreach( BudgetCatData data in allBudget )
            {
                //if( data.Date1 < day && data.Date2 < day )
                {
                    if( data.Amount > 0 )
                        data.Amount = data.Amount / (int)((data.Date2 - data.Date1).TotalDays);
                    dailyBudget.Add( data );
                }
            }
            return dailyBudget;
        }

        public async Task<IEnumerable<BudgetData>> GetAll( int collocId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<BudgetData>(
                    @"select * from rm.tBudget where CollocId = @CollocId",
                    new { CollocId = collocId } );
            }
        }

        public async Task<IEnumerable<BudgetData>> GetAllBudgetOfCategory( int categoryId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<BudgetData>(
                    @"select * from rm.vCategoryBudget where CategoryId = @CategoryId",
                    new { CategoryId = categoryId } );

            }
        }

        public Task<IEnumerable<BudgetCatData>> GetMonthlyBudget( int collocId, DateTime month )
        {
            throw new NotImplementedException();
        }

        public async Task<Result<int>> CreateBudget( int categoryId, DateTime date1, DateTime date2, int amount )
        {
           
                    using( SqlConnection con = new SqlConnection( _connectionString ) )
                    {
                        var p = new DynamicParameters();
                        p.Add( "@CategoryId", categoryId );
                        p.Add( "@Date1", date1 );
                        p.Add( "@Date2", date2 );
                        p.Add( "@Amount", amount );
                        p.Add( "@BudgetId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                        p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                        await con.ExecuteAsync( "rm.sBudgetCreate", p, commandType: CommandType.StoredProcedure );

                        int status = p.Get<int>( "@Status" );
                        Debug.Assert( status == 0 );
                        return Result.Success( Status.Created, p.Get<int>( "@BudgetId" ) );
                    }
           
        }

        public async Task<BudgetData> FindBudgetByCollocIdAndDate( int collocId, DateTime date )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryFirstOrDefaultAsync<BudgetData>(
                    @"select * from rm.tBudget where CollocId = @CollocId and Date1 < @Date and @Date < Date2",
                    new { CollocId = collocId, Date = date } );
            }
        }

        public async Task<Result> DeleteBudget( int budgetId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@BudgetId", budgetId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sBudgetDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Budget not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> UpdateBudget( int budgetId, int categoryId, DateTime date1, DateTime date2, int amount, int collocId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@BudgetId", budgetId );
                p.Add( "@CategoryId", categoryId );
                p.Add( "@Date1", date1 );
                p.Add( "@Date2", date2 );
                p.Add( "@Amount", amount );
                p.Add( "@CollocId", collocId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sBudgetUpdate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Budget not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result<List<DateTime>>> GetOffDates( int categoryId)
        {
            IEnumerable<BudgetData> budgetsOfCategory = await this.GetAllBudgetOfCategory(  categoryId );
            var dates = new List<DateTime>();
            foreach( BudgetData budget in budgetsOfCategory)
            {
                for( var dt = budget.Date1; dt <= budget.Date2; dt = dt.AddDays( 1 ) )
                {
                    dates.Add( dt );
                }
            }
            return Result.Success(dates);
        }
    }
}
