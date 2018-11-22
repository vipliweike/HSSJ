using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness
{
    public partial class main : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Common.TCContext.Current.OnlineUserType == "公司领导" && Common.TCContext.Current.OnlineRealName == "admin")
            {
                this.person.Visible = true;//人员管理显示
                this.welcome.Visible = true;//商机概览显示
                this.news.Visible = false;//消息中心影藏
                this.currentPage.InnerHtml = "商机预览";
                this.reminder.Visible = true;//设置提醒
            }
            else
            {
                if (Common.TCContext.Current.OnlineUserType == "公司领导" || Common.TCContext.Current.OnlineUserType == "政企部主管")
                {
                    this.person.Visible = false;//人员管理隐藏
                    this.busShow.Visible = true;//商机业务报表
                    this.welcome.Visible = true;//商机概览显示
                    this.news.Visible = false;//消息中心影藏
                    this.currentPage.InnerHtml = "商机预览";
                    this.reminder.Visible = false;//设置提醒
                }
                if (Common.TCContext.Current.OnlineUserType == "区县经理")
                {
                    this.person.Visible = false;//人员管理隐藏
                    this.busShow.Visible = false;//商机业务报表
                    this.welcome.Visible = true;//商机概览显示
                    this.news.Visible = false;//消息中心影藏
                    this.currentPage.InnerHtml = "商机预览";
                    this.reminder.Visible = false;//设置提醒
                }
                if (Common.TCContext.Current.OnlineUserType == "客户经理" || Common.TCContext.Current.OnlineUserType == "网格助理" || Common.TCContext.Current.OnlineUserType == "行业经理")
                {
                    this.person.Visible = false;//人员管理隐藏
                    this.busShow.Visible = false;//商机业务报表
                    this.welcome.Visible = false;//商机概览隐藏
                    this.news.Visible = true;//消息中心显示
                    this.currentPage.InnerHtml = "消息中心";
                    this.reminder.Visible = false;//设置提醒
                }
            }
            
        }
    }
}