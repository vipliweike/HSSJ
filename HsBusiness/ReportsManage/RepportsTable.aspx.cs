using Newtonsoft.Json;
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
    public partial class RepportsTable : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="Grids"></param>
        /// <param name="Search"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [WebMethod]
        public static string Get(string Areas, string Search, string pageIndex,string mouth)
        {
            using (dbDataContext db = new dbDataContext())
            {

                int pageSize = 10;
                var list = db.Reports.Where(x =>x.Month==mouth);
                if (!string.IsNullOrEmpty(Search))
                {
                    list = list.Where(t => t.Name.Contains(Search) || t.Grids.Contains(Search));
                }
                //if (!string.IsNullOrEmpty(Month))
                //{
                //    list = list.Where(x => x.Month == Month);
                //}
                if (!string.IsNullOrEmpty(Areas))
                {
                    var areaUserList = db.Users.Where(x => x.Areas == Areas).Select(x => x.Name).ToList();//区县内所有人
                    list = list.Where(x => areaUserList.Contains(x.Name));
                }
                var resultList = list
                    .OrderByDescending(x => x.Month)
                    .ThenBy(x => x.Rank)
                    .Skip((int.Parse(pageIndex) - 1) * pageSize).Take(pageSize).Select(x => new
                    {
                        x.ID,
                        x.Grids,
                        x.Month,
                        x.Name,
                        x.Rank,
                        x.NewIncome,
                        x.R0Aims,
                        x.R0CompleteRate,
                        x.R2Aims,
                        x.R2CompleteRate,
                        x.StockIncome,
                        x.StockIncomeRate,
                        x.TotalIncome,
                    }).ToList()
                .Select(x => new
                {
                    x.ID,
                    x.Grids,
                    x.Month,
                    x.Name,
                    x.Rank,
                    NewIncome = Convert.ToDouble(x.NewIncome).ToString("0.00"),
                    R0Aims = Convert.ToDouble(x.R0Aims).ToString("0.00"),
                    R0CompleteRate = (Convert.ToDouble(x.R0CompleteRate) * 100).ToString("0.00"),
                    R2Aims = Convert.ToDouble(x.R2Aims).ToString("0.00"),
                    R2CompleteRate = (Convert.ToDouble(x.R2CompleteRate) * 100).ToString("0.00"),
                    StockIncome = Convert.ToDouble(x.StockIncome).ToString("0.00"),
                    StockIncomeRate = (Convert.ToDouble(x.StockIncomeRate) * 100).ToString("0.00"),
                    TotalIncome = Convert.ToDouble(x.TotalIncome).ToString("0.00"),
                });

                var result = new { status = "1", data = resultList, pagecount = list.Count().ToString(), pagesize = pageSize.ToString() };
                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }

        }

        [WebMethod]
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <returns></returns>
        public static string Delete(int id)
        {
            using (dbDataContext db = new dbDataContext())
            {
                string result = "";
                var rep = db.Reports.FirstOrDefault(x => x.ID == id);//用户信息

                db.Reports.DeleteOnSubmit(rep);

                db.SubmitChanges();
                result = JsonConvert.SerializeObject(new { msg = "删除成功", state = 1 });
                return result;
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string BatchDel(string ids)
        {
            using (dbDataContext db = new dbDataContext())
            {


                string result = "";
                string[] strArray = ids.Split(',');
                int[] intArray = Array.ConvertAll<string, int>(strArray, x => Convert.ToInt32(x));
                var item = db.Reports.Where(x => intArray.Contains(x.ID)).ToList();
                db.Reports.DeleteAllOnSubmit(item);

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