using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.Information
{
    public partial class News : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 得到商机提醒
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetBusRemind()
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.BusRemind.Where(x => x.Business.UserID == int.Parse(Common.TCContext.Current.OnlineUserID) && x.State == 0).Select(x => new
                {
                    ID = x.BusID,
                    CompanyName = x.Business.CompanyName,
                    x.Type,
                    x.AddTime,

                }).OrderByDescending(x => x.AddTime).Take(5).ToList();
                var list1 = list.Select(x => new
                {
                    x.ID,
                    //1移动提醒	2固网提醒  3回访提醒  4小余额
                    x.CompanyName,
                    Type = x.Type == 1 ? "移动提醒" : x.Type == 2 ? "固网提醒" : x.Type == 3 ? "回访提醒" : x.Type == 4 ? "小余额" : "",
                    AddTime = string.IsNullOrEmpty(x.AddTime) ? "" : Convert.ToDateTime(x.AddTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    Number = list.Count()
                });
                var result = new { state = 1, data = list1 };

                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }
        }

        /// <summary>
        /// 门店提醒
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetStoreRemind()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.StoreRemind.Where(x => x.Store.UserID == int.Parse(Common.TCContext.Current.OnlineUserID) && x.State == 0).Select(x => new
                {
                    x.StoreID,
                    StoreName = x.Store.StoreName,
                    x.Type,
                    x.AddTime

                }).OrderByDescending(x => x.AddTime).Take(5).ToList();
                var list1 = list.Select(x => new
                { //1宽带到期 	2拜访预约
                    x.StoreID,
                    x.StoreName,
                    Type = x.Type == 1 ? "宽带到期" : x.Type == 2 ? "拜访预约" : "",
                    AddTime = string.IsNullOrEmpty(x.AddTime) ? "" : Convert.ToDateTime(x.AddTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    Number = list.Count()
                });
                var result = new { state = 1, data = list1 };

                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }
        }
        /// <summary>
        /// 专线提醒
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetZxRemind()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.PlRemind.Where(x => x.PrivateLine.UserID == int.Parse(Common.TCContext.Current.OnlineUserID) && x.State == 0).Select(x => new
                {
                    x.PlID,
                    CompanyName = x.PrivateLine.CompanyName,
                    x.Type,
                    x.AddTime

                }).OrderByDescending(x => x.AddTime).Take(5).ToList();
                var list1 = list.Select(x => new
                { //1宽带到期 	2拜访预约
                    x.PlID,
                    x.CompanyName,
                    Type = x.Type == 1 ? "专线" : x.Type == 2 ? "走访" : "",
                    AddTime = string.IsNullOrEmpty(x.AddTime) ? "" : Convert.ToDateTime(x.AddTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    Number = list.Count()
                });
                var result = new { state = 1, data = list1 };

                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }
        }

        /// <summary>
        /// 项目派单提醒
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string ProRemind()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.ProRemind.Where(x => x.Project.UserID == int.Parse(Common.TCContext.Current.OnlineUserID) && x.State == 0).Select(x => new
                {
                    ProjectName = x.Project.Introduce,
                    x.Type,
                    x.AddTime

                }).OrderByDescending(x => x.AddTime).Take(5).ToList();
                var list1 = list.Select(x => new
                {
                    //1派单提醒	2走访提醒
                    x.ProjectName,
                    Type = x.Type == 1 ? "派单提醒" : x.Type == 2 ? "走访提醒" : "",
                    AddTime = string.IsNullOrEmpty(x.AddTime) ? "" : Convert.ToDateTime(x.AddTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    Number = list.Count()
                });
                var result = new { state = 1, data = list1 };

                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }
        }

        /// <summary>
        /// 小余额提醒
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Balance()
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.SmallBalance.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID))
                    .Where(x => x.SmaBaRead.Where(y => y.UserID == int.Parse(Common.TCContext.Current.OnlineUserID) && y.IsRead == 0).Count() > 0 || x.SmaBaRead.Where(y => y.UserID == int.Parse(Common.TCContext.Current.OnlineUserID)).Count() == 0)
                    .Select(x => new
                    {
                        x.ID,
                        x.CustomerName,
                        x.Balance,
                        AddTime = x.AddTime,
                        IsRead = x.SmaBaRead.Where(y => y.UserID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault()

                    }).OrderByDescending(x => x.AddTime).Take(5);
                var list1 = list.ToList().Select(x => new
                {
                    x.ID,
                    x.CustomerName,
                    Balance = Convert.ToDouble(x.Balance).ToString("0.00"),
                    AddTime = string.IsNullOrEmpty(x.AddTime) ? "" : Convert.ToDateTime(x.AddTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    Number = list.Count(),
                    IsRead = x.IsRead == null ? 0 : x.IsRead.IsRead,
                });
                var result = new { state = 1, data = list1 };

                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }
        }
        /// <summary>
        /// 当月项目派单处理
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string MonthlyProject()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();

                var prolist = db.Project.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString() && x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));//派单数量
                var proreport = prolist.Select(y => new
                {

                    num = y.ProRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString() && x.State == 1).Count()

                });
                var count = prolist.Select(y => new
                {
                    num = y.ProRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count()
                });
                double procount = prolist.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count();//实际派单数量
                double counreport = count.Count() <= 0 ? 0 : count.Sum(x => x.num);//总的提醒量
                double reportcount = proreport.Count() <= 0 ? 0 : proreport.Sum(x => x.num);//当月回执数量（处理数量）
                double nonereport = counreport - reportcount;//未处理
                var list = new { Name = "项目派单", Procount = procount, Reportcount = reportcount, NoneReport = nonereport, CounReport = counreport };

                string list1 = new JavaScriptSerializer().Serialize(list);
                return list1;
            }
        }
        /// <summary>
        /// 当月存量派单处理
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string MonthlyStock()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                //var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();

                //var bancelist = db.SmallBalance.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString() && x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                //var bancereport = bancelist.Select(y => new
                //{

                //    num = y.SmaBaRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString() && x.State == 1).Count()

                //});
                //var count = bancelist.Select(y => new
                //{
                //    num = y.SmaBaRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count()
                //});
                //double bancecount = bancelist.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count();//实际派单数量
                //double counreport = count.Count() <= 0 ? 0 : count.Sum(x => x.num);//总的回执量
                //double reportcount = bancereport.Count() <= 0 ? 0 : bancereport.Sum(x => x.num);//当月回执数量（处理数量）
                //double nonereport = counreport - reportcount;//未处理
                //var list = new { Name = "存量派单", Bancecount = bancecount, Reportcount = reportcount, NoneReport = nonereport, CounReport = counreport };

                //string list1 = new JavaScriptSerializer().Serialize(list);
                //return list1;
                var banacelist = db.SmallBalance.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID)).Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString());

                var banacereport = banacelist.Select(y => new
                {
                    num = y.SmaBaRead.Where(x => x.IsRead == 1 && x.UserID == y.UserID).Count()
                });
                var allremind = banacelist.Select(y => new
                {
                    num = y.SmaBaRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count()
                });
                double countremind = allremind.Count() <= 0 ? 0 : allremind.Sum(t => t.num);//总提醒
                double banacecount = banacelist.Count();//派单数量
                double reportcount = banacereport.Count() <= 0 ? 0 : banacereport.Sum(t => t.num);//当月已读(已处理)
                double noneread = banacecount - reportcount;//未读
                var list = new { Name = "到期派单", Procount = banacecount, Reportcount = countremind, IsRead = reportcount, NoneRead = noneread };


                string list1 = new JavaScriptSerializer().Serialize(list);
                return list1;
            }
        }
        /// <summary>
        /// 当月商机派单
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string MonthlyBusiness()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();

                var buslist = db.Business.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                var busreport = buslist.Select(y => new
                {

                    num = y.BusRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString() && x.State == 1).Count()

                });
                var count = buslist.Select(y => new
                {
                    num = y.BusRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count()
                });
                double buscount = buslist.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count();//派单数量
                double reportcount = busreport.Count() <= 0 ? 0 : busreport.Sum(x => x.num);//当月回执数量（处理数量）
                double counreport = count.Count() <= 0 ? 0 : count.Sum(x => x.num);//总的提醒量
                double nonereport = counreport - reportcount;//未处理
                var list = new { Name = "商机派单", Buscount = buscount, Reportcount = reportcount, NoneReport = nonereport, CounReport = counreport };

                string list1 = new JavaScriptSerializer().Serialize(list);
                return list1;
            }
        }
        /// <summary>
        /// 当月门店派单
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string MonthlyStore()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();

                var storelist = db.Store.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString() && x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                var storereport = storelist.Select(y => new
                {

                    num = y.StoreRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString() && x.State == 1).Count()

                });
                var count = storelist.Select(y => new
                {
                    num = y.StoreRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count()
                });
                double storecount = storelist.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count();//派单数量
                double reportcount = storereport.Count() <= 0 ? 0 : storereport.Sum(x => x.num);//当月回执数量（处理数量）
                double counreport = count.Count() <= 0 ? 0 : count.Sum(x => x.num);//总的回执量
                double nonereport = counreport - reportcount;//未处理
                var list = new { Name = "门店派单", Storecount = storecount, Reportcount = reportcount, NoneReport = nonereport, CounReport = counreport };

                string list1 = new JavaScriptSerializer().Serialize(list);
                return list1;
            }
        }
        /// <summary>
        /// 当月专线派单
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string MonthlySpecil()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();

                //实际拍单数
                var specillist = db.PrivateLine.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString() && x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                //回执数
                var specilreport = specillist.Select(y => new
                {

                    num = y.PlRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString() && x.State == 1).Count()

                });
                //总提醒量
                var count = specillist.Select(y => new
                {
                    num = y.PlRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count()
                });
                var no = specillist.Select(y => new
                {

                    num = y.PlRemind.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString() && x.State != 1).Count()

                });
                double specilcount = specillist.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count();//派单数量
                double reportcount = specilreport.Count() <= 0 ? 0 : specilreport.Sum(x => x.num);//当月回执数量（处理数量）
                double counreport = count.Count() <= 0 ? 0 : count.Sum(x => x.num);//总的提醒量
                //double nonereport = counreport - reportcount;//未处理
                double nonereport = no.Count() == 0 ? 0 : no.Sum(x => x.num);
                var list = new { Name = "专线派单", Specilcount = specilcount, Reportcount = reportcount, NoneReport = nonereport, CounReport = counreport };

                string list1 = new JavaScriptSerializer().Serialize(list);
                return list1;
            }
        }
        /// <summary>
        /// 当月业务报表统计(全部)
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Report()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Reports.Where(x => x.Month == DateTime.Now.ToString("yyyyMM").Substring(0, 6));//当月报表
                var resultList = list
                .OrderBy(x => x.Rank).Take(3)
               .Select(x => new
               {
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
                var result = new { status = "1", data = resultList };
                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;

            }

        }
        /// <summary>
        /// 个人当月业务报表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string PeperReport()
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Reports.Where(x => x.Month == DateTime.Now.ToString("yyyyMM").Substring(0, 6) && x.Name == Common.TCContext.Current.OnlineRealName);//当月报表
                var resultList = list
               .Select(x => new
               {
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
                var result = new { status = "1", data = resultList };
                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;

            }

        }

    }
}