using HsBusiness.Api.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HsBusiness.Api.Filter
{
    public class DecrypAttribute:ActionFilterAttribute
    {
        public bool  IsCheck=false;
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!IsCheck)
            {
                var request = actionContext.Request;
                var method = request.Method.Method;
                var req = HttpContext.Current.Request;
                System.Collections.Specialized.NameValueCollection parameters = null;
                var keys = actionContext.ModelState.Keys;
                if (keys.Contains("data") && keys.Contains("secret"))
                {
                    var data = "";
                    var secret = "";
                    if (method.ToUpper() == "GET")
                    {

                        data = req.QueryString["data"];
                        secret = req.QueryString["secret"];
                    }
                    else if (method.ToUpper() == "POST")
                    {
                        data = req.Form["data"];
                        secret = req.Form["secret"];
                    }
                    //解析请求参数
                    parameters = UrlAnalytical.GetRealParameters(data);
                    if (!UrlAnalytical.IsSecret(UrlAnalytical.GetRealUrlAes(data), secret))
                    {
                        throw new Exception("加密信息不一致!");
                    }
                }
                
               
               
                
            }
            return;
        }
        
    }
}