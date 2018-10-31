using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChoixResto;
using ChoixResto.Controllers;

namespace ChoixResto.Tests.Controllers
{
    [TestClass]
    public class AccueilControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            AccueilController controller = new AccueilController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccueilController_Index_RenvoiVueParDefaut()
        {
            AccueilController controller = new AccueilController();

            ViewResult resultat = (ViewResult)controller.Index();

            Assert.AreEqual(string.Empty, resultat.ViewName);
        }

        //[TestMethod]
        //public void AccueilController_AfficheDate_RenvoiVueIndexEtViewData()
        //{
        //    AccueilController controller = new AccueilController();

        //    ViewResult resultat = (ViewResult)controller.AfficheDate("Nicolas");

        //    Assert.AreEqual("Index", resultat.ViewName);
        //    Assert.AreEqual(new DateTime(2012, 4, 28), resultat.ViewData["date"]);
        //    Assert.AreEqual("Bonjour Nicolas !", resultat.ViewBag.Message);
        //}
    }
}
