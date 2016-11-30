using EmotionPlatzi.Web.Models;
using EmotionPlatzi.Web.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmotionPlatzi.Web.Controllers
{
    public class EmoUploaderController : Controller
    {
        string serverFolderPath;
        EmotionHelper emoHelper;
        string key;
        EmotionPlatziWebContext db = new EmotionPlatziWebContext();
        public EmoUploaderController()
        {
            key = ConfigurationManager.AppSettings["KEY_EMOTION"];
            serverFolderPath = ConfigurationManager.AppSettings["UPLOAD_DIR"];
            emoHelper = new EmotionHelper(key);
        }
        // GET: EmoUploader
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase file)
        {

            if (file?.ContentLength > 0)//Si es nulo y mayor que 0
            {
                try
                {
                    var pictureName = Guid.NewGuid().ToString();
                    pictureName += Path.GetExtension(file.FileName);
                    var route = Server.MapPath(serverFolderPath);
                    route += $"/{pictureName}";
                    file.SaveAs(route);
                    var emoPicture = await emoHelper.DetectAndExtracFacesAsync(file.InputStream);
                    emoPicture.Name = Path.GetFileName(file.FileName);
                    emoPicture.Path = $"{serverFolderPath}/{pictureName}";
                    db.EmoPictures.Add(emoPicture);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Details", "EmoPictures", new { Id = emoPicture.Id });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return View();

        }
    }
}