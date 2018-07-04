using HotelSystem.Model;
using HotelSystem.Web.Models.View;
using System.Linq;
using System.Web.Mvc;

namespace HotelSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Home
        public ActionResult Index()
        {
            var homeConfig = from c in DbContext.HomeConfig
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
            ViewBag.Feature = (from f in DbContext.Feature where f.Type == 1 orderby f.UpdateTime ascending select f).ToList();
            ViewBag.Hotels = (from h in DbContext.FeatureHotel
                              where h.Valid == true && h.Feature.Type == 1
                              orderby h.Sort
                              select h).ToList();
            ViewBag.Tag = (from f in DbContext.Feature where f.Type == 2 orderby f.UpdateTime descending select f).ToList();
            ViewBag.TagHotels = (from h in DbContext.FeatureHotel
                                 where h.Valid == true && h.Feature.Type == 2
                                 orderby h.Sort
                                 select h).ToList();
            ViewBag.Cooperative= (from m in DbContext.Cooperative
                                  orderby m.CreateTime
                                  select m).ToList();
            return View();
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            if (SessionInfo.guestUser != null)
            {
                ViewBag.User = SessionInfo.guestUser;
                //获得未读消息总数
                ViewBag.Message = (from m in DbContext.Message where m.ToUser == SessionInfo.guestUser.Id && m.Read == false select m).Count();
            }
            return View();
        }
        public ActionResult Cooperation()
        {
            return View();
        }
        public ActionResult Search(HotelView hotelView)
        {
            ViewBag.SearchParams = hotelView;
            var hotels = hotelView.Search();
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", hotels);
            return View(hotels);
        }
    }
}