using HotelSystem.Helper;
using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User

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
            var loginUser = Query.db.Users.Where(u => u.Name == user.Name & u.Password ==user.Password );
            if (loginUser.Count() > 0)
            {
                if (loginUser.First().Role.Equals("system"))
                {
                    Session["User"] = loginUser.First();
                    return RedirectToAction("Index", "Home");
                }
                else
                {   
                    return View();
                }
            }
            else
            {
                return View();
            } 
        }
    }
}