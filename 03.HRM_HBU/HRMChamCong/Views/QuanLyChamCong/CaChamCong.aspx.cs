﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRMChamCong.Utility;

namespace HRMChamCong.Views.QuanLyChamCong
{
    public partial class CaChamCong : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            QuickHelper.ChoseMasterPage(this);
        }
    }
}