using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace HsBusiness.Api.Common
{
    public static class WebHelper
    {
        #region 获取路径


        public static IEnumerable<string> GetFileUrls(string images)
        {
            if (!string.IsNullOrEmpty(images))
            {
                foreach (var img in images.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    yield return GetFileUrl(img);
                }
            }
        }

        public static string GetFileUrl(string img)
        {
            if (string.IsNullOrEmpty(img)) return img;
            return WebHelper.ApplicationPath + img;
        }

        public static string GetFilePath(string file)
        {
            var context = HttpContext.Current;
            if (context == null) return file;
            return context.GetFilePath(file);
        }

        public static string GetFilePath(this HttpContext context, string path)
        {
            return System.IO.Path.Combine(context.Request.PhysicalApplicationPath, path).Replace('/', '\\');
        }

        public static string GetFileUrl(this HttpContext context, string file)
        {
            return "http://" + context.Request.Url.Authority + context.Request.ApplicationPath + file;
        }

        /// <summary>
        /// 获取应用程序物理根路径
        /// </summary>
        public static string PhysicalApplicationPath
        {
            get { return HttpContext.Current.Request.PhysicalApplicationPath; }
        }

        /// <summary>
        /// 获取应用程序虚拟根路径
        /// </summary>
        public static string ApplicationPath
        {
            get { return "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath; }
        }

        #endregion

        #region image

        public static readonly string[] IMG_TYPES = { "jpg", "png", "bmp" };

        public static bool vimg(string file)
        {
            return IMG_TYPES.Contains(Path.GetExtension(file).ToLower());
        }

        public static string GetImageUrl(string img)
        {
            return GetFileUrl(img);
        }

        //public static string GetImagePath(string img)
        //{
        //    return GetFilePath(img);
        //}

        #endregion

        #region HttpPostedFile upload

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="hifile">控件</param>
        /// <param name="path">中间路径</param>
        /// <returns>返回长路径</returns>
        public static string UploadMD5Name(this FileUpload hifile, string path)
        {
            if (hifile.HasFile)
            {
                string fileName = (DateTime.Now.ToString("yyyyMMddHHmmssfff") + hifile.FileName).ToMD5();
                return Upload(hifile, path, fileName);
            }
            return string.Empty;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="hifile">控件</param>
        /// <param name="path">中间路径</param>
        /// <returns>返回长路径</returns>
        public static string Upload(this FileUpload hifile, string path)
        {
            if (hifile.HasFile)
            {
                string name = Path.GetFileNameWithoutExtension(hifile.FileName);
                return Upload(hifile, path, name);
            }
            return null;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="hifile">控件</param>
        /// <param name="path">中间路径</param>
        /// <returns>返回长路径</returns>
        public static string Upload(this FileUpload hifile, string path, string fileName)
        {
            if (hifile.HasFile)
            {
                return hifile.PostedFile.Upload(path, fileName);
            }
            return null;
        }

        //public static string UpLoadFileByBase64(string filestr, string dir)
        //{
        //    string top_dir = System.IO.Path.Combine(WebHelper.PhysicalApplicationPath, dir);
        //    if (!Directory.Exists(top_dir)) Directory.CreateDirectory(top_dir);

        //    byte[] buffer = Convert.FromBase64String(filestr);

        //    using (var ms = new MemoryStream(buffer))
        //    {
        //        var img = System.Drawing.Image.FromStream(ms);
        //        string name = TopUtils.GetMD5FileNameByExtension(".png");
        //        img.Save(Path.Combine(top_dir, name), System.Drawing.Imaging.ImageFormat.Png);
        //        return System.IO.Path.Combine(dir, name).Replace(@"\", "/");
        //    }
        //}

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="hifile">控件</param>
        /// <param name="path">中间路径</param>
        /// <returns>返回长路径</returns>
        public static string Upload(this HttpPostedFile hifile, string path)
        {
            string name = Path.GetFileNameWithoutExtension(hifile.FileName);
            return Upload(hifile, path, name);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="hifile">控件</param>
        /// <param name="path">中间路径</param>
        /// <returns>返回长路径</returns>
        public static string Upload(this HttpPostedFile hifile, string path, string fileName)
        {
            string fullName = fileName + Path.GetExtension(hifile.FileName);
            string longPath = Path.Combine(PhysicalApplicationPath, path);

            if (!Directory.Exists(longPath)) Directory.CreateDirectory(longPath);
            hifile.SaveAs(Path.Combine(longPath, fullName));
            return Path.Combine(path, fullName).Replace('\\', '/');
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="hifile">控件</param>
        /// <param name="path">中间路径</param>
        /// <returns>返回长路径</returns>
        public static string UploadWithExtension(this HttpPostedFile hifile, string path, string fullName)
        {
            string longPath = Path.Combine(PhysicalApplicationPath, path);

            try
            {
                if (!Directory.Exists(longPath)) Directory.CreateDirectory(longPath);
                hifile.SaveAs(Path.Combine(longPath, fullName));
                return Path.Combine(path, fullName).Replace('\\', '/');
            }
            catch //(Exception ex)
            {
                //SqlHelp.SaveLog.WriteLog("出现异常：" + ex.Message + ";" + ex.StackTrace + "; 路径str = " + longPath);
            }
            return null;
        }

        public static IEnumerable<HttpPostedFile> GetFiles(this HttpFileCollection files, int max_length, params string[] filter)
        {
            if (files != null && files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var extension = System.IO.Path.GetExtension(file.FileName);

                    if (max_length == 0 || (max_length > 0 && file.ContentLength <= max_length))
                    {
                        if (filter == null || filter.Length == 0)
                        {
                            yield return file;
                        }
                        else
                        {
                            foreach (var ext in filter)
                            {
                                if (string.Equals(ext, extension, StringComparison.OrdinalIgnoreCase))
                                {
                                    yield return file;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<string> UploadFilesMD5(this HttpFileCollection files, string path, int max_length, params string[] filter)
        {
            var fs = files.GetFiles(max_length, filter);
            foreach (var file in fs)
            {
                string name = (DateTime.Now.Ticks + file.FileName);
                yield return file.Upload(path, name);
            }
        }

        /// <summary>
        /// 截取文件后缀名
        /// </summary>
        /// <param name="hifile"></param>
        /// <returns></returns>
        public static string GetFileType(FileUpload hifile)
        {
            if (hifile.HasFile)
            {
                string fileName = hifile.PostedFile.FileName;
                return fileName.Substring(fileName.LastIndexOf("."));
            }
            return null;
        }

        public static string UserUpImages(FileUpload hifile, string folder)
        {
            string str = PhysicalApplicationPath;
            string fileType = GetFileType(hifile);
            string imgname = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileType;
            string str3 = folder + "/" + imgname;
            try
            {
                if (!Directory.Exists(PhysicalApplicationPath + folder))
                    Directory.CreateDirectory(PhysicalApplicationPath + folder);
                hifile.PostedFile.SaveAs(str + str3);

                return imgname;
            }
            catch
            {
                return "no";
            }
        }

        #endregion

        public static void MessageBoxShow(this System.Web.UI.Page page, string msg)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "alert", "<script>alert('" + msg + "')</script>");
        }

        public static void MessageBoxShowAndRedirect(this System.Web.UI.Page page, string msg, string url)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "alert", "<script>alert('" + msg + "');window.location.href =" + url + ";</script>");
        }

        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转（顶级菜单）
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirectTop(System.Web.UI.Page page, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.AppendFormat("window.top.location.href='{0}'", url);
            Builder.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }
    }
}