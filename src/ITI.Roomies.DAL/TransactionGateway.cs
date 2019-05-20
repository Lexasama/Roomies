using Dapper;
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

        public async Task<Result<TransactionData>> FindById(int transacId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                TransactionData transac = await con.QueryFirstOrDefaultAsync<TransactionData>(
                    @"select t.TransacId,
                             t.TransacDesc,
                             t.TransacPrice,
                             t.TransacDate,
                             t.CollocId,
                             t.RoomieId
                     from rm.tTransaction t
                     where t.TransacId = @TransacId;",
                    new { TransacId = transacId } );
                if( transac == null ) return Result.Failure<TransactionData>( Status.NotFound, "Transaction not found" );
                return Result.Success( transac );
            }
        }

        public async Task<Result<int>> CreateTransac (string transacDesc, int transacPrice, int collocId, int roomieId)
        {
            if( !IsNameValid( transacDesc ) ) return Result.Failure<int>( Status.BadRequest, "The description is not valid" );
            
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TransacDesc", transacDesc );
                p.Add( "@TransacPrice", transacPrice );
                p.Add( "@CollocId", collocId  );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@TransacId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTransacCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@TransacId" ) );
            }
        }

        public async Task<Result> Delete( int transacId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TransacId", transacId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTransacDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Transaction not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> Update( int transacId, string transacDesc, int transacPrice, int collocId, int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@TransacId", transacId );
                p.Add( "@TransacDesc", transacDesc );
                p.Add( "@TransacPrice", transacPrice );
                p.Add( "@CollocId", collocId );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sTransacUpdate", p, commandType: CommandType.StoredProcedure );
                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Transaction not found." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
            }
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
