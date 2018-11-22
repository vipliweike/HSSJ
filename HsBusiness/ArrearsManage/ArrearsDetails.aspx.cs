using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.ArrearsManage
{
    public partial class ArrearsDetails : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetOne(int ArrID)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                var list = db.Arrears.Where(x => x.ID == ArrID).Select(x => new
                {

                    x.ID,
                    x.AmountOwed,
                    x.Area,
                    x.CustomerName,
                    x.InNetTime,
                    x.Payment,
                    x.Period,
                    x.ServiceStatus,
                    x.UserNumber,
                    x.UserTypeItem,
                    UserName = x.Users.Name,
                    x.InstalledAddress,//装机地址
                    x.ContactTel,
                    x.State,
                    x.UserID,
                    x.AddTime,
                    RemindTime = x.ArrRemind.OrderByDescending(y => y.AddTime).FirstOrDefault().AddTime,
                    IsRead = x.ArrRead.Where(y => y.UserID == user.ID).FirstOrDefault() == null ? 0 : x.ArrRead.Where(y => y.UserID == user.ID).FirstOrDefault().IsRead
                }).FirstOrDefault();
                return JsonConvert.SerializeObject(new { data = list, msg = "请求成功", state = 1 });
            }
        }
    }
}