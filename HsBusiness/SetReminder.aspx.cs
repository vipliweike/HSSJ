using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness
{
    public partial class SetReminder : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 读取设置提醒
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetReminder()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Config.Where(x => 1 == 1).ToList();
                return new JavaScriptSerializer().Serialize(list);
            }

        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="MExpire"></param>
        /// <param name="FExpire"></param>
        /// <param name="VExpire"></param>
        /// <returns></returns>
        [WebMethod]
        public static string Edit(string data)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);//将json对象转换成字典
            using (var db = new dbDataContext())
            {
                foreach (var item in dic)
                {
                    var list = db.Config.Where(x => x.EnlishName == item.Key).FirstOrDefault();
                    if (list != null)
                    {
                        list.Numerical = item.Value;
                    }
                }
                db.SubmitChanges();

            }

            var result = JsonConvert.SerializeObject(new { msg = "修改成功", state = 1 });
            return result;
        }
    }
}