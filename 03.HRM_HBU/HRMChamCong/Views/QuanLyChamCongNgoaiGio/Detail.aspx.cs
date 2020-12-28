
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;
using System.Web.Script.Services;
namespace HRMChamCong.Views.QuanLyChamCongNgoaiGio
{
    public partial class Detail : System.Web.UI.Page
    {
        public static string PhongBan { get; set; }
        public static string KyTinhLuong { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            PhongBan = HttpContext.Current.Request.QueryString["PhongBan"];
            KyTinhLuong = HttpContext.Current.Request.QueryString["KyTinhLuong"];
            DataBind();
        }
   
    }
}