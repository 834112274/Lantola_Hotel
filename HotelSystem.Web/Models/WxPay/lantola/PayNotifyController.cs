using HotelSystem.Model;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WxPayAPI;

namespace HotelSystem.Web.Models.WxPay
{
    public class PayNotifyController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        public void CheckOrder()
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
    }
}