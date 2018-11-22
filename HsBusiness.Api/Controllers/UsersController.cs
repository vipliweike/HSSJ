using HsBusiness.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{

    public class UsersController : ApiController
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Login([FromBody]RequestModel req)//string data,string secret
        {
            try
            {
                //真实参数
                var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);

                LoginUser u = new LoginUser();
                u.Mobile = parameters["Mobile"];
                u.Pwd = parameters["Pwd"];
                if (!string.IsNullOrEmpty(u.Mobile) && !string.IsNullOrEmpty(u.Pwd))
                {
                    using (dbDataContext db = new dbDataContext())
                    {
                        string pwd = Common.MD5.Encrypt(u.Pwd, 32);
                        var list = db.Users.Where(x => x.Mobile == u.Mobile && x.Pwd == pwd).Select(x => new
                        {
                            x.ID,
                            x.Mobile,
                            x.Name,
                            x.RoleID,
                            x.Roles.RoleName,
                            x.Areas,
                            x.Grids,
                            x.Post,
                            x.AddTime,

                        }).FirstOrDefault();
                        if (list != null)
                        {
                            #region 添加登录日志
                            ApiLog log = new ApiLog();
                            log.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            log.ClientType = 0;
                            log.Code = (int?)System.Net.HttpStatusCode.OK;
                            log.ErrMsg = "";
                            log.Parameters = Newtonsoft.Json.JsonConvert.SerializeObject(parameters.AllKeys.ToDictionary(k => k, k => parameters[k]));
                            log.RequestName = "/Users/Login";
                            log.UserID = list.ID;

                            db.ApiLog.InsertOnSubmit(log);
                            db.SubmitChanges();
                            #endregion
                            return Json(new { data = list, state = 1, msg = "登录成功" });
                        }
                        return Json(new { state = 0, msg = "手机号或密码错误" });
                    }
                }
                return Json(new { state = 0, msg = "手机号或密码为空" });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ModifyPwd([FromBody]RequestModel req)
        {
            try
            {
                ModifyPwdModel model = new ModifyPwdModel();
                var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);
                model.ID = Convert.ToInt32(parameters["ID"]);
                model.NewPwd = Common.MD5.Encrypt(parameters["NewPwd"], 32);//加密后
                model.OldPwd = Common.MD5.Encrypt(parameters["OldPwd"], 32);//加密后
                model.RepeatPwd = Common.MD5.Encrypt(parameters["RepeatPwd"], 32);//加密后
                if ((!string.IsNullOrEmpty(model.NewPwd) && !string.IsNullOrEmpty(model.OldPwd) && model.ID != 0))
                {
                    if (model.NewPwd == model.RepeatPwd)
                    {
                        using (dbDataContext db = new dbDataContext())
                        {
                            var list = db.Users.Where(x => x.ID == model.ID).FirstOrDefault();
                            if (list != null)
                            {
                                if (list.Pwd == model.OldPwd)
                                {
                                    list.Pwd = model.NewPwd;
                                    db.SubmitChanges();

                                    return Json(new { state = 1, msg = "修改成功" });
                                }
                                return Json(new { state = 0, msg = "旧密码不正确" });
                            }
                            return Json(new { state = 0, msg = "用户不存在" });
                        }
                    }
                    return Json(new { state = 0, msg = "两次密码不一样" });
                }
                return Json(new { state = 0, msg = "密码或ID为空" });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        /// <summary>
        /// 获取个人基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetInfo(string data, string secret)
        {
            try
            {
                var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                int ID = Convert.ToInt32(parameters["ID"]);
                using (dbDataContext db = new dbDataContext())
                {
                    var list = db.Users.Where(x => x.ID == ID).Select(x => new
                    {
                        x.ID,
                        x.Name,
                        x.Mobile,
                        x.Post,
                        x.Areas

                    }).FirstOrDefault();
                    return Json(new { data = list, msg = "请求成功", state = 1 });
                }
            }
            catch (Exception ex)
            {
                //return Json(new { state = 0, msg = "请求失败" });
                throw ex;
            }
        }
        /// <summary>
        /// 获取个人详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Details(string data, string secret)
        {
            try
            {
                var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                int ID = Convert.ToInt32(parameters["ID"]);
                using (dbDataContext db = new dbDataContext())
                {
                    var list = db.Users.Where(x => x.ID == ID).Select(x => new
                    {
                        x.ID,
                        x.Name,
                        x.Mobile,
                        x.Post,
                        x.Pwd,
                        x.Roles.RoleName,
                        x.Remark,
                        x.Areas

                    }).FirstOrDefault();

                    return Json(new { data = list, msg = "请求成功", state = 1 });
                }
            }
            catch (Exception ex)
            {
                //return Json(new { state = 0, msg = "请求失败" });
                throw ex;
            }
        }

        [HttpGet]
        /// <summary>
        /// 获取首页未读状态
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public IHttpActionResult GetUnRead(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    //真实参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var UserID = Convert.ToInt32(parameters["UserID"]);//用户ID

                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    if (user != null)
                    {
                        //未读（需要看的）
                        var bus = db.Business.Where(x => x.MState == 1 || x.FState == 1 || x.Visit.Where(y => y.State == 1).Count() > 0);//商机
                        //var store = db.Store.Where(x => x.State == 1 || x.StoreVisit.Where(y => y.State == 1).Count() > 0);//门店
                        var pro = db.Project.Where(x => x.State == 1 || x.ProVisit.Where(y => y.State == 1).Count() > 0 || x.ProRead.Where(y => y.UserID == UserID).Count() == 0 || x.ProRead.Where(y => y.UserID == UserID && y.IsRead == 0).Count() > 0);//项目
                        //当前用户的未读
                        var balance = db.SmallBalance.Where(x => x.SmaBaRead.Where(y => y.UserID == UserID && y.IsRead == 0).Count() > 0 || x.SmaBaRead.Where(y => y.UserID == UserID).Count() == 0);//小余额
                        //专线
                        var pl = db.PrivateLine.Where(x => x.PlInfo.Where(y => y.State == 1).Count() > 0 || x.PlVisit.Where(y => y.State == 1).Count() > 0);
                        #region  角色判断
                        if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        {
                            bus = bus.Where(x => 1 == 1);
                            //store = store.Where(x => 1 == 1);
                            pro = pro.Where(x => 1 == 1);
                            balance = balance.Where(x => 1 == 1);
                            pl = pl.Where(x => 1 == 1);
                        }
                        else
                        if (user.Roles.RoleName == "区县经理")
                        {
                            bus = bus.Where(x => x.Areas == user.Areas);
                            //store = store.Where(x => x.Users.Areas == user.Areas);
                            pro = pro.Where(x => x.UserID == UserID);
                            balance = balance.Where(x => 1 == 1);
                            pl = pl.Where(x => x.Users.Areas == user.Areas);
                        }
                        else
                        if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")
                        {
                            bus = bus.Where(x => x.UserID == UserID);
                            //store = store.Where(x => x.UserID == UserID);
                            pro = pro.Where(x => x.UserID == UserID);
                            balance = balance.Where(x => x.UserID == UserID);
                            pl = pl.Where(x => x.UserID == UserID);
                        }
                        else
                        {
                            bus = bus.Where(x => x.ID < 0);//无数据
                            //store = store.Where(x => x.ID < 0);//无数据
                            pro = pro.Where(x => x.ID < 0);//无数据
                            balance = balance.Where(x => x.ID < 0);//无数据
                            pl = pl.Where(x => x.ID < 0);//无数据
                        }
                        #endregion

                        //结果
                        var result = new
                        {
                            // 0未读    1已读
                            BusIsRead = bus.Count(),//商机//BusIsRead = bus.Count() == 0 ? 1 : 0
                            //StoreIsRead = store.Count() == 0 ? 1 : 0,//门店
                            ProIsRead = pro.Count(),//项目派单
                            BalanceIsRead = balance.Count(),//小余额
                            PrivateLineIsRead = pl.Count(),//专线
                        };

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
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SendCode([FromBody]RequestModel req)
        {
            try
            {
                var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);
                var mobile = parameters["Mobile"];

                using (dbDataContext db = new dbDataContext())
                {
                    var strCode = GetCode();//验证码
                    var mailService = new Mafly.Mail.Mail();//发送邮件
                    mailService.Send("sss1345156413" + "@qq.com", "衡水商机", "验证码", "<h2>" + strCode + "</h2>");


                    var code = new VCode();
                    code.Mobile = mobile;
                    code.Code = strCode;
                    code.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    code.Flag = false;

                    db.VCode.InsertOnSubmit(code);
                    db.SubmitChanges();

                    return Json(new { msg = "发送成功", state = 1 });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("发送失败，请稍后重试");
            }
        }

        [HttpPost]
        public IHttpActionResult ForgetPwd([FromBody]RequestModel req)
        {
            try
            {
                var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);
                var mobile = parameters["Mobile"];
                var vcode = parameters["VCode"];
                var pwd = parameters["Pwd"];
                using (dbDataContext db = new dbDataContext())
                {
                    var code = db.VCode.Where(x => x.Mobile == mobile && DateTime.Now.AddDays(-1) < Convert.ToDateTime(x.AddTime) && x.Flag == false)
                        .OrderByDescending(x => x.ID).FirstOrDefault();
                    if (code != null)
                    {
                        if (code.Code == vcode)
                        {
                            var user = db.Users.FirstOrDefault(x => x.Mobile == mobile);
                            if (user != null)
                            {
                                user.Pwd = Common.MD5.Encrypt(pwd, 32);
                                code.Flag = true;

                                db.SubmitChanges();

                                return Json(new { msg = "修改成功", state = 1 });
                            }
                            return Json(new { msg = "用户不存在", state = 0 });
                        }
                        return Json(new { msg = "验证码不正确", state = 0 });
                    }
                    return Json(new { msg = "请发送验证码", state = 0 });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //随机验证码
        public string GetCode()
        {
            string strnum = "";
            string ranstr = "0|1|2|3|4|5|6|7|8|9";
            //string ranstr = "0|1|2|3|4|5|6|7|8|9|a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z";
            string[] usestr = ranstr.Split('|');
            Random r = new Random();
            for (int i = 0; i < 6; i++)
            {
                strnum += usestr[r.Next(i, usestr.Length)];
            }
            return strnum;
        }
    }
}
