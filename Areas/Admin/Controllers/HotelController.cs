using HotelSystem.Model;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class HotelController : Controller
    {
        // GET: Admin/Hotel
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Examine(int pageIndex = 1)
        {
            var hotels = Query.db.HotelInfo.OrderByDescending(a => a.Apply).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", hotels);
            return View(hotels);
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult Examine(string key, int pageIndex = 1)
        {
            if (key == null) key = "";
            var hotels = Query.db.HotelInfo.Where(h => h.Name.Contains(key) || h.Adress.Contains(key)).OrderByDescending(a => a.Apply).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", hotels);
            return View(hotels);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult ExamineDetails(string id)
        {
            var hotels = Query.db.HotelInfo.Where(h => h.Id == id);
            if (hotels.Count() == 0)
            {
                ViewBag.Message = "未找到申请单";
                HotelInfo h = new HotelInfo();
                return View(h);
            }
            else
            {
                return View(hotels.First());
            }
        }
        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult ExamineHandle(string id,string result,string userName)
        {
            var hotels = Query.db.HotelInfo.Where(h => h.Id == id);
            if (hotels.Count() == 0)
            {
                ViewBag.Message = "未找到申请单";
                HotelInfo h = new HotelInfo();
                return View("ExamineDetails", h);
            }
            else
            {
                var hotel = hotels.First();
                if (Query.ExistHotelUserName(userName))
                {
                    ViewBag.Message = "登录用户名已存在";
                    return View("ExamineDetails", hotel);
                }
                hotel.ExamineTime = DateTime.Now;
                if (result.Equals("通过"))
                {
                    hotel.Valid = 1;
                    hotel.ExamineStatus = 1;
                }
                else
                {
                    hotel.ExamineStatus = 2;
                }
                hotel.ExamineUser = SessionInfo.user.Name;
                HotelUsers user = new HotelUsers()
                {
                    Id = Guid.NewGuid().ToString(),
                    HotelInfoId= id,
                    Name = userName,
                    Password = "e10adc3949ba59abbe56e057f20f883e",
                    Role="hotel",
                    CreateTime = DateTime.Now,
                    LastLogin = DateTime.Now
                };
                Query.db.HotelUsers.Add(user);
                Query.db.SaveChanges();
                return View("ExamineDetails", hotel);
            }
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Policy(int pageIndex = 1)
        {
            var policys = Query.db.Policy.OrderByDescending(a => a.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_PolicyList", policys);
            return View(policys);
        }

        public ActionResult AddPolicy(Policy policy, HttpPostedFileBase Icon)
        {
            policy.Id = Guid.NewGuid().ToString();
            policy.CreateTime = DateTime.Now;
            if (Icon != null)
            {
                string FileType = Icon.FileName.Substring(Icon.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                policy.Icon = "/PolicyImages/"+ Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                Icon.SaveAs(Server.MapPath(policy.Icon)); //保存操作
            }
            Query.db.Policy.Add(policy);
            Query.db.SaveChanges();
            return RedirectToAction("Policy");
        }
    }
}