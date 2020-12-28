using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMChamCong.Views.BangLuong
{
    public partial class PrintManage : System.Web.UI.Page
    {
       
        public static string webUserId { get; set; }
        public static string kyTinhLuong { get; set; }
        public static string ThangNam { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            webUserId = HttpContext.Current.Request.QueryString["webUserId"];
            kyTinhLuong = HttpContext.Current.Request.QueryString["kyTinhLuong"];
            ThangNam = HttpContext.Current.Request.QueryString["ThangNam"];
            DataBind();
        }
    }
}