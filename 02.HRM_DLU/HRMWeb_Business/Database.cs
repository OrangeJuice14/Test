using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Reflection;

using System.IO;
using System.Configuration;
using HRMWeb_Business.Model.Context;

namespace HRMWeb_Business
{
    public class Database
    {
        private static readonly String NormalConnectionStringFormat = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}";
        private static readonly String EntityConnectionStringFormat = "metadata=res://*/Model.MainModel.csdl|res://*/Model.MainModel.ssdl|res://*/Model.MainModel.msl;provider=System.Data.SqlClient;provider connection string='Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};MultipleActiveResultSets=True'";


        public static string ServerName = null;
        public static string DatabaseName = null;
        public static string Username = null;
        public static string Password = null;

        public static Entities NewEntities()
        {
            Entities context = new Entities(EntityConnectionString);
            context.CommandTimeout = 60 * 10;
            return context;
        }
        private static String _entityConnectionString = null;
        private static String _normalConnectionString = null;
        public static String NormalConnectionString
        {
            get
            {
                GetConnectionString();
                return _normalConnectionString;
            }
        }
        private static string EntityConnectionString
        {
            get
            {
                GetConnectionString();

                return _entityConnectionString;
            }
        }

        private static void GetConnectionString()
        {
            if (String.IsNullOrWhiteSpace(_entityConnectionString))
            {
                    _normalConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    String[] somePartsOfConnectionString = _normalConnectionString.Split(';');
                    const String prefixDataSource = "Data Source=";
                    const String prefixInitialCatalog = "Initial Catalog=";
                    const String prefixUserID = "User ID=";
                    const String prefixPassword = "Password=";
                    ServerName = (from s in somePartsOfConnectionString where s.ToLower().StartsWith(prefixDataSource.ToLower(), StringComparison.Ordinal) select s).Single().Replace(prefixDataSource, "");
                    DatabaseName = (from s in somePartsOfConnectionString where s.ToLower().StartsWith(prefixInitialCatalog.ToLower(), StringComparison.Ordinal) select s).Single().Replace(prefixInitialCatalog, "");
                    Username = (from s in somePartsOfConnectionString where s.ToLower().StartsWith(prefixUserID.ToLower(), StringComparison.Ordinal) select s).Single().Replace(prefixUserID, "");
                    Password = (from s in somePartsOfConnectionString where s.ToLower().StartsWith(prefixPassword.ToLower(), StringComparison.Ordinal) select s).Single().Replace(prefixPassword, "");
                    _entityConnectionString = String.Format(EntityConnectionStringFormat, ServerName, DatabaseName, Username, Password);
            }
        }

    }
}
