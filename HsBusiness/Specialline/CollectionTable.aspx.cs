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

namespace HsBusiness.Specialline
{
    public partial class CollectionTable : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// /获取列表数据
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Get(string Time, string StartTime, string EndTime, string Search, string OverTime, string State, string pageIndex)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                int pageSize = 10;
                var list = db.PrivateLine.Where(x => 1 == 1).Select(x => new
                {
                    x.ID,
                    UserArea = x.Users.Areas,
                    x.UserID,
                    x.CompanyName,
                    x.CompanyAddress,
                    CompanyScale = x.PlInfo.Count,
                    UserName = x.Users.Name,
                    x.AddTime,
                    x.LastTime,
                    OverTime = x.PlInfo.Select(y => new { y.OverTime }),//到期时间
                    x.State
                }).ToList().Select(x => new
                {
                    x.ID,
                    x.UserArea,
                    x.UserID,
                    x.CompanyName,
                    x.CompanyAddress,
                    x.CompanyScale,
                    x.UserName,
                    AddTimeDetial = string.IsNullOrEmpty(x.AddTime) ? "" : Convert.ToDateTime(x.AddTime).ToString("yyyy-MM-dd hh:mm:ss"),
                    LastTimeDetail = string.IsNullOrEmpty(x.LastTime) ? "" : Convert.ToDateTime(x.LastTime).ToString("yyyy-MM-dd hh:mm:ss"),
                    AddTime = string.IsNullOrEmpty(x.AddTime) ? "" : Convert.ToDateTime(x.AddTime).ToString("yyyy-MM-dd"),
                    LastTime = string.IsNullOrEmpty(x.LastTime) ? "" : Convert.ToDateTime(x.LastTime).ToString("yyyy-MM-dd"),
                    OverTime = x.OverTime.Select(y => Convert.ToDateTime(y.OverTime).ToString("yyyyMM")),//到期时间
                    x.State
                });
                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                }

                if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                {
                    list = list.Where(x => x.UserArea == user.Areas);
                }

                if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    list = list.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                }
                if (!string.IsNullOrEmpty(Time))
                {

                    if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    {
                        if (Time == "1")//发布时间
                        {
                            list = list.Where(x => Convert.ToDateTime(x.AddTime) >= Convert.ToDateTime(StartTime) && Convert.ToDateTime(x.AddTime) < Convert.ToDateTime(EndTime).AddDays(1));
                        }
                        else //更新时间
                        {
                            list = list.Where(x => Convert.ToDateTime(x.LastTime) >= Convert.ToDateTime(StartTime) && Convert.ToDateTime(x.LastTime) < Convert.ToDateTime(EndTime).AddDays(1));
                        }

                    }
                }
                if (!string.IsNullOrEmpty(OverTime))
                {
                    list = list.Where(x => x.OverTime.Contains(OverTime));
                }
                if (!string.IsNullOrEmpty(Search))

                {
                    list = list.Where(x => x.CompanyName.Contains(Search) || x.CompanyAddress.Contains(Search) || x.UserName.Contains(Search) || x.CompanyScale.ToString().Contains(Search));
                }
                if (!string.IsNullOrEmpty(State))
                {
                    list = list.Where(x => x.State == int.Parse(State));
                }
                var list1 = list.OrderByDescending(x => x.LastTime).Skip((int.Parse(pageIndex) - 1) * pageSize).Take(pageSize).ToList().Select(x => new
                {
                    x.ID,
                    x.UserArea,
                    x.UserID,
                    x.CompanyName,
                    x.CompanyAddress,
                    x.CompanyScale,
                    x.UserName,
                    x.AddTime,
                    x.LastTime,
                    x.LastTimeDetail,
                    x.AddTimeDetial,
                    State = x.State == 0 ? "跟进" : x.State == 1 ? "落单" : x.State == 2 ? "放弃" : ""

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
                var item = db.PrivateLine.FirstOrDefault(x => x.ID == id);//用户信息
                //var visitlist = db.Visit.Where(x => x.UserID == id).ToList();//拜访记录
                //var buinesslist = db.Business.FirstOrDefault(x=>x.UserID==id);//商机信息
                //var contactlist = db.Contacts.Where(x => x.BusID == buinesslist.ID).ToList();
                //db.Visit.DeleteAllOnSubmit(visitlist);
                //db.Contacts.DeleteAllOnSubmit(contactlist);
                //db.Business.DeleteOnSubmit(buinesslist);
                db.PrivateLine.DeleteOnSubmit(item);
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
                var item = db.PrivateLine.Where(x => intArray.Contains(x.ID)).ToList();
                db.PrivateLine.DeleteAllOnSubmit(item);
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

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                var list = db.PrivateLine.Where(x => 1 == 1).Select(x => new
                {
                    x.ID,
                    UserArea = x.Users.Areas,
                    x.CompanyName,
                    x.CompanyAddress,
                    x.CompanyScale,//电脑台数
                    SpecilNum = x.PlInfo.ToList(),//专线数量
                    Operator = x.PlInfo.FirstOrDefault().Operator,//合作运营商
                    WeekPrice = x.PlInfo.FirstOrDefault().WeekPrice,//周价
                    BandWidth = x.PlInfo.FirstOrDefault().BandWidth,//宽带
                    PayType = x.PlInfo.FirstOrDefault().PayType,//付费方式
                    OverTime = x.PlInfo.Select(y => new { y.OverTime }),//到期时间
                    LinkMan = x.PlContacts.FirstOrDefault().Name,
                    LinkManTel = x.PlContacts.FirstOrDefault().Tel,
                    UserName = x.Users.Name,//提交人姓名
                    x.UserID,
                    x.AddTime,//建档时间
                    Month = x.AddTime,//月份
                    x.LastTime,
                    x.PlVisit,
                    x.PlInfo,
                    x.State,
                    ServerBerSys = x.PlInfo.FirstOrDefault().ServerBerSys,
                    ServerUsingTime = x.PlInfo.FirstOrDefault().ServerUsingTime,
                    IsCloudPlan = x.PlInfo.FirstOrDefault().IsCloudPlan

                }).OrderByDescending(x => x.AddTime).ToList().Select(x => new
                {
                    x.ID,
                    x.UserArea,
                    x.CompanyName,
                    x.CompanyAddress,
                    x.CompanyScale,//电脑台数
                    x.SpecilNum,//专线数量
                    x.Operator,//合作运营商
                    x.WeekPrice,//周价
                    x.BandWidth,//宽带
                    x.PayType,//付费方式
                    //x.OverTime,//到期时间
                    OverTime = x.OverTime.Select(y => Convert.ToDateTime(y.OverTime).ToString("yyyyMM")),
                    x.LinkMan,
                    x.LinkManTel,
                    x.UserName,//提交人姓名
                    x.UserID,
                    AddTime = string.IsNullOrEmpty(x.AddTime) ? "" : Convert.ToDateTime(x.AddTime).ToString("MM-dd-yyyy HH:mm:ss"),//建档时间
                    Month = Convert.ToDateTime(x.Month).ToString("yyyyMM"),//月份
                    LastTime = string.IsNullOrEmpty(x.LastTime) ? "" : Convert.ToDateTime(x.LastTime).ToString("MM-dd-yyyy HH:mm:ss"),
                    x.PlVisit,
                    x.PlInfo,
                    x.State,
                    x.ServerBerSys,
                    x.ServerUsingTime,
                    x.IsCloudPlan
                    //x.Month = Convert.ToDateTime(x.Month).ToString("yyyyMM")

                });

                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                }

                if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                {
                    list = list.Where(x => x.UserArea == user.Areas);
                }

                if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    list = list.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                }

                if (!string.IsNullOrEmpty(Request.Form["timecontorl"]))
                {

                    if (!string.IsNullOrEmpty(Request.Form["sbirth"]) && !string.IsNullOrEmpty(Request.Form["sbirth2"]))
                    {
                        if (Request.Form["timecontorl"] == "1")//发布时间
                        {
                            list = list.Where(x => Convert.ToDateTime(x.AddTime) >= Convert.ToDateTime(Request.Form["sbirth"]) && Convert.ToDateTime(x.AddTime) < Convert.ToDateTime(Request.Form["sbirth2"]).AddDays(1));
                        }
                        else //更新时间
                        {
                            list = list.Where(x => Convert.ToDateTime(x.LastTime) >= Convert.ToDateTime(Request.Form["sbirth"]) && Convert.ToDateTime(x.LastTime) < Convert.ToDateTime(Request.Form["sbirth2"]).AddDays(1));
                        }

                    }
                }
                if (!string.IsNullOrEmpty(Request.Form["ovretime"]))
                {
                    list = list.Where(x => x.OverTime.Contains(Request.Form["ovretime"]));
                }
                if (!string.IsNullOrEmpty(Request.Form["Search"]))
                {
                    var Search = Request.Form["Search"];
                    list = list.Where(x => x.CompanyName.Contains(Search) || x.CompanyAddress.Contains(Search) || x.UserName.Contains(Search) || x.CompanyScale.ToString().Contains(Search));
                }
                if (!string.IsNullOrEmpty(Request.Form["state"]))
                {
                    list = list.Where(x => x.State == int.Parse(Request.Form["state"]));
                }
                //var result = list.ToList().Select(x => new
                //{

                //    x.ID,
                //    x.UserArea,
                //    x.CompanyName,
                //    x.CompanyAddress,
                //    x.CompanyScale,//电脑台数
                //   // x.SpecilNum,//专线数量
                //    x.Operator,//合作运营商
                //    x.WeekPrice,//周价
                //    x.BandWidth,//宽带
                //    x.PayType,//付费方式
                //    x.OverTime,//到期时间
                //    x.LinkMan,
                //    x.LinkManTel,
                //    x.UserName,//提交人姓名
                //    x.UserID,
                //    x.AddTime,
                //    x.LastTime,
                //    x.PlVisit,
                //    x.PlInfo,
                //    Month = Convert.ToDateTime(x.Month).ToString("yyyyMM")//月份
                //});
                var allBusIDs = list.Select(t => t.ID).ToList();

                var allVisit = db.VPLVisitNum.Where(t => allBusIDs.Contains(Convert.ToInt32(t.PlID)));
                var maxVisitNum = Convert.ToInt32(allVisit.Max(t => t.num));
                var columnNum = 20 + (maxVisitNum * 2);
                string[] COLUMNS = new string[columnNum];
                // static readonly string[] COLUMNS = {  "宽带", "付费方式", "到期时间","联系人姓名","联系人电话","提交人姓名",};//"最新走访时间", "最新走访内容", "下次预约时间"
                COLUMNS[0] = "月份";
                COLUMNS[1] = "建档时间";
                COLUMNS[2] = "最近更新时间";
                COLUMNS[3] = "区县";
                COLUMNS[4] = "单位名称";
                COLUMNS[5] = "单位地址";
                COLUMNS[6] = "电脑台数";
                COLUMNS[7] = "专线条数";
                COLUMNS[8] = "合作运营商";
                COLUMNS[9] = "周价";
                COLUMNS[10] = "带宽";
                COLUMNS[11] = "付费方式";
                COLUMNS[12] = "到期时间";
                COLUMNS[13] = "联系人姓名";
                COLUMNS[14] = "联系人电话";
                COLUMNS[15] = "提交人姓名";
                COLUMNS[16] = "状态";
                COLUMNS[17] = "服务器承载系统";
                COLUMNS[18] = "现服务器开始使用时间";
                COLUMNS[19] = "是否有上云计划";
                var addColumn = maxVisitNum * 2;
                var j = 1;
                for (int i = 1; i <= addColumn; i++)
                {
                    if (i % 2 == 1)
                    {
                        COLUMNS[19 + i] = "走访记录" + j + "时间";
                    }
                    else
                    {
                        COLUMNS[19 + i] = "走访记录" + j + "内容";
                        j++;
                    }
                }
                var visitList = db.PlVisit.Where(t => allBusIDs.Contains(Convert.ToInt32(t.PlID)));
                var pllist = db.PrivateLine.Where(x => 1 == 1).ToList();
                List<string[]> rows = new List<string[]>();
                rows.Add(COLUMNS);

                foreach (var item in list)
                {
                    for (int k = 0; k < item.PlInfo.Count; k++)
                    {
                        string[] row = new string[columnNum];
                        row[0] = item.Month;
                        row[1] = item.AddTime;
                        row[2] = item.LastTime;
                        row[3] = item.UserArea;
                        row[4] = item.CompanyName;
                        row[5] = item.CompanyAddress;
                        row[6] = item.CompanyScale;
                        row[13] = item.LinkMan;
                        row[14] = item.LinkManTel;
                        row[15] = item.UserName;
                        row[16] = item.State.ToString() == "0" ? "跟进" : item.State.ToString() == "1" ? "落单" : item.State.ToString() == "2" ? "放弃" : "";
                        if (item.PlInfo[k].Type == 3)
                        {
                            row[17] = item.PlInfo[k].ServerBerSys;
                            row[18] = item.PlInfo[k].ServerUsingTime;
                            row[19] = item.PlInfo[k].IsCloudPlan.ToString() == "0" ? "否" : "是";
                        }

                        var currentList = visitList.Where(t => t.PlID == item.ID).Select(t => new { t.ID, t.VisitTime, t.VisitContents }).ToList();
                        for (int i = 1; i <= currentList.Count; i++)
                        {
                            row[19 + (i * 2 - 1)] = currentList[i - 1].VisitTime;
                            row[19 + (i * 2)] = currentList[i - 1].VisitContents;
                        }
                        row[7] = (item.PlInfo[k].Type.GetValueOrDefault(1) == 1 ? "专线" : item.PlInfo[k].Type.GetValueOrDefault(1) == 2 ? "电路" : "云业务") + (k + 1);//专线条数

                        row[8] = item.PlInfo[k].Operator;
                        row[9] = item.PlInfo[k].WeekPrice;
                        row[10] = item.PlInfo[k].BandWidth;
                        row[11] = item.PlInfo[k].PayType;
                        row[12] = item.PlInfo[k].OverTime.ToString();
                        rows.Add(row);
                    }

                }
                var ms = (System.IO.MemoryStream)ExcelHelper.RowsToExcel(rows);
                ExcelHelper.RenderToBrowser(ms, HttpContext.Current, "专线建档统计.xls");
            }
        }
        // static readonly string[] COLUMNS = {"区县" ,"单位名称", "单位地址", "电脑台数","专线条数", "合作运营商", "周价", "宽带", "付费方式", "到期时间","联系人姓名","联系人电话","提交人姓名",};//"最新走访时间", "最新走访内容", "下次预约时间"
    }
}