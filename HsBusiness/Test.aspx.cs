using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pushUrl = "https://bjapi.push.jiguang.cn/v3/push";
            string JsonString = "";
            if (true)
            {
                JsonString = new JObject(
                                              new JProperty("platform", "all"),
                                              new JProperty("audience", new JObject(new JObject(
                                                    new JProperty("alias", new JArray(150))
                                                  ))),

                                              new JProperty("notification", new JObject(
                                                  new JProperty("android", new JObject(new JProperty("alert", "移动到期"),
                                                    new JProperty("extras",
                                                        new JObject(
                                                            new JProperty("ID", 779),
                                                            new JProperty("Type", 1)))))
                                                  ))
                                              ).ToString();
                Common.PostHelper.PostData(pushUrl, JsonString);
            }
            //using (dbDataContext db = new dbDataContext())
            //{
            //    var time = db.Config.Where(x => 1 == 1);//

            //    var MExpire = Convert.ToInt32(time.Where(y => y.Name == "商机移动到期时间").FirstOrDefault().Numerical);
            //    var mlist = db.Business.Where(x => (bool)x.IsMove);
            //    mlist = db.Business.Where(x => x.MOverTime != "" && x.MState == 0 && Convert.ToDateTime(x.MOverTime).AddHours(-MExpire) <= DateTime.Now);//

            //    if (mlist.Count() == 0)
            //    {

            //    }
            //    else
            //    {
            //        foreach (var item in mlist)
            //        {
            //            var BusUserIDList = mlist.Select(x => x.UserID);

            //            #region Url及数据
            //            string pushUrl = "https://bjapi.push.jiguang.cn/v3/push";
            //            string JsonString = "";
            //            if (BusUserIDList.Count() > 0)
            //            {
            //                JsonString = new JObject(
            //                                               new JProperty("platform", "all"),
            //                                               new JProperty("audience", new JObject(new JObject(
            //                                                   new JProperty("tag", new JArray(item.UserID))
            //                                                   ))),
            //                                               new JProperty("notification", new JObject(new JProperty("alert", item.CompanyName+"商机移动到期")))
            //                                               ).ToString();
            //                //Common.PostHelper.PostData(pushUrl, JsonString);
            //            }

            //            #endregion
            //        }

            //    }



        }

    }

}