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
    /// ImportBalance 的摘要说明
    /// </summary>
    public class ImportBalance : IHttpHandler
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
                                dbDataContext db = new dbDataContext();
                                int rowCount = sheet.LastRowNum;
                                for (int i = 1; i <= rowCount; i++)
                                {

                                    IRow row = sheet.GetRow(i);
                                    SmallBalance model = new SmallBalance();
                                    SmaBaRemind remind = new SmaBaRemind();
                                    // 接入号 帐号ID    维系人 责任区域    余额 客户名称    集团ID 集团名称    入网时间 周价  带宽 装机地址    联系人 联系电话
                                    if (row.GetCell(2) != null ? true : !string.IsNullOrEmpty(row.GetCell(2).ToString()))
                                    {
                                        var user = db.Users.Where(x => x.Name == row.GetCell(2).ToString()).FirstOrDefault();
                                        if (user == null)
                                        {
                                            return JsonConvert.SerializeObject(new { msg = "第" + (count + 2) + "行维系人不存在", state = 2 });
                                        }
                                        else
                                        {
                                            model.AccessNumber = row.GetCell(0).ToString();
                                            model.AccountID = row.GetCell(1).ToString();
                                            model.UserID = user.ID;
                                            model.Responsibility = row.GetCell(3).ToString();
                                            model.Balance = row.GetCell(4).ToString();
                                            model.CustomerName = row.GetCell(5).ToString();
                                            model.GroupID = row.GetCell(6) == null ? string.Empty : row.GetCell(6).ToString();
                                            model.GroupName = row.GetCell(7) == null ? string.Empty : row.GetCell(7).ToString();
                                            model.NetTime = row.GetCell(8).DateCellValue.ToString("yyyy-MM-dd");//Convert.ToDateTime(row.GetCell(8).ToString()).ToString("yyyy-MM-dd")
                                            model.WeekPrice = row.GetCell(9).ToString();
                                            model.Broadband = row.GetCell(10).ToString();
                                            model.InstalledAddress = row.GetCell(11).ToString();
                                            model.Contacts = row.GetCell(12).ToString();
                                            model.ContactTel = row.GetCell(13).ToString();
                                            model.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            model.State = 1;

                                            //提醒
                                            remind.AddTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            remind.State = 0;
                                            remind.SmallBalance = model;
                                        }
                                    }
                                    else
                                    {
                                        return JsonConvert.SerializeObject(new { msg = "第" + (count + 2) + "行未指定维系人", state = 2 });
                                    }
                                    count++;
                                    db.SmallBalance.InsertOnSubmit(model);
                                    db.SmaBaRemind.InsertOnSubmit(remind);
                                }
                                db.SubmitChanges();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            { return JsonConvert.SerializeObject(new { msg = "第" + (count + 2) + "导入失败，字段错误！", state = 0 }); }
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