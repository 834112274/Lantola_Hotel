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
        public ActionResult History(int pageIndex = 1)
        {
            var viewData = (from m in DbContext.Settlement where m.HotelInfoId==SessionInfo.hotel.Id select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HistoryList", viewData);
            return View(viewData);
        }
    }
}