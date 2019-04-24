using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Threading.Tasks;
using System.Security.Claims;


namespace ITI.Roomies.WebApp.Controllers
{
    public class ImageController : Controller
    {
        readonly ImageGateway _imageGateway;

        [HttpPost]
        [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
        public async Task<IActionResult> ImageUpload( IFormFile image)
        {
            //image.GetType();
            long size = image.Length;
           
            //string userId = User.FindFirst(  ClaimTypes.NameIdentifier).Value;
            int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            
                       
            string imgPath = @"..\Roomies\src\ITI.Roomies.DB\Pics\"+userId;
            if ( size > 0 )
            {
                using( var stream = new FileStream( imgPath, FileMode.Create ) )
                {
                    await image.CopyToAsync( stream );
                }
            }

            await _imageGateway.AddImageOfRoomie( userId );

            return Ok( new {size, imgPath });
        }
    }
}
