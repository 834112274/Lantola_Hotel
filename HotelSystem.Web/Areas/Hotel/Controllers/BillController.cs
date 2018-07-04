using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Hotel.Controllers
{
    public class BillController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Hotel/Bill
        /// <summary>
        /// 账户
        /// </summary>
        /// <returns></returns>
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult Account()
        {
            return View();
        }

        [Login(Area = "Hotel", Role = "hotel")]
        /// <summary>
        /// 结算
        /// </summary>
        /// <returns></returns>
        public ActionResult Settlement()
        {
            return View();
        }
        [Login(Area = "Hotel", Role = "hotel")]
        public ActionResult History()
        {
            return View();
        }
        
    }
}