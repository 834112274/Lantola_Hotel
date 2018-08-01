using HotelSystem.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class HotelController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Admin/Hotel
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Examine(int pageIndex = 1)
        {
            var hotels = DbContext.HotelInfo.OrderByDescending(a => a.Apply).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", hotels);
            return View(hotels);
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult Examine(string key, int pageIndex = 1)
        {
            if (key == null) key = "";
            var hotels = DbContext.HotelInfo.Where(h => h.Name.Contains(key) || h.Adress.Contains(key)).OrderByDescending(a => a.Apply).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", hotels);
            return View(hotels);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult ExamineDetails(string id)
        {
            var hotels = DbContext.HotelInfo.Where(h => h.Id == id);
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
            var hotels = DbContext.HotelInfo.Where(h => h.Id == id);
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
                hotel.ExamineUser = SessionInfo.systemUser.Name;
                HotelUsers user = new HotelUsers()
                {
                    Id = Guid.NewGuid().ToString(),
                    HotelInfoId= id,
                    Name = userName,
                    Password = "e10adc3949ba59abbe56e057f20f883e",
                    Role="hotel",
                    CreateTime = DateTime.Now,
                    LastLogin = DateTime.Now,
                    Vail=true,
                    Status=0
                };
                DbContext.HotelUsers.Add(user);
                //分配所有权限
                var cur_menus_id = from m in DbContext.UserRole where m.UserType == "hotel" && m.UsersId == user.Id select m.MenuId;
                var menus = from m in DbContext.Menu where m.Type == "hotel" select m;
                var new_menus = (from m in menus
                                 where !cur_menus_id.Contains(m.Id)
                                 select m).ToList().Select(m => new UserRole() { Id = Guid.NewGuid().ToString(), UsersId = user.Id, MenuId = m.Id, CreateTime = DateTime.Now, UserType = "hotel" }).ToList();

                DbContext.UserRole.AddRange(new_menus);
                DbContext.SaveChanges();
                return View("ExamineDetails", hotel);
            }
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Policy(int pageIndex = 1)
        {
            var policys = DbContext.Policy.OrderByDescending(a => a.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_PolicyList", policys);
            string json = string.Empty;
            using (FileStream fs = new FileStream(Server.MapPath("/Scripts/ico/ico.json"), FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            if (!string.IsNullOrEmpty(json))
            {
                ViewBag.Ico = JsonConvert.DeserializeObject(json);
            }
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
            DbContext.Policy.Add(policy);
            DbContext.SaveChanges();
            return RedirectToAction("Policy");
        }
    }
}