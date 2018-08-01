using Com.Alipay;
using HotelSystem.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WxPayAPI;

namespace HotelSystem.Web.Models.WxPay
{
    public class PayNotifyController : Controller
    {
        private DBModelContainer DbContext = new DBModelContainer();

        #region 微信

        public void WxCheckOrder()
        {
            var data = GetNotifyData();
            //判断是否支付成功
            if (data.GetValue("return_code").ToString() == "SUCCESS")
            {
                //检查订单号
                var id = data.GetValue("out_trade_no").ToString();
                var orders = from m in DbContext.Order
                             where m.Number == id
                             select m;
                if (orders.Count() > 0)
                {
                    var order = orders.First();
                    //如果已支付直接通知微信
                    if (order.Payment)
                    {
                        //通知微信
                        WxPayData res = new WxPayData();
                        res.SetValue("return_code", "SUCCESS");
                        res.SetValue("return_msg", "");
                        Log.Info(this.GetType().ToString(), "The data WeChat post is error : " + res.ToXml());
                        Response.Write(res.ToXml());
                        Response.End();
                        return;
                    }
                    var total = double.Parse(data.GetValue("total_fee").ToString());//微信订单金额
                    //判断订单金额
                    if ((order.Total * 100) == total)
                    {
                        //更新订单状态
                        order.Payment = true;
                        order.PayTime = DateTime.Now;
                        transaction t = new transaction()
                        {
                            id = Guid.NewGuid().ToString(),
                            data = data.ToJson()
                        };
                        order.transaction = t;
                        DbContext.SaveChanges();
                        //通知微信
                        WxPayData res = new WxPayData();
                        res.SetValue("return_code", "SUCCESS");
                        res.SetValue("return_msg", "支付成功");
                        Log.Info(this.GetType().ToString(), "SUCCESS : " + res.ToXml());
                        Response.Write(res.ToXml());
                        Response.End();
                        return;
                    }
                    else
                    {
                        //通知微信
                        WxPayData res = new WxPayData();
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", "支付成功");
                        Log.Info(this.GetType().ToString(), "金额不符 FAIL : " + res.ToXml());
                        Response.Write(res.ToXml());
                        Response.End();
                        return;
                    }
                }
                else
                {
                    //通知微信
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "支付成功");
                    Log.Info(this.GetType().ToString(), "未找到订单 FAIL : " + res.ToXml());
                    Response.Write(res.ToXml());
                    Response.End();
                    return;
                }
            }
            else
            {
                Log.Info(this.GetType().ToString(), "支付失败 FAIL : " + data.ToXml());
                return;
            }
        }

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            Log.Info(this.GetType().ToString(), "Receive data from WeChat : " + builder.ToString());

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                Log.Error(this.GetType().ToString(), "Sign check error : " + res.ToXml());
                Response.Write(res.ToXml());
                Response.End();
            }

            Log.Info(this.GetType().ToString(), "Check sign success");
            return data;
        }

        #endregion 微信

        #region 支付宝

        public void AlipayCheckOrder()
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Log.Info(this.GetType().ToString(), "pay msg:"+ JsonConvert.SerializeObject(Request.Form));
                //Notify aliNotify = new Notify();
                Com.Alipay.Notify aliNotify = new Com.Alipay.Notify(Config.charset, Config.sign_type, Config.pid, Config.mapiUrl, Config.alipay_public_key);

                //对异步通知进行验签
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);
                //对验签结果
                //bool isSign = Aop.Api.Util.AlipaySignature.RSACheckV2(sPara, Config.alipay_public_key ,Config.charset,Config.sign_type,false );

                if (verifyResult && CheckParams()) //验签成功 && 关键业务参数校验成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码

                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //商户订单号
                    string out_trade_no = Request.Form["out_trade_no"];
                    var orders = from m in DbContext.Order
                                 where m.Number == out_trade_no
                                 select m;
                    if (orders.Count() > 0)
                    {
                        var order = orders.First();
                        //判断系统订单是否已支付
                        if (!order.Payment)
                        {
                            //支付宝交易号
                            string trade_no = Request.Form["trade_no"];

                            //交易状态
                            //在支付宝的业务通知中，只有交易通知状态为TRADE_SUCCESS或TRADE_FINISHED时，才是买家付款成功。
                            string trade_status = Request.Form["trade_status"];
                            //判断支付宝订单是否支付成功
                            if (trade_status == "TRADE_SUCCESS" || trade_status == "TRADE_FINISHED")
                            {
                                string total_amount = Request.Form["total_amount"];
                                var total = Math.Round(double.Parse(total_amount), 2);//支付宝订单金额
                                //验证金额
                                if (total == Math.Round(order.Total, 2))
                                {
                                    order.Payment = true;
                                    order.PayTime = DateTime.Now;
                                    transaction t = new transaction()
                                    {
                                        id = Guid.NewGuid().ToString(),
                                        data = JsonConvert.SerializeObject(Request.Form)
                                    };
                                    order.transaction = t;
                                    DbContext.SaveChanges();
                                    Log.Info(this.GetType().ToString(), $"pay success id:{order.Id}");
                                    Response.Write("success");  //请不要修改或删除
                                }
                                else
                                {
                                    Log.Info(this.GetType().ToString(), $"total_amount non conformity: total_amount({total_amount}),total{order.Total}");
                                    Response.Write("fail");  //请不要修改或删除
                                }
                            }
                            else
                            {
                                Log.Info(this.GetType().ToString(), "trade_status fail:"+ trade_status);
                                Response.Write("fail");  //请不要修改或删除
                            }
                            //判断是否在商户网站中已经做过了这次通知返回的处理
                            //如果没有做过处理，那么执行商户的业务程序
                            //如果有做过处理，那么不执行商户的业务程序
                        }
                        else
                        {
                            Response.Write("success");  //请不要修改或删除
                        }

                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                    }
                    else
                    {
                        Log.Info(this.GetType().ToString(), "order not find");
                        Response.Write("fail");
                    }
                }
                else//验证失败
                {
                    Log.Info(this.GetType().ToString(), "verifyResult fail");
                    Response.Write("fail");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }
            Response.End();
        }

        /// <summary>
        /// 对支付宝异步通知的关键参数进行校验
        /// </summary>
        /// <returns></returns>
        private bool CheckParams()
        {
            bool ret = true;

            //获得商户订单号out_trade_no
            string out_trade_no = Request.Form["out_trade_no"];
            //TODO 商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，

            //获得支付总金额total_amount
            string total_amount = Request.Form["total_amount"];
            //TODO 判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），

            //获得卖家账号seller_email
            string seller_email = Request.Form["seller_email"];
            //TODO 校验通知中的seller_email（或者seller_id) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id / seller_email）

            //获得调用方的appid；
            //如果是非授权模式，appid是商户的appid；如果是授权模式（token调用），appid是系统商的appid
            string app_id = Request.Form["app_id"];
            //TODO 验证app_id是否是调用方的appid；。

            //验证上述四个参数，完全吻合则返回参数校验成功
            return ret;
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        #endregion 支付宝
    }
}