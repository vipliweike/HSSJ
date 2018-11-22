using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.Specialline
{
    public partial class CollectionDetial : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

      
        /// <summary>
        /// 得到联系人信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string BasicInfo(int PLID)
        {
            using (dbDataContext db=new HsBusiness.dbDataContext ())
            {
                var list = db.PrivateLine.Where(x => x.ID == PLID).Select(x => new
                {
                    Contacts = x.PlContacts.Where(y => y.PlID == x.ID).Select(y => new
                    {
                        CompanyName = y.PrivateLine.CompanyName,
                        CompanyAddress = y.PrivateLine.CompanyAddress,
                        LastTime = y.PrivateLine.LastTime,
                        x.Remark,//备注
                        y.ID,
                        y.Name,
                        y.Post,
                        y.Tel
                    }).ToList()

                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }
        /// <summary>
        /// 基础信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetBasisInfo(int PLID)
        {
            using (dbDataContext db=new HsBusiness.dbDataContext ())
            {
                var list = db.PrivateLine.Where(x => x.ID == PLID).Select(x => new
                {
                    x.ID,
                    x.CompanyName,
                    x.CompanyAddress,
                    x.Remark,
                    x.AddTime


                });
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }

        }
        /// <summary>
        /// 专线信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string SpecialInfo(int PLID)
        {

            using (dbDataContext db=new HsBusiness.dbDataContext ())
            {
                var list = db.PlInfo.Where(x => x.PlID == PLID&&x.Type!=3).Select(x => new
                {
                    x.ID,
                    x.Operator,
                    x.WeekPrice,
                    x.BandWidth,
                    x.PayType,
                    x.OverTime,
                    State=x.State==0?"正常":x.State==1?"已提醒":x.State==2?"已回执":"",
                    Type=x.Type==1?"专线":"电路"

                }).OrderByDescending(x=>x.OverTime);
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string TyyInfo(int PLID)
        {
            using (dbDataContext db=new dbDataContext ())
            {
                var tyylist = db.PlInfo.Where(x => x.Type == 3 && x.PlID == PLID).Select(x => new
                {
                    x.ID,
                    IsCloudPlan=x.IsCloudPlan==0?"否":"是",
                    x.ServerBerSys,
                    x.ServerUsingTime

                });
                var result = JsonConvert.SerializeObject(new { data = tyylist, stste = 1, msg = "成功" });
                return result;
            }
        }
        /// <summary>
        /// 拜访记录
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string VisitInfo(int PLID)
        {
            using (dbDataContext db=new HsBusiness.dbDataContext ())
            {
                var list = db.PlVisit.Where(x => x.PlID == PLID).Select(x => new
                {
                    x.ID,
                    IsNeed=x.IsNeed==0?"否":"是",
                    IsAgain=x.IsAgain==0?"否":"是",
                    x.VisitTime
                }).ToList();
                var list1 = list.Select(x => new
                {
                    x.ID,
                    x.IsNeed,
                    x.IsAgain,
                    VisitTime=x.VisitTime!=null?Convert.ToDateTime(x.VisitTime).ToString("yyyy-MM-dd") :""
                }).OrderBy(x => x.VisitTime);
                var result = JsonConvert.SerializeObject(new { data = list1, stste = 1, msg = "成功" });
                return result;
            }
        }
        /// <summary>
        /// 回执记录
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string RemindInfo(int PLID)
        {
            using (dbDataContext db=new HsBusiness.dbDataContext ())
            {
                var list = db.PlRemind.Where(x => x.PlID == PLID&&x.State==1).Select(x => new
                {
                    x.ID,
                    x.Contents,
                    x.ReceiptTime,
                    Type = x.Type == 1 ? "专线到期" : x.Type == 2 ? "走访到期" : "",
                    x.AddTime
                }).ToList();
                var list1 = list.Select(x => new
                {
                    x.ID,
                    x.Contents,
                    ReceiptTime=x.ReceiptTime!=null?Convert.ToDateTime(x.ReceiptTime).ToString("yyyy-MM-dd") :"",
                    x.Type,
                    AddTime = x.AddTime != null ? Convert.ToDateTime(x.AddTime).ToString("yyyy-MM-dd"):""

                }).OrderByDescending(x => x.ReceiptTime);
                var result = JsonConvert.SerializeObject(new { data = list1, stste = 1, msg = "成功" });
                return result;

            }

        }

    }
}