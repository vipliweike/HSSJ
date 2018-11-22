using HsBusiness.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class StoreRemindController : ApiController
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

                    var StoreRemindID = Convert.ToInt32(parameters["StoreRemindID"]);//提醒回执ID                    
                    var sr = db.StoreRemind.Where(x => x.ID == StoreRemindID).FirstOrDefault();
                    if (sr != null)
                    {
                        sr.Contents = parameters["Contents"];
                        sr.ReceiptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var store = db.Store.Where(x => x.ID == sr.StoreID).FirstOrDefault();//商机                        
                        if (store != null)
                        {
                            if (sr.Type == 1)//宽带到期回执
                            {
                                store.State = 2;
                                store.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (sr.Type == 2)//回访回执
                            {
                                var vis = db.StoreVisit.Where(x => x.ID == sr.StoreVisitID).FirstOrDefault();
                                if (vis != null)
                                {
                                    vis.State = 2;
                                }
                                else
                                {
                                    return Json(new { state = 0, msg = "回访记录不存在" });
                                }
                            }
                            sr.State = 1;//是否回执
                            db.SubmitChanges();

                            return Json(new { state = 1, msg = "回执成功" });
                        }
                        return Json(new { state = 1, msg = "门店不存在" });
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
