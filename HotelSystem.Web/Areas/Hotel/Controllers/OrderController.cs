using HotelSystem.Model;
using System;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class OrderController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Hotel/Order
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Manage(string Payment, string CreateStart, string CreateEnd, string Number, string Occupant, string StartTime, string EndTime, int pageIndex = 1)
        {
            var orders = from m in DbContext.Order
                         where m.HotelInfoId == SessionInfo.hotelUser.HotelInfoId
                         select m;
            if (!string.IsNullOrEmpty(Payment))
            {
                if (Payment == "0")
                {
                    orders = from m in orders
                             where m.Payment == false
                             select m;
                }
                else if (Payment == "1")
                {
                    orders = from m in orders
                             where m.Payment == true
                             select m;
                }
            }
            //申请时间
            if (!string.IsNullOrEmpty(CreateStart))
            {
                var createStart = DateTime.Parse(CreateStart);
                orders = from m in orders
                         where m.CreateTime >= createStart
                         select m;
            }
            if (!string.IsNullOrEmpty(CreateEnd))
            {
                var createEnd = DateTime.Parse(CreateEnd);
                orders = from m in orders
                         where m.CreateTime <= createEnd
                         select m;
            }
            //入住时间
            if (!string.IsNullOrEmpty(StartTime))
            {
                var startTime = DateTime.Parse(StartTime);
                orders = from m in orders
                         where m.StartTime >= startTime
                         select m;
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                var endTime = DateTime.Parse(EndTime);
                orders = from m in orders
                         where m.StartTime <= endTime
                         select m;
            }

            if (!string.IsNullOrEmpty(Number))
            {
                orders = from m in orders
                         where m.Number.Contains(Number)
                         select m;
            }

            if (!string.IsNullOrEmpty(Occupant))
            {
                var ids = from m in DbContext.Occupant where m.Name.Contains(Occupant) select m.Id;
                orders = from m in orders
                         where ids.Contains( m.Id)
                         select m;
            }
            var viewOrder = orders.OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_OrderList", viewOrder);
            return View(viewOrder);
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Confirm(string id)
        {
            var order = DbContext.Order.Single(m => m.Id == id);
            order.State = "2";
            order.UpdateUser = SessionInfo.hotelUser.Id;
            order.UpdateTime = DateTime.Now;
            //添加消息通知
            Message msg = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                ToUser = order.GuestUserId,
                FromUser = order.HotelName,
                Title = string.Format("你预定的：《{0}》 酒店管理员 已确认", order.HotelName),
                Content = string.Format(string.Format("尊敬的客户，您的订单{0}，{1}，{2}入住{3}间{4}晚，订单已确认，感谢您的预订。酒店信息：{5}，{6}",
                    order.Number,
                    order.HotelName,
                    order.StartTime.ToString("yyyy年MM月dd日") + "入住",
                    order.RoomName + order.RoomCount.ToString(),
                    (order.EndTime - order.StartTime).Days.ToString(),
                    order.HotelInfo.District.DistrictName + order.HotelInfo.Adress,
                    order.HotelInfo.Tel)),
                CreateTime = DateTime.Now,
                Send = false,
                Read = false,
                ParentMessageId = "",
                SendTime = DateTime.Now,
                RendTime = DateTime.Now
            };
            DbContext.Message.Add(msg);
            DbContext.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult cancel(string id)
        {
            var order = DbContext.Order.Single(m => m.Id == id);
            order.State = "0";
            order.UpdateUser = SessionInfo.hotelUser.Id;
            order.UpdateTime = DateTime.Now;
            DbContext.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}