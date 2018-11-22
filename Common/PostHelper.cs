using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public  class PostHelper
    {
        public static string PostData(string Url, string RequestPara)
        {
            try
            {
                WebRequest hr = HttpWebRequest.Create(Url);

                string AppKey = "1a129796d8af76627342fb17";
                string MasterSecret = "2b48385bbb9f850a8ecb0344";
                string Authorization = Common.Base64Helper.EncodeBase64(AppKey + ":" + MasterSecret, "65001");

                byte[] buf = System.Text.Encoding.GetEncoding("utf-8").GetBytes(RequestPara);
                hr.ContentType = "application/json";
                hr.ContentLength = buf.Length;
                hr.Method = "POST";
                hr.Headers.Add("Authorization", "Basic " + Authorization);

                System.IO.Stream RequestStream = hr.GetRequestStream();
                RequestStream.Write(buf, 0, buf.Length);
                RequestStream.Close();

                System.Net.WebResponse response = hr.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                string ReturnVal = reader.ReadToEnd();
                reader.Close();
                response.Close();

                return ReturnVal;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
