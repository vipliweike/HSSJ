using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.ProjectManage
{
    public partial class ProjectDetails : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 得到基础信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetInfo(int ID)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Project.Where(x =>x.ID==ID).Select(x => new
                {

                    x.AddTime,
                    x.Introduce,
                    x.Instruction,
                    x.Areas,
                    x.LastTime

                });
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }
        /// <summary>
        /// 得到走访记录
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetVistInfo(int ID)
        {
            using (dbDataContext db=new HsBusiness.dbDataContext())
            {
                string[] imgNULL = { };
                var list = db.ProVisit.Where(x => x.ProID == ID).Select(x => new
                {

                    x.AddTime,
                    x.VisitContents,
                    Img = x.Img != null ? x.Img.Split(';') : imgNULL,
                    FzrName=x.Users.Name,
                    x.NextTime,
                    State=x.State==0?"正常":x.State==1?"已派单":x.State==2?"已回执":""

                }).OrderByDescending(x=>x.AddTime).ToList();
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }

        }
        /// <summary>
        /// 回执记录
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetRemind(int ID)
        {
            using (dbDataContext db=new HsBusiness.dbDataContext())
            {

                var list = db.Project.Where(x => x.ID == ID).Select(x => new
                {
                    Remind = x.ProRemind.Where(y => y.State == 1).Select(y => new
                    {
                        y.RemindContents,//回执内容
                        y.ReceiptTime,//回执时间
                        Areas= y.Project.Areas,//回执区县
                        UserName = y.Project.Users.Name//回执人
                      
                    }).OrderByDescending(y=>y.ReceiptTime)
                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(new { data = list, stste = 1, msg = "成功" });
                return result;
            }
        }
    }
}