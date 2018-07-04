using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class ActivityController : Controller
    {
        // GET: Hotel/Activity
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Index()
        {
            var activit = (from m in Query.db.Activity
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
        public ActionResult Create(Activity activity,HttpPostedFileBase Images)
        {

                // TODO: Add insert logic here
                activity.Id = Guid.NewGuid().ToString();
                activity.CreateTime = DateTime.Now;
                activity.HotelInfoId = SessionInfo.hotelUser.HotelInfoId;
                string TimePath = "/HotelImages/" + SessionInfo.hotelUser.HotelInfoId + "/";

                if (Images != null && Images.ContentLength > 0)
                {
                    string FileType = Images.FileName.Substring(Images.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                    string imgName = Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                    Images.SaveAs(Server.MapPath(TimePath) + imgName); //保存操作
                    activity.Images = TimePath + imgName;
                }
                else
                {
                    activity.Images = "";
                }
                Query.db.Activity.Add(activity);
                Query.db.SaveChanges();

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
