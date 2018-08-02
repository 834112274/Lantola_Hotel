using HotelSystem.Model;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private DBModelContainer DbContext = new DBModelContainer();

        // GET: Hotel/Order
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Manage(string state, string Payment, string CreateStart, string CreateEnd, string Number, string Occupant, string StartTime, string EndTime, int pageIndex = 1)
        {
            var orders = from m in DbContext.Order
                         select m;
            //订单状态
            if (!string.IsNullOrEmpty(state))
            {
                orders = from m in orders
                         where m.State == state
                         select m;
            }
            //支付状态
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
            //订单号
            if (!string.IsNullOrEmpty(Number))
            {
                orders = from m in orders
                         where m.Number.Contains(Number)
                         select m;
            }
            //联系人
            if (!string.IsNullOrEmpty(Occupant))
            {
                var ids = from m in DbContext.Occupant where m.Name.Contains(Occupant) select m.Id;
                orders = from m in orders
                         where ids.Contains(m.Id)
                         select m;
            }
            var viewOrder = orders.OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_OrderList", viewOrder);
            return View(viewOrder);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Invoice(string Type, string Status, int pageIndex = 1)
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

            var data = invoice.OrderByDescending(m => m.Status).ThenByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_InvoiceList", data);
            return View(data);
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult OpneInvoice(string id, string Express, string ExpressNumber)
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
            var invoices = from m in DbContext.Invoice where m.Id == id select m;
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
                    Content = string.Format("开票：{0}", JsonConvert.SerializeObject(invoice)),
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

        [Login(Area = "Admin", Role = "system")]
        public ActionResult ExportOrder(string state, string Payment, string CreateStart, string CreateEnd, string Number, string Occupant, string StartTime, string EndTime)
        {
            ViewBag.state = state;
            var orders = from m in DbContext.Order
                         select m;
            //订单状态
            if (!string.IsNullOrEmpty(state))
            {
                orders = from m in orders
                         where m.State == state
                         select m;
            }
            //支付状态
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
            //订单号
            if (!string.IsNullOrEmpty(Number))
            {
                orders = from m in orders
                         where m.Number.Contains(Number)
                         select m;
            }
            //联系人
            if (!string.IsNullOrEmpty(Occupant))
            {
                var ids = from m in DbContext.Occupant where m.Name.Contains(Occupant) select m.Id;
                orders = from m in orders
                         where ids.Contains(m.Id)
                         select m;
            }
            var ExportData = orders.ToList();
            string fullName = HttpUtility.UrlEncode(DateTime.Now.ToString("yyyyMMdd") + "ORDER.xlsx", Encoding.UTF8);
            //创建workbook
            IWorkbook workbook;

            string fileExt = Path.GetExtension(fullName).ToLower();
            if (fileExt == ".xlsx")
                workbook = new XSSFWorkbook();
            else if (fileExt == ".xls")
                workbook = new HSSFWorkbook();
            else
                workbook = null;

            //创建sheet
            ISheet sheet = workbook.CreateSheet("Sheet1");

            //表头
            IRow headrow = sheet.CreateRow(0);

            headrow.CreateCell(0).SetCellValue("订单号");

            headrow.CreateCell(1).SetCellValue("来源");

            headrow.CreateCell(2).SetCellValue("下单时间");

            headrow.CreateCell(3).SetCellValue("酒店");

            headrow.CreateCell(4).SetCellValue("房型");

            headrow.CreateCell(5).SetCellValue("联系人");

            headrow.CreateCell(6).SetCellValue("联系人电话");

            headrow.CreateCell(7).SetCellValue("入住人");

            headrow.CreateCell(8).SetCellValue("入住时间");

            headrow.CreateCell(9).SetCellValue("退房时间");

            headrow.CreateCell(10).SetCellValue("间夜数");

            headrow.CreateCell(11).SetCellValue("订单金额");

            headrow.CreateCell(12).SetCellValue("支付方式");

            headrow.CreateCell(13).SetCellValue("订单状态");

            headrow.CreateCell(14).SetCellValue("支付状态");
            //表内数据
            for (int i = 0; i < ExportData.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(ExportData[i].Number);

                row.CreateCell(1).SetCellValue(ExportData[i].GuestUser.Type == 1 ? "普通客户" : "企业客户");

                row.CreateCell(2).SetCellValue(ExportData[i].CreateTime.ToString("yyyy-MM-dd HH:mm"));

                row.CreateCell(3).SetCellValue(ExportData[i].HotelName);

                row.CreateCell(4).SetCellValue(ExportData[i].RoomName);

                row.CreateCell(5).SetCellValue(ExportData[i].ApplyUser);

                row.CreateCell(6).SetCellValue(ExportData[i].ApplyPhone);

                string occ = string.Empty;
                foreach (var t in ExportData[i].Occupant)
                {
                    occ += t.Name + "/";
                }
                occ.TrimEnd('/');
                row.CreateCell(7).SetCellValue(occ);

                row.CreateCell(8).SetCellValue(ExportData[i].StartTime.ToString("yyyy-MM-dd"));

                row.CreateCell(9).SetCellValue(ExportData[i].EndTime.ToString("yyyy-MM-dd"));

                row.CreateCell(10).SetCellValue($"{ExportData[i].RoomCount} 间{((ExportData[i].StartTime - ExportData[i].EndTime).Days.ToString())}晚");

                row.CreateCell(11).SetCellValue(ExportData[i].HousingPrice);

                row.CreateCell(12).SetCellValue(Enum.GetName(typeof(PayType), Convert.ToInt32(ExportData[i].PaymentMethod)));

                row.CreateCell(13).SetCellValue(Enum.GetName(typeof(OrderState), Convert.ToInt32(ExportData[i].State)));

                row.CreateCell(14).SetCellValue(ExportData[i].Payment ? "已支付" : "未支付");
            }

            //转化为字节数组
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return File(bytes, "application/vnd.ms-excel", fullName);
        }
    }
}