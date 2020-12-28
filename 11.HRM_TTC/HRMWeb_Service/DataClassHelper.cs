using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace HRMWeb_Service
{
    public class DataClassHelper
    {
        private static string connectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }

        public static int spd_WebChamCong_CapNhatMatKhau_URM(string username, string password)
        {
            return 1;
            //int result = 0;
            //SqlConnection connection = new SqlConnection();
            //connection.ConnectionString = connectionString;
            //connection.Open();
            //try
            //{
            //    using (connection)
            //    {
            //        SqlCommand sqlCom = new SqlCommand();
            //        sqlCom.Connection = connection;
            //        sqlCom.CommandText = "spd_WebChamCong_CapNhatMatKhau_URM";
            //        sqlCom.CommandType = CommandType.StoredProcedure;
            //        sqlCom.CommandTimeout = 0;
            //        sqlCom.Parameters.Add(new SqlParameter("@UserName", username));
            //        sqlCom.Parameters.Add(new SqlParameter("@Password", password));
            //        result = Convert.ToInt32(sqlCom.ExecuteScalar());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Helper.ErrorLog("DataClassHelper/spd_WebChamCong_CapNhatMatKhau_URM", ex);
            //    throw ex;
            //}

            //return result;
        }
        public static int spd_WebChamCong_TaoTaiKhoan_URM(string username, string password)
        {
            return 1;
            //int result = 0;
            //SqlConnection connection = new SqlConnection();
            //connection.ConnectionString = connectionString;
            //connection.Open();
            //try
            //{
            //    using (connection)
            //    {
            //        SqlCommand sqlCom = new SqlCommand();
            //        sqlCom.Connection = connection;
            //        sqlCom.CommandText = "spd_WebChamCong_TaoTaiKhoan_URM";
            //        sqlCom.CommandType = CommandType.StoredProcedure;
            //        sqlCom.CommandTimeout = 0;
            //        sqlCom.Parameters.Add(new SqlParameter("@UserName", username));
            //        sqlCom.Parameters.Add(new SqlParameter("@Password", password));
            //        result = Convert.ToInt32(sqlCom.ExecuteScalar());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Helper.ErrorLog("DataClassHelper/spd_WebChamCong_TaoTaiKhoan_URM", ex);
            //    throw ex;
            //}

            //return result;
        }
    }
}