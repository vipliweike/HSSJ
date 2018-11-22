using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.Businesss
{
    public partial class Details : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string GetInfo(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {

                var list = db.Business.Where(x => x.ID == ID).Select(x => new
                {
                    x.ID,
                    x.Industry,
                    x.IsFixed,
                    x.IsMove,
                    x.Areas,
                    x.Levels,//等级
                    x.AddTime,
                    x.LastTime,//最后更新时间
                    x.CompanyName,
                    x.CustomerScale,
                    x.CompanyAddress,
                    x.IsHavePhoneList,
                    x.Remark,//备注

                    Contacts = x.Contacts.Select(y => new { y.Name, y.Post, y.Tel })
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }



        [WebMethod]
        public static string GetMove(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {

                var list = db.Business.Where(x => x.ID == ID).Select(x => new
                {
                    x.ID,
                    x.IsMove,
                    //移动
                    x.MAgeGroup,

                    x.MCardUse,
                    x.MFocus,
                    x.MIncome,
                    x.MIsSubsidy,
                    x.MMonthFee,
                    x.MOperator,
                    x.MOtherWork,
                    x.MOverTime,
                    x.MPolicy,
                    x.MPushWork,
                    x.MQuota,
                    x.MRange,
                    x.MRatio,
                    x.MMobile,
                    x.MTelecom,
                    x.MUnicom,
                    x.MState,
                    RemindID = x.BusRemind.Where(y => y.Type == 1 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault() != null ?
                                            x.BusRemind.Where(y => y.Type == 1 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault().RemindID
                                            : 0,
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }
        [WebMethod]
        public static string GetFixed(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {

                var list = db.Business.Where(x => x.ID == ID).Select(x => new
                {
                    x.ID,
                    x.IsFixed,
                    //固网
                    x.FAlsAnlIncome,
                    x.FBandWidth,
                    x.FComputerNum,
                    x.FIsDomain,
                    x.FIsSatisfy,
                    x.FIsServer,
                    x.FOperator,
                    x.FOtherWork,
                    x.FOverTime,
                    x.FPlatform,
                    x.FPreAnlIncome,
                    x.FPreInNetMouth,
                    x.FPreWeekPrice,
                    x.FPushWork,
                    x.FScale,
                    x.FUseBandWidth,
                    x.FUseScale,
                    x.FUseWork,
                    x.FWeekPrice,
                    x.FState,
                    RemindID = x.BusRemind.Where(y => y.Type == 2 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault() != null ?
                                            x.BusRemind.Where(y => y.Type == 2 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault().RemindID
                                            : 0,
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }

        [WebMethod]
        public static string GetVisit(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {
                string[] imgNULL = { };
                var list = db.Business.Where(x => x.ID == ID).Select(x => new
                {
                    Visit = x.Visit.Select(y => new
                    {
                        y.IsAgain,
                        y.IsLeader,
                        y.IsNeed,
                        y.Leader,
                        y.NextTime,
                        y.State,
                        Img = y.Img != null ? y.Img.Split(';') : imgNULL,
                        y.VisitTime,
                        y.VisitContent,
                        UserName = y.Users.Name,
                        UserTel = y.Users.Mobile,
                        RemindID = y.BusRemind.Where(t => t.Type == 3 && t.State == 0).OrderByDescending(t => t.ID).Select(t => new { RemindID = t.ID, t.AddTime, }).FirstOrDefault() != null ?
                                                      y.BusRemind.Where(t => t.Type == 3&&t.State == 0).OrderByDescending(t => t.ID).Select(t => new { RemindID = t.ID, t.AddTime, }).FirstOrDefault().RemindID
                                                      : 0,//走访提醒
                    })
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }

        [WebMethod]
        public static string GetRemind(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {

                var list = db.Business.Where(x => x.ID == ID).Select(x => new
                {
                    Remind = x.BusRemind.Where(y => y.State == 1).Select(y => new
                    {
                        y.AddTime,
                        y.Contents,
                        y.State,
                        y.Type,
                        UserName = y.Users.Name,
                        UserTel = y.Users.Mobile,
                        y.ReceiptTime
                    })
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }

        [WebMethod]
        public static string ReminderReceipt(string Contents, int RemindID)
        {

            using (dbDataContext db = new dbDataContext())
            {
                if (!string.IsNullOrEmpty(Common.TCContext.Current.OnlineUserID))
                {
                    var br = db.BusRemind.Where(x => x.ID == RemindID).FirstOrDefault();
                    if (br != null)
                    {
                        br.Contents = Contents;
                        br.UserID = Convert.ToInt32(Common.TCContext.Current.OnlineUserID);
                        br.ReceiptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var bus = db.Business.Where(x => x.ID == br.BusID).FirstOrDefault();//商机                        
                        if (bus != null)
                        {
                            if (br.Type == 1)//移动回执
                            {
                                bus.MState = 2;
                                bus.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (br.Type == 2)//固网回执
                            {
                                bus.FState = 2;
                                bus.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (br.Type == 3)//回访回执
                            {
                                var vis = db.Visit.Where(x => x.ID == br.VisitID).FirstOrDefault();
                                if (vis != null)
                                {
                                    vis.State = 2;
                                }
                                else
                                {
                                    return JsonConvert.SerializeObject(new { state = 0, msg = "回访记录不存在" });
                                }
                            }
                            br.State = 1;//是否回执
                            db.SubmitChanges();

                            return JsonConvert.SerializeObject(new { state = 1, msg = "回执成功" });
                        }
                        return JsonConvert.SerializeObject(new { state = 1, msg = "商机不存在" });
                    }
                    return JsonConvert.SerializeObject(new { state = 1, msg = "提醒不存在" });
                }
                return JsonConvert.SerializeObject(new { stste = 0, msg = "回执失败，请重新登陆" }); ;
            }
        }

    }
}