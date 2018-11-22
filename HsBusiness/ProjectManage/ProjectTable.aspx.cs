using HsBusiness.Interface.Comm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.ProjectManage
{
    public partial class ProjectTable : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 加载数据列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Get(string Areas, string State, string UpdateStartTime, string UpdateEndTime, string pageIndex)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                int pageSize = 10;
                var list = db.Project.Where(x => 1 == 1).Select(x => new
                {
                    x.ID,
                    x.Areas,
                    x.Introduce,
                    x.AddTime,
                    x.LastTime,
                    x.State,
                    x.UserID,
                    UserAreas = x.Users.Areas,
                    UserName = x.Users.Name,
                    x.Type,
                });
                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                }

                if (user.Roles.RoleName == "区县经理")//区县经理看自己
                {
                    list = list.Where(x => x.UserAreas == user.Areas);
                    //list = list.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                }

                if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    list = list.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                }
                if (!string.IsNullOrEmpty(Areas))
                {
                    list = list.Where(x => x.UserAreas == Areas);
                }
                if (State != "")
                {
                    list = list.Where(x => x.State == int.Parse(State));
                }
                if (!string.IsNullOrEmpty(UpdateStartTime) && !string.IsNullOrEmpty(UpdateEndTime))
                {
                    list = list.Where(x => Convert.ToDateTime(x.LastTime) >= Convert.ToDateTime(UpdateStartTime) && Convert.ToDateTime(x.LastTime) < Convert.ToDateTime(UpdateEndTime).AddDays(1));
                }
                var list1 = list.OrderByDescending(x => x.LastTime).Skip((int.Parse(pageIndex) - 1) * pageSize).Take(pageSize).ToList();
                var result = new { status = "1", data = list1, pagecount = list.Count().ToString(), pagesize = pageSize.ToString() };
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
                var perject = db.Project.FirstOrDefault(x => x.ID == id);//用户信息
                //var visitlist = db.Visit.Where(x => x.UserID == id).ToList();//拜访记录
                //var buinesslist = db.Business.FirstOrDefault(x=>x.UserID==id);//商机信息
                //var contactlist = db.Contacts.Where(x => x.BusID == buinesslist.ID).ToList();
                //db.Visit.DeleteAllOnSubmit(visitlist);
                //db.Contacts.DeleteAllOnSubmit(contactlist);
                //db.Business.DeleteOnSubmit(buinesslist);
                db.Project.DeleteOnSubmit(perject);
                OperateLog opermodel = new OperateLog();
                opermodel.Operator = perject.Users.Name;//操作人
                opermodel.OperTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作时间
                opermodel.OperType = "删除";//操作类型
                opermodel.Operdescribe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + perject.Users.Name + "进行了单个删除操作";
                db.OperateLog.InsertOnSubmit(opermodel);
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

                // string msg = "删除失败";
                //string[] strArray = ids.Split(',');
                //int[] intArray = Array.ConvertAll<string, int>(strArray, x => Convert.ToInt32(x));
                //bool result = bll.Delete(x => intArray.Contains(x.ID));
                //// var ss = bll.Modify(x => intArray.Contains(x.ID));
                //if (result)
                //{
                //    state = "true";
                //    msg = "删除成功";
                //}
                //return Json(new { state = state, msg = msg });
                string result = "";
                string[] strArray = ids.Split(',');
                int[] intArray = Array.ConvertAll<string, int>(strArray, x => Convert.ToInt32(x));
                var item = db.Project.Where(x => intArray.Contains(x.ID)).ToList();
                db.Project.DeleteAllOnSubmit(item);
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
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            dbDataContext db = new HsBusiness.dbDataContext();
            var list = db.Project.Where(x => 1 == 1);

            var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
            if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
            {
                list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
            }

            if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
            {
                list = list.Where(x => x.Users.Areas == user.Areas);
            }

            if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
            {
                list = list.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
            }
            if (!string.IsNullOrEmpty(Request.Form["Areas"].ToString()))
            {
                list = list.Where(x => x.Areas == Request.Form["Areas"].ToString());
            }
            if (Request.Form["State"] != "")
            {

                list = list.Where(x => x.State == int.Parse(Request.Form["State"]));

            }
            if (!string.IsNullOrEmpty(Request.Form["sbirth"].ToString()) && !string.IsNullOrEmpty(Request.Form["sbirth2"].ToString()))
            {
                list = list.Where(x => Convert.ToDateTime(x.LastTime) > Convert.ToDateTime(Request.Form["sbirth"].ToString()) && Convert.ToDateTime(x.LastTime) < Convert.ToDateTime(Request.Form["sbirth2"].ToString()).AddDays(1));
            }
            var list1 = list.ToList().Where(x => 1 == 1).Select(x => new
            {
                x.ID,
                x.Areas,
                x.Introduce,
                x.Instruction,
                x.AddTime,
                x.LastTime,
                State = x.State == 0 ? "正常" : x.State == 1 ? "已派单" : x.State == 2 ? "已回执" : "",
                x.IsRead,
                ProRemind = x.ProRemind.OrderByDescending(y => y.AddTime).FirstOrDefault(),
                ProVisit = x.ProVisit.OrderByDescending(y => y.AddTime).FirstOrDefault(),
                AllVisit = x.ProVisit,

            }).Select(x => new
            {
                x.ID,
                x.Areas,
                x.Introduce,
                x.Instruction,
                AddTime= string.IsNullOrEmpty(x.AddTime) ? "" : Convert.ToDateTime(x.AddTime).ToString("MM-dd-yyyy HH:mm:ss"),
                LastTime= string.IsNullOrEmpty(x.LastTime) ? "" : Convert.ToDateTime(x.LastTime).ToString("MM-dd-yyyy HH:mm:ss"),
                x.State,
                x.IsRead,
                ProRemindContent = x.ProRemind != null ? x.ProRemind.RemindContents : "",
                VisContents = x.ProVisit != null ? x.ProVisit.VisitContents : "",
                VisNextTime = x.ProVisit != null ? x.ProVisit.NextTime : "",
                VisVisitTime = x.ProVisit != null ? x.ProVisit.VisitTime : "",
                x.AllVisit,
            });
            List<string[]> rows = new List<string[]>();
            rows.Add(COLUMNS);
            foreach (var item in list1)
            {
                string[] row = { item.Areas,item.Introduce,item.Instruction,item.AddTime,item.LastTime,item.State,item.ProRemindContent,
                                   };//item.VisVisitTime,item.VisContents,item.VisNextTime
                rows.Add(row);
                var i = 0;
                foreach (var vis in item.AllVisit)
                {
                    i++;
                    string[] visitRow = {"","走访记录"+i,"本次走访时间："+vis.VisitTime,"下次预约时间："+vis.NextTime,"走访内容："+vis.VisitContents,
                    };
                    rows.Add(visitRow);
                }
            }

            var ms = (System.IO.MemoryStream)ExcelHelper.RowsToExcel(rows);
            ExcelHelper.RenderToBrowser(ms, HttpContext.Current, "项目派单统计.xls");
        }
        static readonly string[] COLUMNS = { "区县", "项目简介", "领导批示", "添加时间", "最近更新时间", "状态", "最新回执内容", };//"最新走访时间", "最新走访内容", "下次预约时间"
    }

}