using System.Net;
using System.Net.Mail;
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
        // readonly IEmailService _emailService;

        public RoomiesController( RoomiesGateway roomiesGateway )
        {
            _roomiesGateway = roomiesGateway;
        }


        [HttpGet( "getRoomie" )]
        public async Task<IActionResult> GetRoomieByEmail()
        {
            string email = HttpContext.User.FindFirst( c => c.Type == ClaimTypes.Email ).Value;
            Result<RoomiesData> result = await _roomiesGateway.getRoomieIdByEmail( email );
            return this.CreateResult( result );
        }


        [HttpGet( "{id}", Name = "checkRoomie" )]
        public async Task<IActionResult> GetRoomieById( int id )
        {
            Result<RoomiesData> result = await _roomiesGateway.FindById2( id );
            return this.CreateResult( result );
        }

        [HttpPost( "createRoomie" )]
        public async Task<IActionResult> CreateRoomie( [FromBody] RoomiesViewModel model )
        {
            int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result<int> result = await _roomiesGateway.CreateUpdateRoomie( model.FirstName, model.LastName, model.BirthDate, model.Phone, userId );
            return this.CreateResult( result, o =>
            {
                o.RouteName = "checkRoomie";
                o.RouteValues = id => new { id };
            } );
        }

        [HttpPost( "{email}/invite" )]
        public async Task<IActionResult> Invite( string email )
        {
            //EmailMessage message = new EmailMessage();
            //EmailAddress emailAddress = new EmailAddress();
            //emailAddress.Address = email ;
            //emailAddress.Name = "";
            //message.ToAddresses.Add( emailAddress);
            //message.FromAddresses.Add( emailAddress );
            //_emailService.Send( message );
            //return Ok( 0 );


            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;
            string emailFromAddress = "ITI.Roomies@gmail.com"; //Sender Email Address
            string password = "0123456789A@"; //Sender Password
            string subject = "Hello";
            string body = "Test";

            using( MailMessage mail = new MailMessage() )
            {
                mail.From = new MailAddress( emailFromAddress );
                mail.To.Add( email );
                mail.Subject = "Testing";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using( SmtpClient smtp = new SmtpClient( smtpAddress, portNumber ) )
                {

                    smtp.Credentials = new NetworkCredential( emailFromAddress, password );
                    smtp.EnableSsl = enableSSL;
                    smtp.Send( mail );
                }
            }

            return Ok( 0 );
        }
    }
}
