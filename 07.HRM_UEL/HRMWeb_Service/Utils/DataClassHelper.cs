using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using HRMWeb_Service.DTO.BangLuong;

namespace HRMWeb_Service.Utils
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
        public static spd_Service_TongHopTienGiangVaThuNhapKhac spd_Service_TongHopTienGiangVaThuNhapKhac(Guid kyTinhLuong, Guid? soChungTu, Guid nhanVien, Guid? kyTinhPMS)
        {
            try
            {
                spd_Service_TongHopTienGiangVaThuNhapKhac result = new spd_Service_TongHopTienGiangVaThuNhapKhac();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_Service_TongHopTienGiangVaThuNhapKhac";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@KyTinhLuong", kyTinhLuong));
                    sqlCom.Parameters.Add(new SqlParameter("@SoChungTuHienWeb", soChungTu ?? Guid.Empty));
                    sqlCom.Parameters.Add(new SqlParameter("@NhanVien", nhanVien));
                    sqlCom.Parameters.Add(new SqlParameter("@KyTinhPMS", kyTinhPMS ?? Guid.Empty));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        ThongTinCanBo newItem = new ThongTinCanBo();
                        try { newItem.HoTen = reader["HoTen"].ToString(); } catch { }
                        try { newItem.MaSoThue = reader["MaSoThue"].ToString(); } catch { }
                        try { newItem.SoNguoiPhuThuoc = reader["SoNguoiPhuThuoc"].ToString(); } catch { }
                        try { newItem.MaNhanSu = reader["MaNhanSu"].ToString(); } catch { }
                        try { newItem.NgachLuong = reader["NgachLuong"].ToString(); } catch { }
                        try { newItem.BacLuong = reader["BacLuong"].ToString(); } catch { }
                        try { newItem.HeSoLuong = Convert.ToDecimal(reader["HeSoLuong"]); } catch { }
                        try { newItem.HeSoPhuCapChucVu = Convert.ToDecimal(reader["HeSoPhuCapChucVu"]); } catch { }
                        try { newItem.LuongCoBan = reader["LuongCoBan"].ToString(); } catch { }
                        try { newItem.TyLeHuongLuong = reader["TyLeHuongLuong"].ToString(); } catch { }
                        try { newItem.XepLoai = reader["XepLoai"].ToString(); } catch { }
                        try { newItem.HeSoTNTT = reader["HeSoTNTT"].ToString(); } catch { }
                        try { newItem.MucGiamTru = reader["MucGiamTru"].ToString(); } catch { }
                        try { newItem.DonVi = reader["DonVi"].ToString(); } catch { }
                        try { newItem.NganHang = reader["NganHang"].ToString(); } catch { }
                        result.ThongTinCanBo = newItem;
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        TongHopLuong newItem = new TongHopLuong();
                        try { newItem.LuongNgachBac = Convert.ToDecimal(reader["LuongNgachBac"]); } catch { }
                        try { newItem.ThuNhapTangThem = Convert.ToDecimal(reader["ThuNhapTangThem"]); } catch { }
                        try { newItem.TruyLinh = Convert.ToDecimal(reader["TruyLinh"]); } catch { }
                        try { newItem.ThuNhapKhac = Convert.ToDecimal(reader["ThuNhapKhac"]); } catch { }
                        try { newItem.ThueTNCN = Convert.ToDecimal(reader["ThueTNCN"]); } catch { }
                        try { newItem.CacKhoanKhauTru = Convert.ToDecimal(reader["CacKhoanKhauTru"]); } catch { }
                        try { newItem.CacKhoanPhuCap = Convert.ToDecimal(reader["CacKhoanPhuCap"]); } catch { }
                        try { newItem.KhenThuong = Convert.ToDecimal(reader["KhenThuong"]); } catch { }
                        result.TongHopLuong = newItem;
                    }

                    reader.NextResult();
                    result.ChiTietLuong_PhuCap = new List<ChiTietLuong_PhuCap>();
                    decimal tongSoTien = 0;
                    decimal tongSoTienChiuThue = 0;
                    while (reader.Read())
                    {
                        ChiTietLuong_PhuCap newItem = new ChiTietLuong_PhuCap();
                        try { newItem.DienGiai = reader["DienGiai"].ToString(); } catch { }
                        try { newItem.SoTien = Convert.ToDecimal(reader["SoTien"]); } catch { }
                        try { newItem.SoTienChiuThue = Convert.ToDecimal(reader["SoTienChiuThue"]); } catch { }
                        result.ChiTietLuong_PhuCap.Add(newItem);
                        tongSoTien += newItem.SoTien ?? 0;
                        tongSoTienChiuThue += newItem.SoTienChiuThue ?? 0;
                    }
                    if (result.ChiTietLuong_PhuCap.Count() > 0)
                    {
                        result.ChiTietLuong_PhuCap.Add(new ChiTietLuong_PhuCap() { DienGiai = "Tổng cộng", SoTien = tongSoTien, SoTienChiuThue = tongSoTienChiuThue });
                    }

                    reader.NextResult();
                    result.ChiTietLuong_KhenThuongPhucLoi = new List<ChiTietLuong_KhenThuongPhucLoi>();
                    tongSoTien = 0;
                    tongSoTienChiuThue = 0;
                    while (reader.Read())
                    {
                        ChiTietLuong_KhenThuongPhucLoi newItem = new ChiTietLuong_KhenThuongPhucLoi();
                        try { newItem.DienGiai = reader["TenLoai"].ToString(); } catch { }
                        try { newItem.SoTien = Convert.ToDecimal(reader["SoTien"]); } catch { }
                        try { newItem.SoTienChiuThue = Convert.ToDecimal(reader["SoTienChiuThue"]); } catch { }
                        result.ChiTietLuong_KhenThuongPhucLoi.Add(newItem);
                        tongSoTien += newItem.SoTien ?? 0;
                        tongSoTienChiuThue += newItem.SoTienChiuThue ?? 0;
                    }
                    if (result.ChiTietLuong_KhenThuongPhucLoi.Count() > 0)
                    {
                        result.ChiTietLuong_KhenThuongPhucLoi.Add(new ChiTietLuong_KhenThuongPhucLoi() { DienGiai = "Tổng cộng", SoTien = tongSoTien, SoTienChiuThue = tongSoTienChiuThue });
                    }

                    reader.NextResult();
                    result.ChiTietLuong_ThuNhapKhac = new List<ChiTietLuong_ThuNhapKhac>();
                    tongSoTien = 0;
                    tongSoTienChiuThue = 0;
                    while (reader.Read())
                    {
                        ChiTietLuong_ThuNhapKhac newItem = new ChiTietLuong_ThuNhapKhac();
                        try { newItem.DienGiai = reader["TenLoaiThuNhapKhac"].ToString(); } catch { }
                        try { newItem.SoTien = Convert.ToDecimal(reader["SoTien"]); } catch { }
                        try { newItem.SoTienChiuThue = Convert.ToDecimal(reader["SoTienChiuThue"]); } catch { }
                        result.ChiTietLuong_ThuNhapKhac.Add(newItem);
                        tongSoTien += newItem.SoTien ?? 0;
                        tongSoTienChiuThue += newItem.SoTienChiuThue ?? 0;
                    }
                    if (result.ChiTietLuong_ThuNhapKhac.Count() > 0)
                    {
                        result.ChiTietLuong_ThuNhapKhac.Add(new ChiTietLuong_ThuNhapKhac() { DienGiai = "Tổng cộng", SoTien = tongSoTien, SoTienChiuThue = tongSoTienChiuThue });
                    }

                    reader.NextResult();
                    result.ChiTietLuong_KhauTru = new List<ChiTietLuong_KhauTru>();
                    tongSoTien = 0;
                    tongSoTienChiuThue = 0;
                    while (reader.Read())
                    {
                        ChiTietLuong_KhauTru newItem = new ChiTietLuong_KhauTru();
                        try { newItem.DienGiai = reader["DienGiai"].ToString(); } catch { }
                        try { newItem.TyLeTru = reader["TyLeTru"].ToString(); } catch { }
                        try { newItem.SoTien = Convert.ToDecimal(reader["SoTien"]); } catch { }
                        try { newItem.SoTienChiuThue = Convert.ToDecimal(reader["SoTienChiuThue"]); } catch { }
                        result.ChiTietLuong_KhauTru.Add(newItem);
                        tongSoTien += newItem.SoTien ?? 0;
                        tongSoTienChiuThue += newItem.SoTienChiuThue ?? 0;
                    }
                    if (result.ChiTietLuong_KhauTru.Count() > 0)
                    {
                        result.ChiTietLuong_KhauTru.Add(new ChiTietLuong_KhauTru() { DienGiai = "Tổng cộng", SoTien = tongSoTien, SoTienChiuThue = tongSoTienChiuThue });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static spd_BangThuLaoNhanVien_ChiTietThanhToanPMS spd_BangThuLaoNhanVien_ChiTietThanhToanPMS(Guid nhanVien, Guid kyTinhLuong, Guid? soChungTu, Guid? kyTinhPMS)
        {
            try
            {
                spd_BangThuLaoNhanVien_ChiTietThanhToanPMS result = new spd_BangThuLaoNhanVien_ChiTietThanhToanPMS();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_BangThuLaoNhanVien_ChiTietThanhToanPMS";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@KyTinhLuong", kyTinhLuong));
                    sqlCom.Parameters.Add(new SqlParameter("@NhanVien", nhanVien));
                    sqlCom.Parameters.Add(new SqlParameter("@ChungTu", soChungTu ?? Guid.Empty));
                    sqlCom.Parameters.Add(new SqlParameter("@KyTinhPMS", kyTinhPMS ?? Guid.Empty));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    result.PMS1 = new List<spd_BangThuLaoNhanVien_ChiTietThanhToanPMS1>();
                    while (reader.Read())
                    {
                        spd_BangThuLaoNhanVien_ChiTietThanhToanPMS1 newItem = new spd_BangThuLaoNhanVien_ChiTietThanhToanPMS1();
                        try { newItem.KhoanChi = reader["KhoanChi"].ToString(); } catch { }
                        try { newItem.TenMonHoc = reader["TenMonHoc"].ToString(); } catch { }
                        try { newItem.LopHocPhan = reader["LopHocPhan"].ToString(); } catch { }
                        try { newItem.SoBaiQuaTrinh = Convert.ToDecimal(reader["SoBaiQuaTrinh"]); } catch { }
                        try { newItem.SoBaiGiuaKy = Convert.ToDecimal(reader["SoBaiGiuaKy"]); } catch { }
                        try { newItem.SoBaiCuoiKy = Convert.ToDecimal(reader["SoBaiCuoiKy"]); } catch { }
                        try { newItem.DonGiaQuaTrinh = Convert.ToDecimal(reader["DonGiaQuaTrinh"]); } catch { }
                        try { newItem.DonGiaGiuaKy = Convert.ToDecimal(reader["DonGiaGiuaKy"]); } catch { }
                        try { newItem.DonGiaCuoiKy = Convert.ToDecimal(reader["DonGiaCuoiKy"]); } catch { }
                        try { newItem.TongTien = Convert.ToDecimal(reader["TongTien"]); } catch { }
                        result.PMS1.Add(newItem);
                    }
                    if (result.PMS1.Count() > 0)
                    {
                        spd_BangThuLaoNhanVien_ChiTietThanhToanPMS1 lastItem1 = new spd_BangThuLaoNhanVien_ChiTietThanhToanPMS1();
                        lastItem1.TenMonHoc = "Tổng cộng";
                        lastItem1.TongTien = result.PMS1.Sum(q => q.TongTien);
                        result.PMS1.Add(lastItem1);
                    }

                    reader.NextResult();
                    result.PMS2 = new List<spd_BangThuLaoNhanVien_ChiTietThanhToanPMS2>();
                    while (reader.Read())
                    {
                        spd_BangThuLaoNhanVien_ChiTietThanhToanPMS2 newItem = new spd_BangThuLaoNhanVien_ChiTietThanhToanPMS2();
                        try { newItem.TenMonHoc = reader["TenMonHoc"].ToString(); } catch { }
                        try { newItem.KhoanChi = reader["KhoanChi"].ToString(); } catch { }
                        try { newItem.LopHocPhan = reader["LopHocPhan"].ToString(); } catch { }
                        try { newItem.SoTiet = Convert.ToDecimal(reader["SoTiet"]); } catch { }
                        try { newItem.SoLuongSV = Convert.ToDecimal(reader["SoLuongSV"]); } catch { }
                        try { newItem.HeSo_ChucDanh = Convert.ToDecimal(reader["HeSo_ChucDanh"]); } catch { }
                        try { newItem.HeSo_CoSo = Convert.ToDecimal(reader["HeSo_CoSo"]); } catch { }
                        try { newItem.HeSo_LopDong = Convert.ToDecimal(reader["HeSo_LopDong"]); } catch { }
                        try { newItem.HeSo_Khac = Convert.ToDecimal(reader["HeSo_Khac"]); } catch { }
                        try { newItem.TongHeSo = Convert.ToDecimal(reader["TongHeSo"]); } catch { }
                        try { newItem.ChiPhiDiLai = Convert.ToDecimal(reader["ChiPhiDiLai"]); } catch { }
                        try { newItem.DonGiaTietChuan = Convert.ToDecimal(reader["DonGiaTietChuan"]); } catch { }
                        try { newItem.ThanhTien = Convert.ToDecimal(reader["ThanhTien"]); } catch { }
                        try { newItem.TongTien = Convert.ToDecimal(reader["TongTien"]); } catch { }
                        try { newItem.NoGioHKTruoc = Convert.ToDecimal(reader["NoGioHKTruoc"]); } catch { }
                        try { newItem.NoGioHKNay = Convert.ToDecimal(reader["NoGioHKNay"]); } catch { }
                        try { newItem.TongTienNo = Convert.ToDecimal(reader["TongTienNo"]); } catch { }
                        try { newItem.ThueTNCNTamTru = Convert.ToDecimal(reader["ThueTNCNTamTru"]); } catch { }
                        try { newItem.ConLaiThanhToan = Convert.ToDecimal(reader["ConLaiThanhToan"]); } catch { }
                        result.PMS2.Add(newItem);
                    }
                    if (result.PMS2.Count() > 0)
                    {
                        spd_BangThuLaoNhanVien_ChiTietThanhToanPMS2 lastItem2 = new spd_BangThuLaoNhanVien_ChiTietThanhToanPMS2();
                        lastItem2.TenMonHoc = "Tổng cộng";
                        lastItem2.ChiPhiDiLai = result.PMS2.Sum(q => q.ChiPhiDiLai);
                        lastItem2.DonGiaTietChuan = result.PMS2.Sum(q => q.DonGiaTietChuan);
                        lastItem2.ThanhTien = result.PMS2.Sum(q => q.ThanhTien);
                        lastItem2.TongTien = result.PMS2.Sum(q => q.TongTien);
                        lastItem2.TongTienNo = result.PMS2.Sum(q => q.TongTienNo);
                        lastItem2.ThueTNCNTamTru = result.PMS2.Sum(q => q.ThueTNCNTamTru);
                        lastItem2.ConLaiThanhToan = result.PMS2.Sum(q => q.ConLaiThanhToan);
                        result.PMS2.Add(lastItem2);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        try { result.SoTien = Convert.ToDecimal(reader["SoTien"]); } catch { }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<spd_PMS_Web_ChiTietTheoDoiTruTietChuan> spd_PMS_Web_ChiTietTheoDoiTruTietChuan(Guid nhanVien, Guid kyTinhLuong, Guid soChungTu)
        {
            try
            {
                List<spd_PMS_Web_ChiTietTheoDoiTruTietChuan> result = new List<spd_PMS_Web_ChiTietTheoDoiTruTietChuan>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_PMS_Web_ChiTietTheoDoiTruTietChuan";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@KyTinhLuong", kyTinhLuong));
                    sqlCom.Parameters.Add(new SqlParameter("@NhanVien", nhanVien));
                    sqlCom.Parameters.Add(new SqlParameter("@ChungTu", soChungTu));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        spd_PMS_Web_ChiTietTheoDoiTruTietChuan newItem = new spd_PMS_Web_ChiTietTheoDoiTruTietChuan();
                        try { newItem.TietChuanTruocThanhToan = Convert.ToDecimal(reader["TietChuanTruocThanhToan"]); } catch { }
                        try { newItem.TietChuanDaTruThanhToan = Convert.ToDecimal(reader["TietChuanDaTruThanhToan"]); } catch { }
                        try { newItem.TietChuanConLaiSauThanhToan = Convert.ToDecimal(reader["TietChuanConLaiSauThanhToan"]); } catch { }
                        result.Add(newItem);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool spd_WebUser_KiemTraLoaiNhanVien(string MaQuanLy)
        {
            try
            {
                bool result = true;
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_WebUser_KiemTraLoaiNhanVien";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@MaQuanLy", MaQuanLy));
                    result = Convert.ToBoolean(sqlCom.ExecuteScalar());
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<spd_PMS_ThuLaoGiangDay_ThinhGiang> spd_PMS_ThuLaoGiangDay_ThinhGiang(Guid nhanVien, Guid kyTinhLuong, Guid kyTinhPMS)
        {
            try
            {
                List<spd_PMS_ThuLaoGiangDay_ThinhGiang> result = new List<spd_PMS_ThuLaoGiangDay_ThinhGiang>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_PMS_ThuLaoGiangDay_ThinhGiang";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@KyTinhLuong", kyTinhLuong));
                    sqlCom.Parameters.Add(new SqlParameter("@NhanVien", nhanVien));
                    sqlCom.Parameters.Add(new SqlParameter("@KyTinhPMS", kyTinhPMS));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        spd_PMS_ThuLaoGiangDay_ThinhGiang newItem = new spd_PMS_ThuLaoGiangDay_ThinhGiang();
                        try { newItem.TongThuNhap = Convert.ToDecimal(reader["TongThuNhap"]); } catch { }
                        try { newItem.ThueTNCN = Convert.ToDecimal(reader["ThueTNCN"]); } catch { }
                        try { newItem.ThucNhan = Convert.ToDecimal(reader["ThucNhan"]); } catch { }
                        result.Add(newItem);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<NamHocDTO> GetNamHocPMS()
        {
            try
            {
                List<NamHocDTO> result = new List<NamHocDTO>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = @"SELECT NamHoc.Oid, TenNamHoc FROM dbo.NamHoc
                                           JOIN dbo.KyTinhPMS ON KyTinhPMS.NamHoc = NamHoc.Oid
                                           GROUP BY NamHoc.Oid, NgayBatDau, TenNamHoc
                                           ORDER BY NgayBatDau DESC";
                    sqlCom.CommandType = CommandType.Text;
                    sqlCom.CommandTimeout = 0;
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        NamHocDTO newItem = new NamHocDTO();
                        try { newItem.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { newItem.TenNamHoc = reader["TenNamHoc"].ToString(); } catch { }
                        result.Add(newItem);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<KyTinhPMSDTO> GetKyTinhPMSTheoNamHoc(Guid NamHocOid)
        {
            try
            {
                List<KyTinhPMSDTO> result = new List<KyTinhPMSDTO>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = @"SELECT Oid, Dot
                                           FROM dbo.KyTinhPMS
                                           WHERE NamHoc = @NamHoc
                                           ORDER BY Dot";
                    sqlCom.CommandType = CommandType.Text;
                    sqlCom.Parameters.Add(new SqlParameter("@NamHoc", NamHocOid));
                    sqlCom.CommandTimeout = 0;
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        KyTinhPMSDTO newItem = new KyTinhPMSDTO();
                        try { newItem.Oid = new Guid(reader["Oid"].ToString()); } catch { }
                        try { newItem.Dot = reader["Dot"].ToString(); } catch { }
                        result.Add(newItem);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class NamHocDTO
    {
        public Guid Oid { get; set; }
        public string TenNamHoc { get; set; }
    }
    public class KyTinhPMSDTO
    {
        public Guid Oid { get; set; }
        public string Dot { get; set; }
    }
}