using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace HRMWebApp.Helpers
{
    public class ConfigurationUtil
    {
        public static String ReadAppSetting(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }
    }
}