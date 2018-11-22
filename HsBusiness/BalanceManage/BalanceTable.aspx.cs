using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.BalanceManage
{
    public partial class BalanceTable : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Common.TCContext.Current.OnlineUserType == "公司领导" || Common.TCContext.Current.OnlineUserType == "政企部主管")
            {
                this.ImportTable.Visible = true;
            }
            if (Common.TCContext.Current.OnlineUserType == "区县经理")
            {
                this.ImportTable.Visible = false;
            }
            if (Common.TCContext.Current.OnlineUserType == "客户经理" || Common.TCContext.Current.OnlineUserType == "网格助理" || Common.TCContext.Current.OnlineUserType == "行业经理")
            {
                this.ImportTable.Visible = false;
            }
        }


        [WebMethod]
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <returns></returns>
        public static string Delete(int id)
        {
            using (dbDataContext db = new dbDataContext())
            {
                string result = "";
                var balance = db.SmallBalance.FirstOrDefault(x => x.ID == id);//用户信息
                db.SmallBalance.DeleteOnSubmit(balance);
                OperateLog opermodel = new OperateLog();
                opermodel.Operator = balance.Users.Name;//操作人
                opermodel.OperTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作时间
                opermodel.OperType = "删除";//操作类型
                opermodel.Operdescribe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + balance.Users.Name + "进行了单个删除操作";
                db.OperateLog.InsertOnSubmit(opermodel);
                db.SubmitChanges();
                result = JsonConvert.SerializeObject(new { msg = "删除成功", state = 1 });
                return result;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string BatchDel(string ids)
        {
            using (dbDataContext db = new dbDataContext())
            {

                // string msg = "删除失败";
                //string[] strArray = ids.Split(',');
                //int[] intArray = Array.ConvertAll<string, int>(strArray, x => Convert.ToInt32(x));
                //bool result = bll.Delete(x => intArray.Contains(x.ID));
                //// var ss = bll.Modify(x => intArray.Contains(x.ID));
                //if (result)
                //{
                //    state = "true";
                //    msg = "删除成功";
                //}
                //return Json(new { state = state, msg = msg });
                string result = "";
                string[] strArray = ids.Split(',');
                int[] intArray = Array.ConvertAll<string, int>(strArray, x => Convert.ToInt32(x));
                var item = db.SmallBalance.Where(x => intArray.Contains(x.ID)).ToList();
                db.SmallBalance.DeleteAllOnSubmit(item);
                OperateLog opermodel = new OperateLog();
                opermodel.Operator = Common.TCContext.Current.OnlineRealName.ToString();//操作人
                opermodel.OperTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作时间
                opermodel.OperType = "批量删除";//操作类型
                opermodel.Operdescribe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Common.TCContext.Current.OnlineRealName + "进行了批量删除操作";
                db.SubmitChanges();
                result = JsonConvert.SerializeObject(new { msg = "删除成功", stste = 1 });
                return result;

            }
        }
        /// <summary>
        /// 全部删除
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string DeleteAll()
        {
            using (dbDataContext db=new HsBusiness.dbDataContext ())
            {
                string result = "";
                var item = db.SmallBalance.Where(x=>true).ToList();
                db.SmallBalance.DeleteAllOnSubmit(item);
                OperateLog opermodel = new OperateLog();
                opermodel.Operator = Common.TCContext.Current.OnlineRealName.ToString();//操作人
                opermodel.OperTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作时间
                opermodel.OperType = "全部删除";//操作类型
                opermodel.Operdescribe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Common.TCContext.Current.OnlineRealName + "进行了批量删除操作";
                db.SubmitChanges();
                result = JsonConvert.SerializeObject(new { msg = "删除成功", stste = 1 });
                return result;
            }
        }
        /// <summary>
        /// 得到数据列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Get(string Areas, string State, string CusName, string FzrName, string Search, string pageIndex)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                int pageSize = 10;
                var user = db.Users.Where(x => x.ID == int.Parse(Common.TCContext.Current.OnlineUserID)).FirstOrDefault();
                var list = db.SmallBalance.Where(x => 1 == 1);
                if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                {
                    list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                }

                if (user.Roles.RoleName == "区县经理")//区县经理
                {
                    list = list.Where(x => x.Users.Areas == user.Areas);
                    //list = list.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                }

                if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                {
                    list = list.Where(x => x.UserID == int.Parse(Common.TCContext.Current.OnlineUserID));
                }
                HttpContext.Current.Session["UserType"] = user.Roles.RoleName;
                if (!string.IsNullOrEmpty(Areas))
                {
                    list = list.Where(x => x.Users.Areas == Areas);
                }
                if (!string.IsNullOrEmpty(CusName))
                {
                    list = list.Where(x => x.CustomerName.Contains(CusName));
                }
                if (!string.IsNullOrEmpty(FzrName))
                {
                    list = list.Where(x => x.Users.Name.Contains(FzrName));
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    list = list.Where(x => x.Users.Name.Contains(Search) || x.AccessNumber.Contains(Search) || x.Responsibility.Contains(Search) || x.Balance == Search || x.GroupName.Contains(Search) || x.NetTime.Contains(Search) || x.Contacts.Contains(Search) || x.ContactTel.Contains(Search) || x.AddTime.Contains(Search));
                }

                var list1 = list.Select(x => new
                {
                    x.ID,
                    x.AccountID,//账号id
                    x.AccessNumber,//接入号
                    UserName = x.Users.Name,
                    x.Responsibility,//责任区域
                    x.Balance,
                    x.CustomerName,
                    x.GroupID,
                    x.GroupName,
                    x.NetTime,
                    x.WeekPrice,
                    x.Broadband,//宽带
                    x.InstalledAddress,//装机地址
                    x.Contacts,
                    x.ContactTel,
                    x.State,
                    x.UserID,
                    x.AddTime,
                    IsRead = x.SmaBaRead.Where(y => y.UserID == user.ID).FirstOrDefault() == null ? 0 : x.SmaBaRead.Where(y => y.UserID == user.ID).FirstOrDefault().IsRead
                });
                if (!string.IsNullOrEmpty(State))
                {
                    list1 = list1.Where(x => x.IsRead == int.Parse(State));
                }
                var resultList = list1.OrderByDescending(x => x.AddTime).Skip((int.Parse(pageIndex) - 1) * pageSize).Take(pageSize).ToList()
                .Select(x => new
                {
                    x.ID,
                    x.AccountID,//账号id
                    x.AccessNumber,//接入号
                    UserName = x.UserName,
                    x.Responsibility,//责任区域
                    x.Balance,
                    x.CustomerName,
                    x.GroupID,
                    x.GroupName,
                    x.NetTime,
                    x.WeekPrice,
                    x.Broadband,//宽带
                    x.InstalledAddress,//装机地址
                    x.Contacts,
                    x.ContactTel,
                    x.State,
                    x.UserID,
                    AddTime = string.IsNullOrEmpty(x.AddTime) ? "" : Convert.ToDateTime(x.AddTime).ToString("yyyy-MM-dd"),
                    x.IsRead,//为空是未读，不为空按照状态
                });

                var result = new { status = "1", data = resultList, pagecount = list1.Count().ToString(), pagesize = pageSize.ToString() };
                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }
        }
        /// <summary>
        /// 得到回执记录
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetRemind(int BaID)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.SmaBaRemind.Where(x => x.SmBaID == BaID).Select(x => new
                {
                    x.ReceiptTime,
                    x.ReceiptContent
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(new { data = list });
                return result;
            }
        }

        [WebMethod]
        public static string Read(int SmBaID)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                if (Common.TCContext.Current.OnlineUserID != "")
                {
                    var UserID = Convert.ToInt32(Common.TCContext.Current.OnlineUserID);//用户ID
                    var list = db.SmaBaRead.Where(x => x.SmBaID == SmBaID && x.UserID == UserID).FirstOrDefault();
                    if (list == null)
                    {
                        SmaBaRead read = new SmaBaRead();
                        read.UserID = UserID;
                        read.SmBaID = SmBaID;
                        read.IsRead = 1;

                        db.SmaBaRead.InsertOnSubmit(read);
                    }
                    else
                    {
                        list.IsRead = 1;
                    }
                    db.SubmitChanges();
                    return JsonConvert.SerializeObject(new { state = 1, msg = "请求成功" });
                }
                return JsonConvert.SerializeObject(new { state = 0, msg = "已读出错，请重新登录" });
            }
        }
    }
}