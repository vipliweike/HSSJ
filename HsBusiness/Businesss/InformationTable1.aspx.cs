using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.Businesss
{
    public partial class InformationTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="Areas"></param>
        /// <param name="Industry"></param>
        /// <param name="Operator"></param>
        /// <param name="Scale"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        [WebMethod]
        public static string Get(int PageIndex = 1, string Areas = "", string Industry = "", string Operator = "", string Scale = "", string State = "")
        {
            //var data = "";

            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Business.Where(x => 1 == 1).Select(x => new
                {
                    x.Areas,
                    x.ID,
                    x.UserID,
                    x.Industry,
                    x.IsFixed,
                    x.IsMove,
                    x.AddTime,
                    x.CompanyName,
                    x.CustomerScale,
                    x.MOperator,
                    x.FOperator,
                    x.FScale,
                    x.FUseScale,                    
                    VisitNum=x.Visit.Sum(y=>y.BusID)
                });
                if (Areas != "")
                {
                    list = list.Where(x =>x.Areas==Areas);
                }
                if (Industry != "")
                {
                    list = list.Where(x => x.Industry == Industry);
                }
                if (Operator != "")
                {
                    list = list.Where(x => x.MOperator == Operator || x.FOperator == Operator);
                }
                if (Scale != "")
                {
                    list = list.Where(x =>x.CustomerScale==Scale|| x.FScale == Scale || x.FUseScale == Scale);
                }

                int total = list.Count();

                list = list.Skip((PageIndex > 0 ? PageIndex - 1 : 0) * 10).Take(10);
                var result = JsonConvert.SerializeObject(list);
                return result;
            }

        }
        [WebMethod]
        /// <summary>
        /// 查询移动业务
        /// </summary>
        /// <returns></returns>
        public static string GetMove(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Business.Where(x => x.ID == ID && x.IsMove == true).Select(x => new
                {
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
                    x.MRatio
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(list);
                return result;
            }
        }
        [WebMethod]
        /// <summary>
        /// 查询固网业务
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetFixed(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Business.Where(x => x.ID == ID && x.IsFixed == true).Select(x => new
                {
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
                    x.FWeekPrice
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(list);
                return result;                
            }
        }
        [WebMethod]
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string Delete(int ids)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var result = "";
                var list = db.Business.FirstOrDefault(x => x.ID == ids);
                db.Business.DeleteOnSubmit(list);
                OperateLog opermodel = new OperateLog();
                opermodel.Operator = list.Users.Name;//操作人
                opermodel.OperTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作时间
                opermodel.OperType = "删除";//操作类型
                opermodel.Operdescribe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + list.Users.Name + "进行了单个删除操作";
                db.OperateLog.InsertOnSubmit(opermodel);
                db.SubmitChanges();
                result = JsonConvert.SerializeObject(new { msg = "删除成功", stste = 1 });
                return result;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [WebMethod]
        public static string BatchDel(string ids)
        {
            using (dbDataContext db = new dbDataContext())
            {
                string result = "";
                string[] strArray = ids.Split(',');
                int[] intArray = Array.ConvertAll<string, int>(strArray, x => Convert.ToInt32(x));//将string数组类型转化成int数组
                var item = db.Business.Where(x => intArray.Contains(x.ID)).ToList();
                db.Business.DeleteAllOnSubmit(item);
                OperateLog opermodel = new OperateLog();
                opermodel.Operator = Common.TCContext.Current.OnlineRealName.ToString();//操作人
                opermodel.OperTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作时间
                opermodel.OperType = "批量删除";//操作类型
                opermodel.Operdescribe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Common.TCContext.Current.OnlineRealName + "进行了批量删除操作";
                db.SubmitChanges();
                
                result = JsonConvert.SerializeObject(new { msg = "删除成功", stste = 1 });
                return result;
            }
        }
    }
}