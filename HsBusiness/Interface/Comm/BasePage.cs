using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HsBusiness.Interface.Comm
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (string.IsNullOrEmpty(Common.TCContext.Current.OnlineUserID))
            {
                string url = "";
                string[] path = Server.MapPath("").Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (path[path.Length - 1].Equals("HsBusiness"))
                {
                    url = "Login.aspx";
                }
                else
                {
                    url = "../Login.aspx";
                }
                Response.Write("<script>alert(\"登录超时或非法登录！\");location.href='"+url+"';</script>");
                Response.End();
                //Response.Write("<script>alert('登录超时或非法登录')</script>");
                //Response.Redirect(url,false);
            }            
            return;
        }
    }
}