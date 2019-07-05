using System.Diagnostics;
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

        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfileData()
        {
            int roomieId = int.Parse(HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier).Value);
            Result<RoomieProfileData> result  = await _roomiesGateway.GetProfile( roomieId );
            return this.CreateResult( result );
        }

        [HttpGet("getRoomiePic")]
        public async Task<IActionResult> getRoomiePic(int roomieId)
        {
             roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result<string> roomiePic = await _roomiesGateway.GetRoomiePicAsync( roomieId );

            return this.CreateResult( roomiePic );

        }

        [HttpGet("getCollocPic/{collocId}")]
        public async Task<IActionResult> getCollocPic(int collocId )
        {
            Result<string> collocPic = await _roomiesGateway.GetCollocPicAsync( collocId );
            return this.CreateResult( collocPic );
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
            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;
            string emailFromAddress = "ITI.Roomies@gmail.com"; //Sender Email Address
            string password = "0123456789A@"; //Sender Password
            string body = "Bienvenue sur Roomies";

            using( MailMessage mail = new MailMessage() )
            {


                //const string fileName = "invitationRoomie.html";
                ////Read HTML from file
                //var content = System.IO.File.ReadAllText( fileName );
                ////Replace all values in the HTML
                //content = content.Replace( "{MY_TITLE}", titleTextBox.Text );
                ////Write new HTML string to file
                //System.IO.File.WriteAllText( fileName, content );
                ////Show it in the default application for handling .html files
                //Process.Start( fileName );


                mail.From = new MailAddress( emailFromAddress );
                mail.To.Add( email );
                mail.Subject = "Bienvenue sur Roomie";
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
