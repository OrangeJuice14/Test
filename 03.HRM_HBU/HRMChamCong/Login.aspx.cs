using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRMChamCong.Helper;
using HRMChamCong.Utility;

namespace HRMChamCong
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataBind();
        }
        protected override void OnPreInit(EventArgs e)
        {
            QuickHelper.ChoseLoginLogo(this.logo);
            //QuickHelper.LoginEmail(this.loginEmail);
        }
    }
}