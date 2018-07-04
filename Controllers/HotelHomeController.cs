using HotelSystem.Model;
using System.Linq;
using System.Web.Mvc;

namespace HotelSystem.Web.Controllers
{
    public class HotelHomeController : Controller
    {
        // GET: HotelHome
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json("无法访问该酒店", JsonRequestBehavior.AllowGet);
            }
            var Hotel = Query.db.HotelInfo.Single(m => m.Id == id);
            ViewBag.SlideImg = (from m in Query.db.HotelImages
                                where m.HotelInfoId == id && m.Type == 1
                                select m).ToList();
            var imgs = from m in Query.db.HotelImages
                       where m.HotelInfoId == id && (m.Type == 2 || m.Type == 3)
                       orderby m.CreateTime
                       select m;
            ViewBag.Hotel = Hotel;
            var c = imgs.Count();
            if (imgs.Count() > 0)
            {
                ViewBag.Ablum = imgs.First();
                ViewBag.AblumCount = c;
            }
            else
            {
                ViewBag.Ablum = new HotelImages() { Url = "~/Images/fail.jpg" };
            }
            var rooms = Query.db.Room.Include("RoomImages").Include("Price").Where(m => m.HotelInfoId == id).ToList();
            ViewBag.Rooms = rooms;
            var p = (from m in Query.db.HotelPolicy where m.HotelInfoId == id select m.Policy).ToList();
            ViewBag.Policy0 = p.Where(m => m.Type == 0);
            ViewBag.Policy1 = p.Where(m => m.Type == 1);
            ViewBag.Policy2 = p.Where(m => m.Type == 2);

            var s = Query.db.HotelInfo.Single(m => m.Id == id).HotelService;
            if (s == null)
            {
                ViewBag.Service = new HotelService();
            }
            else
            {
                ViewBag.Service = s;
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult Header(string id)
        {
            ViewBag.Id = id;
            var logo = from m in Query.db.HotelImages
                       where m.HotelInfoId == id && m.Type == 0
                       select m;
            if (logo.Count() > 0)
            {
                ViewBag.Logo = logo.First().Url;
            }
            else
            {
                ViewBag.Logo = "/Images/lantola-logo.png";
            }
            return View();
        }

        public ActionResult Boardroom(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View(new Conference());
            var h = Query.db.HotelInfo.Single(m => m.Id == id);
            @ViewBag.Hotel = h;
            return View(h.Conference);
        }

        public ActionResult Restaurant(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View(new Restaurant());
            var r = Query.db.HotelInfo.Single(m => m.Id == id).Restaurant;
            return View(r);
        }

        public ActionResult Activity(string id)
        {
            var activit = (from m in Query.db.Activity
                           where m.HotelInfoId == id
                           select m).ToList();
            return View(activit);
        }

        public ActionResult Album(string id)
        {
            var images = from m in Query.db.HotelImages
                         where m.HotelInfoId == id && (m.Type == 2 || m.Type == 3)
                         orderby m.CreateTime descending
                         select m;
            return View(images);
        }

        public ActionResult Map(string id)
        {
            var g = from m in Query.db.Guide
                    where m.HotelInfoId == id
                    select m;
            ViewBag.Airport = g.Where(m => m.Type == 1);
            ViewBag.Train = g.Where(m => m.Type == 2);
            ViewBag.Landmark = g.Where(m => m.Type == 3);
            var Hotel = Query.db.HotelInfo.Single(m => m.Id == id);
            ViewBag.Adress = Hotel.City.CityName + " " + Hotel.District.DistrictName + " " + Hotel.Adress;
            return View();
        }
    }
}