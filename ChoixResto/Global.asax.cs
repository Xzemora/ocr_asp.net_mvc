﻿using ChoixResto.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChoixResto
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            IDatabaseInitializer<BddContext> init = new InitChoixResto();
            Database.SetInitializer(init);
            init.InitializeDatabase(new BddContext());
        }
    }
}
