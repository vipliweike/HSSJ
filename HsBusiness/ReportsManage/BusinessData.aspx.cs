using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.ReportsManage
{
    public partial class BusinessData : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 得到列表数据
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Get(string Month,string Search,string pageIndex)
        {
            using (dbDataContext db=new dbDataContext ())
            {
                int pageSize = 10;
                var list = db.Reports.Where(x => 1 == 1).Select(x=>new {x.ID, x.AddTime,x.Month, UserName = db.Users.Where(y => y.ID ==x.UserID).Select(y => new { y.Name }) });
                if (!string.IsNullOrEmpty(Month))
                {
                    list = list.Where(x => x.Month == Month);
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    list = list.Where(x=>x.UserName.FirstOrDefault().Name.Contains(Search));
                }
                var list1 = list.GroupBy(x => x.Month);
                var resultList = list1.Select(x => new { x.FirstOrDefault().Month, SalesManNumber = x.Count(), x.FirstOrDefault().UserName.FirstOrDefault().Name, x.FirstOrDefault().AddTime }).OrderByDescending(x=>x.Month).Skip((int.Parse(pageIndex) - 1) * pageSize).Take(pageSize);
                var result = new { status = "1", data = resultList, pagecount = list1.Count().ToString(), pagesize = pageSize.ToString() };
                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }
        }

    }
}