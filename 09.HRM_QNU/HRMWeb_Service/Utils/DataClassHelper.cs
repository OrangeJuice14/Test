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
        public static spd_Service_TongHopTienGiangVaThuNhapKhac spd_Service_TongHopTienGiangVaThuNhapKhac(Guid kyTinhLuong, Guid soChungTu, Guid nhanVien)
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
                    sqlCom.Parameters.Add(new SqlParameter("@SoChungTuHienWeb", soChungTu));
                    sqlCom.Parameters.Add(new SqlParameter("@NhanVien", nhanVien));
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
                HRMWebApp.Helpers.Helper.ErrorLog("DataClassHelper/spd_Service_TongHopTienGiangVaThuNhapKhac", ex);
                throw ex;
            }
        }

        public static List<spd_Service_GetSoChungTuHienWeb> spd_Service_GetSoChungTuHienWeb(Guid kyTinhLuong)
        {
            try
            {
                List<spd_Service_GetSoChungTuHienWeb> result = new List<spd_Service_GetSoChungTuHienWeb>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_Service_GetSoChungTuHienWeb";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@KyTinhLuong", kyTinhLuong));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        spd_Service_GetSoChungTuHienWeb item = new spd_Service_GetSoChungTuHienWeb();
                        try { item.Id = Guid.Parse(reader["Id"].ToString()); } catch { }
                        try { item.Name = reader["Name"].ToString(); } catch { }
                        try { item.KyTinhLuong = Guid.Parse(reader["KyTinhLuong"].ToString()); } catch { }
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                HRMWebApp.Helpers.Helper.ErrorLog("DataClassHelper/spd_Service_GetSoChungTuHienWeb", ex);
                throw ex;
            }
        }

        public static IEnumerable<spd_Service_TongHopThueTNCNTrongThang> spd_Service_TongHopThueTNCNTrongThang(Guid nhanVien, Guid kyTinhLuong, Guid soChungTu)
        {
            try
            {
                List<spd_Service_TongHopThueTNCNTrongThang> result = new List<spd_Service_TongHopThueTNCNTrongThang>();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                using (connection)
                {
                    SqlCommand sqlCom = new SqlCommand();
                    sqlCom.Connection = connection;
                    sqlCom.CommandText = "spd_Service_TongHopThueTNCNTrongThang";
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 0;
                    sqlCom.Parameters.Add(new SqlParameter("@NhanVien", nhanVien));
                    sqlCom.Parameters.Add(new SqlParameter("@KyTinhLuong", kyTinhLuong));
                    sqlCom.Parameters.Add(new SqlParameter("@SoChungTuHienWeb", soChungTu));
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        spd_Service_TongHopThueTNCNTrongThang item = new spd_Service_TongHopThueTNCNTrongThang();
                        try { item.HoTen = reader["HoTen"].ToString(); } catch { }
                        try { item.MaSoThue = reader["MaSoThue"].ToString(); } catch { }
                        try { item.TongThuNhap = Convert.ToDecimal(reader["TongThuNhap"]); } catch { }
                        try { item.TongThuNhapChiuThue = Convert.ToDecimal(reader["TongThuNhapChiuThue"]); } catch { }
                        try { item.MienThue = Convert.ToDecimal(reader["MienThue"]); } catch { }
                        try { item.TongGiamTru = Convert.ToDecimal(reader["TongGiamTru"]); } catch { }
                        try { item.GiamTruGiaCanh = Convert.ToDecimal(reader["GiamTruGiaCanh"]); } catch { }
                        try { item.GiamTruBanThan = Convert.ToDecimal(reader["GiamTruBanThan"]); } catch { }
                        try { item.GiamTruBaoHiem = Convert.ToDecimal(reader["GiamTruBaoHiem"]); } catch { }
                        try { item.SoNguoiPhuThuoc = Convert.ToDecimal(reader["SoNguoiPhuThuoc"]); } catch { }
                        try { item.ThuNhapTinhThue = Convert.ToDecimal(reader["ThuNhapTinhThue"]); } catch { }
                        try { item.ThueTNCNPhaiNop = Convert.ToDecimal(reader["ThueTNCNPhaiNop"]); } catch { }
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                HRMWebApp.Helpers.Helper.ErrorLog("DataClassHelper/spd_Service_TongHopThueTNCNTrongThang", ex);
                throw ex;
            }
        }
    }
}