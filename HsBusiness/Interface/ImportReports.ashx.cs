using Newtonsoft.Json;
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
    /// ImportReports 的摘要说明
    /// </summary>
    public class ImportReports : IHttpHandler
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
                                        Reports model = new Reports();
                                        model.Rank = Convert.ToInt32(row.GetCell(0).ToString());//排名
                                        model.Grids = row.GetCell(1).ToString();
                                        model.Name = row.GetCell(2).ToString();
                                        if (row.GetCell(3).ToString().Length != 6)
                                        {
                                            return JsonConvert.SerializeObject(new { msg = "第" + (count + 2) + "行的月份格式不正确", state = 0 });
                                        }
                                        model.Month = row.GetCell(3).ToString();
                                        var report = db.Reports.Where(x => x.Month == model.Month).FirstOrDefault();
                                        if (report != null)
                                        {
                                            return JsonConvert.SerializeObject(new { msg = "已存在" + model.Month + "月份的数据，请删除后重新导入", state = 0 });
                                        }
                                        //model.StockIncome = row.GetCell(4).ToString();
                                        //model.NewIncome = row.GetCell(5).ToString();
                                        model.TotalIncome = row.GetCell(4).ToString();
                                        model.StockIncomeRate = row.GetCell(5).ToString();
                                        model.R0Aims = row.GetCell(6).ToString();
                                        model.R0CompleteRate = row.GetCell(7).ToString();
                                        model.R2Aims = row.GetCell(8).ToString();
                                        model.R2CompleteRate = row.GetCell(9).ToString();
                                        model.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        if (Common.TCContext.Current.OnlineUserID == "")
                                        {
                                            return JsonConvert.SerializeObject(new { msg = "导入失败，请重新登录后再导入！", state = 0 });
                                        }
                                        model.UserID = int.Parse(Common.TCContext.Current.OnlineUserID);
                                        count++;

                                        db.Reports.InsertOnSubmit(model);

                                    }
                                    db.SubmitChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    msg = "导入失败，格式不正确！",
                    state = 0
                });
            }
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