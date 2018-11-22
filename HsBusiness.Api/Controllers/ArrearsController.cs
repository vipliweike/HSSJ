using HsBusiness.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{

    public class ArrearsController : ApiController
    {
        /// <summary>
        /// 获取列表
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
                        var arrList = db.Arrears.Where(x => 1 == 1);
                        if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        {
                            arrList = arrList.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                        }
                        else
                        if (user.Roles.RoleName == "区县经理")//区县经理
                        {
                            arrList = arrList.Where(x => x.Users.Areas == user.Areas);//本区县的
                        }
                        else
                        if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                        {
                            arrList = arrList.Where(x => x.UserID == UserID);
                        }
                        else
                        {
                            arrList = arrList.Where(x => x.ID < 0);//无数据
                        }

                        if (!string.IsNullOrEmpty(Areas))//所属区县
                        {
                            arrList = arrList.Where(x => x.Area == Areas);
                        }

                        if (!string.IsNullOrEmpty(Search))
                        {
                            //客户名称、维系人姓名
                            arrList = arrList.Where(x => x.CustomerName.Contains(Search) || x.Users.Name.Contains(Search));
                        }
                        var arrResult = arrList.Select(x => new
                        {
                            x.ID,
                            x.CustomerName,
                            x.AmountOwed,
                            x.Area,
                            x.ContactTel,
                            x.InNetTime,
                            x.InstalledAddress,
                            x.Payment,
                            x.Period,
                            x.ServiceStatus,
                            x.UserNumber,
                            x.UserTypeItem,
                            x.AddTime,
                            x.State,
                            UserName = x.Users.Name,
                            IsRead = x.ArrRead.Where(y => y.UserID == user.ID).FirstOrDefault()
                        }).Select(x => new
                        {
                            x.ID,
                            x.CustomerName,
                            x.AmountOwed,
                            x.Area,
                            x.ContactTel,
                            x.InNetTime,
                            x.InstalledAddress,
                            x.Payment,
                            x.Period,
                            x.ServiceStatus,
                            x.UserNumber,
                            x.UserTypeItem,
                            x.AddTime,
                            x.State,
                            x.UserName,
                            IsRead = x.IsRead == null ? 0 : x.IsRead.IsRead,//为空是未读，不为空按照状态
                        })
                        .OrderBy(x => x.IsRead)//0未读   1已读
                        .ThenBy(x => x.AddTime)
                        .Skip((PageIndex > 0 ? PageIndex - 1 : 0) * PageSize).Take(PageSize)//分页
                        .ToList();
                        return Json(new { data = arrResult, state = 1, msg = "请求成功" });
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

                    var ArrID = Convert.ToInt32(parameters["ArrID"]);
                    var bal = db.Arrears.Where(x => x.ID == ArrID).Select(x => new
                    {
                        x.ID,
                        x.CustomerName,
                        x.AmountOwed,
                        x.Area,
                        x.ContactTel,
                        x.InNetTime,
                        x.InstalledAddress,
                        x.Payment,
                        x.Period,
                        x.ServiceStatus,
                        x.UserNumber,
                        x.UserTypeItem,
                        x.AddTime,
                        x.State,//状态（1派单中（也就是未回执）  2已回执）
                        UserName = x.Users.Name,
                        RemindID = x.ArrRemind.Where(y => y.State == 0).OrderByDescending(y => y.ID).FirstOrDefault() != null ?
                                 x.ArrRemind.Where(y => y.State == 0).OrderByDescending(y => y.ID).FirstOrDefault().ID
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
                    var rem = db.ArrRemind.Where(x => x.ID == RemindID).FirstOrDefault();
                    if (rem != null)
                    {
                        rem.ReceiptContent = parameters["ReceiptContent"];
                        rem.ReceiptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var arr = db.Arrears.Where(x => x.ID == rem.ArrID).FirstOrDefault();//小余额
                        if (arr != null)
                        {
                            arr.State = 2;//已回执

                            rem.State = 1;//是否回执
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
                    var ArrID = Convert.ToInt32(parameters["ArrID"]);
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var arr = db.Arrears.Where(x => x.ID == ArrID).FirstOrDefault();
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    if (user != null)
                    {
                        if (arr != null)
                        {
                            var model = db.ArrRead.Where(x => x.ArrID == arr.ID && x.UserID == user.ID).FirstOrDefault();
                            if (model == null)//添加已读记录
                            {
                                ArrRead read = new ArrRead();
                                read.UserID = user.ID;
                                read.ArrID = arr.ID;
                                read.IsRead = 1;

                                db.ArrRead.InsertOnSubmit(read);
                            }
                            else//修改已读记录
                            {
                                model.IsRead = 1;
                            }

                            db.SubmitChanges();
                            return Json(new { state = 1, msg = "请求成功" });
                        }

                        return Json(new { state = 0, msg = "小余额不存在" });
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
