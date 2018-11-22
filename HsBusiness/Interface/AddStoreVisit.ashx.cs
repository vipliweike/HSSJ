using HsBusiness.Interface.Comm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace HsBusiness.Interface
{
    /// <summary>
    /// AddVisit 的摘要说明
    /// </summary>
    public class AddStoreVisit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var data = context.Request.Form["data"];
            var storeVisit= context.Request.Form["storeVisit"];
            this.Files = context.Request.Files;
            var model = JsonConvert.DeserializeObject<Store>(data);
            var vis = JsonConvert.DeserializeObject<StoreVisit>(storeVisit);
            if (!string.IsNullOrEmpty(Common.TCContext.Current.OnlineUserID))
            {
                using (dbDataContext db = new dbDataContext())
                {
                    var item = new Store();
                    item.StoreName = model.StoreName;
                    item.StoreAddress = model.StoreAddress;
                    item.Broadband = model.Broadband;
                    item.Price = model.Price;
                    item.OtherNeeds = model.OtherNeeds;
                    item.State = 0;
                    item.OverTime = model.OverTime;
                    item.ContactName = model.ContactName;
                    item.ContactTel = model.ContactTel;
                    item.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    item.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    item.UserID = int.Parse(Common.TCContext.Current.OnlineUserID);

                    //图片
                    var names = "";//
                    if (this.Files.Count > 0)
                    {
                        var result = Images_C("0", "Images", 0).Select(t => t).ToArray();
                        names = string.Join(";", result);
                        item.Img = names;
                    }

                    vis.Store = item;
                    vis.VisitTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    vis.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    vis.State = 0;
                    
                    db.Store.InsertOnSubmit(item);
                    db.StoreVisit.InsertOnSubmit(vis);
                    db.SubmitChanges();


                    context.Response.Write(JsonConvert.SerializeObject(new { data = names, state = 1, msg = "添加成功" }));
                }

            }
            else
            {
                context.Response.Write(JsonConvert.SerializeObject(new { state = 0, msg = "添加失败，请重新登录" }));
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
                //string path = "http://124.239.149.190:8001";
                var path = System.Configuration.ConfigurationManager.AppSettings["OthorWebSite"].ToString();//项目根目录
                //string path1 = HttpContext.Current.Request.PhysicalApplicationPath;
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}