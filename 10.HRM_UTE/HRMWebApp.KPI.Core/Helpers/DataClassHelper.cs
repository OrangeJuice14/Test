using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using HRMWebApp.KPI.Core.Helpers;
using System.Data.SqlClient;
using HRMWebApp.KPI.Core.DTO.AdoDataClass;
using System.Data;

namespace HRMWebApp.KPI.Core.Helpers
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

        //***Dòng này Minh thêm mới ngày 10/06/2016 dùng để lấy thông tin chung từ store
        public static Dictionary<Guid, Guid> GetPlanKPIDetailData(Guid departmentId)
        {

            Dictionary<Guid, Guid> result = new Dictionary<Guid, Guid>();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_KPI_GetMucTieuChiTiet";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandTimeout = 0;
                sqlCom.Parameters.Add(new SqlParameter("@DepartmentId", departmentId));
                SqlDataReader reader = sqlCom.ExecuteReader();

                List<PlanKPIDetailADO> planKPIdetails = new List<PlanKPIDetailADO>();
                List<PlanKPIDetailADO> parentPlanKPIdetails = new List<PlanKPIDetailADO>();
                List<MethodADO> methods = new List<MethodADO>();
                while (reader.Read())
                {
                    PlanKPIDetailADO newItem = new PlanKPIDetailADO();
                    newItem.Id = new Guid(reader["PlanDetailId"].ToString());
                    newItem.Name = reader["TargetDetail"].ToString();
                    newItem.ParentPlanKPIDetailId = reader["ParentPlanKPIDetailId"] != DBNull.Value ? new Guid(reader["ParentPlanKPIDetailId"].ToString()) : Guid.Empty;
                    planKPIdetails.Add(newItem);
                }
                //result["planDetailList"] = planKPIdetails;

                reader.NextResult();

                while (reader.Read())
                {
                    PlanKPIDetailADO newItem = new PlanKPIDetailADO();
                    newItem.Id = new Guid(reader["ID"].ToString());
                    newItem.Name = reader["TargetDetail"].ToString();
                    newItem.ParentPlanKPIDetailId = reader["ParentPlanKPIDetailId"] != DBNull.Value ? new Guid(reader["ParentPlanKPIDetailId"].ToString()) : Guid.Empty;
                    parentPlanKPIdetails.Add(newItem);
                    result[newItem.Id] = newItem.ParentPlanKPIDetailId;
                }
                //result["parentPlanDetailList"] = parentPlanKPIdetails;

                reader.NextResult();

                while (reader.Read())
                {
                    MethodADO newItem = new MethodADO();
                    newItem.Id = new Guid(reader["ID"].ToString());
                    newItem.Name = reader["Name"].ToString();
                    newItem.PlanKPIDetailId = new Guid(reader["PlanKPIDetailId"].ToString());
                    methods.Add(newItem);
                }

                //result["methods"] = methods;
            }

            return result;
        }


        //public static Dictionary<string,object> GetPlanKPIDetailData(Guid departmentId)
        //{

        //    Dictionary<string, object> result = new Dictionary<string, object>();
        //    SqlConnection connection = new SqlConnection();
        //    connection.ConnectionString = "Data Source=192.168.1.169;Initial Catalog=PSC_HRM_Test2;Persist Security Info=True;User ID=sa;Password=psc@123";
        //    connection.Open();

        //    using (connection)
        //    {
        //        SqlCommand sqlCom = new SqlCommand();
        //        sqlCom.Connection = connection;
        //        sqlCom.CommandText = "spd_KPI_GetMucTieuChiTiet";
        //        sqlCom.CommandType = CommandType.StoredProcedure;
        //        sqlCom.Parameters.Add(new SqlParameter("@DepartmentId", departmentId));
        //        SqlDataReader reader = sqlCom.ExecuteReader();

        //        List<PlanKPIDetailADO> planKPIdetails = new List<PlanKPIDetailADO>();
        //        List<PlanKPIDetailADO> parentPlanKPIdetails = new List<PlanKPIDetailADO>();
        //        List<MethodADO> methods = new List<MethodADO>();
        //        while (reader.Read())
        //        {
        //            PlanKPIDetailADO newItem = new PlanKPIDetailADO();
        //            newItem.Id = new Guid(reader["PlanDetailId"].ToString());
        //            newItem.Name = reader["TargetDetail"].ToString();
        //            newItem.ParentPlanKPIDetailId = reader["ParentPlanKPIDetailId"]!=DBNull.Value?new Guid(reader["ParentPlanKPIDetailId"].ToString()):Guid.Empty;
        //            planKPIdetails.Add(newItem);
        //        }
        //        result["planDetailList"] = planKPIdetails;

        //        reader.NextResult();

        //        while (reader.Read())
        //        {
        //            PlanKPIDetailADO newItem = new PlanKPIDetailADO();
        //            newItem.Id = new Guid(reader["ID"].ToString());
        //            newItem.Name = reader["TargetDetail"].ToString();
        //            newItem.ParentPlanKPIDetailId = reader["ParentPlanKPIDetailId"] != DBNull.Value ? new Guid(reader["ParentPlanKPIDetailId"].ToString()) : Guid.Empty;
        //            parentPlanKPIdetails.Add(newItem);
        //        }
        //        result["parentPlanDetailList"] = parentPlanKPIdetails;

        //        reader.NextResult();

        //        while (reader.Read())
        //        {
        //            MethodADO newItem = new MethodADO();
        //            newItem.Id = new Guid(reader["ID"].ToString());
        //            newItem.Name = reader["Name"].ToString();
        //            newItem.PlanKPIDetailId = new Guid(reader["PlanKPIDetailId"].ToString());
        //            methods.Add(newItem);
        //        }

        //        result["methods"] = methods;
        //    }

        //    return result;
        //}

        public static IEnumerable<Report_DepartmentStaffResultADO> GetReportDepartmentStaffResult(Guid planId, Guid departmentId)
        {
            List<Report_DepartmentStaffResultADO> result = new List<Report_DepartmentStaffResultADO>();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_KPI_Report_KetQuaDanhGiaTungPhongBan";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandTimeout = 0;
                sqlCom.Parameters.Add(new SqlParameter("@plankpi_ID", planId));
                sqlCom.Parameters.Add(new SqlParameter("@bophanID", departmentId));
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    Report_DepartmentStaffResultADO newItem = new Report_DepartmentStaffResultADO();
                    newItem.TenBoPhan = reader["TenBoPhan"] != DBNull.Value ? reader["TenBoPhan"].ToString() : "";
                    newItem.HoTen = reader["HoTen"] != DBNull.Value ? reader["HoTen"].ToString() : "";
                    newItem.TruongDonViDuyet = reader["TruongPhongDuyet"] != DBNull.Value ? Convert.ToBoolean(reader["TruongPhongDuyet"]) : false;
                    newItem.CaNhanDanhGia = reader["CaNhanDanhGia"] != DBNull.Value ? Convert.ToBoolean(reader["CaNhanDanhGia"]) : false;
                    newItem.TruongDonViDanhGia = reader["TruongPhongDanhGia"] != DBNull.Value ? Convert.ToBoolean(reader["TruongPhongDanhGia"]) : false;
                    result.Add(newItem);
                }

            }
            connection.Close();

            return result;
        }

        public static IEnumerable<Report_TotalDepartmentResultADO> GetReportTotalDepartmentResult(Guid planId)
        {
            List<Report_TotalDepartmentResultADO> result = new List<Report_TotalDepartmentResultADO>();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_KPI_Report_KetQuaDanhGiaTongHopPhongBan";
                sqlCom.CommandType = CommandType.StoredProcedure;
                //sqlCom.CommandTimeout = 0;
                // sqlCom.Parameters.Add(new SqlParameter("@plankpi_ID", planId));
                SqlParameter param1 = new SqlParameter("@plankpi_ID", SqlDbType.UniqueIdentifier);
                param1.Value = planId;
                sqlCom.Parameters.Add(param1);
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    Report_TotalDepartmentResultADO newItem = new Report_TotalDepartmentResultADO();
                    newItem.TenBoPhan = reader["TenBoPhan"] != DBNull.Value ? reader["TenBoPhan"].ToString() : "";
                    newItem.TenBoMon = reader["TenBoMon"] != DBNull.Value ? reader["TenBoMon"].ToString() : "";
                    newItem.SoNV = reader["SoNV"] != DBNull.Value ? Convert.ToInt32(reader["SoNV"]) : 0;
                    newItem.BoPhanKhoa = reader["ChaDaKhoa"] != DBNull.Value ? Convert.ToBoolean(reader["ChaDaKhoa"]) : false;
                    newItem.BoMonKhoa = reader["ConDaKhoa"] != DBNull.Value ? Convert.ToBoolean(reader["ConDaKhoa"]) : false;
                    newItem.TruongDVDaDanhGia = reader["TruongDVDaDanhGia"] != DBNull.Value ? Convert.ToBoolean(reader["TruongDVDaDanhGia"]) : false;
                    newItem.NVDaDanhGia = reader["NVDaDanhGia"] != DBNull.Value ? Convert.ToBoolean(reader["NVDaDanhGia"]) : false;
                    result.Add(newItem);
                }

            }
            connection.Close();

            return result;
        }

        public static IEnumerable<Report_KetQuaDanhGiaKeHoachHoatDongCaNhanADO> Report_KetQuaDanhGiaKeHoachHoatDongCaNhan(Guid planId)
        {
            List<Report_KetQuaDanhGiaKeHoachHoatDongCaNhanADO> result = new List<Report_KetQuaDanhGiaKeHoachHoatDongCaNhanADO>();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_KPI_Report_KetQuaDanhGiaKeHoachHoatDongCaNhan";
                sqlCom.CommandType = CommandType.StoredProcedure;
                //sqlCom.CommandTimeout = 0;
                // sqlCom.Parameters.Add(new SqlParameter("@plankpi_ID", planId));
                SqlParameter param1 = new SqlParameter("@plankpi_ID", SqlDbType.UniqueIdentifier);
                param1.Value = planId;
                sqlCom.Parameters.Add(param1);
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    Report_KetQuaDanhGiaKeHoachHoatDongCaNhanADO newItem = new Report_KetQuaDanhGiaKeHoachHoatDongCaNhanADO();
                    newItem.SoHieuCongChuc = reader["SoHieuCongChuc"] != DBNull.Value ? reader["SoHieuCongChuc"].ToString() : "";
                    newItem.HoTen = reader["HoTen"] != DBNull.Value ? reader["HoTen"].ToString() : "";
                    newItem.BoPhan = reader["BoPhan"] != DBNull.Value ? reader["BoPhan"].ToString() : "";
                    newItem.NMT1NV = reader["NMT1NV"] != DBNull.Value ? Convert.ToDouble(reader["NMT1NV"]) : 0;
                    newItem.NMT2NV = reader["NMT2NV"] != DBNull.Value ? Convert.ToDouble(reader["NMT2NV"]) : 0;
                    newItem.NMT3NV = reader["NMT3NV"] != DBNull.Value ? Convert.ToDouble(reader["NMT3NV"]) : 0;
                    newItem.TongNV = reader["TongNV"] != DBNull.Value ? Convert.ToDouble(reader["TongNV"]) : 0;
                    newItem.NMT1QL = reader["NMT1QL"] != DBNull.Value ? Convert.ToDouble(reader["NMT1QL"]) : 0;
                    newItem.NMT2QL = reader["NMT2QL"] != DBNull.Value ? Convert.ToDouble(reader["NMT2QL"]) : 0;
                    newItem.NMT3QL = reader["NMT3QL"] != DBNull.Value ? Convert.ToDouble(reader["NMT3QL"]) : 0;
                    newItem.TongQL = reader["TongQL"] != DBNull.Value ? Convert.ToDouble(reader["TongQL"]) : 0;
                    newItem.TongKPI = reader["TongKPI"] != DBNull.Value ? Convert.ToDouble(reader["TongKPI"]) : 0;
                    newItem.XepLoai = reader["XepLoai"] != DBNull.Value ? reader["XepLoai"].ToString() : "";
                    result.Add(newItem);
                }

            }
            connection.Close();

            return result;
        }

        public static IEnumerable<Report_KetQuaDanhGiaKeHoachHoatDongDonViADO> Report_KetQuaDanhGiaKeHoachHoatDongDonVi(Guid planId)
        {
            List<Report_KetQuaDanhGiaKeHoachHoatDongDonViADO> result = new List<Report_KetQuaDanhGiaKeHoachHoatDongDonViADO>();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_KPI_Report_KetQuaDanhGiaKeHoachHoatDongDonVi";
                sqlCom.CommandType = CommandType.StoredProcedure;
                //sqlCom.CommandTimeout = 0;
                // sqlCom.Parameters.Add(new SqlParameter("@plankpi_ID", planId));
                SqlParameter param1 = new SqlParameter("@plankpi_ID", SqlDbType.UniqueIdentifier);
                param1.Value = planId;
                sqlCom.Parameters.Add(param1);
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    Report_KetQuaDanhGiaKeHoachHoatDongDonViADO newItem = new Report_KetQuaDanhGiaKeHoachHoatDongDonViADO();
                    newItem.TenBoPhan = reader["TenBoPhan"] != DBNull.Value ? reader["TenBoPhan"].ToString() : "";
                    newItem.NMT1QL = reader["NMT1QL"] != DBNull.Value ? Convert.ToDouble(reader["NMT1QL"]) : 0;
                    newItem.NMT2QL = reader["NMT2QL"] != DBNull.Value ? Convert.ToDouble(reader["NMT2QL"]) : 0;
                    newItem.NMT3QL = reader["NMT3QL"] != DBNull.Value ? Convert.ToDouble(reader["NMT3QL"]) : 0;
                    newItem.NMT1BGH = reader["NMT1BGH"] != DBNull.Value ? Convert.ToDouble(reader["NMT1BGH"]) : 0;
                    newItem.NMT2BGH = reader["NMT2BGH"] != DBNull.Value ? Convert.ToDouble(reader["NMT2BGH"]) : 0;
                    newItem.NMT3BGH = reader["NMT3BGH"] != DBNull.Value ? Convert.ToDouble(reader["NMT3BGH"]) : 0;
                    newItem.TongDiem_XepLoai = reader["TongDiem_XepLoai"] != DBNull.Value ? reader["TongDiem_XepLoai"].ToString() : "";
                    newItem.SoNV = reader["SoNV"] != DBNull.Value ? Convert.ToInt32(reader["SoNV"]) : 0;
                    newItem.NV_A = reader["NV_A"] != DBNull.Value ? Convert.ToInt32(reader["NV_A"]) : 0;
                    newItem.PhanTram_A = reader["PhanTram_A"] != DBNull.Value ? Convert.ToDouble(reader["PhanTram_A"]) : 0;
                    newItem.NV_B = reader["NV_B"] != DBNull.Value ? Convert.ToInt32(reader["NV_B"]) : 0;
                    newItem.PhanTram_B = reader["PhanTram_B"] != DBNull.Value ? Convert.ToDouble(reader["PhanTram_B"]) : 0;
                    newItem.NV_C = reader["NV_C"] != DBNull.Value ? Convert.ToInt32(reader["NV_C"]) : 0;
                    newItem.PhanTram_C = reader["PhanTram_C"] != DBNull.Value ? Convert.ToDouble(reader["PhanTram_C"]) : 0;
                    newItem.NV_D = reader["NV_D"] != DBNull.Value ? Convert.ToInt32(reader["NV_D"]) : 0;
                    newItem.PhanTram_D = reader["PhanTram_D"] != DBNull.Value ? Convert.ToDouble(reader["PhanTram_D"]) : 0;
                    newItem.NV_E = reader["NV_E"] != DBNull.Value ? Convert.ToInt32(reader["NV_E"]) : 0;
                    newItem.PhanTram_E = reader["PhanTram_E"] != DBNull.Value ? Convert.ToDouble(reader["PhanTram_E"]) : 0;
                    newItem.NV_F = reader["NV_F"] != DBNull.Value ? Convert.ToInt32(reader["NV_F"]) : 0;
                    newItem.PhanTram_F = reader["PhanTram_F"] != DBNull.Value ? Convert.ToDouble(reader["PhanTram_F"]) : 0;
                    result.Add(newItem);
                }

            }
            connection.Close();

            return result;
        }

        public static IEnumerable<Report_KetQuaDangKyCheDoLamViecDTO> Report_KetQuaDangKyCheDoLamViec(Guid planId)
        {
            List<Report_KetQuaDangKyCheDoLamViecDTO> result = new List<Report_KetQuaDangKyCheDoLamViecDTO>();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_KPI_Report_KetQuaDangKyCheDoLamViec";
                sqlCom.CommandType = CommandType.StoredProcedure;
                SqlParameter param1 = new SqlParameter("@plankpi_ID", SqlDbType.UniqueIdentifier);
                param1.Value = planId;
                sqlCom.Parameters.Add(param1);
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    Report_KetQuaDangKyCheDoLamViecDTO newItem = new Report_KetQuaDangKyCheDoLamViecDTO();
                    newItem.PlanStaffId = new Guid(reader["ID"].ToString());
                    newItem.PlanKPIId = new Guid(reader["PlanKPIId"].ToString());
                    newItem.StaffId = new Guid(reader["StaffId"].ToString());
                    newItem.MaCBVC = reader["SoHieuCongChuc"] != DBNull.Value ? reader["SoHieuCongChuc"].ToString() : "";
                    newItem.HoTen = reader["HoTen"] != DBNull.Value ? reader["HoTen"].ToString() : "";
                    newItem.BoMonId = new Guid(reader["BoPhanId"].ToString());
                    newItem.BoMon = reader["BoMon"] != DBNull.Value ? reader["BoMon"].ToString() : "";
                    newItem.HocHam = reader["TenHocHam"] != DBNull.Value ? reader["TenHocHam"].ToString() : "";
                    newItem.HocVi = reader["TenHocVi"] != DBNull.Value ? reader["TenHocVi"].ToString() : "";
                    newItem.Duyet = Convert.ToBoolean(reader["Duyet"].ToString());
                    newItem.KhoaId = reader["KhoaId"] != DBNull.Value ? new Guid(reader["KhoaId"].ToString()) : Guid.Empty;
                    newItem.CheDoDangKyId = new Guid(reader["WorkingModeId"].ToString());
                    newItem.CheDoDangKy = reader["WorkingModeName"] != DBNull.Value ? reader["WorkingModeName"].ToString() : "";
                    newItem.Time = Convert.ToDateTime(reader["ModifiedTime"].ToString());
                    result.Add(newItem);
                }

            }
            connection.Close();

            SessionManager.DoWork( session =>
            {
                foreach (Report_KetQuaDangKyCheDoLamViecDTO item in result)
                {
                    if (item.KhoaId != Guid.Empty)
                    {
                        Department dep = session.Query<Department>().Where(r => r.Id == item.KhoaId).SingleOrDefault();
                        item.Khoa = dep.Name;
                    }
                    else
                        item.Khoa = " ";
                  
                }
            });
           
            return result;
        }

        public static List<Dictionary<string, object>> UpdateDensityResult(Guid resultId)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_KPI_CapNhatTrongSoTrongDanhGia";
                sqlCom.CommandType = CommandType.StoredProcedure;
                //sqlCom.CommandTimeout = 0;
                SqlParameter param1 = new SqlParameter("@Result_ID", SqlDbType.UniqueIdentifier);
                param1.Value = resultId;
                sqlCom.Parameters.Add(param1);
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    Dictionary<string, object> item = new Dictionary<string, object>();
                    item["ID"] = reader["ID"] != DBNull.Value ? new Guid(reader["ID"].ToString()) : Guid.Empty;
                    item["DensityResult"] = reader["DensityResult"] != DBNull.Value ? Convert.ToDouble(reader["DensityResult"]) : 0;
                    result.Add(item);
                }
            }
            connection.Close();

            return result;
        }

        public static void SaveRatingResult(Guid resultId)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_KPI_CapNhatKetQuaDanhGia";
                sqlCom.CommandType = CommandType.StoredProcedure;
                //sqlCom.CommandTimeout = 0;
                SqlParameter param1 = new SqlParameter("@Result_ID", SqlDbType.UniqueIdentifier);
                param1.Value = resultId;
                sqlCom.Parameters.Add(param1);
                sqlCom.ExecuteReader();
            }
            connection.Close();
        }
        public static IEnumerable<OtherActivityDataDTO> DanhSachHoatDongKhac(string ManageCode)
        {
            List<OtherActivityDataDTO> result = new List<OtherActivityDataDTO>();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_KPI_XemDanhSachHoatDongKhac";
                sqlCom.CommandType = CommandType.StoredProcedure;
                //sqlCom.CommandTimeout = 0;
                SqlParameter param1 = new SqlParameter("@ManageCode", SqlDbType.NVarChar);
                param1.Value = ManageCode;
                sqlCom.Parameters.Add(param1);
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    OtherActivityDataDTO item = new OtherActivityDataDTO();
                    item.Id = reader["ID"] != DBNull.Value ? new Guid(reader["ID"].ToString()) : Guid.Empty;
                    item.StaffCode = reader["StaffCode"] != DBNull.Value ? reader["StaffCode"].ToString() : "";
                    item.ManageCode = reader["ManageCode"] != DBNull.Value ? reader["ManageCode"].ToString() : "";
                    item.StaffName = reader["HoTen"] != DBNull.Value ? reader["HoTen"].ToString() : "";
                    item.Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "";
                    item.StudyYear = reader["StudyTerm"] != DBNull.Value ? reader["StudyTerm"].ToString() : "";
                    item.StudyTerm = reader["StudyYear"] != DBNull.Value ? reader["StudyYear"].ToString() : "";
                    // item.StaffId = reader["StaffId"] != DBNull.Value ? new Guid(reader["StaffId"].ToString()) : Guid.Empty;
                    // item. = reader["CriterionDictionaryId"] != DBNull.Value ? new Guid(reader["CriterionDictionaryId"].ToString()) : Guid.Empty;
                    // item["TenHoatDong"] = reader["TenHoatDong"] != DBNull.Value ? reader["TenHoatDong"].ToString() : "";
                    item.NumberOfTime = reader["NumberOfTime"] != DBNull.Value ? Convert.ToDouble(reader["NumberOfTime"]) : 0;
                    result.Add(item);
                }
            }
            connection.Close();

            return result;
        }
    }
}
