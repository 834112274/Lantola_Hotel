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
    public class BoardroomController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Hotel/Boardroom
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Manage()
        {
            var hotel = DbContext.HotelInfo.Where(h => h.Id == SessionInfo.hotelUser.HotelInfoId).First();
            if (hotel.Conference == null)
            {
                ViewBag.Conference = new Conference();
                return View(new List<ConferenceRoom>());
            }
            else
            {
                ViewBag.Conference = hotel.Conference;
                return View(hotel.Conference.ConferenceRoom);
            }
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult ChangeInfo()
        {
            //草特么奇葩，参数直接放action映射，直接特么报错，还特么没发断点
            Conference conference = new Conference();

            conference.Id = Request["Id"];
            conference.TeaBreak = Request["TeaBreak"];
            conference.RoomCount = int.Parse(Request["RoomCount"]);
            conference.MaxArea = Request["MaxArea"];
            conference.OpenYear = "";
            conference.NewestRenovation = "";
            conference.GuestRoom = 0;
            conference.Buffet = Request["Buffet"];
            conference.Banquet = Request["Banquet"];
            var hotel = DbContext.HotelInfo.Where(h => h.Id == SessionInfo.hotelUser.HotelInfoId).First();
            if (hotel.Conference == null)
            {
                conference.CreateTime = DateTime.Now;
                hotel.Conference = conference;
                hotel.Conference.Id = Guid.NewGuid().ToString();
            }
            else
            {
                hotel.Conference.TeaBreak = conference.TeaBreak;
                hotel.Conference.RoomCount = conference.RoomCount;
                hotel.Conference.MaxArea = conference.MaxArea;
                hotel.Conference.OpenYear = conference.OpenYear;
                hotel.Conference.NewestRenovation = conference.NewestRenovation;
                hotel.Conference.GuestRoom = conference.GuestRoom;
                hotel.Conference.Buffet = conference.Buffet;
                hotel.Conference.Banquet = conference.Banquet;
            }
            DbContext.SaveChanges();
            return RedirectToAction("Manage");
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult AddBoardroom(ConferenceRoom room, HttpPostedFileBase Images)
        {
            room.Id = Guid.NewGuid().ToString();
            var hotel = DbContext.HotelInfo.Where(h => h.Id == SessionInfo.hotelUser.HotelInfoId).First();
            if (hotel.Conference != null)
            {
                if (Images != null && Images.ContentLength > 0)
                {
                    string TimePath = ServerConfig.GetHotelImgRoute(SessionInfo.hotelUser.HotelInfoId);
                    string FileType = Images.FileName.Substring(Images.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                    string imgName = Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名

                    ImgHelper.Compress(Images.InputStream, Server.MapPath(TimePath) + imgName, ServerConfig.Level);//压缩保存操作

                    room.Images = TimePath + imgName;
                }
                room.ConferenceId = hotel.Conference.Id;
                DbContext.ConferenceRoom.Add(room);
                DbContext.SaveChanges();
            }

            return RedirectToAction("Manage");
        }

        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult DeleteRoom(string id)
        {
            var rooms = DbContext.ConferenceRoom.Where(r=>r.Id==id);
            if (rooms.Count() > 0)
            {
                var room = rooms.First();
                FileInfo file = new FileInfo(Server.MapPath(room.Images));//指定文件路径
                if (file.Exists)//判断文件是否存在
                {
                    file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                    file.Delete();//删除文件
                }
                DbContext.ConferenceRoom.Remove(room);
                DbContext.SaveChanges();
            }
            return RedirectToAction("Manage");
        }
    }
}