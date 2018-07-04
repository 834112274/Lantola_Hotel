using HotelSystem.Model;
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
        // GET: Hotel/Room
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Manage(int pageIndex = 1)
        {
            var rooms = (from r in Query.db.Room
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
                var room = (from r in Query.db.Room
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
                    Query.db.Room.Add(room);
                }
                else
                {
                    var entity = (from r in Query.db.Room
                                  where r.Id == room.Id
                                  select r).First();
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
                string TimePath = "/HotelImages/" + SessionInfo.hotelUser.HotelInfo.Id + "/Room/";
                string pathForSaving = Server.MapPath(TimePath);
                if (!Directory.Exists(pathForSaving))//判断文件夹是否存在
                {
                    Directory.CreateDirectory(pathForSaving);//不存在则创建文件夹
                }
                foreach (var file in images)
                {
                    if (file != null && file.ContentLength > 0)

                    {
                        string FileType = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                        string fileName = Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                        var path = Path.Combine(pathForSaving, fileName);

                        file.SaveAs(path);

                        RoomImages img = new RoomImages()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Url = TimePath + fileName,
                            Sort = 0,
                            Summary = "",
                            RoomId = room.Id,
                            CreateTime = DateTime.Now
                        };
                        Query.db.RoomImages.Add(img);
                    }
                }
                Query.db.SaveChanges();
                return RedirectToAction("Manage");
            }
            catch
            {
                return View();
            }
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Delete(string id)
        {
            return RedirectToAction("Manage");
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult DeleteRoomImage(string id, string roomId)
        {
            var images = Query.db.RoomImages.Where(m => m.Id == id);
            if (images.Count() > 0)
            {
                var image = images.First();
                FileInfo file = new FileInfo(Server.MapPath(image.Url));//指定文件路径
                if (file.Exists)//判断文件是否存在
                {
                    file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                    file.Delete();//删除文件
                }
                Query.db.RoomImages.Remove(image);
                Query.db.SaveChanges();
            }
            return RedirectToAction("EditRoom", new { id = roomId });
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult PriceList(string id)
        {
            var list = from p in Query.db.Price
                       where p.RoomId == id
                       select p;
            ViewBag.Price = new Price();
            return View(list);
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult CreatePrice(string id)
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
                    Value="双早"
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
        public ActionResult CreatePrice(string id,Price price)
        {
            price.Id = Guid.NewGuid().ToString();
            price.RoomId = id;
            price.CreateTime = DateTime.Now;
            price.UpdateTime = DateTime.Now;
            Query.db.Price.Add(price);
            Query.db.SaveChanges();
            return RedirectToAction("PriceList", new { id = id });
        }
    }
}