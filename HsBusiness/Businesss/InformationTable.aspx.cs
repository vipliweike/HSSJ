using HsBusiness.Interface.Comm;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.Businesss
{
    public partial class PersonnelTable : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 商机查询
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="Areas"></param>
        /// <param name="Industry"></param>
        /// <param name="Operator"></param>
        /// <param name="Scale"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        [WebMethod]
        public static string Get(string Areas, string Industry, string Operator, string Scale, string starttime, string endtime, string Search, string pageIndex, string TimeType)
        {
            //var data = "";

            using (dbDataContext db = new dbDataContext())
            {
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();

                int pageSize = 10;

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
                    x.LastTime,
                    x.MState,
                    x.FState,
                    UserName = x.Users.Name,
                    VisitNum = x.Visit.Count
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
                    list = list.Where(x => x.CustomerScale == Scale || x.FScale == Scale || x.FUseScale == Scale);
                }
                if (starttime != "" && endtime != "")
                {
                    if (TimeType == "1")//更新时间
                    {
                        list = list.Where(x => Convert.ToDateTime(x.LastTime) >= Convert.ToDateTime(starttime) && Convert.ToDateTime(x.LastTime) < Convert.ToDateTime(endtime).AddDays(1));
                    }
                    if (TimeType == "2")//创建时间
                    {
                        list = list.Where(x => Convert.ToDateTime(x.AddTime) >= Convert.ToDateTime(starttime) && Convert.ToDateTime(x.AddTime) < Convert.ToDateTime(endtime).AddDays(1));
                    }
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    list = list.Where(x => x.CompanyName.Contains(Search) || x.Areas.Contains(Search) || x.UserName.Contains(Search));
                }

                var list1 = list.OrderByDescending(x => x.AddTime).Skip((int.Parse(pageIndex) - 1) * pageSize).Take(pageSize).ToList();
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
        [WebMethod]
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
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
        [WebMethod]
        /// <summary>
        /// 得到区县
        /// </summary>
        /// <returns></returns>
        public static string GetRegion1()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Region.Where(x => x.Pid == 0).Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Region.Where(x => x.Pid == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void Export(string Areas, string Industry, string Operator, string Scale, string starttime, string endtime, string Search)
        {
            dbDataContext db = new HsBusiness.dbDataContext();
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
                x.LastTime,
                x.MState,
                x.FState,
                UserName = x.Users.Name,
                VisitNum = x.Visit.Count
            });
            if (Areas != "")
            {
                list = list.Where(x => x.Areas == Areas);
            }
            if (Industry != "")
            {
                list = list.Where(x => x.Industry == Industry);
            }
            if (Operator != "")
            {
                list = list.Where(x => x.FOperator == Operator);
            }
            if (Scale != "")
            {
                list = list.Where(x => x.CustomerScale == Scale);
            }
            if (starttime != "" && endtime != "")
            {
                list = list.Where(x => Convert.ToDateTime(x.LastTime) > Convert.ToDateTime(starttime) && Convert.ToDateTime(x.LastTime) < Convert.ToDateTime(endtime));
            }
            if (!string.IsNullOrEmpty(Search))
            {
                list = list.Where(x => x.CompanyName.Contains(Search) || x.Areas.Contains(Search));
            }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            dbDataContext db = new HsBusiness.dbDataContext();
            var list = db.Business.Where(x => 1 == 1).Select(x => new
            {
                x.Areas,
                x.ID,
                x.UserID,
                x.Industry,
                x.IsFixed,
                x.IsMove,
                x.AddTime,
                x.MCardUse,
                x.MPushWork,
                x.MMobile,//移动占比
                x.MUnicom,//联通占比
                x.MTelecom,//电信占比
                x.MAgeGroup,
                x.MIsSubsidy,
                x.MQuota,
                x.MRange,
                x.MPolicy,
                x.MOverTime,//租机到期时间
                x.MMonthFee,
                x.MFocus,
                x.MIncome,
                x.MOtherWork,
                x.FPushWork,
                x.FBandWidth,//宽带
                x.FIsDomain,
                x.FPreWeekPrice,
                x.FPreInNetMouth,
                x.FPreAnlIncome,
                x.FUseWork,
                x.FUseScale,
                x.FUseBandWidth,
                x.FWeekPrice,
                x.FOverTime,
                x.FAlsAnlIncome,
                x.FComputerNum,
                x.FIsSatisfy,
                x.FIsServer,
                x.FPlatform,
                x.FOtherWork,
                x.CompanyAddress,
                x.IsHavePhoneList,
                x.CompanyName,
                x.CustomerScale,
                x.MOperator,
                x.FOperator,
                x.FScale,//规模
                x.LastTime,
                x.MState,
                x.FState,
                UserName = x.Users.Name,
                usertel = x.Users.Mobile,//提交人手机号
                LinkMan = x.Contacts.FirstOrDefault().Name,
                LinkManTel = x.Contacts.FirstOrDefault().Tel,
                VisitNum = x.Visit.Count,
                BusRemind = x.BusRemind.Where(y=>y.State==1).OrderByDescending(y => y.AddTime).FirstOrDefault(),
                Visit = x.Visit.OrderByDescending(y => y.AddTime).FirstOrDefault(),
                AllVisit = x.Visit,
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
            //dynamic aa = JsonConvert.DeserializeObject(where.Value);
            if (Request.Form["Areas"].ToString() != "")
            {

                list = list.Where(x => x.Areas == Request.Form["Areas"].ToString());
            }
            if (Request.Form["Industry"].ToString() != "")
            {
                list = list.Where(x => x.Industry == Request.Form["Industry"].ToString());
            }
            if (Request.Form["Operator"].ToString() != "")
            {
                list = list.Where(x => x.FOperator == Request.Form["Operator"].ToString());
            }
            if (Request.Form["Scale"].ToString() != "")
            {
                list = list.Where(x => x.CustomerScale == Request.Form["Scale"].ToString());
            }
            
            if (Request.Form["sbirth"].ToString() != "" && Request.Form["sbirth2"].ToString() != "")
            {
                if (Request.Form["TimeType"] == "1")//更新时间
                {
                    list = list.Where(x => Convert.ToDateTime(x.LastTime) >= Convert.ToDateTime(Request.Form["sbirth"].ToString()) && Convert.ToDateTime(x.LastTime) < Convert.ToDateTime(Request.Form["sbirth2"].ToString()).AddDays(1));
                }
                if (Request.Form["TimeType"] == "2")//创建时间
                {
                    list = list.Where(x => Convert.ToDateTime(x.AddTime) >= Convert.ToDateTime(Request.Form["sbirth"].ToString()) && Convert.ToDateTime(x.AddTime) < Convert.ToDateTime(Request.Form["sbirth2"].ToString()).AddDays(1));
                }
               
            }
            if (!string.IsNullOrEmpty(Request.Form["Search"].ToString()))
            {
                list = list.Where(x => x.CompanyName.Contains(Request.Form["Search"].ToString()) || x.Areas.Contains(Request.Form["Search"].ToString()));
            }
            var result = list.ToList().Select(x => new
            {
                //mm-dd-yyyy hh:mm:ss
                x.Areas,
                x.ID,
                x.UserID,
                x.Industry,
                x.IsFixed,
                x.IsMove,
                AddTime = string.IsNullOrEmpty(x.AddTime) ? "": Convert.ToDateTime(x.AddTime).ToString("MM-dd-yyyy HH:mm:ss"),
                x.MCardUse,
                x.MPushWork,
                x.MMobile,//移动占比
                x.MUnicom,//联通占比
                x.MTelecom,//电信占比
                x.MAgeGroup,
                MIsSubsidy = x.MIsSubsidy == "1" ? "是" : "否",
                x.MQuota,
                x.MRange,
                x.MPolicy,
                x.MOverTime,//租机到期时间
                x.MMonthFee,
                x.MFocus,
                x.MIncome,
                x.MOtherWork,
                x.FPushWork,
                x.FBandWidth,//宽带
                FIsDomain = x.FIsDomain.GetValueOrDefault() == 1 ? "是" : "否",
                x.FPreWeekPrice,
                x.FPreInNetMouth,
                x.FPreAnlIncome,
                x.FUseWork,
                x.FUseScale,
                x.FUseBandWidth,
                x.FWeekPrice,
                x.FOverTime,
                x.FAlsAnlIncome,
                x.FComputerNum,
                FIsSatisfy = x.FIsSatisfy == "1" ? "是" : "否",
                FIsServer = x.FIsServer.GetValueOrDefault() == 1 ? "是" : "否",
                x.FPlatform,
                x.FOtherWork,
                x.CompanyAddress,
                IsHavePhoneList = x.IsHavePhoneList.GetValueOrDefault() == 1 ? "是" : "否",
                x.CompanyName,
                x.CustomerScale,
                x.MOperator,
                x.FOperator,
                x.FScale,//规模
                LastTime = string.IsNullOrEmpty(x.LastTime) ? "":Convert.ToDateTime(x.LastTime).ToString("MM-dd-yyyy HH:mm:ss"),
                x.MState,
                x.FState,
                UserName = x.UserName,
                usertel = x.usertel,//提交人手机号
                LinkMan = x.LinkMan,
                LinkManTel = x.LinkManTel,
                VisitNum = x.VisitNum,
                RemindContent = x.BusRemind != null ? x.BusRemind.Contents : "",//最新回执内容
               
            });
            var allBusIDs = result.Select(t => t.ID).ToList();
            var allVisit = db.VVisitNum.Where(t => allBusIDs.Contains(Convert.ToInt32(t.BusID)));
            var maxVisitNum = Convert.ToInt32(allVisit.Max(t => t.num));
            var columnNum = 47 + (maxVisitNum * 2);
            string[] COLUMNS = new string[columnNum];
            COLUMNS[0] = "区县";
            COLUMNS[1] = "企业名称";
            COLUMNS[2] = "行业归属";
            COLUMNS[3] = "客户规模";
            COLUMNS[4] = "建档时间";
            COLUMNS[5] = "最近更新时间";
            COLUMNS[6] = "手机卡用途";
            COLUMNS[7] = "移动可推业务";
            COLUMNS[8] = "联通占比";
            COLUMNS[9] = "移动占比";
            COLUMNS[10] = "电信占比";
            COLUMNS[11] = "员工年龄段";
            COLUMNS[12] = "是否有员工补贴";
            COLUMNS[13] = "补贴额度";
            COLUMNS[14] = "补贴范围";
            COLUMNS[15] = "使用政策";
            COLUMNS[16] = "租机到期时间";
            COLUMNS[17] = "使用月消费";
            COLUMNS[18] = "使用侧重";
            COLUMNS[19] = "年收入预测";
            COLUMNS[20] = "移动其他业务";
            COLUMNS[21] = "固网可推业务";
            COLUMNS[22] = "规模";
            COLUMNS[23] = "带宽";
            COLUMNS[24] = "是否跨域";
            COLUMNS[25] = "预计周价";
            COLUMNS[26] = "预计入网月份";
            COLUMNS[27] = "预计年收入";
            COLUMNS[28] = "合作运营商";
            COLUMNS[29] = "固网在用业务";
            COLUMNS[30] = "在用业务规模";
            COLUMNS[31] = "在用业务带宽";
            COLUMNS[32] = "周价";
            COLUMNS[33] = "到期时间";
            COLUMNS[34] = "友商年收益";
            COLUMNS[35] = "电脑台数";
            COLUMNS[36] = "现有业务是否满意";
            COLUMNS[37] = "是否有服务器";
            COLUMNS[38] = "服务器承载平台";
            COLUMNS[39] = "固网其他业务";
            COLUMNS[40] = "客户地址";
            COLUMNS[41] = "是否有员工通讯录";
            COLUMNS[42] = "提交人姓名";
            COLUMNS[43] = "提交人手机号";
            COLUMNS[44] = "联系人姓名";
            COLUMNS[45] = "联系人电话";
        
            //……
            COLUMNS[46] = "最新回执内容";
            var addColumn = maxVisitNum * 2;
            var j = 1;
            for (int i = 1; i <= addColumn; i++)
            {
                if (i % 2 == 1)
                {
                    COLUMNS[46 + i] = "走访记录" + j + "时间";
                }
                else
                {
                    COLUMNS[46 + i] = "走访记录" + j + "内容";
                    j++;
                }
            }

            var visitList = db.Visit.Where(t => allBusIDs.Contains(Convert.ToInt32(t.BusID)));

            List<string[]> rows = new List<string[]>();
            rows.Add(COLUMNS);
            foreach (var item in result)
            {
                string[] row = new string[columnNum];
                row[0] = item.Areas;
                row[1] = item.CompanyName;
                row[2] = item.Industry;
                row[3] = item.CustomerScale;
                row[4] = item.AddTime;//建档时间
                row[5] = item.LastTime;
                row[6] = item.MCardUse;
                row[7] = item.MPushWork;
                row[8] = item.MUnicom;
                row[9] = item.MMobile;
                row[10] = item.MTelecom;
                row[11] = item.MAgeGroup;
                row[12] = item.MIsSubsidy;
                row[13] = item.MQuota;
                row[14] = item.MRange;
                row[15] = item.MPolicy;
                row[16] = item.MOverTime;//建档时间
                row[17] = item.MMonthFee;
                row[18] = item.MFocus;
                row[19] = item.MIncome;
                row[20] = item.MOtherWork;
                row[21] = item.FPushWork;//建档时间
                row[22] = item.FScale;
                row[23] = item.FBandWidth;
                row[24] = item.FIsDomain.ToString();//建档时间
                row[25] = item.FPreWeekPrice;
                row[26] = item.FPreInNetMouth;
                row[27] = item.FPreAnlIncome;
                row[28] = item.FOperator;
                row[29] = item.FUseWork;//建档时间
                row[30] = item.FUseScale;
                row[31] = item.FUseBandWidth;

                row[32] = item.FWeekPrice;//建档时间
                row[33] = item.FOverTime;
                row[34] = item.FAlsAnlIncome;
                row[35] = item.FComputerNum;
                row[36] = item.FIsSatisfy;
                row[37] = item.FIsServer;//建档时间
                row[38] = item.FPlatform;
                row[39] = item.FOtherWork;
                row[40] = item.CompanyAddress;//建档时间
                row[41] = item.IsHavePhoneList;
                row[42] = item.UserName;//建档时间
                row[43] = item.usertel;
                row[44] = item.LinkMan;
                row[45] = item.LinkManTel;//建档时间
                //……
                row[46] = item.RemindContent;
                var currentList = visitList.Where(t => t.BusID == item.ID).Select(t => new { t.ID, t.VisitTime, t.VisitContent }).ToList();
                for (int i = 1; i <= currentList.Count; i++)
                {
                    row[46 + (i * 2 - 1)] = currentList[i - 1].VisitTime;
                    row[46 + (i * 2)] = currentList[i - 1].VisitContent;
                }

                //string[] row = { item.Areas,item.CompanyName,item.Industry,item.CustomerScale,item.AddTime,item.MCardUse,item.MPushWork,item.MUnicom,item.MMobile,item.MTelecom,item.MAgeGroup,item.MIsSubsidy,item.MQuota,item.MRange,item.MPolicy,item.MOverTime,item.MMonthFee,item.MFocus,item.MIncome,item.MOtherWork,item.FPushWork,item.FScale,item.FBandWidth,item.FIsDomain.ToString(),item.FPreWeekPrice,item.FPreInNetMouth,item.FPreAnlIncome,item.FOperator,item.FUseWork,item.FUseScale,item.FUseBandWidth,item.FWeekPrice,item.FOverTime,item.FAlsAnlIncome,item.FComputerNum,item.FIsSatisfy,item.FIsServer.ToString(),item.FPlatform,item.FOtherWork,item.CompanyAddress,item.IsHavePhoneList.ToString(),item.UserName,item.usertel,item.LinkMan,item.LinkManTel,item.RemindContent,
                //                    };//item.VisitTime,item.VisitContent,item.IsLeader,item.Leader,item.NextTime

                rows.Add(row);
                //var i = 0;
                //foreach (var vis in item.AllVisit)
                //{
                //    i++;
                //    string[] visitRow = {"","走访记录"+i,"本次走访时间："+vis.VisitTime,"是否需要二次拜访："+(vis.IsAgain == 0?"否":"是"),"是否有业务需求："+(vis.IsNeed==0?"否":"是")
                //            ,"是否需协同领导："+(vis.IsLeader == 0?"否":"是"),"协同领导："+vis.Leader,"下次预约时间："+vis.NextTime,"走访内容："+vis.VisitContent
                //    };
                //    rows.Add(visitRow);
                //}
            }

            var ms = (System.IO.MemoryStream)ExcelHelper.RowsToExcel(rows);
            ExcelHelper.RenderToBrowser(ms, HttpContext.Current, "商机列表统计.xls");
        }

        //static readonly string[] COLUMNS = { "区县", "企业名称", "行业归属", "客户规模", "建档时间", "手机卡用途", "移动可推业务", "联通占比", "移动占比", "电信占比", "员工年龄段", "是否有员工补贴", "补贴额度", "补贴范围", "使用政策", "租机到期时间", "使用月消费", "使用侧重", "年收入预测", "移动其他业务", "固网可推业务", "规模", "带宽", "是否跨域", "预计周价", "预计入网月份", "预计年收入", "合作运营商", "固网在用业务", "在用业务规模", "在用业务带宽", "周价", "到期时间", "友商年收益", "电脑台数", "现有业务是否满意", "是否有服务器", "服务器承载平台", "固网其他业务", "客户地址", "是否有员工通讯录", "提交人姓名", "提交人手机号", "联系人姓名", "联系人电话", "最新回执内容" };//"最新走访时间", "最新走访内容", "是否需协同领导", "协同领导", "下次预约时间"
        //static readonly string[] VISCOLUMS = { "是否需要二次拜访", "是否有业务需求", "是否需协同领导", "协同领导", "本次拜访时间", "下次预约时间", "本次拜访内容" };
    }
}