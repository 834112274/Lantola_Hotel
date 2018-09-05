using HotelSystem.Helper;
using HotelSystem.Model;
using HotelSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class InfoController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Hotel/Info
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Index()
        {
            var hotel = (from h in DbContext.HotelInfo
                        where h.Id == SessionInfo.hotelUser.HotelInfoId
                        select h).First();
            var ProvinceId = hotel.City.ProvinceID;
            ViewBag.Province = DbContext.Province.ToList();
            ViewBag.City = DbContext.City.Where(m => m.ProvinceID == ProvinceId).ToList();
            ViewBag.District = DbContext.District.Where(m => m.CityID == hotel.CityId).ToList();
            ViewBag.ProvinceId = ProvinceId;
            return View(hotel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hotel"></param>
        /// <param name="image"></param>
        /// <param name="imageId"></param>
        /// <param name="summary"></param>
        /// <returns></returns>
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult ChangeInfo(HotelInfo hotel, List<HttpPostedFileBase> image, List<string> imageId, List<string> summary,long ProvinceId)
        {
            ViewBag.Province = DbContext.Province.ToList();
            ViewBag.City = DbContext.City.Where(m => m.ProvinceID == ProvinceId).ToList();
            ViewBag.District = DbContext.District.Where(m => m.CityID == hotel.CityId).ToList();
            ViewBag.ProvinceId = ProvinceId;
            var h = DbContext.HotelInfo.Where(t => t.Id == hotel.Id);
            if (h.Count() > 0)
            {
                var hotelInfo = h.First();
                hotelInfo.Name = hotel.Name;
                hotelInfo.Email = hotel.Email;
                hotelInfo.EnglishName = hotel.EnglishName;
                hotelInfo.DomainName = hotel.DomainName;
                hotelInfo.CityId = hotel.CityId;
                hotelInfo.DistrictId = hotel.DistrictId;
                hotelInfo.Adress = hotel.Adress;
                hotelInfo.OpeningTime = hotel.OpeningTime;
                hotelInfo.RenovationTime = hotel.RenovationTime;
                hotelInfo.RoomCount = hotel.RoomCount;
                hotelInfo.Synopsis = hotel.Synopsis;
                hotelInfo.Star = hotel.Star;
                hotelInfo.Tel = hotel.Tel;
                string TimePath = ServerConfig.GetHotelImgRoute(SessionInfo.hotelUser.HotelInfoId);
                for (var i = 0; i < image.Count; i++)
                {
                    if (image[i] != null)
                    {
                        string FileType = image[i].FileName.Substring(image[i].FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                        string imgName = Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                        ImgHelper.Compress(image[i].InputStream, Server.MapPath(TimePath) + imgName, ServerConfig.Level);//压缩保存操作

                        if (string.IsNullOrEmpty(imageId[i]))
                        {
                            HotelImages img = new HotelImages()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Url = TimePath + imgName,
                                Type = summary[i] == "LOGO" ? 0 : 1,
                                Summary = summary[i],
                                CreateTime = DateTime.Now,
                                HotelInfoId = hotelInfo.Id,
                                Sort = i
                            };
                            DbContext.HotelImages.Add(img);
                        }
                        else
                        {
                            var img = hotelInfo.HotelImages.Where(m => m.Id == imageId[i]).First();
                            FileInfo file = new FileInfo(Server.MapPath(img.Url));//指定文件路径
                            if (file.Exists)//判断文件是否存在
                            {
                                file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                                file.Delete();//删除文件
                            }
                            img.Url = TimePath + imgName;
                            img.CreateTime = DateTime.Now;
                        }
                    }
                }
                DbContext.SaveChanges();
            }
            ViewBag.Message = "修改成功!";
            return View("Index", h.First());
        }

        /// <summary>
        /// 设施及政策
        /// </summary>
        /// <returns></returns>
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Policy()
        {
            ViewBag.Policy0 = DbContext.Policy.Where(p => p.Type == 1);
            ViewBag.Policy1 = DbContext.Policy.Where(p => p.Type == 2);
            ViewBag.Policy2 = DbContext.Policy.Where(p => p.Type == 3);
            ViewBag.HotelPolicy= DbContext.HotelPolicy.Where(m=>m.HotelInfoId== SessionInfo.hotelUser.HotelInfoId).ToList();
            var s = DbContext.HotelInfo.Where(p => p.Id == SessionInfo.hotelUser.HotelInfoId).First().HotelService;
            if (s == null)
            {
                ViewBag.Service = new HotelService();
            }
            else
            {
                ViewBag.Service = s;
            }
            return View();
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult ChangePolicy(List<string> policy, HotelService service, List<string> CreditCard)
        {
            if (policy != null)
            {
                var cur = from m in DbContext.HotelPolicy where m.HotelInfoId == SessionInfo.hotelUser.HotelInfoId select m;
                DbContext.HotelPolicy.RemoveRange(cur);
                foreach (var id in policy)
                {
                    var p = new HotelPolicy()
                    {
                        Id = Guid.NewGuid().ToString(),
                        HotelInfoId = SessionInfo.hotelUser.HotelInfoId,
                        PolicyId = id
                    };
                    DbContext.HotelPolicy.Add(p);
                }
            }

            var hotel = DbContext.HotelInfo.Where(h => h.Id == SessionInfo.hotelUser.HotelInfoId).First();
            if (hotel.HotelService == null)
            {
                service.Id = Guid.NewGuid().ToString();
                hotel.HotelService = service;
            }
            else
            {
                hotel.HotelService.Buffet = service.Buffet;
                hotel.HotelService.CheckInTime = service.CheckInTime;
                hotel.HotelService.LeaveTime = service.LeaveTime;
                hotel.HotelService.Pet = service.Pet;
                hotel.HotelService.Buffet = service.Buffet;
            }
            string card = string.Empty;
            if (CreditCard != null)
            {
                foreach (var c in CreditCard)
                {
                    card += c + ",";
                }
            }

            hotel.HotelService.CreditCard = card.TrimEnd(',');
            DbContext.SaveChanges();
            return RedirectToAction("Policy");
        }

        /// <summary>
        /// 餐饮美食
        /// </summary>
        /// <returns></returns>
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Restaurant()
        {
            var restaurants = DbContext.Restaurant.Where(g => g.HotelInfo.Id == SessionInfo.hotelUser.HotelInfoId);
            if (restaurants.Count() > 0)
            {
                var r = restaurants.First();
                return View(r);
            }
            return View();
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult ChangeRestaurant(Restaurant r, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3)
        {
            string TimePath = ServerConfig.GetHotelImgRoute(SessionInfo.hotelUser.HotelInfoId);

            if (Image1 != null && Image1.ContentLength > 0)
            {
                string FileType = Image1.FileName.Substring(Image1.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                string imgName = Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名

                ImgHelper.Compress(Image1.InputStream, Server.MapPath(TimePath) + imgName, ServerConfig.Level);//压缩保存操作


                r.Image1 = TimePath + imgName;
            }
            if (Image2 != null && Image2.ContentLength > 0)
            {
                string FileType = Image2.FileName.Substring(Image2.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                string imgName = Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名

                ImgHelper.Compress(Image2.InputStream, Server.MapPath(TimePath) + imgName, ServerConfig.Level);//压缩保存操作

                r.Image2 = TimePath + imgName;
            }
            if (Image3 != null && Image3.ContentLength > 0)
            {
                string FileType = Image3.FileName.Substring(Image3.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                string imgName = Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名

                ImgHelper.Compress(Image3.InputStream, Server.MapPath(TimePath) + imgName, ServerConfig.Level);//压缩保存操作
                r.Image3 = TimePath + imgName;
            }
            if (string.IsNullOrEmpty(r.Id))
            {
                r.Id = Guid.NewGuid().ToString();
                var h = DbContext.HotelInfo.Where(a => a.Id == SessionInfo.hotelUser.HotelInfoId).First();
                h.Restaurant = r;
            }
            else
            {
                var t = DbContext.Restaurant.Where(a => a.Id == r.Id).First();
                if (!string.IsNullOrEmpty(r.Image1))
                {
                    FileInfo file = new FileInfo(Server.MapPath(t.Image1));//指定文件路径
                    if (file.Exists)//判断文件是否存在
                    {
                        file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                        file.Delete();//删除文件
                    }
                    t.Image1 = r.Image1;
                }
                   
                if (!string.IsNullOrEmpty(r.Image2))
                {
                    FileInfo file = new FileInfo(Server.MapPath(t.Image2));//指定文件路径
                    if (file.Exists)//判断文件是否存在
                    {
                        file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                        file.Delete();//删除文件
                    }
                    t.Image2 = r.Image2;
                }
                
                if (!string.IsNullOrEmpty(r.Image3))
                {
                    FileInfo file = new FileInfo(Server.MapPath(t.Image3));//指定文件路径
                    if (file.Exists)//判断文件是否存在
                    {
                        file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                        file.Delete();//删除文件
                    }
                    t.Image3 = r.Image3;
                }
                t.Describe = r.Describe;
                t.BreakfastTime = r.BreakfastTime;
                t.LunchTime = r.LunchTime;
                t.DinnerTime = r.DinnerTime;
                t.Seat = r.Seat;
                t.Clothing = r.Clothing;
                t.Place = r.Place;
                t.Reserve = r.Reserve;
            }
            DbContext.SaveChanges();
            return RedirectToAction("Restaurant");
        }

        /// <summary>
        /// 到达指引
        /// </summary>
        /// <returns></returns>
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Map()
        {
            var guide = DbContext.Guide.Where(g => g.HotelInfoId == SessionInfo.hotelUser.HotelInfoId);
            return View(guide);
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult AddMap(Guide guide)
        {
            guide.Id = Guid.NewGuid().ToString();
            guide.HotelInfoId = SessionInfo.hotelUser.HotelInfoId;
            DbContext.Guide.Add(guide);
            DbContext.SaveChanges();
            return RedirectToAction("Map");
        }

        public ActionResult DeleteMap(string id)
        {
            var guide = DbContext.Guide.Where(g => g.Id == id);
            if (guide.Count() > 0)
            {
                DbContext.Guide.Remove(guide.First());
                DbContext.SaveChanges();
            }
            return RedirectToAction("Map");
        }
    }
}