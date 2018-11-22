using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApplication3
{
    public class ParamsOrder
    {
        /// <summary>
        /// 获取排序后参数
        /// </summary>
        public static string ParamsOrderStr(string strParams)
        {
            Dictionary<string, string> dics = new Dictionary<string, string>();
            var paramArra = strParams.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in paramArra)
            {
                dics.Add(str.Split('=')[0], str.Split('=')[1]);
            }

            return getParamSrc(dics);
        }

        public static string getParamSrc(Dictionary<string, string> paramsMap)
        {
            var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vDic)
            {
                string pkey = kv.Key;
                string pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }
            string result="";
            if (str.Length > 0)
            {
                result = str.ToString().Substring(0, str.ToString().Length - 1);
            }
            return result;
        }

    }
}