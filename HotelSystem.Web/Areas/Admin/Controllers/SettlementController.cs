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

        [Login(Area = "Admin", Role = "system")]
        public ActionResult History()
        {
            return View();
        }
    }
}