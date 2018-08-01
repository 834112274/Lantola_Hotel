using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using HotelSystem.Model;
using HotelSystem.Web.Models;
using HotelSystem.Web.Models.View;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WxPayAPI;

namespace HotelSystem.Web.Controllers
{
    public class OrderController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Order
        [Login(Area = "Guest", Role = "guest")]
        public ActionResult Apply(string id, Order order, int RoomCount = 1)
        {
            var p = DbContext.Province.ToList();
            ViewBag.Province = p;
            var pid = p.First().Id;
            var c = DbContext.City.Where(m => m.ProvinceID == pid).ToList();
            ViewBag.City = c;
            pid = c.First().Id;
            ViewBag.District = DbContext.District.Where(m => m.CityID == pid).ToList();

            //订单数据初始化
            order.RoomCount = order.RoomCount == 0 ? 1 : order.RoomCount;
            order.PriceTypeId = id;

            //房价
            ViewBag.startDate = order.StartTime.ToString("yyyy-MM-dd");
            ViewBag.endDate = order.EndTime.ToString("yyyy-MM-dd");
            int days = 0;
            var room = HotelView.Single(order.HotelInfoId, id, order.RoomCount, order.StartTime, order.EndTime, out days);
            if (room == null)
            {
                ViewBag.RoomView = null;
                ViewBag.Message = "库存不足，请重新选择";
            }
            else
            {
                ViewBag.RoomView = room;
            }

            ViewBag.RoomCount = order.RoomCount;
            ViewBag.Days = days;

            //酒店信息
            var Hotel = DbContext.HotelInfo.Single(m => m.Id == order.HotelInfoId);
            order.HotelName = Hotel.Name;
            ViewBag.Hotel = Hotel;
            var slideImg = (from m in DbContext.HotelImages
                            where m.HotelInfoId == order.HotelInfoId && m.Type == 1
                            select m).ToList();
            if (slideImg.Count > 0)
            {
                ViewBag.SlideImgFirst = slideImg[0];
            }
            else
            {
                ViewBag.SlideImgFirst = new HotelImages();
            }
            return View(order);
        }

        [Login(Area = "Guest", Role = "guest")]
        [HttpPost]
        public ActionResult Create(Order order,string invoiceType,
            string ReceivingAddress, string Addressee, string Phone, string Company, string Address, string Tel, string Bank, string BankAccount, string TaxpayerNumber,
            List<string> surname, List<string> name, string head, string companyHead, string taxpayer, string Requirement,
            int Province = 0, int City = 0, int District = 0)
        {
            order.Id = Guid.NewGuid().ToString();
            order.Number = "1" + DateTime.Now.ToString("yyMMddssfff");
            //订单状态
            order.State = "1";

            order.CreateTime = DateTime.Now;
            //酒店信息
            var Hotel = DbContext.HotelInfo.Single(m => m.Id == order.HotelInfoId);

            GuestUser gu = SessionInfo.guestUser;
            int days = 0;
            var room = HotelView.Single(order.HotelInfoId, order.PriceTypeId, order.RoomCount, order.StartTime, order.EndTime, out days);
            if (room == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            //支付方式
            order.PaymentMethod = room.priceView.priceType.PaymentMethod;
            //是否支付
            order.Payment = false;
            //是否有效
            order.IsValid = true;
            //订单备注
            order.Remarks = Requirement + order.Remarks;
            order.Source = "official";
            order.GuestUserId = gu.Id;
            //记录每天价格
            List<DaysPrice> d = new List<DaysPrice>();
            foreach (var p in room.priceView.price)
            {
                DaysPrice t = new DaysPrice()
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = p.Date,
                    ConfigId = p.ConfigId,
                    OrderId = order.Id,
                    UnitPrice = p.UnitPrice,
                    PriceTypeId = p.PriceTypeId
                };
                d.Add(t);
            }
            //减少库存

            var stocks = from m in DbContext.Stock where m.RoomId == room.room.Id && m.Date >= order.StartTime && m.Date < order.EndTime select m;
            foreach(var stock in stocks)
            {
                stock.SurplusStock -=short.Parse( order.RoomCount.ToString());
            }
            //入住人
            List < Occupant > os = new List<Occupant>();
            for (int i = 0; i < surname.Count; i++)
            {
                if (!string.IsNullOrEmpty(surname[i]))
                {
                    Occupant o = new Occupant()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = surname[i] + name[i],
                        Phone = "",
                        Sex = 1,
                        OrderId = order.Id
                    };
                    os.Add(o);
                }
            }

            order.AvgPrice = room.priceView.avg;
            order.Total = order.HousingPrice = room.priceView.sum;
            order.Breakfast = room.priceView.priceType.Breakfast;
            order.Broadband = room.priceView.priceType.Broadband;
            order.BedType = room.room.BedType;
            order.RoomName = room.room.Name;
            order.PriceName = room.priceView.priceType.Name;

