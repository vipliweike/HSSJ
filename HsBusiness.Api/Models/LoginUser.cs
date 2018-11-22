using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HsBusiness.Api.Models
{
    public class LoginUser
    {
        /// <summary>
        /// 登录手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Pwd { get; set; }
    }
}