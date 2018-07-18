using HotelSystem.Helper;
using HotelSystem.Model;
using HotelSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class UserController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Hotel/User
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LoginOut()
        {
            Session["HotelUser"] = null;
            Response.Cookies["hotel_remember"].HttpOnly = true; //只能服务器访问
            Response.Cookies["hotel_remember"].Expires = DateTime.Now.AddMonths(1); //用键名为ID的Cookie设置生存时间
            Response.Cookies["hotel_remember"].Value = "0"; //将键名为ID的Cookie的值设置为文本框内容
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(Users user,string remember)
        {
            ViewBag.Message = "登录用户名或密码错误";
            user.Password = Encrypt.Md5(user.Password);
            var loginUser = DbContext.HotelUsers.Where(u => u.Name == user.Name & u.Password == user.Password);
            if (loginUser.Count() > 0)
            {
                var u = loginUser.First();
                if (remember == "1")
                {
                    Response.Cookies["hotel_remember"].HttpOnly = true; //只能服务器访问
                    Response.Cookies["hotel_remember"].Expires = DateTime.Now.AddMonths(1); //用键名为ID的Cookie设置生存时间
                    Response.Cookies["hotel_remember"].Value = "1"; //将键名为ID的Cookie的值设置为文本框内容

                    Response.Cookies["hotel_user"].HttpOnly = true;
                    Response.Cookies["hotel_user"].Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies["hotel_user"].Value = Url.Encode(u.Name);

                    Response.Cookies["hotel_password"].HttpOnly = true;
                    Response.Cookies["hotel_password"].Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies["hotel_password"].Value = u.Password;
                }

                Session["HotelUser"] = u;
                UserLog log = new UserLog()
                {
                    Id = Guid.NewGuid().ToString(),
                    Level = "info",
                    TypeName = "login",
                    UserId = SessionInfo.hotelUser.Id,
                    UserName = SessionInfo.hotelUser.Name,
                    UserType = "hotel",
                    Content = string.Format("登录"),
                    CreateTime = DateTime.Now
                };
                DbContext.UserLog.Add(log);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Register()
        {
            var p = DbContext.Province.ToList();
            ViewBag.Province = p;
            var id = p.First().Id;
            var c = DbContext.City.Where(m => m.ProvinceID == id).ToList();
            ViewBag.City = c;
            id = c.First().Id;
            ViewBag.District = DbContext.District.Where(m => m.CityID == id).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Register(HttpPostedFileBase BusinessLicense, HttpPostedFileBase Franchise, HttpPostedFileBase Card, HotelInfo hotel, long ProvinceId)
        {
            ViewBag.Province = DbContext.Province.ToList();
            ViewBag.City = DbContext.City.Where(m => m.ProvinceID == ProvinceId).ToList();
            ViewBag.District = DbContext.District.Where(m => m.CityID == hotel.CityId).ToList();
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
            string TimePath = ServerConfig.GetHotelImgRoute(hotel.Id);


            if (BusinessLicense != null)
            {
                string FileType = BusinessLicense.FileName.Substring(BusinessLicense.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                hotel.BusinessLicense = TimePath + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名

                ImgHelper.Compress(BusinessLicense.InputStream, Server.MapPath(hotel.BusinessLicense), ServerConfig.Level);//压缩保存操作

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

                ImgHelper.Compress(Franchise.InputStream, Server.MapPath(hotel.Franchise), ServerConfig.Level);//压缩保存操作
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
            DbContext.HotelInfo.Add(hotel);
            try
            {
                DbContext.SaveChanges();
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

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult ChangePassword()
        {
            var u = SessionInfo.hotelUser;
            return View(u);
        }

        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult ChangePassword(string id, string Password,string OldPassword)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Message = "<strong>修改失败!</strong> 未找到有效用户.";
                return View();
            }
            if (string.IsNullOrEmpty(Password))
            {
                ViewBag.Message = "<strong>修改失败!</strong> 新密码不能为空.";
                return View();
            }
            
            var u = DbContext.HotelUsers.Single(m => m.Id == id);
            var old = Encrypt.Md5(OldPassword);
            if (u.Password != old)
            {
                ViewBag.Message = "<strong>修改失败!</strong> 原密码错误.";
                return View();
            }
            u.Password = Encrypt.Md5(Password);
            UserLog log = new UserLog()
            {
                Id = Guid.NewGuid().ToString(),
                Level = "worning",
                TypeName = "change-password",
                UserId = SessionInfo.hotelUser.Id,
                UserName = SessionInfo.hotelUser.Name,
                UserType = "hotel",
                Content = string.Format("修改密码"),
                CreateTime = DateTime.Now
            };
            DbContext.UserLog.Add(log);
            DbContext.SaveChanges();
            ViewBag.Message = "<strong>密码修改成功!</strong> 下次登录请使用新密码登录，旧密码已失效.";
            return View();
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Children(string message)
        {
            var users = (from m in DbContext.HotelUsers where m.ParentId == SessionInfo.hotelUser.Id && m.Vail == true select m);
            ViewBag.Message = TempData["message"] as string;
            return View(users);
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult CreateUser(HotelUsers user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                TempData["message"] = "<strong> 添加失败!</strong> 用户名不能为空.";
                return RedirectToAction("Children");
            }
            if (Query.ExistUserName(user.Name))
            {
                TempData["message"] = "<strong> 添加失败!</strong> 用户名已存在.";
                return RedirectToAction("Children");
            }
            user.Id = Guid.NewGuid().ToString();
            user.Password = "e10adc3949ba59abbe56e057f20f883e";
            user.Role = "hotel";
            user.CreateTime = DateTime.Now;
            user.Vail = true;
            user.Status = 1;
            user.ParentId = SessionInfo.hotelUser.Id;
            user.HotelInfoId = DbContext.HotelUsers.Single(m => m.Id == SessionInfo.hotelUser.Id).HotelInfoId;
            user.LastLogin = DateTime.Now;
            DbContext.HotelUsers.Add(user);
            UserLog log = new UserLog()
            {
                Id = Guid.NewGuid().ToString(),
                Level = "worning",
                TypeName = "add-user",
                UserId = SessionInfo.hotelUser.Id,
                UserName = SessionInfo.hotelUser.Name,
                UserType = "hotel",
                Content = string.Format("添加子账号{0}",user.Name),
                CreateTime = DateTime.Now
            };
            DbContext.UserLog.Add(log);
            DbContext.SaveChanges();
            TempData["message"] = "<strong> 添加成功!</strong> 用户已成功添加，已分配默认权限，请注意查看修改.";
            return RedirectToAction("Children");
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["message"] = "<strong> 删除失败!</strong> 未知用户.";
                return RedirectToAction("Children");
            }
            var u = DbContext.HotelUsers.Single(m => m.Id == id);
            DbContext.HotelUsers.Remove(u);
            UserLog log = new UserLog()
            {
                Id = Guid.NewGuid().ToString(),
                Level = "worning",
                TypeName = "delete-user",
                UserId = SessionInfo.hotelUser.Id,
                UserName = SessionInfo.hotelUser.Name,
                UserType = "hotel",
                Content = string.Format("删除子账号{0}", u.Name),
                CreateTime = DateTime.Now
            };
            DbContext.UserLog.Add(log);
            DbContext.SaveChanges();
            TempData["message"] = "<strong> 删除成功!</strong>" + u.Name + "已成功删除，该账号将无法使用.";
            return RedirectToAction("Children");
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Reset(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["message"] = "<strong> 重置密码失败!</strong> 未知用户.";
                return RedirectToAction("Children");
            }
            var u = DbContext.HotelUsers.Single(m => m.Id == id);
            u.Password = "e10adc3949ba59abbe56e057f20f883e";
            UserLog log = new UserLog()
            {
                Id = Guid.NewGuid().ToString(),
                Level = "worning",
                TypeName = "reset",
                UserId = SessionInfo.hotelUser.Id,
                UserName = SessionInfo.hotelUser.Name,
                UserType = "hotel",
                Content = string.Format("重置密码", u.Name),
                CreateTime = DateTime.Now
            };
            DbContext.UserLog.Add(log);
            DbContext.SaveChanges();
            TempData["message"] = "<strong> 重置密码成功!</strong>" + u.Name + "密码已重置，下次登录需使用新密码登录.";
            return RedirectToAction("Children");
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Permission(string id)
        {
            var user_role = from m in DbContext.UserRole where m.UsersId == id && m.UserType == "hotel" select m.MenuId;
            var role = from m in DbContext.Menu where m.Type == "hotel" select m;
            ViewBag.Menu = role.ToList();
            ViewBag.UserName = DbContext.HotelUsers.Single(m => m.Id == id).Name;
            return View(user_role);
        }

        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult Permission(string id, List<string> menu)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (menu != null && menu.Count > 0)
                {
                    var cur_menu = from m in DbContext.UserRole where m.UsersId == id select m;
                    DbContext.UserRole.RemoveRange(cur_menu);
                    foreach (var t in menu)
                    {
                        if (!string.IsNullOrEmpty(t))
                        {
                            UserRole r = new UserRole()
                            {
                                Id = Guid.NewGuid().ToString(),
                                UsersId = id,
                                MenuId = t,
                                CreateTime = DateTime.Now,
                                UserType = "hotel"
                            };
                            DbContext.UserRole.Add(r);
                        }
                    }
                    UserLog log = new UserLog()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Level = "worning",
                        TypeName = "change-permission",
                        UserId = SessionInfo.hotelUser.Id,
                        UserName = SessionInfo.hotelUser.Name,
                        UserType = "hotel",
                        Content = string.Format("修改权限，用户ID", id),
                        CreateTime = DateTime.Now
                    };
                    DbContext.UserLog.Add(log);
                    DbContext.SaveChanges();
                    TempData["message"] = "<strong> 保存成功!</strong> 已成功分配权限，可登录该用户刷新页面查看.";
                }
                else
                {
                    TempData["message"] = "<strong> 保存失败!</strong> 至少分配一个权限.";
                }
            }
            else
            {
                TempData["message"] = "<strong> 保存失败!</strong> 未知用户.";
            }
            return RedirectToAction("Children");
        }
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Log()
        {
            return View();
        }
    }
}