using HotelSystem.Helper;
using HotelSystem.Model;
using HotelSystem.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace HotelSystem.Web.Controllers
{
    public class UserController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: User
        [Login(Area = "Guest", Role = "guest")]
        public ActionResult Header()
        {
            var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
            return View(user);
        }

        [Login(Area = "Guest", Role = "guest")]
        public ActionResult Order(int pageIndex = 1)
        {
            ViewBag.Message = TempData["message"] as string;
            var orders = DbContext.Order.Where(m => m.GuestUserId == SessionInfo.guestUser.Id).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            if (Request.IsAjaxRequest())
                return PartialView("_OrderList", orders);
            return View(orders);
        }
        [Login(Area = "Guest", Role = "guest")]
        public ActionResult CancelOrder(string id)
        {
            var orders = from m in DbContext.Order where m.Id == id select m;
            if (orders.Count() > 0)
            {
                return View(orders.First());
            }
            else
            {
                return View(new Order());
            }

        }
        [Login(Area = "Guest", Role = "guest")]
        [HttpPost]
        public ActionResult CancelOrder(string id,string cause)
        {
            var orders = from m in DbContext.Order where m.Id == id select m;
            if (orders.Count() > 0)
            {
                var order = orders.First();
                var days = (DateTime.Now - order.StartTime).Days;
                if (days > 3)
                {
                    TempData["message"] = "<strong>取消失败!</strong> 入住前3天内不可取消订单.";
                    return RedirectToAction("Order");
                }
                order.State = "0";
                order.CancelRemarks = cause;
                order.UpdateTime = DateTime.Now;
                DbContext.SaveChanges();
                TempData["message"] = "<strong>订单取消成功!</strong>";
                return RedirectToAction("Order");
            }
            else
            {
                TempData["message"] = "<strong>取消失败!</strong> 未选择有效订单.";
                return RedirectToAction("Order");
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(GuestUser user, string remember)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                ViewBag.Message = "登录名不能为空!";
                return View();
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Message = "密码不能为空!";
                return View();
            }
            user.Password = Encrypt.Md5(user.Password);
            var loginUser = from u in DbContext.GuestUser
                            where (u.Name == user.Name || u.Phone == user.Name || u.Email == user.Name) && u.Password == user.Password
                            select u;
            if (loginUser.Count() > 0)
            {
                var u = loginUser.First();
                u.LastLogin = DateTime.Now;
                DbContext.SaveChanges();
                Session["GuestUser"] = u;
                if (remember == "1")
                {
                    Response.Cookies["guest_remember"].HttpOnly = true; //只能服务器访问
                    Response.Cookies["guest_remember"].Expires = DateTime.Now.AddMonths(1); //用键名为ID的Cookie设置生存时间
                    Response.Cookies["guest_remember"].Value = "1"; //将键名为ID的Cookie的值设置为文本框内容

                    Response.Cookies["guest_user"].HttpOnly = true;
                    Response.Cookies["guest_user"].Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies["guest_user"].Value = u.Name;

                    Response.Cookies["guest_password"].HttpOnly = true;
                    Response.Cookies["guest_password"].Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies["guest_password"].Value = u.Password;
                }
                if (Request.UrlReferrer.AbsolutePath == "/User/Login")
                {
                    return RedirectToAction("Info");
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                ViewBag.Message = "用户名或密码错误!";
                return View();
            }
        }
        public ActionResult CompanyRegister()
        {
            var p = DbContext.Province.ToList();
            ViewBag.Province = p;
            var id = p.First().Id;
            var c = DbContext.City.Where(m => m.ProvinceID == id).ToList();
            ViewBag.City = c;
            id = c.First().Id;
            ViewBag.District = DbContext.District.Where(m => m.CityID == id).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult CompanyRegister(Company company,string telarea, HttpPostedFileBase BusinessLicense, HttpPostedFileBase CardPositive, HttpPostedFileBase CardOpposite)
        {
            company.Tel = telarea + "-" + company.Tel;
            if (string.IsNullOrEmpty(company.Name))
            {
                ViewBag.Message = "注册错误：公司名称不能为空";
                return View();
            }
            if (string.IsNullOrEmpty(company.Tel))
            {
                ViewBag.Message = "注册错误：电话不能为空";
                return View();
            }
            if (string.IsNullOrEmpty(company.Address))
            {
                ViewBag.Message = "注册错误：地址不能为空";
                return View();
            }
            if (string.IsNullOrEmpty(company.Contact))
            {
                ViewBag.Message = "注册错误：联系人不能为空";
                return View();
            }
            if (string.IsNullOrEmpty(company.Email))
            {
                ViewBag.Message = "注册错误：邮箱不能为空";
                return View();
            }
            if (string.IsNullOrEmpty(company.Phone))
            {
                ViewBag.Message = "注册错误：联系人电话不能为空";
                return View();
            }
            string TimePath = ServerConfig.UserImgRoute;
            if (BusinessLicense != null)
            {
                string FileType = BusinessLicense.FileName.Substring(BusinessLicense.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                company.BusinessLicense = TimePath + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名

                ImgHelper.Compress(BusinessLicense.InputStream, Server.MapPath(company.BusinessLicense), ServerConfig.Level);//压缩保存操作

            }
            else
            {
                ViewBag.Message = "注册错误：需上传营业执照";
                return View();
            }
            if (CardPositive != null)
            {
                string FileType = CardPositive.FileName.Substring(CardPositive.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                company.CardPositive = TimePath + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名

                ImgHelper.Compress(CardPositive.InputStream, Server.MapPath(company.CardPositive), ServerConfig.Level);//压缩保存操作

            }
            else
            {
                Directory.Delete(Server.MapPath(company.BusinessLicense), true);
                ViewBag.Message = "注册错误：需上传法人身份证正面";
                return View();
            }
            if (CardOpposite != null)
            {
                string FileType = CardOpposite.FileName.Substring(CardOpposite.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                company.CardOpposite = TimePath + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名

                ImgHelper.Compress(CardOpposite.InputStream, Server.MapPath(company.CardOpposite), ServerConfig.Level);//压缩保存操作

            }
            else
            {
                Directory.Delete(Server.MapPath(company.BusinessLicense), true);
                Directory.Delete(Server.MapPath(company.CardPositive), true);
                ViewBag.Message = "注册错误：需上传法人身份证反面";
                return View();
            }
            DbContext.Company.Add(company);
            DbContext.SaveChanges();
            return Redirect("/Hotel/User/RegisterResult");
        }
        public ActionResult Login()
        {
            return View();
        }

        

        public ActionResult LoginOut()
        {
            if (SessionInfo.guestUser != null)
            {
                Session["GuestUser"] = null;
                Response.Cookies["guest_remember"].HttpOnly = true; //只能服务器访问
                Response.Cookies["guest_remember"].Expires = DateTime.Now.AddMonths(1); //用键名为ID的Cookie设置生存时间
                Response.Cookies["guest_remember"].Value = "0"; //将键名为ID的Cookie的值设置为文本框内容
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult RegisterUser(GuestUser user, string verification)
        {
            if (string.IsNullOrEmpty(user.Phone))
            {
                ViewBag.Message = "手机号不能为空!";
                return View("Register");
            }
            if (Query.ExistGuestUserPhone(user.Phone))
            {
                ViewBag.Message = "手机号已注册!";
                return View("Register");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Message = "密码不能为空!";
                return View("Register");
            }
            if (string.IsNullOrEmpty(verification))
            {
                ViewBag.Message = "验证码不能为空!";
                return View("Register");
            }
            else
            {
                if (Session["verification"] != null)
                {
                    string v = Session["verification"].ToString();
                    DateTime d = DateTime.Parse(Session["SendVerification"].ToString());
                    if ((DateTime.Now - d).Seconds > 300)
                    {
                        ViewBag.Message = "验证码一过期!";
                        return View("Register");
                    }
                    if (!v.Equals(verification))
                    {
                        ViewBag.Message = "验证码不匹配!";
                        return View("Register");
                    }
                    else
                    {
                        Session["verification"] = null;
                        Session["SendVerification"] = null;
                    }
                }
                else
                {
                    ViewBag.Message = "请先发送验证码!";
                    return View("Register");
                }
            }
            user.Id = Guid.NewGuid().ToString();
            user.Name = user.Phone;
            user.NickName = user.Phone;
            user.Type = 1;
            user.Sex = 1;
            user.Head = "";
            user.RealName = "";
            user.Summary = "";
            user.RegisterType = "phone";
            user.RegisterIp = Request.ServerVariables.Get("Remote_Addr").ToString();
            user.RegisterTime = DateTime.Now;
            user.Password = Encrypt.Md5(user.Password);
            user.LastLogin = DateTime.Now;
            DbContext.GuestUser.Add(user);
            DbContext.SaveChanges();

            Session["GuestUser"] = user;
            return RedirectToAction("Info", "User");
        }

        [Login(Area = "Guest", Role = "guest")]
        public ActionResult Info()
        {
            GuestUser u = SessionInfo.guestUser;
            return View(u);
        }

        [Login(Area = "Guest", Role = "guest")]
        [HttpPost]
        public ActionResult Info(GuestUser user, HttpPostedFileBase HeadImg)
        {
            var u = DbContext.GuestUser.Single(m => m.Id == user.Id);

            if (HeadImg != null && HeadImg.ContentLength > 0)
            {
                string FileType = HeadImg.FileName.Substring(HeadImg.FileName.LastIndexOf(".") + 1); //得到文件的后缀名
                u.Head = ServerConfig.UserImgRoute + Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名

                ImgHelper.Compress(HeadImg.InputStream, Server.MapPath(u.Head), ServerConfig.Level);//压缩保存操作

            }
            u.NickName = user.NickName;
            u.RealName = user.RealName;
            u.QQ = user.QQ;
            u.WeChat = user.WeChat;
            u.Sex = user.Sex;
            u.Summary = user.Summary;
            u.City = user.City;
            DbContext.SaveChanges();
            Session["GuestUser"] = u;
            return View(u);
        }

        [Login(Area = "Guest", Role = "guest")]
        public ActionResult ApplyInvoice()
        {
            var p = DbContext.Province.ToList();
            ViewBag.Province = p;
            var id = p.First().Id;
            var c = DbContext.City.Where(m => m.ProvinceID == id).ToList();
            ViewBag.City = c;
            id = c.First().Id;
            ViewBag.District = DbContext.District.Where(m => m.CityID == id).ToList();
            var order = from m in DbContext.Order
                        where m.GuestUserId == SessionInfo.guestUser.Id && m.Payment == true && m.PaymentMethod == 0 && m.IsInvoice == 0 && m.IsValid == true && m.State != "0"
                        select m;
            ViewBag.Order = order;
            ViewBag.Message = TempData["message"] as string;
            return View();
        }

        /// <summary>
        /// 创建发票申请单
        /// </summary>
        /// <param name="ordersId">订单ID列表</param>
        /// <param name="invoiceType">发票类型</param>
        /// <param name="ReceivingAddress">收件地址</param>
        /// <param name="Addressee">收件人</param>
        /// <param name="Phone">收件人手机</param>
        /// <param name="Company">公司</param>
        /// <param name="Address">公司地址</param>
        /// <param name="Tel">公司电话</param>
        /// <param name="Bank">开户行</param>
        /// <param name="BankAccount">银行账户</param>
        /// <param name="TaxpayerNumber">纳税人识别号</param>
        /// <param name="head">发票抬头</param>
        /// <param name="companyHead">普通增值税抬头</param>
        /// <param name="taxpayer">普通增值税纳税人识别号</param>
        /// <param name="Province">省份</param>
        /// <param name="City">城市</param>
        /// <param name="District">区县</param>
        /// <returns></returns>
        [Login(Area = "Guest", Role = "guest")]
        [HttpPost]
        public ActionResult CreateInvoice(List<string> ordersId, short invoiceType,
            string ReceivingAddress, string Addressee, string Phone, string Company, string Address, string Tel, string Bank, string BankAccount, string TaxpayerNumber, string head, string companyHead, string taxpayer,
            int Province = 0, int City = 0, int District = 0)
        {
            if (ordersId == null)
            {
                TempData["message"] = "<strong>申请失败!</strong> 未选择有效订单.";
                return RedirectToAction("ApplyInvoice");
            }
            GuestUser gu = SessionInfo.guestUser;

            //检查企业用户发票纳税人识别号不能超过3个
            var existingInvoice = from m in DbContext.GuestInvoice where m.GuestUserId == gu.Id select m;
            bool isExist = false;
            if (!string.IsNullOrEmpty(TaxpayerNumber))
            {
                foreach (var t in existingInvoice)
                {
                    if (t.TaxpayerNumber == TaxpayerNumber)
                    {
                        isExist = true;
                    }
                }

                //企业用户最多三个纳税识别号
                if (gu.Type == 2 && existingInvoice.Count() >= 3 && !isExist)
                {
                    TempData["message"] = "<strong>申请失败!</strong> 企业用户最多可以使用3个以内的纳税人识别号，请使用您曾经使用过的纳税人识别号.";
                    return RedirectToAction("ApplyInvoice");
                }
            }
            double amount = 0;
            Invoice invoice = new Invoice()
            {
                Id = Guid.NewGuid().ToString(),
                ProvinceId = Province,
                CityId = City,
                DistrictId = District,
                Addressee = Addressee,
                Phone = Phone,
                ReceivingAddress = ReceivingAddress,
                Company = Company,
                Address = Address,
                Tel = Tel,
                Bank = Bank,
                BankAccount = BankAccount,
                TaxpayerNumber = TaxpayerNumber,
                Amount = amount,
                GuestUserId = gu.Id,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Status = 1,
                Type = invoiceType
            };
            //获取订单
            var orders = from m in DbContext.Order where m.GuestUserId == gu.Id && ordersId.Contains(m.Id) && m.Payment == true && m.PaymentMethod == 0 && m.IsInvoice == 0 && m.IsValid == true && m.State != "0" select m;
            if (orders.Count() > 0)
            {
                foreach (var order in orders)
                {
                    amount += order.HousingPrice;
                    order.IsInvoice = 1;
                    order.InvoiceId = invoice.Id;
                }
            }
            else
            {
                TempData["message"] = "<strong>申请失败!</strong> 未选择有效订单.";
                return RedirectToAction("ApplyInvoice");
            }
            if (amount == 0)
            {
                TempData["message"] = "<strong>申请失败!</strong> 发票申请金额必须大于0.";
                return RedirectToAction("ApplyInvoice");
            }
            invoice.Amount = amount;
            //个人发票
            if (invoiceType == 1)
            {
                invoice.Company = head;
            }
            //增值税普通发票
            if (invoiceType == 2)
            {
                invoice.Company = companyHead;
                invoice.TaxpayerNumber = taxpayer;
            }
            //不存在发票抬头添加到个人发票抬头记录
            if (!isExist&&!string.IsNullOrEmpty(invoice.TaxpayerNumber))
            {
                GuestInvoice guestInvoice = new GuestInvoice()
                {
                    Id = Guid.NewGuid().ToString(),
                    Company = invoice.Company,
                    Address = invoice.Address,
                    Tel = invoice.Tel,
                    Bank = invoice.Bank,
                    BankAccount = invoice.BankAccount,
                    TaxpayerNumber = invoice.TaxpayerNumber,
                    GuestUserId = invoice.GuestUserId
                };
                DbContext.GuestInvoice.Add(guestInvoice);
            }
            DbContext.Invoice.Add(invoice);
            DbContext.SaveChanges();
            TempData["message"] = string.Format("<strong>申请成功!</strong> 已成功申请发票，发票总金额:{0}.", amount);
            return RedirectToAction("ApplyInvoice");
        }

        [Login(Area = "Guest", Role = "guest")]
        public ActionResult InvoiceList()
        {
            var invoice = from m in DbContext.Invoice
                          where m.GuestUserId == SessionInfo.guestUser.Id
                          select m;
            ViewBag.Invoice = invoice;
            var guestInvoice = from m in DbContext.GuestInvoice
                               where m.GuestUserId == SessionInfo.guestUser.Id
                               select m;
            ViewBag.GuestInvoice = guestInvoice;
            ViewBag.Message = TempData["message"] as string;
            return View();
        }

        [Login(Area = "Guest", Role = "guest")]
        public ActionResult AddInvoiceHead(GuestInvoice guestInvoice)
        {
            GuestUser gu = SessionInfo.guestUser;
            if (!string.IsNullOrEmpty(guestInvoice.TaxpayerNumber))
            {
                var existingInvoice = from m in DbContext.GuestInvoice where m.GuestUserId == gu.Id select m;
                bool isExist = false;
                //企业用户最多三个纳税识别号
                //判断纳税人识别号释放存在记录
                foreach (var t in existingInvoice)
                {
                    if (t.TaxpayerNumber == guestInvoice.TaxpayerNumber)
                    {
                        isExist = true;
                    }
                }

                //企业用户最多三个纳税识别号
                if (gu.Type == 2 && existingInvoice.Count() >= 3 && !isExist)
                {
                    TempData["message"] = "<strong>添加失败!</strong> 企业用户最多可以使用3个以内的纳税人识别号.";
                    return RedirectToAction("InvoiceList");
                }
            }
            
            guestInvoice.Id = Guid.NewGuid().ToString();
            guestInvoice.GuestUserId = SessionInfo.guestUser.Id;
            DbContext.GuestInvoice.Add(guestInvoice);
            DbContext.SaveChanges();
            TempData["message"] = "<strong>添加成功!</strong> ";
            return RedirectToAction("InvoiceList");
        }
        [Login(Area = "Guest", Role = "guest")]
        public ActionResult DeleteInvoiceHead(string id)
        {
            var existingInvoice = from m in DbContext.GuestInvoice where m.Id == id select m;
            if (existingInvoice.Count() > 0)
            {
                DbContext.GuestInvoice.RemoveRange(existingInvoice);
            }
            DbContext.SaveChanges();
            TempData["message"] = "<strong>删除成功!</strong> ";
            return RedirectToAction("InvoiceList");
        }
        [Login(Area = "Guest", Role = "guest")]
        public ActionResult Collection(int pageIndex = 1)
        {
            var collection = (from m in DbContext.Collection
                              where m.GuestUserId == SessionInfo.guestUser.Id
                              select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);

            if (Request.IsAjaxRequest())
                return PartialView("_CollectionList", collection);
            return View(collection);
        }

        [Login(Area = "Guest", Role = "guest")]
        public void AddCollection(string id)
        {
            var hotel = DbContext.HotelInfo.Where(m => m.Id == id);
            var collection = DbContext.Collection.Where(m => m.HotelInfoId == id && m.GuestUserId == SessionInfo.guestUser.Id);
            if (collection.Count() > 0)
            {
                Response.Write("true");
                Response.End();
                return;
            }
            if (hotel.Count() > 0)
            {
                Collection c = new Collection()
                {
                    Id = Guid.NewGuid().ToString(),
                    GuestUserId = SessionInfo.guestUser.Id,
                    HotelInfoId = id,
                    Remarks = "",
                    CreateTime = DateTime.Now
                };
                DbContext.Collection.Add(c);
                DbContext.SaveChanges();
                Response.Write("true");
                Response.End();
            }
            else
            {
                Response.Write("酒店不存在");
                Response.End();
            }
        }

        [Login(Area = "Guest", Role = "guest")]
        public ActionResult Comment(int pageIndex = 1)
        {
            var comment = (from m in DbContext.Comment
                           where m.GuestUserId == SessionInfo.guestUser.Id
                           select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);

            if (Request.IsAjaxRequest())
                return PartialView("_CommentList", comment);
            return View(comment);
        }

        [Login(Area = "Guest", Role = "guest")]
        public ActionResult CreateComment(string id)
        {
            var order = DbContext.Order.Single(m => m.Id == id);
            ViewBag.Order = order;
            ViewBag.ScoreType = DbContext.ScoreType.Where(m => m.Valid == true).ToList();
            return View();
        }

        /// <summary>
        /// 创建评论
        /// </summary>
        /// <param name="order">订单ID</param>
        /// <param name="hotel">酒店ID</param>
        /// <param name="content"></param>
        /// <param name="imgs"></param>
        /// <param name="score"></param>
        [Login(Area = "Guest", Role = "guest")]
        [HttpPost]
        public void CreateComment(string order, string hotel, string content, List<string> imgs, List<string> score)
        {
            if (string.IsNullOrEmpty(content))
            {
                Response.Write(JsonConvert.SerializeObject(new { result = "fail", msg = "评论内容不能为空！" }));
                Response.End();
                return;
            }
            Comment comment = new Comment();
            comment.Content = content;
            comment.Id = Guid.NewGuid().ToString();
            comment.CreateTime = DateTime.Now;
            comment.GuestUserId = SessionInfo.guestUser.Id;
            comment.OrderId = order;
            comment.Images = "";

            foreach (var img in imgs)
            {
                if (!string.IsNullOrEmpty(img))
                {
                    var tempath = ServerConfig.UserImgRoute + Guid.NewGuid().ToString() + ".jpg";
                    Stream s = DataHelper.Base64StringToImage(img.Split(',')[1]);
                    
                    if (s!=null)
                    {
                        ImgHelper.Compress(s, Server.MapPath(tempath), ServerConfig.Level);//压缩保存操作
                        comment.Images += tempath + ",";
                    }
                }
            }
            comment.Images = comment.Images.TrimEnd(',');
            DbContext.Comment.Add(comment);
            foreach (var s in score)
            {
                if (s.Split(',').Count() > 1)
                {
                    Score t = new Score()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ScoreTypeId = s.Split(',')[0],
                        Value = short.Parse(s.Split(',')[1]),
                        CommentId = comment.Id,
                        HotelInfoId = hotel
                    };
                    DbContext.Score.Add(t);
                }
            }
            DbContext.SaveChanges();
            Response.Write(JsonConvert.SerializeObject(new { result = "success", msg = "评论成功！" }));
            Response.End();
        }

        [Login(Area = "Guest", Role = "guest")]
        public ActionResult Message(int pageIndex=1)
        {

            var message = (from m in DbContext.Message
                           where m.ToUser==SessionInfo.guestUser.Id
                           select m).OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, 15);
            bool change = false;
            foreach (var m in message)
            {
                if (!m.Read)
                {
                    change = true;
                    m.Read = true;
                    m.RendTime = DateTime.Now;
                }
            }
            if (change)
            {
                DbContext.SaveChanges();
            }
            if (Request.IsAjaxRequest())
                return PartialView("_CommentList", message);
            return View(message);
        }

        [Login(Area = "Guest", Role = "guest")]
        public ActionResult Safe()
        {
            ViewBag.Message = TempData["message"] as string;
            var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
            return View(user);
        }

        [Login(Area = "Guest", Role = "guest")]
        public ActionResult ChangePassword(string Password, string NewPassword, string ComparePassword)
        {
            var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
            var p = Encrypt.Md5(Password);
            if (p == user.Password)
            {
                if (string.IsNullOrEmpty(NewPassword))
                {
                    TempData["message"] = "<strong>错误!</strong> 新密码不能为空.";
                    return RedirectToAction("Safe");
                }
                if (NewPassword == ComparePassword)
                {
                    user.Password = Encrypt.Md5(NewPassword);
                    DbContext.SaveChanges();
                    TempData["message"] = "<strong>修改成功!</strong> 下次登录请使用新密码登录.";
                    return RedirectToAction("Safe");
                }
                else
                {
                    TempData["message"] = "<strong>错误!</strong> 新密码两次输入不一致.";
                    return RedirectToAction("Safe");
                }
            }
            else
            {
                TempData["message"] = "<strong>错误!</strong> 新旧密码不一致.";
                return RedirectToAction("Safe");
            }
        }

        /// <summary>
        /// 解绑手机
        /// </summary>
        /// <returns></returns>
        [Login(Area = "Guest", Role = "guest")]
        public ActionResult RelievePhone()
        {
            var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
            return View(user);
        }

        [Login(Area = "Guest", Role = "guest")]
        [HttpPost]
        public ActionResult RelievePhone(string VerificationCode)
        {
            var Validate = Session["Validate"] as Verification;
            var result = Validate.Check(VerificationCode);
            if (result == 1)
            {
                var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
                user.Phone = "";
                DbContext.SaveChanges();
                TempData["message"] = "<strong>解绑成功!</strong> 已成功解绑手机，现可重新绑定.";
                return RedirectToAction("Safe");
            }
            else
            {
                TempData["message"] = "<strong>错误!</strong> 验证码错误.";
                return RedirectToAction("Safe");
            }
        }

        /// <summary>
        /// 绑定手机
        /// </summary>
        /// <returns></returns>
        [Login(Area = "Guest", Role = "guest")]
        public ActionResult BindingPhone()
        {
            var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
            return View(user);
        }

        [Login(Area = "Guest", Role = "guest")]
        [HttpPost]
        public ActionResult BindingPhone(string Phone, string VerificationCode)
        {
            var Validate = Session["Validate"] as Verification;
            var result = Validate.Check(VerificationCode, Phone);
            if (result == 1)
            {
                var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
                user.Phone = Phone;
                DbContext.SaveChanges();
                TempData["message"] = "<strong>绑定成功!</strong> 已成功绑定手机.";
                return RedirectToAction("Safe");
            }
            else
            {
                TempData["message"] = "<strong>错误!</strong> 验证码错误.";
                return RedirectToAction("Safe");
            }
        }

        /// <summary>
        /// 解绑邮箱
        /// </summary>
        /// <returns></returns>
        [Login(Area = "Guest", Role = "guest")]
        public ActionResult RelieveEmail()
        {
            var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
            return View(user);
        }

        [Login(Area = "Guest", Role = "guest")]
        [HttpPost]
        public ActionResult RelieveEmail(string VerificationCode)
        {
            var Validate = Session["Validate"] as Verification;
            var result = Validate.Check(VerificationCode);
            if (result == 1)
            {
                var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
                user.Email = "";
                DbContext.SaveChanges();
                TempData["message"] = "<strong>解绑成功!</strong> 已成功解绑邮箱，现可重新绑定.";
                return RedirectToAction("Safe");
            }
            else
            {
                TempData["message"] = "<strong>错误!</strong> 验证码错误.";
                return RedirectToAction("Safe");
            }
        }

        /// <summary>
        /// 绑定邮箱
        /// </summary>
        /// <returns></returns>
        [Login(Area = "Guest", Role = "guest")]
        public ActionResult BindingEmail()
        {
            var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
            return View(user);
        }

        [Login(Area = "Guest", Role = "guest")]
        [HttpPost]
        public ActionResult BindingEmail(string Email, string VerificationCode)
        {
            var Validate = Session["Validate"] as Verification;
            var result = Validate.Check(VerificationCode, Email);
            if (result == 1)
            {
                var user = DbContext.GuestUser.Single(m => m.Id == SessionInfo.guestUser.Id);
                user.Email = Email;
                DbContext.SaveChanges();
                TempData["message"] = "<strong>绑定成功!</strong> 已成功绑定邮箱.";
                return RedirectToAction("Safe");
            }
            else
            {
                TempData["message"] = "<strong>错误!</strong> 验证码错误.";
                return RedirectToAction("Safe");
            }
        }
    }
}