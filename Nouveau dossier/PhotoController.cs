using IO.Swagger.Model;
using Microsoft.AspNetCore.Mvc;
using Pick_n_Trip.DAL;
using Pick_n_Trip.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pick_n_Trip.DAL.tPictures;
using Pick_n_Trip.WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Pick_n_Trip.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class PhotoController : Controller
    {
        readonly PicturesGateway _picturesGateway;

        public PhotoController(PicturesGateway picturesGateway)
        {
            _picturesGateway = picturesGateway;
            
        }

        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadImage(IFormCollection model)
        {
            int idGroup = int.Parse(model.ToList().Find(x => x.Key == "idGroup").Value.ToString());
            List<string> result = await _picturesGateway.UploadFiles(model.Files,idGroup);
            if(result.Count == 0)
            {
                return Ok();
            }
            return Ok(result);
        }

        [HttpGet("AllPictures")]
        public async Task<IActionResult> AllPictures(int idGroup)
        {
            List<string> result = await _picturesGateway.GetAllFiles(idGroup);
            return Ok(result);
        }

        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            MemoryStream file = await _picturesGateway.DownloadFileAsync(filename);
            string GetType = await _picturesGateway.GetContentType(filename);
            return File(file, "application/octet-stream", "e5u1qyO.jpg");
        }

        [HttpGet("DownloadAllFiles")]
        public async Task<IActionResult> DownloadAllFiles(int idGroup)
        {
            Result<MemoryStream> file = await _picturesGateway.GetZip(idGroup);
            if (file.HasError)
            {
                return this.CreateResult(file);
            }
            return File(file.Content.ToArray(), "application/zip", "Photos.zip");
        }


    }
}
