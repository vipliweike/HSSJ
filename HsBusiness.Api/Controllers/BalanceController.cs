using HsBusiness.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class BalanceController : ApiController
    {
        /// <summary>
        /// 获取小余额列表
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

                    var PageIndex = Convert.ToInt32(parameters["PageIndex"]);//当前页数
                    var PageSize = Convert.ToInt32(parameters["PageSize"]);//数量
                    var UserID = Convert.ToInt32(parameters["UserID"]);//用户ID
                    var Search = parameters["Search"];//搜索
                    var Areas = parameters["Areas"];//区县
                    var user = db.Users.FirstOrDefault(x => x.ID == UserID);
                    if (user != null)
                    {
                        var list = db.SmallBalance.Where(x => 1 == 1);
                        if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        {
                            list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                        }
                        else
                        if (user.Roles.RoleName == "区县经理")//区县经理
                        {
                            list = list.Where(x => x.Users.Areas == user.Areas);//本区县的
                                                                                // list = list.Where(x => x.UserID == UserID);//自己的
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

                        if (!string.IsNullOrEmpty(Areas))//所属区县
                        {
                            list = list.Where(x => x.Users.Areas == Areas);
                        }

                        if (!string.IsNullOrEmpty(Search))
                        {
                            //客户名称、联系人、联系人电话、接入号
                            list = list.Where(x => x.CustomerName.Contains(Search) || x.Contacts.Contains(Search) || x.Contacts.Contains(Search) || x.AccountID.Contains(Search) || x.Users.Name.Contains(Search));
                        }
                        var result = list.Select(x => new
                        {
                            x.ID,
                            x.CustomerName,
                            x.WeekPrice,
                            x.Broadband,
                            x.Balance,
                            x.AddTime,
                            x.Contacts,
                            x.ContactTel,
                            x.State,
                            UserName = x.Users.Name,
                            IsRead = x.SmaBaRead.Where(y => y.UserID == user.ID).FirstOrDefault()
                        }).Select(x => new
                        {
                            x.ID,
                            x.CustomerName,
                            x.WeekPrice,
                            x.Broadband,
                            x.Balance,
                            x.AddTime,
                            x.Contacts,
                            x.ContactTel,
                            x.State,
                            x.UserName,
                            IsRead = x.IsRead == null ? 0 : x.IsRead.IsRead,//为空是未读，不为空按照状态
                        })
                        .OrderBy(x => x.IsRead)//0未读   1已读
                        .ThenBy(x => x.AddTime)
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
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetOne(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    //真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var SmBaID = Convert.ToInt32(parameters["SmBaID"]);
                    var bal = db.SmallBalance.Where(x => x.ID == SmBaID).Select(x => new
                    {
                        x.ID,
                        x.AccessNumber,//接入号
                        x.AccountID,//账号ID
                        x.Balance,//余额
                        x.Broadband,//宽带
                        x.Contacts,//联系人
                        x.ContactTel,//联系人电话                     
                        x.CustomerName,//客户名称
                        x.AddTime,//添加时间
                        x.GroupID,//集团ID
                        x.GroupName,//集团名称
                        x.InstalledAddress,//
                        x.Responsibility,//责任区域
                        x.NetTime,//入网时间
                        x.WeekPrice,//周假 
                        x.State,//状态（1派单中（也就是未回执）  2已回执）
                        UserName = x.Users.Name,
                        RemindID = x.SmaBaRemind.Where(y => y.State == 0).OrderByDescending(y => y.ID).FirstOrDefault() != null ?
                                 x.SmaBaRemind.Where(y => y.State == 0).OrderByDescending(y => y.ID).FirstOrDefault().ID
                                 : 0,//提醒ID
                    }).FirstOrDefault();
                    return Json(new { data = bal, state = 1, msg = "请求成功" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 回执
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Receipt([FromBody]RequestModel req)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);

                    var RemindID = Convert.ToInt32(parameters["RemindID"]);//提醒回执ID                    
                    var sbr = db.SmaBaRemind.Where(x => x.ID == RemindID).FirstOrDefault();
                    if (sbr != null)
                    {
                        sbr.ReceiptContent = parameters["ReceiptContent"];
                        sbr.ReceiptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var smba = db.SmallBalance.Where(x => x.ID == sbr.SmBaID).FirstOrDefault();//小余额                        
                        if (smba != null)
                        {
                            smba.State = 2;//已回执

                            sbr.State = 1;//是否回执
                            db.SubmitChanges();

                            return Json(new { state = 1, msg = "回执成功" });
                        }
                        return Json(new { state = 1, msg = "小余额不存在" });
                    }

                    return Json(new { state = 0, msg = "提醒不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 小余额已读
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Read([FromBody]RequestModel req)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    // 真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);

                    //项目ID
                    var SmBaID = Convert.ToInt32(parameters["SmBaID"]);
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var smba = db.SmallBalance.Where(x => x.ID == SmBaID).FirstOrDefault();
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    if (user != null)
                    {
                        if (smba != null)
                        {
                            var model = db.SmaBaRead.Where(x => x.SmBaID == smba.ID && x.UserID == user.ID).FirstOrDefault();
                            if (model == null)//添加已读记录
                            {
                                SmaBaRead read = new SmaBaRead();
                                read.UserID = user.ID;
                                read.SmBaID = smba.ID;
                                read.IsRead = 1;

                                db.SmaBaRead.InsertOnSubmit(read);
                            }
                            else//修改已读记录
                            {
                                model.IsRead = 1;
                            }

                            db.SubmitChanges();
                            return Json(new { state = 1, msg = "请求成功" });
                        }

                        return Json(new { state = 0, msg = "存量小余额不存在" });
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
