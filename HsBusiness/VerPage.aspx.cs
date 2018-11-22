using HsBusiness.Interface.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness
{
    public partial class VerPage : System.Web.UI.Page
    {
        protected Common.TCContext UserSession
        {
            get { return Common.TCContext.Current; }
        }
       
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            if (!User.Identity.IsAuthenticated)
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
                WebHelper.ShowAndRedirectTop(this, "登陆超时", url);

            }

        }
    }
}