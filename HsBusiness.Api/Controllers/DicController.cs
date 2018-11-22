using HsBusiness.Api.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class DicController : ApiController
    {
        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetDic(string data, string secret)
        {
            try
            {
                //真实的参数
                var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                string Name = parameters["Name"];
                string Type = parameters["Type"];
                using (dbDataContext db = new dbDataContext())
                {
                    dynamic list = null;
                    if (Type == "1")
                    {
                        list = db.Dic.Where(t => t.PID == db.Dic.Where(x => x.Name == Name).Select(x => x.ID).FirstOrDefault()).Select(x => new
                        {
                            x.Name
                        }).ToList();
                    }
                    else if (Type == "0")
                    {
                        list = db.Dic.Where(x => db.Dic.Where(t => t.PID == 0).Select(t => t.ID).Contains((int)x.PID))
                            .Select(x => new {x.ID, x.Name, x.PID })
                            .OrderBy(x => x.ID)
                            .GroupBy(x => x.PID)
                            .Select(x => new
                            {
                                //x.ID,
                                NameList = x.Select(y => new { y.ID,y.Name }).ToList(),
                                //x.PID,
                                PName = db.Dic.Where(y => y.ID == x.FirstOrDefault().PID).Select(y => y.Name).FirstOrDefault(),
                            }).ToList();
                    }
                    return Json(new { data = list, state = 1, msg = "请求成功" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
