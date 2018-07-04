using HotelSystem.Model;
using System;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class CommentController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Hotel/Comment
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Manage(int pageIndex = 1)
        {
            ViewBag.Message = TempData["message"] as string;
            var comment = (from m in DbContext.Comment
                           where m.Order.HotelInfoId == SessionInfo.hotelUser.HotelInfoId
                           select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);

            if (Request.IsAjaxRequest())
                return PartialView("_CommentList", comment);
            return View(comment);
        }
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult ReplyComment(string id,string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                TempData["message"] = "<strong>回复失败!</strong>回复内容不能空. ";
                return RedirectToAction("Manage");
            }
            Reply reply = new Reply() {
                Id = Guid.NewGuid().ToString(),
                Content = Content,
                CommentId = id,
                HotelUsersId = SessionInfo.hotelUser.Id,
                CreateTime = DateTime.Now
            };
            var guser = (from m in DbContext.Comment where m.Id == id select m.GuestUserId).First();
            var hotelName = SessionInfo.hotelUser.HotelInfo.Name;
            //添加消息通知
            Message msg = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                ToUser = guser,
                FromUser = hotelName,
                Title = string.Format("你发布的评论：《{0}》 酒店管理员 已回复", hotelName),
                Content = reply.Content,
                CreateTime = DateTime.Now,
                Send=false,
                Read=false,
                ParentMessageId="",
                SendTime=DateTime.Now,
                RendTime=DateTime.Now
            };
            DbContext.Message.Add(msg);
            DbContext.Reply.Add(reply);
            DbContext.SaveChanges();
            TempData["message"] = "<strong>回复成功!</strong> ";
            return RedirectToAction("Manage");
        }
    }
}