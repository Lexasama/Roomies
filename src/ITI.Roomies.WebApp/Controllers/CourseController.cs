using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.CourseViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Roomies.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class CourseController : Controller
    {
        readonly CourseGateway _courseGateway;

        public CourseController( CourseGateway courseGateway )
        {
            _courseGateway = courseGateway;
        }

        [HttpGet( "getLists/{collocId}" )]
        public async Task<IActionResult> GetAllLists( int collocId )
        {
            IEnumerable<CourseData> result = await _courseGateway.GetAllLists( collocId );
            return Ok( result );
        }

        //[HttpGet( "getTemplates/{collocId}" )]
        //public async Task<IActionResult> GetTemplates( int collocId )
        //{
        //    IEnumerable<CourseTempData> result = await _courseGateway.GetAllTemplate( collocId );
        //    return Ok( result );
        //}

        [HttpGet( "GetList/{courseId}", Name = "GetGroceryList" )]
        public async Task<IActionResult> GetGroceryListById( int courseId )
        {
            Result<CourseData> result = await _courseGateway.FindById( courseId );
            return this.CreateResult( result );
        }

        //[HttpGet( "GetTemplate/{courseId}", Name = "GetTemplate" )]
        //public async Task<IActionResult> GetATemplateById( int courseTempId )
        //{
        //    Result<CourseTempData> result = await _courseGateway.FindTempById( courseTempId );
        //    return this.CreateResult( result );
        //}

        [HttpPost( "create" )]
        public async Task<IActionResult> CreateGroceryList( [FromBody] CourseViewModel model )
        {

            //if( model.IsTemplate)
            //{
            //    Result<int> result = await _courseGateway.CreateTempGroceryList( model.CourseName, model.CollocId );
            //    return this.CreateResult( result, o =>
            //    {
            //        o.RouteName = "GetTemplate";
            //        o.RouteValues = courseId => new { courseId };
            //    } );
            //}
            //else
            //{ 
                Result<int> result = await _courseGateway.CreateGroceryList( model.CourseName, model.CourseDate, model.CollocId );
                return this.CreateResult( result, o =>
                {
                    o.RouteName = "GetGroceryList";
                    o.RouteValues = courseId => new { courseId };
                } );
            //}
        }


        [HttpPut( "update" )]
        public async Task<IActionResult> UpdateAGroceryList( [FromBody] CourseViewModel model )
        {
            //if (model.IsTemplate)
            //{
            //    Result result = await _courseGateway.UpdateGroceryListTemp( model.CourseId, model.CourseName );
            //    return this.CreateResult( result );
            //}
            //else
            //{
                Result result = await _courseGateway.UpdateGroceryList( model.CourseId, model.CourseName, model.CourseDate );
                return this.CreateResult( result );
            //}
            
        }

        [HttpDelete( "{courseId}" )]
        public async Task<IActionResult> DeleteAgroceryList( int courseId )
        {
            Result result = await _courseGateway.DeleteGroceryList( courseId );
            return this.CreateResult( result );
        }

        //[HttpDelete( "deleteTemplate/{courseId}" )]
        //public async Task<IActionResult> DeleteTemplate( int courseId)
        //{
        //    Result result = await _courseGateway.DeleteGroceryListTemp( courseId );
        //    return this.CreateResult( result );
        //}
    }
}
