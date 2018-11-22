using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.Stores
{
    public partial class VisitNewly : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 添加走访记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [WebMethod]
        public static string Add(string data)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var model = JsonConvert.DeserializeObject<StoreVisit>(data);

                model.VisitTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                model.AddTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                model.State = 0;

                db.StoreVisit.InsertOnSubmit(model);
                db.SubmitChanges();
                var result = JsonConvert.SerializeObject(new { state = 1, msg = "添加成功" });
                return result;
            }
        }
    }
}