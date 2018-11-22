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
    public partial class Welcome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 本月商机上报统计
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string BusStatistics()
        {
            using (dbDataContext db = new dbDataContext())
            {

                //     var q =
                // from p in db.Business
                // group p by p.Areas into g
                // select new
                // {

                //    g.Key,
                //    NumProducts = g.Count()
                //};
                //var NewBusinessNum = busList.Where(n => Convert.ToDateTime(n.AddTime).Month.ToString() == DateTime.Now.Month.ToString());
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                var busList = db.Business.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString());

                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    busList = busList.Where(x => 1 == 1);//公司领导、政企部主管,都能看

                }
                else if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                {
                    busList = busList.Where(x => x.Areas == user.Areas);

                }
                else if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    busList = busList.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));

                }
                Dictionary<string, int> dic = new Dictionary<string, int>();
                //查询所有区县
                var reginlist = db.Region.Where(x => db.Region.Where(y => y.Pid == 0).Select(y => y.ID).Contains((int)x.Pid))
                    .Select(x => new
                    {
                        Areas = x.Name,
                        addnumber = 0
                    }).OrderByDescending(x => x.addnumber).ToList();

                for (int i = 0; i < reginlist.Count; i++)
                {
                    dic.Add(reginlist[i].Areas, reginlist[i].addnumber);
                }

                var list = busList.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Select(x => new
                {
                    x.UserID,
                    x.AddTime,
                    x.Areas,
                }).GroupBy(x => x.Areas).Select(x => new { x.FirstOrDefault().Areas, AddNumber = x.Count() }).OrderByDescending(x => x.AddNumber);
                foreach (var item in dic.Keys.ToArray())
                {
                    foreach (var item1 in list)
                    {
                        if (item == item1.Areas)
                        {
                            dic[item] = item1.AddNumber;
                            
                        }
                    }
                }
                //排序
                List<KeyValuePair<string, int>> lst = new List<KeyValuePair<string, int>>(dic);
                lst.Sort(delegate (KeyValuePair<string, int> s1, KeyValuePair<string, int> s2)
                {
                    return s2.Value.CompareTo(s1.Value);
                });
                dic.Clear();
                var result = new { state = 1, data = lst };

                string list1 = new JavaScriptSerializer().Serialize(result);
                return list1;
            }
        }
        /// <summary>
        /// 商机上报数量
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string BusNum()
        {
            using (dbDataContext db=new dbDataContext ())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                var busList = db.Business.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString());

                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    busList = busList.Where(x => 1 == 1);//公司领导、政企部主管,都能看

                }
                else if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                {
                    busList = busList.Where(x => x.Areas == user.Areas);

                }
                else if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    busList = busList.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));

                }
                //var list = db.Business.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count();
                var result = new { status = "1", data = busList.Count() };
                string list1 = new JavaScriptSerializer().Serialize(result);
                return list1;

            }
        }

        /// <summary>
        /// 本月专线统计
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string PlStatisc()
        {
            using (dbDataContext db = new dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                var zxList = db.PrivateLine.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString());

                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    zxList = zxList.Where(x => 1 == 1);//公司领导、政企部主管,都能看

                }
                else if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                {
                    zxList = zxList.Where(x => x.Users.Areas == user.Areas);

                }
                else if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    zxList = zxList.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));

                }
                Dictionary<string, int> dic = new Dictionary<string, int>();
                //查询所有区县
                var reginlist = db.Region.Where(x => db.Region.Where(y => y.Pid == 0).Select(y => y.ID).Contains((int)x.Pid))
                    .Select(x => new
                    {
                        Areas = x.Name,
                        addnumber = 0
                    }).OrderByDescending(x => x.addnumber).ToList();

                for (int i = 0; i < reginlist.Count; i++)
                {
                    dic.Add(reginlist[i].Areas, reginlist[i].addnumber);
                }

                var list = zxList.Select(x => new

                {
                    ZxCount = x.PlInfo.Count,
                    x.AddTime,
                    Areas = x.Users.Areas,
                }).GroupBy(x => x.Areas).Select(x => new { x.FirstOrDefault().Areas, AddNumber = x.Sum(y=>y.ZxCount) }).OrderByDescending(x => x.AddNumber);
                foreach (var item in dic.Keys.ToArray())
                {
                    foreach (var item1 in list)
                    {
                        if (item == item1.Areas)
                        {
                            dic[item] = item1.AddNumber;

                        }
                    }
                }
                //排序
                List<KeyValuePair<string, int>> lst = new List<KeyValuePair<string, int>>(dic);
                lst.Sort(delegate (KeyValuePair<string, int> s1, KeyValuePair<string, int> s2)
                {
                    return s2.Value.CompareTo(s1.Value);
                });
                dic.Clear();

                var result = new { state = 1, data = lst };
                string list1 = new JavaScriptSerializer().Serialize(result);
                return list1;
            }
        }
        /// <summary>
        /// 专线数量
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string ZXNum()
        {
            using (dbDataContext db = new dbDataContext())
            {
                var zxList = db.PrivateLine.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString());
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    zxList = zxList.Where(x => 1 == 1);//公司领导、政企部主管,都能看

                }
                else if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                {
                    zxList = zxList.Where(x => x.Users.Areas == user.Areas);

                }
                else if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    zxList = zxList.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));

                }
                var zxcount = zxList.Select(x => new { sum = x.PlInfo.Count });
                //var zxList = db.PrivateLine.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString() && Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count();
                var result = new { status = "1", data = zxList.Count()>0? zxcount.Sum(t=>t.sum):0 };
                string list1 = new JavaScriptSerializer().Serialize(result);
                return list1;

            }
        }
        /// <summary>
        /// 项目派单统计
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string ProjectAss()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();

                var prolist = db.Project.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString());
                //var proreport = db.ProRemind.Where(x => Convert.ToDateTime(x.ReceiptTime).Month.ToString() == DateTime.Now.Month.ToString() && x.State == 1);//当月的回执记录
                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    prolist = prolist.Where(x => 1 == 1);//公司领导、政企部主管,都能看

                }
                else if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                {
                    prolist = prolist.Where(x => x.Areas == user.Areas);

                }
                else if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    prolist = prolist.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));

                }
                var proreport = prolist.Select(y => new
                {

                    num = y.ProRemind.Where(x => Convert.ToDateTime(x.ReceiptTime).Month.ToString() == DateTime.Now.Month.ToString() && x.State == 1).Count()

                });
                double procount = prolist.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count();//派单数量
                double reportcount = proreport.Count() <= 0 ? 0 : proreport.Sum(x => x.num);//当月回执数量
                var reportrate = procount == 0 ? "0" : Convert.ToDouble(reportcount / procount).ToString("0.00%");
                var list = new { Procount = procount, Reportcount = reportcount, Reportrate = reportrate };

                string list1 = new JavaScriptSerializer().Serialize(list);
                return list1;
            }
        }
        /// <summary>
        /// 小余额统计
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string BalanceStatic()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                var banacelist = db.SmallBalance.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString());
                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    banacelist = banacelist.Where(x => 1 == 1);//公司领导、政企部主管,都能看

                }
                else if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                {
                    banacelist = banacelist.Where(x => x.Users.Areas == user.Areas);

                }
                else if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    banacelist = banacelist.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));

                }
                var banacereport = banacelist.Select(y => new
                {
                    num = y.SmaBaRead.Where(x => x.IsRead == 1&&x.UserID==y.UserID).Count() 
                });

                double banacecount = banacelist.Where(x => Convert.ToDateTime(x.AddTime).Year.ToString() == DateTime.Now.Year.ToString()&& Convert.ToDateTime(x.AddTime).Month.ToString() == DateTime.Now.Month.ToString()).Count();//派单数量
                double reportcount = banacereport.Count() <= 0 ? 0 : banacereport.Sum(t => t.num);//当月已读

                var list = new { Procount = banacecount, Reportcount = reportcount, Reportrate = banacecount - reportcount };


                string list1 = new JavaScriptSerializer().Serialize(list);
                return list1;
            }
        }
    }
}