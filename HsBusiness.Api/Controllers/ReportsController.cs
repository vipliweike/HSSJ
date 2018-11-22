using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq.Dynamic;

namespace HsBusiness.Api.Controllers
{
    public class ReportsController : ApiController
    {

        /// <summary>
        /// 通过用户查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetByUser(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    // 真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var Areas = parameters["Grids"];//网格
                    var Name = parameters["Name"];
                    var Month = parameters["Month"];//月份查询                    
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    if (user != null)
                    {
                        var list = db.Reports.Where(x => 1 == 1);
                        if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        {
                            list = list.Where(x => 1 == 1);//公司领导、政企部主管,都能看
                        }
                        else
                        if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                        {
                            list = list.Where(x => db.Users.Where(y => y.Areas == user.Areas).Select(y => y.Name).Contains(x.Name));//
                        }
                        else
                        if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、行业经理，能看自己的
                        {
                            list = list.Where(x => x.Name == user.Name);
                        }
                        else
                        {
                            list = list.Where(x => x.ID < 0);//无数据
                        }
                        if (!string.IsNullOrEmpty(Month))//月份查询
                        {
                            list = list.Where(x => x.Month == Month);
                        }
                        if (!string.IsNullOrEmpty(Name))//姓名查询
                        {
                            list = list.Where(x => x.Name == Name);
                        }
                        if (!string.IsNullOrEmpty(Areas))//区县查询
                        {
                            var areaUserList = db.Users.Where(x => x.Areas == Areas).Select(x => x.Name).ToList();//区县内所有人
                            list = list.Where(x => areaUserList.Contains(x.Name));
                        }


                        var resultList = list
                            .OrderByDescending(x => x.Month)
                            .ThenBy(x => x.Rank)
                            .Select(y => new
                            {
                                y.ID,
                                y.Grids,
                                y.Month,
                                y.Name,
                                y.Rank,
                                y.NewIncome,
                                y.R0Aims,
                                y.R0CompleteRate,
                                y.R2Aims,
                                y.R2CompleteRate,
                                y.StockIncome,
                                y.StockIncomeRate,
                                y.TotalIncome,
                            }).ToList()
                            .Select(y => new
                            {
                                y.ID,
                                y.Grids,
                                y.Month,
                                y.Name,
                                y.Rank,
                                //NewIncome = Convert.ToDouble(y.NewIncome).ToString("0.00"),
                                R0Aims = Convert.ToDouble(y.R0Aims).ToString("0.00"),
                                R0CompleteRate = (Convert.ToDouble(y.R0CompleteRate) * 100).ToString("0.00"),
                                R2Aims = Convert.ToDouble(y.R2Aims).ToString("0.00"),
                                R2CompleteRate = (Convert.ToDouble(y.R2CompleteRate) * 100).ToString("0.00"),
                                //StockIncome = Convert.ToDouble(y.StockIncome).ToString("0.00"),
                                StockIncomeRate = (Convert.ToDouble(y.StockIncomeRate) * 100).ToString("0.00"),
                                TotalIncome = Convert.ToDouble(y.TotalIncome).ToString("0.00"),
                            });

                        //个人获取前12条
                        if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")
                        {
                            resultList = resultList.Take(12);
                        }

                        return Json(new { data = resultList, state = 1, msg = "请求成功" });
                    }

