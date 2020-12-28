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
        const string PublicKey = "HRMChamCong";
        const string SecretKey = "pscvietnam@hoasua";
        readonly string _token = EncryptUtil.MakeToken(PublicKey, SecretKey);
        readonly Service1 _service = new Service1();
        private static readonly GoogleClient googleClient = new GoogleClient
        {
            ClientIdentifier = ConfigurationManager.AppSettings["googleClientID"],
            ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(ConfigurationManager.AppSettings["googleClientSecret"]),
        };

        protected void page_load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    IAuthorizationState authorization = googleClient.ProcessUserAuthorization();
                    if (authorization == null)
                    {
                        googleClient.RequestUserAuthorization(scope: new[] { "https://www.googleapis.com/auth/userinfo.profile", "https://www.googleapis.com/auth/userinfo.email", "https://www.googleapis.com/auth/plus.me" });
                        googleClient.RequestUserAuthorization(scope: new[] { GoogleClient.Scopes.UserInfo.Profile, GoogleClient.Scopes.UserInfo.Email });
                    }
                    else
                    {
                        IOAuth2Graph oauth2Graph = googleClient.GetGraph(authorization);
                        var user = _service.WebUser_GetByEmail(PublicKey, _token, oauth2Graph.Email);
                        if (user == null)
                            return;
                        var authTicket = new FormsAuthenticationTicket(user.UserName, true, 15);
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
                        SessionHelper.Data(SessionKey.UserId, user.Oid);
                        SessionHelper.Data(SessionKey.ThongTinNhanVien, user.ThongTinNhanVien);
                        SessionHelper.Data(SessionKey.UserName, user.UserName);
                        Response.Redirect("/Views/User/AccountInfo.aspx");
                    }
            }
                catch (Exception ex) { }
        }
        }

    }
}
