using Com.Alipay.Business;
using Com.Alipay.Model;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using HotelSystem.Model;
using HotelSystem.Web.Models;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using WxPayAPI;

namespace HotelSystem.Web.Controllers
{
    public class InsideApiController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: InsideApi
        public void SendSms(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                Response.Write("手机号不能为空");
                Response.End();
                return;
            }
            Verification verification = new Verification(phone);
            string msg = string.Format("验证码：{0}，{1}分钟内有效，为了保障您的账户安全，请勿向他人泄露验证码信息。", verification.Code, "2");
            if (Session["SendVerification"] != null)
            {
                DateTime d = DateTime.Parse(Session["SendVerification"].ToString());
                if ((DateTime.Now - d).Seconds < 60)
                {
                    Response.Write("请勿重复发送");
                    Response.End();
                    return;
                }
                else
                {
                    if (SMS.Send(msg, phone, 0))
                    {
                        Session["SendVerification"] = verification.CreateTime;
                        Session["verification"] = verification.Code;
                        Session["Validate"] = verification;
                        Response.Write("success");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Response.Write("短信发送失败");
                        Response.End();
                        return;
                    }
                }
            }
            else
            {
                if (SMS.Send(msg, phone, 0))
                {
                    Session["SendVerification"] = verification.CreateTime;
                    Session["verification"] = verification.Code;
                    Session["Validate"] = verification;
                    Response.Write("success");
                    Response.End();
                    return;
                }
                else
                {
                    Response.Write("短信发送失败");
                    Response.End();
                    return;
                }
            }
        }

        public void SendEmailCode(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                Response.Write("邮箱不能为空");
                Response.End();
                return;
            }
            Verification verification = new Verification(email);
            // 设置发送方的邮件信息,例如使用网易的smtp
            string smtpServer = "smtp.exmail.qq.com"; //SMTP服务器
            string mailFrom = "automail@lantola.com"; //登陆用户名
            string userPassword = "Xiebin359873";//登陆密码

            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

            // 发送邮件设置
            MailMessage mailMessage = new MailMessage(mailFrom, email); // 发送人和收件人
            mailMessage.Subject = "LANTOLA验证码";//主题
            mailMessage.Body = "您的验证码为：" + verification.Code + " (此为系统自动发送，请勿回复)";//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.High;//优先级

            try
            {
                smtpClient.Send(mailMessage);
                Session["Validate"] = verification;
                Response.Write("success");
                Response.End();
                return;
            }
            catch (SmtpException ex)
            {
                Response.Write("验证码发送失败，请检查邮箱是否有效！");
                Response.End();
                return;
            }
        }

        public ActionResult WxQRCode(string orderId)
        {
            var orders = (from m in DbContext.Order where m.Id == orderId select m).ToList();
            if (orders.Count>0)
            {
                var order = orders.First();
                if (order.Payment)
                {
                    return Json(new { result = "fail", msg = "订单已支付完成" }, JsonRequestBehavior.AllowGet);
                }

                var url = NativePay.GetPayUrl(order);

                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(url, out qrCode);
                var renderer = new GraphicsRenderer(new FixedModuleSize(12, QuietZoneModules.Two));
                //Renderer renderer = new Renderer(5, Brushes.Black, Brushes.White);
                MemoryStream ms = new MemoryStream();
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);

                string buffer = Convert.ToBase64String(ms.GetBuffer());
                return Json(new { result = "success", msg = "二维码已生成", qrcode = buffer }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "fail", msg = "订单不存在" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AlipayQrCode(string orderId)
        {
            var orders = (from m in DbContext.Order where m.Id == orderId select m).ToList();
            if (orders.Count > 0)
            {
                var order = orders.First();
                if (order.Payment)
                {
                    return Json(new { result = "fail", msg = "订单已支付完成" }, JsonRequestBehavior.AllowGet);
                }
                Alipay alipay = new Alipay();
                AlipayF2FPrecreateResult precreateResult = alipay.GetPayUrl(order);
                switch (precreateResult.Status)
                {
                    case ResultEnum.SUCCESS:
                        QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                        QrCode qrCode = new QrCode();
                        qrEncoder.TryEncode(precreateResult.response.QrCode, out qrCode);
                        var renderer = new GraphicsRenderer(new FixedModuleSize(12, QuietZoneModules.Two));
                        //Renderer renderer = new Renderer(5, Brushes.Black, Brushes.White);
                        MemoryStream ms = new MemoryStream();
                        renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);

                        string buffer = Convert.ToBase64String(ms.GetBuffer());
                        return Json(new { result = "success", msg = "二维码已生成", qrcode = buffer });

                    case ResultEnum.FAILED:
                        return Json(new { result = "fail", msg = precreateResult.response.Body });

                    case ResultEnum.UNKNOWN:
                        if (precreateResult.response == null)
                        {
                            return Json(new { result = "fail", msg = "配置或网络异常，请检查后重试" });
                        }
                        else
                        {
                            return Json(new { result = "fail", msg = "系统异常，请更新外部订单后重新发起请求" });
                        }

                    default:
                        return Json(new { result = "fail", msg = "未知错误" });
                }
            }
            else
            {
                return Json(new { result = "fail", msg = "订单不存在" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}