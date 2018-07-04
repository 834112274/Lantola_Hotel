using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class FeatureController : Controller
    {
        // GET: Admin/Feature
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Hotel()
        {
            var feature = from f in Query.db.Feature
                          select f;
            return View(feature);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Edit()
        {
            ViewBag.Type = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="特色酒店",Value="1"
                },
                new SelectListItem()
                {
                    Text="标签酒店",Value="2"
                }
            };
            return View();
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult Edit(Feature feature)
        {
            if (string.IsNullOrEmpty(feature.Id))
            {
                feature.CreateTime = DateTime.Now;
                feature.UpdateTime = DateTime.Now;
                feature.Id = Guid.NewGuid().ToString();
                feature.Ico = "";
                Query.db.Feature.Add(feature);
            }
            else
            {
                var t = Query.db.Feature.Single(m => m.Id == feature.Id);
                t.Name = feature.Name;
                t.Summary = feature.Summary;
                t.Ico = feature.Ico;
                t.Valid = feature.Valid;
            }
            Query.db.SaveChanges();
            return RedirectToAction("Hotel");
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult DeleteFeature(string id)
        {
            var t = Query.db.Feature.Single(m => m.Id == id);
            Query.db.FeatureHotel.RemoveRange(t.FeatureHotel);
            Query.db.Feature.Remove(t);
            Query.db.SaveChanges();
            return RedirectToAction("Hotel");
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult HotelList(string id)
        {
            var hotels = from f in Query.db.FeatureHotel
                         where f.FeatureId == id
                         select f;
            return View(hotels);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult EditHotel(string featureId)
        {
            ViewBag.FeatureId = featureId;
            ViewBag.Hotels = (from h in Query.db.HotelInfo
                              where h.ExamineStatus == 1 && h.Valid == 1
                              select h).ToList();
            return View(new FeatureHotel());
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult EditHotel(FeatureHotel featureHotel, HttpPostedFileBase ImgUrl)
        {
            string id = RouteData.Values["id"].ToString();
            string TimePath = "/Images/Feature/";//获取上传路径的物理地址
            if (!Directory.Exists(Server.MapPath(TimePath)))//判断文件夹是否存在
            {
                Directory.CreateDirectory(Server.MapPath(TimePath));//不存在则创建文件夹
            }

            if (ImgUrl != null && ImgUrl.ContentLength > 0)
            {
                string FileType = ImgUrl.FileName.Substring(ImgUrl.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                featureHotel.ImgUrl = TimePath + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                ImgUrl.SaveAs(Server.MapPath(featureHotel.ImgUrl)); //保存操作
            }
            if (string.IsNullOrEmpty(featureHotel.Id))
            {
                featureHotel.Id = Guid.NewGuid().ToString();
                featureHotel.FeatureId = id;
                Query.db.FeatureHotel.Add(featureHotel);
            }
            else
            {
                var t = Query.db.FeatureHotel.Single(f => f.Id == featureHotel.Id);
                t.Title = featureHotel.Title;
                t.Summary = featureHotel.Summary;
                t.Valid = featureHotel.Valid;
                t.Sort = featureHotel.Sort;
            }
            Query.db.SaveChanges();
            return RedirectToAction("HotelList", new { id = id });
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult DeleteHotel(string id)
        {
            var t = Query.db.FeatureHotel.Single(f => f.Id == id);
            FileInfo file = new FileInfo(Server.MapPath(t.ImgUrl));//指定文件路径
            if (file.Exists)//判断文件是否存在
            {
                file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                file.Delete();//删除文件
            }
            var FeatureId = t.FeatureId;
            Query.db.FeatureHotel.Remove(t);
            Query.db.SaveChanges();
            return RedirectToAction("HotelList", new { id = FeatureId });
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Cooperative()
        {
            var cooperative = from f in Query.db.Cooperative
                              select f;
            return View(cooperative);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult EditCooperative()
        {
            return View();
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult EditCooperative(Cooperative cooperative, HttpPostedFileBase LogoUrl)
        {
            string TimePath = "/Images/WebSite/";//获取上传路径的物理地址
            if (!Directory.Exists(Server.MapPath(TimePath)))//判断文件夹是否存在
            {
                Directory.CreateDirectory(Server.MapPath(TimePath));//不存在则创建文件夹
            }

            if (LogoUrl != null && LogoUrl.ContentLength > 0)
            {
                string FileType = LogoUrl.FileName.Substring(LogoUrl.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                cooperative.LogoUrl = TimePath + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                LogoUrl.SaveAs(Server.MapPath(cooperative.LogoUrl)); //保存操作
            }
            if (string.IsNullOrEmpty(cooperative.Id))
            {
                cooperative.Id = Guid.NewGuid().ToString();
                cooperative.CreateTime = DateTime.Now;
                Query.db.Cooperative.Add(cooperative);
            }
            else
            {
                var t = Query.db.Cooperative.Single(m => m.Id == cooperative.Id);
                FileInfo file = new FileInfo(Server.MapPath(t.LogoUrl));//指定文件路径
                if (file.Exists)//判断文件是否存在
                {
                    file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                    file.Delete();//删除文件
                }
                t.LogoUrl = cooperative.LogoUrl;
                t.Name = cooperative.Name;
                t.Summary = cooperative.Summary;
                t.Url = cooperative.Url;
            }
            Query.db.SaveChanges();
            return RedirectToAction("Cooperative");
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult DeleteCooperative(string id)
        {
            var t = Query.db.Cooperative.Single(f => f.Id == id);
            FileInfo file = new FileInfo(Server.MapPath(t.LogoUrl));//指定文件路径
            if (file.Exists)//判断文件是否存在
            {
                file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                file.Delete();//删除文件
            }
            Query.db.Cooperative.Remove(t);
            Query.db.SaveChanges();
            return RedirectToAction("Cooperative");
        }
    }
}