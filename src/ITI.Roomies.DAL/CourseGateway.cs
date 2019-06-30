using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ITI.Roomies.DAL
{
    public class CourseGateway
    {
        readonly string _connectionString;

        public CourseGateway( string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Result<CourseData>> FindById( int courseId )
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                CourseData course = await con.QueryFirstOrDefaultAsync<CourseData>(
                    @"select c.CourseId,
                             c.CourseName,
                             c.CourseDate,
                             c.CoursePrice,
                            c.CollocId
                        from rm.tCourse c
                        where c.CourseId = @CourseId;",
                    new { CourseId = courseId } );

                if( course == null ) return Result.Failure<CourseData>( Status.NotFound, "Course not found" );
                return Result.Success( course );
            }
        }
        //public async Task<Result<CourseTempData>> FindTempById ( int courseTempId)
        //{
        //    using( SqlConnection con = new SqlConnection( _connectionString ) )
        //    {
        //        CourseTempData courseTemp = await con.QueryFirstOrDefaultAsync<CourseTempData>(
        //            @"select ct.CourseTempId,
        //                     ct.CourseName,
        //                    ct.CoursePrice,
        //                    ct.CollocId
        //                from rm.tCourseTemp ct
        //                where ct.CourseTempId = @CourseTempId;",
        //            new { CourseTempId = courseTempId } );

        //        if( courseTemp == null ) return Result.Failure<CourseTempData>( Status.NotFound, "Course Template not found" );
        //        return Result.Success( courseTemp );
        //    }
        //}

        //public async Task<IEnumerable<CourseTempData>> GetAllTemplate( int collocId )
        //{
        //    using( SqlConnection con = new SqlConnection( _connectionString ) )
        //    {
        //        return await con.QueryAsync<CourseTempData>(
        //            @"select ct.CourseTempId,
        //                      ct.CourseName,
        //                      ct.CoursePrice,
        //                      ct.CollocId
        //                from rm.tCourseTemp ct
        //                 where ct.CollocId = @CollocId;",
        //            new { CollocId = collocId } );
        //    }
        //}

        public async Task<IEnumerable<CourseData>> GetAllLists( int collocId)
        {
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                return await con.QueryAsync<CourseData>(
                     @"select c.CourseId,
                              c.CourseName,
                              c.CourseDate,
                              c.CoursePrice,
                              c.CollocId
                        from rm.tCourse c
                         where c.CollocId = @CollocId;",
                    new { CollocId = collocId } );
            }
        }

        public async Task<Result<int>> CreateGroceryList( string courseName, DateTime courseDate, int collocId )
        {
            if( !IsNameValid( courseName ) ) return Result.Failure<int>( Status.BadRequest, "The Name is not valid." );

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                if( courseDate == default( DateTime ) )
                    p.Add( "@CourseDate", null );
                else
                    p.Add( "@CourseDate", courseDate );

                p.Add( "@CourseName", courseName);
                //p.Add( "@CourseDate", courseDate );
                p.Add( "@CollocId", collocId );
                p.Add( "@CourseId", dbType: DbType.Int32, direction: ParameterDirection.Output );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCourseCreate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "A Grocery List with this name already exists." );

                Debug.Assert( status == 0 );
                return Result.Success( Status.Created, p.Get<int>( "@CourseId" ) );
            }
        }

        //public async Task<Result<int>> CreateTempGroceryList( string courseName, int collocId )
        //{
        //    if( !IsNameValid( courseName ) ) return Result.Failure<int>( Status.BadRequest, "The Name is not valid." );

        //    using( SqlConnection con = new SqlConnection( _connectionString ) )
        //    {
        //        var p = new DynamicParameters();
        //        p.Add( "@CourseName", courseName );
        //        p.Add( "@CollocId", collocId );
        //        p.Add( "@CourseTempId", dbType: DbType.Int32, direction: ParameterDirection.Output );
        //        p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
        //        await con.ExecuteAsync( "rm.sCourseTempCreate", p, commandType: CommandType.StoredProcedure );

        //        int status = p.Get<int>( "@Status" );
        //        if( status == 1 ) return Result.Failure<int>( Status.BadRequest, "A Grocery List Template with this name already exists." );

        //        Debug.Assert( status == 0 );
        //        return Result.Success( Status.Created, p.Get<int>( "@CourseTempId" ) );

        //    }
        //}


        public async Task<Result> DeleteGroceryList( int courseId )
        {

            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {
                var p = new DynamicParameters();
                p.Add( "@CourseId", courseId );
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCourseDelete", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Grocery list  not found." );

                Debug.Assert( status == 0 );
                return Result.Success();
            }
        }

        //public async Task<Result> DeleteGroceryListTemp( int courseTempId )
        //{
        //    using( SqlConnection con = new SqlConnection( _connectionString ) )
        //    {
        //        var p = new DynamicParameters();
        //        p.Add( "@CourseTempId", courseTempId );
        //        p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
        //        await con.ExecuteAsync( "rm.sCourseTempDelete", p, commandType: CommandType.StoredProcedure );

        //        int status = p.Get<int>( "@Status" );
        //        if( status == 1 ) return Result.Failure( Status.NotFound, "Grocery list Template  not found." );

        //        Debug.Assert( status == 0 );
        //        return Result.Success();
        //    }
        //}


        public async Task<Result> UpdateGroceryList( int courseId, string courseName, DateTime courseDate)
        {
            if( !IsNameValid( courseName ) ) return Result.Failure( Status.BadRequest, "The course name is not valid." );
            using( SqlConnection con = new SqlConnection( _connectionString ) )
            {

                var p = new DynamicParameters();
                p.Add( "@CourseName", courseName );
                if( courseDate == default( DateTime ) )
                    p.Add( "@CourseDate", null );
                else
                    p.Add( "@CourseDate", courseDate );
                p.Add( "@CourseId", courseId, dbType: DbType.Int32);
                p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
                await con.ExecuteAsync( "rm.sCourseUpdate", p, commandType: CommandType.StoredProcedure );

                int status = p.Get<int>( "@Status" );
                if( status == 1 ) return Result.Failure( Status.NotFound, "Grocery List not found." );
               
                Debug.Assert( status == 0 );
                return Result.Success( Status.Ok );
            }
        }

        //public async Task<Result> UpdateGroceryListTemp( int courseTempId, string courseName)
        //{
        //    if( !IsNameValid( courseName ) ) return Result.Failure( Status.BadRequest, "The name is not valid." );
        //    using( SqlConnection con = new SqlConnection( _connectionString ) )
        //    {

        //        var p = new DynamicParameters();
        //        p.Add( "@CourseName", courseName );
        //        p.Add( "@CourseTempId", courseTempId, dbType: DbType.Int32 );
        //        p.Add( "@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue );
        //        await con.ExecuteAsync( "rm.sCourseTempUpdate", p, commandType: CommandType.StoredProcedure );

        //        int status = p.Get<int>( "@Status" );
        //        if( status == 1 ) return Result.Failure( Status.NotFound, "Grocery List Template not found." );

        //        Debug.Assert( status == 0 );
        //        return Result.Success( Status.Ok );
        //    }
        //}


        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
