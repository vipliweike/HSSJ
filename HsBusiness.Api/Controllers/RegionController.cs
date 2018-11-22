using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class RegionController : ApiController
    {
        /// <summary>
        /// 获取区县列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(string data, string secret)
        {
            try
            {
                var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                using (dbDataContext db = new dbDataContext())
                {
                    var list = db.Region.Where(x =>
                            db.Region.Where(y => y.Pid == 0).Select(y => y.ID).ToList().Contains((int)x.Pid)
                        ).Select(x => new { x.Name }).ToList();

                    return Json(new { data = list, state = 1, msg = "请求成功" });
                }
            }
            catch (Exception ex)
            {
                //return Json(new { state = 0, msg = "请求失败" });
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有网格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public IHttpActionResult GetGrids(string data, string secret)
        {
            try
            {
                var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                using (dbDataContext db = new dbDataContext())
                {
                    var list = db.Region.Where(t => db.Region.Where(x =>
                              db.Region.Where(y => y.Pid == 0).Select(y => y.ID).ToList().Contains((int)x.Pid)
                        ).Select(x => x.ID).ToList().Contains((int)t.Pid)).Select(x => new { x.Name }).ToList();

                    return Json(new { data = list, state = 1, msg = "请求成功" });
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
