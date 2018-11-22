using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace HsBusiness.Api.Common
{
    public static class AesDecryp
    {
        public static NameValueCollection GetAesDecryp(string data,string secret)
        {
            var parameters = Common.UrlAnalytical.GetRealParameters(data);
            if (!UrlAnalytical.IsSecret(UrlAnalytical.GetRealUrlAes(data), secret))
            {
                throw new Exception("加密信息不一致!");
            }            
            return parameters;
        }
    }
}