using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixResto.Controllers
{
    public class RestaurantController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
      
    }
}