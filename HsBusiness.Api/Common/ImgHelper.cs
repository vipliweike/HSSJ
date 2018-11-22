using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HsBusiness.Api.Common
{
    public static class ImgHelper
    {
        /// <summary>
        /// 给图片加上全路径
        /// </summary>
        /// <param name="imgname"></param>
        /// <returns></returns>
        public static string[] GetImgAllPath(string imgname)
        {
            string[] list = { };
            if (imgname != null && imgname != "")
            {
                list = imgname.Split(';');
                if (list.Length > 0)
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        list[i] = PublicConst.ImgFileAllPath + list[i];
                    }
                }
            }
            return list;
        }

        

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="FileName"></param>
        public static void DelFile(string FilesName)
        {
            try
            {
                if (FilesName != null && FilesName != "")
                {
                    string[] namePrefix = {"", "min", "logo" };//无前缀  、min 、logo
                    string filePath = HttpContext.Current.Server.MapPath(PublicConst.ImgFilePath);
                    var list = FilesName.Split(';');
                    if (list.Length > 0)
                    {
                        FileInfo fi = new FileInfo(filePath);
                        for (int i = 0; i < list.Length; i++)
                        {
                            for (int j = 0; j < namePrefix.Length; j++)
                            {
                                var path = Path.Combine(filePath, namePrefix[j]+FilesName);

                                //文件是否存在
                                if (System.IO.File.Exists(path))
                                {
                                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                                        fi.Attributes = FileAttributes.Normal;

                                    System.IO.File.Delete(path);
                                }
                            }                           
                        }
                    }
                    
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}