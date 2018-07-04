using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class PermissionController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Admin/Permission
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Menu()
        {
            var menus = from m in DbContext.Menu orderby m.Type select m;
            ViewBag.Message = TempData["message"] as string;
            return View(menus);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult CreateMenu()
        {
            var menus = (from m in DbContext.Menu where m.Level == 1 select m).ToList();
            List<SelectListItem> sl = new List<SelectListItem>();
            foreach (var m in menus)
            {
                SelectListItem s = new SelectListItem()
                {
                    Text = "(" + m.Type + ")" + m.Name,
                    Value = m.Id
                };
                sl.Add(s);
            }
            ViewBag.Menus = sl;
            return View();
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult CreateMenu(Menu menu)
        {
            menu.Id = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(menu.ParentId))
            {
                menu.Level = 1;
            }
            else
            {
                menu.Level = 2;
            }
            if (string.IsNullOrEmpty(menu.Type))
            {
                menu.Type = "system";
            }
            DbContext.Menu.Add(menu);
            DbContext.SaveChanges();
            //检查更新admin用户权限
            var number = from u in DbContext.Users where u.Name == "admin" select u;
            var admin_id = number.First().Id;
            var admin_menus_id = from m in DbContext.UserRole where m.UserType == "system" && m.UsersId == admin_id select m.MenuId;
            var system_menus = from m in DbContext.Menu where m.Type == "system" select m;
            var admin_new_menus = (from m in system_menus
                                   where !admin_menus_id.Contains(m.Id)
                             select m).ToList().Select(m => new UserRole() { Id = Guid.NewGuid().ToString(), UsersId = admin_id, MenuId = m.Id, CreateTime = DateTime.Now, UserType = "system" }).ToList();

            DbContext.UserRole.AddRange(admin_new_menus);
            
            //更新酒店菜单
            var hotelUsers = (from m in DbContext.HotelUsers where m.ParentId == null || m.ParentId == "" select m).ToList();
            foreach (var huser in hotelUsers)
            {
                var cur_menus_id = from m in DbContext.UserRole where m.UserType == "hotel" && m.UsersId == huser.Id select m.MenuId;
                var menus = from m in DbContext.Menu where m.Type == "hotel" select m;
                var new_menus = (from m in menus
                                 where !cur_menus_id.Contains(m.Id)
                                 select m).ToList().Select(m => new UserRole() { Id = Guid.NewGuid().ToString(), UsersId = huser.Id, MenuId = m.Id, CreateTime = DateTime.Now, UserType = "hotel" }).ToList();

                DbContext.UserRole.AddRange(new_menus);
            }
            DbContext.SaveChanges();

            TempData["message"] = "<strong> 添加成功!</strong> 菜单已添加,可配置权限查看菜单.";
            return RedirectToAction("Menu");
        }
    }
}