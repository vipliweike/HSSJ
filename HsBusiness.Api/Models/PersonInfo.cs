using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HsBusiness.Api.Models
{
    public class PersonInfo
    {
        public int ID { get; set; }//个人信息编号
        public string Name { get; set; }//姓名
        public string Mobile { get; set; }//手机号
        public string Areas { get; set; }//区县
        public string Post { get; set; }//岗位
    }
}