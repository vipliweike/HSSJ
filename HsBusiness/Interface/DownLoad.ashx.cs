using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace HsBusiness.Interface
{
    /// <summary>
    /// DownLoad 的摘要说明
    /// </summary>
    public class DownLoad : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            var action = context.Request["action"];

            switch (action)
            {
                case "person":
                    string filePath = context.Server.MapPath("/App_Data/人员模板.xlsx");
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Dispose();
                    String filename = HttpUtility.UrlEncode("人员模板.xlsx", System.Text.Encoding.UTF8);    //对中文文件名进行HTML转码
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.BinaryWrite(bytes);
                    context.Response.Flush();
                    break;
                case "reports":
                    string reportsfilePath = context.Server.MapPath("/App_Data/业务报表模板.xlsx");
                    FileStream reportsfs = new FileStream(reportsfilePath, FileMode.Open);
                    byte[] reportsbytes = new byte[reportsfs.Length];
                    reportsfs.Read(reportsbytes, 0, reportsbytes.Length);
                    reportsfs.Dispose();
                    String reportsfilename = HttpUtility.UrlEncode("业务报表模板.xlsx", System.Text.Encoding.UTF8);    //对中文文件名进行HTML转码
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + reportsfilename);
                    context.Response.BinaryWrite(reportsbytes);
                    context.Response.Flush();
                    break;
                case "project":
                    string projectfilePath = context.Server.MapPath("/App_Data/项目派单模版.xlsx");
                    FileStream projectfs = new FileStream(projectfilePath, FileMode.Open);
                    byte[] projectbytes = new byte[projectfs.Length];
                    projectfs.Read(projectbytes, 0, projectbytes.Length);
                    projectfs.Dispose();
                    String projectfilename = HttpUtility.UrlEncode("项目派单模板.xlsx", System.Text.Encoding.UTF8);    //对中文文件名进行HTML转码
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + projectfilename);
                    context.Response.BinaryWrite(projectbytes);
                    context.Response.Flush();
                    break;
                case "balance":
                    string balancefilePath = context.Server.MapPath("/App_Data/存量小余额模板.xlsx");
                    FileStream balancefs = new FileStream(balancefilePath, FileMode.Open);
                    byte[] balancebytes = new byte[balancefs.Length];
                    balancefs.Read(balancebytes, 0, balancebytes.Length);
                    balancefs.Dispose();
                    String balancefilename = HttpUtility.UrlEncode("存量小余额模板.xlsx", System.Text.Encoding.UTF8);    //对中文文件名进行HTML转码
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + balancefilename);
                    context.Response.BinaryWrite(balancebytes);
                    context.Response.Flush();
                    break;
                case "topicsproject":
                    string tprojectfilePath = context.Server.MapPath("/App_Data/专题项目派单模版.xlsx");
                    FileStream tprojectfs = new FileStream(tprojectfilePath, FileMode.Open);
                    byte[] tprojectbytes = new byte[tprojectfs.Length];
                    tprojectfs.Read(tprojectbytes, 0, tprojectbytes.Length);
                    tprojectfs.Dispose();
                    String tprojectfilename = HttpUtility.UrlEncode("专题项目派单模板.xlsx", System.Text.Encoding.UTF8);    //对中文文件名进行HTML转码
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + tprojectfilename);
                    context.Response.BinaryWrite(tprojectbytes);
                    context.Response.Flush();
                    break;
                case "arrears":
                    string arrearsfilePath = context.Server.MapPath("/App_Data/欠费小余额模板.xlsx");
                    FileStream arrears = new FileStream(arrearsfilePath, FileMode.Open);
                    byte[] arrearsbytes = new byte[arrears.Length];
                    arrears.Read(arrearsbytes, 0, arrearsbytes.Length);
                    arrears.Dispose();
                    String arrearsfilename = HttpUtility.UrlEncode("欠费小余额模版.xlsx", System.Text.Encoding.UTF8);    //对中文文件名进行HTML转码
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + arrearsfilename);
                    context.Response.BinaryWrite(arrearsbytes);
                    context.Response.Flush();
                    break;
                default:
                    break;
            }

        }


        public string DownLoadExcel()
        {
            var data = new List<Users>();

            //List<CounterMan> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CounterMan>>(jsonData);
            //2、创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook excel = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet = excel.CreateSheet("Sheet1");
            //给sheet1添加标题行
            NPOI.SS.UserModel.IRow head = sheet.CreateRow(0);
            head.CreateCell(0).SetCellValue("姓名");
            head.CreateCell(1).SetCellValue("手机号");
            head.CreateCell(2).SetCellValue("角色");
            head.CreateCell(3).SetCellValue("所属区县");
            head.CreateCell(4).SetCellValue("所属网格");
            head.CreateCell(5).SetCellValue("岗位");


            ICellStyle HeadercellStyle = excel.CreateCellStyle();
            HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            //字体
            NPOI.SS.UserModel.IFont headerfont = excel.CreateFont();
            headerfont.Boldweight = (short)FontBoldWeight.Bold;
            HeadercellStyle.SetFont(headerfont);
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < data.Count; i++)
            {
                NPOI.SS.UserModel.IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(data[i].Name);
                row.CreateCell(1).SetCellValue(data[i].Mobile);
                row.CreateCell(2).SetCellValue(data[i].Roles.RoleName);
                row.CreateCell(3).SetCellValue(data[i].Areas);
                row.CreateCell(4).SetCellValue(data[i].Grids);
                row.CreateCell(5).SetCellValue(data[i].Post);
                sheet.AutoSizeColumn(i);//自适应行
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            excel.Write(ms);
            //ms.Seek(0, SeekOrigin.Begin);
            ms.Flush();
            var fileName = "Excel_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
            //將生成的文件保存到服務器的臨時目錄里
            var path = HttpContext.Current.Server.MapPath("~/UpFiles/");
            string fullPath = Path.Combine(path, fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] data1 = ms.ToArray();

                fs.Write(data1, 0, data1.Length);
                fs.Flush();

                data1 = null;
            }

            var errorMessage = "you can return the errors in here!";

            //返回生成的文件名
            //return  Newtonsoft.Json.JsonConvert.SerializeObject(new { fileName = fileName, errorMessage = errorMessage });
            return fileName;
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