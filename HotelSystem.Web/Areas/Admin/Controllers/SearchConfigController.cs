using HotelSystem.Helper;
using HotelSystem.Model;
using HotelSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class SearchConfigController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Admin/SearchConfig
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Config()
        {
            var t = from c in DbContext.HomeConfig
                     where c.Valid==true
                     select c;
            if (t.Count() > 0)
            {
                var config = t.First();
                return View(config);
            }
            return View();
        }
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Save(HomeConfig config, HttpPostedFileBase ImgUrl)
        {
            if (!Directory.Exists(Server.MapPath(ServerConfig.WebImgRoute)))//判断文件夹是否存在
            {
                Directory.CreateDirectory(Server.MapPath(ServerConfig.WebImgRoute));//不存在则创建文件夹
            }

            if (ImgUrl != null && ImgUrl.ContentLength > 0)
            {
                string FileType = ImgUrl.FileName.Substring(ImgUrl.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                config.ImgUrl = ServerConfig.WebImgRoute + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                ImgHelper.Compress(ImgUrl.InputStream, Server.MapPath(config.ImgUrl), ServerConfig.Level);//压缩保存操作
            }
            config.UpdateTime = DateTime.Now;
            if (string.IsNullOrEmpty(config.Id))
            {
                config.Id = Guid.NewGuid().ToString();
                config.CreateTime = DateTime.Now;
                DbContext.HomeConfig.Add(config);
            }
            else
            {
                var t = (from c in DbContext.HomeConfig
                         where c.Id == config.Id
                         select c).First();
                FileInfo file = new FileInfo(Server.MapPath(t.ImgUrl));//指定文件路径
                if (file.Exists)//判断文件是否存在
                {
                    file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                    file.Delete();//删除文件
                }
                t.ImgUrl = config.ImgUrl;
                t.Title = config.Title;
                t.Summary = config.Summary;
                t.Valid = config.Valid;
            }
            DbContext.SaveChanges();
            return View("Config");
        }
    }
}