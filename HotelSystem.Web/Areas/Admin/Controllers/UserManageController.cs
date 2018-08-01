using HotelSystem.Helper;
using HotelSystem.Model;
using HotelSystem.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class UserManageController : Controller
    {
        private DBModelContainer DbContext = new DBModelContainer();

        // GET: Admin/GuestUserManage
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Guest(string key, int pageIndex = 1)
        {
            if (key == null) key = "";
            var users = (from m in DbContext.GuestUser where m.Name.Contains(key) || m.Phone.Contains(key) select m).OrderByDescending(m => m.LastLogin).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_GuestList", users);
            return View(users);
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public void ResetPassword(string id)
        {
            var u = DbContext.GuestUser.Single(m => m.Id == id);
            u.Password = Encrypt.Md5("123456");
            try
            {
                DbContext.SaveChanges();
                Response.Write("success");
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.End();
            }
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult Hotel(string key, int pageIndex = 1)
        {
            if (key == null) key = "";
            var users = (from m in DbContext.HotelUsers where m.Name.Contains(key) || m.HotelInfo.Name.Contains(key) select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_HotelList", users);
            return View(users);
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public void HotelUserResetPassword(string id)
        {
            var u = DbContext.HotelUsers.Single(m => m.Id == id);
            u.Password = Encrypt.Md5("123456");
            try
            {
                DbContext.SaveChanges();
                Response.Write("success");
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.End();
            }
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult CompanyUser(string key, int pageIndex = 1)
        {
            if (key == null) key = "";
            var companys = (from m in DbContext.Company where m.Name.Contains(key) || m.Phone.Contains(key) select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_CompanyList", companys);
            return View(companys);
        }

        [Login(Area = "Admin", Role = "system")]
        public ActionResult CompanyExamine(string id)
        {
            var company = DbContext.Company.Single(m => m.Id == id);
            return View(company);
        }

        [Login(Area = "Admin", Role = "system")]
        [HttpPost]
        public ActionResult CompanyExamine(string id, string result, string opinion)
        {
            short status = 0;
            if (result == "通过")
            {
                status = 1;
            }
            else
            {
                status = 2;
                if (string.IsNullOrEmpty(opinion))
                {
                    ViewBag.Message = "审核意见不能为空";
                    return View();
                }
            }
            var company = DbContext.Company.Single(m => m.Id == id);
            company.Status = status;
            company.Opinion = opinion;
            company.ExamineTime = DateTime.Now;
            company.ExamineUser = SessionInfo.systemUser.Name;
            //添加用户
            GuestUser user = new GuestUser();
            user.Id = Guid.NewGuid().ToString();
            user.Name = company.Name;
            user.NickName = company.Name;
            user.Phone = "";
            user.Password = Encrypt.Md5("123456");
            user.Type = 2;
            user.Sex = 1;
            user.Head = "";
            user.RealName = "";
            user.Summary = "";
            user.RegisterType = "phone";
            user.RegisterIp = Request.ServerVariables.Get("Remote_Addr").ToString();
            user.RegisterTime = DateTime.Now;
            user.Password = Encrypt.Md5(user.Password);
            user.LastLogin = DateTime.Now;
            user.Company = company;
            DbContext.GuestUser.Add(user);
            try
            {
                DbContext.SaveChanges();
                //短信通知
                if (status == 1)
                {
                    string msg = string.Format("尊敬的客户，您申请的公司用户管理员已审核通过,登录用户：{0}，登录手机：{1},初始密码：{2},请及时登录修改密码。",
                            company.Name,
                            company.Phone,"123456");
                    SMS.Send(msg, company.Phone, 0);
                }
                else
                {
                    string msg = string.Format("尊敬的客户，您申请的公司用户审核未通过,公司名{0}，手机{1}。失败原因：{2}",
                            company.Name,
                            company.Phone,
                            company.Opinion);
                    SMS.Send(msg, company.Phone, 0);
                }
                ViewBag.Message = "审核成功,已短信通知用户";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "错误：" + ex.Message;
            }

            return View();
        }
    }
}