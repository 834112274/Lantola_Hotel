﻿using HotelSystem.Helper;
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
    public class AlbumController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Hotel/Album
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Manage(int pageIndex = 1)
        {
            var images = DbContext.HotelImages.Where(m => m.HotelInfoId == SessionInfo.hotelUser.HotelInfo.Id && (m.Type == 2 || m.Type == 3)).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_List", images);
            return View(images);
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult UpLoad(string Type,List<HttpPostedFileBase> albumImages)
        {
            string TimePath = ServerConfig.GetHotelImgRoute(SessionInfo.hotelUser.HotelInfoId);

            string pathForSaving = Server.MapPath(TimePath);

            foreach (HttpPostedFileBase file in albumImages)

            {
                if (file != null && file.ContentLength > 0)

                {
                    string FileType = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                    string fileName = Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                    var path = Path.Combine(pathForSaving, fileName);

                    ImgHelper.Compress(file.InputStream, path, ServerConfig.Level);//压缩保存操作
  

                    HotelImages img = new HotelImages()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Url = TimePath + fileName,
                        Type = int.Parse(Type),
                        Summary = "",
                        CreateTime = DateTime.Now,
                        HotelInfoId = SessionInfo.hotelUser.HotelInfo.Id,
                        Sort = 0
                    };
                    DbContext.HotelImages.Add(img);
                }
            }
            DbContext.SaveChanges();
            return RedirectToAction("Manage");
        }
        public ActionResult Delete(string id)
        {
            var images=DbContext.HotelImages.Where(m=>m.Id==id);
            if (images.Count() > 0)
            {
                var image = images.First();
                FileInfo file = new FileInfo(Server.MapPath(image.Url));//指定文件路径
                if (file.Exists)//判断文件是否存在
                {
                    file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                    file.Delete();//删除文件
                }
                DbContext.HotelImages.Remove(image);
                DbContext.SaveChanges();
            }
            return RedirectToAction("Manage");
        }
    }
}