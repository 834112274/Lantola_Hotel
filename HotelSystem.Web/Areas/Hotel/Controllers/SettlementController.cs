using HotelSystem.Model;
using System;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class SettlementController : Controller
    {
        private DBModelContainer DbContext = new DBModelContainer();

        // GET: Admin/Settlement
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult OrderList(string hotel, DateTime start, DateTime end, string settlementType, int pageIndex = 1)
        {
            ViewBag.hotel = hotel;
            ViewBag.start = start;
            ViewBag.end = end;
            ViewBag.settlementType = settlementType;
            var orders = from m in DbContext.Order
                         where m.EndTime >= start && m.EndTime <= end && m.HotelInfoId == hotel
                         && m.Payment == true
                         && m.State != "0"
                         && m.IsValid == true
                         select m;
            string type = string.Empty;
            if (settlementType != "1")
            {
                orders = from m in orders where m.SettlementId == null select m;
            }
            else
            {
                orders = from m in orders where m.SettlementId != null select m;
            }
            var viewOrder = orders.OrderByDescending(m => m.EndTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_OrderList", viewOrder);
            return View(viewOrder);
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult History(string start, string end, int pageIndex = 1)
        {
            DateTime Start = DateTime.Parse(string.IsNullOrEmpty(start) ? DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") : start);
            DateTime End = DateTime.Parse(string.IsNullOrEmpty(end) ? DateTime.Now.ToString("yyyy-MM-dd") : end).AddDays(1);
            var hotelId = SessionInfo.hotel.Id;
            End = End.AddDays(1);
            var viewData = (from m in DbContext.Settlement where m.CreateTime >= Start && m.CreateTime <= End && m.HotelInfoId == hotelId select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HistoryList", viewData);
            return View(viewData);
        }
    }
}