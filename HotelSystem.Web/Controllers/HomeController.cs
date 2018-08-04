using HotelSystem.Model;
using HotelSystem.Web.Models.View;
using System;
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

            //获取特色
            ViewBag.Feature = (from f in DbContext.Feature where f.Type == 1 orderby f.UpdateTime ascending select f).ToList();


            //获取标签
            ViewBag.Tag = (from f in DbContext.Feature where f.Type == 2 orderby f.UpdateTime descending select f).ToList();



            //获取酒店当日价格
            var hotel = (from m in DbContext.FeatureHotel
                        where m.Valid == true && m.Feature.Type == 2
                        orderby m.Sort
                        select m.HotelInfo).Distinct();
            DateTime start  = DateTime.Now;
            DateTime end = DateTime.Now.AddDays(1);
            var d = (end - start).Days;
            var price = from t in DbContext.Price
                        where t.Date >= start && t.Date < end && t.Status == 1
                        group t by t.PriceTypeId into g
                        select new { g.Key, days = g.Count(), avg = g.Average(t => t.UnitPrice), sum = g.Sum(t => t.UnitPrice), min = g.Min(t => t.UnitPrice), max = g.Max(t => t.UnitPrice) };
            var priceType = from p in price
                            join t in DbContext.PriceType on p.Key equals t.Id
                            where p.days >= d
                            select new { id = t.Room.HotelInfoId, min = p.min, max = p.max };
            var room = from m in priceType
                       group m by m.id into g
                       select new { id = g.Key, min = g.Min(t => t.min), max = g.Max(t => t.max) };

            var hv = from m in hotel.ToList()
                     join r in room.ToList() on m.Id equals r.id
                     select new HotelView()
                     {
                         hotelInfo = m,
                         maxPrice = r.max,
                         minPrice = r.min
                     };
            ViewBag.TagHotelsPrice = hv.ToList();

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
        public ActionResult Search(HotelView hotelView,string city)
        {
            var c = DbContext.City.ToList();
            ViewBag.City = c;
            ViewBag.SearchParams = hotelView;
            var hotels = hotelView.Search();
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", hotels);
            return View(hotels);
        }
    }
}