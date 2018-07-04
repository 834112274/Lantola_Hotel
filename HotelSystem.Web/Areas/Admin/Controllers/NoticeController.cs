using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class NoticeController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Admin/Notice
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Manage()
        {

            return View();
        }
        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult Manage(string ToUser,string Title, string Content)
        {
            ViewBag.ToUser = ToUser;
            ViewBag.Content = Content;
            if (string.IsNullOrEmpty(ToUser))
            {
                ViewBag.Message = "<strong>错误!</strong> 接收人不能为空.";
                return View();
            }
            if (string.IsNullOrEmpty(Title))
            {
                ViewBag.Message = "<strong>错误!</strong> 消息标题不能为空.";
                return View();
            }
            if (string.IsNullOrEmpty(Content))
            {
                ViewBag.Message = "<strong>错误!</strong> 消息内容不能为空.";
                return View();
            }
            var users = from m in DbContext.GuestUser where m.Name == ToUser || m.Id == ToUser || m.Phone == ToUser || m.Email == ToUser select m;
            if (users.Count() > 0)
            {
                var user = users.First();
                //添加消息通知
                Message msg = new Message()
                {
                    Id = Guid.NewGuid().ToString(),
                    ToUser = user.Id,
                    FromUser = "LANTOLA运营专员",
                    Title = Title,
                    Content = Content,
                    CreateTime = DateTime.Now,
                    Send = false,
                    Read = false,
                    ParentMessageId = "",
                    SendTime = DateTime.Now,
                    RendTime = DateTime.Now
                };
                DbContext.Message.Add(msg);
                DbContext.SaveChanges();
                ViewBag.Message = "<strong>消息发送成功!</strong> ";
                return View();
            }
            else
            {
                ViewBag.Message = "<strong>错误!</strong> 未找到接收用户.";
                return View();
            }
        }
    }
}