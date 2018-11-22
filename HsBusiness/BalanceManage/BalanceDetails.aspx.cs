using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.BalanceManage
{
    public partial class BalanceDetails : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetOne(int BalID)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                var list = db.SmallBalance.Where(x => x.ID == BalID).Select(x => new
                {

                    x.ID,
                    x.AccessNumber,
                    x.AccountID,
                    x.AddTime,
                    x.Balance,
                    x.Broadband,
                    x.Contacts,
                    x.ContactTel,
                    x.CustomerName,
                    x.GroupID,
                    x.GroupName,
                    x.InstalledAddress,
                    x.NetTime,
                    x.Responsibility,
                    x.State,
                    x.WeekPrice,
                    UserName=x.Users.Name,
                    RemindTime = x.SmaBaRemind.OrderByDescending(y => y.AddTime).FirstOrDefault().AddTime,
                    IsRead = x.SmaBaRead.Where(y => y.UserID == user.ID).FirstOrDefault() == null ? 0 : x.SmaBaRead.Where(y => y.UserID == user.ID).FirstOrDefault().IsRead
                }).FirstOrDefault();
                return JsonConvert.SerializeObject(new { data=list,msg="请求成功", state = 1 });
            }
        }
    }
}