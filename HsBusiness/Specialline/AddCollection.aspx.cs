using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.Specialline
{
    public partial class AddCollection : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Add(string data, string lxr, string vis, string zx)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    var model = JsonConvert.DeserializeObject<PrivateLine>(data);
                    var lxrs = JsonConvert.DeserializeObject<List<PlContacts>>(lxr);
                    var vism = JsonConvert.DeserializeObject<PlVisit>(vis);
                    var ZX = JsonConvert.DeserializeObject<List<PlInfo>>(zx);
                    if (!string.IsNullOrEmpty(Common.TCContext.Current.OnlineUserID))
                    {
                        var user = db.Users.Where(x => x.ID == Convert.ToInt32(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                        var pl = new PrivateLine();
                        pl.Users = user;//关联负责人
                        pl.CompanyName = model.CompanyName;
                        pl.CompanyAddress = model.CompanyAddress;
                        pl.CompanyScale = model.CompanyScale;//电脑台数
                        pl.Remark = model.Remark;//备注
                        pl. State= model.State;//状态
                        pl.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pl.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                       
                        for (int i = 0; i < lxrs.Count; i++)
                        {
                            lxrs[i].PrivateLine = pl;//关联联系人
                        }
                        for (int i = 0; i < ZX.Count; i++)
                        {
                            ZX[i].State = 0;//状态 
                            ZX[i].PrivateLine = pl;//专线关联
                        }
                        vism.PrivateLine = pl;//走访关联
                        vism.VisitTime = DateTime.Now.ToString("yyyy-MM-dd");
                        vism.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                        db.PrivateLine.InsertOnSubmit(pl);
                        db.PlContacts.InsertAllOnSubmit(lxrs);
                        db.PlInfo.InsertAllOnSubmit(ZX);
                        db.PlVisit.InsertOnSubmit(vism);
                        db.SubmitChanges();
                        var result = JsonConvert.SerializeObject(new { state = 1, msg = "添加成功" });
                        return result;

                    }
                    return JsonConvert.SerializeObject(new { state = 0, msg = "添加失败" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { state = 0, msg = "添加失败" });

            }
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetOne(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.PrivateLine.Where(x => x.ID == ID).Select(x => new
                {
                    x.ID,
                    Contacts = x.PlContacts.Select(y => new { y.ID, y.Name, y.Post, y.Tel }),//联系人
                    x.CompanyName,//单位名称
                    x.CompanyAddress,//单位地址
                    x.CompanyScale,//电脑台数
                    x.Remark,
                    x.State,
                    Dedicate = x.PlInfo.Select(y => new { y.ID, y.Operator, y.WeekPrice, y.BandWidth, y.PayType, y.OverTime,y.Type,y.ServerBerSys,y.ServerUsingTime,y.IsCloudPlan }),//专线
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(list);
                return result;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="data"></param>
        /// <param name="lxr"></param>
        /// <param name="zx"></param>
        /// <returns></returns>
        [WebMethod]
        public static string Modify(string data, string lxr, string zx)
        {

            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    var model = JsonConvert.DeserializeObject<PrivateLine>(data);
                    var lxrs = JsonConvert.DeserializeObject<List<PlContacts>>(lxr);
                    var ZX = JsonConvert.DeserializeObject<List<PlInfo>>(zx);//专线
                    var pl = db.PrivateLine.Where(x => x.ID == model.ID).FirstOrDefault();
                    if (pl != null)
                    {

                        pl.CompanyName = model.CompanyName;
                        pl.CompanyAddress = model.CompanyAddress;
                        pl.CompanyScale = model.CompanyScale;//电脑台数
                        pl.Remark = model.Remark;//备注
                        pl.State = model.State;
                        pl.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        for (int i = 0; i < lxrs.Count; i++)
                        {
                            lxrs[i].PrivateLine = pl;//关联联系人
                        }
                        //for (int i = 0; i < ZX.Count; i++)
                        //{
                        //    ZX[i].PrivateLine = pl;//专线关联
                        //}
                        var Contacts = db.PlContacts.Where(x => x.PlID == model.ID).ToList();
                        db.PlContacts.DeleteAllOnSubmit(Contacts);
                        db.PlContacts.InsertAllOnSubmit(lxrs);

                        var PlInfoList = new List<PlInfo>();
                        //原有的专线信息ID
                        var oldPlInfoIDList = db.PlInfo.Where(x => x.PlID == pl.ID).Select(x => x.ID).ToList();//
                        for (int i = 0; i < ZX.Count; i++)//遍历最新数据
                        {
                            if (ZX[i].ID == 0)//新增的
                            {
                                var plinfo = new PlInfo();
                                plinfo.Operator = ZX[i].Operator;
                                plinfo.WeekPrice = ZX[i].WeekPrice;
                                plinfo.BandWidth = ZX[i].BandWidth;
                                plinfo.PayType = ZX[i].PayType;
                                plinfo.OverTime = ZX[i].OverTime;
                                plinfo.State = 0;
                                plinfo.Type = ZX[i].Type;//录入类型
                                plinfo.ServerBerSys = ZX[i].ServerBerSys;//服务器
                                plinfo.ServerUsingTime = ZX[i].ServerUsingTime;//使用时间
                                plinfo.IsCloudPlan = ZX[i].IsCloudPlan;//上云计划
                                plinfo.PrivateLine = pl;
                                PlInfoList.Add(plinfo);
                            }
                            else//修改的
                            {
                                //专线信息
                                var plinfo = db.PlInfo.Where(x => x.ID == Convert.ToInt32(ZX[i].ID)).FirstOrDefault();
                                plinfo.Operator = ZX[i].Operator;
                                plinfo.WeekPrice = ZX[i].WeekPrice;
                                plinfo.BandWidth = ZX[i].BandWidth;
                                plinfo.PayType = ZX[i].PayType;
                                plinfo.OverTime = ZX[i].OverTime;
                                plinfo.Type = ZX[i].Type;//录入类型
                                plinfo.ServerBerSys = ZX[i].ServerBerSys;//服务器
                                plinfo.ServerUsingTime = ZX[i].ServerUsingTime;//使用时间
                                plinfo.IsCloudPlan = ZX[i].IsCloudPlan;//上云计划
                                #region 状态判断
                                if (plinfo.State == 0)//正常
                                {
                                    //正常修改不处理
                                }
                                else if (plinfo.State == 1)//已提醒
                                {
                                    var plremind = db.PlRemind.Where(x => x.PlID == model.ID && x.PlInfoID == Convert.ToInt32(ZX[i].ID)).Where(x => x.Type == 1).OrderByDescending(x => x.AddTime).FirstOrDefault();
                                    if (plremind != null)
                                    {
                                        plremind.State = 2;//未处理
                                    }
                                }
                                else if (plinfo.State == 2)//已回执
                                {
                                    plinfo.State = 0;
                                }
                                #endregion

                                //最新的数据中不包含原有的数据，则删除原有的数据
                                if (!oldPlInfoIDList.Contains(ZX[i].ID))//删除
                                {
                                    var oldPlInfo = db.PlInfo.Where(x => x.ID == Convert.ToInt32(ZX[i].ID)).FirstOrDefault();
                                    var plremind = db.PlRemind.Where(x => x.PlInfoID == oldPlInfo.ID);

                                    db.PlInfo.DeleteOnSubmit(oldPlInfo);//删除旧的专线数据
                                    db.PlRemind.DeleteAllOnSubmit(plremind);//删除原有的的提醒记录
                                }
                            }
                        }

                        db.PlInfo.InsertAllOnSubmit(PlInfoList);

                        db.SubmitChanges();
                        var result = JsonConvert.SerializeObject(new { state = 1, msg = "修改成功" });
                        return result;
                    }
                    return JsonConvert.SerializeObject(new { state = 0, msg = "修改失败" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { state = 0, msg = "修改失败" });

            }

        }
    }
}