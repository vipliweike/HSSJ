using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace HsBusiness.Api.Common
{
    public class UrlAnalytical
    {
        /// <summary>
        /// 获取真实参数值
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static NameValueCollection GetRealParameters(string strData)
        {
            if (string.IsNullOrEmpty(strData)) return new NameValueCollection();
            string strRealParam = AES.Decrypt(strData, PublicConst.AesKey);
            string[] strArray = strRealParam.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            NameValueCollection parameters = new NameValueCollection();
            foreach (var str in strArray)
            {
                string[] subArray = str.Split(new[] { '=' }, 2);
                if (subArray.Length == 2)
                    parameters.Add(subArray[0], subArray[1]);
            }
            return parameters;
        }

        /// <summary>
        /// 获取未解码地址
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static string GetRealUrlAes(string strData)
        {
            return AES.Decrypt(strData, PublicConst.AesKey);
        }

        /// <summary>
        /// 判断加密值是否一致
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="strSecret"></param>
        /// <returns></returns>
        public static bool IsSecret(string strData, string strSecret)
        {
            string strMD5 = MD5.Encrypt(strData, 32);
            return strMD5.Equals(strSecret);
        }

    }

}