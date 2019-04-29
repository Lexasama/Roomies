using System.Security.Claims;
using System.Threading.Tasks;
using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.CollocModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Roomies.WebApp.Controllers
{

    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class CollocationController : Controller
    {
        readonly CollocGateway _collocGateway;
        readonly CollRoomGateway _collRoomGateway;

        public CollocationController( CollocGateway collocGateway, CollRoomGateway collroomGateway )
        {
            _collocGateway = collocGateway;
            _collRoomGateway = collroomGateway;
        }


        [HttpPost]
        public async Task<int> CreateColloc( [FromBody] CollocViewModel model )
        {
            Result<int> result = await _collocGateway.CreateColloc( model.CollocName );
            int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result<int> result2 = await _collRoomGateway.AddCollRoom( result.Content, userId );

            return  result.Content ;

        }

        // Renvoie l'id de la première colloc dans laquelle le roomie est présent
        //[HttpGet]
        //public async Task<IActionResult> GetCollocIdByRoomieIdAsync( int id )
        //{
        //    int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
        //    Result<int> result = await _collRoomGateway.FindCollocByRoomieId( userId );
        //    return this.CreateResult( result );
        //}

        // Renvoie les id des collocs dans lesquelles le roomie est présent
        [HttpGet( "getNameId" )]
        public async Task<IActionResult> GetCollocNameIdByRoomieIdAsync( int id )
        {
            int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result <CollocData> result = await _collRoomGateway.FindCollocNameByRoomieId( userId );
            return this.CreateResult( result );
        }

    }
}