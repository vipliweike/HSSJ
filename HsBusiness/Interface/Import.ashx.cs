using Common;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace HsBusiness.Interface
{
    /// <summary>
    /// Import 的摘要说明
    /// </summary>
    public class Import : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpPostedFile filePost = context.Request.Files["file"]; // 获取上传的文件 
            string filename = "";
            string filePath = SaveExcelFile(filePost, out filename); // 保存文件并获取文件路径  
            string msg = ExcelToDataTable(filePath, true, filename);
            context.Response.Write(msg);

        }
        /// <summary>  
        /// 保存Excel文件  
        /// <para>Excel的导入导出都会在服务器生成一个文件</para>  
        /// <para>路径：UpFiles/ExcelFiles</para>  
        /// </summary>  
        /// <param name="file">传入的文件对象</param>  
        /// <returns>如果保存成功则返回文件的位置;如果保存失败则返回空</returns>  
        public static string SaveExcelFile(HttpPostedFile file, out string filename)
        {
            try
            {
                var fileName = file.FileName.Insert(file.FileName.LastIndexOf('.'), "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload"), fileName);
                string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                file.SaveAs(filePath);
                filename = fileName;
                return filePath;
            }
            catch
            {
                filename = "";
                return string.Empty;
            }
        }
        /// <summary>  
        /// 上传读取文件  
        /// </summary>  
        /// <param name="filePath"></param>  
        /// <param name="isColumnName"></param>  
        public string ExcelToDataTable(string filePath, bool isColumnName, string fileName)
        {
            int count = 0;
            try
            {


                DataTable dataTable = new DataTable();
                FileStream fs = null;
                IWorkbook workbook = null;
                ISheet sheet = null;
                // var result = "";

                using (fs = new FileStream(filePath, FileMode.Open))
                {
                    if (filePath.IndexOf(".xlsx") > 0)
                    {
                        workbook = new XSSFWorkbook(fs);
                        if (workbook != null)
                        {
                            sheet = workbook.GetSheetAt(0);
                            if (sheet != null)
                            {
                                using (dbDataContext db = new dbDataContext())
                                {
                                    int rowCount = sheet.LastRowNum;
                                    for (int i = 1; i <= rowCount; i++)
                                    {

                                        IRow row = sheet.GetRow(i);
                                        Users model = new Users();

                                        model.Name = row.GetCell(0).ToString();
                                        model.Mobile = row.GetCell(1).ToString();
                                        var role = db.Roles.Where(x => x.RoleName == row.GetCell(2).ToString()).FirstOrDefault();
                                        if (role == null)
                                        {
                                            return JsonConvert.SerializeObject(new { msg = "导入的角色错误", state = 2 });
                                            //result = JsonConvert.SerializeObject(new { msg = "导入的角色错误", state = 2 });
                                        }
                                        string pwd = Interface.Comm.MD5.Encrypt("123456", 32);
                                        model.Pwd = pwd;
                                        model.Roles = role;
                                        model.Areas = row.GetCell(3) == null ? string.Empty : row.GetCell(3).ToString();
                                        if (model.Roles.RoleName == "公司领导" || model.Roles.RoleName == "政企部主管")
                                        {
                                            model.Areas = "衡水市";
                                        }
                                        model.Grids = row.GetCell(4) == null ? string.Empty : row.GetCell(4).ToString();
                                        model.Post = row.GetCell(5) == null ? string.Empty : row.GetCell(5).ToString();

                                        count++;

                                        db.Users.InsertOnSubmit(model);

                                    }
                                    db.SubmitChanges();
                                }

                            }

                        }
                    }
                }
            }
            catch { return "导入失败，字段错误！"; }
            finally
            {


                FileInfo fi = new FileInfo(fileName);
                //文件是否存在
                if (System.IO.File.Exists(filePath))
                {
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;

                    System.IO.File.Delete(filePath);
                }
            }
            return JsonConvert.SerializeObject(new
            {
                msg = "成功导入" + count + "条数据",
                state = 1
            });
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}