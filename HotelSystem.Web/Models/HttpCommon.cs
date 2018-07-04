using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace HotelSystem.Web.Models
{
    public class HttpCommon
    {
        /// <summary>
        /// Http同步Get同步请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="encode">编码(默认UTF8)</param>
        /// <returns></returns>
        public static string HttpGet(string url, Encoding encode = null)
        {
            string result;

            try
            {
                var webClient = new WebClient { Encoding = Encoding.UTF8 };

                if (encode != null)
                    webClient.Encoding = encode;

                result = webClient.DownloadString(url);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Http同步Get异步请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="callBackDownStringCompleted">回调事件</param>
        /// <param name="encode">编码(默认UTF8)</param>
        public static void HttpGetAsync(string url,
            DownloadStringCompletedEventHandler callBackDownStringCompleted = null, Encoding encode = null)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };

            if (encode != null)
                webClient.Encoding = encode;

            if (callBackDownStringCompleted != null)
                webClient.DownloadStringCompleted += callBackDownStringCompleted;

            webClient.DownloadStringAsync(new Uri(url));
        }

        /// <summary>
        ///  Http同步Post同步请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="postStr">请求Url数据</param>
        /// <param name="encode">编码(默认UTF8)</param>
        /// <returns></returns>
        public static string HttpPost(string url, string postStr = "", Encoding encode = null)
        {
            string result;

            try
            {
                var webClient = new WebClient { Encoding = Encoding.UTF8 };

                if (encode != null)
                    webClient.Encoding = encode;

                var sendData = Encoding.GetEncoding("GB2312").GetBytes(postStr);

                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                webClient.Headers.Add("ContentLength", sendData.Length.ToString(CultureInfo.InvariantCulture));

                var readData = webClient.UploadData(url, "POST", sendData);

                result = Encoding.GetEncoding("GB2312").GetString(readData);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Http同步Post异步请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="postStr">请求Url数据</param>
        /// <param name="callBackUploadDataCompleted">回调事件</param>
        /// <param name="encode"></param>
        public static void HttpPostAsync(string url, string postStr = "",
            UploadDataCompletedEventHandler callBackUploadDataCompleted = null, Encoding encode = null)
        {
            var webClient = new WebClient { Encoding = Encoding.UTF8 };

            if (encode != null)
                webClient.Encoding = encode;

            var sendData = Encoding.GetEncoding("GB2312").GetBytes(postStr);

            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.Headers.Add("ContentLength", sendData.Length.ToString(CultureInfo.InvariantCulture));

            if (callBackUploadDataCompleted != null)
                webClient.UploadDataCompleted += callBackUploadDataCompleted;

            webClient.UploadDataAsync(new Uri(url), "POST", sendData);
        }

        /// <summary>
        /// 返回JSon数据
        /// </summary>
        /// <param name="JSONData">要处理的JSON数据</param>
        /// <param name="Url">要提交的URL</param>
        /// <returns>返回的JSON处理字符串</returns>
        public static string GetResponseData(string JSONData, string Url)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "text/json";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            //声明一个HttpWebRequest请求
            request.Timeout = 90000;
            //设置连接超时时间
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;

            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            return strResult;
        }
    }
}