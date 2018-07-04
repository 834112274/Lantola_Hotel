using HotelSystem.Model;
using System.Linq;
using System.Web.Mvc;

namespace HotelSystem.Web.Controllers
{
    public class PartialController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Partial
        public ActionResult Province()
        {
            ViewBag.Province = DbContext.Province.ToList();
            return View();
        }

        public ActionResult City(long id)
        {
            ViewBag.City = DbContext.City.Where(m => m.ProvinceID == id).ToList();

            return View();
        }

        public ActionResult District(long id)
        {
            ViewBag.District = DbContext.District.Where(m => m.CityID == id).ToList();
            return View();
        }

        public ActionResult Demo()
        {
            return View();
        }
    }
}