using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.ItemModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Roomies.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class ItemController : Controller
    {

        readonly ItemGateway _itemGateway;

        public ItemController( ItemGateway itemGateway )
        {
            _itemGateway = itemGateway;
        }

        [HttpGet( "getItemList/{courseId}" )]
        public async Task<IActionResult> GetItemsFromList( int courseId )
        {
            IEnumerable<ItemData> result = await _itemGateway.GetAllItemFromList( courseId );
            return Ok( result );
        }

        [HttpGet( "getItemByItemId/{itemId}" )]
        public async Task<IActionResult> GetItemByItemIdAsync( int itemId )
        {
            Result<ItemData> result = await _itemGateway.FindById( itemId );
            return Ok( result );
        }

        [HttpPost("addItem")]
        public async Task<IActionResult> AddItem( [FromBody] ItemViewModel model )
        {

            int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result result = await _itemGateway.CreateItem( model.ItemPrice, model.ItemName,  model.CourseId, userId );
            return this.CreateResult( result );
        }

        [HttpDelete( "delete/{itemId}" )]
        public async Task<IActionResult> DeleteItem( int itemId )
        {
            Result result = await _itemGateway.Delete( itemId );
            return this.CreateResult( result );
        }

        [HttpPost( "updateItem" )]
        public async Task<IActionResult> updateItemAsync([FromBody] ItemViewModel model)
        {
            int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result result = await _itemGateway.UpdateItem( model.ItemId, model.ItemPrice, model.ItemName, model.CourseId, userId );
            return this.CreateResult( result );
        }
    }
}
