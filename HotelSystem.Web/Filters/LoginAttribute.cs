using HotelSystem.Model;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web
{
    public class LoginAttribute : ActionFilterAttribute
    {
        DBModelContainer DbContext = new DBModelContainer();
        /// <summary>
        /// 用户角色
        /// </summary>
        public string Role { get; set; }

        public string Area { get; set; }

        public override void OnActionExecuting(ActionExecutingContext Context)
        {
            switch (Role)
            {
                case "hotel":
                    if (Context.HttpContext.Session["HotelUser"] != null)
                    {
                        return;
                    }
                    else if (Context.HttpContext.Request.Cookies["hotel_remember"] != null)
                    {
                        if (Context.HttpContext.Request.Cookies["hotel_remember"].Value == "1")
                        {
                            var name = HttpUtility.UrlDecode(Context.HttpContext.Request.Cookies["hotel_user"].Value);
                            var password = Context.HttpContext.Request.Cookies["hotel_password"].Value;
                            var loginUser = from u in DbContext.HotelUsers
                                            where u.Name == name && u.Password == password
                                            select u;
                            if (loginUser.Count() > 0)
                            {
                                var u = loginUser.First();
                                Context.HttpContext.Session["HotelUser"] = u;
                                return;
                            }
                            else
                            {
                                Context.Result = GetRedirectResult();
                            }
                        }
                        else
                        {
                            Context.Result = GetRedirectResult();
                        }
                    }
                    else
                    {
                        Context.Result = GetRedirectResult();
                    }
                    break;

                case "system":
                    if (Context.HttpContext.Session["SystemUser"] != null)
                    {
                        return;
                    }
                    else if (Context.HttpContext.Request.Cookies["system_remember"] != null)
                    {
                        if (Context.HttpContext.Request.Cookies["system_remember"].Value == "1")
                        {
                            var name = HttpUtility.UrlDecode(Context.HttpContext.Request.Cookies["system_user"].Value);
                            var password = Context.HttpContext.Request.Cookies["system_password"].Value;
                            var loginUser = from u in DbContext.Users
                                            where u.Name == name && u.Password == password
                                            select u;
                            if (loginUser.Count() > 0)
                            {
                                var u = loginUser.First();
                                Context.HttpContext.Session["SystemUser"] = u;
                                return;
                            }
                            else
                            {
                                Context.Result = GetRedirectResult();
                            }
                        }
                        else
                        {
                            Context.Result = GetRedirectResult();
                        }
                    }
                    else
                    {
                        Context.Result = GetRedirectResult();
                    }
                    break;

                default:
                    
                    if (Context.HttpContext.Session["GuestUser"] != null)
                    {
                        return;
                    }
                    else if (Context.HttpContext.Request.Cookies["guest_remember"] != null)
                    {
                        if (Context.HttpContext.Request.Cookies["guest_remember"].Value == "1")
                        {
                            var name = HttpUtility.UrlDecode(Context.HttpContext.Request.Cookies["guest_user"].Value);
                            var password = Context.HttpContext.Request.Cookies["guest_password"].Value;
                            var loginUser = from u in DbContext.GuestUser
                                            where (u.Name == name || u.Phone == name || u.Email == name) && u.Password == password
                                            select u;
                            if (loginUser.Count() > 0)
                            {
                                var u = loginUser.First();
                                Context.HttpContext.Session["GuestUser"] = u;
                                return;
                            }
                            else
                            {
                                Context.Result = GetRedirectResult();
                            }
                        }
                        else
                        {
                            Context.Result = GetRedirectResult();
                        }
                    }
                    else
                    {
                        Context.Result = GetRedirectResult();
                    }
                    break;
            }
        }

        public RedirectResult GetRedirectResult()
        {
            switch (Area)
            {
                case "Admin":
                    return new RedirectResult("/Admin/User/Login");

                case "Hotel":
                    return new RedirectResult("/Hotel/User/Login");

                default:
                    return new RedirectResult("/User/Login");
            }
        }
    }
}