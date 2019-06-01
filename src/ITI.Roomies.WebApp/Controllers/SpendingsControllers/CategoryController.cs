using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Roomies.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class CategoryController : Controller
    {
        readonly CategoryGateway _categoryGateway;

        public CategoryController( CategoryGateway categoryGateway)
        {
            _categoryGateway = categoryGateway;
        }

       [HttpGet("GetCategories/{collocId}")]
       public async Task<IActionResult> GetCategories(int collocId)
       {
            IEnumerable<CategoryData> result = await _categoryGateway.GetAll( collocId );
            return Ok( result );
       }

        [HttpGet("GetCategory/{categoryId}", Name="GetCategory")]
        public async Task<IActionResult> GetGategory(int categoryId)
        {
            Result<CategoryData> result = await _categoryGateway.FindCategoryId( categoryId );
            return this.CreateResult( result );
        }

        [HttpGet( "getIcons" )]
        public async Task<IActionResult> GetIcons()
        {
            IEnumerable<CategoryIconsData> result = await _categoryGateway.GetIcons();
            return Ok( result );
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryViewModel model )
        {
            int collocId = model.CollocId;
            Result<int> result = await _categoryGateway.CreateCategory( model.CategoryName, model.IconName, collocId );
            return this.CreateResult( result, o =>
            {
                o.RouteName = "GetCategory";
                o.RouteValues = categoryId => new { categoryId };
            } );
        }

        [HttpPut( "{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryViewModel model)
        {
            Result result = await _categoryGateway.UpdateCategory( categoryId, model.CategoryName, model.IconName, model.CollocId );
            return this.CreateResult( result );
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId )
        {
            Result result = await _categoryGateway.DeleteCategory( categoryId );
            return this.CreateResult( result );
        }
    }
}
