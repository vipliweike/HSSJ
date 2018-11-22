using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness
{
    public partial class Password_Modification : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        [WebMethod]
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public static string ModifyPwd(string pwd,string oldpwd)
        {
            using (dbDataContext db=new HsBusiness.dbDataContext())
            {
                //string currentpwd = HttpContext.Current.Session["CurrentPwd"].ToString();//获取当前用户登陆密码
                //var newpwditem = db.Users.FirstOrDefault(x => x.Pwd == currentpwd);
                var user = db.Users.FirstOrDefault(x => x.ID.Equals( int.Parse(Common.TCContext.Current.OnlineUserID)));
                var result="";
             
                if (user != null)
                {
                    if (Interface.Comm.MD5.Encrypt(oldpwd, 32)!= user.Pwd)
                    {
                        result = JsonConvert.SerializeObject(new { msg = "旧密码输入错误", state = 2 });                    
                    }
                    else
                    {
                        user.Pwd = Interface.Comm.MD5.Encrypt(pwd,32);//md5加密
                        db.SubmitChanges();
                        result = JsonConvert.SerializeObject(new { msg = "修改成功", state = 1 });
                        //HttpContext.Current.Session["CurrentPwd"] = newpwditem.Pwd;//修改完存session
                    }
                }
              
                else
                {
                    result = JsonConvert.SerializeObject(new { msg = "修改失败", state = 0 });
                }
                return result;
            }
        }
    }
}