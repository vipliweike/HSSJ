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
    public partial class StoreDetails : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 得到基础信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetInfo(int ID)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                string[] imgNULL = { };
                var list = db.Store.Where(x => x.ID == ID).Select(x => new
                {

                    Areas = x.Users.Areas,
                    x.Broadband,
                    x.StoreAddress,
                    x.Price,
                    x.OverTime,
                    CusManage = x.Users.Name,
                    x.ContactName,
                    x.ContactTel,
                    x.StoreName,
                    x.LastTime,
                    Img = x.Img != null ? x.Img.Split(';') : imgNULL,
                    x.State,
                    RemindID = x.StoreRemind.Where(y => y.Type == 1 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault() != null ?
                                   x.StoreRemind.Where(y => y.Type == 1 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault().RemindID
                                   : 0,//宽带到期提醒

                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }


        }
        /// <summary>
        /// 得到走访记录信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetVistInfo(int ID)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                string[] imgNULL = { };
                var list = db.StoreVisit.Where(x => x.StoreID == ID).Select(x => new
                {
                    x.NextTime,
                    x.State,
                    x.VisitContent,
                    FZR = x.Store.Users.Name,
                    FZrTel = x.Store.Users.Mobile,
                    x.IsNeed,
                    x.IsAgain,
                    x.VisitTime,
                    Img = x.VisitImg != null ? x.VisitImg.Split(';') : imgNULL,
                    RemindID = x.StoreRemind.Where(t => t.State == 0).OrderByDescending(t => t.ID).Select(t => new { RemindID = t.ID, t.AddTime, }).FirstOrDefault() != null ?
                                            x.StoreRemind.Where(t => t.State == 0).OrderByDescending(t => t.ID).Select(t => new { RemindID = t.ID, t.AddTime, }).FirstOrDefault().RemindID
                                            : 0,//走访提醒

                }).OrderBy(x => x.VisitTime);
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }
        /// <summary>
        /// 回执记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetRemind(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {

                var list = db.Store.Where(x => x.ID == ID).Select(x => new
                {
                    Remind = x.StoreRemind.Where(y => y.State == 1).Select(y => new
                    {
                        y.Contents,//回执内容
                        y.AddTime,//提醒时间
                        y.Type,//提醒内容
                        UserName = y.Store.Users.Name,//负责人
                        UserTel = y.Store.Users.Mobile,//手机
                        y.ReceiptTime//回执时间
                    })
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }

        /// <summary>
        /// 回执
        /// </summary>
        /// <param name="Contents"></param>
        /// <param name="RemindID"></param>
        /// <returns></returns>
        [WebMethod]
        public static string ReminderReceipt(string Contents, int RemindID)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var sr = db.StoreRemind.Where(x => x.ID == RemindID).FirstOrDefault();
                if (sr != null)
                {
                    sr.Contents = Contents;
                    sr.ReceiptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    var store = db.Store.Where(x => x.ID == sr.StoreID).FirstOrDefault();//商机                        
                    if (store != null)
                    {
                        if (sr.Type == 1)//宽带到期回执
                        {
                            store.State = 2;
                            store.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (sr.Type == 2)//回访回执
                        {
                            var vis = db.StoreVisit.Where(x => x.ID == sr.StoreVisitID).FirstOrDefault();
                            if (vis != null)
                            {
                                vis.State = 2;
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { state = 0, msg = "回访记录不存在" });
                            }
                        }
                        sr.State = 1;//是否回执
                        db.SubmitChanges();

                        return JsonConvert.SerializeObject(new { state = 1, msg = "回执成功" });
                    }
                    return JsonConvert.SerializeObject(new { state = 1, msg = "门店不存在" });
                }

                return JsonConvert.SerializeObject(new { state = 0, msg = "提醒不存在" });
            }
        }
    }
}