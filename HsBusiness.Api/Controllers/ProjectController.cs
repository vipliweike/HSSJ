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
    public class ProjectController : ApiController
    {
        /// <summary>
        /// 获取项目派单列表
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

                    // 真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var PageIndex = Convert.ToInt32(parameters["PageIndex"]);//当前页数
                    var PageSize = Convert.ToInt32(parameters["PageSize"]);//数量
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var Search = parameters["Search"];
                    var Areas = parameters["Areas"];//区县搜索
                    var Type = parameters["Type"];
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();//用户
                    if (user != null)
                    {
                        var list = db.Project.Where(x => 1 == 1);
                        #region 角色判断
                        if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        {
                            list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                        }
                        else
                        if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                        {
                            list = list.Where(x => x.Users.Areas == user.Areas);//本区县的
                            //list = list.Where(x => x.UserID == UserID);//个人的
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
                        if (!string.IsNullOrEmpty(Type))
                        {
                            list = list.Where(x => x.Type == Convert.ToInt32(Type));
                        }
                        if (!string.IsNullOrEmpty(Search))//查询
                        {
                            list = list.Where(x => x.Users.Name.Contains(Search) && x.Instruction.Contains(Search) || x.Introduce.Contains(Search) || x.Areas.Contains(Search) || x.Users.Name.Contains(Search));
                        }
                        if (!string.IsNullOrEmpty(Areas))
                        {
                            list = list.Where(x => x.Users.Areas == Areas);
                        }

                        var result = list
                            .Select(x => new
                            {
                                x.ID,
                                x.Areas,
                                x.Instruction,
                                x.Introduce,
                                x.AddTime,
                                UserName = x.Users.Name,
                                IsRead = x.ProRead.Where(y => y.UserID == user.ID).FirstOrDefault(),//用户自己的已读状态
                                x.LastTime,
                                LatestProgress = x.ProVisit.OrderByDescending(y => y.AddTime).FirstOrDefault(),
                                x.Type,//类型
                                ProRemind = x.ProRemind.Where(y => y.State == 0).Count(),//回执数量
                            }).Select(x => new
                            {
                                x.ID,
                                x.Areas,
                                x.Instruction,
                                x.Introduce,
                                x.AddTime,
                                x.UserName,
                                IsRead = x.IsRead == null ? 0 : x.IsRead.IsRead,//为空是未读，不为空按照状态
                                x.LastTime,
                                LatestProgress = x.LatestProgress != null ? x.LatestProgress.VisitContents : "",
                                x.Type,
                                Remind = x.ProRemind > 0 ? 0 : 1,//提醒数量大于0时是未读，否则为已读
                            }).Select(x => new
                            {
                                x.ID,
                                x.Areas,
                                x.Instruction,
                                x.Introduce,
                                x.AddTime,
                                x.UserName,
                                x.LastTime,
                                x.LatestProgress,
                                x.Type,
                                IsRead = x.IsRead == 1 && x.Remind == 1 ? 1 : 0,//未读且无提醒返回0，否则返回1；0未读显示，1已读隐藏
                            })
                            .OrderBy(x => x.IsRead)//先
                            .ThenByDescending(x => x.LastTime)//再
                            .ThenByDescending(x => x.LatestProgress)//最后
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
        public IHttpActionResult GetOne(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    // 真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var ProID = Convert.ToInt32(parameters["ProID"]);

                    var list = db.Project.Where(x => x.ID == ProID)
                        .Select(x => new
                        {
                            x.ID,
                            x.Areas,
                            x.Instruction,
                            x.Introduce,
                            x.AddTime,
                            UserName = x.Users.Name,
                            UserID = x.UserID,
                            x.IsRead,
                            x.LastTime,
                            x.Type,
                            ProVisit = x.ProVisit.OrderByDescending(y => y.AddTime).Select(y => new
                            {
                                y.ID,
                                AddTime = y.VisitTime,//本次回访记录
                                y.Img,
                                y.NextTime,
                                y.VisitContents,
                                ProVisitRemind = y.ProRemind.Where(t => t.Type == 2 && t.State == 0).OrderByDescending(t => t.AddTime).FirstOrDefault(),
                            }),
                            ProRemind = x.ProRemind.Where(y => y.Type == 1 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { y.ID, y.AddTime }).FirstOrDefault()
                        })
                        .Select(x => new
                        {
                            x.ID,
                            x.Areas,
                            x.Instruction,
                            x.Introduce,
                            x.AddTime,
                            x.UserName,
                            x.IsRead,
                            x.LastTime,
                            x.Type,
                            x.UserID,
                            ProVisit = x.ProVisit.Select(y => new
                            {
                                y.ID,
                                y.AddTime,
                                Img = ImgHelper.GetImgAllPath(y.Img),
                                y.NextTime,
                                y.VisitContents,
                                ProRemindID = y.ProVisitRemind != null ? y.ProVisitRemind.ID : 0,//走访提醒回执
                            }),
                            ProRemindID = x.ProRemind != null ? x.ProRemind.ID : 0,//派单提醒回执

                        })
                        .FirstOrDefault();

                    return Json(new { data = list, state = 1, msg = "请求成功" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 已读
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
                    var ProID = Convert.ToInt32(parameters["ProID"]);
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var pro = db.Project.Where(x => x.ID == ProID).FirstOrDefault();
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    if (user != null)
                    {
                        if (pro != null)
                        {
                            var model = db.ProRead.Where(x => x.ProID == pro.ID && x.UserID == user.ID).FirstOrDefault();
                            if (model == null)//添加已读记录
                            {
                                ProRead read = new ProRead();
                                read.UserID = user.ID;
                                read.ProID = pro.ID;
                                read.IsRead = 1;

                                db.ProRead.InsertOnSubmit(read);
                            }
                            else//修改已读记录
                            {
                                model.IsRead = 1;
                            }
                            #region 以弃用
                            pro.IsRead = 1;//已读()
                            #endregion
                            pro.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//最后更新时间

                            db.SubmitChanges();
                            return Json(new { state = 1, msg = "请求成功" });
                        }

                        return Json(new { state = 0, msg = "项目不存在" });
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
