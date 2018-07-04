using HotelSystem.Model;
using System.Web;

namespace HotelSystem.Web
{
    public class SessionInfo
    {
        public static Users systemUser
        {
            get
            {
                var u = HttpContext.Current.Session["SystemUser"] as Users;
                return u;
            }
        }
        public static HotelUsers hotelUser
        {
            get
            {
                var u = HttpContext.Current.Session["HotelUser"] as HotelUsers;
                return u;
            }
        }
        public static GuestUser guestUser
        {
            get
            {
                var u = HttpContext.Current.Session["GuestUser"] as GuestUser;
                return u;
            }
        }
    }
}