using Common;
using HsBusiness.Interface.Comm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.PersonManage
{
    public partial class PersonTable : Interface.Comm.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string Get(string Areas, string Post, string Search, string pageIndex)
        {
            using (dbDataContext db = new dbDataContext())
            {

                int pageSize = 10;
                var list = db.Users.Where(x => x.Name != "admin").Select(x => new { x.ID, x.Areas, x.AddTime, x.Grids, x.Mobile, x.Name, x.Post, x.Pwd, x.Remark });
                if (!string.IsNullOrEmpty(Search))
                {
                    list = list.Where(t => t.Name.Contains(Search) || t.Mobile.Contains(Search));
                }
                if (!string.IsNullOrEmpty(Areas))
                {
                    list = list.Where(t => t.Areas == Areas);
                }
                if (!string.IsNullOrEmpty(Post))
                {
                    list = list.Where(t => t.Post == Post);
                }
                var list1 = list.Skip((int.Parse(pageIndex) - 1) * pageSize).Take(pageSize);

                var result = new { status = "1", data = list1, pagecount = list.Count().ToString(), pagesize = pageSize.ToString() };
                string list2 = new JavaScriptSerializer().Serialize(result);
                return list2;
            }

        }
        [WebMethod]
        /// <summary>
        /// 获取一条数据（修改时）
        /// </summary>
        /// <returns></returns>

        public static string GetOne(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Users.Where(x => x.ID == ID).Select(x => new { x.ID, x.Mobile, x.Name, x.Grids, x.Post, x.Pwd, x.Remark, x.Areas, x.Roles.RoleName, x.RoleID }).FirstOrDefault();
                string list1 = new JavaScriptSerializer().Serialize(list);
                return list1;
            }

        }
        [WebMethod]
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="Mobile"></param>
        /// <param name="Post"></param>
        /// <param name="Areas"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public static string Edit(int ID, string Name, string Mobile, string Post, string Areas, string Remark, int roleid, string Grids)

        {

            using (var db = new dbDataContext())
            {
                var item = db.Users.FirstOrDefault(x => x.ID == ID);
                var result = "";
                if (item != null)
                {
                    item.ID = ID;
                    item.Name = Name;
                    item.Mobile = Mobile;
                    item.Post = Post;
                    //item.Areas = Areas;
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
                    item.Remark = Remark;
                    item.RoleID = roleid;
                    //item.Grids = Grids;
                    db.SubmitChanges();
                    result = JsonConvert.SerializeObject(new { msg = "修改成功", state = 1 });
                }
                else
                {
                    result = JsonConvert.SerializeObject(new { msg = "修改失败", state = 0 });
                }

                return result;

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
                var person = db.Users.FirstOrDefault(x => x.ID == id);//用户信息
                //var visitlist = db.Visit.Where(x => x.UserID == id).ToList();//拜访记录
                //var buinesslist = db.Business.FirstOrDefault(x=>x.UserID==id);//商机信息
                //var contactlist = db.Contacts.Where(x => x.BusID == buinesslist.ID).ToList();
                //db.Visit.DeleteAllOnSubmit(visitlist);
                //db.Contacts.DeleteAllOnSubmit(contactlist);
                //db.Business.DeleteOnSubmit(buinesslist);
                db.Users.DeleteOnSubmit(person);
                OperateLog opermodel = new OperateLog();
                opermodel.Operator = person.Name;//操作人
                opermodel.OperTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//操作时间
                opermodel.OperType = "删除";//操作类型
                opermodel.Operdescribe = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + person.Name + "进行了单个删除操作";
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
                var item = db.Users.Where(x => intArray.Contains(x.ID)).ToList();
                db.Users.DeleteAllOnSubmit(item);
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
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string ResetPwd(int id)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var result = "";
                try
                {
                    var list = db.Users.FirstOrDefault(x => x.ID == id);
                    string pwd = Interface.Comm.MD5.Encrypt("123456", 32);
                    list.Pwd = pwd;
                    db.SubmitChanges();
                    result = JsonConvert.SerializeObject(new { msg = "重置成功", state = 1 });
                    return result;
                }
                catch (Exception ex)
                {
                    result += ex.Message;

                }

                return result;
            }
        }
        static readonly string[] COLUMNS = { "*姓名", "*手机号", "*角色", "所属区县", "所属网格", "岗位", "备注" };


        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string Import(HttpContext context)
        {
            dbDataContext db = new dbDataContext();
            //HttpPostedFile
            HttpPostedFile file = HttpContext.Current.Request.Files["file"];//接收客户端传递过来的数据.
            HttpFileCollection files = context.Request.Files;

            var result = "";
            var filename = file.FileName;
            if (file == null)
            {
                result = JsonConvert.SerializeObject(new { msg = "请选择上传的excel", state = 2 });
            }

            StringBuilder errorMsg = new StringBuilder(); // 错误信息
            try
            {

                #region 1.获取Excel文件并转换为一个List集合

                // 1.1存放Excel文件到本地服务器
                // HttpPostedFile filePost = context.Request.Files["filed"]; // 获取上传的文件
                string filePath = ImportHelper.SaveExcelFile(file); // 保存文件并获取文件路径

                // 单元格抬头
                // key：实体对象属性名称，可通过反射获取值
                // value：属性对应的中文注解
                Dictionary<string, string> cellheader = new Dictionary<string, string> {
                    { "Name", "姓名" },
                    { "Mobile", "手机号" },
                    { "RoleID", "角色" },
                    { "Areas", "所属区县" },
                    { "Grids", "所属网格" },
                    { "Post", "岗位" },
                   // { "Grids", "备注" },
                };

                // 1.2解析文件，存放到一个List集合里
                List<Users> enlist = ImportHelper.ExcelToEntityList<Users>(cellheader, filePath, out errorMsg);
                if (enlist.Count == 0)
                {
                    result = JsonConvert.SerializeObject(new { msg = "导入失败", state = 0 });

                }
                #region 2.2检测Excel中是否有重复对象

                var list = db.Users.Where(x => 1 == 1).ToList();
                for (int i = 0; i < enlist.Count; i++)
                {
                    Users enA = enlist[i];

                    for (int j = 0; j < list.Count; j++)
                    {
                        Users enB = list[j];
                        if (enA.Name == enB.Name)
                        {
                            //enA.IsExcelVaildateOK = false;
                            //enB.IsExcelVaildateOK = false;
                            errorMsg.AppendLine("EXCEL表中第" + (i + 1) + "行与第" + (j + 1) + "行的数据重复了");
                            //return Json(new { state = 2, msg = errorMsg.ToString() });
                            result = JsonConvert.SerializeObject(new { msg = errorMsg.ToString(), state = errorMsg.ToString() });
                        }
                    }

                }

                #endregion

                // TODO：其他检测

                #endregion

                // 3.TODO：对List集合进行持久化存储操作。如：存储到数据库

                db.Users.InsertAllOnSubmit(enlist);
                db.SubmitChanges();
                // 4.返回操作结果
                bool isSuccess = false;
                if (errorMsg.Length == 0)
                {
                    isSuccess = true; // 若错误信息成都为空，表示无错误信息
                    //delImg(filename);

                    result = JsonConvert.SerializeObject(new { msg = "导入成功", state = 1 });

                }
                result = JsonConvert.SerializeObject(new { state = 0, msg = errorMsg.ToString() });

            }
            catch (Exception ex)
            {

                result = JsonConvert.SerializeObject(new { state = 0, msg = "导入失败" + ex });
            }
            return result;

        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool delImg(string filename)
        {
            try
            {
                string filePath = Server.MapPath("~/UpFiles/ExcelFiles/");
                var path = Path.Combine(filePath, filename);
                FileInfo fi = new FileInfo(filePath);
                //文件是否存在
                if (System.IO.File.Exists(path))
                {
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;

                    System.IO.File.Delete(path);
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        /// <summary>
        /// 查询所有区县
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetRegion()
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Region.Where(x => db.Region.Where(y => y.Pid == 0).Select(y => y.ID).Contains((int)x.Pid))
                    .Select(x => new
                    {
                        x.Name
                    }).ToList();
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                return result;
            }
        }
    }
}