using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using F2FPay;
using HotelSystem.Model;
using System;
using System.Collections.Generic;

namespace HotelSystem.Web.Models
{
    public class Alipay
    {
        private LogHelper log = new LogHelper(AppDomain.CurrentDomain.BaseDirectory + "/logs/alipay"+ DateTime.Now.ToString("yyyy-MM-dd") + ".log");

        private IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(Config.serverUrl, Config.appId, Config.merchant_private_key, Config.version,
                             Config.sign_type, Config.alipay_public_key, Config.charset);

        public AlipayF2FPrecreateResult GetPayUrl(Order order)
         {
            AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();
            //收款账号
            builder.seller_id = Config.pid;
            //订单编号
            builder.out_trade_no = order.Number;
            //订单总金额
            builder.total_amount = order.Total.ToString();

            //参与优惠计算的金额
            //builder.discountable_amount = "";
            //不参与优惠计算的金额
            //builder.undiscountable_amount = "";
            //订单名称
            builder.subject = $"酒店预定({order.HotelName})-{order.RoomCount}间{(order.StartTime - order.EndTime).Days.ToString()}晚";
            //自定义超时时间
            builder.timeout_express = "5m";
            //订单描述
            builder.body = $"酒店名称:{order.HotelName},房型:{order.RoomName},间夜数:{order.RoomCount}间{(order.StartTime - order.EndTime).Days.ToString()}晚";

            //门店编号，很重要的参数，可以用作之后的营销
            builder.store_id = "Lantola-001";
            //操作员编号，很重要的参数，可以用作之后的营销
            builder.operator_id = "lantola-admin";

            //传入商品信息详情
            //List<GoodsInfo> gList = new List<GoodsInfo>();
            //GoodsInfo goods = new GoodsInfo();
            //goods.goods_id = "goods id";
            //goods.goods_name = "goods name";
            //goods.price = "0.01";
            //goods.quantity = "1";
            //gList.Add(goods);
            //builder.goods_detail = gList;

            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sysServiceProviderId = "20880000000000";
            //builder.extendParams = exParam;

            //如果需要接收扫码支付异步通知，那么请把下面两行注释代替本行。
            //推荐使用轮询撤销机制，不推荐使用异步通知,避免单边账问题发生。
            //AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder);

            //商户接收异步通知的地址
            AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder, Config.notify_url);

            //以下返回结果的处理供参考。
            //payResponse.QrCode即二维码对于的链接
            //将链接用二维码工具生成二维码打印出来，顾客可以用支付宝钱包扫码支付。

            return precreateResult;
            
        }
    }
}