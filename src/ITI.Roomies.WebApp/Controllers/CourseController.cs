using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.CourseTempViewModels;
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

        [HttpGet( "getList/{id}" )]
        public async Task<IActionResult> GetLists( int id )
        {
            IEnumerable<CourseData> result = await _courseGateway.GetAll( id );
            return Ok( result );
        }

        [HttpGet( "getTemplates/{id}" )]
        public async Task<IActionResult> GetTemplates( int id )
        {
            IEnumerable<CourseTempData> result = await _courseGateway.GetAllTemp( id );
            return Ok( result );
        }

        [HttpGet( "GetAGroceryList/{courseId}", Name = "GetAGroceryList" )]
        public async Task<IActionResult> GetGroceryListById( int courseId )
        {
            Result<CourseData> result = await _courseGateway.FindById( courseId );
            return this.CreateResult( result );
        }

        [HttpGet( "GetATemplate/{courseId}", Name = "GetATemplate" )]
        public async Task<IActionResult> GetATemplateById( int courseId )
        {
            Result<CourseTempData> result = await _courseGateway.FindTempById( courseId );
            return this.CreateResult( result );
        }

        [HttpPost( "CreateGroceryTemp")]
        public async Task<IActionResult> CreateTemplate ( [FromBody] CourseTempViewModel model)
        {
            Result<int> result = await _courseGateway.CreateTempGroceryList( model.CourseName, model.CollocId );
            return this.CreateResult( result );
        }

        [HttpPost( "createGroceryList" )]
        public async Task<IActionResult> CreateGroceryList( [FromBody] CourseViewModel model )
        {
            Result<int> result = await _courseGateway.CreateGroceryList( model.CourseName, model.CourseDate, model.CollocId );
            return this.CreateResult( result, o =>
            {
                o.RouteName = "GetAGroceryList";
                o.RouteValues = courseId => new { courseId };
            } );
        }

        [HttpPost("CreateTemplate")]
        public async Task<IActionResult> CreateTemplate( [FromBody] CourseTempData model)
        {
            Result<int> result = await _courseGateway.CreateTempGroceryList( model.CourseName, model.CollocId);
            return this.CreateResult( result, o =>
            {
                o.RouteName = "GetATemplate";
                o.RouteValues = courseId => new { courseId };
            } );
        }

        [HttpPut( "updateGroceryList" )]
        public async Task<IActionResult> UpdateAGroceryList( [FromBody] CourseViewModel model )
        {
            Result result = await _courseGateway.UpdateGroceryList( model.CourseId, model.CourseName, model.CourseDate );
            return this.CreateResult( result );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteAgroceryList( int courseId )
        {
            Result result = await _courseGateway.DeleteGroceryList( courseId );
            return this.CreateResult( result );
        }
    }
}
