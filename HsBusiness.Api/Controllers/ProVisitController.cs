using HsBusiness.Api.Common;
using HsBusiness.Api.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class ProVisitController : ApiController
    {
        /// <summary>
        /// 项目派单走访记录添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Add()
        {
            try
            {
                var context = HttpContext.Current;//当前
                var request = context.Request;//请求
                var form = request.Form;//参数
                this.Files = request.Files;//文件

                //真实的参数
                var parameters = Common.AesDecryp.GetAesDecryp(form["data"], form["secret"]);
                var ProID = Convert.ToInt32(parameters["ProID"]);//项目ID
                var UserID = Convert.ToInt32(parameters["UserID"]);//用户ID
                using (dbDataContext db = new dbDataContext())
                {
                    var pro = db.Project.Where(x => x.ID == ProID).FirstOrDefault();
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    if (pro != null)
                    {
                        if (user != null)
                        {
                            ProVisit model = new ProVisit();

                            model.NextTime = parameters["NextTime"];
                            model.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            model.VisitTime = parameters["VisitTime"];//DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            model.VisitContents = parameters["VisitContent"];
                            model.State = 0;
                            model.Users = user;
                            model.Project = pro;

                            //最近更新时间
                            pro.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            #region 以弃用
                            pro.IsRead = 0;//已读()
                            #endregion

                            #region 最新进度修改未读状态
                            var proReadList = db.ProRead.Where(x => x.ProID == pro.ID);//当前项目派单
                            if (proReadList.Count() > 0)//是否为空
                            {
                                foreach (var item in proReadList)
                                {
                                    var prouser = db.Users.FirstOrDefault(x => x.ID == item.UserID);//当前项目派单已读用户
                                    if (prouser != null)
                                    {
                                        if (prouser.Roles.RoleName == "公司领导" || prouser.Roles.RoleName == "政企部主管")//角色判断
                                        {
                                            if (prouser.ID != UserID)//不是自己
                                            {
                                                item.IsRead = 0;//未读
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                            var names = "";//
                            if (this.Files.Count > 0)
                            {
                                var result = Images_C("0", "Images", 0).Select(t => t).ToArray();
                                names = string.Join(";", result);
                                model.Img = names;
                            }
                            db.ProVisit.InsertOnSubmit(model);

                            db.SubmitChanges();
                            return Json(new { state = 1, msg = "添加成功", });
                        }
                        return Json(new { state = 0, msg = "用户不存在" });
                    }
                    return Json(new { state = 0, msg = "项目不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #region 上传文件      

        private HttpFileCollection Files { get; set; }

        protected IEnumerable<string> GetFiles(string path, int max_length, params string[] filter)
        {
            var files = this.Files.GetFiles(max_length, filter);
            foreach (var file in files)
            {
                string name = (file.FileName);
                yield return file.Upload(path, name);
            }
        }

        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }
        /// <summary>
        /// 图片post处理
        /// </summary>
        /// <param name="types"></param>
        /// <param name="max_length"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private IEnumerable<string> Images_C(string types, string reportNumber, int max_length, params string[] filter)
        {
            string result = "";
            string names = "";
            var files = this.Files.GetFiles(max_length, filter);
            foreach (var file in files)
            {
                //首先先将图片保存
                string path = AppDomain.CurrentDomain.BaseDirectory;
                var format = Path.GetExtension(file.FileName);
                names = DateTime.Now.ToString("yyyyMMddhhmmssfff") + format;
                var savePath = path + "/UploadFile/" + reportNumber + "/";
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                file.SaveAs(Path.Combine(savePath, names));

                if (format.ToUpper().Equals(".MP4"))
                {
                    result = names;
                }
                else
                {
                    System.IO.Stream oStream = file.InputStream;
                    System.Drawing.Image img = System.Drawing.Image.FromStream(oStream);
                    #region 图片剪裁
                    switch (types)
                    {
                        //生成一个小的缩略图（暂定100*100） logo  同时生成一个小一倍的缩略图 min
                        case "0":
                            //生成一个小的缩略图（暂定100*100） logo
                            string newimgname = "logo" + names;

                            decimal oWidth = img.Width; //原图宽度 
                            decimal oHeight = img.Height; //原图高度 
                            decimal dStandard = 150;
                            decimal dWidth = img.Width, dHight = img.Height;

                            //判断图片大小
                            //宽高比对、低于标准原图创建
                            if (dWidth <= dHight)
                            {
                                if (dWidth > dStandard)
                                {
                                    dHight = Math.Round(dHight / (dWidth / dStandard));
                                    dWidth = dStandard;
                                }
                            }
                            else
                            {
                                if (dHight > dStandard)
                                {
                                    dWidth = Math.Round(dWidth / (dHight / dStandard));
                                    dHight = dStandard;
                                }
                            }
                            Bitmap tImage = new Bitmap(int.Parse(dWidth.ToString()), int.Parse(dHight.ToString()));
                            Graphics g = Graphics.FromImage(tImage);

                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; //设置高质量插值法 
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度 
                            g.Clear(Color.Transparent); //清空画布并以透明背景色填充 

                            g.DrawImage(img, new Rectangle(0, 0, int.Parse(dWidth.ToString()), int.Parse(dHight.ToString())), new Rectangle(0, 0, int.Parse(oWidth.ToString()), int.Parse(oHeight.ToString())), GraphicsUnit.Pixel);

                            //图片质量设置
                            EncoderParameter p;
                            EncoderParameters ps;

                            ps = new EncoderParameters(1);

                            p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)100));
                            ps.Param[0] = p;

                            ImageCodecInfo ii = GetCodecInfo("image/jpeg");

                            try
                            {
                                //以JPG格式保存图片 
                                tImage.Save(path + "/UploadFile/" + reportNumber + "/" + newimgname, ii, ps);

                            }
                            catch
                            {
                                result = "";

                                break;
                            }
                            //生成小缩略图结束，生成一个小一倍的缩略图 min
                            newimgname = "min" + names;
                            //int i_s = 400 / oWidth;
                            //tWidth = oWidth *i_s;
                            //tHeight = oHeight * i_s;
                            dStandard = 400;
                            decimal dWidths = img.Width, dHights = img.Height;

                            //判断图片大小
                            //宽高比对、低于标准原图创建
                            if (dWidths <= dHights)
                            {
                                if (dWidths > dStandard)
                                {
                                    dHights = Math.Round(dHights / (dWidths / dStandard));
                                    dWidths = dStandard;
                                }
                            }
                            else
                            {
                                if (dHights > dStandard)
                                {
                                    dWidths = Math.Round(dWidths / (dHights / dStandard));
                                    dHights = dStandard;
                                }
                            }
                            tImage = new Bitmap(int.Parse(dWidths.ToString()), int.Parse(dHights.ToString()));
                            g = Graphics.FromImage(tImage);
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; //设置高质量插值法 
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度 
                            g.Clear(Color.Transparent); //清空画布并以透明背景色填充 

                            g.DrawImage(img, new Rectangle(0, 0, int.Parse(dWidths.ToString()), int.Parse(dHights.ToString())), new Rectangle(0, 0, int.Parse(oWidth.ToString()), int.Parse(oHeight.ToString())), GraphicsUnit.Pixel);


                            // newimgname = "new" + newimgname;
                            try
                            {
                                //以JPG格式保存图片 
                                tImage.Save(path + "/UploadFile/" + reportNumber + "/" + newimgname, ii, ps);

                            }
                            catch
                            {
                                result = "";
                                break;
                            }
                            result = names;
                            //完成，不存入数据库只存原图片，因为缩略图是以原图片加前缀命名的
                            break;
                    }
                    #endregion
                }
                yield return result;
            }

            //存储图片所需的绝对路径

        }
        #endregion 上传文件
    }
}
