using HotelSystem.Helper;
using HotelSystem.Model;
using System;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class UserManageController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Admin/GuestUserManage
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Guest(string key,int pageIndex = 1)
        {
            if (key == null) key = "";
            var users = (from m in DbContext.GuestUser where m.Name.Contains(key)|| m.Phone.Contains(key) select m).OrderByDescending(m => m.LastLogin).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_GuestList", users);
            return View(users);
        }
        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public void ResetPassword(string id)
        {
            var u = DbContext.GuestUser.Single(m=>m.Id==id);
            u.Password= Encrypt.Md5("123456");
            try
            {
                DbContext.SaveChanges();
                Response.Write("success");
                Response.End();
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
                Response.End();
            }
        }
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Hotel(string key, int pageIndex = 1)
        {
            var users = (from m in DbContext.HotelUsers select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", users);
            return View(users);
        }
    }
}