using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;

namespace ITI.Roomies.DAL
{
    public class ItemGateway
    { 
        readonly string _connectionString;

        public ItemGateway( string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Result<ItemData>> FindById( int itemId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                ItemData item = await con.QueryFirstOrDefaultAsync<ItemData>(
                    @"select i.RoomieId,
                             i.ItemPrice,
                             i.ItemName,
                             i.CourseId,
                             i.RoomieId
                      from rm.tItem i
                      where i.ItemId = @ItemId;",
                    new { ItemId = itemId } );

                if( item == null ) return Result.Failure<ItemData>( Status.NotFound, "Item not found." );
                return Result.Success( item );
            }
        }

        public async Task<Result<RItemData>> FindRItemById( int rItemId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                RItemData rItem = await con.QueryFirstOrDefaultAsync<RItemData>(
                    @"select i.RoomieId,
                             i.ItemPrice,
                             i.ItemName,
                             i.CourseId,
                             i.RoomieId
                      from rm.tItem i
                      where i.ItemId = @ItemId;",
                    new { ItemId = rItemId } );
                if( rItem == null ) return Result.Failure<RItemData>( Status.NotFound, "Item not found." );
                return Result.Success( rItem );
            }
        }
        public async Task<IEnumerable<ItemData>> GetAllItemFromList( int courseId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<ItemData>(
                    @"select i.ItemId,
                             i.ItemPrice,
                             i.ItemName,
                             i.RoomieId
                       from rm.tItem i
                       where i.CourseId = @CourseId",
                    new {CourseId = courseId} );
            }
        }

        public async Task<Result> CreateItem( int itemPrice, string itemName, int courseId, int roomieId )
        {
            if( !IsNameValid( itemName ) ) return Result.Failure<int>( Status.BadRequest, "The item name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@ItemPrice", itemPrice );
                p.Add( "@ItemName", itemName );
                p.Add( "@CourseId", courseId );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sItemCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                Debug.Assert( status == 0 );
                return Result.Success( status);
            }
        }

        public async Task<Result> Delete( int itemId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@ItemId", itemId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sItemDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Item not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> UpdateItem(int itemId, int itemPrice, string itemName, int courseId,  int roomieId)
        {
            if( !IsNameValid( itemName ) ) return Result.Failure( Status.BadRequest, "The item name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@ItemId", itemId );
                p.Add( "@ItemPrice", itemPrice );
                p.Add( "@ItemName", itemName );
                p.Add( "@CourseId", courseId );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sItemUpdate", p, commandType: CommandType.StoredProcedure );
                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Item not found." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
            }
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
