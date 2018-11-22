using Common;
using HsBusiness.Interface.Comm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace HsBusiness.Interface
{
    /// <summary>
    /// ImportProject 的摘要说明
    /// </summary>
    public class ImportProject : IHttpHandler
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
                                        Project model = new Project();
                                        ProRemind models = new ProRemind();//回执记录

                                        if (row.GetCell(3) != null ? !string.IsNullOrEmpty(row.GetCell(3).ToString()) : false)
                                        {
                                            var fzr = db.Users.Where(x => x.Name == row.GetCell(3).ToString()).FirstOrDefault();//负责人
                                            if (fzr == null)
                                            {
                                                return JsonConvert.SerializeObject(new { msg = "第" + (count + 2) + "行的项目负责人不存在", state = 0 });
                                            }
                                            else
                                            {
                                                //项目表
                                                model.Areas = row.GetCell(0) == null ? string.Empty : row.GetCell(0).ToString();//区县
                                                if (row.GetCell(1).ToString() == "")
                                                {
                                                    return JsonConvert.SerializeObject(new { msg = "第" + (count + 2) + "行的项目简介为必填项", state = 0 });
                                                }
                                                model.Introduce = row.GetCell(1).ToString();
                                                model.Instruction = row.GetCell(2) == null ? string.Empty : row.GetCell(2).ToString(); //领导批示
                                                model.UserID = fzr.ID;
                                                model.State = 1;//已派单
                                                model.IsRead = 0;
                                                model.Type = 0;//非专题项目
                                                model.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                model.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                //项目回执
                                                models.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                models.State = 0;//未回执
                                                models.Type = 1;//派单提醒
                                                models.Project = model;
                                            }
                                        }
                                        else
                                        {
                                            return JsonConvert.SerializeObject(new { msg = "第" + (count + 2) + "行未指定项目负责人", state = 2 });
                                        }

                                        count++;

                                        db.Project.InsertOnSubmit(model);
                                        db.ProRemind.InsertOnSubmit(models);
                                        db.SubmitChanges();
                                        try
                                        {
                                            #region 推送
                                            string pushUrl = "https://bjapi.push.jiguang.cn/v3/push";
                                            string JsonString = new JObject(
                                                                       new JProperty("platform", "all"),
                                                                       new JProperty("audience", new JObject(new JObject(
                                                                           new JProperty("alias", new JArray(model.UserID))
                                                                           ))),
                                                                       new JProperty("notification", new JObject(
                                                                            new JProperty("android", new JObject(
                                                                                new JProperty("alert", "项目派单"),
                                                                                new JProperty("extras",
                                                                                    new JObject(
                                                                                        new JProperty("ID", model.ID),
                                                                                        new JProperty("Type", 2)))))

                                                                            ))
                                                                       ).ToString();

                                            PostHelper.PostData(pushUrl, JsonString);
                                            #endregion
                                        }
                                        catch (Exception ex)
                                        {
                                            SaveLog.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
                                            continue;
                                        }
                                    }
                                    
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex) { return JsonConvert.SerializeObject(new { msg = "导入失败，字段错误！" + (count + 2) + "行错误", state = 0 }); }
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