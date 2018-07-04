using HotelSystem.Helper;
using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Admin/User

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LoginOut()
        {
            Session["SystemUser"] = null;
            Response.Cookies["system_remember"].HttpOnly = true; //只能服务器访问
            Response.Cookies["system_remember"].Expires = DateTime.Now.AddMonths(1); //用键名为ID的Cookie设置生存时间
            Response.Cookies["system_remember"].Value = "0"; //将键名为ID的Cookie的值设置为文本框内容
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(Users user, string remember)
        {
            ViewBag.Message = "登录用户名或密码错误";
            user.Password = Encrypt.Md5(user.Password);
            var loginUser = DbContext.Users.Where(u => u.Name == user.Name & u.Password == user.Password);
            if (loginUser.Count() > 0)
            {
                var u = loginUser.First();
                if (remember == "1")
                {
                    Response.Cookies["system_remember"].HttpOnly = true; //只能服务器访问
                    Response.Cookies["system_remember"].Expires = DateTime.Now.AddMonths(1); //用键名为ID的Cookie设置生存时间
                    Response.Cookies["system_remember"].Value = "1"; //将键名为ID的Cookie的值设置为文本框内容

                    Response.Cookies["system_user"].HttpOnly = true;
                    Response.Cookies["system_user"].Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies["system_user"].Value = Url.Encode(u.Name);

                    Response.Cookies["system_password"].HttpOnly = true;
                    Response.Cookies["system_password"].Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies["system_password"].Value = u.Password;
                }

                Session["SystemUser"] = u;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult ChangePassword()
        {
            var u = SessionInfo.systemUser;
            return View(u);
        }

        [Login(Area = "Admin", Role = "system")]
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
            var u = DbContext.Users.Single(m => m.Id == id);
            var old = Encrypt.Md5(OldPassword);
            if (u.Password!=old)
            {
                ViewBag.Message = "<strong>修改失败!</strong> 原密码错误.";
                return View();
            }
            u.Password = Encrypt.Md5(Password);
            DbContext.SaveChanges();
            ViewBag.Message = "<strong>密码修改成功!</strong> 下次登录请使用新密码登录，旧密码已失效.";
            return View();
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Children(string message)
        {
            var users = (from m in DbContext.Users where m.ParentId == SessionInfo.systemUser.Id && m.Vail == true select m);
            ViewBag.Message = TempData["message"] as string;
            return View(users);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult CreateUser(Users user)
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
            user.Role = "system";
            user.RegisterTime = DateTime.Now;
            user.RegisterIP = Request.ServerVariables.Get("Remote_Addr").ToString();
            user.Vail = true;
            user.Status = 1;
            user.ParentId = SessionInfo.systemUser.Id;
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
            TempData["message"] = "<strong> 添加成功!</strong> 用户已成功添加，已分配默认权限，请注意查看修改.";
            return RedirectToAction("Children");
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["message"] = "<strong> 删除失败!</strong> 未知用户.";
                return RedirectToAction("Children");
            }
            var u = DbContext.Users.Single(m => m.Id == id);
            u.Vail = false;
            DbContext.SaveChanges();
            TempData["message"] = "<strong> 删除成功!</strong>" + u.Name + "已成功删除，该账号将无法使用.";
            return RedirectToAction("Children");
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Reset(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["message"] = "<strong> 重置密码失败!</strong> 未知用户.";
                return RedirectToAction("Children");
            }
            var u = DbContext.Users.Single(m => m.Id == id);
            u.Password = "e10adc3949ba59abbe56e057f20f883e";
            DbContext.SaveChanges();
            TempData["message"] = "<strong> 重置密码成功!</strong>" + u.Name + "密码已重置，下次登录需使用新密码登录.";
            return RedirectToAction("Children");
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Permission(string id)
        {
            var user_role = from m in DbContext.UserRole where m.UsersId == id&& m.UserType == "system" select m.MenuId;
            var role = from m in DbContext.Menu where m.Type== "system" select m;
            ViewBag.Menu = role.ToList();
            ViewBag.UserName = DbContext.Users.Single(m=>m.Id==id).Name;
            return View(user_role);
        }

        [Login(Area = "Admin", Role = "system")]
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
                                UserType = "system"
                            };
                            DbContext.UserRole.Add(r);
                        }
                    }
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
    }
}