                    return Json(new { state = 0, msg = "用户不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 通过用户查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAll(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    // 真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var Areas = parameters["Grids"];//网格
                    var Name = parameters["Name"];
                    var Month = parameters["Month"];//月份查询                    
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    if (user != null)
                    {
                        var list = db.Reports.Where(x => 1 == 1);


                        if (!string.IsNullOrEmpty(Month))//月份查询
                        {
                            list = list.Where(x => x.Month == Month);
                        }
                        if (!string.IsNullOrEmpty(Name))//姓名查询
                        {
                            list = list.Where(x => x.Name == Name);
                        }
                        if (!string.IsNullOrEmpty(Areas))//区县查询
                        {
                            var areaUserList = db.Users.Where(x => x.Areas == Areas).Select(x => x.Name).ToList();//区县内所有人
                            list = list.Where(x => areaUserList.Contains(x.Name));
                        }


                        var resultList = list
                            .OrderByDescending(x => x.Month)
                            .ThenBy(x => x.Rank)
                            .Take(3)
                            .Select(y => new
                            {
                                y.ID,
                                y.Grids,
                                y.Month,
                                y.Name,
                                y.Rank,
                                y.NewIncome,
                                y.R0Aims,
                                y.R0CompleteRate,
                                y.R2Aims,
                                y.R2CompleteRate,
                                y.StockIncome,
                                y.StockIncomeRate,
                                y.TotalIncome,
                            }).ToList()
                            .Select(y => new
                            {
                                y.ID,
                                y.Grids,
                                y.Month,
                                y.Name,
                                y.Rank,
                                //NewIncome = Convert.ToDouble(y.NewIncome).ToString("0.00"),
                                R0Aims = Convert.ToDouble(y.R0Aims).ToString("0.00"),
                                R0CompleteRate = (Convert.ToDouble(y.R0CompleteRate) * 100).ToString("0.00"),
                                R2Aims = Convert.ToDouble(y.R2Aims).ToString("0.00"),
                                R2CompleteRate = (Convert.ToDouble(y.R2CompleteRate) * 100).ToString("0.00"),
                                //StockIncome = Convert.ToDouble(y.StockIncome).ToString("0.00"),
                                StockIncomeRate = (Convert.ToDouble(y.StockIncomeRate) * 100).ToString("0.00"),
                                TotalIncome = Convert.ToDouble(y.TotalIncome).ToString("0.00"),
                            });

                        return Json(new { data = resultList, state = 1, msg = "请求成功" });
                    }

                    return Json(new { state = 0, msg = "用户不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取最新月份
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetLastMonth(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    // 真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var list = db.Reports.Where(x => 1 == 1).OrderByDescending(x => x.Month).Select(x => new
                    {
                        x.Month
                    }).Take(1).FirstOrDefault();
                    if (list != null)
                    {
                        return Json(new { data = list, state = 1, msg = "请求成功" });
                    }
                    return Json(new { state = 0, msg = "暂无月份" });

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取全部月份
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMonth(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    // 真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var list = db.Reports.Where(x => 1 == 1)
                        .GroupBy(x => x.Month)
                        .Select(x => new
                        {
                            Month = x.FirstOrDefault().Month
                        }).OrderByDescending(x => x.Month).ToList();
                    return Json(new { data = list, state = 1, msg = "请求成功" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetData(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    // 真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();

                    if (user != null)
                    {
                        var busList = db.Business.Where(x => 1 == 1);//商机
                        //var stoteList = db.Store.Where(x => 1 == 1);//门店
                        var plList = db.PrivateLine.Where(x => 1 == 1);//专线
                        if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        {
                            busList = busList.Where(x => 1 == 1);//公司领导、政企部主管,都能看
                            //stoteList = stoteList.Where(x => 1 == 1);//公司领导、政企部主管,都能看
                            plList = plList.Where(x => 1 == 1);
                        }
                        else
                        if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                        {
                            busList = busList.Where(x => x.Areas == user.Areas);
                            //stoteList = stoteList.Where(x => x.Users.Areas == user.Areas);
                            plList = plList.Where(x => x.Users.Areas == user.Areas);
                        }
                        else
                        if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                        {
                            busList = busList.Where(x => x.UserID == UserID);
                            //stoteList = stoteList.Where(x => x.UserID == UserID);
                            plList = plList.Where(x => x.UserID == UserID);
                        }
                        else
                        {
                            busList = busList.Where(x => x.ID < 0);//无数据
                            //stoteList = stoteList.Where(x => x.ID < 0);//无数据
                            plList = plList.Where(x => x.ID < 0);//无数据
                        }
                        //当月新增商机
                        var NewBusinessNum = busList.Where(n => Convert.ToDateTime(n.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(n.AddTime).Month.ToString() == DateTime.Now.Month.ToString());
                        //当月商机走访
                        var MonthBusVisitNum = busList.Where(n => Convert.ToDateTime(n.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(n.AddTime).Month.ToString() == DateTime.Now.Month.ToString())
                                                .Select(x => new { Num = x.Visit.Count });
                        #region 门店
                        ////当月新增门店
                        //var NewStoreNum = stoteList.Where(n => Convert.ToDateTime(n.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(n.AddTime).Month.ToString() == DateTime.Now.Month.ToString());

                        ////本月门店走访
                        //var MonthStoreVisitNum = stoteList.Where(n => Convert.ToDateTime(n.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(n.AddTime).Month.ToString() == DateTime.Now.Month.ToString())
                        //                        .Select(x => new { Num = x.StoreVisit.Count() });
                        #endregion
                        //当月新增专线
                        var NewPrivateLineNum = plList.Where(n => Convert.ToDateTime(n.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(n.AddTime).Month.ToString() == DateTime.Now.Month.ToString());
                        //当月专线走访
                        var MonthPlVisitNum = plList.Where(n => Convert.ToDateTime(n.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(n.AddTime).Month.ToString() == DateTime.Now.Month.ToString())
                                                .Select(x => new { Num = x.PlVisit.Count });

                        //返回结果
                        var list = new
                        {
                            #region 商机
                            //当月新增商机
                            NewBusinessNum = NewBusinessNum.Count(),
                            //累计商机
                            TotalBusinessNum = busList.Count(),
                            //本月商机走访
                            MonthBusVisitNum = MonthBusVisitNum.Count() <= 0 ? 0 : MonthBusVisitNum.Sum(x => x.Num),
                            #endregion
                            #region 门店
                            ////当月新增门店
                            //NewStoreNum = NewStoreNum.Count(),
                            ////累计门店
                            //TotalStoreNum = stoteList.Count(),
                            ////本月门店走访
                            //MonthStoreVisitNum = MonthStoreVisitNum.Count() <= 0 ? 0 : MonthStoreVisitNum.Sum(x => x.Num),
                            #endregion
                            #region 专线
                            //当月新增专线
                            NewPrivateLineNum = NewPrivateLineNum.Count(),
                            //累计专线
                            TotalPrivateLineNum = plList.Count(),
                            //当月专线走访
                            MonthPlVisitNum = MonthPlVisitNum.Count() <= 0 ? 0 : MonthPlVisitNum.Sum(x => x.Num),
                            #endregion
                        };
                        return Json(new { data = list, state = 1, msg = "请求成功" });
                    }



                    return Json(new { state = 0, msg = "用户不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取首页统计数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetStatistics(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    // 真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var Display = Convert.ToInt32(parameters["Display"]);//显示  0部分（4条，包括总计）   1全部
                    DateTime Month;
                    if (!string.IsNullOrEmpty(parameters["Month"]))
                    {
                        Month = Convert.ToDateTime(parameters["Month"]);//月份
                    }
                    else
                    {
                        Month = DateTime.Now;
                    }

                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();

                    if (user != null)
                    {
                        #region 结果list
                        var areaList = db.Region.Where(x => x.Pid == (db.Region.Where(y => y.Pid == 0).Select(y => y.ID).FirstOrDefault())).Select(x => x.Name).ToList();


                        //areaList.Remove("衡水市");
                        List<dynamic> result = new List<dynamic>();

                        foreach (var area in areaList)
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("Rank", "");
                            dic.Add("Area", area);
                            dic.Add("PLMonth", 0);
                            dic.Add("PLTotal", 0);
                            dic.Add("BusMonth", 0);
                            dic.Add("BusTotal", 0);

                            result.Add(dic);
                        }
                        result.Insert(0, new Dictionary<string, object>()
                        {
                            { "Rank",  "" },
                            { "Area","总计" },
                            { "PLMonth",0},
                            { "PLTotal",0},
                            { "BusMonth",0},
                            { "BusTotal",0},
                        });
                        #endregion

                        var busList = db.Business.Where(x => 1 == 1);//商机

                        var plList = db.PrivateLine.Where(x => 1 == 1);//专线

                        //if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        //{
                        //    busList = busList.Where(x => 1 == 1);//公司领导、政企部主管,都能看                           
                        //    plList = plList.Where(x => 1 == 1);
                        //}
                        //else
                        //if (user.Roles.RoleName == "区县经理" && Display == 1)//区县经理，能看本区县
                        //{
                        //    busList = busList.Where(x => x.Areas == user.Areas);
                        //    plList = plList.Where(x => x.Users.Areas == user.Areas);
                        //}
                        //else
                        //if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                        //{
                        //    busList = busList.Where(x => x.UserID == UserID);
                        //    plList = plList.Where(x => x.UserID == UserID);
                        //}
                        //else
                        //{
                        //    busList = busList.Where(x => x.ID < 0);//无数据 
                        //    plList = plList.Where(x => x.ID < 0);//无数据
                        //}

                        //当月新增商机
                        var NewBusinessNum = busList.Where(n => Convert.ToDateTime(n.AddTime).Year == Month.Year && Convert.ToDateTime(n.AddTime).Month == Month.Month)
                            .GroupBy(x => x.Users.Areas).Select(x => new { x.FirstOrDefault().Users.Areas, Count = x.Count() }).ToList();
                        //累计商机
                        var TotalBusinessNum = busList.GroupBy(x => x.Users.Areas).Select(x => new { x.FirstOrDefault().Users.Areas, Count = x.Count() }).ToList();

                        //当月新增专线
                        var NewPrivateLineNum = plList.Where(n => Convert.ToDateTime(n.AddTime).Year == Month.Year && Convert.ToDateTime(n.AddTime).Month == Month.Month)
                        .GroupBy(x => x.Users.Areas).Select(x => new { x.FirstOrDefault().Users.Areas, Count = x.Count() <= 0 ? 0 : x.Sum(y => y.PlInfo.Count) }).OrderByDescending(x => x.Count).ToList();
                        //累计专线
                        var TotalPrivateLineNum = plList.GroupBy(x => x.Users.Areas).Select(x => new { x.FirstOrDefault().Users.Areas, Count = x.Count() <= 0 ? 0 : x.Sum(y => y.PlInfo.Count) }).OrderByDescending(x => x.Count).ToList();

                        #region 结果
                        foreach (var item in result)
                        {
                            //当月商机
                            foreach (var newbus in NewBusinessNum)
                            {
                                if (newbus.Areas == item["Area"]) { item["BusMonth"] = newbus.Count; }
                                if (item["Area"] == "总计") { item["BusMonth"] += newbus.Count; }
                            }
                            //累计商机
                            foreach (var totalbus in TotalBusinessNum)
                            {
                                if (totalbus.Areas == item["Area"]) { item["BusTotal"] = totalbus.Count; }
                                if (item["Area"] == "总计") { item["BusTotal"] += totalbus.Count; }
                            }
                            //当月专线
                            foreach (var newpl in NewPrivateLineNum)
                            {
                                if (newpl.Areas == item["Area"]) { item["PLMonth"] = newpl.Count; }
                                if (item["Area"] == "总计") { item["PLMonth"] += newpl.Count; }
                            }
                            //累计专线
                            foreach (var totalpl in TotalPrivateLineNum)
                            {
                                if (totalpl.Areas == item["Area"]) { item["PLTotal"] = totalpl.Count; }
                                if (item["Area"] == "总计") { item["PLTotal"] += totalpl.Count; }
                            }
                        }
                        #endregion

                        //结果排序
                        var r = result.OrderByDescending(x => x["PLTotal"]).ThenByDescending(x => x["PLMonth"]).ThenByDescending(x => x["BusTotal"]).ThenByDescending(x => x["BusMonth"]);


                        //最终结果
                        dynamic rr = null;//
                        if (Display == 1)//全部
                        {
                            rr = r;
                            if (user.Roles.RoleName == "区县经理")
                            {
                                rr = r.Where(x => x["Area"] == user.Areas || x["Area"] == "总计");
                            }
                        }
                        else//默认(部分)
                        {
                            rr = r.Take(4);
                        }
                        return Json(new { data = rr, state = 1, msg = "请求成功" });
                    }
                    return Json(new { state = 0, msg = "用户不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取数据统计通过区县(区县下人员数据统计)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetDataByArea(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    // 真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var Area = parameters["Area"];//区县
                    DateTime Month;//月份
                    if (!string.IsNullOrEmpty(parameters["Month"]))
                    {
                        Month = Convert.ToDateTime(parameters["Month"]);
                    }
                    else
                    {
                        Month = DateTime.Now;
                    }
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    if (user != null)
                    {

                        var busList = db.Business.Where(x => 1 == 1);//商机

                        var plList = db.PrivateLine.Where(x => 1 == 1);//专线
                        //if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        //{
                        //    busList = busList.Where(x => 1 == 1);//公司领导、政企部主管,都能看                           
                        //    plList = plList.Where(x => 1 == 1);
                        //}
                        //else
                        //if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                        //{
                        //    busList = busList.Where(x => x.Areas == user.Areas);
                        //    plList = plList.Where(x => x.Users.Areas == user.Areas);
                        //}
                        //else
                        //if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                        //{
                        //    busList = busList.Where(x => x.UserID == UserID);
                        //    plList = plList.Where(x => x.UserID == UserID);
                        //}
                        //else
                        //{
                        //    busList = busList.Where(x => x.ID < 0);//无数据 
                        //    plList = plList.Where(x => x.ID < 0);//无数据
                        //}
                        if (!string.IsNullOrEmpty(Area))
                        {
                            busList = busList.Where(x => x.Users.Areas.Contains(Area));//区县 
                            plList = plList.Where(x => x.Users.Areas.Contains(Area));//区县
                        }
                        //当月新增商机
                        var NewBusinessNum = busList.Where(n => Convert.ToDateTime(n.AddTime).Year == Month.Year && Convert.ToDateTime(n.AddTime).Month == Month.Month)
                            .GroupBy(x => x.UserID).Select(x => new { x.FirstOrDefault().Users.Name, Count = x.Count() }).OrderByDescending(x => x.Count).ToList();
                        //累计商机
                        var TotalBusinessNum = busList.GroupBy(x => x.UserID).Select(x => new { x.FirstOrDefault().Users.Name, Count = x.Count() }).OrderByDescending(x => x.Count).ToList();

                        //当月新增专线
                        var NewPrivateLineNum = plList.Where(n => Convert.ToDateTime(n.AddTime).Year == Month.Year && Convert.ToDateTime(n.AddTime).Month == Month.Month)
                        .GroupBy(x => x.UserID).Select(x => new { x.FirstOrDefault().Users.Name, Count = x.Count() <= 0 ? 0 : x.Sum(y => y.PlInfo.Count) }).OrderByDescending(x => x.Count).ToList();
                        //累计专线
                        var TotalPrivateLineNum = plList.GroupBy(x => x.UserID).Select(x => new { x.FirstOrDefault().Users.Name, Count = x.Count() <= 0 ? 0 : x.Sum(y => y.PlInfo.Count) }).OrderByDescending(x => x.Count).ToList();

                        #region 全部人员统计
                        //全部姓名
                        var NameList = NewBusinessNum.Select(x => x.Name).Union(TotalBusinessNum.Select(x => x.Name)).Union(NewPrivateLineNum.Select(x => x.Name)).Union(TotalPrivateLineNum.Select(x => x.Name)).OrderBy(x => x);

                        List<dynamic> result = new List<dynamic>();

                        foreach (var name in NameList)
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("Name", name);
                            dic.Add("PLMonth", 0);
                            dic.Add("PLTotal", 0);
                            dic.Add("BusMonth", 0);
                            dic.Add("BusTotal", 0);
                            result.Add(dic);
                        }

                        #endregion
                        #region 结果
                        foreach (var item in result)
                        {
                            //当月商机
                            foreach (var newbus in NewBusinessNum)
                            {
                                if (newbus.Name == item["Name"]) { item["BusMonth"] = newbus.Count; }
                            }
                            //累计商机
                            foreach (var totalbus in TotalBusinessNum)
                            {
                                if (totalbus.Name == item["Name"]) { item["BusTotal"] = totalbus.Count; }
                            }
                            //当月专线
                            foreach (var newpl in NewPrivateLineNum)
                            {
                                if (newpl.Name == item["Name"]) { item["PLMonth"] = newpl.Count; }
                            }
                            //累计专线
                            foreach (var totalpl in TotalPrivateLineNum)
                            {
                                if (totalpl.Name == item["Name"]) { item["PLTotal"] = totalpl.Count; }
                            }
                        }
                        #endregion

                        var orderResult = result.OrderByDescending(x => x["PLTotal"]).ThenByDescending(x => x["PLMonth"]).ThenByDescending(x => x["BusTotal"]).ThenByDescending(x => x["BusMonth"]);

                        ////结果
                        //var r = new
                        //{
                        //    PLMonth = NewPrivateLineNum,//本月专线
                        //    PLTotal = TotalPrivateLineNum,//累计专线
                        //    BusMonth = NewBusinessNum,//本月商机
                        //    BusTotal = TotalBusinessNum,//累计商机
                        //    NameList = NameList,//姓名列表
                        //};


                        return Json(new { data = orderResult, state = 1, msg = "请求成功" });
                    }
                    return Json(new { state = 0, msg = "用户不存在" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
