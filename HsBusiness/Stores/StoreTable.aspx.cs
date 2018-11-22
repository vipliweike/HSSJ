
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

namespace HsBusiness.Stores
{
    public partial class StoreTable : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Get(string Areas, string Operator, string starttime, string endtime, string Search, string pageIndex)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                int pageSize = 10;
                var list = db.Store.Where(x => 1 == 1).Select(x => new
                {
                    x.ID,
                    Areas = x.Users.Areas,
                    x.StoreName,
                    x.StoreAddress,
                    x.Broadband,
                    x.Price,
                    x.OverTime,
                    UserName = x.Users.Name,
                    x.UserID,
                    x.State,
                    HzState = x.StoreVisit.OrderBy(y => y.VisitTime).Select(y => y.State).FirstOrDefault(),
                    VisitNum = x.StoreVisit.Count,
                    x.LastTime,
                    x.AddTime,

                });
                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                }

                if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                {
                    list = list.Where(x => x.Areas == user.Areas);
                }

                if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    list = list.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                }
                if (Areas != "")
                {
                    list = list.Where(x => x.Areas == Areas);
                }
                if (Operator != "")
                {
                    list = list.Where(x => x.Broadband == Operator);
                }
                if (starttime != "" && endtime != "")
                {
                    list = list.Where(x => Convert.ToDateTime(x.LastTime) >= Convert.ToDateTime(starttime) && Convert.ToDateTime(x.LastTime) < Convert.ToDateTime(endtime).AddDays(1));
                }
                if (Search != "")
                {
                    list = list.Where(x => x.StoreName.Contains(Search) || x.StoreAddress.Contains(Search));
                }
                var list1 = list.OrderByDescending(x => x.AddTime).Skip((int.Parse(pageIndex) - 1) * pageSize).Take(pageSize).ToList().Select(x => new
                {
                    x.ID,
                    x.Areas,
                    x.StoreName,
                    x.StoreAddress,
                    x.Broadband,
                    x.Price,
                    OverTime = string.IsNullOrEmpty(x.OverTime) ? "" : Convert.ToDateTime(x.OverTime).ToString("yyyy-MM-dd"),
                    x.UserName,
                    x.UserID,
                    x.State,
                    x.HzState,
                    x.VisitNum,
                    LastTime = string.IsNullOrEmpty(x.LastTime) ? "" : Convert.ToDateTime(x.LastTime).ToString("yyyy-MM-dd"),
                });
                var result = new { status = "1", data = list1, pagecount = list.Count().ToString(), pagesize = pageSize.ToString() };
                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [WebMethod]
        public static string Delete(int id)
        {
            using (dbDataContext db = new dbDataContext())
            {
                string result = "";
                var item = db.Store.FirstOrDefault(x => x.ID == id);//用户信息
                //var visitlist = db.Visit.Where(x => x.UserID == id).ToList();//拜访记录
                //var buinesslist = db.Business.FirstOrDefault(x=>x.UserID==id);//商机信息
                //var contactlist = db.Contacts.Where(x => x.BusID == buinesslist.ID).ToList();
                //db.Visit.DeleteAllOnSubmit(visitlist);
                //db.Contacts.DeleteAllOnSubmit(contactlist);
                //db.Business.DeleteOnSubmit(buinesslist);
                db.Store.DeleteOnSubmit(item);
                OperateLog opermodel = new OperateLog();
                opermodel.Operator = item.Users.Name;//操作人
                opermodel.OperTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作时间
                opermodel.OperType = "删除";//操作类型
                opermodel.Operdescribe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + item.Users.Name + "进行了单个删除操作";
                db.OperateLog.InsertOnSubmit(opermodel);
                db.SubmitChanges();
                result = JsonConvert.SerializeObject(new { msg = "删除成功", state = 1 });
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
                var item = db.Store.Where(x => intArray.Contains(x.ID)).ToList();
                db.Store.DeleteAllOnSubmit(item);
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
        /// 修改时得到数据
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetOne(int ID)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Store.Where(x => x.ID == ID).Select(x => new { x.StoreName, x.StoreAddress, x.Broadband, x.Price, x.OverTime, x.ContactName, x.ContactTel }).FirstOrDefault();
                string list1 = new JavaScriptSerializer().Serialize(list);
                return list1;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Edit(int ID, string StoreName, string Broadband, string Price, string OverTime, string ContactName, string ContactTel, string StoreAddress, string OtherNeeds)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var item = db.Store.FirstOrDefault(x => x.ID == ID);
                var result = "";
                if (item != null)
                {
                    item.ID = ID;
                    item.StoreName = StoreName;
                    item.Broadband = Broadband;
                    item.Price = Price;
                    item.OverTime = OverTime;
                    item.ContactName = ContactName;
                    item.ContactTel = ContactTel;
                    item.StoreAddress = StoreAddress;
                    item.OtherNeeds = OtherNeeds;
                    item.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    #region 状态判断
                    if (item.State == 0)//正常
                    {
                        //正常修改不处理
                    }
                    else if (item.State == 1)//已派单
                    {
                        item.State = 0;
                        var sr = item.StoreRemind.Where(x => x.Type == 1).OrderByDescending(x => x.AddTime).FirstOrDefault();//最新提醒
                        if (sr != null)
                        {
                            sr.State = 2;//未处理
                        }
                    }
                    else if (item.State == 2)//已回执
                    {
                        item.State = 0;//正常
                    }
                    #endregion

                    db.SubmitChanges();
                    result = JsonConvert.SerializeObject(new { msg = "修改成功", state = 1 });
                }
                else
                {
                    result = JsonConvert.SerializeObject(new { msg = "修改失败", state = 0 });
                }

                return result;
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            dbDataContext db = new HsBusiness.dbDataContext();
            var list = db.Store.Where(x => 1 == 1).Select(x => new
            {
                x.ID,
                Areas = x.Users.Areas,
                x.StoreName,
                x.StoreAddress,
                x.Broadband,
                x.Price,
                x.OverTime,
                UserName = x.Users.Name,
                x.UserID,
                x.State,
                HzState = x.StoreVisit.OrderBy(y => y.VisitTime).Select(y => y.State).FirstOrDefault(),
                VisitNum = x.StoreVisit.Count,
                x.LastTime,
                x.ContactName,
                x.ContactTel,
                x.AddTime,
                StoreRemind=x.StoreRemind.OrderByDescending(y=>y.AddTime).FirstOrDefault(),
                StoreVisit=x.StoreVisit.OrderByDescending(y=>y.AddTime).FirstOrDefault(),

            }).Select(x=>new {
                x.ID,
                x.Areas,
                x.StoreName,
                x.StoreAddress,
                x.Broadband,
                x.Price,
                x.OverTime,
                x.UserName,
                x.UserID,
                x.State,
                x.HzState ,
                x.VisitNum ,
                x.LastTime,
                x.ContactName,
                x.ContactTel,
                x.AddTime,
                StoreRemindContent=x.StoreRemind !=null?x.StoreRemind.Contents:"",
                VisNextTime=x.StoreVisit!=null?x.StoreVisit.NextTime:"",
                VisVisitTime = x.StoreVisit != null ? x.StoreVisit.VisitTime : "",
                VisContents= x.StoreVisit != null ? x.StoreVisit.VisitContent : "",



            });
            var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
            if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
            {
                list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
            }

            if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
            {
                list = list.Where(x => x.Areas == user.Areas);
            }

            if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
            {
                list = list.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
            }
            if (Request.Form["Areas"].ToString() != "")
            {

                list = list.Where(x => x.Areas == Request.Form["Areas"].ToString());
            }

            if (Request.Form["Operator"].ToString() != "")
            {
                list = list.Where(x => x.Broadband == Request.Form["Operator"].ToString());
            }
            if (Request.Form["sbirth"].ToString() != "" && Request.Form["sbirth2"].ToString() != "")
            {
                list = list.Where(x => Convert.ToDateTime(x.LastTime) > Convert.ToDateTime(Request.Form["sbirth"].ToString()) && Convert.ToDateTime(x.LastTime) < Convert.ToDateTime(Request.Form["sbirth2"].ToString()));
            }
            if (!string.IsNullOrEmpty(Request.Form["Search"].ToString()))
            {
                list = list.Where(x => x.StoreName.Contains(Request.Form["Search"].ToString()) || x.StoreAddress.Contains(Request.Form["Search"].ToString()));
            }
            List<string[]> rows = new List<string[]>();
            rows.Add(COLUMNS);
            foreach (var item in list)
            {
                string[] row = { item.Areas,item.StoreName,item.StoreAddress,item.Broadband,item.Price,item.OverTime,item.UserName,item.AddTime,item.LastTime,item.ContactName,item.ContactTel,item.StoreRemindContent,item.VisVisitTime,item.VisContents,item.VisNextTime
                                   };
                rows.Add(row);
            }

            var ms = (System.IO.MemoryStream)ExcelHelper.RowsToExcel(rows);
            ExcelHelper.RenderToBrowser(ms, HttpContext.Current, "门店列表统计.xls");
        }
        static readonly string[] COLUMNS = { "区县", "门店名称", "地址", "宽带运营商", "宽带价格", "到期时间", "负责人", "建档时间", "最近更新时间", "联系人", "联系人电话","最新回执内容","最新走访时间","最新走访内容","下次预约时间" };
    }
}