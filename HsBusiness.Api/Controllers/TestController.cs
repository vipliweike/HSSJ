using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        /// <summary>
        /// 加密用户密码（MD5）
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult PasswordEncryption()
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Users.Where(x => 1 == 1);
                if (list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        item.Pwd = Common.MD5.Encrypt(item.Pwd, 32);
                    }
                    db.SubmitChanges();
                }
                return Json(new { });
            }
        }

        [HttpPost]
        /// <summary>
        /// 同步商机专线信息到专线表
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult DataSynchronization()
        {
            using (dbDataContext db = new dbDataContext())
            {
                var plList = db.PrivateLine.Where(x => 1 == 1).Select(x => x.CompanyName);
                var bus = db.Business.Where(x => x.FUseWork == "专线" && plList.Contains(x.CompanyName) == false);//商机专线信息
                if (bus.Count() > 0)
                {
                    foreach (var item in bus)//遍历商机的专线信息
                    {
                        var pl = new PrivateLine();
                        pl.CompanyAddress = item.CompanyAddress;//单位地址
                        pl.CompanyName = item.CompanyName;//单位名称
                        pl.CompanyScale = item.FComputerNum;//单位规模-------》电脑台数
                        pl.AddTime = item.AddTime;
                        pl.LastTime = item.LastTime;
                        pl.UserID = item.UserID;
                        //for (int i = 0; i < Convert.ToInt32(item.FUseScale.Replace("条", "")); i++)//循环专线条数
                        //{
                        var plinfo = new PlInfo();
                        plinfo.BandWidth = item.FUseBandWidth;//带宽
                        plinfo.Operator = item.FOperator;//运营商
                        plinfo.OverTime = item.FOverTime;//到期时间
                        plinfo.PayType = "";//付费方式
                        plinfo.State = 0;//状态
                        plinfo.WeekPrice = item.FWeekPrice;//周价
                        plinfo.PrivateLine = pl;//关联
                        db.PlInfo.InsertOnSubmit(plinfo);
                        //}
                        if (item.Contacts.Count > 0)
                        {
                            foreach (var l in item.Contacts)
                            {
                                var lxr = new PlContacts();
                                lxr.Name = l.Name;
                                lxr.Post = l.Post;
                                lxr.Tel = l.Tel;
                                lxr.PrivateLine = pl; 
                                db.PlContacts.InsertOnSubmit(lxr);
                            }
                        }



                        db.PrivateLine.InsertOnSubmit(pl);


                    }
                    db.SubmitChanges();
                }

                return Json(new { });
            }
        }

        [HttpPost]
        /// <summary>
        /// 同步商机专线信息到专线表
        /// 修改同步联系人数据
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult ModifyDataSynchronization()
        {
            using (dbDataContext db = new dbDataContext())
            {
                //var plList = db.PrivateLine.Where(x => 1 == 1).Select(x => x.CompanyName);
                var bus = db.Business.Where(x => x.FUseWork == "专线");//商机专线信息
                if (bus.Count() > 0)
                {
                    foreach (var item in bus)//遍历商机的专线信息
                    {
                        var pl = db.PrivateLine.Where(x => x.CompanyName == item.CompanyName).FirstOrDefault();

                        if (pl != null)
                        {
                            if (item.Contacts.Count > 0)
                            {
                                foreach (var l in item.Contacts)
                                {
                                    var lxr = new PlContacts();
                                    lxr.Name = l.Name;
                                    lxr.Post = l.Post;
                                    lxr.Tel = l.Tel;
                                    lxr.PlID = pl.ID;
                                    db.PlContacts.InsertOnSubmit(lxr);
                                }
                            }
                        }                        

                    }
                    db.SubmitChanges();
                }

                return Json(new { });
            }
        }
    }
}
