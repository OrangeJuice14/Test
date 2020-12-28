using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using HRMWebApp.KPI.Core.DTO.ABC;
using HRMWebApp.KPI.Core.DTO.AdoDataClass;
using HRMWebApp.Helpers;
using System.Configuration;

namespace HRMWebApp.KPI.Core.Helpers
{
    public class DataClassHelper
    {
        private static string connectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }
        private static string connectionStringEsurvey
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ESurveyConnection"].ConnectionString;
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

        public static List<ESurveyObject> GetESurveyClassId(string userName)
        {
            try
            {
                List<ESurveyObject> result = new List<ESurveyObject>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionStringEsurvey;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "VoteResult_GetObligate_ByLoginGV_NV";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@UserId", userName));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new ESurveyObject();
                        try { item.ClassId = Convert.ToInt32(reader["ClassId"]); } catch { }
                        try { item.ClassName = reader["ClassName"].ToString(); } catch { }
                        item.auth = HRMWebApp.Helpers.Helper.getMd5Hash(item.ClassId + userName + "psc.com@123");
                        item.Link = "http://esurvey.neu.edu.vn/FrontEnd/VoteRatingTemplate/VoteIndex.aspx";
                        item.Link += $"?NID={userName}&classId={item.ClassId}&auth={item.auth}";
                        if (item.ClassId != 0)
                            result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/GetESurveyClassId", ex);
                throw ex;
            }
        }


