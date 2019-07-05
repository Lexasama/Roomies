using System.IO;
using ITI.Roomies.DAL;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ITI.Roomies.WebApp.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;

namespace ITI.Roomies.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class ImageController : Controller
    {
        readonly ImageGateway _imageGateway;
        public ImageController( ImageGateway imageGateway )
        {
            _imageGateway = imageGateway;
        }

        [HttpPost( "uploadImage/{id}/{isRoomie}" )]
        public async Task<IActionResult> UploadImage( IFormCollection model, int id, bool isRoomie )
        {
            if( isRoomie )
            { 
                //id = int.Parse( model.ToList().Find( x => x.Key == "roomieId" ).Value.ToString() );
                id = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            }

            List<string> result = await _imageGateway.UploadImage( model.Files, id, isRoomie );
            if( result.Count == 0 )
            {
                return Ok();
            }
            return Ok(result);
            
        }

        [HttpPost( "DownloadImage" )]
        public async Task<IActionResult> DownloadImage( string imageName )
        {
            MemoryStream file = await _imageGateway.DownloadImage( imageName );
            string GetType = await _imageGateway.GetContentType( imageName );
            return File( file, "application/octet-stream", "profilePic.jpg" );
        }
    }
}
