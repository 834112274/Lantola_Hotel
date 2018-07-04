using System.IO;
using System.Web;

namespace HotelSystem.Web.Models
{
    public class ServerConfig
    {
        private static string _hotelImgRoute = System.Web.Configuration.WebConfigurationManager.AppSettings["HotelImgRoute"];
        private static string _userImgRoute = System.Web.Configuration.WebConfigurationManager.AppSettings["UserImgRoute"];
        private static string _webImgRoute = System.Web.Configuration.WebConfigurationManager.AppSettings["WebImgRoute"];

        /// <summary>
        /// 酒店图片根目录
        /// </summary>
        public static string HotelImgRoute
        {
            get
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(_hotelImgRoute)))//判断文件夹是否存在
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(_hotelImgRoute));//不存在则创建文件夹
                }
                return _hotelImgRoute;
            }
        }

        public static string GetHotelImgRoute(string path)
        {
            string newPath = _hotelImgRoute + path + "/";
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(newPath)))//判断文件夹是否存在
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(newPath));//不存在则创建文件夹
            }
            return newPath;
        }

        /// <summary>
        /// 用户图片根目录
        /// </summary>
        public static string UserImgRoute
        {
            get
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(_userImgRoute)))//判断文件夹是否存在
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(_userImgRoute));//不存在则创建文件夹
                }
                return _userImgRoute;
            }
        }

        public static string GetUserImgRoute(string path)
        {
            string newPath = _userImgRoute + path + "/";
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(newPath)))//判断文件夹是否存在
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(newPath));//不存在则创建文件夹
            }
            return newPath;
        }

        /// <summary>
        /// 网站图片根目录
        /// </summary>
        public static string WebImgRoute
        {
            get
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(_webImgRoute)))//判断文件夹是否存在
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(_webImgRoute));//不存在则创建文件夹
                }
                return _webImgRoute;
            }
        }

        public static string GetWebImgRoute(string path)
        {
            string newPath = _userImgRoute + path + "/";
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(newPath)))//判断文件夹是否存在
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(newPath));//不存在则创建文件夹
            }
            return newPath;
        }

        /// <summary>
        /// 图片压缩等级
        /// </summary>
        public static long Level = long.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["ImgCompressLevel"]);
    }
}