        ///////////////////////////// PMS /////////////////////////////////
        public static List<NamHoc> GetNamHoc()
        {
            try
            {
                List<NamHoc> result = new List<NamHoc>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "SELECT * FROM dbo.NamHoc WHERE GCRecord IS NULL ORDER BY NgayBatDau DESC";
                    sqlCom.CommandType = CommandType.Text;
                    sqlCom.CommandTimeout = 0;
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new NamHoc();
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.TenNamHoc = reader["TenNamHoc"].ToString(); } catch { }
                        try { item.NgayBatDau = Convert.ToDateTime(reader["NgayBatDau"]); } catch { }
                        try { item.NgayKetThuc = Convert.ToDateTime(reader["NgayKetThuc"]); } catch { }
                        result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/GetNamHoc", ex);
                throw ex;
            }
        }
        public static List<HocKy> GetHocKy(Guid NamHoc)
        {
            try
            {
                List<HocKy> result = new List<HocKy>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "SELECT * FROM dbo.HocKy WHERE GCRecord IS NULL AND NamHoc = @NamHoc ORDER BY TuNgay";
                    sqlCom.CommandType = CommandType.Text;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new HocKy();
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.MaQuanLy = reader["MaQuanLy"].ToString(); } catch { }
                        try { item.TenHocKy = reader["TenHocKy"].ToString(); } catch { }
                        try { item.TuNgay = Convert.ToDateTime(reader["TuNgay"]); } catch { }
                        try { item.DenNgay = Convert.ToDateTime(reader["DenNgay"]); } catch { }
                        result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/GetNamHoc", ex);
                throw ex;
            }
        }

        public static List<spd_PMS_ThongTinThoiKhoaBieu_GiangVien> spd_PMS_ThongTinThoiKhoaBieu_GiangVien(Guid NamHoc, Guid HocKy, Guid GiangVien)
        {
            try
            {
                List<spd_PMS_ThongTinThoiKhoaBieu_GiangVien> result = new List<spd_PMS_ThongTinThoiKhoaBieu_GiangVien>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_PMS_ThongTinThoiKhoaBieu_GiangVien";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@GiangVien", GiangVien));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new spd_PMS_ThongTinThoiKhoaBieu_GiangVien();
                        try { item.HoTen = reader["HoTen"].ToString(); } catch { }
                        try { item.MaNhanVien = reader["MaNhanVien"].ToString(); } catch { }
                        try { item.CMND = reader["CMND"].ToString(); } catch { }
                        try { item.TenDonVi = reader["TenDonVi"].ToString(); } catch { }
                        try { item.KhoaVien = reader["KhoaVien"].ToString(); } catch { }
                        try { item.BoMon = reader["BoMon"].ToString(); } catch { }
                        try { item.TenTrinhDoChuyenMon = reader["TenTrinhDoChuyenMon"].ToString(); } catch { }
                        try { item.TenChucDanh = reader["TenChucDanh"].ToString(); } catch { }
                        try { item.MaMonHoc = reader["MaMonHoc"].ToString(); } catch { }
                        try { item.TenMonHoc = reader["TenMonHoc"].ToString(); } catch { }
                        try { item.MaLopHocPhan = reader["MaLopHocPhan"].ToString(); } catch { }
                        try { item.TenLopHocPhan = reader["TenLopHocPhan"].ToString(); } catch { }
                        try { item.HinhThucGiangDay = reader["HinhThucGiangDay"].ToString(); } catch { }
                        try { item.LoaiHocPhan = reader["LoaiHocPhan"].ToString(); } catch { }
                        try { item.TenBacDaoTao = reader["TenBacDaoTao"].ToString(); } catch { }
                        try { item.TenHeDaoTao = reader["TenHeDaoTao"].ToString(); } catch { }
                        try { item.SoTinChi = Convert.ToDouble(reader["SoTinChi"]); } catch { }
                        try { item.SoTietDungLop = Convert.ToDouble(reader["SoTietDungLop"]); } catch { }
                        try { item.SoTietHeThong = Convert.ToDouble(reader["SoTietHeThong"]); } catch { }
                        try { item.SoGioChuanDungLop = Convert.ToDouble(reader["SoGioChuanDungLop"]); } catch { }
                        try { item.SoSinhVienDK = Convert.ToDouble(reader["SoSinhVienDK"]); } catch { }
                        try { item.ThoiGianGiangDay = reader["ThoiGianGiangDay"].ToString(); } catch { }
                        try { item.KhoaDaoTao = reader["KhoaDaoTao"].ToString(); } catch { }
                        try { item.GhiChu = reader["GhiChu"].ToString(); } catch { }
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.XacNhan = Convert.ToBoolean(reader["XacNhan"]); } catch { }
                        result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_ThongTinThoiKhoaBieu_GiangVien", ex);
                throw ex;
            }
        }

        public static int spd_PMS_ThoiKhoaBiet_XacNhan_Web(Guid? OidChiTiet, string User, string GhiChu)
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
                    sqlCom.CommandText = "spd_PMS_ThoiKhoaBiet_XacNhan_Web";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@OidChiTiet", OidChiTiet));
                    sqlCom.Parameters.Add(new SqlParameter("@User", User));
                    sqlCom.Parameters.Add(new SqlParameter("@GhiChu", GhiChu));
                    result = Convert.ToInt32(sqlCom.ExecuteNonQuery());
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_ThoiKhoaBiet_XacNhan_Web", ex);
                throw ex;
            }
        }

        public static List<spd_PMS_GetLoaiHoatDongAll> spd_PMS_GetLoaiHoatDongAll()
        {
            try
            {
                List<spd_PMS_GetLoaiHoatDongAll> result = new List<spd_PMS_GetLoaiHoatDongAll>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_PMS_GetLoaiHoatDongAll";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new spd_PMS_GetLoaiHoatDongAll();
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.MaQuanLy = reader["MaQuanLy"].ToString(); } catch { }
                        try { item.TenLoaiHoatDong = reader["TenLoaiHoatDong"].ToString(); } catch { }
                        result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_GetLoaiHoatDongAll", ex);
                throw ex;
            }
        }

        public static List<spd_PMS_KeKhai_CacHD_TKB> spd_PMS_KeKhai_CacHD_TKB(Guid NamHoc, Guid HocKy, Guid GiangVien)
        {
            try
            {
                List<spd_PMS_KeKhai_CacHD_TKB> result = new List<spd_PMS_KeKhai_CacHD_TKB>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_PMS_KeKhai_CacHD_TKB";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@GiangVien", GiangVien));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new spd_PMS_KeKhai_CacHD_TKB();
                        try { item.HoTen = reader["HoTen"].ToString(); } catch { }
                        try { item.MaNhanVien = reader["MaNhanVien"].ToString(); } catch { }
                        try { item.CMND = reader["CMND"].ToString(); } catch { }
                        try { item.TenBoPhan = reader["TenBoPhan"].ToString(); } catch { }
                        try { item.BoMonQuanLy = reader["BoMonQuanLy"].ToString(); } catch { }
                        try { item.TenMonHoc = reader["TenMonHoc"].ToString(); } catch { }
                        try { item.LopMonHoc = reader["LopMonHoc"].ToString(); } catch { }
                        try { item.SoBaiKiemTra = Convert.ToInt32(reader["SoBaiKiemTra"]); } catch { }
                        try { item.SoBaiThi = Convert.ToInt32(reader["SoBaiThi"]); } catch { }
                        try { item.SoBaiTapLon = Convert.ToInt32(reader["SoBaiTapLon"]); } catch { }
                        try { item.SoBaiTieuLuan = Convert.ToInt32(reader["SoBaiTieuLuan"]); } catch { }
                        try { item.SoDeAnTotNghiep = Convert.ToInt32(reader["SoDeAnTotNghiep"]); } catch { }
                        try { item.SoChuyenDeTotNghiep = Convert.ToInt32(reader["SoChuyenDeTotNghiep"]); } catch { }
                        try { item.SoHDKhac = Convert.ToInt32(reader["SoHDKhac"]); } catch { }
                        try { item.SoSlotHoc = Convert.ToInt32(reader["SoSlotHoc"]); } catch { }
                        try { item.SoTraLoiCauHoiTrenHeThongHocTap = Convert.ToInt32(reader["SoTraLoiCauHoiTrenHeThongHocTap"]); } catch { }
                        try { item.SoTruyCapLopHoc = Convert.ToInt32(reader["SoTruyCapLopHoc"]); } catch { }
                        try { item.SoDeRaDe = Convert.ToInt32(reader["SoDeRaDe"]); } catch { }
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.OidHuongDan = new Guid(reader["OidHuongDan"].ToString()); } catch { }
                        try { item.TenHuongDan = reader["TenHuongDan"].ToString(); } catch { }
                        try { item.SoLuongHuongDan = Convert.ToInt32(reader["SoLuongHuongDan"]); } catch { }
                        try { item.HieuLucXacNhan = Convert.ToBoolean(reader["HieuLucXacNhan"]); } catch { }
                        result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_KeKhai_CacHD_TKB", ex);
                throw ex;
            }
        }

        public static int spd_PMS_KeKhai_CacHD_TKB_CapNhat(Guid? OidChiTiet, string User, int SoBaiKT, int SoBaiThi, int SoBaiTapLon, int SoBaiTieuLuan, int SoDeAnMonHoc, int SoChuyenDeTN, int SoHDKhac, int SoSlotHoc, int SoTraLoiCauHoi, int SoTruyCapLopHoc, int SoDeRaDe, int SoLuongHuongDan)
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
                    sqlCom.CommandText = "spd_PMS_KeKhai_CacHD_TKB_CapNhat";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@OidChiTiet", OidChiTiet));
                    sqlCom.Parameters.Add(new SqlParameter("@User", User));
                    sqlCom.Parameters.Add(new SqlParameter("@SoBaiKT", SoBaiKT));
                    sqlCom.Parameters.Add(new SqlParameter("@SoBaiThi", SoBaiThi));
                    sqlCom.Parameters.Add(new SqlParameter("@SoBaiTapLon", SoBaiTapLon));
                    sqlCom.Parameters.Add(new SqlParameter("@SoBaiTieuLuan", SoBaiTieuLuan));
                    sqlCom.Parameters.Add(new SqlParameter("@SoDeAnMonHoc", SoDeAnMonHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@SoChuyenDeTN", SoChuyenDeTN));
                    sqlCom.Parameters.Add(new SqlParameter("@SoHDKhac", SoHDKhac));
                    sqlCom.Parameters.Add(new SqlParameter("@SoSlotHoc", SoSlotHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@SoTraLoiCauHoi", SoTraLoiCauHoi));
                    sqlCom.Parameters.Add(new SqlParameter("@SoTruyCapLopHoc", SoTruyCapLopHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@SoDeRaDe", SoDeRaDe));
                    sqlCom.Parameters.Add(new SqlParameter("@SoLuongHuongDan", SoLuongHuongDan));
                    result = Convert.ToInt32(sqlCom.ExecuteNonQuery());
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_KeKhai_CacHD_TKB_CapNhat", ex);
                throw ex;
            }
        }

        public static int spd_PMS_KeKhai_CacHD_TKB_Them(Guid? Oid_NhanVien, Guid? Oid_LoaiHuongDan, Guid? NamHoc, Guid? HocKy, Guid? BacDaoTao, Guid? HeDaoTao, string TenMonHoc, string LopHocPhan, Guid? BoMon, int? SoBaiKiemTra, int? SoBaiThi, int? SoBaiTapLon, int? SoBaiTieuLuan, int? SoDeAnMonHoc, int? SoChuyenDeTN, int? SoHDKhac, int? SoSlotHoc, int? SoTraLoiCauHoi, int? SoTruyCapLopHoc, int? SoDeRaDe, int? SoLuongHuongDan)
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
                    sqlCom.CommandText = "spd_PMS_KeKhai_CacHD_TKB_Them";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@Oid_NhanVien", Oid_NhanVien));
                    sqlCom.Parameters.Add(new SqlParameter("@Oid_LoaiHuongDan", Oid_LoaiHuongDan));
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@BacDaoTao", BacDaoTao));
                    sqlCom.Parameters.Add(new SqlParameter("@HeDAoTAo", HeDaoTao));
                    sqlCom.Parameters.Add(new SqlParameter("@TenMonHoc", TenMonHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@LopHocPhan", LopHocPhan));
                    sqlCom.Parameters.Add(new SqlParameter("@BoMon", BoMon));
                    sqlCom.Parameters.Add(new SqlParameter("@SoBaiKiemTra", SoBaiKiemTra));
                    sqlCom.Parameters.Add(new SqlParameter("@SoBaiThi", SoBaiThi));
                    sqlCom.Parameters.Add(new SqlParameter("@SoBaiTapLon", SoBaiTapLon));
                    sqlCom.Parameters.Add(new SqlParameter("@SoBaiTieuLuan", SoBaiTieuLuan));
                    sqlCom.Parameters.Add(new SqlParameter("@SoDeAnMonHoc", SoDeAnMonHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@SoChuyenDeTN", SoChuyenDeTN));
                    sqlCom.Parameters.Add(new SqlParameter("@SoHDKhac", SoHDKhac));
                    sqlCom.Parameters.Add(new SqlParameter("@SoSlotHoc", SoSlotHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@SoTraLoiCauHoi", SoTraLoiCauHoi));
                    sqlCom.Parameters.Add(new SqlParameter("@SoTruyCapLopHoc", SoTruyCapLopHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@SoDeRaDe", SoDeRaDe));
                    sqlCom.Parameters.Add(new SqlParameter("@SoLuongHuongDan", SoLuongHuongDan));
                    result = Convert.ToInt32(sqlCom.ExecuteNonQuery());
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_KeKhai_CacHD_TKB_Them", ex);
                throw ex;
            }
        }

        public static List<spd_DanhMuc_GetDSHoatDong> spd_DanhMuc_GetDSHoatDong(Guid? NhomHoatDong)
        {
            try
            {
                List<spd_DanhMuc_GetDSHoatDong> result = new List<spd_DanhMuc_GetDSHoatDong>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_DanhMuc_GetDSHoatDong";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    if (NhomHoatDong.HasValue)
                        sqlCom.Parameters.Add(new SqlParameter("@NhomHoatDong", NhomHoatDong));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new spd_DanhMuc_GetDSHoatDong();
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.MaQuanLy = reader["MaQuanLy"].ToString(); } catch { }
                        try { item.TenHoatDong = reader["TenHoatDong"].ToString(); } catch { }
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_DanhMuc_GetDSHoatDong", ex);
                throw ex;
            }
        }

        public static List<spd_KeKhaiHDKhac_LayDanhSachKeKhai> spd_KeKhaiHDKhac_LayDanhSachKeKhai(Guid NamHoc, Guid HocKy, Guid GiangVien)
        {
            try
            {
                List<spd_KeKhaiHDKhac_LayDanhSachKeKhai> result = new List<spd_KeKhaiHDKhac_LayDanhSachKeKhai>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_KeKhaiHDKhac_LayDanhSachKeKhai";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@GiangVien", GiangVien));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new spd_KeKhaiHDKhac_LayDanhSachKeKhai();
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.TenHoatDong = reader["TenHoatDong"].ToString(); } catch { }
                        try { item.SoGioThucHien = Convert.ToDecimal(reader["SoGioThucHien"]); } catch { }
                        try { item.NgayThucHien = Convert.ToDateTime(reader["NgayThucHien"]); } catch { }
                        try { item.GhiChu = reader["GhiChu"].ToString(); } catch { }
                        try { item.TrangThai = Convert.ToInt32(reader["TrangThai"]); } catch { }
                        try { item.TenTrangThai = reader["TenTrangThai"].ToString(); } catch { }
                        result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_KeKhaiHDKhac_LayDanhSachKeKhai", ex);
                throw ex;
            }
        }

        public static spd_PMS_KeKhai_CacHoatDongKhac spd_PMS_KeKhai_CacHoatDongKhac(Guid NamHoc, Guid HocKy, Guid GiangVien, Guid HoatDong, Guid BoMon, decimal SoGioThucHien, string GhiChu, DateTime? NgayThucHien)
        {
            try
            {
                var result = new spd_PMS_KeKhai_CacHoatDongKhac();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_PMS_KeKhai_CacHoatDongKhac";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@GiangVien", GiangVien));
                    sqlCom.Parameters.Add(new SqlParameter("@HoatDong", HoatDong));
                    sqlCom.Parameters.Add(new SqlParameter("@BoMon", BoMon));
                    sqlCom.Parameters.Add(new SqlParameter("@SoGioThucHien", SoGioThucHien));
                    sqlCom.Parameters.Add(new SqlParameter("@GhiChu", GhiChu));
                    sqlCom.Parameters.Add(new SqlParameter("@NgayThucHien", NgayThucHien));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        try { result.TrangThai = Convert.ToInt32(reader["TrangThai"]); } catch { }
                        try { result.DienGiai = reader["DienGiai"].ToString(); } catch { }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_KeKhai_CacHoatDongKhac", ex);
                throw ex;
            }
        }

        public static int spd_KeKhaiHDKhac_CapNhatGioKeKhai(Guid OidChiTiet, decimal SoGio, string GhiChu, string User)
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
                    sqlCom.CommandText = "spd_KeKhaiHDKhac_CapNhatGioKeKhai";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@OidChiTiet", OidChiTiet));
                    sqlCom.Parameters.Add(new SqlParameter("@SoGio", SoGio));
                    sqlCom.Parameters.Add(new SqlParameter("@GhiChu", GhiChu));
                    sqlCom.Parameters.Add(new SqlParameter("@User", User));
                    result = Convert.ToInt32(sqlCom.ExecuteNonQuery());
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_KeKhaiHDKhac_CapNhatGioKeKhai", ex);
                throw ex;
            }
        }

        public static List<spd_PMS_XacNhanKeKhai> spd_PMS_XacNhanKeKhai(Guid NamHoc, Guid HocKy, Guid BoPhan, Guid NhanVien)
        {
            try
            {
                List<spd_PMS_XacNhanKeKhai> result = new List<spd_PMS_XacNhanKeKhai>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_PMS_XacNhanKeKhai";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@BoPhan", BoPhan));
                    sqlCom.Parameters.Add(new SqlParameter("@NhanVien", NhanVien));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new spd_PMS_XacNhanKeKhai();
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.DonVi = reader["DonVi"].ToString(); } catch { }
                        try { item.HoTen = reader["HoTen"].ToString(); } catch { }
                        try { item.MaQuanLy = reader["MaQuanLy"].ToString(); } catch { }
                        try { item.TenHoatDong = reader["TenHoatDong"].ToString(); } catch { }
                        try { item.SoGioThucHien = Convert.ToDecimal(reader["SoGioThucHien"]); } catch { }
                        try { item.NgayThucHien = Convert.ToDateTime(reader["NgayThucHien"]); } catch { }
                        try { item.GhiChu = reader["GhiChu"].ToString(); } catch { }
                        try { item.TrangThai = Convert.ToInt32(reader["TrangThai"]); } catch { }
                        try { item.TenHocKy = reader["TenHocKy"].ToString(); } catch { }
                        result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_XacNhanKeKhai", ex);
                throw ex;
            }
        }

        public static int spd_KeKhaiHDKhac_DonVi_Duyet(Guid OidChiTiet, int TrangThai, string User)
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
                    sqlCom.CommandText = "spd_KeKhaiHDKhac_DonVi_Duyet";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@OidChiTiet", OidChiTiet));
                    sqlCom.Parameters.Add(new SqlParameter("@TrangThai", TrangThai));
                    sqlCom.Parameters.Add(new SqlParameter("@User", User));
                    result = Convert.ToInt32(sqlCom.ExecuteNonQuery());
                }

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_KeKhaiHDKhac_DonVi_Duyet", ex);
                throw ex;
            }
        }

        public static spd_PMS_KiemTraThongTin_ImportTKB spd_PMS_KiemTraThongTin_ImportTKB(string MaQuanLy, string MaDonVi, string TenBoMonQuanLy)
        {
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_PMS_KiemTraThongTin_ImportTKB";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@MaQuanLy", MaQuanLy));
                    sqlCom.Parameters.Add(new SqlParameter("@MaDonVi", MaDonVi));
                    sqlCom.Parameters.Add(new SqlParameter("@TenBoMonQuanLy", TenBoMonQuanLy));
                    var reader = sqlCom.ExecuteReader();

                    var item = new spd_PMS_KiemTraThongTin_ImportTKB();
                    while (reader.Read())
                    {
                        try { item.NhanVien = reader["NhanVien"].ToString(); } catch { }
                        try { item.DonVi = reader["DonVi"].ToString(); } catch { }
                        try { item.BoMonQuanLy = reader["BoMonQuanLy"].ToString(); } catch { }
                    }
                    if (item.NhanVien == null || item.DonVi == null || item.BoMonQuanLy == null)
                    {
                        return null;
                    }
                    else return item;
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_KiemTraThongTin_ImportTKB", ex);
                throw ex;
            }
        }
        public static IEnumerable<spd_PMS_ThoiKhoaBieu_DanhSachPhanCongGiangDay> spd_PMS_ThoiKhoaBieu_DanhSachPhanCongGiangDay_Web(Guid NamHoc, Guid HocKy, Guid BoMonQuanLy)
        {
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                List<spd_PMS_ThoiKhoaBieu_DanhSachPhanCongGiangDay> items = new List<spd_PMS_ThoiKhoaBieu_DanhSachPhanCongGiangDay>();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_PMS_ThoiKhoaBieu_DanhSachPhanCongGiangDay_Web";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@BoMonQuanLy", BoMonQuanLy));
                    var reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        spd_PMS_ThoiKhoaBieu_DanhSachPhanCongGiangDay item = new spd_PMS_ThoiKhoaBieu_DanhSachPhanCongGiangDay();
                        try { item.LopHocPhan = reader["LopHocPhan"].ToString(); } catch { }
                        try { item.MaMonHoc = reader["MaMonHoc"].ToString(); } catch { }
                        try { item.OidChiTiet = new Guid(reader["OidChiTiet"].ToString()); } catch { }
                        try { item.TenBoPhan = reader["TenBoPhan"].ToString(); } catch { }
                        try { item.TenMonHoc = reader["TenMonHoc"].ToString(); } catch { }
                        try { item.SoHieuCongChuc = reader["SoHieuCongChuc"].ToString(); } catch { }
                        try { item.HoTen = reader["HoTen"].ToString(); } catch { }
                        items.Add(item);
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                HRMWebApp.Helpers.Helper.ErrorLog("DataClassHelper/spd_PMS_KiemTraThongTin_ImportTKB", ex);
                throw ex;
            }
        }

        public static int spd_PMS_ThoiKhoaBieu_PhanCongGiangDay_Web(Guid id_Chitiet, Guid giangVienId)
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
                    sqlCom.CommandText = "spd_PMS_ThoiKhoaBieu_PhanCongGiangDay_Web";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@Oid_Chitiet", id_Chitiet));
                    sqlCom.Parameters.Add(new SqlParameter("@NhanVien", giangVienId));
                    result = sqlCom.ExecuteNonQuery();
                }
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                HRMWebApp.Helpers.Helper.ErrorLog("DataClassHelper/spd_PMS_ThoiKhoaBieu_PhanCongGiangDay_Web", ex);
                throw ex;
            }
        }
        public static int spd_PMS_Import_TBK_WEB(Guid? NamHoc, Guid? HocKy, string StringSQL, string UserName)
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
                    sqlCom.CommandText = "spd_PMS_Import_TBK_WEB";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@StringSQL", StringSQL));
                    sqlCom.Parameters.Add(new SqlParameter("@UserName", UserName));
                    result = Convert.ToInt32(sqlCom.ExecuteNonQuery());
                }
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_Import_TBK_WEB", ex);
                throw ex;
            }
        }

        public static spd_PMS_KiemTraPhanQuyenImport spd_PMS_KiemTraPhanQuyenImport(Guid? NamHoc, Guid? HocKy, Guid? NhanVien)
        {
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_PMS_KiemTraPhanQuyenImport";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHoc));
                    sqlCom.Parameters.Add(new SqlParameter("@HocKy", HocKy));
                    sqlCom.Parameters.Add(new SqlParameter("@NhanVien", NhanVien));
                    var reader = sqlCom.ExecuteReader();

                    var item = new spd_PMS_KiemTraPhanQuyenImport();
                    while (reader.Read())
                    {
                        try { item.KetQua_Import = Convert.ToBoolean(Convert.ToInt32(reader["KetQua_Import"])); } catch { }
                        try { item.KetQua_XacNhan = Convert.ToBoolean(Convert.ToInt32(reader["KetQua_XacNhan"])); } catch { }
                    }
                    return item;
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/spd_PMS_KiemTraPhanQuyenImport", ex);
                throw ex;
            }
        }

        public static List<BacDaoTao> GetBacDaoTao()
        {
            try
            {
                List<BacDaoTao> result = new List<BacDaoTao>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "SELECT * FROM dbo.BacDaoTao WHERE GCRecord IS NULL";
                    sqlCom.CommandType = CommandType.Text;
                    sqlCom.CommandTimeout = 0;
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new BacDaoTao();
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.TenBacDaoTao = reader["TenBacDaoTao"].ToString(); } catch { }
                        result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/GetBacDaoTao", ex);
                throw ex;
            }
        }

        public static List<HeDaoTao> GetHeDaoTao()
        {
            try
            {
                List<HeDaoTao> result = new List<HeDaoTao>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "SELECT * FROM dbo.HeDaoTao WHERE GCRecord IS NULL";
                    sqlCom.CommandType = CommandType.Text;
                    sqlCom.CommandTimeout = 0;
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new HeDaoTao();
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.TenHeDaoTao = reader["TenHeDaoTao"].ToString(); } catch { }
                        result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/GetHeDaoTao", ex);
                throw ex;
            }
        }

        public static List<BoPhan> GetBoPhan()
        {
            try
            {
                List<BoPhan> result = new List<BoPhan>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "SELECT * FROM dbo.BoPhan WHERE LoaiBoPhan != 0 AND LoaiBoPhan != 1 AND GCRecord IS NULL ORDER BY BoPhanCha";
                    sqlCom.CommandType = CommandType.Text;
                    sqlCom.CommandTimeout = 0;
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new BoPhan();
                        try { item.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { item.TenBoPhan = reader["TenBoPhan"].ToString(); } catch { }
                        result.Add(item);
                    }
                }
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("DataClassHelper/GetBoPhan", ex);
                throw ex;
            }
        }
        public static int spd_DanhGiaABC_ThongThongTinUserTheoKy(Guid? kyDanhGiaId, Guid? userId)
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
                Helper.ErrorLog("DataClassHelper/spd_DanhGiaABC_ThongThongTinUserTheoKy ", ex);
                throw ex;
            }
        }
    }

    public class ESurveyObject
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string auth { get; set; }
        public string Link { get; set; }
    }
}
