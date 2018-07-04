using HotelSystem.Helper;
using HotelSystem.Model;
using HotelSystem.Web.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class ActivityController : Controller
    {
        private DBModelContainer DbContext = new DBModelContainer();

        // GET: Hotel/Activity
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Index()
        {
            var activit = (from m in DbContext.Activity
                           where m.HotelInfoId == SessionInfo.hotelUser.HotelInfoId
                           select m).ToList();
            return View(activit);
        }

        // GET: Hotel/Activity/Details/5
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Hotel/Activity/Create
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hotel/Activity/Create
        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult Create(Activity activity, HttpPostedFileBase Images)
        {
            // TODO: Add insert logic here
            activity.Id = Guid.NewGuid().ToString();
            activity.CreateTime = DateTime.Now;
            activity.HotelInfoId = SessionInfo.hotelUser.HotelInfoId;
            string TimePath = ServerConfig.HotelImgRoute + SessionInfo.hotelUser.HotelInfoId + "/";
            if (!Directory.Exists(Server.MapPath(TimePath)))//判断文件夹是否存在
            {
                Directory.CreateDirectory(Server.MapPath(TimePath));//不存在则创建文件夹
            }
            if (Images != null && Images.ContentLength > 0)
            {
                string FileType = Images.FileName.Substring(Images.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                string imgName = Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名

                ImgHelper.Compress(Images.InputStream, Server.MapPath(TimePath) + imgName, ServerConfig.Level);//压缩保存操作

                activity.Images = TimePath + imgName;
            }
            else
            {
                activity.Images = "";
            }
            DbContext.Activity.Add(activity);
            DbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Hotel/Activity/Edit/5
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Hotel/Activity/Edit/5
        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hotel/Activity/Delete/5
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Hotel/Activity/Delete/5
        [Login(Area = "Hotel", Role = "hotel")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}