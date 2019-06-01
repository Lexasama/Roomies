using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pick_n_Trip.DAL.tPictures
{
    public class PicturesGateway
    {
        readonly string _connectionString;
        readonly string _path;
        readonly string _pathForDownload;
        readonly List<string> _listAuthorizeType;

        public PicturesGateway(string connectionString)
        {
            _connectionString = connectionString;
            _path = "../Pick-n-Trip.WebApp/wwwroot/Photos";
            _pathForDownload = "../Pick-n-Trip.WebApp/wwwroot";
            _listAuthorizeType =  GetTypeAuthorize();
            
        }

        public async Task<List<string>> UploadFiles(IFormFileCollection files, int groupID)
        {
            string path = _path + "/" + groupID;
            this.ExistDirectory(path);
            List<string> FilesWhoAreNotUpload = new List<string>() ;
            for (int i = 0; i < files.Count; i++)
            {
                IFormFile file = files[i];
                string IsAuthorize = _listAuthorizeType.Find(x => x == file.ContentType);
                if (IsAuthorize != null)
                {
                    string filePath = Path.Combine(path, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    FilesWhoAreNotUpload.Add(file.FileName+" n'a pas été upload à cause de son format inadapté");
                }
            }
            return FilesWhoAreNotUpload;
        }

        public async Task<List<string>> GetAllFiles(int idGroup)
        {
            List<string> listOfFilesPath = new List<string>();
            string path = _path + "/" + idGroup;
            this.ExistDirectory(path);
            string[] listOfFiles =  Directory.GetFiles(path);
            string basePathInWeb = "/Photos/" + idGroup;

            foreach (var item in listOfFiles)
            {
                string fileName = Path.GetFileName(item);
                string pathInWeb = basePathInWeb + "/" + fileName;

                listOfFilesPath.Add(pathInWeb);
                 
            }
            return listOfFilesPath;


        }

        public async Task<string> GetFirstFile(int groupId)
        {
            string path = _path + "/" + groupId;
            this.ExistDirectory(path);
            string[] listOfFiles = Directory.GetFiles(path);
            if (listOfFiles.Length > 0)
            {
                string basePathInWeb = "/Photos/" + groupId;
                string pathInWeb = basePathInWeb + "/" + Path.GetFileName(listOfFiles[0]);
                return pathInWeb;

            }
            else
            {
                return null;
            }


        }

        public async Task<Result<MemoryStream>> GetZip(int idGroup)
        {
            List<string> AllFiles = await GetAllFiles(idGroup);

            if (AllFiles.Count != 0)
            {
                MemoryStream zipStream = new MemoryStream();

                using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {

                    foreach (var item in AllFiles)
                    {
                        string fileName = Path.GetFileName(item);
                        ZipArchiveEntry entry = zip.CreateEntry(fileName);
                        string path = _pathForDownload + item;

                        using (var fileStream = new FileStream(path, FileMode.Open))
                        {
                            using (Stream entryStream = entry.Open())
                            {
                                await fileStream.CopyToAsync(entryStream);
                            }
                        }
                    }
                }
                return Result.Success(zipStream);
            }
            else
            {
                return Result.Failure<MemoryStream>(Status.NotFound, "Il n'y a pas de fichiers");
            }
        }

        public async Task<string> GetContentType(string filename)
        {
            string path = _pathForDownload + filename;

            string extension = Path.GetExtension(path);

            return extension;
        }

        public async Task<MemoryStream> DownloadFileAsync(string filename)
        {
            MemoryStream download = new MemoryStream();

            string path = _pathForDownload + filename;

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(download);
            }

            download.Position = 0;
            return download;
        }

        internal void ExistDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                var test = Directory.GetCurrentDirectory();
                Directory.CreateDirectory(path);
            }

        }

        internal List<string> GetTypeAuthorize()
        {
            using (StreamReader stream = new StreamReader("authorizeType.json"))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(stream))
                {
                    JToken json = JToken.Load(jsonReader);

                    IEnumerable<string> authorizeString =
                     from p in json["AuthorizeType"]
                     select (string)p;

                   List<string> List =  authorizeString.ToList();
                    

                    return List;

                }
            }

        }
    }
}
