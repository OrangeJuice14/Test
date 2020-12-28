using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetOpenAuth.ApplicationBlock;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using System.Configuration;
using System.Data;
using System.Net;
using System.IO;
using System.Web.Security;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;
using DotNetOpenAuth.OpenId.Extensions.OAuth;
using System.Xml.Linq;
using System.Text;
using HRMChamCong.Helper;

using HRMWeb_Service.Utils;
using HRMWeb_Service;

namespace HRMChamCong
{

    public partial class LoginGoogleApi : System.Web.UI.Page
    {
        string googleClientID = ConfigurationManager.AppSettings["googleClientID"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string uriGoogleCallBack = Request.Url.GetLeftPart(UriPartial.Authority) + "/Login.aspx";
                string Url = "https://accounts.google.com/o/oauth2/auth?";
                StringBuilder UrlBuilder = new StringBuilder(Url);
                UrlBuilder.Append("client_id=" + googleClientID);
                UrlBuilder.Append("&redirect_uri=" + uriGoogleCallBack);
                UrlBuilder.Append("&response_type=code");
                UrlBuilder.Append("&scope=" + "https://www.googleapis.com/auth/userinfo.email");
                Response.Redirect(UrlBuilder.ToString(), false);
            }
        }
    }
}
