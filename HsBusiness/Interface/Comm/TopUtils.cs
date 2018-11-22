using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HsBusiness.Interface.Comm
{
    public static class TopUtils
    {
        public static void ValidateTimestamp(string s)
        {
            var now = DateTime.Now;
            DateTime timestamp;
            if (!DateTime.TryParse(s, out timestamp))
            {
                throw new TopException(31, "Invalid Timestamp");
            }
            if (Math.Abs((now - timestamp).TotalMinutes) > 10)
            {
                throw new TopException(31, "Invalid Timestamp");
            }
        }

        public static void ValidateSign(NameValueCollection parameters)
        {
            string key = parameters.ValidateRequired("app_key", false);
            string sign = parameters.ValidateRequired("sign", false);
            string secret = "";// SqlHelper.GetSecret(key);
            if (sign != SignTopRequest(parameters, secret, false))
            {
                throw new TopException(25, "Invalid Signature");
            }
        }

        public static string GetParametersString(this NameValueCollection parameters)
        {
            var sb = new StringBuilder();
            bool is_start = false;

            var dic = parameters.ToDictionary().Where(t => !string.IsNullOrEmpty(t.Key) && !string.IsNullOrEmpty(t.Value));

            foreach (var item in dic)
            {
                if (is_start) sb.Append("&");
                sb.AppendFormat("{0}={1}", item.Key, item.Value);
                is_start = true;
            }

            return sb.ToString();
        }

        public static IDictionary<string, string> ToDictionary(this NameValueCollection parameters)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string key in parameters.AllKeys)
            {
                if (!dic.ContainsKey(key) && !string.IsNullOrEmpty(parameters[key]))
                {
                    dic.Add(key, parameters[key]);
                }
            }
            return dic;
        }

        private static string SignTopRequest(NameValueCollection parameters, string secret, bool qhs)
        {
            var dic = parameters.ToDictionary();
            return SignTopRequest(dic, secret, qhs);
        }

        public static string SignTopRequest(IDictionary<string, string> parameters, string secret, bool qhs)
        {
            StringBuilder query = new StringBuilder(secret);
            foreach (var item in parameters.Where(t => t.Key != "sign").OrderBy(t => t.Key))
            {
                if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                {
                    query.Append(item.Key).Append(item.Value);
                }
            }
            if (qhs)
            {
                query.Append(secret);
            }

            return query.ToString().ToMD5();
        }

        public static string NowToMD5(string query)
        {
            string name = DateTime.Now.ToString("yyyyMMddHHmmssfff") + query;
            return ToMD5(name);
        }

        public static string ToMD5(this string query)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }

            return result.ToString();
        }

        public static string GetMD5FileName(string fileName)
        {
            string old = DateTime.Now.Ticks + System.IO.Path.GetFileNameWithoutExtension(fileName);
            var buffer = Encoding.UTF8.GetBytes(old);
            return ComputeHash(buffer) + System.IO.Path.GetExtension(fileName);
        }

        public static string GetMD5FileNameByExtension(string extension)
        {
            string old = DateTime.Now.Ticks.ToString();
            var buffer = Encoding.UTF8.GetBytes(old);
            return ComputeHash(buffer) + extension;
        }

        public static string ComputeHash(byte[] buffer)
        {
            var md5 = new MD5CryptoServiceProvider();
            var array = md5.ComputeHash(buffer);
            return Convert.ToBase64String(array);
        }
    }
}