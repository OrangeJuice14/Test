
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
    public partial class Print : System.Web.UI.Page
    {
        public static string NhanVien { get; set; }
        public static string Nam { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            NhanVien = HttpContext.Current.Request.QueryString["NhanVien"];
            Nam = HttpContext.Current.Request.QueryString["Nam"];
            DataBind();
        }
    }
}