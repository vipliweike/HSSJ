using HsBusiness.Api.Common;
using HsBusiness.Api.Filter;
using HsBusiness.Api.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HsBusiness.Api.Controllers
{
    public class StoreController : ApiController
    {
        /// <summary>
        /// 门店添加
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

                using (dbDataContext db = new dbDataContext())
                {
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var user = db.Users.FirstOrDefault(x => x.ID == UserID);
                    if (user != null)
                    {
                        var model = new Store();

                        model.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        model.StoreName = parameters["StoreName"];
                        model.Broadband = parameters["Broadband"];
                        model.ContactName = parameters["ContactName"];
                        model.ContactTel = parameters["ContactTel"];
                        model.OverTime = parameters["OverTime"];
                        model.Price = parameters["Price"];
                        model.OtherNeeds = parameters["OtherNeeds"];
                        model.StoreAddress = parameters["StoreAddress"];
                        model.State = 0;
                        model.Users = user;
                        //最近更新时间
                        model.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        var names = "";//
                        if (this.Files.Count > 0)
                        {
                            var result = Images_C("0", "Images", 0).Select(t => t).ToArray();
                            names = string.Join(";", result);
                            model.Img = names;
                        }
                        //走访内容
                        var storeVisit = new StoreVisit();
                        storeVisit.IsAgain = Convert.ToInt32(parameters["IsAgain"]);
                        storeVisit.IsNeed = Convert.ToInt32(parameters["IsNeed"]);
                        storeVisit.NextTime = parameters["NextTime"];
                        storeVisit.VisitContent = parameters["TalkInfo"];//本次洽谈内容                        
                        storeVisit.VisitTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //parameters["VisitTime"];
                        storeVisit.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        storeVisit.State = 0;
                        storeVisit.Store = model;

                        db.StoreVisit.InsertOnSubmit(storeVisit);
                        db.Store.InsertOnSubmit(model);
                        db.SubmitChanges();
                        return Json(new { state = 1, msg = "添加成功", });
                    }
                    return Json(new { state = 0, msg = "该用户不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 门店修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Modify()
        {
            try
            {
                var context = HttpContext.Current;//当前
                var request = context.Request;//请求
                var form = request.Form;//参数
                this.Files = request.Files;//文件

                //真实的参数
                var parameters = Common.AesDecryp.GetAesDecryp(form["data"], form["secret"]);

                using (dbDataContext db = new dbDataContext())
                {
                    var StoreID = Convert.ToInt32(parameters["StoreID"]);
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var user = db.Users.FirstOrDefault(x => x.ID == UserID);//用户
                    var model = db.Store.Where(x => x.ID == StoreID).FirstOrDefault();//门店
                    if (user != null)
                    {
                        model.StoreName = parameters["StoreName"];
                        model.Broadband = parameters["Broadband"];
                        model.ContactName = parameters["ContactName"];
                        model.ContactTel = parameters["ContactTel"];
                        model.OverTime = parameters["OverTime"];
                        model.Price = parameters["Price"];
                        model.Users = user;
                        //最近更新时间
                        model.LastTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        #region 状态判断

                        if (model.State == 0)//正常
                        {
                            //正常修改不处理
                        }
                        else if (model.State == 1)//已派单
                        {
                            model.State = 0;
                            var sr = model.StoreRemind.Where(x => x.Type == 1).OrderByDescending(x => x.AddTime).FirstOrDefault();//最新提醒
                            if (sr != null)
                            {
                                sr.State = 2;//未处理
                            }
                        }
                        else if (model.State == 2)//已回执
                        {
                            model.State = 0;//正常
                        }

                        #endregion


                        ////删除原有图片
                        //ImgHelper.DelFile(model.Img);

                        var names = "";//
                        if (this.Files.Count > 0)
                        {
                            var result = Images_C("0", "Images", 0).Select(t => t).ToArray();
                            names = string.Join(";", result);
                            model.Img = names;
                        }

                        db.SubmitChanges();
                        return Json(new { state = 1, msg = "修改成功", });
                    }
                    return Json(new { state = 0, msg = "该用户不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    //真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var PageIndex = Convert.ToInt32(parameters["PageIndex"]);
                    var PageSize = Convert.ToInt32(parameters["PageSize"]);
                    var Search = parameters["Search"];
                    var Broadband = parameters["Broadband"];
                    var Areas = parameters["Areas"];
                    var UserID = Convert.ToInt32(parameters["UserID"]);
                    var user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                    if (user != null)
                    {
                        var list = db.Store.Where(x => 1 == 1);
                        #region 角色判断
                        if (user.Roles.RoleName == "公司领导" || user.Roles.RoleName == "政企部主管")
                        {
                            list = list.Where(x => 1 == 1);//公司领导、政企部主管、行业经理,都能看
                        }
                        else
                        if (user.Roles.RoleName == "区县经理")//区县经理，能看本区县
                        {
                            list = list.Where(x => x.Users.Areas == user.Areas);
                        }
                        else
                        if (user.Roles.RoleName == "客户经理" || user.Roles.RoleName == "网格助理" || user.Roles.RoleName == "行业经理")//客户经理、网格助理、行业经理，能看自己的
                        {
                            list = list.Where(x => x.UserID == UserID);
                        }
                        else
                        {
                            list = list.Where(x => x.ID < 0);//无数据
                        }
                        #endregion
                        #region 查询                       
                        if (!string.IsNullOrEmpty(Broadband))//宽带运营商查询
                        {
                            list = list.Where(x => x.Broadband.Contains(Broadband));
                        }
                        if (!string.IsNullOrEmpty(Areas))//区县查询
                        {
                            list = list.Where(x => x.Users.Areas.Contains(Areas));
                        }
                        if (!string.IsNullOrEmpty(Search))//通过门店名称、联系人姓名、联系人电话查询
                        {
                            list = list.Where(x => x.StoreName.Contains(Search) || x.ContactName.Contains(Search) || x.ContactTel.Contains(Search) || x.Users.Name.Contains(Search));
                        }
                        #endregion
                        var resultlist = list
                            .Select(x => new
                            {
                                x.ID,
                                x.StoreName,//门店名称
                                x.AddTime,//建档时间
                                x.LastTime,//最后修改时间
                                x.Broadband,//宽带
                                Img = ImgHelper.GetImgAllPath(x.Img),//图片
                                Next = x.StoreRemind.Where(y => y.State == 0).OrderByDescending(y => y.AddTime).FirstOrDefault().Type == 1 && x.State == 1 ? "宽带到期"
                                    : x.StoreRemind.Where(y => y.State == 0).OrderByDescending(y => y.AddTime).FirstOrDefault().Type == 2 && x.StoreVisit.OrderByDescending(y => y.VisitTime).FirstOrDefault().State == 1 ? "拜访预约"
                                    : "",//提醒内容（最近的到期提醒和拜访预约）
                            })
                            .OrderByDescending(x => x.Next)//先根据提醒排序                      
                            .ThenByDescending(x => x.LastTime)//再根据最近更新时间排序
                            .Skip((PageIndex > 0 ? PageIndex - 1 : 0) * PageSize).Take(PageSize)//分页
                            .ToList();
                        return Json(new { data = resultlist, state = 1, msg = "请求成功" });
                    }
                    return Json(new { state = 0, msg = "用户不存在" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取单个门店
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetOne(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    //真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);
                    var StoreID = Convert.ToInt32(parameters["StoreID"]);//门店ID
                    var store = db.Store.Where(x => x.ID == StoreID).Select(x => new
                    {
                        x.ID,
                        x.AddTime,
                        x.Broadband,
                        x.ContactName,
                        x.ContactTel,
                        x.LastTime,
                        x.OtherNeeds,
                        x.OverTime,
                        x.Price,
                        x.StoreAddress,
                        x.StoreName,
                        Img = ImgHelper.GetImgAllPath(x.Img),//图片
                        StoreVisitNum = x.StoreVisit.Count,//走访数量
                        //回访记录
                        StoreVisitList = x.StoreVisit.OrderByDescending(y => y.VisitTime).ThenByDescending(y => y.ID).Select(q => new
                        {
                            q.ID,
                            q.IsAgain,
                            q.IsNeed,
                            q.NextTime,
                            q.VisitTime,
                            q.VisitContent,
                            VisitImg = ImgHelper.GetImgAllPath(q.VisitImg),
                            VisitRemindID = q.StoreRemind.Where(t => t.State == 0).OrderByDescending(t => t.ID).Select(t => new { RemindID = t.ID, t.AddTime, }).FirstOrDefault() != null ?
                                            q.StoreRemind.Where(t => t.State == 0).OrderByDescending(t => t.ID).Select(t => new { RemindID = t.ID, t.AddTime, }).FirstOrDefault().RemindID
                                            : 0,//走访提醒
                        }),
                        RemindID = x.StoreRemind.Where(y => y.Type == 1 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault() != null ?
                                   x.StoreRemind.Where(y => y.Type == 1 && y.State == 0).OrderByDescending(y => y.ID).Select(y => new { RemindID = y.ID, y.AddTime, }).FirstOrDefault().RemindID
                                   : 0,//宽带到期提醒


                    }).FirstOrDefault();


                    return Json(new { data = store, state = 1, msg = "请求成功" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 门店名称排重检索
        /// </summary>
        /// <param name="data"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult CheckStoreName(string data, string secret)
        {
            try
            {
                using (dbDataContext db = new dbDataContext())
                {
                    //真实的参数
                    var parameters = Common.AesDecryp.GetAesDecryp(data, secret);

                    var StoreName = parameters["StoreName"];
                    if (!string.IsNullOrEmpty(StoreName))
                    {
                        var list = db.Store.Where(x => x.StoreName.Contains(StoreName)).Select(x => x.StoreName).ToList();
                        if (list.Count > 0)
                        {
                            return Json(new { state = 0, msg = "门店名称重复" });
                        }
                        return Json(new { state = 1, msg = "门店名称可用" });


                    }
                    return Json(new { state = 0, msg = "门店名称为空" });
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
