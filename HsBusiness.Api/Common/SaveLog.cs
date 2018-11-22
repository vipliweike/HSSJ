using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace HsBusiness.Api.Common
{
    public class SaveLog
    {
        private static Object thisLock = new Object();//用于锁定代码，使锁之内的代码排队执行。
        /// <summary>文件型日志。调用方法请粘贴以下代码并作适当修改。文件日志路径不要改变。
        /// SaveLog.WriteLog(AppDomain.CurrentDomain.BaseDirectory+"log\\",  DateTime.Now.ToShortDateString() + "-FB.txt", "事实上\r\n");
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="name"></param>
        /// <param name="strbody"></param>
        /// <returns></returns>
        public static bool WriteLog(string filepath, string name, string strbody)
        {
            StreamWriter my_writer = null;
            lock (thisLock)
            {
                try
                {
                    if (Directory.Exists(filepath) == false)
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    //如果文件存在，则自动追加方式写
                    my_writer = new StreamWriter(filepath + name, true, System.Text.Encoding.Default);
                    my_writer.Write(strbody);
                    my_writer.Flush();
                    return true;

                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (my_writer != null)
                        my_writer.Close();
                }
            }
        }

        /// <summary>
        /// SendSms记录临时信息专用
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="name">文件名</param>
        /// <param name="strbody">删除的sql</param>
        /// <returns></returns>
        public static bool TempLogOut(string path, string name, string strbody)
        {
            StreamReader my_reader = null;
            StreamWriter my_writer = null;
            string str = "";
            lock (thisLock)
            {
                try
                {
                    path = path + name;
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    my_reader = new StreamReader(fs, System.Text.Encoding.GetEncoding("GB2312"));

                    my_reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    str = my_reader.ReadToEnd();
                    str = str.Replace(strbody, "");
                    my_reader.Close();

                    my_writer = new StreamWriter(path, false, System.Text.Encoding.GetEncoding("GB2312"));
                    my_writer.Write(str);
                    my_writer.Flush();
                    my_writer.Close();
                    return true;
                }
                catch
                {
                    if (my_reader != null)
                    {
                        my_reader.Close();
                    }
                    if (my_writer != null)
                    {
                        my_writer.Close();
                    }
                    return false;
                }
                finally
                {
                    if (my_reader != null)
                    {
                        my_reader.Close();
                    }
                    if (my_writer != null)
                    {
                        my_writer.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 服务开启后获取未执行完的sql
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ArrayList TempLogRead(string path, string name)
        {
            StreamReader my_reader = null;
            StreamWriter my_writer = null;
            ArrayList arr = new ArrayList();
            lock (thisLock)
            {
                try
                {
                    path = path + name;
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    my_reader = new StreamReader(fs, System.Text.Encoding.GetEncoding("GB2312"));

                    my_reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    string s = my_reader.ReadLine();
                    while (s != null)
                    {
                        arr.Add(s);
                        s = my_reader.ReadLine();
                    }
                    my_reader.Close();

                    my_writer = new StreamWriter(path, false, System.Text.Encoding.GetEncoding("GB2312"));
                    my_writer.Write("");
                    my_writer.Flush();
                    my_writer.Close();
                    return arr;
                }
                catch
                {
                    if (my_reader != null)
                    {
                        my_reader.Close();
                    }
                    if (my_writer != null)
                    {
                        my_writer.Close();
                    }
                    return arr;
                }
                finally
                {
                    if (my_reader != null)
                    {
                        my_reader.Close();
                    }
                    if (my_writer != null)
                    {
                        my_writer.Close();
                    }
                }
            }
        }


        /// <summary>
        /// 数据库状态打开的情况下赋值一份到新文件，清空本文件
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="name1"></param>
        /// <param name="path2"></param>
        /// <param name="name2"></param>
        public static void ErrorLogSave(string path1, string name1, string path2, string name2)
        {
            StreamWriter my_writer = null;
            lock (thisLock)
            {
                try
                {
                    path1 = path1 + name1;
                    path2 = path2 + name2;
                    FileStream fs = File.OpenRead(path1);
                    if (fs.Length > 0)
                    {
                        File.Copy(path1, path2, true);
                        fs.Close();
                        my_writer = new StreamWriter(path1, false, System.Text.Encoding.GetEncoding("GB2312"));
                        my_writer.Write("");
                        my_writer.Flush();
                        my_writer.Close();
                    }
                }
                catch
                {
                    if (my_writer != null)
                    {
                        my_writer.Close();
                    }
                }
                finally
                {
                    if (my_writer != null)
                    {
                        my_writer.Close();
                    }
                }
            }
        }

        #region 已弃用
        private static string ErrFileDir = AppDomain.CurrentDomain.BaseDirectory + "\\log\\";

        /// <summary>保存日志到application\log201102\20110216\目录下
        /// </summary>
        /// <param name="name"></param>
        /// <param name="strbody"></param>
        /// <returns></returns>
        public static bool WriteLog(string name, string strbody)
        {

            try
            {
                string mm = System.DateTime.Now.ToString(@"yyyyMM");
                string date = System.DateTime.Now.ToString(@"yyyyMMdd");
                date = mm + @"\" + date;
                string fileDir = @ErrFileDir + date + @"\";
                string filepath = fileDir;// +name + ".htm";
                if (Directory.Exists(filepath) == false)
                {
                    Directory.CreateDirectory(filepath);
                }
                return WriteLog(filepath, name, strbody);

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 普通错误日志 按日自动创建文件夹，日志文件名为 Error.htm
        /// </summary>
        /// <param name="strbody">日志详细内容</param>
        /// <returns></returns>
        public static bool WriteLog(string strbody)
        {
            StackTrace ss = new StackTrace(true);
            Type t = ss.GetFrame(1).GetMethod().DeclaringType;
            string MethodName = ss.GetFrame(1).GetMethod().Name.ToString();
            strbody = System.DateTime.Now.ToString() + " " + t.FullName + "." + MethodName + " " + strbody.Replace("\r\n", "<br>") + "<br>";
            return WriteLog("Error.htm", strbody);
        }


        private static bool WriteFormatLog(string strbody, params object[] arg)
        {
            string error = string.Format(strbody, arg);
            return WriteLog(error);
        }
        #endregion
    }
}