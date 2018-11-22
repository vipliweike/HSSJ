using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.PersonManage
{
    public partial class AddPerson : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <returns></returns>
        public static string AddPersons(string Name, string Mobile, string Post, string Areas, string Remark, int roleid, string Grids)
        {

            using (var db = new dbDataContext())
            {

                var item = new Users();
                item.Name = Name;
                item.Mobile = Mobile;
                string pwd = Interface.Comm.MD5.Encrypt("123456", 32);
                item.Pwd = pwd;//默认密码
                item.Post = Post;
                if (Areas == "请选择区县")
                {
                    item.Areas = "衡水市";
                }
                if (Areas != "请选择区县")
                {
                    item.Areas = Areas;
                }
                if (Grids == "请选择网格")
                {
                    item.Grids = "";
                }
                if (Grids != "请选择网格")
                {
                    item.Grids = Grids;
                }
                item.RoleID = roleid;

                item.Remark = Remark;
                db.Users.InsertOnSubmit(item);
                db.SubmitChanges();
                var result = new { msg = "添加成功", state = 1 };
                return new JavaScriptSerializer().Serialize(result);

            }
        }
        /// <summary>
        /// 查询区县字典
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetRegion()
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Region.Where(x => x.Pid == 0).Select(x => new { x.ID }).FirstOrDefault();
                var list1 = db.Region.Where(x => x.Pid == list.ID).Select(x => new { x.ID, x.Name }).ToList();
                return new JavaScriptSerializer().Serialize(list1);
            }

        }
        /// <summary>
        /// 得到角色字典
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetRole()
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Roles.Where(x => 1 == 1).Select(x => new { x.ID, x.RoleName });
                return new JavaScriptSerializer().Serialize(list);

            }
        }
        /// <summary>
        /// 得到区县
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetRegion1(int countyid)
        {
            using (dbDataContext db = new HsBusiness.dbDataContext())
            {
                var list = db.Region.Where(x => x.Pid == countyid).Select(x => new { x.ID, x.Name });
                return new JavaScriptSerializer().Serialize(list);
            }
        }
        /// <summary>
        /// 得到网格
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetGrids(int userid)
        {

            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Users.FirstOrDefault(x => x.ID == userid);
                var areaslist = db.Region.FirstOrDefault(x => x.Name == list.Areas);
                var grids = db.Region.Where(x => x.Pid == areaslist.ID).ToList();
                return new JavaScriptSerializer().Serialize(grids);

            }
        }
    }
}