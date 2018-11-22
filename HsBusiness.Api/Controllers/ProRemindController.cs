using HsBusiness.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class ProRemindController : ApiController
    {
        /// <summary>
        /// 回执
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RemindReceipt([FromBody]RequestModel req)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);

                    var UserID = Convert.ToInt32(parameters["UserID"]);//用户ID
                    var ProRemindID = Convert.ToInt32(parameters["ProRemindID"]);//提醒回执ID     

                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    var pr = db.ProRemind.Where(x => x.ID == ProRemindID).FirstOrDefault();
                    if (user != null)
                    {
                        if (pr != null)
                        {
                            pr.RemindContents = parameters["RemindContents"];
                            pr.ReceiptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            pr.Users = user;
                            var pro = db.Project.Where(x => x.ID == pr.ProID).FirstOrDefault();//项目                       
                            if (pro != null)
                            {
                                if (pr.Type == 1)//派单回执
                                {
                                    pro.State = 2;
                                    pro.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                if (pr.Type == 2)//回访回执
                                {
                                    var vis = db.ProVisit.Where(x => x.ID == pr.ProVisitID).FirstOrDefault();
                                    if (vis != null)
                                    {
                                        vis.State = 2;
                                    }
                                    else
                                    {
                                        return Json(new { state = 0, msg = "回访记录不存在" });
                                    }
                                }
                                pr.State = 1;//是否回执
                                db.SubmitChanges();

                                return Json(new { state = 1, msg = "回执成功" });
                            }
                            return Json(new { state = 1, msg = "项目不存在" });
                        }

                        return Json(new { state = 0, msg = "提醒不存在" });
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
