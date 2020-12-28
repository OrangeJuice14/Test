
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
    public partial class Compare : System.Web.UI.Page
    {
        public static string Oid { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Oid = HttpContext.Current.Request.QueryString["Oid"];
            DataBind();
        }
   
    }
}