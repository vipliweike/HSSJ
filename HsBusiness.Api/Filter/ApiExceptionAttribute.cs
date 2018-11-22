using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Collections.Specialized;

namespace HsBusiness.Api.Filter
{
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Common.SaveLog.WriteLog(path + "/Logs/", DateTime.Now.ToString("yyyy-MM-dd") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + actionExecutedContext.Exception.ToString() + "\r\n\r\n");
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK,
                new
                {
                    state = 0,
                    msg = actionExecutedContext.Exception.Message.ToString()
                });
        }
    }
}