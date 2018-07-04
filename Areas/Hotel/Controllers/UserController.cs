using HotelSystem.Helper;
using HotelSystem.Model;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class UserController : Controller
    {
        // GET: Hotel/User
        public ActionResult Login()
        {
            Session["User"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users user)
        {
            ViewBag.Message = "登录用户名或密码错误";
            user.Password = Encrypt.Md5(user.Password);
            var loginUser = Query.db.HotelUsers.Where(u => u.Name == user.Name & u.Password == user.Password);
            if (loginUser.Count() > 0)
            {
                Session["User"] = loginUser.First();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Register()
        {
            var p = Query.db.Province.ToList();
            ViewBag.Province = p;
            var id = p.First().Id;
            var c= Query.db.City.Where(m => m.ProvinceID == id).ToList();
            ViewBag.City = c;
            id = c.First().Id;
            ViewBag.District = Query.db.District.Where(m => m.CityID == id).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Register(HttpPostedFileBase BusinessLicense, HttpPostedFileBase Franchise, HttpPostedFileBase Card, HotelInfo hotel,long ProvinceId)
        {
            ViewBag.Province = Query.db.Province.ToList();
            ViewBag.City = Query.db.City.Where(m=>m.ProvinceID== ProvinceId).ToList();
            ViewBag.District = Query.db.District.Where(m => m.CityID == hotel.CityId).ToList();
            ViewBag.ProvinceId = ProvinceId;
            hotel.Id = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(hotel.Name))
            {
                ViewBag.Message = "注册错误：企业名称不能为空！";
                return View(hotel);
            }
            if (string.IsNullOrEmpty(hotel.Adress))
            {
                ViewBag.Message = "注册错误：地址不能为空！";
                return View(hotel);
            }
            if (string.IsNullOrEmpty(hotel.Contact))
            {
                ViewBag.Message = "注册错误：联系人不能为空！";
                return View(hotel);
            }
            if (string.IsNullOrEmpty(hotel.Phone))
            {
                ViewBag.Message = "注册错误：联系人电话不能为空！";
                return View(hotel);
            }
            string TimePath = "/HotelImages/" + hotel.Id + "/";//获取上传路径的物理地址
            if (!Directory.Exists(Server.MapPath(TimePath)))//判断文件夹是否存在
            {
                Directory.CreateDirectory(Server.MapPath(TimePath));//不存在则创建文件夹
            }

            if (BusinessLicense != null)
            {
                string FileType = BusinessLicense.FileName.Substring(BusinessLicense.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                hotel.BusinessLicense = TimePath + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                BusinessLicense.SaveAs(Server.MapPath(hotel.BusinessLicense)); //保存操作
            }
            else
            {
                Directory.Delete(Server.MapPath(TimePath), true);
                ViewBag.Message = "注册错误：需上传经营许可证";
                return View(hotel);
            }
            if (Franchise != null)
            {
                string FileType = Franchise.FileName.Substring(Franchise.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                hotel.Franchise = TimePath + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                Franchise.SaveAs(Server.MapPath(hotel.Franchise)); //保存操作
            }
            else
            {
                Directory.Delete(Server.MapPath(TimePath), true);
                ViewBag.Message = "注册错误：需上传特许经营许可证";
                return View(hotel);
            }
            if (Card != null)
            {
                string FileType = Card.FileName.Substring(Card.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                hotel.Card = TimePath + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                Card.SaveAs(Server.MapPath(hotel.Card)); //保存操作
            }
            hotel.Apply = DateTime.Now;
            Query.db.HotelInfo.Add(hotel);
            try
            {
                Query.db.SaveChanges();
                return View("RegisterResult");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "注册错误：" + ex.Message;
                return View(hotel);
            }
        }

        public ActionResult RegisterResult()
        {
            return View();
        }
    }
}