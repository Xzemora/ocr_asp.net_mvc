using ChoixResto.Models;
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
            using (IDal dal = new Dal())
            {
                List<Resto> listeDesRestaurants = dal.ObtientTousLesRestaurants();
                return View(listeDesRestaurants);
            }
        }

        public ActionResult ModifierRestaurant(int? id)
        {
            if (id.HasValue)
            {
                using (IDal dal = new Dal())
                {
                    Resto restaurant = dal.ObtientTousLesRestaurants().FirstOrDefault(r => r.Id == id.Value);
                    if (restaurant == null)
                        return View("Error");
                    return View(restaurant);
                }
            }
            else
                return View("Error");
        }

        [HttpPost]
        public ActionResult ModifierRestaurant(Resto resto)
        {
            using (IDal dal = new Dal())
            {
                dal.ModifierRestaurant(resto.Id, resto.Nom, resto.Telephone);
                return RedirectToAction("Index");
            }
        }

        public ActionResult CreerRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreerRestaurant(Resto resto)
        {
            using (IDal dal = new Dal())
            {
                if (dal.RestaurantExiste(resto.Nom))
                {
                    ModelState.AddModelError("Nom", "Ce nom de restaurant existe déjà");
                    return View(resto);
                }
                if (!ModelState.IsValid)
                    return View(resto);
                dal.CreerRestaurant(resto.Nom, resto.Telephone);
                return RedirectToAction("Index");
            }
        }
    }
}