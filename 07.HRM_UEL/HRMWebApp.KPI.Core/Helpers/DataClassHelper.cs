using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using HRMWebApp.KPI.Core.DTO.ABC;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO;

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

        public static Dictionary<string, object> GetThanhTichCaNhan(Guid evaluationId, Guid nhanvien)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<spd_Web_DanhGia_ThanhTichCaNhan_SangKien> listSangKien = new List<spd_Web_DanhGia_ThanhTichCaNhan_SangKien>();
            List<spd_Web_DanhGia_ThanhTichCaNhan_NCKH> listNCKH = new List<spd_Web_DanhGia_ThanhTichCaNhan_NCKH>();
            List<spd_Web_DanhGia_ThanhTichCaNhan_KhenThuong> listKhenThuong = new List<spd_Web_DanhGia_ThanhTichCaNhan_KhenThuong>();
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_Web_DanhGia_ThanhTichCaNhan";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandTimeout = 0;
                sqlCom.Parameters.Add(new SqlParameter("@ABC_EvaluationBoard", evaluationId));
                sqlCom.Parameters.Add(new SqlParameter("@ThongTinNhanVien", nhanvien));
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    spd_Web_DanhGia_ThanhTichCaNhan_SangKien newItem = new spd_Web_DanhGia_ThanhTichCaNhan_SangKien();
                    newItem.TenSangKien = reader["TenSangKien"] != DBNull.Value ? reader["TenSangKien"].ToString() : "";
                    newItem.CapDoThamGia = reader["CapDoThamGia"] != DBNull.Value ? reader["CapDoThamGia"].ToString() : "";
                    newItem.ThoiGianHoanThanh = reader["ThoiGianHoanThanh"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["ThoiGianHoanThanh"]) : null;
                    newItem.Diem = reader["Diem"] != DBNull.Value ? reader["Diem"].ToString() : "";
                    listSangKien.Add(newItem);
                }
                result.Add("SangKien", listSangKien);

                reader.NextResult();
                while (reader.Read())
                {
                    spd_Web_DanhGia_ThanhTichCaNhan_NCKH newItem = new spd_Web_DanhGia_ThanhTichCaNhan_NCKH();
                    newItem.TenHoatDong = reader["TenHoatDong"] != DBNull.Value ? reader["TenHoatDong"].ToString() : "";
                    newItem.TenNCKH = reader["TenNCKH"] != DBNull.Value ? reader["TenNCKH"].ToString() : "";
                    newItem.CapDoThamGia = reader["CapDoThamGia"] != DBNull.Value ? reader["CapDoThamGia"].ToString() : "";
                    newItem.ThoiGianHoanThanh = reader["ThoiGianHoanThanh"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["ThoiGianHoanThanh"]) : null;
                    newItem.SoTietQuyDoi = reader["SoTietQuyDoi"] != DBNull.Value ? Convert.ToDecimal(reader["SoTietQuyDoi"]) : 0;
                    newItem.Diem = reader["Diem"] != DBNull.Value ? reader["Diem"].ToString() : "";
                    listNCKH.Add(newItem);
                }
                result.Add("NCKH", listNCKH);

                reader.NextResult();
                while (reader.Read())
                {
                    spd_Web_DanhGia_ThanhTichCaNhan_KhenThuong newItem = new spd_Web_DanhGia_ThanhTichCaNhan_KhenThuong();
                    newItem.TenHinhThucKhenThuong = reader["TenHinhThucKhenThuong"] != DBNull.Value ? reader["TenHinhThucKhenThuong"].ToString() : "";
                    newItem.LyDo = reader["LyDo"] != DBNull.Value ? reader["LyDo"].ToString() : "";
                    newItem.SoQuyetDinh = reader["SoQuyetDinh"] != DBNull.Value ? reader["SoQuyetDinh"].ToString() : "";
                    newItem.NgayQuyetDinh = reader["NgayQuyetDinh"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["NgayQuyetDinh"]) : null;
                    newItem.CoQuanRaQuyetDinh = reader["CoQuanRaQuyetDinh"] != DBNull.Value ? reader["CoQuanRaQuyetDinh"].ToString() : "";
                    listKhenThuong.Add(newItem);
                }
                result.Add("KhenThuong", listKhenThuong);

            }
            connection.Close();

            return result;
        }

        public static List<ABC_UserVMDTO> spd_DanhGiaABC_GetListThongTinUserInDonVi(Guid kyDanhGiaId, Guid donViId)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            List<ABC_UserVMDTO> result = new List<ABC_UserVMDTO>();
            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_DanhGiaABC_GetListThongTinUserInDonVi";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandTimeout = 0;
                sqlCom.Parameters.Add(new SqlParameter("@kyDanhGiaId", kyDanhGiaId));
                sqlCom.Parameters.Add(new SqlParameter("@donViId", donViId));
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCom);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    result.Add(new ABC_UserVMDTO(row));
                }
                //result = ds.Tables[0].AsEnumerable().ToList().Map<ABC_UserVMDTO>();

            }
            connection.Close();

            return result;
        }
        public static int spd_DanhGiaABC_ThongThongTinUserTheoKy(Guid kyDanhGiaId, Guid userId)
        {
            try
            {
                int result = 0;
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_DanhGiaABC_ThongThongTinUserTheoKy";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@KyDanhGiaId", kyDanhGiaId));
                    sqlCom.Parameters.Add(new SqlParameter("@UserId", userId));
                    result = Convert.ToInt32(sqlCom.ExecuteNonQuery());
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_DanhGiaABC_ThongThongTinUserTheoKy", ex);
                throw ex;
            }
        }
    }
}
