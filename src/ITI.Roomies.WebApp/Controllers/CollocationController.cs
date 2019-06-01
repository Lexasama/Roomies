using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.CollocModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System;

namespace ITI.Roomies.WebApp.Controllers
{

    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class CollocationController : Controller
    {
        readonly CollocGateway _collocGateway;
        readonly CollRoomGateway _collRoomGateway;

        public CollocationController( CollocGateway collocGateway, CollRoomGateway collRoomGateway)
        {
            _collocGateway = collocGateway;
            _collRoomGateway = collRoomGateway;
        }


        [HttpPost]
        public async Task<int> CreateColloc( [FromBody] CollocViewModel model )
        {
            int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );

            Result<int> result = await _collocGateway.CreateColloc( model.CollocName, roomieId );
          

            return  result.Content ;

        }

        [HttpDelete("quitColloc/{collocId}")]
        public async Task<IActionResult> LeaveCollocation(int collocId)
        {

            int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result result = await _collRoomGateway.LeaveCollocation(collocId, roomieId);
            return this.Ok( result );
        }

        // Renvoie les id des collocs dans lesquelles le roomie est présent
        [HttpGet( "getNameId" )]
        public async Task<IActionResult> GetCollocNameIdByRoomieIdAsync()
        {
            int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result <CollocData> result = await _collRoomGateway.FindCollocNameByRoomieId( userId );
            return this.CreateResult( result );
        }

        // Renvoie les ids, noms et prénoms des roomies d'une colloc
        [HttpGet( "getRoomieIdNames/{collocId}" )]
        public async Task<IActionResult> GetRoomiesIdNamesByCollocIdAsync( int collocId )
        {
            IEnumerable<RoomiesData> result = await _collRoomGateway.FindRoomiesIdNamesByCollocId( collocId );
            return this.Ok( result );
        }

        [HttpPost( "{email}/invite/{idColloc}" )]
        public async Task<int> Invite( string email ,int idColloc)
        {

            int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            string invitationKey = Guid.NewGuid().ToString();

            int receiverId = await _collocGateway.getRoomieIdByEmail(email);

            if(receiverId != 0)
            {
                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true;
                string emailFromAddress = "ITI.Roomies@gmail.com"; //Sender Email Address
                string password = "0123456789A@"; //Sender Password
                string body = "Voici le code d'invitation " + invitationKey;

                using( MailMessage mail = new MailMessage() )
                {
                    mail.From = new MailAddress( emailFromAddress );
                    mail.To.Add( email );
                    mail.Subject = "Invitation à une collocation";
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using( SmtpClient smtp = new SmtpClient( smtpAddress, portNumber ) )
                    {

                        smtp.Credentials = new NetworkCredential( emailFromAddress, password );
                        smtp.EnableSsl = enableSSL;
                        smtp.Send( mail );
                    }
                }
                await _collocGateway.Invitation(invitationKey,receiverId,roomieId,idColloc);


                return 1;
            }
            return 0;
        }

        [HttpPost( "join/{invitationKey}" )]
        public async Task<int> InviteAsync( string invitationKey )
        {
            int result = await _collocGateway.CheckInvitation( invitationKey );
            if (result != 0 )
            {
                int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
                await _collRoomGateway.AddCollRoom( result, roomieId );
                await _collocGateway.DeleteInvite(invitationKey);
                return result;
            }


            return result;
        }

        [HttpGet( "getCollocInformation/{collocId}" )]
        public async Task<IActionResult> getCollocInformation( int collocId )
        {
            IEnumerable<CollocData> result = await _collocGateway.getCollocInformation(collocId);
            return this.Ok( result );
        }
    }
}
