using HsBusiness.Interface.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.PersonManage
{
    public partial class ActivityTable : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 平台登陆信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetInfo(string Areas, string Post, string Search, string starttime, string endtime, string pageIndex)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                int pageSize = 10;
                var list = db.Users.Where(x => x.Name != "admin").Select(x => new
                {
                    x.Areas,
                    x.Post,
                    x.Name,
                    x.Mobile,
                    AppLogCount = db.ApiLog.Where(y => y.UserID == x.ID && y.RequestName == "/Users/Login").Count(),//app登陆次数
                    AppLogTime = db.ApiLog.Where(y => y.UserID == x.ID && y.RequestName == "/Users/Login").OrderByDescending(y => y.AddTime).Select(y => new { y.AddTime }).FirstOrDefault().AddTime,//app最近登陆时间
                    WebLogCount = db.OperateLog.Where(t => t.Operator == x.Name).Count(),//web登陆次数
                    WebLogTime = db.OperateLog.Where(t => t.Operator == x.Name).OrderByDescending(y => y.OperTime).Select(t => new { t.OperTime }).FirstOrDefault().OperTime,//web最近登陆时间

                });

                if (!string.IsNullOrEmpty(Areas))
                {
                    list = list.Where(x => x.Areas == Areas);
                }
                if (!string.IsNullOrEmpty(Post))
                {
                    list = list.Where(x => x.Post == Post);
                }

                if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime))
                {
                    list = list.Where(x => (Convert.ToDateTime(x.AppLogTime) >= Convert.ToDateTime(starttime) && Convert.ToDateTime(x.AppLogTime) <= Convert.ToDateTime(endtime).AddDays(1))
                                      || (Convert.ToDateTime(x.WebLogTime) >= Convert.ToDateTime(starttime) && Convert.ToDateTime(x.WebLogTime) <= Convert.ToDateTime(endtime).AddDays(1)));

                }
                if (!string.IsNullOrEmpty(Search))
                {
                    list = list.Where(x => x.Name.Contains(Search) || x.Mobile.Contains(Search));
                }
                var list1 = list.Skip((int.Parse(pageIndex) - 1) * pageSize).Take(pageSize);
                var result = new { status = "1", data = list1, pagecount = list.Count().ToString(), pagesize = pageSize.ToString() };
                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }

        }


        /// <summary>
        /// 查询所有区县
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetRegion()
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Region.Where(x => db.Region.Where(y => y.Pid == 0).Select(y => y.ID).Contains((int)x.Pid))
                    .Select(x => new
                    {
                        x.Name
                    }).ToList();
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                return result;
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Users.Where(x => x.Name != "admin").Select(x => new
                {
                    x.Areas,
                    x.Post,
                    x.Name,
                    x.Mobile,
                    AppLogCount = db.ApiLog.Where(y => y.UserID == x.ID && y.RequestName == "/Users/Login").Count(),//app登陆次数
                    AppLogTime = db.ApiLog.Where(y => y.UserID == x.ID && y.RequestName == "/Users/Login").OrderByDescending(y => y.AddTime).Select(y => new { y.AddTime }).FirstOrDefault().AddTime,//app最近登陆时间
                    WebLogCount = db.OperateLog.Where(t => t.Operator == x.Name).Count(),//web登陆次数
                    WebLogTime = db.OperateLog.Where(t => t.Operator == x.Name).OrderByDescending(y => y.OperTime).Select(t => new { t.OperTime }).FirstOrDefault().OperTime,//web最近登陆时间

                });
                if (Request.Form["areas"].ToString() != "")
                {

                    list = list.Where(x => x.Areas == Request.Form["areas"].ToString());
                }
                if (Request.Form["post"].ToString() != "")
                {

                    list = list.Where(x => x.Post == Request.Form["post"].ToString());
                }
                if (!string.IsNullOrEmpty(Request.Form["sbirth"].ToString()) && !string.IsNullOrEmpty(Request.Form["sbirth2"].ToString()))
                {
                    list = list.Where(x => (Convert.ToDateTime(x.AppLogTime) >= Convert.ToDateTime(Request.Form["sbirth"].ToString()) && Convert.ToDateTime(x.AppLogTime) <= Convert.ToDateTime(Request.Form["sbirth2"].ToString()).AddDays(1))
                                      || (Convert.ToDateTime(x.WebLogTime) >= Convert.ToDateTime(Request.Form["sbirth"].ToString()) && Convert.ToDateTime(x.WebLogTime) <= Convert.ToDateTime(Request.Form["sbirth2"].ToString()).AddDays(1)));

                }
                if (Request.Form["Search"].ToString() != "")
                {

                    list = list.Where(x => x.Name.Contains(Request.Form["Search"].ToString()) || x.Mobile.Contains(Request.Form["Search"].ToString()));
                }
                List<string[]> rows = new List<string[]>();
                rows.Add(COLUMNS);
                foreach (var item in list)
                {
                    string[] row = { item.Areas,item.Post,item.Name,item.Mobile,item.AppLogCount.ToString(),item.AppLogTime,item.WebLogCount.ToString(),item.WebLogTime,
                                   };
                    rows.Add(row);
                }

                var ms = (System.IO.MemoryStream)ExcelHelper.RowsToExcel(rows);
                ExcelHelper.RenderToBrowser(ms, HttpContext.Current, "用户活跃度统计.xls");
            }
                
        }
        static readonly string[] COLUMNS = { "区县", "岗位", "负责人", "联系电话", "App登录次数", "App登录最近时间", "Web登录次数", "Web登录最近时间" };
    }
}