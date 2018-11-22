using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace HsBusiness
{
    public partial class Login : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        [System.Web.Services.WebMethod(enableSession: true)]
        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        public static string Login1(string UserName,string Password,string code)
        {
            
            using (dbDataContext db=new HsBusiness.dbDataContext ())
            {
                var result = "";
                //var pwd = Interface.Comm.MD5.Encrypt(Password, 32);
                var loginuser = db.Users.FirstOrDefault(x =>x.Mobile== UserName && x.Pwd == Password);
                var exitlogin = db.Users.FirstOrDefault(x=>x.Mobile==UserName);
                if (!code.ToUpper().Equals(HttpContext.Current.Request.Cookies["yzm"].Value))
                {
                    result = JsonConvert.SerializeObject(new { msg = "验证码错误", state = 4 });
                    //return;
                    return result;
                }
                if (loginuser != null)
                {
                    
                    result= JsonConvert.SerializeObject(new { msg = "登陆成功", state = 1 });
                    OperateLog opermodel = new OperateLog();//日志记录
                    opermodel.Operator = loginuser.Name;//操作人
                    opermodel.OperTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作时间
                    opermodel.OperType = "登陆";//操作类型
                    opermodel.Operdescribe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + loginuser.Name + "登陆该系统";//操作描述
                    db.OperateLog.InsertOnSubmit(opermodel);
                    db.SubmitChanges();
                    //HttpContext.Current.Session["CurrentUerName"] = loginuser.Name;
                    //HttpContext.Current.Session["CurrentPwd"] = loginuser.Pwd;
                    //HttpContext.Current.Session["CurrentUer"] = loginuser;//登陆信息存入session
                    FormsAuthentication.SetAuthCookie(loginuser.ID.ToString(), false);
                    Common.TCContext.Current.OnlineUserNickName = loginuser.Mobile;
                    Common.TCContext.Current.OnlineRealName = loginuser.Name;
                    Common.TCContext.Current.OnlineUserID = loginuser.ID.ToString();
                    Common.TCContext.Current.OnlineUserType = loginuser.Roles.RoleName.ToString();
                    Common.TCContext.Current.OnlineArea = loginuser.Areas.ToString();
                    if (loginuser.Pwd == "123456")
                    {
                        result = JsonConvert.SerializeObject(new { msg = "请修改密码", state = 2 });
                    }
                   
                }
                else
                {
                    if (exitlogin==null)
                    {
                        result = JsonConvert.SerializeObject(new { msg = "用户名不存在", state = 3 });
                    }
                    
                    else
                    {
                        result = JsonConvert.SerializeObject(new { msg = "用户名或密码错误", state = 0 });
                    }

                }
                return result;
            }

        }
        [WebMethod]
        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <returns></returns>
        public static string ExitLogin()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Clear();
            Common.TCContext.Current.OnlineUserNickName = string.Empty;
            Common.TCContext.Current.OnlineRealName = string.Empty;
            Common.TCContext.Current.OnlineUserID = string.Empty;
            Common.TCContext.Current.OnlineUserType = string.Empty;
            Common.TCContext.Current.OnlineArea = string.Empty;
            var result = "";
            //HttpContext.Current.Session.Clear();
            //HttpContext.Current.Session.RemoveAll();
            //HttpContext.Current.Session.Clear();
            result = JsonConvert.SerializeObject(new { msg = "注销成功", state = 1 });
            return result;
        }

      /// <summary>
      /// 登陆
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>

        protected void Button1_Click(object sender, EventArgs e)
        {
            ClientScriptManager scriptManager = ((Page)System.Web.HttpContext.Current.Handler).ClientScript;
           
            try
            {
                string code = Session["yzm"].ToString();
                if (txtCode.Text.Trim().ToUpper()!= code)
                {
                    Session["yzm"] = "sss";
                  
                    Response.Write("<script>alert('验证码错误！')</script>");
                    //scriptManager.RegisterStartupScript(typeof(string), "", "alert('验证码错误');", true);
                    return;
                }

                using (dbDataContext db = new dbDataContext())
                {
                    string name = txtUserName.Text.Trim();
                    string pwd = txtPwd.Text.Trim();

                    var loginer = db.Users.FirstOrDefault(t => t.Mobile == name && t.Pwd == pwd);
              
                    if (loginer == null)
                    {
                        Response.Write("<script>alert('用户名或密码错误！')</script>");
                        //scriptManager.RegisterStartupScript(typeof(string), "", "alert('用户名或密码错误');", true);
                        //CTClass.Comm.runClientScript("msg('用户名或密码错误！')");
                        return;
                    }
                  
                    OperateLog opermodel = new OperateLog();//日志记录
                    opermodel.Operator = loginer.Name;//操作人
                    opermodel.OperTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作时间
                    opermodel.OperType = "登陆";//操作类型
                    opermodel.Operdescribe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + loginer.Name + "登陆该系统";//操作描述
                    db.OperateLog.InsertOnSubmit(opermodel);
                    db.SubmitChanges();
                    //HttpContext.Current.Session["CurrentUerName"] = loginuser.Name;
                    //HttpContext.Current.Session["CurrentPwd"] = loginuser.Pwd;
                    //HttpContext.Current.Session["CurrentUer"] = loginuser;//登陆信息存入session
                    FormsAuthentication.SetAuthCookie(loginer.ID.ToString(), false);
                    Common.TCContext.Current.OnlineUserNickName = loginer.Mobile;
                    Common.TCContext.Current.OnlineRealName = loginer.Name;
                    Common.TCContext.Current.OnlineUserID = loginer.ID.ToString();
                    Common.TCContext.Current.OnlineUserType = loginer.Roles.RoleName.ToString();
                    Common.TCContext.Current.OnlineArea = loginer.Areas.ToString();

                    Response.Redirect("Main.aspx", false);
                    if (loginer.Pwd == Interface.Comm.MD5.Encrypt("123456", 32))
                    {
                        Response.Write("<script>alert('请修改密码！')</script>");
                        Response.Redirect("Main.aspx?opr=pwd", false);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //CTClass.Comm.runClientScript("msg('" + ex.Message + "')");
                Response.Write("<script>alert('" + ex.Message + "！')</script>");
                //scriptManager.RegisterStartupScript(typeof(string), "", "alert('"+ex.Message+"');", true);
            }
        }
    
    }
}