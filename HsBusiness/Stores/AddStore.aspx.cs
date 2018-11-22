using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.Stores
{
    public partial class AddStore : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Add(string StoreName,string StoreAddress, string Broadband,string Price,string OverTime,string ContactName,string ContactTel,string OtherNeeds)
        {
            using (dbDataContext db=new HsBusiness.dbDataContext())
            {
                var item = new Store();
                item.StoreName = StoreName;
                item.StoreAddress = StoreAddress;
                item.Broadband = Broadband;
                item.Price = Price;
                item.OtherNeeds = OtherNeeds;
                item.State = 0;
                item.OverTime = OverTime;
                item.ContactName = ContactName;
                item.ContactTel = ContactTel;
                item.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                item.UserID = int.Parse(Common.TCContext.Current.OnlineUserID);
                db.Store.InsertOnSubmit(item);
                db.SubmitChanges();
                var result = new { msg = "添加成功", state = 1 };
                return new JavaScriptSerializer().Serialize(result);
            }
        }
    }
}