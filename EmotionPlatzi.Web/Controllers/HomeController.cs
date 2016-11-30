using EmotionPlatzi.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmotionPlatzi.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.WelcomeMessage = "Hola mundo!";
            ViewBag.ValorEntero = 1;
            return View();
        }

        public ActionResult IndexAlt()
        {
            var modelo = new Home();
            modelo.WelcomeMessage = "Hola Mundo desde el modelo!";

            return View(modelo);
        }

        public ActionResult IndexSinLayout()
        {
            var modelo = new Home();
            modelo.WelcomeMessage = "Hola Mundo desde el modelo sin layout!";
            modelo.FooterMessage = "Footer sin layout!";
            return View(modelo);
        }
        public ActionResult IndexCustomLayout()
        {
            var modelo = new Home();
            modelo.WelcomeMessage = "Hola Mundo desde el modelo custom layout!";
            modelo.FooterMessage = "Footer custom layout!";

            return View(modelo);
        }
    }
}