            //发票
            if (order.IsInvoice == 1)
            {
               
                    var existingInvoice = from m in DbContext.GuestInvoice where m.GuestUserId == gu.Id select m;
                bool isExist = false;
                //企业用户最多三个纳税识别号
                //判断纳税人识别号释放存在记录
                if (!string.IsNullOrEmpty(TaxpayerNumber))
                {
                    foreach (var t in existingInvoice)
                    {
                        if (t.TaxpayerNumber == TaxpayerNumber)
                        {
                            isExist = true;
                        }
                    }

                    //企业用户最多三个纳税识别号
                    if (gu.Type == 2 && existingInvoice.Count() >= 3 && !isExist)
                    {
                        TempData["message"] = "<strong>申请失败!</strong> 企业用户最多可以使用3个以内的纳税人识别号，请使用您曾经使用过的纳税人识别号.";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                }
                

                Invoice invoice = new Invoice()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProvinceId = Province,
                    CityId = City,
                    DistrictId = District,
                    Addressee = Addressee,
                    Phone = Phone,
                    ReceivingAddress = ReceivingAddress,
                    Company = Company,
                    Address = Address,
                    Tel = Tel,
                    Bank = Bank,
                    BankAccount = BankAccount,
                    TaxpayerNumber = TaxpayerNumber,
                    Amount = order.HousingPrice,
                    GuestUserId=order.GuestUserId,
                    CreateTime=DateTime.Now,
                    UpdateTime=DateTime.Now,
                    Status=1,
                    Type=short.Parse( invoiceType)
                };

                DbContext.Invoice.Add(invoice);

                //不存在发票抬头添加到个人发票抬头记录
                if (!isExist&& !string.IsNullOrEmpty(invoice.TaxpayerNumber))
                {
                    GuestInvoice guestInvoice = new GuestInvoice()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Company = Company,
                        Address = Address,
                        Tel = Tel,
                        Bank = Bank,
                        BankAccount = BankAccount,
                        TaxpayerNumber = TaxpayerNumber,
                        GuestUserId = gu.Id
                    };
                    DbContext.GuestInvoice.Add(guestInvoice);
                }
                order.Total += 10;
                order.InvoiceId = invoice.Id;
                if (!string.IsNullOrEmpty(head)) { invoice.Company = head; }
                if (!string.IsNullOrEmpty(companyHead)) { invoice.Company = companyHead; }
                if (!string.IsNullOrEmpty(taxpayer)) { invoice.TaxpayerNumber = taxpayer; }
            }
            else
            {
                order.IsInvoice = 0;
            }
            
            DbContext.DaysPrice.AddRange(d);
            DbContext.Occupant.AddRange(os);
            DbContext.Order.Add(order);
            try
            {
                DbContext.SaveChanges();
                string msg = string.Format("尊敬的客户，您的订单{0}，{1}，{2}入住{3}间{4}晚，订单已确认，感谢您的预订。酒店信息：{5}，{6}",
                    order.Number,
                    order.HotelName,
                    order.StartTime.ToString("yyyy年MM月dd日") + "入住",
                    order.RoomName + order.RoomCount.ToString(),
                    (order.EndTime - order.StartTime).Days.ToString(),
                    Hotel.District.DistrictName + Hotel.Adress,
                    Hotel.Tel);
                SMS.Send(msg, order.ApplyPhone, 0);
            }
            catch (Exception ex)
            {
                return View("Pay", new { id = order.Id });
            }

            if (order.PaymentMethod != 1)
            {
                return RedirectToAction("Pay", "Order", new { id = order.Id });
            }
            return RedirectToAction("Complete", "Order", new { id = order.Id });
        }

        public ActionResult Pay(string id)
        {
            var order = DbContext.Order.Single(m => m.Id == id);
            if (order.Payment)
            {
                return RedirectToAction("Complete", new { id = id });
            }
            ViewBag.Hotel = order.HotelInfo;
            if (order.HotelInfo.HotelImages.Count > 0)
            {
                ViewBag.SlideImgFirst = order.HotelInfo.HotelImages.First();
            }
            else
            {
                ViewBag.SlideImgFirst = new HotelImages();
            }

            return View(order);
        }

        public ActionResult Complete(string id)
        {
            var order = DbContext.Order.Single(m => m.Id == id);
            ViewBag.Hotel = order.HotelInfo;
            if (order.HotelInfo.HotelImages.Count > 0)
            {
                ViewBag.SlideImgFirst = order.HotelInfo.HotelImages.First();
            }
            else
            {
                ViewBag.SlideImgFirst = new HotelImages();
            }
            return View(order);
        }

        public ActionResult Detail()
        {
            return View();
        }

        [HttpPost]
        public void CheckPay(string id)
        {
            var order = DbContext.Order.Single(m => m.Id == id);
            if (order.Payment)
            {
                Response.Write("success");
                Response.End();
            }
            else
            {
                Response.Write("fail");
                Response.End();
            }
        }
    }
}