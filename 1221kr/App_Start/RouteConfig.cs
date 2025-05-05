using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _1221kr
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Маршрут для профиля юриста
            routes.MapRoute(
                name: "LawyerProfile",
                url: "Lawyer/Profile",
                defaults: new { controller = "Lawyer", action = "Profile" }
            );

            // Маршрут для редактирования профиля
            routes.MapRoute(
                name: "LawyerUpdateProfile",
                url: "Lawyer/UpdateProfile",
                defaults: new { controller = "Lawyer", action = "UpdateProfile" }
            );

            // Стандартный маршрут
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
            );
        }
    }
}
