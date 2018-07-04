using HotelSystem.Model;
using System.Linq;
using System.Web.Mvc;

namespace HotelSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var homeConfig = from c in Query.db.HomeConfig
                             where c.Valid == true
                             select c;
            if (homeConfig.Count() > 0)
            {
                ViewBag.HomeConfig = homeConfig.First();
            }
            else
            {
                ViewBag.HomeConfig = new HomeConfig();
            }
            ViewBag.Feature = (from f in Query.db.Feature where f.Type == 1 orderby f.UpdateTime descending select f).ToList();
            ViewBag.Hotels = (from h in Query.db.FeatureHotel
                              where h.Valid == true && h.Feature.Type == 1
                              orderby h.Sort
                              select h).ToList();
            ViewBag.Tag = (from f in Query.db.Feature where f.Type == 2 orderby f.UpdateTime descending select f).ToList();
            ViewBag.TagHotels = (from h in Query.db.FeatureHotel
                                 where h.Valid == true && h.Feature.Type == 2
                                 orderby h.Sort
                                 select h).ToList();
            ViewBag.Cooperative= (from m in Query.db.Cooperative
                                  orderby m.CreateTime
                                  select m).ToList();
            return View();
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return View();
        }
        public ActionResult Cooperation()
        {
            return View();
        }
    }
}