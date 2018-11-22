using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class VersionController : ApiController
    {
        /// <summary>
        /// 获取最新版本信息
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

                    //var VersionType = Convert.ToInt32(parameters["VersionType"]);

                    //安卓
                    var list = db.Version.Where(x => x.VersionType == 0).OrderByDescending(x => x.AddTime).Take(1);

                    if (list.Count() > 0)
                    {
                        var result = list.ToList().Select(x => new
                        {
                            x.ID,
                            x.VersionCode,
                            x.VersionInfo,
                            x.VersionName,
                            x.VersionType,
                            x.IsForced,
                            x.DownUrl,
                            AddTime=x.AddTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        }).FirstOrDefault();
                        return Json(new { data = result, state = 1, msg = "请求成功" });
                    }
                    return Json(new { state = 0, msg = "暂无版本信息" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
