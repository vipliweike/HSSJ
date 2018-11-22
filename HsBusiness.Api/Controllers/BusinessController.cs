using HsBusiness.Api.Common;
using HsBusiness.Api.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.SqlServer;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace HsBusiness.Api.Controllers
{
    public class BusinessController : ApiController
    {
        /// <summary>
        /// 获取单个商机信息
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
                var ID = Convert.ToInt32(parameters["ID"]);
                string[] imgNUll = { };//空数组

                if (ID != 0)
                {
                    using (dbDataContext db = new dbDataContext())
                    {
                        var list = db.Business.Where(x => x.ID == ID)
                            .OrderByDescending(x => x.LastTime)
                            .Select(x => new
                            {
                                x.ID,
                                x.Industry,
                                x.IsFixed,
                                x.IsMove,
                                x.Areas,
                                x.Levels,//等级
                                x.AddTime,
                                x.LastTime,//最后更新时间
                                x.CompanyName,
                                x.CustomerScale,
                                x.CompanyAddress,
                                x.IsHavePhoneList,
                                //固网                                
                                x.FAlsAnlIncome,
                                x.FBandWidth,
                                x.FComputerNum,
                                x.FIsDomain,
                                x.FIsSatisfy,
                                x.FIsServer,
                                x.FOperator,
                                x.FOtherWork,
                                x.FOverTime,
                                x.FPlatform,
                                x.FPreAnlIncome,
                                x.FPreInNetMouth,
                                x.FPreWeekPrice,
                                x.FPushWork,
                                x.FScale,
                                x.FUseBandWidth,
                                x.FUseScale,
                                x.FUseWork,
                                x.FWeekPrice,
                                //移动
                                x.MAgeGroup,
                                x.MCardUse,
                                x.MFocus,
                                x.MIncome,
                                x.MIsSubsidy,
                                x.MMonthFee,
                                //x.MOperator,
                                x.MOtherWork,
                                x.MOverTime,
                                x.MPolicy,
                                x.MPushWork,
                                x.MQuota,
                                x.MRange,
                                //x.MRatio,
                                x.MMobile,//移动占比
                                x.MUnicom,//联通占比
                                x.MTelecom,//电信占比
                                x.Remark,//备注
                                ContactsNum = x.Contacts.Count,//联系人数                            
                                Contacts = x.Contacts.Select(y => new { ContactName = y.Name, ContactTel = y.Tel, ContactPost = y.Post, }),//联系人
                                VisitNum = x.Visit.Count,//回访数
                                //回访记录
                                VisitList = x.Visit.OrderByDescending(y => y.AddTime).ThenByDescending(y => y.ID).Select(q => new
                                {
                                    q.ID,
                                    q.IsAgain,
                                    q.IsNeed,
                                    q.NextTime,
                                    q.IsLeader,
                                    q.Leader,
                                    q.VisitTime,
                                    q.VisitContent,
                                    //Img = q.Img != null ?  q.Img.Split(';') : imgNUll,
                                    Img = ImgHelper.GetImgAllPath(q.Img),
                                    VisitRemindID = q.BusRemind.Where(t => t.State == 0).OrderByDescending(t => t.ID).Select(t => new { RemindID = t.ID, t.AddTime, }).FirstOrDefault() != null ?
                                                      q.BusRemind.Where(t => t.State == 0).OrderByDescending(t => t.ID).Select(t => new { RemindID = t.ID, t.AddTime, }).FirstOrDefault().RemindID
                                                      : 0,//走访提醒
                                }),
                                //移动提醒
                                MRemindID = x.BusRemind.Where(y => y.Type == 1 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault() != null ?
                                            x.BusRemind.Where(y => y.Type == 1 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault().RemindID
                                            : 0,
                                //固网提醒
                                FRemindID = x.BusRemind.Where(y => y.Type == 2 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault() != null ?
                                            x.BusRemind.Where(y => y.Type == 2 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault().RemindID
                                            : 0,

                            }).FirstOrDefault();
                        return Json(new { data = list, state = 1, msg = "请求成功" });
                    }
                }
                return Json(new { state = 0, msg = "请求失败，参数不正确" });
            }
            catch (Exception ex)
            {
                //return Json(new { state = 0, msg = "请求失败" });
                throw ex;
            }

        }

        /// <summary>
        /// 获取固网业务
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFixed(string data, string secret)
        {
            try
            {
                //真实的参数
                var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                var ID = Convert.ToInt32(parameters["ID"]);
                using (dbDataContext db = new dbDataContext())
                {

                    var list = db.Business.Where(x => x.ID == ID).Select(x => new
                    {
                        x.ID,
                        x.IsFixed,
                        //固网
                        x.FAlsAnlIncome,
                        x.FBandWidth,
                        x.FComputerNum,
                        x.FIsDomain,
                        x.FIsSatisfy,
                        x.FIsServer,
                        x.FOperator,
                        x.FOtherWork,
                        x.FOverTime,
                        x.FPlatform,
                        x.FPreAnlIncome,
                        x.FPreInNetMouth,
                        x.FPreWeekPrice,
                        x.FPushWork,
                        x.FScale,
                        x.FUseBandWidth,
                        x.FUseScale,
                        x.FUseWork,
                        x.FWeekPrice,
                    }).FirstOrDefault();
                    return Json(new { data = list, state = 1, msg = "请求成功" });
                }
            }
            catch (Exception)
            {
                return Json(new { state = 0, msg = "请求失败" });
            }
        }

        /// <summary>
        /// 获取移动业务
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMove(string data, string secret)
        {
            try
            {
                //真实的参数
                var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                var ID = Convert.ToInt32(parameters["ID"]);
                using (dbDataContext db = new dbDataContext())
                {

                    var list = db.Business.Where(x => x.ID == ID).Select(x => new
                    {
                        x.ID,
                        x.IsMove,
                        //移动
                        x.MAgeGroup,
                        x.MCardUse,
                        x.MFocus,
                        x.MIncome,
                        x.MIsSubsidy,
                        x.MMonthFee,
                        x.MOperator,
                        x.MOtherWork,
                        x.MOverTime,
                        x.MPolicy,
                        x.MPushWork,
                        x.MQuota,
                        x.MRange,
                        x.MRatio,
                    }).FirstOrDefault();
                    return Json(new { data = list, state = 1, msg = "请求成功" });
                }
            }
            catch (Exception)
            {
                return Json(new { state = 0, msg = "请求失败" });
            }
        }

        /// <summary>
        /// 获取商机列表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(string data, string secret)
        {
            try
            {
                //真实的参数
                var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                var PageIndex = Convert.ToInt32(parameters["PageIndex"]);//当前页数
                var PageSize = Convert.ToInt32(parameters["PageSize"]);//数量
                var UserID = Convert.ToInt32(parameters["UserID"]);//用户ID
                var Levels = parameters["Levels"] != "" ? Convert.ToInt32(parameters["Levels"]) : 0;//等级
                //var Levels = strLevels == "重要" ? 1 : strLevels == "一般" ? 2 : strLevels == "轻缓" ? 3 : 0;
                var Areas = parameters["Areas"];//所属区县
                var Industry = parameters["Industry"];// 行业区县
                var Month = parameters["Month"];//月份
                var Search = parameters["Search"];//搜索

                using (dbDataContext db = new dbDataContext())
                {
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    var list = db.Business.Where(x => 1 == 1);
                    if (user != null)
                    {
                        if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        {
                            list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                        }
                        else
                        if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                        {
                            list = list.Where(x => x.Areas == user.Areas);
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
                        #region 查询
                        if (Levels != 0)//等级查询
                        {
                            list = list.Where(x => x.Levels == Levels);
                        }
                        if (!string.IsNullOrEmpty(Industry))//行业归属查询
                        {
                            list = list.Where(x => x.Industry.Contains(Industry));
                        }
                        if (!string.IsNullOrEmpty(Areas))//区县查询
                        {
                            list = list.Where(x => x.Areas.Contains(Areas));
                        }
                        //新增（2018年6月26日16:30:19）
                        if (!string.IsNullOrEmpty(Month))//月份查询
                        {
                            list = list.Where(x => Convert.ToDateTime(x.AddTime).Year == Convert.ToDateTime(Month).Year && Convert.ToDateTime(x.AddTime).Month == Convert.ToDateTime(Month).Month);
                        }
                        if (!string.IsNullOrEmpty(Search))
                        {
                            list = list.Where(x => x.CompanyName.Contains(Search) || x.CompanyAddress.Contains(Search) || x.Users.Name.Contains(Search));
                        }
                        #endregion
                        var stateNum = "1,0,2".Split(',').Select(v => Convert.ToInt32(v)).ToArray();//状态数组
                        var resultList = list
                             .Select(x => new
                             {
                                 x.ID,//商机ID
                                 x.UserID,//用户ID                                 
                                 UsersName = x.Users.Name,//用户名称
                                 x.Levels,//等级
                                 x.AddTime,//添加时间
                                 x.CompanyName,//客户名称
                                 x.LastTime,//最后更新时间
                                 //RemindID= x.BusRemind.OrderByDescending(y => y.AddTime).FirstOrDefault()!=null? x.BusRemind.OrderByDescending(y => y.AddTime).FirstOrDefault().ID:0,
                                 IsRead = 1,
                                 //到期提醒内容
                                 Next = x.BusRemind.Where(y => y.State == 0).OrderByDescending(y => y.AddTime).FirstOrDefault().Type == 1 && x.MState == 1 ? "移动到期"
                                    : x.BusRemind.Where(y => y.State == 0).OrderByDescending(y => y.AddTime).FirstOrDefault().Type == 2 && x.FState == 1 ? "固网到期"
                                    : x.BusRemind.Where(y => y.State == 0).OrderByDescending(y => y.AddTime).FirstOrDefault().Type == 3 && x.Visit.OrderByDescending(y => y.AddTime).FirstOrDefault().State == 1 ? "拜访预约" : "",//提醒内容
                             })
                             .OrderByDescending(x => x.Next)//先                            
                            .ThenByDescending(x => x.LastTime)//再根据最近更新时间排序
                            .Skip((PageIndex > 0 ? PageIndex - 1 : 0) * PageSize).Take(PageSize)//分页
                            .ToList();


                        return Json(new { data = resultList, state = 1, msg = "请求成功" });
                    }
                    return Json(new { state = 0, msg = "请求失败，用户不存在" });
                }
            }
            catch (Exception ex)
            {
                //return Json(new { state = 0, msg = "请求失败" });
                throw ex;
            }
        }

        /// <summary>
        /// 商机添加
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
                    int UserID = Convert.ToInt32(parameters["UserID"]);
                    var user = db.Users.FirstOrDefault(x => x.ID == UserID);
                    if (user != null)
                    {
                        Business bus = new Business();
                        bus.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        bus.Areas = user.Areas;
                        bus.CompanyName = parameters["CompanyName"];
                        bus.CompanyAddress = parameters["CompanyAddress"];//单位地址
                        bus.IsHavePhoneList = Convert.ToInt32(parameters["IsHavePhoneList"]);//是否有员工通讯录
                        #region 联系人
                        //联系人数量
                        var ContactsNum = Convert.ToInt32(parameters["ContactsNum"]);
                        var Contacts = new List<Contacts>();//联系人列表
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
                            var contact = new Contacts();
                            contact.Name = ContactsNameList[i];
                            contact.Tel = ContactsTelList[i];
                            contact.Post = ContactsPostList[i];
                            contact.Business = bus;
                            Contacts.Add(contact);
                        }
                        #endregion
                        bus.CustomerScale = parameters["CustomerScale"];
                        bus.FAlsAnlIncome = parameters["FAlsAnlIncome"];
                        bus.FBandWidth = parameters["FBandWidth"];
                        bus.FComputerNum = parameters["FComputerNum"];
                        bus.FIsDomain = Convert.ToInt32(parameters["FIsDomain"]);
                        bus.FIsSatisfy = parameters["FIsSatisfy"];
                        bus.FIsServer = Convert.ToInt32(parameters["FIsServer"]);
                        bus.FOperator = parameters["FOperator"];
                        bus.FOtherWork = parameters["FOtherWork"];
                        bus.FOverTime = parameters["FOverTime"];
                        bus.FPlatform = parameters["FPlatform"];
                        bus.FPreAnlIncome = parameters["FPreAnlIncome"];
                        bus.FPreInNetMouth = parameters["FPreInNetMouth"];
                        bus.FPreWeekPrice = parameters["FPreWeekPrice"];
                        bus.FPushWork = parameters["FPushWork"];
                        bus.FScale = parameters["FScale"];
                        bus.FUseBandWidth = parameters["FUseBandWidth"];
                        bus.FUseScale = parameters["FUseScale"];
                        bus.FUseWork = parameters["FUseWork"];
                        bus.FWeekPrice = parameters["FWeekPrice"];
                        bus.Industry = parameters["Industry"];
                        bus.IsFixed = Convert.ToInt32(parameters["IsFixed"]) == 1 || parameters["IsFixed"] == "true" ? true : false;
                        bus.IsMove = Convert.ToInt32(parameters["IsMove"]) == 1 || parameters["IsMove"] == "true" ? true : false;
                        //等级根据用户规模判断
                        bus.Levels = bus.CustomerScale == "50-100" || bus.CustomerScale == "50以下" ? 3 : bus.CustomerScale == "100-200" ? 2 : bus.CustomerScale == "200以上" ? 1 : 0;
                        bus.MAgeGroup = parameters["MAgeGroup"];
                        bus.MCardUse = parameters["MCardUse"];
                        bus.MFocus = parameters["MFocus"];
                        bus.MIncome = parameters["MIncome"];
                        bus.MIsSubsidy = parameters["MIsSubsidy"];
                        bus.MMonthFee = parameters["MMonthFee"];
                        bus.MOperator = parameters["MOperator"];
                        bus.MOtherWork = parameters["MOtherWork"];
                        bus.MOverTime = parameters["MOverTime"];
                        bus.MPolicy = parameters["MPolicy"];
                        bus.MPushWork = parameters["MPushWork"];
                        bus.MQuota = parameters["MQuota"];
                        bus.MRange = parameters["MRange"];
                        bus.MRatio = parameters["MRatio"];
                        bus.Remark = parameters["Remark"];
                        bus.UserID = Convert.ToInt32(parameters["UserID"]);//所属用户
                        bus.MState = 0;
                        bus.FState = 0;
                        bus.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//最后修改时间
                        bus.IsRead = 1;//已读

                        bus.MUnicom = parameters["MUnicom"];//联通占比
                        bus.MMobile = parameters["MMobile"];//移动占比
                        bus.MTelecom = parameters["MTelecom"];//电信占比

                        bus.Remark = parameters["Remark"];//备注

                        #region 回访记录

                        var visit = new Visit();//回访记录
                        visit.IsNeed = Convert.ToInt32(parameters["IsNeed"]);
                        visit.IsAgain = Convert.ToInt32(parameters["IsAgain"]);
                        visit.NextTime = parameters["NextTime"];
                        visit.State = 0;
                        visit.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        visit.VisitTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//parameters["VisitTime"];
                        //visit.VisitContent = parameters["VisitContent"];
                        visit.IsLeader = Convert.ToInt32(parameters["IsLeader"]);
                        visit.Leader = parameters["Leader"];
                        visit.Business = bus;
                        visit.UserID = user.ID;
                        #endregion

                        db.Business.InsertOnSubmit(bus);//商机添加
                        db.Contacts.InsertAllOnSubmit(Contacts);//联系人添加
                        db.Visit.InsertOnSubmit(visit);//回访记录添加


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
        /// 商机修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Modify([FromBody] RequestModel req)
        {
            try
            {
                //真实的参数
                var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);

                using (dbDataContext db = new dbDataContext())
                {
                    int ID = Convert.ToInt32(parameters["ID"]);//商机ID                    
                    Business bus = db.Business.FirstOrDefault(x => x.ID == ID);
                    if (bus != null)
                    {
                        #region 联系人
                        var ContactsNum = Convert.ToInt32(parameters["ContactsNum"]);
                        var Contacts = new List<Contacts>();//联系人列表
                        var ContactsName = parameters["ContactsName"];//联系人姓名（逗号隔开的多个）
                        var ContactsTel = parameters["ContactsTel"];//联系人电话（逗号隔开的多个）
                        var ContactsPost = parameters["ContactsPost"];//联系人岗位（逗号隔开的多个）

                        var ContactsNameList = ContactsName.Split(',');
                        var ContactsTelList = ContactsTel.Split(',');
                        var ContactsPostList = ContactsPost.Split(',');

                        for (int i = 0; i < ContactsNum; i++)
                        {
                            var contact = new Contacts();
                            contact.Name = ContactsNameList[i];
                            contact.Tel = ContactsTelList[i];
                            contact.Post = ContactsPostList[i];
                            contact.Business = bus;
                            Contacts.Add(contact);
                        }
                        #endregion
                        bus.CompanyAddress = parameters["CompanyAddress"];//单位地址
                        bus.IsHavePhoneList = Convert.ToInt32(parameters["IsHavePhoneList"]);//是否有员工通讯录
                        bus.CompanyName = parameters["CompanyName"];
                        bus.CustomerScale = parameters["CustomerScale"];
                        bus.FAlsAnlIncome = parameters["FAlsAnlIncome"];
                        bus.FBandWidth = parameters["FBandWidth"];
                        bus.FComputerNum = parameters["FComputerNum"];
                        bus.FIsDomain = Convert.ToInt32(parameters["FIsDomain"]);
                        bus.FIsSatisfy = parameters["FIsSatisfy"];
                        bus.FIsServer = Convert.ToInt32(parameters["FIsServer"]);
                        bus.FOperator = parameters["FOperator"];
                        bus.FOtherWork = parameters["FOtherWork"];
                        bus.FOverTime = parameters["FOverTime"];
                        bus.FPlatform = parameters["FPlatform"];
                        bus.FPreAnlIncome = parameters["FPreAnlIncome"];
                        bus.FPreInNetMouth = parameters["FPreInNetMouth"];
                        bus.FPreWeekPrice = parameters["FPreWeekPrice"];
                        bus.FPushWork = parameters["FPushWork"];
                        bus.FScale = parameters["FScale"];
                        bus.FUseBandWidth = parameters["FUseBandWidth"];
                        bus.FUseScale = parameters["FUseScale"];
                        bus.FUseWork = parameters["FUseWork"];
                        bus.FWeekPrice = parameters["FWeekPrice"];
                        bus.Industry = parameters["Industry"];
                        bus.IsFixed = Convert.ToInt32(parameters["IsFixed"]) == 1 || parameters["IsFixed"] == "true" ? true : false;
                        bus.IsMove = Convert.ToInt32(parameters["IsMove"]) == 1 || parameters["IsMove"] == "true" ? true : false;
                        //等级根据用户规模判断
                        bus.Levels = bus.CustomerScale == "50-100" || bus.CustomerScale == "50以下" ? 3 : bus.CustomerScale == "100-200" ? 2 : bus.CustomerScale == "200以上" ? 1 : 0;
                        bus.MAgeGroup = parameters["MAgeGroup"];
                        bus.MCardUse = parameters["MCardUse"];
                        bus.MFocus = parameters["MFocus"];
                        bus.MIncome = parameters["MIncome"];
                        bus.MIsSubsidy = parameters["MIsSubsidy"];
                        bus.MMonthFee = parameters["MMonthFee"];
                        bus.MOperator = parameters["MOperator"];
                        bus.MOtherWork = parameters["MOtherWork"];
                        bus.MOverTime = parameters["MOverTime"];
                        bus.MPolicy = parameters["MPolicy"];
                        bus.MPushWork = parameters["MPushWork"];
                        bus.MQuota = parameters["MQuota"];
                        bus.MRange = parameters["MRange"];
                        bus.MRatio = parameters["MRatio"];
                        bus.Remark = parameters["Remark"];
                        bus.UserID = Convert.ToInt32(parameters["UserID"]);//所属用户
                        bus.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        bus.MUnicom = parameters["MUnicom"];//联通占比
                        bus.MMobile = parameters["MMobile"];//移动占比
                        bus.MTelecom = parameters["MTelecom"];//电信占比


                        bus.Remark = parameters["Remark"];//备注
                        #region 状态判断
                        if (bus.MState == 0)//正常
                        {
                            //正常修改不处理
                        }
                        else if (bus.MState == 1)//已派单
                        {
                            bus.MState = 0;
                            var br = bus.BusRemind.Where(x => x.Type == 1).OrderByDescending(x => x.AddTime).FirstOrDefault();//最新提醒
                            if (br != null)
                            {
                                br.State = 2;//未处理
                            }
                        }
                        else if (bus.MState == 2)//已回执
                        {
                            bus.MState = 0;//正常
                        }
                        #endregion

                        //删除商机旧联系人
                        db.Contacts.DeleteAllOnSubmit(db.Contacts.Where(x => x.BusID == bus.ID).ToList());
                        //联系人添加
                        db.Contacts.InsertAllOnSubmit(Contacts);

                        //提交
                        db.SubmitChanges();
                        return Json(new { state = 1, msg = "修改成功" });
                    }
                    return Json(new { state = 0, msg = "商机信息不存在" });
                }
            }
            catch (Exception ex)
            {
                //return Json(new { state = 0, msg = "请求失败" });
                throw ex;
            }
        }


        /// <summary>
        /// 单位名称排重检索
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult CheckCompanyName(string data, string secret)//string data, string secret
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    //真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var CompanyName = parameters["CompanyName"];
                    if (!string.IsNullOrEmpty(CompanyName))
                    {
                        var list = db.Business.Where(x => x.CompanyName.Contains(CompanyName)).Select(x => x.CompanyName).ToList();
                        if (list.Count > 0)
                        {
                            return Json(new { state = 0, msg = "单位名称重复" });
                        }
                        //List<decimal>  declsit = new List<decimal>();
                        //Common.IAnalyser analyser = new Common.SimHashAnalyser();

                        //foreach (var item in list)
                        //{
                        //    var xsd = Common.CheckSimilarity.GetSimilarityWith(item, CompanyName);
                        //    declsit.Add(xsd);                                                 
                        //}
                        //return Json(new { data = new { decimalList = declsit}, state = 0, msg = "单位名称重复" });
                        return Json(new { state = 1, msg = "单位名称可用" });


                    }
                    return Json(new { state = 0, msg = "单位名称为空" });
                }
            }
            catch (Exception ex)
            {
                //return Json(new { state = 0, msg = "请求失败" });
                throw ex;
            }
        }

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IHttpActionResult Read([FromBody]Models.RequestModel req)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);
                    var BusID = Convert.ToInt32(parameters["BusID"]);//商机提醒ID

                    var bus = db.Business.Where(x => x.ID == BusID).FirstOrDefault();
                    if (bus != null)
                    {
                        bus.IsRead = 1;//已读

                        db.SubmitChanges();

                        return Json(new { stata = 1, msg = "请求成功" });
                    }

                    return Json(new { stata = 0, msg = "商机不存在" });
                }
            }
            catch (Exception ex)
            {
                //return Json(new { state = 0, msg = "请求失败" });
                throw ex;
            }

        }

    }
}
