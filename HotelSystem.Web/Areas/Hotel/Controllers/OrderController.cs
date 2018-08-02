using HotelSystem.Model;
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

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class OrderController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Hotel/Order
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Manage(string state,string Payment, string CreateStart, string CreateEnd, string Number, string Occupant, string StartTime, string EndTime, int pageIndex = 1)
        {
            ViewBag.state = state;
            var orders = from m in DbContext.Order
                         where m.HotelInfoId == SessionInfo.hotelUser.HotelInfoId
                         select m;
            //订单状态
            if (!string.IsNullOrEmpty(state))
            {
                orders = from m in orders
                         where m.State == state
                         select m;
            }

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
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult ExportOrder(string state, string Payment, string CreateStart, string CreateEnd, string Number, string Occupant, string StartTime, string EndTime)
        {
            var orders = from m in DbContext.Order
                         where m.HotelInfoId == SessionInfo.hotelUser.HotelInfoId
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