using EmotionPlatzi.Web.Models;
using EmotionPlatzi.Web.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace EmotionPlatzi.Web.Controllers
{
    public class EmoUploaderAPIController : ApiController
    {
        private EmotionPlatziWebContext db = new EmotionPlatziWebContext();
        // POST: api/EmoPicturesAPI
        [ResponseType(typeof(EmoPicture))]
        public async Task<IHttpActionResult> PostEmoUploaderAsync(string fileName, Stream file)
        {
            string key = ConfigurationManager.AppSettings["KEY_EMOTION"];
            string serverFolderPath = ConfigurationManager.AppSettings["UPLOAD_DIR"];
            EmotionHelper emoHelper = new EmotionHelper(key);

            var pictureName = Guid.NewGuid().ToString();
            pictureName += Path.GetExtension(fileName);
            var route = System.Web.HttpContext.Current.Server.MapPath(serverFolderPath);
            route += $"/{pictureName}";

            FileStream fileStream = File.Create(route, (int)file.Length);
            byte[] bytesInStream = new byte[file.Length];
            file.Read(bytesInStream, 0, bytesInStream.Length);            
            fileStream.Write(bytesInStream, 0, bytesInStream.Length);

            var emoPicture = await emoHelper.DetectAndExtracFacesAsync(file);
            emoPicture.Name = Path.GetFileName(fileName);
            emoPicture.Path = $"{serverFolderPath}/{pictureName}";
            db.EmoPictures.Add(emoPicture);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = emoPicture.Id }, emoPicture);
        }
    }
}
