using System.Web.Mvc;
using System.Web.Routing;

namespace HotelSystem.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                 "Default", // 路由名称
                 "{controller}/{action}/{id}", // 带有参数的 URL
                 new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // 参数默认值
                 new string[] { "HotelSystem.Web.Controllers" }
                 );
            
        }
    }
}