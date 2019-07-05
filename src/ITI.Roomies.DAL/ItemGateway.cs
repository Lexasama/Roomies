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

        public ItemGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public async Task<Result<ItemData>> FindById( int itemId )
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
        
        public async Task<Result<ItemData>> FindItemByIdInCourse( int itemId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                ItemData item = await con.QueryFirstOrDefaultAsync<ItemData>(
                    @"select i.itemId
                      from rm.tiItemCourse i
                      where i.ItemId = @ItemId;",
                    new { ItemId = itemId } );

                if( item == null ) return Result.Failure<ItemData>( Status.NotFound, "Item not found." );
                return Result.Success( item );
            }
        }

        //public async Task<Result<RItemData>> FindRItemById( int rItemId )
        //{
        //    using( SqlConnection con = new SqlConnection( _connectionString ) )
        //    {
        //        RItemData rItem = await con.QueryFirstOrDefaultAsync<RItemData>(
        //            @"select ri.CollocId,
        //                     ri.ItemPrice,
        //                     ri.ItemName,
        //                     ri.CourseTempId,
        //                     ri.CourseTempId
        //              from rm.tRItem ri
        //              where ri.ItemId = @RItemId;",
        //            new { RItemId = rItemId } );
        //        if( rItem == null ) return Result.Failure<RItemData>( Status.NotFound, "Item not found." );
        //        return Result.Success( rItem );
        //    }
        //}

        //public async Task<IEnumerable<ItemData>> GetAllItemFromList( int courseId )
        //{
        //    using( SqlConnection con = new SqlConnection( _connectionString ) )
        //    {
        //        return await con.QueryAsync<ItemData>(
        //            @"select i.ItemId,
        //                     i.ItemPrice,
        //                     i.ItemName,
        //                     i.RoomieId,
        //                     i.itemBought
        //               from rm.tItem i
        //               where i.CourseId = @CourseId",
        //            new {CourseId = courseId} );
        //    }
        //}
        public async Task<IEnumerable<ItemCourseData>> GetAllItemFromList( int courseId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<ItemCourseData>(
                    @"select i.ItemId,
								i.ItemName,
                                i.ItemSaved,
                                ic.CourseId,
								ic.ItemQuantite,
								ic.ItemBought,
								case when r.RoomieId = 0 then 0
								else r.RoomieId
								end as RoomieId,
								case when r.RoomieId = 0 then 'N/A'
								else r.FirstName
								end as FirstName
						  from rm.tItem i
							inner join rm.tiItemCourse ic on i.ItemId = ic.ItemId
							inner join rm.tRoomie r on ic.RoomieId = r.RoomieId
						where ic.CourseId = @CourseId;",
                    new { CourseId = courseId } );
            }
        }

        public async Task<IEnumerable<ItemData>> GetAllSavedItemFromColloc( int collocId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                //return await con.QueryAsync<ItemData>(
                //    @"select i.ItemId,
                //             i.ItemPrice,
                //             i.ItemName
                //       from rm.tItem i
                //       where i.CollocId = @CollocId
                //       and i.ItemSaved = 1
                //       order by i.ItemName",
                //    new {CollocId = collocId} );
                return await con.QueryAsync<ItemData>(
                    @"select i.ItemId,
                             i.ItemPrice,
                             i.ItemName
                       from rm.tItem i
                       where i.CollocId = @CollocId
                       and i.ItemSaved = 1
                       order by i.ItemName",
                    new { CollocId = collocId } );
            }
        }

        //public async Task<IEnumerable<RItemData>> GetAllRItemFromTemplate(int courseTempId)
        //{
        //    using (SqlConnection con = new SqlConnection( _connectionString))
        //    {
        //        return await con.QueryAsync<RItemData>(
        //            @"select ri.RItemId,
        //                     ri.ItemPrice,
        //                     ri.ItemName,
        //                from rm.tRItem ri
        //                where ri.CourseTempId = @CourseTempId",
        //            new { CourseTempId = courseTempId } );
        //    }
        //}

        //public async Task<Result> CreateRItem( int itemPrice, string  itemName, int courseTempId, int collocId)
        //{
        //    if(!IsNameValid(itemName)) return Result.Failure<int>( Status.BadRequest, "The item Name is not valid." );
        //    using( SqlConnection con = new SqlConnection ( _connectionString ) )
        //    {
        //        var p = new DynamicParameters();
        //        p.Add( "@RItemPrice", itemPrice);
        //        p.Add( "@RItemName", itemName );
        //        p.Add( "@CourseId", courseTempId);
        //        p.Add( "@CollocId", collocId );
        //        p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
        //        await con.ExecuteAsync( "rm.sRItemCreate", p, commandType: CommandType.StoredProcedure );

        //        int status = p.Get<int>( "@Status" );
        //        Debug.Assert( status == 0 );
        //        return Result.Success( status );
        //    }
        //}

        public async Task<Result<int>> CreateItem( int itemPrice, string itemName, int collocId, bool ItemSaved )
        {
            if( !IsNameValid( itemName ) ) return Result.Failure<int>( Status.BadRequest, "The item name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@ItemPrice", itemPrice );
                p.Add( "@ItemName", itemName );
                p.Add( "@CollocId", collocId );
                p.Add( "@ItemSaved", ItemSaved );
                p.Add( "@ItemId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sItemCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok, p.Get<int>( "@ItemId" ) );

            }
        }
                public async Task<Result> CreateItemInList( int itemId, int courseId, int roomieId, string itemQuantite )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@ItemId", itemId );
                p.Add( "@CourseId", courseId );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@ItemQuantite", itemQuantite );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sItemCourseCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                Debug.Assert( status == 0 );
                return Result.Success( status );
            }
        }

        //public async Task<Result> DeleteRItem( int rItemId )
        //{
        //    using( SqlConnection con = new SqlConnection( _connectionString ) )
        //    {
        //        var p = new DynamicParameters();
        //        p.Add( "@RItemId", rItemId );
        //        p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
        //        await con.ExecuteAsync( "rm.sRItemDelete", p, commandType: CommandType.StoredProcedure );

        //        int status = p.Get<int>( "@Status" );
        //        if( status == 1 ) return Result.Failure( Status.NotFound, "Item not found." );

        //        Debug.Assert( status == 0 );
        //        return Result.Success();
        //    }
        //}

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

        public async Task<Result> DeleteItemFromCourse( int itemId, int courseId, int roomieId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@ItemId", itemId );
                p.Add( "@CourseId", courseId );
                p.Add( "@RoomieId", roomieId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sDeleteItemFromCourse", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Item not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> Update( int itemId, int itemPrice, string itemName, int courseId, int roomieId )
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


        public async Task<Result> UpdateTaskState( int itemId, bool itemSaved )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@ItemId", itemId );
                p.Add( "@ItemSaved", itemSaved );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sItemUpdateState", p, commandType: CommandType.StoredProcedure );
                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Item not found." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
            }
        }

        //public async Task<Result> UpdateRItem( int itemId, int itemPrice, string itemName, int CourseTempId, int collocId )
        //{
        //    if( !IsNameValid( itemName ) ) return Result.Failure( Status.BadRequest, "The item name is not valid." );

        //    using( SqlConnection con = new SqlConnection( _connectionString ) )
        //    {
        //        var p = new DynamicParameters();
        //        p.Add( "@RItemId", itemId );
        //        p.Add( "@ItemPrice", itemPrice );
        //        p.Add( "@ItemName", itemName );
        //        p.Add( "@CourseId", CourseTempId );
        //        p.Add( "@CollocId", collocId );
        //        p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
        //        await con.ExecuteAsync( "rm.sRItemUpdate", p, commandType: CommandType.StoredProcedure );
        //        int status = p.Get<int>( "@Status" );
        //        if( status == 1 ) return Result.Failure( Status.NotFound, "Item not found." );
        //        Debug.Assert( status == 0 );
        //        return Result.Success( Status.Ok );
        //    }
        //}

        //public async Task<Result> UpdateItemBought( int itemId, bool itemBought )
        //{
        //    using( SqlConnection con = new SqlConnection( _connectionString ) )
        //    {
        //        var p = new DynamicParameters();
        //        p.Add( "@ItemId", itemId );
        //        p.Add( "@ItemBought", itemBought );
        //        p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
        //        await con.ExecuteAsync( "rm.sUpdateItemBought", p, commandType: CommandType.StoredProcedure );
        //        int status = p.Get<int>( "@Status" );
        //        if( status == 1 ) return Result.Failure( Status.NotFound, "Item not found." );
        //        Debug.Assert( status == 0 );
        //        return Result.Success( Status.Ok );
        //    }
        //}

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
