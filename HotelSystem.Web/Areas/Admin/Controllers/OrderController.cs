using HotelSystem.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Hotel/Order
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Manage(string state, string applyTime, int pageIndex = 1)
        {
            var orders = from m in DbContext.Order
                         select m;
            if (!string.IsNullOrEmpty(state))
            {
                orders = from m in orders
                         where m.State == state
                         select m;
            }
            var time = new DateTime();
            if (!string.IsNullOrEmpty(applyTime))
            {
                time = DateTime.Parse(applyTime);
            }

            if (time >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                orders = from m in orders
                         where m.CreateTime > time
                         select m;
            }
            var viewOrder = orders.OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_OrderList", viewOrder);
            return View(viewOrder);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Invoice(string Type,string Status, int pageIndex = 1)
        {
            ViewBag.Message = TempData["message"] as string;
            var invoice = from m in DbContext.Invoice select m;
            if (!string.IsNullOrEmpty(Type))
            {
                var type = short.Parse(Type);
                invoice = from m in invoice
                          where m.Type == type
                          select m;
            }
            if (!string.IsNullOrEmpty(Status))
            {
                var status = short.Parse(Status);
                invoice = from m in invoice
                          where m.Status == status
                          select m;
            }

            var data = invoice.OrderByDescending(m => m.Status).ThenByDescending(m=> m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_InvoiceList", data);
            return View(data);
        }
        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult OpneInvoice(string id,string Express,string ExpressNumber)
        {
            if (string.IsNullOrEmpty(Express))
            {
                TempData["message"] = "<strong>开票失败!</strong> 快递公司不能为空.";
                return RedirectToAction("Invoice");
            }
            if (string.IsNullOrEmpty(ExpressNumber))
            {
                TempData["message"] = "<strong>开票失败!</strong> 快递单号不能为空.";
                return RedirectToAction("Invoice");
            }
            var invoices = from m in DbContext.Invoice where m.Id==id select m;
            if (invoices.Count() > 0)
            {
                var invoice = invoices.First();
                invoice.Express = Express;
                invoice.ExpressNumber = ExpressNumber;
                invoice.Status = 2;
                UserLog log = new UserLog()
                {
                    Id = Guid.NewGuid().ToString(),
                    Level = "info",
                    TypeName = "invoices",
                    UserId = SessionInfo.systemUser.Id,
                    UserName = SessionInfo.systemUser.Name,
                    UserType = "system",
                    Content = string.Format("开票：{0}",JsonConvert.SerializeObject(invoice)),
                    CreateTime = DateTime.Now
                };
                DbContext.UserLog.Add(log);
                DbContext.SaveChanges();
                TempData["message"] = "<strong>开票成功!</strong> ";
                return RedirectToAction("Invoice");
            }
            else
            {
                TempData["message"] = "<strong>开票失败!</strong> 未找到发票申请.";
                return RedirectToAction("Invoice");
            }
        }
        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult CloseInvoice(string id)
        {
            var invoices = from m in DbContext.Invoice where m.Id == id select m;
            if (invoices.Count() > 0)
            {
                var invoice = invoices.First();
                if (invoice.Status != 1)
                {
                    TempData["message"] = "<strong>取消失败!</strong> 该发票申请状态无法更改.";
                    return RedirectToAction("Invoice");
                }
                //更新发票申请状态
                invoice.Status = 0;
                //更新订单发票状态
                foreach (var order in invoice.Order)
                {
                    order.IsInvoice = 0;
                    order.InvoiceId = null;
                }
                
                DbContext.SaveChanges();
                TempData["message"] = "<strong>取消成功!</strong> ";
                return RedirectToAction("Invoice");
            }
            else
            {
                TempData["message"] = "<strong>取消失败!</strong> 未找到发票申请.";
                return RedirectToAction("Invoice");
            }
        }
    }
}