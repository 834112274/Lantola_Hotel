using System;

namespace HotelSystem.Web.Models
{
    public class Verification
    {
        //有效时长70秒
        public int Seconds
        {
            get; set;
        }

        public DateTime CreateTime
        {
            get; set;
        }

        public string Code
        {
            get; set;
        }
        public string Key
        {
            get; set;
        }
        public Verification()
        {
            this.Seconds = 120;
            this.CreateTime = DateTime.Now;
            this.Code = new Random().Next(100000, 999999).ToString(); 
        }
        public Verification(string key)
        {
            this.Seconds = 120;
            this.CreateTime = DateTime.Now;
            this.Code = new Random().Next(100000, 999999).ToString();
            this.Key = key;
        }
        public int Check(string code)
        {
            if ((DateTime.Now - CreateTime).Seconds > Seconds)
            {
                //过期
                return 2;
            }
            if (!Code.Equals(code))
            {
                //不匹配
                return -1;
            }
            else
            {
                //成功
                return 1;
            }
        }
        public int Check(string code,string key)
        {
            if (key != this.Key)
            {
                return 3;
            }
            if ((DateTime.Now - CreateTime).Seconds > Seconds)
            {
                //过期
                return 2;
            }
            if (!Code.Equals(code))
            {
                //不匹配
                return -1;
            }
            else
            {
                //成功
                return 1;
            }
        }
    }
}