using System.Security.Claims;
using System.Threading.Tasks;
using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.RoomieModel;
using ITI.Roomies.WebApp.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Roomies.WebApp.Controllers
{
  

    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class RoomiesController : Controller
    {
        readonly RoomiesGateway _roomiesGateway;
        readonly IEmailService _emailService;

        public RoomiesController( RoomiesGateway roomiesGateway )
        {
            _roomiesGateway = roomiesGateway;
        }

        [HttpGet( "{id}", Name = "GetRoomie" )]
        public async Task<IActionResult> GetRoomieById( int id )
        {
            Result<RoomiesData> result = await _roomiesGateway.FindById( id );
            return this.CreateResult( result );
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoomie( [FromBody] RoomiesViewModel model )
        {
            int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result<int> result = await _roomiesGateway.CreateRoomie( model.FirstName, model.LastName, model.BirthDate, model.Phone, userId );
            return this.CreateResult( result, o =>
            {
                o.RouteName = "GetRoomie";
                o.RouteValues = id => new { id };
            } );
        }

        [HttpGet]
        async Task Invite( string email )
        {
            EmailMessage message = new EmailMessage();

            _emailService.Send( message );

        }
    }
}
