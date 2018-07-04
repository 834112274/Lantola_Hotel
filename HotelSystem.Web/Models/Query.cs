using HotelSystem.Model;
using System.Linq;

namespace HotelSystem.Web
{
    public class Query
    {
        public static bool ExistUserName(string name)
        {
            using (DBModelContainer DbContext = new DBModelContainer())
            {
                var users = DbContext.Users.Where(u => u.Name == name);
                if (users.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool ExistHotelUserName(string name)
        {
            using (DBModelContainer DbContext = new DBModelContainer())
            {
                var users = DbContext.HotelUsers.Where(u => u.Name == name);
                if (users.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool ExistGuestUserPhone(string phone)
        {
            using (DBModelContainer DbContext = new DBModelContainer())
            {
                var users = DbContext.GuestUser.Where(u => u.Phone == phone);
                if (users.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool ExistGuestUserName(string name)
        {
            using (DBModelContainer DbContext = new DBModelContainer())
            {
                var users = DbContext.GuestUser.Where(u => u.Name == name);
                if (users.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool ExistGuestUserEmail(string email)
        {
            using (DBModelContainer DbContext = new DBModelContainer())
            {
                var users = DbContext.GuestUser.Where(u => u.Email == email);
                if (users.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}