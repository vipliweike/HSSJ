using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness.Businesss
{
    public partial class InformationNewly : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [WebMethod]        
        public static string GetOne(int ID)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var list = db.Business.Where(x => x.ID == ID).Select(x=>new {
                    x.ID,
                    Contacts=x.Contacts.Select(y=>new {y.ID,y.Name,y.Post,y.Tel}),
                    x.CompanyName,
                    x.Industry,
                    x.CustomerScale,
                    x.Areas,
                    x.Remark

                }).FirstOrDefault();
                var result = JsonConvert.SerializeObject(list);
                return result;
            }
        }

        [WebMethod]
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public static string Modify(int ID,string Areas,string CompanyName,string Industry,string CustomerScale,string Remark,string Contacts)
        {
            using (dbDataContext db = new dbDataContext())
            {
                var result = "";
                var model = db.Business.Where(x => x.ID == ID).FirstOrDefault();
                var ContactsList = JsonConvert.DeserializeObject<List<Contacts>>(Contacts);

                db.Contacts.InsertAllOnSubmit(ContactsList);

                if (model != null)
                {
                    model.Areas = Areas;
                    model.Industry = Industry;
                    model.CustomerScale = CustomerScale;
                    model.CompanyName = CompanyName;
                    model.Remark = Remark;
                    if(ContactsList.Count > 0)
                    {
                        foreach (var item in ContactsList)
                        {
                            item.BusID = model.ID;
                        }
                        var oldContacts = db.Contacts.Where(x => x.BusID == model.ID).ToList();
                        db.Contacts.DeleteAllOnSubmit(oldContacts);//删除旧联系人
                        db.Contacts.InsertAllOnSubmit(ContactsList);//添加新联系人
                    }
                   

                    db.SubmitChanges();
                    result = JsonConvert.SerializeObject(new { msg = "修改成功", state = 1 });
                }else
                {
                    result = JsonConvert.SerializeObject(new { msg = "修改失败", state = 0 });
                }
                
                return result;
            }
        }
    }
}