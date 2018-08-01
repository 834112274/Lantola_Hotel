using HotelSystem.Web.Models.WxPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Controllers
{
    public class PayController: PayNotifyController
    {
        // GET: Pay
        public void WxNotify()
        {
            WxCheckOrder();
        }
        public void AlipayNotify()
        {
            AlipayCheckOrder();
        }
    }
}