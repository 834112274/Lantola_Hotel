using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Text;

namespace HotelSystem.Web.Models
{
    public class Msg
    {
        private string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["App_Key"];
        public int random;
        public string ext { get; set; }
        public string extend { get; set; }
        public string msg { get; set; }
        public string sig { get; set; }
        public Tel tel { get; set; }
        public long time { get; set; }
        public int type { get; set; }

        public Msg(string Msg, string Mobile, int Type = 0)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            time = Convert.ToInt64(ts.TotalSeconds);
            type = 0;
            random = GetRandom();
            msg = Msg;
            tel = new Tel() {
                mobile= Mobile,
                nationcode="86",
            };
            ext = "";
            extend = "";
        }

        public void Sig()
        {
            sig = GetSHA256hash();
        }

        private string GetSHA256hash()
        {
            string input = $"appkey={appkey}&random={random}&time={time}&mobile={tel.mobile}";

            byte[] clearBytes = Encoding.UTF8.GetBytes(input);
            SHA256 sha256 = new SHA256Managed();
            sha256.ComputeHash(clearBytes);
            byte[] hashedBytes = sha256.Hash;
            sha256.Clear();
            string output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return output;
        }

        private int GetRandom()
        {
            return new Random().Next(100000, 999999);
        }
    }

    public class Tel
    {
        public string mobile { get; set; }
        public string nationcode { get; set; }
    }

    public class SMS
    {
        public static bool Send(string Msg, string Mobile, int Type = 0)
        {
            Msg m = new Msg(Msg, Mobile, Type);
            m.Sig();
            string config_url = System.Web.Configuration.WebConfigurationManager.AppSettings["tencent_cloud"];
            string SDK_AppID = System.Web.Configuration.WebConfigurationManager.AppSettings["SDK_AppID"];
            string url = $"{config_url}?sdkappid={SDK_AppID}&random={m.random}";
            string result = HttpCommon.GetResponseData(JsonConvert.SerializeObject(m), url);
            JObject o = JsonConvert.DeserializeObject<JObject>(result);
            if (o.GetValue("result").ToString() == "0")
            {
                return true;
            }
            return false;
        }
    }
}