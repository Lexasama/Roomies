using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Roomies.WebApp.Controllers
{
    public class ImageController : Controller
    {
      

        public async Task<IActionResult> ImageUpload( IFormFile image)
        {
           
        }

        protected static string ConvertImageToBase64(string imgPath)
        {
            byte[] imgByte = System.IO.File.ReadAllBytes( imgPath );
            string imgBase64String = Convert.ToString( imgByte );
            return imgBase64String
        }
    }
}
