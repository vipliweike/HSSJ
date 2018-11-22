using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.Businesss
{
    public partial class InformationNewly1 : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetOne(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Business.Where(x => x.ID == ID).Select(x => new
                {
                    x.ID,
                    Contacts = x.Contacts.Select(y => new { y.ID, y.Name, y.Post, y.Tel }),//联系人
                    x.CompanyName,//单位名称
                    x.Industry,//行业归属
                    x.CustomerScale,//客户规模
                    x.Areas,//所属区县
                    x.Remark,//备注
                    x.CompanyAddress,//单位地址
                    x.IsHavePhoneList,//是否有员工通讯录

                    x.MCardUse,//手机卡用途
                    x.MPushWork,//可推业务,
                    x.MUnicom,//联通占比
                    x.MMobile,//移动占比
                    x.MTelecom,//电信占比
                    x.MAgeGroup,//员工年龄段
                    x.MIsSubsidy,//是否有员工补贴
                    x.MQuota,//补贴额度
                    x.MRange,//补贴范围
                    x.MPolicy,//使用政策
                    x.MOverTime,//租机到期时间
                    x.MMonthFee,//使用月消费
                    x.MFocus,//使用侧重
                    x.MIncome,//年收入预测
                    x.MOtherWork,//有无再使用其他业务
                                 ///固网
                    x.FPushWork,//可推业务
                    x.FScale,//规模
                    x.FBandWidth,//带宽
                    x.FIsDomain,//是否跨域
                    x.FPreWeekPrice,//预计周价
                    x.FPreInNetMouth,//预计入网月份
                    x.FPreAnlIncome, //预计年收益(FPreAnlIncome)
                    x.FOperator,  //合作运营商(FOperator)
                    x.FUseWork,    //在用业务（FUseWork）
                    x.FUseScale,//在用业务规模
                    x.FUseBandWidth,//在用业务带宽
                    x.FWeekPrice,//周价
                    x.FOverTime,//到期时间
                    x.FAlsAnlIncome,  //友商年收益（FAlsAnlIncome）
                    x.FComputerNum,//电脑台数
                    x.FIsSatisfy, //现有业务是否满意(FIsSatisfy)
                    x.FIsServer,//是否有服务器
                    x.FPlatform,//服务器承载平台
                    x.FOtherWork,//有无在使用其他业务
                    x.IsMove,
                    x.IsFixed,
                   
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(list);
                return result;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [WebMethod]
        public static string Modify(string data, string lxr)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    var model = JsonConvert.DeserializeObject<Business>(data);
                    var lxrs = JsonConvert.DeserializeObject<List<Contacts>>(lxr);
                    var bus = db.Business.FirstOrDefault(x => x.ID == model.ID);
                    if (bus != null)
                    {

                        bus.CompanyAddress = model.CompanyAddress;//单位地址
                        bus.Areas = bus.Users.Areas;
                        bus.IsHavePhoneList = model.IsHavePhoneList;//是否有员工通讯录
                        bus.CompanyName = model.CompanyName;
                        bus.CustomerScale = model.CustomerScale;
                        bus.FAlsAnlIncome = model.FAlsAnlIncome;
                        bus.FBandWidth = model.FBandWidth;
                        bus.FComputerNum = model.FComputerNum;
                        bus.FIsDomain = model.FIsDomain;
                        bus.FIsSatisfy = model.FIsSatisfy;
                        bus.FIsServer = model.FIsServer;
                        bus.FOperator = model.FOperator;
                        bus.FOtherWork = model.FOtherWork;
                        bus.FOverTime = model.FOverTime;
                        bus.FPlatform = model.FPlatform;
                        bus.FPreAnlIncome = model.FPreAnlIncome;
                        bus.FPreInNetMouth = model.FPreInNetMouth;
                        bus.FPreWeekPrice = model.FPreWeekPrice;
                        bus.FPushWork = model.FPushWork;
                        bus.FScale = model.FScale;
                        bus.FUseBandWidth = model.FUseBandWidth;
                        bus.FUseScale = model.FUseScale;
                        bus.FUseWork = model.FUseWork;
                        bus.FWeekPrice = model.FWeekPrice;
                        bus.Industry = model.Industry;
                        bus.IsFixed = model.IsFixed;
                        bus.IsMove = model.IsMove;
                        //等级根据用户规模判断
                        bus.Levels = bus.CustomerScale == "50-100" || bus.CustomerScale == "50以下" ? 3 : bus.CustomerScale == "100-200" ? 2 : bus.CustomerScale == "200以上" ? 1 : 0;
                        bus.MAgeGroup = model.MAgeGroup;
                        bus.MCardUse = model.MCardUse;
                        bus.MFocus = model.MFocus;
                        bus.MIncome = model.MIncome;
                        bus.MIsSubsidy = model.MIsSubsidy;
                        bus.MMonthFee = model.MMonthFee;
                        bus.MOperator = model.MOperator;
                        bus.MOtherWork = model.MOtherWork;
                        bus.MOverTime = model.MOverTime;
                        bus.MPolicy = model.MPolicy;
                        bus.MPushWork = model.MPushWork;
                        bus.MQuota = model.MQuota;
                        bus.MRange = model.MRange;
                        bus.MRatio = model.MRatio;
                        bus.Remark = model.Remark;
                        bus.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        bus.MUnicom = model.MUnicom;//联通占比
                        bus.MMobile = model.MMobile;//移动占比
                        bus.MTelecom = model.MTelecom;//电信占比
                        bus.Remark = model.Remark;//备注

                        #region 状态判断
                        if (bus.MState == 0)//正常
                        {
                            //正常修改不处理
                        }
                        else if (bus.MState == 1)//已派单
                        {
                            bus.MState = 0;
                            var br = bus.BusRemind.Where(x => x.Type == 1).OrderByDescending(x => x.AddTime).FirstOrDefault();//
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



                        var Contacts = db.Contacts.Where(x => x.BusID == model.ID).ToList();

                        for (int i = 0; i < lxrs.Count; i++)
                        {
                            lxrs[i].BusID = bus.ID;
                        }
                        db.Contacts.DeleteAllOnSubmit(Contacts);
                        db.Contacts.InsertAllOnSubmit(lxrs);

                        db.SubmitChanges();
                        var result = JsonConvert.SerializeObject(new { state = 1, msg = "修改成功" });
                        return result;
                    }
                    return JsonConvert.SerializeObject(new { state = 0, msg = "修改失败" });
                }
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { state = 0, msg = "修改失败" });
            }

        }

        [WebMethod]
        public static string Add(string data, string lxr, string vis)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    var model = JsonConvert.DeserializeObject<Business>(data);
                    var lxrs = JsonConvert.DeserializeObject<List<Contacts>>(lxr);
                    var vism = JsonConvert.DeserializeObject<Visit>(vis);
                    if (!string.IsNullOrEmpty(Common.TCContext.Current.OnlineUserID))
                    {
                        var user = db.Users.FirstOrDefault(x => x.ID == Convert.ToInt32(Common.TCContext.Current.OnlineUserID));
                        var bus = new Business();
                        bus.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        bus.Areas = user.Areas;
                        bus.CompanyAddress = model.CompanyAddress;//单位地址
                        bus.IsHavePhoneList = model.IsHavePhoneList;//是否有员工通讯录
                        bus.CompanyName = model.CompanyName;
                        bus.CustomerScale = model.CustomerScale;
                        bus.FAlsAnlIncome = model.FAlsAnlIncome;
                        bus.FBandWidth = model.FBandWidth;
                        bus.FComputerNum = model.FComputerNum;
                        bus.FIsDomain = model.FIsDomain;
                        bus.FIsSatisfy = model.FIsSatisfy;
                        bus.FIsServer = model.FIsServer;
                        bus.FOperator = model.FOperator;
                        bus.FOtherWork = model.FOtherWork;
                        bus.FOverTime = model.FOverTime;
                        bus.FPlatform = model.FPlatform;
                        bus.FPreAnlIncome = model.FPreAnlIncome;
                        bus.FPreInNetMouth = model.FPreInNetMouth;
                        bus.FPreWeekPrice = model.FPreWeekPrice;
                        bus.FPushWork = model.FPushWork;
                        bus.FScale = model.FScale;
                        bus.FUseBandWidth = model.FUseBandWidth;
                        bus.FUseScale = model.FUseScale;
                        bus.FUseWork = model.FUseWork;
                        bus.FWeekPrice = model.FWeekPrice;
                        bus.Industry = model.Industry;
                        bus.IsFixed = model.IsFixed;
                        bus.IsMove = model.IsMove;
                        //等级根据用户规模判断
                        bus.Levels = bus.CustomerScale == "50-100" || bus.CustomerScale == "50以下" ? 3 : bus.CustomerScale == "100-200" ? 2 : bus.CustomerScale == "200以上" ? 1 : 0;

                        bus.MAgeGroup = model.MAgeGroup;
                        bus.MCardUse = model.MCardUse;
                        bus.MFocus = model.MFocus;
                        bus.MIncome = model.MIncome;
                        bus.MIsSubsidy = model.MIsSubsidy;
                        bus.MMonthFee = model.MMonthFee;
                        bus.MOperator = model.MOperator;
                        bus.MOtherWork = model.MOtherWork;
                        bus.MOverTime = model.MOverTime;
                        bus.MPolicy = model.MPolicy;
                        bus.MPushWork = model.MPushWork;
                        bus.MQuota = model.MQuota;
                        bus.MRange = model.MRange;
                        bus.MRatio = model.MRatio;
                        bus.Remark = model.Remark;
                        bus.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        bus.MUnicom = model.MUnicom;//联通占比
                        bus.MMobile = model.MMobile;//移动占比
                        bus.MTelecom = model.MTelecom;//电信占比
                        bus.MState = 0;
                        bus.FState = 0;
                        bus.IsRead = 0;
                        bus.Remark = model.Remark;//备注
                        bus.Users = user;


                        for (int i = 0; i < lxrs.Count; i++)//联系人关联
                        {
                            lxrs[i].Business = bus;
                        }

                        vism.Business = bus;//走访关联
                        vism.State = 0;
                        vism.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//添加时间
                        vism.VisitTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//走访时间
                        vism.Users = user;


                        db.Business.InsertOnSubmit(bus);
                        db.Contacts.InsertAllOnSubmit(lxrs);
                        db.Visit.InsertOnSubmit(vism);

                        db.SubmitChanges();
                        var result = JsonConvert.SerializeObject(new { state = 1, msg = "添加成功" });
                        return result;
                    }
                    return JsonConvert.SerializeObject(new { state = 0, msg = "添加失败，请重新登录" });
                }
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { state = 0, msg = "添加失败" });
            }
        }







        /// <summary>
        /// 得到区县
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetRegion1()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Region.Where(x => x.Pid == 0).Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Region.Where(x => x.Pid == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        [WebMethod]
        /// <summary>
        /// 得到行业归属
        /// </summary>
        /// <returns></returns>
        public static string GetHYGS(string name)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == name).Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name }).ToList();
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        [WebMethod]
        /// <summary>
        /// 得到用户规模
        /// </summary>
        /// <returns></returns>
        public static string GetYHGM()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "用户规模").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到角色信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetRole()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Roles.Where(x => 1 == 1).Select(x => new { x.RoleName });
                return new JavaScriptSerializer().Serialize(list);
            }
        }
        [WebMethod]
        /// <summary>
        /// 得到手机卡用途
        /// </summary>
        /// <returns></returns>
        public static string GetMUser()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "手机卡用途").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        [WebMethod]
        /// <summary>
        /// 得到移动可推业务
        /// </summary>
        /// <returns></returns>
        public static string GetMKTYW()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "移动可推业务").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        [WebMethod]
        /// <summary>
        /// 得到员工年龄段
        /// </summary>
        /// <returns></returns>
        public static string GetYGNL()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "员工年龄段").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到补贴范围
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetBTFW()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "补贴范围").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到使用政策
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetSYZC()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "使用政策").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 使用月消费
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetYXF()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "使用月消费").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到使用侧重
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetSYCZ()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "使用侧重").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到年收入预测
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetNSRYC()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "年收入预测").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到有无在用其他业务(移动其他业务)
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetOtherYW()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "移动其他业务").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到固网可推业务
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetFYW()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "固网可推业务").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到固话规模
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetFGM()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "固话规模").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到专线规模
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetZXGM()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "专线规模").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到电路规模
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetDLGM()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "电路规模").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到带宽规模
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetDKGM()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "宽带规模").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到天翼云规模
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetTYYGM()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "天翼云规模").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到合作运营商（固网）
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetFYYS()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "合作运营商").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 固网再用业务
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetFUserYW()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "固网在用业务").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        /// <summary>
        /// 得到固网其他业务
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetFOthorYW()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == "固网其他业务").Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list1);
            }
        }
        [WebMethod]
        public static string GetDic(string DicName)
        {
            string[] nullList = { };//空数组
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Dic.Where(x => x.Name == DicName).Select(x => new { x.ID }).FirstOrDefault();
                if (list != null)
                {
                    var list1 = db.Dic.Where(x => x.PID == list.ID).Select(x => new { x.ID, x.Name }).ToList();
                    return new JavaScriptSerializer().Serialize(list1);
                }
                return new JavaScriptSerializer().Serialize(nullList);
            }
        }

    }
}