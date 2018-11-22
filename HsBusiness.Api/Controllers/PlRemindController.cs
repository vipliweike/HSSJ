using HsBusiness.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class PlRemindController : ApiController
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

                    var PlRemindID = Convert.ToInt32(parameters["PlRemindID"]);//提醒回执ID                    
                    var plr = db.PlRemind.Where(x => x.ID == PlRemindID).FirstOrDefault();//提醒回执
                    if (plr != null)
                    {
                        plr.Contents = parameters["Contents"];
                        plr.ReceiptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var pl = db.PrivateLine.Where(x => x.ID == plr.PlID).FirstOrDefault();//专线                        
                        if (pl != null)
                        {
                            if (plr.Type == 1)//专线到期
                            {
                                var plinfo = db.PlInfo.FirstOrDefault(x => x.ID == plr.PlInfoID);//专线信息
                                if (plinfo != null)
                                {
                                    plinfo.State = 2;
                                }
                                else
                                {
                                    return Json(new { state = 0, msg = "专线信息不存在" });
                                }

                            }
                            if (plr.Type == 2)//回访回执
                            {
                                var plv = db.PlVisit.Where(x => x.ID == plr.PlVisitID).FirstOrDefault();//回访记录
                                if (plv != null)
                                {
                                    plv.State = 2;
                                }
                                else
                                {
                                    return Json(new { state = 0, msg = "回访记录不存在" });
                                }
                            }
                            plr.State = 1;//是否回执
                            db.SubmitChanges();

                            return Json(new { state = 1, msg = "回执成功" });
                        }
                        return Json(new { state = 1, msg = "专线不存在" });
                    }

                    return Json(new { state = 0, msg = "提醒不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
