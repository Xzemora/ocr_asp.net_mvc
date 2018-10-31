using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixResto.Controllers
{
    public class AccueilController : Controller
    {
        // GET: Accueil
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult AfficheDate(string id)
        //{
        //    ViewBag.Message = "Bonjour " + id + " !";
        //    ViewData["Date"] = new DateTime(2012, 4, 28);
        //    return View("Index");
        //}
    }
}