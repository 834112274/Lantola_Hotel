﻿using HotelSystem.Model;
using HotelSystem.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class HotelController : Controller
    {
        private DBModelContainer DbContext = new DBModelContainer();

        // GET: Admin/Hotel
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Examine(int pageIndex = 1)
        {
            var hotels = DbContext.HotelInfo.OrderByDescending(a => a.Apply).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", hotels);
            return View(hotels);
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult Examine(string key, int pageIndex = 1)
        {
            if (key == null) key = "";
            var hotels = DbContext.HotelInfo.Where(h => h.Name.Contains(key) || h.Adress.Contains(key)).OrderByDescending(a => a.Apply).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", hotels);
            return View(hotels);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult ExamineDetails(string id)
        {
            var hotels = DbContext.HotelInfo.Where(h => h.Id == id);
            if (hotels.Count() == 0)
            {
                ViewBag.Message = "未找到申请单";
                HotelInfo h = new HotelInfo();
                return View(h);
            }
            else
            {
                return View(hotels.First());
            }
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult ExamineHandle(string id, string result, string userName)
        {
            var hotels = DbContext.HotelInfo.Where(h => h.Id == id);
            if (hotels.Count() == 0)
            {
                ViewBag.Message = "未找到申请单";
                HotelInfo h = new HotelInfo();
                return View("ExamineDetails", h);
            }
            else
            {
                var hotel = hotels.First();
                if (Query.ExistHotelUserName(userName))
                {
                    ViewBag.Message = "登录用户名已存在";
                    return View("ExamineDetails", hotel);
                }
                hotel.ExamineTime = DateTime.Now;
                if (result.Equals("通过"))
                {
                    hotel.Valid = 1;
                    hotel.ExamineStatus = 1;
                }
                else
                {
                    hotel.ExamineStatus = 2;
                }
                hotel.ExamineUser = SessionInfo.systemUser.Name;
                HotelUsers user = new HotelUsers()
                {
                    Id = Guid.NewGuid().ToString(),
                    HotelInfoId = id,
                    Name = userName,
                    Password = "e10adc3949ba59abbe56e057f20f883e",
                    Role = "hotel",
                    CreateTime = DateTime.Now,
                    LastLogin = DateTime.Now,
                    Vail = true,
                    Status = 0
                };
                DbContext.HotelUsers.Add(user);
                //分配所有权限
                var cur_menus_id = from m in DbContext.UserRole where m.UserType == "hotel" && m.UsersId == user.Id select m.MenuId;
                var menus = from m in DbContext.Menu where m.Type == "hotel" select m;
                var new_menus = (from m in menus
                                 where !cur_menus_id.Contains(m.Id)
                                 select m).ToList().Select(m => new UserRole() { Id = Guid.NewGuid().ToString(), UsersId = user.Id, MenuId = m.Id, CreateTime = DateTime.Now, UserType = "hotel" }).ToList();

                DbContext.UserRole.AddRange(new_menus);
                try
                {
                    DbContext.SaveChanges();
                    //短信通知
                    if (hotel.Valid == 1)
                    {
                        string msg = string.Format("尊敬的客户，您申请的酒店入驻管理员已审核通过,登录用户：{0},初始密码：{2},请及时登录修改密码。",
                                user.Name,
                                "123456");
                        SMS.Send(msg, hotel.Phone, 0);
                    }
                    else
                    {
                        string msg = string.Format("尊敬的客户，您申请的酒店入驻审核未通过,公司名{0}，手机{1}。失败原因：{2}",
                                hotel.Name,
                                hotel.Phone,
                                "管理员太懒，没告知原因");
                        SMS.Send(msg, hotel.Phone, 0);
                    }
                    ViewBag.Message = "审核成功,已短信通知用户";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "错误：" + ex.Message;
                }

                
                return View("ExamineDetails", hotel);
            }
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Policy(int pageIndex = 1)
        {
            var policys = DbContext.Policy.OrderByDescending(a => a.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_PolicyList", policys);
            string json = string.Empty;
            using (FileStream fs = new FileStream(Server.MapPath("/Scripts/ico/icon.json"), FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            if (!string.IsNullOrEmpty(json))
            {
                ViewBag.Ico = JsonConvert.DeserializeObject(json);
            }
            return View(policys);
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult Policy(HttpPostedFileBase IconFile, HttpPostedFileBase JsonFile, HttpPostedFileBase CssFile)
        {
            string msg = "";
            if (IconFile != null && IconFile.ContentLength > 0)
            {
                string FileType = IconFile.FileName.Substring(IconFile.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                string path = "/Scripts/ico/icon." + FileType; //得到重命名的文件名
                IconFile.SaveAs(Server.MapPath(path)); //保存操作
                msg += "图标图片已更改,";
            }
            if (JsonFile != null && JsonFile.ContentLength > 0)
            {
                string FileType = JsonFile.FileName.Substring(JsonFile.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                string path = "/Scripts/ico/icon." + FileType; //得到重命名的文件名
                JsonFile.SaveAs(Server.MapPath(path)); //保存操作
                msg += "图标配置数据已更改,";
            }
            if (CssFile != null && CssFile.ContentLength > 0)
            {
                string FileType = CssFile.FileName.Substring(CssFile.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                string path = "/Scripts/ico/icon." + FileType; //得到重命名的文件名
                CssFile.SaveAs(Server.MapPath(path)); //保存操作
                msg += "图标样式文件已更改,";
            }
            msg = msg.TrimEnd(',');
            if (string.IsNullOrEmpty(msg))
            {
                msg = "无任何更改";
            }
            ViewBag.Message = msg;
            string json = string.Empty;
            using (FileStream fs = new FileStream(Server.MapPath("/Scripts/ico/icon.json"), FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            if (!string.IsNullOrEmpty(json))
            {
                var policys = DbContext.Policy.ToList();
                JObject obj = JsonConvert.DeserializeObject<JObject>(json);
                ViewBag.Ico = obj;
                var items = obj["frames"];
                foreach (var item in items)
                {
                    string c = item["filename"].ToString();
                    c = c.Substring(0, c.LastIndexOf("."));
                    if (c.Split('_').Count() > 2)
                    {
                        string typeName = c.Split('_')[0];
                        string Name = c.Split('_')[1];
                        string cssName = c.Split('_')[2];
                        var tmp = policys.Where(m=>m.Name==Name&&m.Icon==cssName&&m.Type==(typeName == "服务项目" ? 1 : typeName == "客房设施" ? 2 : 3));
                        if (tmp.Count() == 0)
                        {
                            Policy p = new Policy()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = typeName == "服务项目" ? 1 : typeName == "客房设施" ? 2 : 3,
                                Name = Name,
                                Values = "",
                                Icon = cssName,
                                CreateTime = DateTime.Now
                            };
                            DbContext.Policy.Add(p);
                        }
                        
                    }
                }
                DbContext.SaveChanges();
            }
            var viewData = DbContext.Policy.ToList();
            return View(viewData);
        }

        public ActionResult AddPolicy(Policy policy, HttpPostedFileBase Icon)
        {
            policy.Id = Guid.NewGuid().ToString();
            policy.CreateTime = DateTime.Now;
            if (Icon != null)
            {
                string FileType = Icon.FileName.Substring(Icon.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                policy.Icon = "/PolicyImages/" + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名
                Icon.SaveAs(Server.MapPath(policy.Icon)); //保存操作
            }
            DbContext.Policy.Add(policy);
            DbContext.SaveChanges();
            return RedirectToAction("Policy");
        }
    }
}