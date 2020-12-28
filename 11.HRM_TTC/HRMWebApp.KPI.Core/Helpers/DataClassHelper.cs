using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using HRMWebApp.KPI.Core.DTO.ABC;

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

        public static Dictionary<string, object> GetThongTinCaNhan(Guid nhanVien)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<spd_WebChamCong_LayThongTinCaNhan> thongTinCaNhans = new List<spd_WebChamCong_LayThongTinCaNhan>();

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            using (connection)
            {
                SqlCommand sqlCom = new SqlCommand();
                sqlCom.Connection = connection;
                sqlCom.CommandText = "spd_WebChamCong_LayThongTinCaNhan";
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandTimeout = 0;
                sqlCom.Parameters.Add(new SqlParameter("@ThongTinNhanVien", nhanVien));
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    spd_WebChamCong_LayThongTinCaNhan newItem = new spd_WebChamCong_LayThongTinCaNhan();
                    newItem.MaNhanVien = reader["MaNhanVien"] != DBNull.Value ? reader["MaNhanVien"].ToString() : "";
                    newItem.HoTen = reader["HoTen"] != DBNull.Value ? reader["HoTen"].ToString() : "";
                    newItem.TenChucDanh = reader["TenChucDanh"] != DBNull.Value ? reader["TenChucDanh"].ToString() : "";
                    newItem.TenCongTy = reader["TenCongTy"] != DBNull.Value ? reader["TenCongTy"].ToString() : "";
                    newItem.TenBoPhan = reader["TenBoPhan"] != DBNull.Value ? reader["TenBoPhan"].ToString() : "";
                    newItem.NgayVaoCongTy = reader["NgayVaoCongTy"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["NgayVaoCongTy"]) : null;
                    newItem.NgayBoNhiemChucVu = reader["NgayBoNhiemChucVu"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["NgayBoNhiemChucVu"]) : null;
                    newItem.QuanLyTrucTiep = reader["QuanLyTrucTiep"] != DBNull.Value ? reader["QuanLyTrucTiep"].ToString() : "";
                    newItem.QuanLyTrenMotCap_ChucDanh = reader["QuanLyTrucTiep_ChucDanh"] != DBNull.Value ? reader["QuanLyTrucTiep_ChucDanh"].ToString() : "";
                    newItem.QuanLyTrenMotCap = reader["QuanLyTrenMotCap"] != DBNull.Value ? reader["QuanLyTrenMotCap"].ToString() : "";
                    newItem.QuanLyTrenMotCap = reader["QuanLyTrenMotCap_ChucDanh"] != DBNull.Value ? reader["QuanLyTrenMotCap_ChucDanh"].ToString() : "";

                    thongTinCaNhans.Add(newItem);
                }

                result.Add("thongTinCaNhan", thongTinCaNhans.FirstOrDefault());

                connection.Close();

                return result;
            }
        }

    }
}
