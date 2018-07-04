using HotelSystem.Helper;
using HotelSystem.Model;
using HotelSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class RoomController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Hotel/Room
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Manage(int pageIndex = 1)
        {
            var rooms = (from r in DbContext.Room
                         where r.HotelInfoId == SessionInfo.hotelUser.HotelInfoId
                         orderby r.CreateTime descending
                         select r).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_RoomList", rooms);
            return View(rooms);
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult EditRoom(string id)
        {
            ViewBag.BedType = Enum.GetValues(typeof(BedType)).Cast<BedType>();
            ViewBag.SceneryType = Enum.GetValues(typeof(SceneryType)).Cast<SceneryType>();
            ViewBag.AddBed = Enum.GetValues(typeof(AddBed)).Cast<AddBed>();
            ViewBag.Smokeless = Enum.GetValues(typeof(Smokeless)).Cast<Smokeless>();
            if (!string.IsNullOrEmpty(id))
            {
                var room = (from r in DbContext.Room
                            where r.Id == id
                            select r).First();
                return View(room);
            }
            return View();
        }

        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult EditRoom(Room room, List<HttpPostedFileBase> images)
        {
            try
            {
                if (string.IsNullOrEmpty(room.Id))
                {
                    room.Id = Guid.NewGuid().ToString();
                    room.CreateTime = DateTime.Now;
                    room.UpdateTime = DateTime.Now;

                    room.HotelInfoId = SessionInfo.hotelUser.HotelInfoId;
                    DbContext.Room.Add(room);
                }
                else
                {
                    var entity = (from r in DbContext.Room
                                  where r.Id == room.Id
                                  select r).First();
                    var cur_dt = DateTime.Now;
                    if (entity.Number != room.Number)
                    {
                        //更新库存
                        var stock = (from m in DbContext.Stock where m.RoomId == room.Id && m.Date > DateTime.Now select m).ToList();
                        //获得已使用的最大库存
                        var maxStock = stock.Max(m => m.Total - m.SurplusStock);
                        if (room.Number < maxStock)
                        {
                            //已使用库存超出新库存
                            TempData["message"] = "保存失败！新库存必须大于已使用裤子，可以配置最小库存为:" + maxStock;
                            return RedirectToAction("Manage");
                        }
                        else
                        {
                            foreach (var m in stock)
                            {
                                m.Total = room.Number;
                            }
                        }
                    }
                    //更新房型
                    entity.Name = room.Name;
                    entity.EnglishName = room.EnglishName;
                    entity.Area = room.Area;
                    entity.Storey = room.Storey;
                    entity.BedType = room.BedType;
                    entity.BedDescribe = room.BedDescribe;
                    entity.OccupancyNumber = room.OccupancyNumber;
                    entity.Scenery = room.Scenery;
                    entity.AddBed = room.AddBed;
                    entity.Number = room.Number;
                    entity.Smokeless = room.Smokeless;
                    entity.Shower = room.Shower;
                    entity.Bathtub = room.Bathtub;
                    entity.HotSpring = room.HotSpring;
                    entity.RoundBathtub = room.RoundBathtub;
                    entity.WindowBathtub = room.WindowBathtub;
                }
                string TimePath = ServerConfig.GetHotelImgRoute(SessionInfo.hotelUser.HotelInfoId+ "/Room"); 

                string pathForSaving = Server.MapPath(TimePath);

                foreach (var file in images)
                {
                    if (file != null && file.ContentLength > 0)

                    {
                        string FileType = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                        string fileName = Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                        var path = Path.Combine(pathForSaving, fileName);

                        ImgHelper.Compress(file.InputStream, path, ServerConfig.Level);//压缩保存操作
  

                        RoomImages img = new RoomImages()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Url = TimePath + fileName,
                            Sort = 0,
                            Summary = "",
                            RoomId = room.Id,
                            CreateTime = DateTime.Now
                        };
                        DbContext.RoomImages.Add(img);
                    }
                }
                DbContext.SaveChanges();
                return RedirectToAction("Manage");
            }
            catch
            {
                return View();
            }
        }
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult RoomPrice(string id)
        {
            ViewBag.Id = id;
            return View();
        }
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult GetRoomPriceType(string id)
        {
            var priceTypes = from m in DbContext.PriceType where m.RoomId == id select m.Id;
            return Json(priceTypes, JsonRequestBehavior.AllowGet);

        }
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Delete(string id)
        {
            return RedirectToAction("Manage");
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult DeleteRoomImage(string id, string roomId)
        {
            var images = DbContext.RoomImages.Where(m => m.Id == id);
            if (images.Count() > 0)
            {
                var image = images.First();
                FileInfo file = new FileInfo(Server.MapPath(image.Url));//指定文件路径
                if (file.Exists)//判断文件是否存在
                {
                    file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                    file.Delete();//删除文件
                }
                DbContext.RoomImages.Remove(image);
                DbContext.SaveChanges();
            }
            return RedirectToAction("EditRoom", new { id = roomId });
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult PriceTypeList(string id)
        {
            var list = from p in DbContext.PriceType
                       where p.RoomId == id
                       select p;
            ViewBag.Price = new Price();
            ViewBag.Message = TempData["message"] as string;
            return View(list);
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult CreatePriceType(string id)
        {
            List<SelectListItem> item = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="双早",
                    Value="双早"
                },
                new SelectListItem()
                {
                    Text="单早",
                    Value="单早"
                },
                new SelectListItem()
                {
                    Text="无",
                    Value="无"
                }
            };
            List<SelectListItem> item1 = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="免费宽带",
                    Value="免费宽带"
                },
                new SelectListItem()
                {
                    Text="宽带额外收费",
                    Value="宽带额外收费"
                },
                new SelectListItem()
                {
                    Text="无",
                    Value="无"
                }
            };
            ViewBag.Broadband = item1;
            ViewBag.PaymentMethod = from int value in Enum.GetValues(typeof(PayType))
                                    select new SelectListItem
                                    {
                                        Text = Enum.GetName(typeof(PayType), value),
                                        Value = value.ToString()
                                    };
            ViewBag.Breakfast = item;
            return View();
        }

        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult CreatePriceType(string id, PriceType priceType)
        {
            priceType.Id = Guid.NewGuid().ToString();
            priceType.RoomId = id;
            priceType.CreateTime = DateTime.Now;
            priceType.UpdateTime = DateTime.Now;
            priceType.CreateUser = SessionInfo.hotelUser.Id;
            priceType.UpdateUser = SessionInfo.hotelUser.Id;
            priceType.Remarks = "";

            //使用房型的熟练，该属性无效
            priceType.Number = "0";

            DbContext.PriceType.Add(priceType);
            DbContext.SaveChanges();

            return RedirectToAction("PriceTypeList", new { id = id });
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult AddPrice(string id,string start, string end)
        {
            if (Request.IsAjaxRequest())
            {
                var list = from p in DbContext.Price
                           where p.PriceTypeId == id
                           select p;
                if (!string.IsNullOrEmpty(start))
                {
                    DateTime Start = DateTime.Parse(start);
                    list = from m in list where m.Date >= Start select m;
                }
                if (!string.IsNullOrEmpty(end))
                {
                    DateTime End = DateTime.Parse(end);
                    list = from m in list where m.Date <= End select m;
                }
                var calendar = from p in list
                           select new
                           {
                               allDay = true,
                               title = p.PriceType.Name + "：￥" + p.UnitPrice.ToString(),
                               end = p.Date,
                               start = p.Date,
                               price = p.UnitPrice.ToString(),
                               id = p.Id,
                               priceTypeId = p.PriceTypeId,
                               date = p.Date,
                               status = p.Status,
                               color = p.Status == 1 ? "#5cb85c" : "#ddd",   // a non-ajax option
                               textColor = "#fff" // a non-ajax option
                           };
                return Json(calendar, JsonRequestBehavior.AllowGet);
            }

            //ViewBag.CalendarData = JsonConvert.SerializeObject(list);
            ViewBag.PriceTypeId = id;
            ViewBag.Message = TempData["message"] as string;
            return View();
        }

        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult AddPrice(string id, double UnitPrice, DateTime StartTime, DateTime EndTime, List<int> week)
        {
            if (DateTime.Compare(StartTime, DateTime.Now) < 0)
            {
                //开始日期必须大于当前日期
                TempData["message"] = "<strong>保存失败!</strong> 开始日期必须大于当前日期.";
                return RedirectToAction("AddPrice", new { id = id });
            }
            if (week == null)
            {
                //必须选择一天
                TempData["message"] = "<strong>保存失败!</strong> 每周至少选择一天.";
                return RedirectToAction("AddPrice", new { id = id });
            }
            //获得已存在价格
            var cur_price = (from u in DbContext.Price
                             where u.Date >= StartTime && u.Date <= EndTime && u.PriceTypeId == id
                             select u).ToList();

            //获得房型库存总
            var room = (from m in DbContext.PriceType where m.Id == id select m.Room).First();
            DateTime d = StartTime;
            string configId = Guid.NewGuid().ToString();
            var n = (EndTime - StartTime).Days;
            for (var i = 0; i <= n; i++)
            {
                //判断周几

                if (week.Contains((int)d.DayOfWeek))
                {
                    var cur = cur_price.Where(m => m.Date == d);
                    if (cur.Count() > 0)
                    {
                        //更新存在价格
                        var exist = cur.First();
                        exist.UnitPrice = UnitPrice;
                    }
                    else
                    {
                        //添加价格
                        Price p = new Price()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Date = d,
                            PriceTypeId = id,
                            UnitPrice = UnitPrice,
                            ConfigId = configId,
                            //下两属性暂时无用
                            Stock = room.Number,
                            SurplusStock = room.Number,

                            Status = 1
                        };
                        DbContext.Price.Add(p);
                        //添加库存
                        Stock s = new Stock()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Date = d,
                            Total = room.Number,
                            SurplusStock = room.Number,
                            Status = 1,
                            RoomId = room.Id
                        };
                        DbContext.Stock.Add(s);
                    }
                }
                d = d.AddDays(1);
            }
            DbContext.SaveChanges();
            TempData["message"] = "<strong>保存成功!</strong> .";
            return RedirectToAction("AddPrice", new { id = id });
        }

        /// <summary>
        /// 关闭房型
        /// </summary>
        /// <param name="id">房型ID</param>
        /// <param name="date">日期</param>
        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult ChangeRoomStatus(string id, DateTime start, DateTime end)
        {
            //关闭房型更新库存状态
            var stock = (from m in DbContext.Stock where m.RoomId == id && m.Date >= start && m.Date <= end select m).ToList();
            if (stock.Count() > 0)
            {
                foreach (var m in stock)
                {
                    m.Status = 0;
                }
                DbContext.SaveChanges();
                return Json(new { result = "success", msg = "修改成功!" });
            }
            else
            {
                return Json(new { result = "fail", msg = "修改失败!未找到" + start.ToString("yyyy-MM-dd") + "至" + end.ToString("yyyy-MM-dd") + "的库存." });
            }
        }

        /// <summary>
        /// 关闭价格
        /// </summary>
        /// <param name="id">价格类型ID</param>
        /// <param name="date">日期</param>
        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult ChangePriceTypeStatus(string id, DateTime start, DateTime end, short status)
        {
            //关闭价格，更新每日价格状态
            var dayPrice = (from m in DbContext.Price where m.PriceTypeId == id && m.Date >= start && m.Date <= end select m).ToList();
            if (dayPrice.Count() > 0)
            {
                foreach (var m in dayPrice)
                {
                    m.Status = status;
                }
                DbContext.SaveChanges();
                return Json(new { result = "success", msg = "修改成功!" });
            }
            else
            {
                return Json(new { result = "fail", msg = "修改失败!未找到" + start.ToString("yyyy-MM-dd") + "至" + end.ToString("yyyy-MM-dd") + "日的价格." });
            }
        }
    }
}