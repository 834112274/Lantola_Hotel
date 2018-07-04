using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Hotel
{
    public class HotelAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Hotel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Hotel_default",
                "Hotel/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "HotelSystem.Web.Areas.Hotel.Controllers" }
            );
        }
    }
}