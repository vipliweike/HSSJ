using HsBusiness.Api.Common;
using HsBusiness.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class PrivateLineController : ApiController
    {
        /// <summary>
        /// 获取专线列表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    //真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var PageIndex = Convert.ToInt32(parameters["PageIndex"]);
                    var PageSize = Convert.ToInt32(parameters["PageSize"]);
                    var Search = parameters["Search"];
                    var Operator = parameters["Operator"];//运营商
                    var Areas = parameters["Areas"];//区县
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var Month = parameters["Month"];//月份
                    var Day = parameters["Day"];//天
                    //查询状态（条件)
                    var QueryState = parameters["QueryState"];//0跟进 1落单 ，2放弃  3 即将到期

                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    if (user != null)
                    {
                        var list = db.PrivateLine.Where(x => 1 == 1);
                        #region 角色判断
                        if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        {
                            list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                        }
                        else
                        if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                        {
                            list = list.Where(x => x.Users.Areas == user.Areas);
                        }
                        else
                        if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                        {
                            list = list.Where(x => x.UserID == UserID);
                        }
                        else
                        {
                            list = list.Where(x => x.ID < 0);//无数据
                        }
                        #endregion
                        #region 查询                       
                        if (!string.IsNullOrEmpty(Operator))//宽带运营商查询
                        {
                            list = list.Where(x => x.PlInfo.Where(y => y.Operator == Operator).Count() > 0);//专线信息有的数量
                        }
                        if (!string.IsNullOrEmpty(Areas))//区县查询
                        {
                            list = list.Where(x => x.Users.Areas.Contains(Areas));
                        }
                        if (!string.IsNullOrEmpty(Month))//当前月份
                        {
                            list = list.Where(x => Convert.ToDateTime(x.AddTime).Year == Convert.ToDateTime(Month).Year && Convert.ToDateTime(x.AddTime).Month == Convert.ToDateTime(Month).Month);
                        }
                        if (!string.IsNullOrEmpty(Search))//通过
                        {
                            list = list.Where(x => x.CompanyName.Contains(Search) || x.CompanyAddress.Contains(Search) || x.Users.Name.Contains(Search));
                        }
                        if (!string.IsNullOrEmpty(Day))//日期
                        {
                            var date = Convert.ToDateTime(Day);//日期
                            list = list.Where(x => Convert.ToDateTime(x.AddTime).Year == date.Year && Convert.ToDateTime(x.AddTime).Month == date.Month && Convert.ToDateTime(x.AddTime).Day == date.Day);
                        }
                        #endregion
                        var resultlist = list
                            .Select(x => new
                            {
                                x.ID,
                                x.CompanyAddress,
                                x.CompanyName,
                                CompanyScale = x.PlInfo.Count,
                                x.AddTime,
                                x.LastTime,
                                UserName = x.Users.Name,
                                State = x.State.GetValueOrDefault(0),
                                Remind = x.PlRemind.Where(y => y.State == 0).OrderByDescending(y => y.AddTime).FirstOrDefault()
                            }).Select(x => new
                            {
                                x.ID,
                                x.CompanyAddress,
                                x.CompanyName,
                                x.CompanyScale,
                                x.AddTime,
                                x.LastTime,
                                x.UserName,
                                x.State,
                                Next = x.Remind != null ? x.Remind.Type == 1 ? "专线到期" : x.Remind.Type == 2 ? "预约到期" : "" : "",
                            });
                        if (!string.IsNullOrEmpty(QueryState))
                        {
                            if (QueryState == "0")//跟进
                            {
                                resultlist = resultlist.Where(x => x.State == 0);
                            }
                            if (QueryState == "1")//落单
                            {
                                resultlist = resultlist.Where(x => x.State == 1);
                            }
                            if (QueryState == "2")//放弃
                            {
                                resultlist = resultlist.Where(x => x.State == 2);
                            }
                            if (QueryState == "3")//即将到期
                            {
                                resultlist = resultlist.Where(x => x.Next != "");
                            }
                        }
                        var result = resultlist
                            .OrderByDescending(x => x.Next)//先根据提醒排序
                            .ThenByDescending(x => x.LastTime)//再根据最近更新时间排序
                            .Skip((PageIndex > 0 ? PageIndex - 1 : 0) * PageSize).Take(PageSize)//分页
                            .ToList();
                        return Json(new { data = result, state = 1, msg = "请求成功" });
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
        /// 获取专线详情
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetOne(string data, string secret)
        {
            try
            {
                //真实的参数
                var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                var PlID = Convert.ToInt32(parameters["PlID"]);
                
                if (PlID != 0)
                {
                    using (dbDataContext db = new dbDataContext())
                    {
                        var list = db.PrivateLine.Where(x => x.ID == PlID).Select(x => new
                        {
                            x.ID,
                            x.CompanyAddress,
                            x.CompanyName,
                            x.CompanyScale,
                            x.AddTime,
                            x.LastTime,
                            x.Remark,//备注
                            UserName = x.Users.Name,
                            State = x.State.GetValueOrDefault(0),
                            PlContacts = x.PlContacts.Select(y => new
                            {
                                y.Name,
                                y.Post,
                                y.Tel,
                            }),
                            //专线信息
                            PlInfo = x.PlInfo.Select(y => new
                            {
                                y.ID,
                                y.BandWidth,
                                y.Operator,
                                y.OverTime,
                                y.PayType,
                                y.WeekPrice,
                                y.Type,
                                y.ServerBerSys,
                                y.ServerUsingTime,
                                y.IsCloudPlan,
                                Remind = y.PlRemind.Where(t => t.Type == 1 && t.State == 0).OrderByDescending(t => t.AddTime).FirstOrDefault()
                            }).Select(y => new
                            {
                                y.ID,
                                y.BandWidth,
                                y.Operator,
                                y.OverTime,
                                y.PayType,
                                y.WeekPrice,
                                Type = y.Type.GetValueOrDefault(1),
                                y.ServerBerSys,
                                y.ServerUsingTime,
                                y.IsCloudPlan,
                                RemindID = y.Remind != null ? y.Remind.ID : 0,
                            }),
                            //走访记录
                            PlVisit = x.PlVisit.OrderByDescending(y => y.AddTime).Select(y => new
                            {
                                y.ID,
                                y.IsAgain,
                                y.IsNeed,
                                y.PlID,
                                y.AddTime,
                                y.VisitTime,
                                y.NextTime,
                                y.VisitContents,
                                y.Img,
                                Remind = y.PlRemind.Where(t => t.Type == 2 && t.State == 0).OrderByDescending(t => t.AddTime).FirstOrDefault()
                            }).Select(y => new
                            {
                                y.ID,
                                y.IsAgain,
                                y.IsNeed,
                                y.PlID,
                                y.AddTime,
                                y.VisitTime,
                                y.NextTime,
                                y.VisitContents,
                                Img = ImgHelper.GetImgAllPath(y.Img),
                                RemindID = y.Remind != null ? y.Remind.ID : 0,
                            }),
                        }).FirstOrDefault();
                        return Json(new { data = list, state = 1, msg = "请求成功" });
                    }
                }
                return Json(new { state = 0, msg = "请求失败，参数不正确" });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Add([FromBody]RequestModel req)
        {
            try
            {
                //真实的参数
                var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);

                using (dbDataContext db = new dbDataContext())
                {
                    var UserID = Convert.ToInt32(parameters["UserID"]);//用户ID
                    var user = db.Users.FirstOrDefault(x => x.ID == UserID);//用户
                    if (user != null)
                    {
                        PrivateLine pl = new PrivateLine();
                        pl.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//添加时间
                        pl.CompanyAddress = parameters["CompanyAddress"];
                        pl.CompanyName = parameters["CompanyName"];
                        pl.CompanyScale = parameters["CompanyScale"];
                        pl.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//最近更新时间
                        pl.Remark = parameters["Remark"];//备注
                        pl.Users = user;
                        pl.State = Convert.ToInt32(parameters["State"]);
                        #region 联系人
                        //联系人数量   
                        var ContactsNum = Convert.ToInt32(parameters["ContactsNum"]);
                        var Contacts = new List<PlContacts>();//联系人列表
                        var ContactsName = parameters["ContactsName"];//联系人姓名（逗号隔开的多个）
                        var ContactsTel = parameters["ContactsTel"];//联系人电话（逗号隔开的多个）
                        var ContactsPost = parameters["ContactsPost"];//联系人岗位（逗号隔开的多个）
                        //分割后
                        var ContactsNameList = ContactsName.Split(',');
                        var ContactsTelList = ContactsTel.Split(',');
                        var ContactsPostList = ContactsPost.Split(',');
                        //遍历
                        for (int i = 0; i < ContactsNum; i++)
                        {
                            var contact = new PlContacts();
                            contact.Name = ContactsNameList[i];
                            contact.Tel = ContactsTelList[i];
                            contact.Post = ContactsPostList[i];
                            contact.PrivateLine = pl;
                            Contacts.Add(contact);
                        }
                        #endregion
                        #region 专线信息
                        var PlInfoNum = Convert.ToInt32(parameters["PlInfoNum"]);//专线信息数量
                        var PlInfoList = new List<PlInfo>();//专线信息列表
                        //分割前
                        var Type = parameters["Type"];//类型 1专线  2电路 3,天翼云
                        var Operator = parameters["Operator"];//运营商
                        var WeekPrice = parameters["WeekPrice"];//周价
                        var BandWidth = parameters["BandWidth"];//带宽
                        var PayType = parameters["PayType"];//付费方式
                        var OverTime = parameters["OverTime"];//到期时间
                        var ServerBerSys = parameters["ServerBerSys"];//服务器承载系统
                        var ServerUsingTime = parameters["ServerUsingTime"];//现服务器开始使用时间
                        var IsCloudPlan = parameters["IsCloudPlan"];//是否有上云计划
                        //分割后
                        var TypeList = Type.Split(',');//类型
                        var OperatorList = Operator.Split(',');//运营商
                        var WeekPriceList = WeekPrice.Split(',');//周价
                        var BandWidthList = BandWidth.Split(',');//带宽
                        var PayTypeList = PayType.Split(',');// 付费方式
                        var OverTimeList = OverTime.Split(',');//到期时间
                        var ServerBerSysList = ServerBerSys.Split(',');//服务器承载系统
                        var ServerUsingTimeList = ServerUsingTime.Split(','); //现服务器开始使用时间
                        var IsCloudPlanList = IsCloudPlan.Split(',');//是否有上云计划
                        for (int i = 0; i < PlInfoNum; i++)
                        {
                            var plinfo = new PlInfo();
                            plinfo.Type = Convert.ToInt32(TypeList[i]);//类型
                            plinfo.Operator = OperatorList[i];
                            plinfo.WeekPrice = WeekPriceList[i];
                            plinfo.BandWidth = BandWidthList[i];
                            plinfo.PayType = PayTypeList[i];
                            plinfo.OverTime = !string.IsNullOrEmpty(OverTimeList[i]) ? Convert.ToDateTime(OverTimeList[i]).ToString("yyyy-MM-dd") : "";
                            plinfo.ServerBerSys = ServerBerSysList[i];//服务器承载系统
                            plinfo.ServerUsingTime = ServerUsingTimeList[i];
                            plinfo.IsCloudPlan = IsCloudPlanList[i] != "" ? int.Parse(IsCloudPlanList[i]) : 0;//是否有上云计划
                            plinfo.State = 0;
                            plinfo.PrivateLine = pl;
                            PlInfoList.Add(plinfo);
                        }
                        #endregion
                        #region 走访记录
                        var plvisit = new PlVisit();
                        plvisit.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//添加时间
                        plvisit.IsAgain = Convert.ToInt32(parameters["IsAgain"]);
                        plvisit.IsNeed = Convert.ToInt32(parameters["IsNeed"]);
                        plvisit.State = 0;
                        plvisit.VisitTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//本次拜访时间
                        plvisit.NextTime = parameters["NextTime"];
                        plvisit.IsOther = Convert.ToInt32(parameters["IsOther"]);//是否有其他业务
                        plvisit.OtherDesc = parameters["OtherDesc"];//其他业务描述
                        plvisit.VisitContents = parameters["VisitContents"];//
                        plvisit.PrivateLine = pl;
                        #endregion

                        db.PrivateLine.InsertOnSubmit(pl);
                        db.PlContacts.InsertAllOnSubmit(Contacts);
                        db.PlInfo.InsertAllOnSubmit(PlInfoList);
                        db.PlVisit.InsertOnSubmit(plvisit);
                        db.SubmitChanges();

                        return Json(new { state = 1, msg = "添加成功" });
                    }
                    return Json(new { state = 0, msg = "用户不存在" });
                }
            }
            catch (Exception ex)
            {
                //return Json(new { state = 0, msg = "请求失败" });
                throw ex;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Modify([FromBody]RequestModel req)
        {
            try
            {
                //真实的参数
                var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);

                using (dbDataContext db = new dbDataContext())
                {
                    var UserID = Convert.ToInt32(parameters["UserID"]);//用户ID
                    var user = db.Users.FirstOrDefault(x => x.ID == UserID);//用户

                    var PlID = Convert.ToInt32(parameters["PlID"]);//专线ID
                    var pl = db.PrivateLine.Where(x => x.ID == PlID).FirstOrDefault();//专线
                    if (pl != null)
                    {
                        pl.CompanyAddress = parameters["CompanyAddress"];
                        pl.CompanyName = parameters["CompanyName"];
                        pl.CompanyScale = parameters["CompanyScale"];
                        pl.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//最近更新时间
                        pl.Remark = parameters["Remark"];//备注

                        #region 联系人
                        //联系人数量
                        var ContactsNum = Convert.ToInt32(parameters["ContactsNum"]);
                        var Contacts = new List<PlContacts>();//联系人列表
                        var ContactsName = parameters["ContactsName"];//联系人姓名（逗号隔开的多个）
                        var ContactsTel = parameters["ContactsTel"];//联系人电话（逗号隔开的多个）
                        var ContactsPost = parameters["ContactsPost"];//联系人岗位（逗号隔开的多个）
                        //分割后
                        var ContactsNameList = ContactsName.Split(',');
                        var ContactsTelList = ContactsTel.Split(',');
                        var ContactsPostList = ContactsPost.Split(',');
                        //遍历
                        for (int i = 0; i < ContactsNum; i++)
                        {
                            var contact = new PlContacts();
                            contact.Name = ContactsNameList[i];
                            contact.Tel = ContactsTelList[i];
                            contact.Post = ContactsPostList[i];
                            contact.PrivateLine = pl;
                            Contacts.Add(contact);
                        }
                        #endregion
                        #region 专线信息
                        var PlInfoNum = Convert.ToInt32(parameters["PlInfoNum"]);//专线信息数量
                        var PlInfoList = new List<PlInfo>();//专线信息列表(新增)
                        //分割前
                        var PlInfoID = parameters["PlInfoID"];//专线信息ID
                        var Type = parameters["Type"];//类型
                        var Operator = parameters["Operator"];//运营商
                        var WeekPrice = parameters["WeekPrice"];//周价
                        var BandWidth = parameters["BandWidth"];//带宽
                        var PayType = parameters["PayType"];//付费方式
                        var OverTime = parameters["OverTime"];//到期时间
                        var ServerBerSys = parameters["ServerBerSys"];//是否有上云计划
                        var ServerUsingTime = parameters["ServerUsingTime"];//现服务器开始使用时间
                        var IsCloudPlan = parameters["IsCloudPlan"];//是否有上云计划
                        //分割后
                        var PlInfoIDList = PlInfoID.Split(',');//专线信息ID
                        var TypeList = Type.Split(',');//类型
                        var OperatorList = Operator.Split(',');//运营商
                        var WeekPriceList = WeekPrice.Split(',');//周价
                        var BandWidthList = BandWidth.Split(',');//带宽
                        var PayTypeList = PayType.Split(',');// 付费方式
                        var OverTimeList = OverTime.Split(',');//到期时间
                        var ServerBerSysList = ServerBerSys.Split(',');//服务器承载系统
                        var ServerUsingTimeList = ServerUsingTime.Split(','); //现服务器开始使用时间
                        var IsCloudPlanList = IsCloudPlan.Split(',');//是否有上云计划
                        //原有的专线信息ID
                        var oldPlInfoIDList = db.PlInfo.Where(x => x.PlID == pl.ID).Select(x => x.ID.ToString()).ToList();//
                        for (int i = 0; i < PlInfoNum; i++)//遍历最新数据
                        {
                            if (PlInfoIDList[i] == "0")//新增的
                            {
                                var plinfo = new PlInfo();
                                plinfo.Type = Convert.ToInt32(TypeList[i]);//类型
                                plinfo.Operator = OperatorList[i];
                                plinfo.WeekPrice = WeekPriceList[i];
                                plinfo.BandWidth = BandWidthList[i];
                                plinfo.PayType = PayTypeList[i];
                                plinfo.OverTime = !string.IsNullOrEmpty(OverTimeList[i]) ? Convert.ToDateTime(OverTimeList[i]).ToString("yyyy-MM-dd") : "";
                                plinfo.ServerBerSys = ServerBerSysList[i];//服务器承载系统
                                plinfo.ServerUsingTime = ServerUsingTimeList[i];
                                plinfo.IsCloudPlan = IsCloudPlanList[i] != "" ? Convert.ToInt32(IsCloudPlanList[i]) : 0;//是否有上云计划
                                plinfo.State = 0;
                                plinfo.PrivateLine = pl;
                                PlInfoList.Add(plinfo);
                            }
                            else//修改的
                            {
                                //专线信息
                                var plinfo = db.PlInfo.Where(x => x.ID == Convert.ToInt32(PlInfoIDList[i])).FirstOrDefault();
                                plinfo.Type = Convert.ToInt32(TypeList[i]);//类型
                                plinfo.Operator = OperatorList[i];
                                plinfo.WeekPrice = WeekPriceList[i];
                                plinfo.BandWidth = BandWidthList[i];
                                plinfo.PayType = PayTypeList[i];
                                plinfo.OverTime = !string.IsNullOrEmpty(OverTimeList[i]) ? Convert.ToDateTime(OverTimeList[i]).ToString("yyyy-MM-dd") : "";
                                plinfo.ServerBerSys = ServerBerSysList[i];//服务器承载系统
                                plinfo.ServerUsingTime = ServerUsingTimeList[i];
                                plinfo.IsCloudPlan = IsCloudPlanList[i]!=""?Convert.ToInt32(IsCloudPlanList[i]):0;//是否有上云计划
                                #region 状态判断
                                if (plinfo.State == 0)//正常
                                {
                                    //正常修改不处理
                                }
                                else if (plinfo.State == 1)//已提醒
                                {
                                    var plremind = db.PlRemind.Where(x => x.PlID == PlID && x.PlInfoID == Convert.ToInt32(PlInfoIDList[i])).Where(x => x.Type == 1).OrderByDescending(x => x.AddTime).FirstOrDefault();
                                    if (plremind != null)
                                    {
                                        plremind.State = 2;//未处理
                                    }
                                }
                                else if (plinfo.State == 2)//已回执
                                {
                                    plinfo.State = 0;
                                }
                                #endregion
                                #region 删除的
                                ////最新的数据中不包含原有的数据，则删除原有的数据
                                //新旧差集
                                var strID = oldPlInfoIDList.Except(PlInfoIDList);
                                if (strID.Count() > 0)
                                {
                                    foreach (var item in strID)//删除
                                    {
                                        var oldPlInfo = db.PlInfo.Where(x => x.ID == Convert.ToInt32(item)).FirstOrDefault();
                                        if (oldPlInfo != null)
                                        {
                                            var plremind = db.PlRemind.Where(x => x.PlInfoID == oldPlInfo.ID);

                                            db.PlInfo.DeleteOnSubmit(oldPlInfo);//删除旧的专线数据
                                            db.PlRemind.DeleteAllOnSubmit(plremind);//删除原有的的提醒记录
                                        }
                                    }
                                }
                                #endregion
                            }
                        }

                        #endregion
                        db.PlContacts.DeleteAllOnSubmit(db.PlContacts.Where(x => x.PlID == pl.ID));//删除旧的联系人
                        db.PlContacts.InsertAllOnSubmit(Contacts);//添加最新的联系人
                        if (PlInfoList.Count > 0)
                        {
                            db.PlInfo.InsertAllOnSubmit(PlInfoList);//添加新的
                        }

                        db.SubmitChanges();

                        return Json(new { state = 1, msg = "修改成功" });
                    }
                    return Json(new { state = 0, msg = "专线信息不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改专线状态
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ModifyState([FromBody]RequestModel req)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    //真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);

                    var PlID = Convert.ToInt32(parameters["PlID"]);
                    var State = Convert.ToInt32(parameters["State"]);//状态
                    var pl = db.PrivateLine.FirstOrDefault(x => x.ID == PlID);
                    if (pl != null)
                    {
                        pl.State = State;
                        db.SubmitChanges();
                        return Json(new { state = 1, msg = "修改成功" });
                    }

                    return Json(new { state = 0, msg = "专线信息不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 专线名称排重检索
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult CheckPrivateLineName(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    //真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var PlName = parameters["PlName"];
                    if (!string.IsNullOrEmpty(PlName))
                    {
                        var list = db.PrivateLine.Where(x => x.CompanyName.Contains(PlName)).Select(x => x.CompanyName).ToList();
                        if (list.Count > 0)
                        {
                            return Json(new { state = 0, msg = "专线名称重复" });
                        }
                        return Json(new { state = 1, msg = "专线名称可用" });


                    }
                    return Json(new { state = 0, msg = "专线名称为空" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取专线统计
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetHeadData(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    //真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var Areas = parameters["Areas"];//区县
                    var Month = parameters["Month"] ?? "";//月份
                    var Day = parameters["Day"] ?? "";
                    var UserID = Convert.ToInt32(parameters["UserID"]);

                    var user = db.Users.FirstOrDefault(x => x.ID == UserID);
                    if (user != null)
                    {
                        var list = db.PrivateLine.Where(x => 1 == 1);
                        #region 角色判断
                        if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        {
                            list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                        }
                        else
                        if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                        {
                            list = list.Where(x => x.Users.Areas == user.Areas);
                        }
                        else
                        if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                        {
                            list = list.Where(x => x.UserID == UserID);
                        }
                        else
                        {
                            list = list.Where(x => x.ID < 0);//无数据
                        }
                        #endregion
                        if (!string.IsNullOrEmpty(Areas))//区县
                        {
                            list = list.Where(x => x.Users.Areas == Areas);
                        }
                        //当前时间
                        var date = DateTime.Now;
                        //本月专线
                        var MonthList = list.Where(x => Convert.ToDateTime(x.AddTime).Year == date.Year && Convert.ToDateTime(x.AddTime).Month == date.Month);

                        IQueryable<PrivateLine> plList = list;
                        if (!string.IsNullOrEmpty(Month))
                        {
                            plList = list.Where(y => Convert.ToDateTime(y.AddTime).Year == Convert.ToDateTime(Month).Year && Convert.ToDateTime(y.AddTime).Month == Convert.ToDateTime(Month).Month);
                        }
                        if (!string.IsNullOrEmpty(Day))
                        {
                            plList = list.Where(y => Convert.ToDateTime(y.AddTime).Year == Convert.ToDateTime(Day).Year && Convert.ToDateTime(y.AddTime).Month == Convert.ToDateTime(Day).Month && Convert.ToDateTime(y.AddTime).Day == Convert.ToDateTime(Day).Day);
                        }
                        //即将到期

                        var ExpiringList = plList.Where(x => x.PlRemind.Where(y => y.State == 0).OrderByDescending(y => y.AddTime).FirstOrDefault() != null);
                        //落单
                        var OrderList = plList.Where(x => x.State == 1);
                        //放弃
                        var ForgoList = plList.Where(x => x.State == 2);
                        //跟进
                        var FollowList = plList.Where(x => x.State.GetValueOrDefault(0) == 0);
                        //结果
                        var result = new
                        {
                            MonthCount = MonthList.Count() > 0 ? MonthList.Sum(x => x.PlInfo.Count()) : 0,//当月
                            TotalCount = list.Count() > 0 ? list.Sum(x => x.PlInfo.Count()) : 0,//累计
                            ExpiringCount = ExpiringList.Count() > 0 ? ExpiringList.Sum(x => x.PlInfo.Count()) : 0,//即将到期
                            OrderCount = OrderList.Count() > 0 ? OrderList.Sum(x => x.PlInfo.Count()) : 0,//落单
                            ForgoCount = ForgoList.Count() > 0 ? ForgoList.Sum(x => x.PlInfo.Count()) : 0,//放弃
                            FollowCount = FollowList.Count() > 0 ? FollowList.Sum(x => x.PlInfo.Count()) : 0,//跟进
                        };
                        return Json(new { data = result, state = 1, msg = "请求成功" });
                    }


                    return Json(new { state = 0, msg = "请求失败,用户不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
