using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;

namespace ITI.Roomies.DAL
{
    public class CategoryGateway
    {
        readonly string _connectionString;
        public CategoryGateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<CategoryData>> GetAll( int collocId )
        {

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<CategoryData>(
                    @"select * from rm.tCategory where CollocId = @CollocId;",
                    new { CollocId = collocId } );
            }
        }

        public async Task<Result<CategoryData>> FindCategoryId( int categoryId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                CategoryData category = await con.QueryFirstOrDefaultAsync<CategoryData>(
                    @"select * from rm.vCategory
                        where CategoryId = @CategoryId;",
                    new { CategoryId = categoryId } );

                if( category == null ) return Result.Failure<CategoryData>( Status.NotFound, "Category not found." );
                return Result.Success( category );
            }

        }

        public async Task<Result<int>> CreateCategory( string categoryName,  string icon, int collocId )
        {
            if( !IsNameValid( categoryName ) ) return Result.Failure<int>( Status.BadRequest, "The category name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CategoryName", categoryName);
                p.Add( "@IconName", icon);
                p.Add( "@CollocId", collocId);
                p.Add( "@CategoryId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCategoryCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "A Category with this name already exists." );

                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@CategoryId" ) );
            }
        }

        public async Task<IEnumerable<CategoryIconsData>> GetIcons()
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<CategoryIconsData>(
                    @"select IconName from rm.tIcon" );
            }
        }

        public async Task<Result> DeleteCategory( int categoryId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CategoryId", categoryId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCategoryDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Category not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        public async Task<Result> UpdateCategory( int categoryId, string categoryName, string icon, int collocId)
        {
            if( !IsNameValid( categoryName ) ) return Result.Failure( Status.BadRequest, "The category name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CategoryId", categoryId );
                p.Add( "@CategoryName", categoryName );
                p.Add( "@Icon", icon);
                p.Add( "@CollocId", collocId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCategoryUpdate", p, commandType: CommandType.StoredProcedure );
                int status = p.Get<int>( "@Status" );

                if( status == 1 ) return Result.Failure( Status.NotFound, "Category not found." );
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
            }
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );

    }
}

