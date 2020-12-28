using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMChamCong.Utility
{
    public class QuickHelper
    {
        public static String DocMaDonViSuDung()
        {
            return ConfigurationUtil.ReadAppSetting("MaDonViSuDung");
        }
        public static void ChoseMasterPage(System.Web.UI.Page page)
        {
            String maDonViSuDung = QuickHelper.DocMaDonViSuDung();
            if (maDonViSuDung == "IUH")
            {
                page.MasterPageFile = "~/MasterPage.Master";
            }
            else if (maDonViSuDung == "LUH")
            {
                page.MasterPageFile = "~/MasterLUH.Master";
            }
            else if (maDonViSuDung == "UTE")
            {
                page.MasterPageFile = "~/MasterUTE.Master";
            }
        }
        public static void ChoseLoginLogo(System.Web.UI.HtmlControls.HtmlImage logo)
        {
            String maDonViSuDung = QuickHelper.DocMaDonViSuDung();
            if (maDonViSuDung == "IUH")
            {
                logo.Src = "~/Images/logo_UIH.png";
            }
            else if (maDonViSuDung == "LUH")
            {
                logo.Src = "~/Images/logo_LUH.png";
            }
            else if (maDonViSuDung == "UTE")
            {
                logo.Src = "~/Images/logo_UTE.png";
            }
        }

        //public static void LoginEmail(System.Web.UI.HtmlControls.HtmlAnchor loginEmail)
        //{
        //    String maDonViSuDung = QuickHelper.DocMaDonViSuDung();
        //    if (maDonViSuDung == "IUH")
        //    {
        //        loginEmail.Visible = false;
        //    }
        //    else if (maDonViSuDung == "LUH")
        //    {
        //        loginEmail.Visible = true;
        //    }
        //}
    }
}