using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HsBusiness.Interface.Comm
{
    public class MD5
    {
        ///  <summary> 
        ///  MD5加密算法 
        ///  </summary> 
        ///  <param name="str">字符串</param> 
        ///  <param name="code">加密方式,16或32</param> 
        ///  <returns> </returns> 
        public static string Encrypt(string str, int code)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符）  
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper().Substring(8, 16);
            }
            else//32位加密  
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper();
            }
        }
    }
}