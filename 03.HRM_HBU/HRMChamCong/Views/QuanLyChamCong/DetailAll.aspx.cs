
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;
using System.Web.Script.Services;
namespace HRMChamCong.Views.QuanLyChamCong
{
    public partial class DetailAll : System.Web.UI.Page
    {
        public static string deparmentId { get; set; }
        public static string month { get; set; }
        public static string year { get; set; }
        public static string value { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            deparmentId = HttpContext.Current.Request.QueryString["PhongBan"];
            month = HttpContext.Current.Request.QueryString["Thang"];
            year = HttpContext.Current.Request.QueryString["Nam"];
            value = HttpContext.Current.Request.QueryString["Value"];
            DataBind();
        }

        //[System.Web.Services.WebMethod]
        //public static string GetDetail()
        //{
        //    List<ChamCong> list = new List<ChamCong>();
        //    SessionManager.DoWork(session =>
        //    {
        //        if (value == string.Empty && deparmentId != string.Empty)
        //            list = session.Query<ChamCong>().Where(a => a.DepartmentId == deparmentId && a.CreateDate.Month == int.Parse(month) && a.CreateDate.Year == int.Parse(year)).ToList();
        //        if (value != string.Empty && deparmentId != string.Empty)
        //            list = session.Query<ChamCong>().Where(a => a.DepartmentId == deparmentId && a.MaNS == value && a.CreateDate.Month == int.Parse(month) && a.CreateDate.Year == int.Parse(year)).ToList();
        //    });
        //    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    return js.Serialize(list.ToList());
      
        //}
    }
}