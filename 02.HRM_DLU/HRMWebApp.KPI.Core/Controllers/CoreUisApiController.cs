using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.Core.Security;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
   
    public class ProfessorInfo
    {
        public string Id { get; set; }
        public string HoTen { get; set; }
        public string  HocVi { get; set; }
        public string HocHam { get; set; }
        public string NgaySinh { get; set; }
        public string ChucVu { get; set; }
        public string NoiSinh { get; set; }
        public string GioiTinh { get; set; }
        public string DanToc { get; set; }
        public string TonGiao { get; set; }
        public string CMND { get; set; }
        public string NgayCap { get; set; }
        public string NoiCap { get; set; }
        public string LoaiGiangVien { get; set; }
        public string ChuyenNganh { get; set; }
        public string SoTaiKhoan { get; set; }
        public string NganHang { get; set; }
        public string MaSoThue { get; set; }
        public string DiaChiThuongTru { get; set; }
        public string DienThoai { get; set; }
        public string DiDong { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
    }


    [RoutePrefix("api/uis")]
    public class CoreUIsApiController : ApiController
    {

        public string CoreUisConnectionString = ConfigurationManager.ConnectionStrings["CoreUisConnnection"].ToString();//   @"Data Source=192.168.1.169\DEV;Initial Catalog=CoreUis;Persist Security Info=True;User ID=sa;Password=psc@123";

        [Authorize]
        [Route("GetProfessorInfo")]
        public ProfessorInfo GetProfessorInfo()
        {
            ProfessorInfo result = new ProfessorInfo();
            SessionManager.DoWork(session =>
            {
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = CoreUisConnectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "sp_Online_Professors_Sel";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.Parameters.Add(new SqlParameter("@ProfessorID",staff.StaffProfile.Email));
                    SqlDataReader reader = sqlCom.ExecuteReader();
                    while (reader.Read())
                    {
                     
                        ProfessorInfo item = new ProfessorInfo();
                        result.Id = reader["ProfessorId"].ToString();
                        result.HoTen = reader["FullName"].ToString();
                        result.HocVi = reader["AcademicDegreeName"].ToString();
                        result.HocHam = reader["AcademicTitleName"].ToString();
                        result.NgaySinh = reader["BirthDay"].ToString();
                        //result.ChucVu = reader["ProfessorId"].ToString();
                        result.NoiSinh = reader["BirthPlace"].ToString();
                        result.GioiTinh = reader["Genders"].ToString();
                        result.DanToc = reader["EthnicName"].ToString();
                        result.TonGiao = reader["ReligionName"].ToString();
                        result.CMND = reader["IDCard"].ToString();
                        result.NgayCap = reader["IssueDate"].ToString();
                        result.NoiCap = reader["IssuePlace"].ToString();
                        result.LoaiGiangVien = reader["ProfessorTypeName"].ToString();
                        result.ChuyenNganh = reader["ChuyenNganh"].ToString();
                        result.SoTaiKhoan = reader["SoTaiKhoan"].ToString();
                        result.NganHang = reader["TenNganHang"].ToString();
                        result.MaSoThue = reader["MaSoThue"].ToString();
                        result.DiaChiThuongTru = reader["ContactAddress"].ToString();
                        result.DienThoai = reader["HomePhone"].ToString();
                        result.DiDong = reader["MobilePhone"].ToString();
                        result.Email = reader["Email"].ToString();
                        result.DiaChi = reader["ContactAddress"].ToString();
                        //result = Convert.ToInt32(reader["totalStudents"].ToString());
                    }
                }
            });

              
           
           
            return result;

        }
    }
}
