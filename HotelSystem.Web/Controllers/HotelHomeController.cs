using HotelSystem.Model;
using HotelSystem.Web.Models.View;
using System;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Controllers
{
    public class HotelHomeController : Controller
    {
        private DBModelContainer DbContext = new DBModelContainer();

        // GET: HotelHome
        public ActionResult Index(string id, string startDate, string endDate)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json("无法访问该酒店", JsonRequestBehavior.AllowGet);
            }
            var Hotel = DbContext.HotelInfo.Single(m => m.Id == id);
            ViewBag.Hotel = Hotel;
            //展示图片
            var slideImg = (from m in DbContext.HotelImages
                            where m.HotelInfoId == id && m.Type == 1
                            select m).ToList();
            if (slideImg.Count > 0)
            {
                ViewBag.SlideImgFirst = slideImg[0];
            }
            else
            {
                ViewBag.SlideImgFirst = new HotelImages();
            }
            if (slideImg.Count > 1)
            {
                ViewBag.SlideImgTwo = slideImg[1];
            }
            else
            {
                ViewBag.SlideImgTwo = new HotelImages();
            }
            if (slideImg.Count > 2)
            {
                ViewBag.SlideImgThree = slideImg[2];
            }
            else
            {
                ViewBag.SlideImgThree = new HotelImages();
            }

            //相册数据
            var imgs = from m in DbContext.HotelImages
                       where m.HotelInfoId == id && (m.Type == 2 || m.Type == 3)
                       orderby m.CreateTime
                       select m;

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
            //初始搜索日期
            DateTime start, end;
            if (string.IsNullOrEmpty(startDate))
            {
                start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else
            {
                start = DateTime.Parse(startDate);
            }

            if (string.IsNullOrEmpty(endDate))
            {
                end = start.AddDays(1);
            }
            else
            {
                end = DateTime.Parse(endDate);
            }
            var score = from m in DbContext.Score where m.HotelInfoId == id select m;
            if (score.Count() > 0)
            {
                ViewBag.Score = Math.Round(score.Average(m => m.Value), 1);
            }
            else
            {
                ViewBag.Score = 10;
            }
            ViewBag.startDate = start.ToString("yyyy-MM-dd");
            ViewBag.endDate = end.ToString("yyyy-MM-dd");
            //搜索房型
            var rooms = HotelView.GetRooms(id, start, end);
            ViewBag.Rooms = rooms;

            //酒店设施
            var p = (from m in DbContext.HotelPolicy where m.HotelInfoId == id select m.Policy).ToList();
            ViewBag.Policy0 = p.Where(m => m.Type == 1);
            ViewBag.Policy1 = p.Where(m => m.Type == 2);
            ViewBag.Policy2 = p.Where(m => m.Type == 3);

            if (Hotel.HotelService == null)
            {
                ViewBag.Service = new HotelService();
            }
            else
            {
                ViewBag.Service = Hotel.HotelService;
            }
            //收藏
            if (SessionInfo.guestUser != null)
            {
                ViewBag.Login = "true";
                var collection = DbContext.Collection.Where(m => m.GuestUserId == SessionInfo.guestUser.Id && m.HotelInfoId == id);
                if (collection.Count() > 0)
                {
                    ViewBag.Collection = "true";
                }
                else
                {
                    ViewBag.Collection = false;
                }
            }
            else
            {
                ViewBag.Login = "false";
            }
            var logo = from m in DbContext.HotelImages
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

        [ChildActionOnly]
        public ActionResult Header(string id)
        {
            ViewBag.Id = id;
            var logo = from m in DbContext.HotelImages
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
            if (SessionInfo.guestUser != null)
            {
                ViewBag.User = SessionInfo.guestUser;
            }
            return View();
        }

        public ActionResult Boardroom(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View(new Conference());
            var h = DbContext.HotelInfo.Single(m => m.Id == id);
            @ViewBag.Hotel = h;
            //酒店设施
            var p = (from m in DbContext.HotelPolicy where m.HotelInfoId == id select m.Policy).ToList();
            ViewBag.Policy0 = p.Where(m => m.Type == 1);
            ViewBag.Policy1 = p.Where(m => m.Type == 2);
            ViewBag.Policy2 = p.Where(m => m.Type == 3);

            if (h.HotelService == null)
            {
                ViewBag.Service = new HotelService();
            }
            else
            {
                ViewBag.Service = h.HotelService;
            }
            if (h.Conference == null)
            {
                return View(new Conference());
            }
            return View(h.Conference);
        }

        public ActionResult Restaurant(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View(new Restaurant());
            var r = DbContext.HotelInfo.Single(m => m.Id == id).Restaurant;
            return View(r);
        }

        public ActionResult Activity(string id)
        {
            var activit = (from m in DbContext.Activity
                           where m.HotelInfoId == id
                           select m).ToList();
            return View(activit);
        }

        public ActionResult Comment(string id, int pageIndex = 1)
        {
            var comment = (from m in DbContext.Comment
                           where m.Order.HotelInfoId == id
                           select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            var score = from m in DbContext.Score where m.HotelInfoId == id select m;
            if (score.Count() > 0)
            {
                ViewBag.SumAvg = Math.Round(score.Average(m => m.Value), 1);
            }
            else
            {
                ViewBag.SumAvg = 10;
            }
            var t = from m in DbContext.ScoreType.ToList()
                    join s in score.ToList() on m.Id equals s.ScoreTypeId into tmp1

                    from tmp2 in tmp1.DefaultIfEmpty(new Score{ Id = "", Value = 10 })
                    select new { name = m.Name, value = tmp2.Value };

            var itemAvg = (from m in t
                           group m by m.name into g
                           select new { name = g.Key, avg = g.Average(m => m.value) }).ToList().Select(m => Tuple.Create(m.name, m.avg));

            ViewBag.ItemAvg = itemAvg;
            if (Request.IsAjaxRequest())
                return PartialView("_CommentList", comment);
            return View(comment);
        }

        public ActionResult Album(string id)
        {
            var images = from m in DbContext.HotelImages
                         where m.HotelInfoId == id && (m.Type == 2 || m.Type == 3)
                         orderby m.CreateTime
                         select m;
            return View(images);
        }

        public ActionResult Map(string id)
        {
            var g = from m in DbContext.Guide
                    where m.HotelInfoId == id
                    select m;
            ViewBag.Airport = g.Where(m => m.Type == 1);
            ViewBag.Train = g.Where(m => m.Type == 2);
            ViewBag.Landmark = g.Where(m => m.Type == 3);
            
            var Hotel = DbContext.HotelInfo.Single(m => m.Id == id);
            ViewBag.Hotel = Hotel;
            ViewBag.Adress = Hotel.City.CityName + " " + Hotel.District.DistrictName + " " + Hotel.Adress;
            var logo = from m in DbContext.HotelImages
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
    }
}