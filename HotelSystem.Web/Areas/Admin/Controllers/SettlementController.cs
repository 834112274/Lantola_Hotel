using HotelSystem.Model;
using System;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class SettlementController : Controller
    {
        private DBModelContainer DbContext = new DBModelContainer();

        // GET: Admin/Settlement
        [Login(Area = "Admin", Role = "system")]
        public ActionResult OrderList(string hotel, DateTime start, DateTime end, int pageIndex = 1)
        {
            var orders = from m in DbContext.Order
                         where m.CreateTime >= start && m.CreateTime <= end && m.HotelInfoId == hotel && m.SettlementId == null
                         && m.Payment == true
                         && m.State != "0"
                         && m.IsValid == true
                         select m;

            var viewOrder = orders.OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_OrderList", viewOrder);
            return View(viewOrder);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult HotelList(string start, string end, int pageIndex = 1)
        {
            ViewBag.Message = TempData["message"] as string;
            DateTime Start = DateTime.Parse(string.IsNullOrEmpty(start) ? DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") : start);
            DateTime End = DateTime.Parse(string.IsNullOrEmpty(end) ? DateTime.Now.ToString("yyyy-MM-dd") : end);
            var orders = (from m in DbContext.Order
                          where m.CreateTime >= Start
                          && m.CreateTime <= End
                          && m.SettlementId == null
                          && m.Payment == true
                          && m.State != "0"
                          && m.IsValid == true
                          group m by new { HotelInfoId = m.HotelInfoId, HotelName = m.HotelName } into g
                          select g).ToList().Select(t => new Settlement
                          {
                              Id = "",
                              StartTime = Start,
                              EndTime = End,
                              Amount = t.Sum(m => m.HousingPrice),
                              OrderCount = t.Count(),
                              CreateTime = DateTime.Now,
                              HotelInfoId = t.Key.HotelInfoId,
                              HotelName = t.Key.HotelName
                          });
            var viewData = orders.OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", viewData);
            return View(viewData);
        }

        public ActionResult CreateSettlement(string hotel, DateTime start, DateTime end)
        {
            if (string.IsNullOrEmpty(hotel))
            {
                TempData["message"] ="<strong>结算失败!</strong> 未找到酒店.";
                return RedirectToAction("HotelList", new { start = start.ToString("yyyy-MM-dd"), end = end.ToString("yyyy-MM-dd") });
            }
            var hotelInfo = DbContext.HotelInfo.Single(m=>m.Id==hotel);
            var orders = from m in DbContext.Order
                         where m.CreateTime >= start && m.CreateTime <= end && m.HotelInfoId == hotel && m.SettlementId == null
                         && m.Payment == true
                         && m.State != "0"
                         && m.IsValid == true
                         select m;
            var settlenment = new Settlement()
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = start,
                EndTime = end,
                Amount = orders.Sum(m => m.HousingPrice),
                OrderCount = orders.Count(),
                CreateTime = DateTime.Now,
                HotelInfoId = hotelInfo.Id,
                HotelName = hotelInfo.Name
            };
            foreach (var order in orders)
            {
                order.SettlementId = settlenment.Id;
            }
            UserLog log = new UserLog()
            {
                Id = Guid.NewGuid().ToString(),
                Level="info",
                TypeName= "settlenment",
                UserId=SessionInfo.systemUser.Id,
                UserName=SessionInfo.systemUser.Name,
                UserType="system",
                Content= string.Format("账单结算： 酒店：{0};结算订单数：{1};结算金额：{2};结算起始日期：{3};", 
                settlenment.HotelName, 
                settlenment.OrderCount, 
                settlenment.Amount,
                settlenment.StartTime.ToString("yyyy-MM-dd")+"~" +settlenment.EndTime.ToString("yyyy-MM-dd")),
                CreateTime=DateTime.Now
            };
            DbContext.UserLog.Add(log);
            DbContext.Settlement.Add(settlenment);
            DbContext.SaveChanges();
            TempData["message"] =string.Format( "<strong>结算成功!</strong> 酒店：{0}，结算订单数：{1},结算金额：{2}.",settlenment.HotelName,settlenment.OrderCount,settlenment.Amount);
            return RedirectToAction("HotelList", new { start = start.ToString("yyyy-MM-dd"), end = end.ToString("yyyy-MM-dd") });
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult History(int pageIndex = 1)
        {
            var viewData = (from m in DbContext.Settlement select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HistoryList", viewData);
            return View(viewData);
        }
    }
}