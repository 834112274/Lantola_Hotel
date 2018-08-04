using HotelSystem.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class SettlementController : Controller
    {
        private DBModelContainer DbContext = new DBModelContainer();

        // GET: Admin/Settlement
        [Login(Area = "Admin", Role = "system")]
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
            if (settlementType != "1")
            {
                orders = from m in orders where m.SettlementId == null select m;
            }
            else
            {
                orders = from m in orders where m.SettlementId != null select m;
            }

            var viewOrder = orders.OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_OrderList", viewOrder);
            return View(viewOrder);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult HotelList(string start, string end, string searchHotel, int pageIndex = 1)
        {
            ViewBag.Message = TempData["message"] as string;
            DateTime Start = DateTime.Parse(string.IsNullOrEmpty(start) ? DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") : start);
            DateTime End = DateTime.Parse(string.IsNullOrEmpty(end) ? DateTime.Now.ToString("yyyy-MM-dd") : end);
            if (string.IsNullOrEmpty(searchHotel))
            {
                searchHotel = "";
            }
            var orders = (from m in DbContext.Order
                          where m.EndTime >= Start
                          && m.EndTime <= End
                          && m.SettlementId == null
                          && m.Payment == true
                          && m.State != "0"
                          && m.IsValid == true
                          && m.HotelName.Contains(searchHotel)
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
                TempData["message"] = "<strong>结算失败!</strong> 未找到酒店.";
                return RedirectToAction("HotelList", new { start = start.ToString("yyyy-MM-dd"), end = end.ToString("yyyy-MM-dd") });
            }
            var hotelInfo = DbContext.HotelInfo.Single(m => m.Id == hotel);
            var orders = from m in DbContext.Order
                         where m.EndTime >= start && m.EndTime <= end && m.HotelInfoId == hotel && m.SettlementId == null
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
                HotelName = hotelInfo.Name,
                UsersId = SessionInfo.systemUser.Id,
                IsPay = 0,
                PayTime = DateTime.Now
            };
            foreach (var order in orders)
            {
                order.SettlementId = settlenment.Id;
            }
            UserLog log = new UserLog()
            {
                Id = Guid.NewGuid().ToString(),
                Level = "info",
                TypeName = "settlenment",
                UserId = SessionInfo.systemUser.Id,
                UserName = SessionInfo.systemUser.Name,
                UserType = "system",
                Content = string.Format("账单结算： 酒店：{0};结算订单数：{1};结算金额：{2};结算起始日期：{3};",
                settlenment.HotelName,
                settlenment.OrderCount,
                settlenment.Amount,
                settlenment.StartTime.ToString("yyyy-MM-dd") + "~" + settlenment.EndTime.ToString("yyyy-MM-dd")),
                CreateTime = DateTime.Now
            };
            DbContext.UserLog.Add(log);
            DbContext.Settlement.Add(settlenment);
            DbContext.SaveChanges();
            TempData["message"] = string.Format("<strong>结算成功!</strong> 酒店：{0}，结算订单数：{1},结算金额：{2}.", settlenment.HotelName, settlenment.OrderCount, settlenment.Amount);
            return RedirectToAction("HotelList", new { start = start.ToString("yyyy-MM-dd"), end = end.ToString("yyyy-MM-dd") });
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult History(string start, string end, string searchHotel, int pageIndex = 1)
        {
            ViewBag.Message = TempData["message"] as string;
            DateTime Start = DateTime.Parse(string.IsNullOrEmpty(start) ? DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") : start);
            DateTime End = DateTime.Parse(string.IsNullOrEmpty(end) ? DateTime.Now.ToString("yyyy-MM-dd") : end).AddDays(1);
            if (string.IsNullOrEmpty(searchHotel))
            {
                searchHotel = "";
            }
            var viewData = (from m in DbContext.Settlement where m.CreateTime >= Start && m.CreateTime <= End && m.HotelName.Contains(searchHotel) select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HistoryList", viewData);
            return View(viewData);
        }
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Pay(string id)
        {
            var bill = DbContext.Settlement.Single(m=>m.Id==id);
            bill.IsPay = 1;
            bill.PayTime = DateTime.Now;
            DbContext.SaveChanges();
            TempData["message"] = $"<strong>确认转账成功!</strong> 酒店：{bill.HotelName},转账时间：{bill.PayTime.ToString("yyyy-MM-dd HH:mm")},金额:{bill.Amount}.";
            return RedirectToAction("History");
        }
        [Login(Area = "Admin", Role = "system")]
        public ActionResult ExportBill(string history,string start, string end, string searchHotel,string hotelId)
        {
            DateTime Start = DateTime.Parse(string.IsNullOrEmpty(start) ? DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") : start);
            DateTime End = DateTime.Parse(string.IsNullOrEmpty(end) ? DateTime.Now.ToString("yyyy-MM-dd") : end);
            if (string.IsNullOrEmpty(searchHotel))
            {
                searchHotel = "";
            }
            if (string.IsNullOrEmpty(hotelId))
            {
                hotelId = "";
            }
            else
            {
                hotelId = SessionInfo.hotel.Id;
            }
            List<Settlement> ExprotData;
            if (history == "1")
            {
                End = End.AddDays(1);
                ExprotData = (from m in DbContext.Settlement where m.CreateTime >= Start && m.CreateTime <= End && m.HotelName.Contains(searchHotel)&&m.HotelInfoId.Contains(hotelId) select m).ToList();
            }
            else
            {
                ExprotData = (from m in DbContext.Order
                              where m.EndTime >= Start
                              && m.EndTime <= End
                              && m.SettlementId == null
                              && m.Payment == true
                              && m.State != "0"
                              && m.IsValid == true
                              && m.HotelName.Contains(searchHotel)
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
                              }).ToList();
            }
            

            string fullName = HttpUtility.UrlEncode(DateTime.Now.ToString("yyyyMMdd") + "BILL.xlsx", Encoding.UTF8);
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

            ICell createTimeCell = headrow.CreateCell(0);
            createTimeCell.SetCellValue("创建时间");

            ICell startCell = headrow.CreateCell(1);
            startCell.SetCellValue("账单开始时间");

            ICell endCell = headrow.CreateCell(2);
            endCell.SetCellValue("账单结束时间");

            ICell hotelCell = headrow.CreateCell(3);
            hotelCell.SetCellValue("酒店");

            ICell amountCell = headrow.CreateCell(4);
            amountCell.SetCellValue("账单金额");

            ICell orderCountCell = headrow.CreateCell(5);
            orderCountCell.SetCellValue("订单数");

            if (history == "1")
            {
                headrow.CreateCell(6).SetCellValue("转账时间");
            }

            //表内数据
            for (int i = 0; i < ExprotData.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                ICell createTimeCellItem = row.CreateCell(0);
                createTimeCellItem.SetCellValue(ExprotData[i].CreateTime.ToString("yyyy-MM-dd HH:mm"));

                ICell startCellItem = row.CreateCell(1);
                startCellItem.SetCellValue(ExprotData[i].StartTime.ToString("yyyy-MM-dd"));

                ICell endCellItem = row.CreateCell(2);
                endCellItem.SetCellValue(ExprotData[i].EndTime.ToString("yyyy-MM-dd"));

                ICell hotelCellItem = row.CreateCell(3);
                hotelCellItem.SetCellValue(ExprotData[i].HotelName);

                ICell amountCellItem = row.CreateCell(4);
                amountCellItem.SetCellValue(ExprotData[i].Amount);

                ICell orderCountCellItem = row.CreateCell(5);
                orderCountCellItem.SetCellValue(ExprotData[i].OrderCount);
                if (history == "1")
                {
                    row.CreateCell(6).SetCellValue(ExprotData[i].PayTime.ToString("yyyy-MM-dd HH:mm"));
                }
            }

            //转化为字节数组
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return File(bytes, "application/vnd.ms-excel", fullName);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult ExportOrder(string hotel, DateTime start, DateTime end, string settlementType)
        {
            var orders = from m in DbContext.Order
                         where m.EndTime >= start && m.EndTime <= end && m.HotelInfoId == hotel
                         && m.Payment == true
                         && m.State != "0"
                         && m.IsValid == true
                         select m;
            if (settlementType != "1")
            {
                orders = from m in orders where m.SettlementId == null select m;
            }
            else
            {
                orders = from m in orders where m.SettlementId != null select m;
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

            headrow.CreateCell(15).SetCellValue("结算状态");
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

                string Occupant = string.Empty;
                foreach (var t in ExportData[i].Occupant)
                {
                    Occupant += t.Name + "/";
                }
                Occupant=Occupant.TrimEnd('/');
                row.CreateCell(7).SetCellValue(Occupant);

                row.CreateCell(8).SetCellValue(ExportData[i].StartTime.ToString("yyyy-MM-dd"));

                row.CreateCell(9).SetCellValue(ExportData[i].EndTime.ToString("yyyy-MM-dd"));

                row.CreateCell(10).SetCellValue($"{ExportData[i].RoomCount} 间{((ExportData[i].StartTime - ExportData[i].EndTime).Days.ToString())}晚");

                row.CreateCell(11).SetCellValue(ExportData[i].HousingPrice);

                row.CreateCell(12).SetCellValue(Enum.GetName(typeof(PayType), Convert.ToInt32(ExportData[i].PaymentMethod)));

                row.CreateCell(13).SetCellValue(Enum.GetName(typeof(OrderState), Convert.ToInt32(ExportData[i].State)));

                row.CreateCell(14).SetCellValue(ExportData[i].Payment ? "已支付" : "未支付");

                row.CreateCell(15).SetCellValue(ExportData[i].SettlementId == null ? "未结算" : "已结算");
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