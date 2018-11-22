using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HsBusiness.Api.Models
{
    public class ModifyPwdModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPwd { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPwd { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string RepeatPwd { get; set; }

    }
}