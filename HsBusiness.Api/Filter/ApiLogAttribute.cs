using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HsBusiness.Api.Filter
{
    public class ApiLogAttribute : ActionFilterAttribute
    {


        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {


            var context = HttpContext.Current;
            var req = context.Request;

            var route = actionExecutedContext.ActionContext.RequestContext.RouteData.Values;//路由
            if (route["controller"].ToString() != "Users" && route["action"].ToString() != "Login")//登录接口除外的所有接口
            {
                var param = actionExecutedContext.ActionContext.ActionArguments;//参数
                NameValueCollection parameters = new NameValueCollection();
                if (req.RequestType == "GET")
                {
                    parameters = Common.AesDecryp.GetAesDecryp(param["data"].ToString(), param["secret"].ToString());//真实参数
                }
                if (req.RequestType == "POST")
                {
                    dynamic pa = null;
                    
                    //带图片的/controller/action{   /Visit/Add   /Store/Add  /Store/Modify    /StoreVisit/Add  }
                    var ca = "/" + route["controller"].ToString() + "/" + route["action"].ToString();
                    if ((ca != "/Visit/Add") && (ca != "/Store/Add") && (ca != "/Store/Modify") && (ca != "/StoreVisit/Add")&&(ca!="/ProVisit/Add")&&(ca!= "/PlVisit/Add"))//不是不带图片的接口
                    {
                        pa = param["req"];
                        parameters = Common.AesDecryp.GetAesDecryp(pa.data.ToString(), pa.secret.ToString());//真实参数
                    }
                    else//带图片的接口
                    {
                        var reqForm = req.Form;
                        pa = reqForm.AllKeys.ToDictionary(k => k, v => reqForm[v]);
                        parameters = Common.AesDecryp.GetAesDecryp(pa["data"].ToString(), pa["secret"].ToString());//真实参数
                    }
                }
                var ex = actionExecutedContext.Exception;//异常
                using (dbDataContext db = new dbDataContext())
                {
                    ApiLog log = new ApiLog();
                    log.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//添加时间
                    log.ClientType = 0;//安卓
                    log.Code = (int?)context.Response.StatusCode;
                    log.ErrMsg = ex != null ? ex.Message.ToString() + "\r\n" + ex.StackTrace : "";//异常信息
                    log.Parameters = Newtonsoft.Json.JsonConvert.SerializeObject(parameters.AllKeys.ToDictionary(k => k, k => parameters[k]));
                    log.RequestName = "/" + route["controller"].ToString() + "/" + route["action"].ToString();
                    log.UserID = Convert.ToInt32(parameters["UserID"]);

                    db.ApiLog.InsertOnSubmit(log);
                    db.SubmitChanges();
                }
            }

        }


    }
}