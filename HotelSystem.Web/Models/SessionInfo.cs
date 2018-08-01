using HotelSystem.Model;
using System.Web;
using System.Linq;

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
        public static HotelInfo hotel
        {
            get
            {
                var u = HttpContext.Current.Session["HotelUser"] as HotelUsers;
                if(u!=null)
                {
                    using (DBModelContainer DbContext = new DBModelContainer())
                    {
                        var hotelInfo = (from m in DbContext.HotelUsers where m.Id == u.Id select m.HotelInfo).First();
                        return hotelInfo;
                    }
                }
                else
                {
                    return null;
                }
                    
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