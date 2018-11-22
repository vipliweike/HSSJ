using HsBusiness.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class BusRemindController : ApiController
    {
                
        /// <summary>
        /// 提醒回执
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ReminderReceipt([FromBody]RequestModel req)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    var parameters = Common.AesDecryp.GetAesDecryp(req.data, req.secret);

                    var RemindID = Convert.ToInt32(parameters["RemindID"]);//提醒回执ID                    
                    var br = db.BusRemind.Where(x => x.ID == RemindID).FirstOrDefault();
                    if (br != null)
                    {
                        br.Contents = parameters["Contents"];
                        br.UserID = Convert.ToInt32(parameters["UserID"]);
                        br.ReceiptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var bus = db.Business.Where(x => x.ID == br.BusID).FirstOrDefault();//商机                        
                        if (bus != null)
                        {
                            if (br.Type == 1)//移动回执
                            {
                                bus.MState = 2;
                                bus.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (br.Type == 2)//固网回执
                            {
                                bus.FState = 2;
                                bus.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (br.Type == 3)//回访回执
                            {
                                var vis = db.Visit.Where(x => x.ID == br.VisitID).FirstOrDefault();
                                if (vis != null)
                                {
                                    vis.State = 2;
                                }
                                else
                                {
                                    return Json(new { state = 0, msg = "回访记录不存在" });
                                }
                            }                            
                            br.State = 1;//是否回执
                            db.SubmitChanges();

                            return Json(new { state = 1, msg = "回执成功" });
                        }
                        return Json(new { state = 1, msg = "商机不存在" });
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
