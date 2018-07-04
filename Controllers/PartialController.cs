using HotelSystem.Model;
using System.Linq;
using System.Web.Mvc;

namespace HotelSystem.Web.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial
        public ActionResult Province()
        {
            ViewBag.Province = Query.db.Province.ToList();
            return View();
        }

        public ActionResult City(long id)
        {
            ViewBag.City = Query.db.City.Where(m => m.ProvinceID == id).ToList();

            return View();
        }

        public ActionResult District(long id)
        {
            ViewBag.District = Query.db.District.Where(m => m.CityID == id).ToList();
            return View();
        }

        public ActionResult Demo()
        {
            return View();
        }
    }
}