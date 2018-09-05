using HotelSystem.Model;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class HomeController : Controller
    {
        private DBModelContainer DbContext = new DBModelContainer();

        // GET: Hotel/Home
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Index(int pageIndex = 1)
        {
            var orders = from m in DbContext.Order
                         where m.HotelInfoId == SessionInfo.hotelUser.HotelInfoId && m.State == "1"&&( m.Payment == true||m.PaymentMethod==1)
                         select m;
            if (orders.Count()>9)
            {
                ViewBag.Total = orders.Sum(m => m.HousingPrice);
            }
            else
            {
                ViewBag.Total = 0;
            }
            var viewOrder = orders.OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_OrderList", viewOrder);
            return View(viewOrder);
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var role = from m in DbContext.UserRole where m.UsersId == SessionInfo.hotelUser.Id select m.Menu;
            ViewBag.User = Session["HotelUser"] as HotelUsers;
            return View(role);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            HotelUsers u = Session["HotelUser"] as HotelUsers;
            return View(u);
        }
    }
}