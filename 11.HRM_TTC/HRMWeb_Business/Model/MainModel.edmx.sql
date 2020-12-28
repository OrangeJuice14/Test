
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/24/2019 11:40:34
-- Generated from EDMX file: D:\Projects\HRMWeb_TTC\HRMWeb_Business\Model\MainModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ERP_TTC_New];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BacLuong_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BacLuong] DROP CONSTRAINT [FK_BacLuong_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_BacLuong_NgachLuong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BacLuong] DROP CONSTRAINT [FK_BacLuong_NgachLuong];
GO
IF OBJECT_ID(N'[dbo].[FK_BoPhan_BoPhanCha]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BoPhan] DROP CONSTRAINT [FK_BoPhan_BoPhanCha];
GO
IF OBJECT_ID(N'[dbo].[FK_BoPhan_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BoPhan] DROP CONSTRAINT [FK_BoPhan_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_Attachments_CC_KhaiBaoCongTac]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_Attachments] DROP CONSTRAINT [FK_CC_Attachments_CC_KhaiBaoCongTac];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_CaChamCong_HinhThucNghi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_CaChamCong] DROP CONSTRAINT [FK_CC_CaChamCong_HinhThucNghi];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_CC_HinhThucNghi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_CC_HinhThucNghi];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_Department]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_Department];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_IDNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_IDNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_WebUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_WebUser];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgoaiGio_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgoaiGio] DROP CONSTRAINT [FK_CC_ChamCongNgoaiGio_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgoaiGio_LoaiNgayNgoaiGio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgoaiGio] DROP CONSTRAINT [FK_CC_ChamCongNgoaiGio_LoaiNgayNgoaiGio];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgoaiGio_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgoaiGio] DROP CONSTRAINT [FK_CC_ChamCongNgoaiGio_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongTheoNgay_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongTheoNgay] DROP CONSTRAINT [FK_CC_ChamCongTheoNgay_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongTheoNgay_CC_CaChamCong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongTheoNgay] DROP CONSTRAINT [FK_CC_ChamCongTheoNgay_CC_CaChamCong];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongTheoNgay_CC_HinhThucKhac]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongTheoNgay] DROP CONSTRAINT [FK_CC_ChamCongTheoNgay_CC_HinhThucKhac];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongTheoNgay_CC_HinhThucNghi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongTheoNgay] DROP CONSTRAINT [FK_CC_ChamCongTheoNgay_CC_HinhThucNghi];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongTheoNgay_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongTheoNgay] DROP CONSTRAINT [FK_CC_ChamCongTheoNgay_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongTheoNgay_HoSo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongTheoNgay] DROP CONSTRAINT [FK_CC_ChamCongTheoNgay_HoSo];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietChamCong_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietChamCong] DROP CONSTRAINT [FK_CC_ChiTietChamCong_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietChamCong_QuanLyChamCong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietChamCong] DROP CONSTRAINT [FK_CC_ChiTietChamCong_QuanLyChamCong];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietChamCong_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietChamCong] DROP CONSTRAINT [FK_CC_ChiTietChamCong_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietCongNgoaiGio_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietCongNgoaiGio] DROP CONSTRAINT [FK_CC_ChiTietCongNgoaiGio_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietCongNgoaiGio_QuanLyCongNgoaiGio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietCongNgoaiGio] DROP CONSTRAINT [FK_CC_ChiTietCongNgoaiGio_QuanLyCongNgoaiGio];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietCongNgoaiGio_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietCongNgoaiGio] DROP CONSTRAINT [FK_CC_ChiTietCongNgoaiGio_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietNghiPhep_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietNghiPhep] DROP CONSTRAINT [FK_CC_ChiTietNghiPhep_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietNghiPhep_CC_QuanLyNghiPhep]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietNghiPhep] DROP CONSTRAINT [FK_CC_ChiTietNghiPhep_CC_QuanLyNghiPhep];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietNghiPhep_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietNghiPhep] DROP CONSTRAINT [FK_CC_ChiTietNghiPhep_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietPhepHe_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietPhepHe] DROP CONSTRAINT [FK_CC_ChiTietPhepHe_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietPhepHe_CC_QuanLyPhepHe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietPhepHe] DROP CONSTRAINT [FK_CC_ChiTietPhepHe_CC_QuanLyPhepHe];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietPhepHe_ChucDanh]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietPhepHe] DROP CONSTRAINT [FK_CC_ChiTietPhepHe_ChucDanh];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChiTietPhepHe_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChiTietPhepHe] DROP CONSTRAINT [FK_CC_ChiTietPhepHe_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_DangKyKhungGioLamViec_CC_CaChamCong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_DangKyKhungGioLamViec] DROP CONSTRAINT [FK_CC_DangKyKhungGioLamViec_CC_CaChamCong];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_DangKyKhungGioLamViec_CC_KyDangKyKhungGio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_DangKyKhungGioLamViec] DROP CONSTRAINT [FK_CC_DangKyKhungGioLamViec_CC_KyDangKyKhungGio];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_DangKyKhungGioLamViec_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_DangKyKhungGioLamViec] DROP CONSTRAINT [FK_CC_DangKyKhungGioLamViec_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_HinhThucChieu_CC_HinhThucNghi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_HinhThucKhac] DROP CONSTRAINT [FK_CC_HinhThucChieu_CC_HinhThucNghi];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_HinhThucKhac_CC_ChamCongTheoNgay]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_HinhThucKhac] DROP CONSTRAINT [FK_CC_HinhThucKhac_CC_ChamCongTheoNgay];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_HinhThucNghi_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_HinhThucNghi] DROP CONSTRAINT [FK_CC_HinhThucNghi_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_HinhThucSang_CC_HinhThucNghi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_HinhThucKhac] DROP CONSTRAINT [FK_CC_HinhThucSang_CC_HinhThucNghi];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_KhaiBaoCongTac_HoSo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_KhaiBaoCongTac] DROP CONSTRAINT [FK_CC_KhaiBaoCongTac_HoSo];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_KhaiBaoCongTac_NguoiKy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_KhaiBaoCongTac] DROP CONSTRAINT [FK_CC_KhaiBaoCongTac_NguoiKy];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_KyChamCong_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_KyChamCong] DROP CONSTRAINT [FK_CC_KyChamCong_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_MailManager_WebUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_MailManager] DROP CONSTRAINT [FK_CC_MailManager_WebUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_NgayNghiTrongNam_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_NgayNghiTrongNam] DROP CONSTRAINT [FK_CC_NgayNghiTrongNam_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_QuanLyChamCong_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyChamCong] DROP CONSTRAINT [FK_CC_QuanLyChamCong_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_QuanLyChamCong_KyChamCong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyChamCong] DROP CONSTRAINT [FK_CC_QuanLyChamCong_KyChamCong];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_QuanLyCongNgoaiGio_CC_KyChamCong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyCongNgoaiGio] DROP CONSTRAINT [FK_CC_QuanLyCongNgoaiGio_CC_KyChamCong];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_QuanLyCongNgoaiGio_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyCongNgoaiGio] DROP CONSTRAINT [FK_CC_QuanLyCongNgoaiGio_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_QuanLyNghiPhep_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyNghiPhep] DROP CONSTRAINT [FK_CC_QuanLyNghiPhep_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_QuanLyNghiPhep_NamHoc]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyNghiPhep] DROP CONSTRAINT [FK_CC_QuanLyNghiPhep_NamHoc];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_QuanLyNghiPhep_NienDoTaiChinh]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyNghiPhep] DROP CONSTRAINT [FK_CC_QuanLyNghiPhep_NienDoTaiChinh];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_QuanLyPhepHe_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyPhepHe] DROP CONSTRAINT [FK_CC_QuanLyPhepHe_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_QuanLyPhepHe_NienDoTaiChinh]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyPhepHe] DROP CONSTRAINT [FK_CC_QuanLyPhepHe_NienDoTaiChinh];
GO
IF OBJECT_ID(N'[dbo].[FK_cc_QuanLyViPham_CC_ChamCongTheoNgay]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyViPham] DROP CONSTRAINT [FK_cc_QuanLyViPham_CC_ChamCongTheoNgay];
GO
IF OBJECT_ID(N'[dbo].[FK_cc_QuanLyViPham_cc_HinhThucViPham]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyViPham] DROP CONSTRAINT [FK_cc_QuanLyViPham_cc_HinhThucViPham];
GO
IF OBJECT_ID(N'[dbo].[FK_ChucDanh_ChucVu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChucDanh] DROP CONSTRAINT [FK_ChucDanh_ChucVu];
GO
IF OBJECT_ID(N'[dbo].[FK_ChucVu_WebGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChucVu] DROP CONSTRAINT [FK_ChucVu_WebGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_ChucVuKiemNhiem_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChucVuKiemNhiem] DROP CONSTRAINT [FK_ChucVuKiemNhiem_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_ChucVuKiemNhiem_ChucVu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChucVuKiemNhiem] DROP CONSTRAINT [FK_ChucVuKiemNhiem_ChucVu];
GO
IF OBJECT_ID(N'[dbo].[FK_ChucVuKiemNhiem_NhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChucVuKiemNhiem] DROP CONSTRAINT [FK_ChucVuKiemNhiem_NhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CongTy_Oid]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CongTy] DROP CONSTRAINT [FK_CongTy_Oid];
GO
IF OBJECT_ID(N'[dbo].[FK_GiayToHoSo_HoSo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiayToHoSo] DROP CONSTRAINT [FK_GiayToHoSo_HoSo];
GO
IF OBJECT_ID(N'[dbo].[FK_GiayToHoSo_LoaiGiayTo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiayToHoSo] DROP CONSTRAINT [FK_GiayToHoSo_LoaiGiayTo];
GO
IF OBJECT_ID(N'[dbo].[FK_KyTinhLuong_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KyTinhLuong] DROP CONSTRAINT [FK_KyTinhLuong_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_KyTinhLuong_QuanLyChamCong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KyTinhLuong] DROP CONSTRAINT [FK_KyTinhLuong_QuanLyChamCong];
GO
IF OBJECT_ID(N'[dbo].[FK_KyTinhLuong_QuanLyCongKhac]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KyTinhLuong] DROP CONSTRAINT [FK_KyTinhLuong_QuanLyCongKhac];
GO
IF OBJECT_ID(N'[dbo].[FK_KyTinhLuong_QuanLyCongNgoaiGio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KyTinhLuong] DROP CONSTRAINT [FK_KyTinhLuong_QuanLyCongNgoaiGio];
GO
IF OBJECT_ID(N'[dbo].[FK_NgachLuong_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NgachLuong] DROP CONSTRAINT [FK_NgachLuong_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_NgachLuong_TotKhung]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NgachLuong] DROP CONSTRAINT [FK_NgachLuong_TotKhung];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_ChucDanh]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_ChucDanh];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_NhanVienThongTinLuong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_NhanVienThongTinLuong];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_NhanVienTrinhDo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_NhanVienTrinhDo];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_Oid]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_Oid];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_TinhTrang]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_TinhTrang];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVienThongTinLuong_BacLuong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVienThongTinLuong] DROP CONSTRAINT [FK_NhanVienThongTinLuong_BacLuong];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVienThongTinLuong_NgachLuong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVienThongTinLuong] DROP CONSTRAINT [FK_NhanVienThongTinLuong_NgachLuong];
GO
IF OBJECT_ID(N'[dbo].[FK_NienDoTaiChinh_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NienDoTaiChinh] DROP CONSTRAINT [FK_NienDoTaiChinh_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_ThongTinNhanVien_ChucVu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThongTinNhanVien] DROP CONSTRAINT [FK_ThongTinNhanVien_ChucVu];
GO
IF OBJECT_ID(N'[dbo].[FK_ThongTinNhanVien_LoaiNhanSu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThongTinNhanVien] DROP CONSTRAINT [FK_ThongTinNhanVien_LoaiNhanSu];
GO
IF OBJECT_ID(N'[dbo].[FK_ThongTinNhanVien_Oid]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThongTinNhanVien] DROP CONSTRAINT [FK_ThongTinNhanVien_Oid];
GO
IF OBJECT_ID(N'[dbo].[FK_ThongTinNhanVien_TaiBoMon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThongTinNhanVien] DROP CONSTRAINT [FK_ThongTinNhanVien_TaiBoMon];
GO
IF OBJECT_ID(N'[dbo].[FK_WebMenu_Role_WebGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebMenu_Role] DROP CONSTRAINT [FK_WebMenu_Role_WebGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_WebMenu_Role_WebMenu1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebMenu_Role] DROP CONSTRAINT [FK_WebMenu_Role_WebMenu1];
GO
IF OBJECT_ID(N'[dbo].[FK_WebMenu_WebMenu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebMenu] DROP CONSTRAINT [FK_WebMenu_WebMenu];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUser_BoPhan_BoPhanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUser_BoPhan] DROP CONSTRAINT [FK_WebUser_BoPhan_BoPhanID];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUser_BoPhan_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUser_BoPhan] DROP CONSTRAINT [FK_WebUser_BoPhan_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUser_BoPhan_WebGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUser_BoPhan] DROP CONSTRAINT [FK_WebUser_BoPhan_WebGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUser_BoPhan_WebUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUser_BoPhan] DROP CONSTRAINT [FK_WebUser_BoPhan_WebUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUsers_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUsers] DROP CONSTRAINT [FK_WebUsers_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUsers_CongTy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUsers] DROP CONSTRAINT [FK_WebUsers_CongTy];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUsers_HoSo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUsers] DROP CONSTRAINT [FK_WebUsers_HoSo];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUsers_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUsers] DROP CONSTRAINT [FK_WebUsers_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUsers_WebGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUsers] DROP CONSTRAINT [FK_WebUsers_WebGroup];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BacLuong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BacLuong];
GO
IF OBJECT_ID(N'[dbo].[BoPhan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BoPhan];
GO
IF OBJECT_ID(N'[dbo].[CC_Attachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_Attachments];
GO
IF OBJECT_ID(N'[dbo].[CC_AttLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_AttLog];
GO
IF OBJECT_ID(N'[dbo].[CC_CaChamCong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_CaChamCong];
GO
IF OBJECT_ID(N'[dbo].[CC_CauHinhChamCong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_CauHinhChamCong];
GO
IF OBJECT_ID(N'[dbo].[CC_ChamCongNgayNghi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ChamCongNgayNghi];
GO
IF OBJECT_ID(N'[dbo].[CC_ChamCongNgoaiGio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ChamCongNgoaiGio];
GO
IF OBJECT_ID(N'[dbo].[CC_ChamCongTheoNgay]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ChamCongTheoNgay];
GO
IF OBJECT_ID(N'[dbo].[CC_ChiTietChamCong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ChiTietChamCong];
GO
IF OBJECT_ID(N'[dbo].[CC_ChiTietCongNgoaiGio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ChiTietCongNgoaiGio];
GO
IF OBJECT_ID(N'[dbo].[CC_ChiTietNghiPhep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ChiTietNghiPhep];
GO
IF OBJECT_ID(N'[dbo].[CC_ChiTietPhepHe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ChiTietPhepHe];
GO
IF OBJECT_ID(N'[dbo].[CC_DangKyKhungGioLamViec]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_DangKyKhungGioLamViec];
GO
IF OBJECT_ID(N'[dbo].[CC_HinhThucKhac]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_HinhThucKhac];
GO
IF OBJECT_ID(N'[dbo].[CC_HinhThucNghi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_HinhThucNghi];
GO
IF OBJECT_ID(N'[dbo].[CC_HinhThucViPham]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_HinhThucViPham];
GO
IF OBJECT_ID(N'[dbo].[CC_KhaiBaoCongTac]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_KhaiBaoCongTac];
GO
IF OBJECT_ID(N'[dbo].[CC_KyChamCong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_KyChamCong];
GO
IF OBJECT_ID(N'[dbo].[CC_KyDangKyKhungGio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_KyDangKyKhungGio];
GO
IF OBJECT_ID(N'[dbo].[CC_MailManager]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_MailManager];
GO
IF OBJECT_ID(N'[dbo].[CC_NgayNghiTrongNam]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_NgayNghiTrongNam];
GO
IF OBJECT_ID(N'[dbo].[CC_QuanLyChamCong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_QuanLyChamCong];
GO
IF OBJECT_ID(N'[dbo].[CC_QuanLyCongNgoaiGio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_QuanLyCongNgoaiGio];
GO
IF OBJECT_ID(N'[dbo].[CC_QuanLyNghiPhep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_QuanLyNghiPhep];
GO
IF OBJECT_ID(N'[dbo].[CC_QuanLyPhepHe]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_QuanLyPhepHe];
GO
IF OBJECT_ID(N'[dbo].[CC_QuanLyViPham]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_QuanLyViPham];
GO
IF OBJECT_ID(N'[dbo].[CC_ThoiGianDangKyKhungGioLamViec]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ThoiGianDangKyKhungGioLamViec];
GO
IF OBJECT_ID(N'[dbo].[ChucDanh]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChucDanh];
GO
IF OBJECT_ID(N'[dbo].[ChucVu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChucVu];
GO
IF OBJECT_ID(N'[dbo].[ChucVuKiemNhiem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChucVuKiemNhiem];
GO
IF OBJECT_ID(N'[dbo].[CongTy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CongTy];
GO
IF OBJECT_ID(N'[dbo].[GiayToHoSo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GiayToHoSo];
GO
IF OBJECT_ID(N'[dbo].[HoSo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HoSo];
GO
IF OBJECT_ID(N'[dbo].[KyTinhLuong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KyTinhLuong];
GO
IF OBJECT_ID(N'[dbo].[LoaiGiayTo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LoaiGiayTo];
GO
IF OBJECT_ID(N'[dbo].[LoaiNgayNgoaiGio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LoaiNgayNgoaiGio];
GO
IF OBJECT_ID(N'[dbo].[LoaiNhanSu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LoaiNhanSu];
GO
IF OBJECT_ID(N'[dbo].[NamHoc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NamHoc];
GO
IF OBJECT_ID(N'[dbo].[NgachLuong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NgachLuong];
GO
IF OBJECT_ID(N'[dbo].[NhanVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NhanVien];
GO
IF OBJECT_ID(N'[dbo].[NhanVienThongTinLuong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NhanVienThongTinLuong];
GO
IF OBJECT_ID(N'[dbo].[NhanVienTrinhDo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NhanVienTrinhDo];
GO
IF OBJECT_ID(N'[dbo].[NienDoTaiChinh]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NienDoTaiChinh];
GO
IF OBJECT_ID(N'[dbo].[ThongTinNhanVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[TinhTrang]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TinhTrang];
GO
IF OBJECT_ID(N'[dbo].[WebGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WebGroup];
GO
IF OBJECT_ID(N'[dbo].[WebMenu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WebMenu];
GO
IF OBJECT_ID(N'[dbo].[WebMenu_Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WebMenu_Role];
GO
IF OBJECT_ID(N'[dbo].[WebUser_BoPhan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WebUser_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[WebUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WebUsers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CC_Attachments'
CREATE TABLE [dbo].[CC_Attachments] (
    [Oid] uniqueidentifier  NOT NULL,
    [KhaiBaoCongTac] uniqueidentifier  NULL,
    [FileName] nvarchar(500)  NULL,
    [Data] varbinary(max)  NULL,
    [Date] datetime  NULL
);
GO

-- Creating table 'CC_AttLog'
CREATE TABLE [dbo].[CC_AttLog] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [EnrollNumber] bigint  NULL,
    [VerifyMode] int  NULL,
    [InOutMode] int  NULL,
    [LogDateTime] datetime  NULL,
    [WorkCode] int  NULL,
    [TimeAttendanceDeviceId] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_ChamCongTheoNgay'
CREATE TABLE [dbo].[CC_ChamCongTheoNgay] (
    [Oid] uniqueidentifier  NOT NULL,
    [IDNhanSu_ChamCong] int  NULL,
    [IDNhanVien] uniqueidentifier  NOT NULL,
    [Ngay] datetime  NOT NULL,
    [IDHinhThucNghi] uniqueidentifier  NULL,
    [IDBoPhan] uniqueidentifier  NOT NULL,
    [IDWebUsers] uniqueidentifier  NOT NULL,
    [GioVaoSang] datetime  NULL,
    [GioVaoChieu] datetime  NULL,
    [DaChamCong] bit  NOT NULL,
    [DiHoc] bit  NULL,
    [GioRaSang] datetime  NULL,
    [GioRaChieu] datetime  NULL,
    [CC_CaChamCong] uniqueidentifier  NULL,
    [GhiChu] nvarchar(max)  NULL,
    [NguoiDungChinhSua] bit  NOT NULL,
    [NgayDoiCa] datetime  NULL,
    [DaDieuChuyen] bit  NULL,
    [CongTy] uniqueidentifier  NULL,
    [IDHinhThucKhac] uniqueidentifier  NULL,
    [LoaiChamCong] tinyint  NULL
);
GO

-- Creating table 'CC_DangKyKhungGioLamViec'
CREATE TABLE [dbo].[CC_DangKyKhungGioLamViec] (
    [Oid] uniqueidentifier  NOT NULL,
    [ThongTinNhanVien] uniqueidentifier  NULL,
    [CaChamCong] uniqueidentifier  NULL,
    [KyDangKy] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_HinhThucViPham'
CREATE TABLE [dbo].[CC_HinhThucViPham] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenHinhThucViPham] nvarchar(100)  NULL
);
GO

-- Creating table 'CC_KyChamCong'
CREATE TABLE [dbo].[CC_KyChamCong] (
    [Oid] uniqueidentifier  NOT NULL,
    [Thang] int  NULL,
    [Nam] int  NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL,
    [SoNgay] decimal(19,4)  NULL,
    [KhoaSo] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [CongTy] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_KyDangKyKhungGio'
CREATE TABLE [dbo].[CC_KyDangKyKhungGio] (
    [Oid] uniqueidentifier  NOT NULL,
    [TenKy] nvarchar(max)  NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL
);
GO

-- Creating table 'CC_MailManager'
CREATE TABLE [dbo].[CC_MailManager] (
    [Oid] uniqueidentifier  NOT NULL,
    [Title] nvarchar(500)  NULL,
    [Contents] nvarchar(max)  NULL,
    [ReceiverEmail] nvarchar(200)  NULL,
    [SendDate] datetime  NULL,
    [FileName] nvarchar(2000)  NULL,
    [SendEmail] nvarchar(200)  NULL,
    [SendPass] nvarchar(200)  NULL,
    [IDWebUser] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_NgayNghiTrongNam'
CREATE TABLE [dbo].[CC_NgayNghiTrongNam] (
    [Oid] uniqueidentifier  NOT NULL,
    [LoaiNgayNghi] tinyint  NULL,
    [TenNgayNghi] nvarchar(100)  NULL,
    [NgayNghi] datetime  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [CongTy] uniqueidentifier  NULL,
    [NguoiTao] uniqueidentifier  NULL,
    [NgayTao] datetime  NULL
);
GO

-- Creating table 'CC_QuanLyChamCong'
CREATE TABLE [dbo].[CC_QuanLyChamCong] (
    [Oid] uniqueidentifier  NOT NULL,
    [CongTy] uniqueidentifier  NULL,
    [KyChamCong] uniqueidentifier  NULL,
    [NgayLap] datetime  NULL,
    [KhoaChamCong] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'CC_QuanLyViPham'
CREATE TABLE [dbo].[CC_QuanLyViPham] (
    [Oid] uniqueidentifier  NOT NULL,
    [cc_HinhThucViPham] uniqueidentifier  NULL,
    [ThoiGianTre] int  NULL,
    [ThoiGianSom] int  NULL,
    [CC_ChamCongTheoNgay] uniqueidentifier  NULL
);
GO

-- Creating table 'GiayToHoSoes'
CREATE TABLE [dbo].[GiayToHoSoes] (
    [Oid] uniqueidentifier  NOT NULL,
    [HoSo] uniqueidentifier  NULL,
    [STT] decimal(19,4)  NULL,
    [TenGiayTo] nvarchar(100)  NULL,
    [NgayLap] datetime  NULL,
    [LoaiGiayTo] uniqueidentifier  NULL,
    [GhiChu] nvarchar(100)  NULL,
    [DuongDanFile] nvarchar(100)  NULL,
    [DuongDanFileWeb] nvarchar(100)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'TinhTrangs'
CREATE TABLE [dbo].[TinhTrangs] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenTinhTrang] nvarchar(100)  NULL,
    [DaNghiViec] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'WebMenus'
CREATE TABLE [dbo].[WebMenus] (
    [Oid] uniqueidentifier  NOT NULL,
    [Name] nvarchar(200)  NULL,
    [Url] nvarchar(max)  NULL,
    [ParentId] uniqueidentifier  NULL,
    [Global_idx] int  NULL,
    [Local_idx] int  NULL,
    [Active] bit  NULL
);
GO

-- Creating table 'WebMenu_Role'
CREATE TABLE [dbo].[WebMenu_Role] (
    [WebMenuID] uniqueidentifier  NOT NULL,
    [WebGroupID] uniqueidentifier  NOT NULL,
    [Description] nvarchar(500)  NULL
);
GO

-- Creating table 'CC_ThoiGianDangKyKhungGioLamViec'
CREATE TABLE [dbo].[CC_ThoiGianDangKyKhungGioLamViec] (
    [Oid] uniqueidentifier  NOT NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL
);
GO

-- Creating table 'LoaiGiayToes'
CREATE TABLE [dbo].[LoaiGiayToes] (
    [Oid] uniqueidentifier  NOT NULL,
    [STT] int  NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenLoaiGiayTo] nvarchar(100)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'CongTies'
CREATE TABLE [dbo].[CongTies] (
    [Oid] uniqueidentifier  NOT NULL,
    [TenVietTat] nvarchar(100)  NULL,
    [DonViChuQuan] nvarchar(100)  NULL,
    [NamThanhLap] int  NULL,
    [DiaChi] uniqueidentifier  NULL,
    [DienThoai] nvarchar(100)  NULL,
    [Email] nvarchar(100)  NULL,
    [Fax] nvarchar(100)  NULL,
    [WebSite] nvarchar(100)  NULL,
    [ThongTinChung] uniqueidentifier  NULL,
    [MaSoThue] nvarchar(100)  NULL,
    [MocTinhThueTNCN] uniqueidentifier  NULL,
    [Logo] varbinary(max)  NULL,
    [HeDaoTao] uniqueidentifier  NULL,
    [DinhPhi] uniqueidentifier  NULL,
    [LoaiTruong] tinyint  NULL,
    [URLLogo] nvarchar(100)  NULL
);
GO

-- Creating table 'LoaiNhanSus'
CREATE TABLE [dbo].[LoaiNhanSus] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenLoaiNhanSu] nvarchar(100)  NULL,
    [CapDo] decimal(19,4)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'WebUsers'
CREATE TABLE [dbo].[WebUsers] (
    [Oid] uniqueidentifier  NOT NULL,
    [ThongTinNhanVien] uniqueidentifier  NULL,
    [UserName] nvarchar(200)  NULL,
    [Password] nvarchar(200)  NULL,
    [HoatDong] bit  NULL,
    [WebGroupID] uniqueidentifier  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [BoPhanId] uniqueidentifier  NULL,
    [EmailTP] nvarchar(200)  NULL,
    [EmailHT] nvarchar(200)  NULL,
    [EmailHDQT] nvarchar(200)  NULL,
    [CongTyId] uniqueidentifier  NULL,
    [LastPasswordChange] datetime  NULL,
    [UserChangePassword] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_CauHinhChamCong'
CREATE TABLE [dbo].[CC_CauHinhChamCong] (
    [Oid] uniqueidentifier  NOT NULL,
    [SoNgayNghiPhep] int  NOT NULL,
    [EmailSender] nvarchar(200)  NULL,
    [PassSender] nvarchar(200)  NULL,
    [SoNgayHieuTruongDuyet] int  NULL,
    [CongTy] uniqueidentifier  NULL,
    [SoNgayDangKyNgoaiGio] int  NULL,
    [NgayXoaPhepCu] datetime  NULL
);
GO

-- Creating table 'CC_ChiTietNghiPhep'
CREATE TABLE [dbo].[CC_ChiTietNghiPhep] (
    [Oid] uniqueidentifier  NOT NULL,
    [QuanLyNghiPhep] uniqueidentifier  NULL,
    [BoPhan] uniqueidentifier  NULL,
    [ThongTinNhanVien] uniqueidentifier  NULL,
    [TongSoNgayPhep] decimal(19,4)  NULL,
    [SoNgayPhepDaNghi] decimal(19,4)  NULL,
    [SoNgayPhepConLai] decimal(19,4)  NULL,
    [SoNgayPhepCongThem] decimal(19,4)  NULL,
    [SoNgayPhepNamHienTai] decimal(19,4)  NULL,
    [SoNgayPhepNamTruoc] decimal(19,4)  NULL,
    [SoNgayTamUngNamTruoc] decimal(19,4)  NULL,
    [SoNgayTamUngHienTai] decimal(19,4)  NULL,
    [SoNgayPhepDaNghi_QuiI] decimal(19,4)  NULL,
    [SoNgayPhepNamTruoc_BK] decimal(19,4)  NULL,
    [TongSoNgayBu] decimal(19,4)  NULL,
    [SoNgayBuDaNghi] decimal(19,4)  NULL,
    [SoNgayBuConLai] decimal(19,4)  NULL,
    [SoNgayBuNamHienTai] decimal(19,4)  NULL,
    [SoNgayBuNamTruoc] decimal(19,4)  NULL,
    [SoNgayBuNamTruoc_BK] decimal(19,4)  NULL
);
GO

-- Creating table 'CC_ChiTietChamCong'
CREATE TABLE [dbo].[CC_ChiTietChamCong] (
    [Oid] uniqueidentifier  NOT NULL,
    [QuanLyChamCong] uniqueidentifier  NULL,
    [BoPhan] uniqueidentifier  NULL,
    [ThongTinNhanVien] uniqueidentifier  NULL,
    [TongCong] decimal(19,4)  NULL,
    [NgayHuongLuong] decimal(19,4)  NULL,
    [NgayPhep] decimal(19,4)  NULL,
    [NgayKhongLuong] decimal(19,4)  NULL,
    [NgayHuongBHXH] decimal(19,4)  NULL,
    [NgayHe] decimal(19,4)  NULL,
    [DienGiai] nvarchar(2000)  NULL,
    [IsWeb] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [Khoa] bit  NULL,
    [CongChuanTheoLoaiGioLamViec] decimal(19,4)  NULL,
    [TongCongTruocDieuChinh] decimal(19,4)  NULL,
    [TongCongSauDieuChinh] decimal(19,4)  NULL,
    [NgayPhepBu] decimal(19,4)  NULL
);
GO

-- Creating table 'CC_QuanLyNghiPhep'
CREATE TABLE [dbo].[CC_QuanLyNghiPhep] (
    [Oid] uniqueidentifier  NOT NULL,
    [NamHoc] uniqueidentifier  NULL,
    [CongTy] uniqueidentifier  NULL,
    [NienDoTaiChinh] uniqueidentifier  NULL
);
GO

-- Creating table 'NamHocs'
CREATE TABLE [dbo].[NamHocs] (
    [Oid] uniqueidentifier  NOT NULL,
    [NgayBatDau] datetime  NULL,
    [NgayKetThuc] datetime  NULL,
    [TenNamHoc] nvarchar(100)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [NgayVaoHoc] datetime  NULL,
    [NgayKetThucHoc] datetime  NULL,
    [NgayVaoHe] datetime  NULL,
    [NgayKetThucHe] datetime  NULL,
    [SoThangHoc] int  NULL,
    [STT] int  NULL
);
GO

-- Creating table 'NhanVienTrinhDoes'
CREATE TABLE [dbo].[NhanVienTrinhDoes] (
    [Oid] uniqueidentifier  NOT NULL,
    [TrinhDoVanHoa] uniqueidentifier  NULL,
    [TrinhDoChuyenMon] uniqueidentifier  NULL,
    [ChuyenNganhDaoTao] uniqueidentifier  NULL,
    [TruongDaoTao] uniqueidentifier  NULL,
    [HinhThucDaoTao] uniqueidentifier  NULL,
    [NamTotNghiep] int  NULL,
    [NgayCapBang] datetime  NULL,
    [TrinhDoTinHoc] uniqueidentifier  NULL,
    [NgoaiNgu] uniqueidentifier  NULL,
    [TrinhDoNgoaiNgu] uniqueidentifier  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [HocHam] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_QuanLyCongNgoaiGio'
CREATE TABLE [dbo].[CC_QuanLyCongNgoaiGio] (
    [Oid] uniqueidentifier  NOT NULL,
    [CongTy] uniqueidentifier  NULL,
    [KyChamCong] uniqueidentifier  NULL,
    [NgayLap] datetime  NULL,
    [KhoaChamCong] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [JobUpdate] bit  NULL
);
GO

-- Creating table 'LoaiNgayNgoaiGios'
CREATE TABLE [dbo].[LoaiNgayNgoaiGios] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenNgayNgoaiGio] nvarchar(100)  NULL,
    [HeSo_CT] decimal(19,4)  NULL,
    [HeSo_KCT] decimal(19,4)  NULL,
    [TongHeSo] decimal(19,4)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [TuGio] int  NULL,
    [DenGio] int  NULL,
    [TuPhut] int  NULL,
    [DenPhut] int  NULL,
    [LoaiNgay] tinyint  NULL
);
GO

-- Creating table 'CC_ChiTietCongNgoaiGio'
CREATE TABLE [dbo].[CC_ChiTietCongNgoaiGio] (
    [Oid] uniqueidentifier  NOT NULL,
    [QuanLyCongNgoaiGio] uniqueidentifier  NULL,
    [BoPhan] uniqueidentifier  NULL,
    [ThongTinNhanVien] uniqueidentifier  NULL,
    [DienGiai] nvarchar(1000)  NULL,
    [GCRecord] decimal(19,4)  NULL,
    [OptimisticLockField] int  NULL,
    [TongSoGio_CT] decimal(19,4)  NULL,
    [TongSoGio_KCT] decimal(19,4)  NULL,
    [CongChuanTheoLoaiGioLamViec] decimal(19,4)  NULL,
    [NgayPhepBuTrongThang] decimal(19,4)  NULL,
    [TongNgayPhepBu] decimal(19,4)  NULL,
    [SoNgayBuConLai] decimal(19,4)  NULL
);
GO

-- Creating table 'BacLuongs'
CREATE TABLE [dbo].[BacLuongs] (
    [Oid] uniqueidentifier  NOT NULL,
    [NgachLuong] uniqueidentifier  NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenBacLuong] nvarchar(100)  NULL,
    [LuongCoBan] decimal(19,4)  NULL,
    [LuongKinhDoanh] decimal(19,4)  NULL,
    [BacLuongCu] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [CongTy] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_ChamCongNgoaiGio'
CREATE TABLE [dbo].[CC_ChamCongNgoaiGio] (
    [Oid] uniqueidentifier  NOT NULL,
    [IDNhanVien] uniqueidentifier  NULL,
    [IDBoPhan] uniqueidentifier  NULL,
    [Ngay] datetime  NULL,
    [SoPhutThucTe] decimal(18,2)  NULL,
    [SoPhutDangKy] decimal(18,2)  NULL,
    [SoGioThucTe] decimal(18,2)  NULL,
    [SoGioDangKy] decimal(18,2)  NULL,
    [TrangThai_TP] tinyint  NULL,
    [TrangThai_Admin] tinyint  NULL,
    [TrangThai_BGH] tinyint  NULL,
    [TuGio] nvarchar(50)  NULL,
    [DenGio] nvarchar(50)  NULL,
    [DaChamCong] bit  NULL,
    [LyDo] nvarchar(max)  NULL,
    [IDWebUsers] uniqueidentifier  NULL,
    [CongTy] uniqueidentifier  NULL,
    [LoaiNgayNgoaiGio] uniqueidentifier  NULL,
    [NgoaiKeHoach] bit  NULL,
    [TuGioThucTe] nvarchar(50)  NULL,
    [DenGioThucTe] nvarchar(50)  NULL,
    [SoNgayQuyDoi] decimal(18,1)  NULL,
    [NgayTao] datetime  NULL
);
GO

-- Creating table 'CC_HinhThucKhac'
CREATE TABLE [dbo].[CC_HinhThucKhac] (
    [Oid] uniqueidentifier  NOT NULL,
    [HinhThucSang] uniqueidentifier  NULL,
    [HinhThucChieu] uniqueidentifier  NULL,
    [MaQuanLy] varchar(100)  NULL,
    [SoLanCapNhat] int  NULL,
    [DoUuTien] decimal(19,4)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [TenHinhThucNghi] nvarchar(200)  NULL
);
GO

-- Creating table 'HoSoes'
CREATE TABLE [dbo].[HoSoes] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaTapDoan] nvarchar(100)  NULL,
    [MaNhanVien] nvarchar(100)  NULL,
    [Ho] nvarchar(100)  NULL,
    [Ten] nvarchar(100)  NULL,
    [HoTen] nvarchar(100)  NULL,
    [TenGoiKhac] nvarchar(100)  NULL,
    [NgaySinh] datetime  NULL,
    [NoiSinh] uniqueidentifier  NULL,
    [GioiTinh] tinyint  NULL,
    [CMND] nvarchar(100)  NULL,
    [NgayCap] datetime  NULL,
    [NoiCap] uniqueidentifier  NULL,
    [SoHoChieu] nvarchar(100)  NULL,
    [NgayCapHoChieu] datetime  NULL,
    [NoiCapHoChieu] nvarchar(100)  NULL,
    [NgayHetHan] datetime  NULL,
    [QueQuan] uniqueidentifier  NULL,
    [DiaChiThuongTru] uniqueidentifier  NULL,
    [NoiOHienNay] uniqueidentifier  NULL,
    [Email] nvarchar(100)  NULL,
    [DienThoaiDiDong] nvarchar(100)  NULL,
    [DienThoaiNhaRieng] nvarchar(100)  NULL,
    [TinhTrangHonNhan] uniqueidentifier  NULL,
    [DanToc] uniqueidentifier  NULL,
    [TonGiao] uniqueidentifier  NULL,
    [SucKhoe] uniqueidentifier  NULL,
    [ChieuCao] int  NULL,
    [CanNang] int  NULL,
    [NhomMau] uniqueidentifier  NULL,
    [QuocTich] uniqueidentifier  NULL,
    [GhiChu] nvarchar(4000)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [ObjectType] int  NULL,
    [HinhThucTuyenDung] tinyint  NULL,
    [MaHoSo] nvarchar(100)  NULL,
    [LoaiHoSo] tinyint  NULL,
    [OidHoSoCha] uniqueidentifier  NULL
);
GO

-- Creating table 'NhanViens'
CREATE TABLE [dbo].[NhanViens] (
    [Oid] uniqueidentifier  NOT NULL,
    [BoPhan] uniqueidentifier  NULL,
    [NgayTuyenDung] datetime  NULL,
    [NgayVaoCongTy] datetime  NULL,
    [ChucDanh] uniqueidentifier  NULL,
    [CongViecHienNay] uniqueidentifier  NULL,
    [LoaiHopDong] uniqueidentifier  NULL,
    [HopDongHienTai] uniqueidentifier  NULL,
    [DonViTuyenDung] nvarchar(100)  NULL,
    [CongViecTruocTuyenDung] nvarchar(100)  NULL,
    [NhanVienThongTinLuong] uniqueidentifier  NULL,
    [NhanVienTrinhDo] uniqueidentifier  NULL,
    [TinhTrang] uniqueidentifier  NULL,
    [CongTy] uniqueidentifier  NULL,
    [NgayNghiViec] datetime  NULL,
    [URLHinh] nvarchar(500)  NULL,
    [KhoiTapDoan] uniqueidentifier  NULL,
    [NgayVaoTapDoan] datetime  NULL,
    [LoaiGioLamViec] uniqueidentifier  NULL,
    [NgayNghiThaiSan] datetime  NULL,
    [NgayHetHanNghiThaiSan] datetime  NULL
);
GO

-- Creating table 'ThongTinNhanViens'
CREATE TABLE [dbo].[ThongTinNhanViens] (
    [Oid] uniqueidentifier  NOT NULL,
    [KhoaHoSo] bit  NULL,
    [LoaiNhanSu] uniqueidentifier  NULL,
    [ChucVu] uniqueidentifier  NULL,
    [NgayBoNhiemChucVu] datetime  NULL,
    [ChucVuDang] uniqueidentifier  NULL,
    [NgayVaoDang] datetime  NULL,
    [ChucVuDoan] uniqueidentifier  NULL,
    [NgayVaoDoan] datetime  NULL,
    [ChamCong] bit  NULL,
    [IDChamCong] nvarchar(100)  NULL,
    [GiangDay] bit  NULL,
    [TaiBoMon] uniqueidentifier  NULL,
    [LoaiChamCong] tinyint  NULL,
    [KhoiTapDoan] uniqueidentifier  NULL,
    [ChucDanhTapDoan] uniqueidentifier  NULL,
    [NgayVaoTapDoan] datetime  NULL,
    [LoaiGioLamViec] uniqueidentifier  NULL
);
GO

-- Creating table 'KyTinhLuongs'
CREATE TABLE [dbo].[KyTinhLuongs] (
    [Oid] uniqueidentifier  NOT NULL,
    [CongTy] uniqueidentifier  NULL,
    [Thang] int  NULL,
    [Nam] int  NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL,
    [SoNgay] decimal(19,4)  NULL,
    [KhoaSo] bit  NULL,
    [ThongTinChung] uniqueidentifier  NULL,
    [MocTinhThueTNCN] uniqueidentifier  NULL,
    [QuanLyChamCong] uniqueidentifier  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [MocDongBaoHiem] datetime  NULL,
    [QuanLyCongNgoaiGio] uniqueidentifier  NULL,
    [QuanLyCongKhac] uniqueidentifier  NULL
);
GO

-- Creating table 'NhanVienThongTinLuongs'
CREATE TABLE [dbo].[NhanVienThongTinLuongs] (
    [Oid] uniqueidentifier  NOT NULL,
    [TinhLuong] bit  NULL,
    [NgachLuong] uniqueidentifier  NULL,
    [BacLuong] uniqueidentifier  NULL,
    [NgayBoNhiemNgach] datetime  NULL,
    [NgayHuongLuong] datetime  NULL,
    [HeSoLuong] decimal(19,4)  NULL,
    [LuongCoBan] decimal(19,4)  NULL,
    [LuongKinhDoanh] decimal(19,4)  NULL,
    [MocNangLuongDieuChinh] datetime  NULL,
    [LyDoDieuChinh] nvarchar(2000)  NULL,
    [MocNangLuongLanSau] datetime  NULL,
    [HSPCChucVu] decimal(19,4)  NULL,
    [NgayHuongHSPCChucVu] datetime  NULL,
    [VuotKhung] int  NULL,
    [HSPCVuotKhung] decimal(19,4)  NULL,
    [NgayHuongVuotKhung] datetime  NULL,
    [ThamNien] decimal(19,4)  NULL,
    [HSPCThamNien] decimal(19,4)  NULL,
    [NgayHuongThamNien] datetime  NULL,
    [HSPCChucVuDang] decimal(19,4)  NULL,
    [HSPCChucVuDoan] decimal(19,4)  NULL,
    [HSPCKiemNhiem] decimal(19,4)  NULL,
    [HSPCTrachNhiem] decimal(19,4)  NULL,
    [HSPCKhac] decimal(19,4)  NULL,
    [KhongDongBHXH] bit  NULL,
    [KhongDongBHYT] bit  NULL,
    [KhongDongBHTN] bit  NULL,
    [KhongDongCongDoan] bit  NULL,
    [PhuCapDienThoai] decimal(19,4)  NULL,
    [PhuCapTienAn] decimal(19,4)  NULL,
    [PhuCapTienXang] decimal(19,4)  NULL,
    [SoNguoiPhuThuoc] int  NULL,
    [SoThangGiamTru] int  NULL,
    [MaSoThue] nvarchar(100)  NULL,
    [NgayCapMST] datetime  NULL,
    [PhanTramTinhLuong] decimal(19,4)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [PhanLoaiLuong] tinyint  NULL,
    [LuongKhoan] decimal(19,4)  NULL,
    [PhuCapKiemNhiem] decimal(19,4)  NULL,
    [PhuCapTrachNhiem] decimal(19,4)  NULL,
    [TinhThueMacDinh] bit  NULL,
    [PhanTramTinhThue] decimal(19,4)  NULL,
    [HieuQuaCongViec] decimal(19,4)  NULL,
    [TinhCongChuanMacDinh] bit  NULL,
    [PhuCapBanTru] decimal(19,4)  NULL,
    [PhuCapNhaO] decimal(19,4)  NULL
);
GO

-- Creating table 'ChucVus'
CREATE TABLE [dbo].[ChucVus] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenChucVu] nvarchar(100)  NULL,
    [HSPCChucVu] decimal(19,4)  NULL,
    [LaQuanLy] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [LaTruongDonVi] bit  NULL,
    [NhomChucVu] uniqueidentifier  NULL,
    [CapBac] decimal(19,4)  NULL,
    [Cap] tinyint  NULL,
    [WebGroup] uniqueidentifier  NULL
);
GO

-- Creating table 'NgachLuongs'
CREATE TABLE [dbo].[NgachLuongs] (
    [Oid] uniqueidentifier  NOT NULL,
    [NhomNgachLuong] uniqueidentifier  NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenNgachLuong] nvarchar(100)  NULL,
    [ThoiGianNangBac] int  NULL,
    [TotKhung] uniqueidentifier  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [CongTy] uniqueidentifier  NULL,
    [CapBac] decimal(19,4)  NULL
);
GO

-- Creating table 'BoPhans'
CREATE TABLE [dbo].[BoPhans] (
    [Oid] uniqueidentifier  NOT NULL,
    [STT] decimal(19,4)  NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenBoPhan] nvarchar(100)  NULL,
    [LoaiBoPhan] tinyint  NULL,
    [BoPhanCha] uniqueidentifier  NULL,
    [CongTy] uniqueidentifier  NULL,
    [NgungHoatDong] bit  NULL,
    [ID] uniqueidentifier  NULL,
    [ParentID] uniqueidentifier  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [ObjectType] int  NULL,
    [BoPhanOld] uniqueidentifier  NULL,
    [CapDonVi] tinyint  NULL,
    [KhongTinhLuong] bit  NULL,
    [Cap] tinyint  NULL
);
GO

-- Creating table 'ChucVuKiemNhiems'
CREATE TABLE [dbo].[ChucVuKiemNhiems] (
    [Oid] uniqueidentifier  NOT NULL,
    [QuyetDinh] uniqueidentifier  NULL,
    [NhanVien] uniqueidentifier  NULL,
    [BoPhan] uniqueidentifier  NULL,
    [ChucVu] uniqueidentifier  NULL,
    [NgayBoNhiem] datetime  NULL,
    [NgayHetNhiemKy] datetime  NULL,
    [PhuCapKiemNhiem] decimal(19,4)  NULL,
    [DaMienNhiem] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'WebGroups'
CREATE TABLE [dbo].[WebGroups] (
    [Oid] uniqueidentifier  NOT NULL,
    [Name] nvarchar(200)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'CC_ChamCongNgayNghi'
CREATE TABLE [dbo].[CC_ChamCongNgayNghi] (
    [Oid] uniqueidentifier  NOT NULL,
    [IDBoPhan] uniqueidentifier  NULL,
    [IDNhanVien] uniqueidentifier  NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL,
    [SoNgay] decimal(18,1)  NULL,
    [DienGiai] nvarchar(1000)  NULL,
    [NgayTao] datetime  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [IDWebUser] uniqueidentifier  NULL,
    [LoaiNghiPhep] int  NULL,
    [DiaChiLienHe] nvarchar(max)  NULL,
    [TenGiayXinPhep] nvarchar(max)  NULL,
    [TrangThai_TP] int  NULL,
    [TrangThai_Admin] int  NULL,
    [IDHinhThucNghi] uniqueidentifier  NULL,
    [NguoiBanGiao] nvarchar(200)  NULL,
    [TrangThai_HT] int  NULL,
    [TrangThai_HDQT] int  NULL,
    [IsBanGiamHieu] bit  NULL,
    [IsTruongPhong] bit  NULL,
    [Buoi] tinyint  NULL,
    [JobUpdated] tinyint  NULL,
    [WebUserDuyet_TP] uniqueidentifier  NULL,
    [WebUserDuyet_Admin] uniqueidentifier  NULL,
    [WebUserDuyet_HT] uniqueidentifier  NULL,
    [WebUserDuyet_HDQT] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_KhaiBaoCongTac'
CREATE TABLE [dbo].[CC_KhaiBaoCongTac] (
    [Oid] uniqueidentifier  NOT NULL,
    [IDNhanVien] uniqueidentifier  NOT NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL,
    [NoiDung] nvarchar(max)  NULL,
    [NgayTao] datetime  NULL,
    [IDWebUser] uniqueidentifier  NOT NULL,
    [TrangThai] int  NULL,
    [Buoi] tinyint  NULL,
    [DiaDiem] nvarchar(max)  NULL,
    [So] int  NULL,
    [NguoiKy] uniqueidentifier  NULL,
    [SoNgay] decimal(18,1)  NULL,
    [WebUserDuyet] uniqueidentifier  NULL
);
GO

-- Creating table 'WebUser_BoPhan'
CREATE TABLE [dbo].[WebUser_BoPhan] (
    [IDWebUser] uniqueidentifier  NULL,
    [BoPhanID] uniqueidentifier  NULL,
    [DienGiai] nvarchar(max)  NULL,
    [Oid] uniqueidentifier  NOT NULL,
    [ThongTinNhanVien] uniqueidentifier  NULL,
    [WebGroup] uniqueidentifier  NULL,
    [ChucVuChinh] bit  NULL,
    [QuyetDinh] uniqueidentifier  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'CC_CaChamCong'
CREATE TABLE [dbo].[CC_CaChamCong] (
    [Oid] uniqueidentifier  NOT NULL,
    [TenCa] nvarchar(max)  NULL,
    [LoaiKhung] tinyint  NULL,
    [HinhThucNghi] uniqueidentifier  NULL,
    [ThoiGianVaoSang] nvarchar(50)  NULL,
    [ThoiGianRaSang] nvarchar(50)  NULL,
    [TongSoGioLamSang] decimal(19,4)  NULL,
    [ThoiGianVaoChieu] nvarchar(50)  NULL,
    [ThoiGianRaChieu] nvarchar(50)  NULL,
    [TongSoGioLamChieu] decimal(19,4)  NULL,
    [ThoiGianBDNghiGiuaCa] nvarchar(50)  NULL,
    [ThoiGianKTNghiGiuaCa] nvarchar(50)  NULL,
    [TongSoGioNghiGiuaCa] decimal(18,1)  NULL,
    [ThoiGianBDQuetBuoiSang] nvarchar(max)  NULL,
    [ThoiGianKTQuetBuoiChieu] nvarchar(max)  NULL,
    [SoPhutCong] int  NULL,
    [SoPhutTru] int  NULL,
    [TongSoGioLamViecCaNgay] decimal(18,1)  NULL,
    [NgungSuDung] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [ThoiGianBatDauNghiGiuaCa] nvarchar(50)  NULL,
    [ThoiGianKetThucNghiGiuaCa] nvarchar(50)  NULL,
    [TongSoGioLamViec] int  NULL,
    [Active] bit  NULL,
    [LoaiCa] tinyint  NULL,
    [TongSoGioLamViecBuoiSang] decimal(18,1)  NULL,
    [TongSoGioLamViecBuoiChieu] decimal(18,1)  NULL
);
GO

-- Creating table 'CC_HinhThucNghi'
CREATE TABLE [dbo].[CC_HinhThucNghi] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenHinhThucNghi] nvarchar(100)  NULL,
    [KyHieu] nvarchar(100)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [PhanLoai] tinyint  NULL,
    [GiaTri] decimal(18,2)  NULL,
    [SoNgayToiDa] decimal(18,2)  NULL,
    [CongTy] uniqueidentifier  NULL,
    [DoUuTien] decimal(18,2)  NULL,
    [HinhThucNghi] bit  NULL
);
GO

-- Creating table 'CC_QuanLyPhepHe'
CREATE TABLE [dbo].[CC_QuanLyPhepHe] (
    [Oid] uniqueidentifier  NOT NULL,
    [NienDoTaiChinh] uniqueidentifier  NULL,
    [CongTy] uniqueidentifier  NULL
);
GO

-- Creating table 'NienDoTaiChinhs'
CREATE TABLE [dbo].[NienDoTaiChinhs] (
    [Oid] uniqueidentifier  NOT NULL,
    [CongTy] uniqueidentifier  NULL,
    [TenNienDo] nvarchar(200)  NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL,
    [SoThang] decimal(19,4)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [BatDauHe] datetime  NULL,
    [KetThucHe] datetime  NULL,
    [SoNgayNghiHe] decimal(19,4)  NULL
);
GO

-- Creating table 'CC_ChiTietPhepHe'
CREATE TABLE [dbo].[CC_ChiTietPhepHe] (
    [Oid] uniqueidentifier  NOT NULL,
    [QuanLyPhepHe] uniqueidentifier  NOT NULL,
    [ThongTinNhanVien] uniqueidentifier  NOT NULL,
    [BoPhan] uniqueidentifier  NOT NULL,
    [NgayVaoCoQuan] datetime  NULL,
    [SoPhepHe] decimal(18,0)  NULL,
    [ChucDanh] uniqueidentifier  NULL
);
GO

-- Creating table 'ChucDanhs'
CREATE TABLE [dbo].[ChucDanhs] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenChucDanh] nvarchar(100)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [ChucVu] uniqueidentifier  NULL,
    [CapBac] decimal(19,4)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Oid] in table 'CC_Attachments'
ALTER TABLE [dbo].[CC_Attachments]
ADD CONSTRAINT [PK_CC_Attachments]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Id] in table 'CC_AttLog'
ALTER TABLE [dbo].[CC_AttLog]
ADD CONSTRAINT [PK_CC_AttLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ChamCongTheoNgay'
ALTER TABLE [dbo].[CC_ChamCongTheoNgay]
ADD CONSTRAINT [PK_CC_ChamCongTheoNgay]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_DangKyKhungGioLamViec'
ALTER TABLE [dbo].[CC_DangKyKhungGioLamViec]
ADD CONSTRAINT [PK_CC_DangKyKhungGioLamViec]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_HinhThucViPham'
ALTER TABLE [dbo].[CC_HinhThucViPham]
ADD CONSTRAINT [PK_CC_HinhThucViPham]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_KyChamCong'
ALTER TABLE [dbo].[CC_KyChamCong]
ADD CONSTRAINT [PK_CC_KyChamCong]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_KyDangKyKhungGio'
ALTER TABLE [dbo].[CC_KyDangKyKhungGio]
ADD CONSTRAINT [PK_CC_KyDangKyKhungGio]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_MailManager'
ALTER TABLE [dbo].[CC_MailManager]
ADD CONSTRAINT [PK_CC_MailManager]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_NgayNghiTrongNam'
ALTER TABLE [dbo].[CC_NgayNghiTrongNam]
ADD CONSTRAINT [PK_CC_NgayNghiTrongNam]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_QuanLyChamCong'
ALTER TABLE [dbo].[CC_QuanLyChamCong]
ADD CONSTRAINT [PK_CC_QuanLyChamCong]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_QuanLyViPham'
ALTER TABLE [dbo].[CC_QuanLyViPham]
ADD CONSTRAINT [PK_CC_QuanLyViPham]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'GiayToHoSoes'
ALTER TABLE [dbo].[GiayToHoSoes]
ADD CONSTRAINT [PK_GiayToHoSoes]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'TinhTrangs'
ALTER TABLE [dbo].[TinhTrangs]
ADD CONSTRAINT [PK_TinhTrangs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'WebMenus'
ALTER TABLE [dbo].[WebMenus]
ADD CONSTRAINT [PK_WebMenus]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [WebMenuID], [WebGroupID] in table 'WebMenu_Role'
ALTER TABLE [dbo].[WebMenu_Role]
ADD CONSTRAINT [PK_WebMenu_Role]
    PRIMARY KEY CLUSTERED ([WebMenuID], [WebGroupID] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ThoiGianDangKyKhungGioLamViec'
ALTER TABLE [dbo].[CC_ThoiGianDangKyKhungGioLamViec]
ADD CONSTRAINT [PK_CC_ThoiGianDangKyKhungGioLamViec]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'LoaiGiayToes'
ALTER TABLE [dbo].[LoaiGiayToes]
ADD CONSTRAINT [PK_LoaiGiayToes]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CongTies'
ALTER TABLE [dbo].[CongTies]
ADD CONSTRAINT [PK_CongTies]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'LoaiNhanSus'
ALTER TABLE [dbo].[LoaiNhanSus]
ADD CONSTRAINT [PK_LoaiNhanSus]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'WebUsers'
ALTER TABLE [dbo].[WebUsers]
ADD CONSTRAINT [PK_WebUsers]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_CauHinhChamCong'
ALTER TABLE [dbo].[CC_CauHinhChamCong]
ADD CONSTRAINT [PK_CC_CauHinhChamCong]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ChiTietNghiPhep'
ALTER TABLE [dbo].[CC_ChiTietNghiPhep]
ADD CONSTRAINT [PK_CC_ChiTietNghiPhep]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ChiTietChamCong'
ALTER TABLE [dbo].[CC_ChiTietChamCong]
ADD CONSTRAINT [PK_CC_ChiTietChamCong]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_QuanLyNghiPhep'
ALTER TABLE [dbo].[CC_QuanLyNghiPhep]
ADD CONSTRAINT [PK_CC_QuanLyNghiPhep]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'NamHocs'
ALTER TABLE [dbo].[NamHocs]
ADD CONSTRAINT [PK_NamHocs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'NhanVienTrinhDoes'
ALTER TABLE [dbo].[NhanVienTrinhDoes]
ADD CONSTRAINT [PK_NhanVienTrinhDoes]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_QuanLyCongNgoaiGio'
ALTER TABLE [dbo].[CC_QuanLyCongNgoaiGio]
ADD CONSTRAINT [PK_CC_QuanLyCongNgoaiGio]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'LoaiNgayNgoaiGios'
ALTER TABLE [dbo].[LoaiNgayNgoaiGios]
ADD CONSTRAINT [PK_LoaiNgayNgoaiGios]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ChiTietCongNgoaiGio'
ALTER TABLE [dbo].[CC_ChiTietCongNgoaiGio]
ADD CONSTRAINT [PK_CC_ChiTietCongNgoaiGio]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'BacLuongs'
ALTER TABLE [dbo].[BacLuongs]
ADD CONSTRAINT [PK_BacLuongs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ChamCongNgoaiGio'
ALTER TABLE [dbo].[CC_ChamCongNgoaiGio]
ADD CONSTRAINT [PK_CC_ChamCongNgoaiGio]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_HinhThucKhac'
ALTER TABLE [dbo].[CC_HinhThucKhac]
ADD CONSTRAINT [PK_CC_HinhThucKhac]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'HoSoes'
ALTER TABLE [dbo].[HoSoes]
ADD CONSTRAINT [PK_HoSoes]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [PK_NhanViens]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'ThongTinNhanViens'
ALTER TABLE [dbo].[ThongTinNhanViens]
ADD CONSTRAINT [PK_ThongTinNhanViens]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'KyTinhLuongs'
ALTER TABLE [dbo].[KyTinhLuongs]
ADD CONSTRAINT [PK_KyTinhLuongs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'NhanVienThongTinLuongs'
ALTER TABLE [dbo].[NhanVienThongTinLuongs]
ADD CONSTRAINT [PK_NhanVienThongTinLuongs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'ChucVus'
ALTER TABLE [dbo].[ChucVus]
ADD CONSTRAINT [PK_ChucVus]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'NgachLuongs'
ALTER TABLE [dbo].[NgachLuongs]
ADD CONSTRAINT [PK_NgachLuongs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'BoPhans'
ALTER TABLE [dbo].[BoPhans]
ADD CONSTRAINT [PK_BoPhans]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'ChucVuKiemNhiems'
ALTER TABLE [dbo].[ChucVuKiemNhiems]
ADD CONSTRAINT [PK_ChucVuKiemNhiems]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'WebGroups'
ALTER TABLE [dbo].[WebGroups]
ADD CONSTRAINT [PK_WebGroups]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [PK_CC_ChamCongNgayNghi]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_KhaiBaoCongTac'
ALTER TABLE [dbo].[CC_KhaiBaoCongTac]
ADD CONSTRAINT [PK_CC_KhaiBaoCongTac]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'WebUser_BoPhan'
ALTER TABLE [dbo].[WebUser_BoPhan]
ADD CONSTRAINT [PK_WebUser_BoPhan]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_CaChamCong'
ALTER TABLE [dbo].[CC_CaChamCong]
ADD CONSTRAINT [PK_CC_CaChamCong]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_HinhThucNghi'
ALTER TABLE [dbo].[CC_HinhThucNghi]
ADD CONSTRAINT [PK_CC_HinhThucNghi]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_QuanLyPhepHe'
ALTER TABLE [dbo].[CC_QuanLyPhepHe]
ADD CONSTRAINT [PK_CC_QuanLyPhepHe]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'NienDoTaiChinhs'
ALTER TABLE [dbo].[NienDoTaiChinhs]
ADD CONSTRAINT [PK_NienDoTaiChinhs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ChiTietPhepHe'
ALTER TABLE [dbo].[CC_ChiTietPhepHe]
ADD CONSTRAINT [PK_CC_ChiTietPhepHe]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'ChucDanhs'
ALTER TABLE [dbo].[ChucDanhs]
ADD CONSTRAINT [PK_ChucDanhs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CC_ChamCongTheoNgay] in table 'CC_QuanLyViPham'
ALTER TABLE [dbo].[CC_QuanLyViPham]
ADD CONSTRAINT [FK_cc_QuanLyViPham_CC_ChamCongTheoNgay]
    FOREIGN KEY ([CC_ChamCongTheoNgay])
    REFERENCES [dbo].[CC_ChamCongTheoNgay]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_cc_QuanLyViPham_CC_ChamCongTheoNgay'
CREATE INDEX [IX_FK_cc_QuanLyViPham_CC_ChamCongTheoNgay]
ON [dbo].[CC_QuanLyViPham]
    ([CC_ChamCongTheoNgay]);
GO

-- Creating foreign key on [KyDangKy] in table 'CC_DangKyKhungGioLamViec'
ALTER TABLE [dbo].[CC_DangKyKhungGioLamViec]
ADD CONSTRAINT [FK_CC_DangKyKhungGioLamViec_CC_KyDangKyKhungGio]
    FOREIGN KEY ([KyDangKy])
    REFERENCES [dbo].[CC_KyDangKyKhungGio]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_DangKyKhungGioLamViec_CC_KyDangKyKhungGio'
CREATE INDEX [IX_FK_CC_DangKyKhungGioLamViec_CC_KyDangKyKhungGio]
ON [dbo].[CC_DangKyKhungGioLamViec]
    ([KyDangKy]);
GO

-- Creating foreign key on [cc_HinhThucViPham] in table 'CC_QuanLyViPham'
ALTER TABLE [dbo].[CC_QuanLyViPham]
ADD CONSTRAINT [FK_cc_QuanLyViPham_cc_HinhThucViPham]
    FOREIGN KEY ([cc_HinhThucViPham])
    REFERENCES [dbo].[CC_HinhThucViPham]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_cc_QuanLyViPham_cc_HinhThucViPham'
CREATE INDEX [IX_FK_cc_QuanLyViPham_cc_HinhThucViPham]
ON [dbo].[CC_QuanLyViPham]
    ([cc_HinhThucViPham]);
GO

-- Creating foreign key on [KyChamCong] in table 'CC_QuanLyChamCong'
ALTER TABLE [dbo].[CC_QuanLyChamCong]
ADD CONSTRAINT [FK_CC_QuanLyChamCong_KyChamCong]
    FOREIGN KEY ([KyChamCong])
    REFERENCES [dbo].[CC_KyChamCong]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_QuanLyChamCong_KyChamCong'
CREATE INDEX [IX_FK_CC_QuanLyChamCong_KyChamCong]
ON [dbo].[CC_QuanLyChamCong]
    ([KyChamCong]);
GO

-- Creating foreign key on [LoaiGiayTo] in table 'GiayToHoSoes'
ALTER TABLE [dbo].[GiayToHoSoes]
ADD CONSTRAINT [FK_GiayToHoSo_LoaiGiayTo]
    FOREIGN KEY ([LoaiGiayTo])
    REFERENCES [dbo].[GiayToHoSoes]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GiayToHoSo_LoaiGiayTo'
CREATE INDEX [IX_FK_GiayToHoSo_LoaiGiayTo]
ON [dbo].[GiayToHoSoes]
    ([LoaiGiayTo]);
GO

-- Creating foreign key on [WebMenuID] in table 'WebMenu_Role'
ALTER TABLE [dbo].[WebMenu_Role]
ADD CONSTRAINT [FK_WebMenu_Role_WebMenu1]
    FOREIGN KEY ([WebMenuID])
    REFERENCES [dbo].[WebMenus]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ParentId] in table 'WebMenus'
ALTER TABLE [dbo].[WebMenus]
ADD CONSTRAINT [FK_WebMenu_WebMenu]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[WebMenus]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebMenu_WebMenu'
CREATE INDEX [IX_FK_WebMenu_WebMenu]
ON [dbo].[WebMenus]
    ([ParentId]);
GO

-- Creating foreign key on [CongTy] in table 'CC_QuanLyChamCong'
ALTER TABLE [dbo].[CC_QuanLyChamCong]
ADD CONSTRAINT [FK_CC_QuanLyChamCong_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_QuanLyChamCong_CongTy'
CREATE INDEX [IX_FK_CC_QuanLyChamCong_CongTy]
ON [dbo].[CC_QuanLyChamCong]
    ([CongTy]);
GO

-- Creating foreign key on [IDWebUser] in table 'CC_MailManager'
ALTER TABLE [dbo].[CC_MailManager]
ADD CONSTRAINT [FK_CC_MailManager_WebUsers]
    FOREIGN KEY ([IDWebUser])
    REFERENCES [dbo].[WebUsers]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_MailManager_WebUsers'
CREATE INDEX [IX_FK_CC_MailManager_WebUsers]
ON [dbo].[CC_MailManager]
    ([IDWebUser]);
GO

-- Creating foreign key on [CongTyId] in table 'WebUsers'
ALTER TABLE [dbo].[WebUsers]
ADD CONSTRAINT [FK_WebUsers_CongTy]
    FOREIGN KEY ([CongTyId])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUsers_CongTy'
CREATE INDEX [IX_FK_WebUsers_CongTy]
ON [dbo].[WebUsers]
    ([CongTyId]);
GO

-- Creating foreign key on [CongTy] in table 'CC_ChamCongTheoNgay'
ALTER TABLE [dbo].[CC_ChamCongTheoNgay]
ADD CONSTRAINT [FK_CC_ChamCongTheoNgay_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongTheoNgay_CongTy'
CREATE INDEX [IX_FK_CC_ChamCongTheoNgay_CongTy]
ON [dbo].[CC_ChamCongTheoNgay]
    ([CongTy]);
GO

-- Creating foreign key on [CongTy] in table 'CC_KyChamCong'
ALTER TABLE [dbo].[CC_KyChamCong]
ADD CONSTRAINT [FK_CC_KyChamCong_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_KyChamCong_CongTy'
CREATE INDEX [IX_FK_CC_KyChamCong_CongTy]
ON [dbo].[CC_KyChamCong]
    ([CongTy]);
GO

-- Creating foreign key on [QuanLyChamCong] in table 'CC_ChiTietChamCong'
ALTER TABLE [dbo].[CC_ChiTietChamCong]
ADD CONSTRAINT [FK_CC_ChiTietChamCong_QuanLyChamCong]
    FOREIGN KEY ([QuanLyChamCong])
    REFERENCES [dbo].[CC_QuanLyChamCong]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietChamCong_QuanLyChamCong'
CREATE INDEX [IX_FK_CC_ChiTietChamCong_QuanLyChamCong]
ON [dbo].[CC_ChiTietChamCong]
    ([QuanLyChamCong]);
GO

-- Creating foreign key on [QuanLyNghiPhep] in table 'CC_ChiTietNghiPhep'
ALTER TABLE [dbo].[CC_ChiTietNghiPhep]
ADD CONSTRAINT [FK_CC_ChiTietNghiPhep_CC_QuanLyNghiPhep]
    FOREIGN KEY ([QuanLyNghiPhep])
    REFERENCES [dbo].[CC_QuanLyNghiPhep]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietNghiPhep_CC_QuanLyNghiPhep'
CREATE INDEX [IX_FK_CC_ChiTietNghiPhep_CC_QuanLyNghiPhep]
ON [dbo].[CC_ChiTietNghiPhep]
    ([QuanLyNghiPhep]);
GO

-- Creating foreign key on [CongTy] in table 'CC_QuanLyNghiPhep'
ALTER TABLE [dbo].[CC_QuanLyNghiPhep]
ADD CONSTRAINT [FK_CC_QuanLyNghiPhep_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_QuanLyNghiPhep_CongTy'
CREATE INDEX [IX_FK_CC_QuanLyNghiPhep_CongTy]
ON [dbo].[CC_QuanLyNghiPhep]
    ([CongTy]);
GO

-- Creating foreign key on [NamHoc] in table 'CC_QuanLyNghiPhep'
ALTER TABLE [dbo].[CC_QuanLyNghiPhep]
ADD CONSTRAINT [FK_CC_QuanLyNghiPhep_NamHoc]
    FOREIGN KEY ([NamHoc])
    REFERENCES [dbo].[NamHocs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_QuanLyNghiPhep_NamHoc'
CREATE INDEX [IX_FK_CC_QuanLyNghiPhep_NamHoc]
ON [dbo].[CC_QuanLyNghiPhep]
    ([NamHoc]);
GO

-- Creating foreign key on [KyChamCong] in table 'CC_QuanLyCongNgoaiGio'
ALTER TABLE [dbo].[CC_QuanLyCongNgoaiGio]
ADD CONSTRAINT [FK_CC_QuanLyCongNgoaiGio_CC_KyChamCong]
    FOREIGN KEY ([KyChamCong])
    REFERENCES [dbo].[CC_KyChamCong]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_QuanLyCongNgoaiGio_CC_KyChamCong'
CREATE INDEX [IX_FK_CC_QuanLyCongNgoaiGio_CC_KyChamCong]
ON [dbo].[CC_QuanLyCongNgoaiGio]
    ([KyChamCong]);
GO

-- Creating foreign key on [CongTy] in table 'CC_QuanLyCongNgoaiGio'
ALTER TABLE [dbo].[CC_QuanLyCongNgoaiGio]
ADD CONSTRAINT [FK_CC_QuanLyCongNgoaiGio_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_QuanLyCongNgoaiGio_CongTy'
CREATE INDEX [IX_FK_CC_QuanLyCongNgoaiGio_CongTy]
ON [dbo].[CC_QuanLyCongNgoaiGio]
    ([CongTy]);
GO

-- Creating foreign key on [QuanLyCongNgoaiGio] in table 'CC_ChiTietCongNgoaiGio'
ALTER TABLE [dbo].[CC_ChiTietCongNgoaiGio]
ADD CONSTRAINT [FK_CC_ChiTietCongNgoaiGio_QuanLyCongNgoaiGio]
    FOREIGN KEY ([QuanLyCongNgoaiGio])
    REFERENCES [dbo].[CC_QuanLyCongNgoaiGio]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietCongNgoaiGio_QuanLyCongNgoaiGio'
CREATE INDEX [IX_FK_CC_ChiTietCongNgoaiGio_QuanLyCongNgoaiGio]
ON [dbo].[CC_ChiTietCongNgoaiGio]
    ([QuanLyCongNgoaiGio]);
GO

-- Creating foreign key on [CongTy] in table 'BacLuongs'
ALTER TABLE [dbo].[BacLuongs]
ADD CONSTRAINT [FK_BacLuong_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BacLuong_CongTy'
CREATE INDEX [IX_FK_BacLuong_CongTy]
ON [dbo].[BacLuongs]
    ([CongTy]);
GO

-- Creating foreign key on [LoaiNgayNgoaiGio] in table 'CC_ChamCongNgoaiGio'
ALTER TABLE [dbo].[CC_ChamCongNgoaiGio]
ADD CONSTRAINT [FK_CC_ChamCongNgoaiGio_LoaiNgayNgoaiGio]
    FOREIGN KEY ([LoaiNgayNgoaiGio])
    REFERENCES [dbo].[LoaiNgayNgoaiGios]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgoaiGio_LoaiNgayNgoaiGio'
CREATE INDEX [IX_FK_CC_ChamCongNgoaiGio_LoaiNgayNgoaiGio]
ON [dbo].[CC_ChamCongNgoaiGio]
    ([LoaiNgayNgoaiGio]);
GO

-- Creating foreign key on [IDHinhThucKhac] in table 'CC_ChamCongTheoNgay'
ALTER TABLE [dbo].[CC_ChamCongTheoNgay]
ADD CONSTRAINT [FK_CC_ChamCongTheoNgay_CC_HinhThucKhac]
    FOREIGN KEY ([IDHinhThucKhac])
    REFERENCES [dbo].[CC_HinhThucKhac]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongTheoNgay_CC_HinhThucKhac'
CREATE INDEX [IX_FK_CC_ChamCongTheoNgay_CC_HinhThucKhac]
ON [dbo].[CC_ChamCongTheoNgay]
    ([IDHinhThucKhac]);
GO

-- Creating foreign key on [Oid] in table 'CC_HinhThucKhac'
ALTER TABLE [dbo].[CC_HinhThucKhac]
ADD CONSTRAINT [FK_CC_HinhThucKhac_CC_ChamCongTheoNgay]
    FOREIGN KEY ([Oid])
    REFERENCES [dbo].[CC_HinhThucKhac]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IDNhanVien] in table 'CC_ChamCongNgoaiGio'
ALTER TABLE [dbo].[CC_ChamCongNgoaiGio]
ADD CONSTRAINT [FK_CC_ChamCongNgoaiGio_ThongTinNhanVien]
    FOREIGN KEY ([IDNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgoaiGio_ThongTinNhanVien'
CREATE INDEX [IX_FK_CC_ChamCongNgoaiGio_ThongTinNhanVien]
ON [dbo].[CC_ChamCongNgoaiGio]
    ([IDNhanVien]);
GO

-- Creating foreign key on [IDNhanVien] in table 'CC_ChamCongTheoNgay'
ALTER TABLE [dbo].[CC_ChamCongTheoNgay]
ADD CONSTRAINT [FK_CC_ChamCongTheoNgay_HoSo]
    FOREIGN KEY ([IDNhanVien])
    REFERENCES [dbo].[HoSoes]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongTheoNgay_HoSo'
CREATE INDEX [IX_FK_CC_ChamCongTheoNgay_HoSo]
ON [dbo].[CC_ChamCongTheoNgay]
    ([IDNhanVien]);
GO

-- Creating foreign key on [ThongTinNhanVien] in table 'CC_ChiTietChamCong'
ALTER TABLE [dbo].[CC_ChiTietChamCong]
ADD CONSTRAINT [FK_CC_ChiTietChamCong_ThongTinNhanVien]
    FOREIGN KEY ([ThongTinNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietChamCong_ThongTinNhanVien'
CREATE INDEX [IX_FK_CC_ChiTietChamCong_ThongTinNhanVien]
ON [dbo].[CC_ChiTietChamCong]
    ([ThongTinNhanVien]);
GO

-- Creating foreign key on [ThongTinNhanVien] in table 'CC_ChiTietCongNgoaiGio'
ALTER TABLE [dbo].[CC_ChiTietCongNgoaiGio]
ADD CONSTRAINT [FK_CC_ChiTietCongNgoaiGio_ThongTinNhanVien]
    FOREIGN KEY ([ThongTinNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietCongNgoaiGio_ThongTinNhanVien'
CREATE INDEX [IX_FK_CC_ChiTietCongNgoaiGio_ThongTinNhanVien]
ON [dbo].[CC_ChiTietCongNgoaiGio]
    ([ThongTinNhanVien]);
GO

-- Creating foreign key on [ThongTinNhanVien] in table 'CC_ChiTietNghiPhep'
ALTER TABLE [dbo].[CC_ChiTietNghiPhep]
ADD CONSTRAINT [FK_CC_ChiTietNghiPhep_ThongTinNhanVien]
    FOREIGN KEY ([ThongTinNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietNghiPhep_ThongTinNhanVien'
CREATE INDEX [IX_FK_CC_ChiTietNghiPhep_ThongTinNhanVien]
ON [dbo].[CC_ChiTietNghiPhep]
    ([ThongTinNhanVien]);
GO

-- Creating foreign key on [ThongTinNhanVien] in table 'CC_DangKyKhungGioLamViec'
ALTER TABLE [dbo].[CC_DangKyKhungGioLamViec]
ADD CONSTRAINT [FK_CC_DangKyKhungGioLamViec_ThongTinNhanVien]
    FOREIGN KEY ([ThongTinNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_DangKyKhungGioLamViec_ThongTinNhanVien'
CREATE INDEX [IX_FK_CC_DangKyKhungGioLamViec_ThongTinNhanVien]
ON [dbo].[CC_DangKyKhungGioLamViec]
    ([ThongTinNhanVien]);
GO

-- Creating foreign key on [CongTy] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NhanVien_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhanVien_CongTy'
CREATE INDEX [IX_FK_NhanVien_CongTy]
ON [dbo].[NhanViens]
    ([CongTy]);
GO

-- Creating foreign key on [HoSo] in table 'GiayToHoSoes'
ALTER TABLE [dbo].[GiayToHoSoes]
ADD CONSTRAINT [FK_GiayToHoSo_HoSo]
    FOREIGN KEY ([HoSo])
    REFERENCES [dbo].[HoSoes]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GiayToHoSo_HoSo'
CREATE INDEX [IX_FK_GiayToHoSo_HoSo]
ON [dbo].[GiayToHoSoes]
    ([HoSo]);
GO

-- Creating foreign key on [Oid] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NhanVien_Oid]
    FOREIGN KEY ([Oid])
    REFERENCES [dbo].[HoSoes]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ThongTinNhanVien] in table 'WebUsers'
ALTER TABLE [dbo].[WebUsers]
ADD CONSTRAINT [FK_WebUsers_HoSo]
    FOREIGN KEY ([ThongTinNhanVien])
    REFERENCES [dbo].[HoSoes]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUsers_HoSo'
CREATE INDEX [IX_FK_WebUsers_HoSo]
ON [dbo].[WebUsers]
    ([ThongTinNhanVien]);
GO

-- Creating foreign key on [LoaiNhanSu] in table 'ThongTinNhanViens'
ALTER TABLE [dbo].[ThongTinNhanViens]
ADD CONSTRAINT [FK_ThongTinNhanVien_LoaiNhanSu]
    FOREIGN KEY ([LoaiNhanSu])
    REFERENCES [dbo].[LoaiNhanSus]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThongTinNhanVien_LoaiNhanSu'
CREATE INDEX [IX_FK_ThongTinNhanVien_LoaiNhanSu]
ON [dbo].[ThongTinNhanViens]
    ([LoaiNhanSu]);
GO

-- Creating foreign key on [NhanVienTrinhDo] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NhanVien_NhanVienTrinhDo]
    FOREIGN KEY ([NhanVienTrinhDo])
    REFERENCES [dbo].[NhanVienTrinhDoes]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhanVien_NhanVienTrinhDo'
CREATE INDEX [IX_FK_NhanVien_NhanVienTrinhDo]
ON [dbo].[NhanViens]
    ([NhanVienTrinhDo]);
GO

-- Creating foreign key on [TinhTrang] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NhanVien_TinhTrang]
    FOREIGN KEY ([TinhTrang])
    REFERENCES [dbo].[TinhTrangs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhanVien_TinhTrang'
CREATE INDEX [IX_FK_NhanVien_TinhTrang]
ON [dbo].[NhanViens]
    ([TinhTrang]);
GO

-- Creating foreign key on [Oid] in table 'ThongTinNhanViens'
ALTER TABLE [dbo].[ThongTinNhanViens]
ADD CONSTRAINT [FK_ThongTinNhanVien_Oid]
    FOREIGN KEY ([Oid])
    REFERENCES [dbo].[NhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ThongTinNhanVien] in table 'WebUsers'
ALTER TABLE [dbo].[WebUsers]
ADD CONSTRAINT [FK_WebUsers_ThongTinNhanVien]
    FOREIGN KEY ([ThongTinNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUsers_ThongTinNhanVien'
CREATE INDEX [IX_FK_WebUsers_ThongTinNhanVien]
ON [dbo].[WebUsers]
    ([ThongTinNhanVien]);
GO

-- Creating foreign key on [BacLuong] in table 'NhanVienThongTinLuongs'
ALTER TABLE [dbo].[NhanVienThongTinLuongs]
ADD CONSTRAINT [FK_NhanVienThongTinLuong_BacLuong]
    FOREIGN KEY ([BacLuong])
    REFERENCES [dbo].[BacLuongs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhanVienThongTinLuong_BacLuong'
CREATE INDEX [IX_FK_NhanVienThongTinLuong_BacLuong]
ON [dbo].[NhanVienThongTinLuongs]
    ([BacLuong]);
GO

-- Creating foreign key on [QuanLyChamCong] in table 'KyTinhLuongs'
ALTER TABLE [dbo].[KyTinhLuongs]
ADD CONSTRAINT [FK_KyTinhLuong_QuanLyChamCong]
    FOREIGN KEY ([QuanLyChamCong])
    REFERENCES [dbo].[CC_QuanLyChamCong]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KyTinhLuong_QuanLyChamCong'
CREATE INDEX [IX_FK_KyTinhLuong_QuanLyChamCong]
ON [dbo].[KyTinhLuongs]
    ([QuanLyChamCong]);
GO

-- Creating foreign key on [NhanVienThongTinLuong] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NhanVien_NhanVienThongTinLuong]
    FOREIGN KEY ([NhanVienThongTinLuong])
    REFERENCES [dbo].[NhanVienThongTinLuongs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhanVien_NhanVienThongTinLuong'
CREATE INDEX [IX_FK_NhanVien_NhanVienThongTinLuong]
ON [dbo].[NhanViens]
    ([NhanVienThongTinLuong]);
GO

-- Creating foreign key on [ChucVu] in table 'ThongTinNhanViens'
ALTER TABLE [dbo].[ThongTinNhanViens]
ADD CONSTRAINT [FK_ThongTinNhanVien_ChucVu]
    FOREIGN KEY ([ChucVu])
    REFERENCES [dbo].[ChucVus]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThongTinNhanVien_ChucVu'
CREATE INDEX [IX_FK_ThongTinNhanVien_ChucVu]
ON [dbo].[ThongTinNhanViens]
    ([ChucVu]);
GO

-- Creating foreign key on [NgachLuong] in table 'BacLuongs'
ALTER TABLE [dbo].[BacLuongs]
ADD CONSTRAINT [FK_BacLuong_NgachLuong]
    FOREIGN KEY ([NgachLuong])
    REFERENCES [dbo].[NgachLuongs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BacLuong_NgachLuong'
CREATE INDEX [IX_FK_BacLuong_NgachLuong]
ON [dbo].[BacLuongs]
    ([NgachLuong]);
GO

-- Creating foreign key on [TotKhung] in table 'NgachLuongs'
ALTER TABLE [dbo].[NgachLuongs]
ADD CONSTRAINT [FK_NgachLuong_TotKhung]
    FOREIGN KEY ([TotKhung])
    REFERENCES [dbo].[BacLuongs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NgachLuong_TotKhung'
CREATE INDEX [IX_FK_NgachLuong_TotKhung]
ON [dbo].[NgachLuongs]
    ([TotKhung]);
GO

-- Creating foreign key on [CongTy] in table 'NgachLuongs'
ALTER TABLE [dbo].[NgachLuongs]
ADD CONSTRAINT [FK_NgachLuong_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NgachLuong_CongTy'
CREATE INDEX [IX_FK_NgachLuong_CongTy]
ON [dbo].[NgachLuongs]
    ([CongTy]);
GO

-- Creating foreign key on [NgachLuong] in table 'NhanVienThongTinLuongs'
ALTER TABLE [dbo].[NhanVienThongTinLuongs]
ADD CONSTRAINT [FK_NhanVienThongTinLuong_NgachLuong]
    FOREIGN KEY ([NgachLuong])
    REFERENCES [dbo].[NgachLuongs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhanVienThongTinLuong_NgachLuong'
CREATE INDEX [IX_FK_NhanVienThongTinLuong_NgachLuong]
ON [dbo].[NhanVienThongTinLuongs]
    ([NgachLuong]);
GO

-- Creating foreign key on [BoPhanCha] in table 'BoPhans'
ALTER TABLE [dbo].[BoPhans]
ADD CONSTRAINT [FK_BoPhan_BoPhanCha]
    FOREIGN KEY ([BoPhanCha])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BoPhan_BoPhanCha'
CREATE INDEX [IX_FK_BoPhan_BoPhanCha]
ON [dbo].[BoPhans]
    ([BoPhanCha]);
GO

-- Creating foreign key on [CongTy] in table 'BoPhans'
ALTER TABLE [dbo].[BoPhans]
ADD CONSTRAINT [FK_BoPhan_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BoPhan_CongTy'
CREATE INDEX [IX_FK_BoPhan_CongTy]
ON [dbo].[BoPhans]
    ([CongTy]);
GO

-- Creating foreign key on [IDBoPhan] in table 'CC_ChamCongTheoNgay'
ALTER TABLE [dbo].[CC_ChamCongTheoNgay]
ADD CONSTRAINT [FK_CC_ChamCongTheoNgay_BoPhan]
    FOREIGN KEY ([IDBoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongTheoNgay_BoPhan'
CREATE INDEX [IX_FK_CC_ChamCongTheoNgay_BoPhan]
ON [dbo].[CC_ChamCongTheoNgay]
    ([IDBoPhan]);
GO

-- Creating foreign key on [BoPhan] in table 'CC_ChiTietChamCong'
ALTER TABLE [dbo].[CC_ChiTietChamCong]
ADD CONSTRAINT [FK_CC_ChiTietChamCong_BoPhan]
    FOREIGN KEY ([BoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietChamCong_BoPhan'
CREATE INDEX [IX_FK_CC_ChiTietChamCong_BoPhan]
ON [dbo].[CC_ChiTietChamCong]
    ([BoPhan]);
GO

-- Creating foreign key on [BoPhan] in table 'CC_ChiTietCongNgoaiGio'
ALTER TABLE [dbo].[CC_ChiTietCongNgoaiGio]
ADD CONSTRAINT [FK_CC_ChiTietCongNgoaiGio_BoPhan]
    FOREIGN KEY ([BoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietCongNgoaiGio_BoPhan'
CREATE INDEX [IX_FK_CC_ChiTietCongNgoaiGio_BoPhan]
ON [dbo].[CC_ChiTietCongNgoaiGio]
    ([BoPhan]);
GO

-- Creating foreign key on [BoPhan] in table 'CC_ChiTietNghiPhep'
ALTER TABLE [dbo].[CC_ChiTietNghiPhep]
ADD CONSTRAINT [FK_CC_ChiTietNghiPhep_BoPhan]
    FOREIGN KEY ([BoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietNghiPhep_BoPhan'
CREATE INDEX [IX_FK_CC_ChiTietNghiPhep_BoPhan]
ON [dbo].[CC_ChiTietNghiPhep]
    ([BoPhan]);
GO

-- Creating foreign key on [Oid] in table 'CongTies'
ALTER TABLE [dbo].[CongTies]
ADD CONSTRAINT [FK_CongTy_Oid]
    FOREIGN KEY ([Oid])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [BoPhan] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NhanVien_BoPhan]
    FOREIGN KEY ([BoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhanVien_BoPhan'
CREATE INDEX [IX_FK_NhanVien_BoPhan]
ON [dbo].[NhanViens]
    ([BoPhan]);
GO

-- Creating foreign key on [BoPhanId] in table 'WebUsers'
ALTER TABLE [dbo].[WebUsers]
ADD CONSTRAINT [FK_WebUsers_BoPhan]
    FOREIGN KEY ([BoPhanId])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUsers_BoPhan'
CREATE INDEX [IX_FK_WebUsers_BoPhan]
ON [dbo].[WebUsers]
    ([BoPhanId]);
GO

-- Creating foreign key on [QuanLyCongKhac] in table 'KyTinhLuongs'
ALTER TABLE [dbo].[KyTinhLuongs]
ADD CONSTRAINT [FK_KyTinhLuong_QuanLyCongKhac]
    FOREIGN KEY ([QuanLyCongKhac])
    REFERENCES [dbo].[CC_KyChamCong]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KyTinhLuong_QuanLyCongKhac'
CREATE INDEX [IX_FK_KyTinhLuong_QuanLyCongKhac]
ON [dbo].[KyTinhLuongs]
    ([QuanLyCongKhac]);
GO

-- Creating foreign key on [QuanLyCongNgoaiGio] in table 'KyTinhLuongs'
ALTER TABLE [dbo].[KyTinhLuongs]
ADD CONSTRAINT [FK_KyTinhLuong_QuanLyCongNgoaiGio]
    FOREIGN KEY ([QuanLyCongNgoaiGio])
    REFERENCES [dbo].[CC_QuanLyCongNgoaiGio]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KyTinhLuong_QuanLyCongNgoaiGio'
CREATE INDEX [IX_FK_KyTinhLuong_QuanLyCongNgoaiGio]
ON [dbo].[KyTinhLuongs]
    ([QuanLyCongNgoaiGio]);
GO

-- Creating foreign key on [CongTy] in table 'KyTinhLuongs'
ALTER TABLE [dbo].[KyTinhLuongs]
ADD CONSTRAINT [FK_KyTinhLuong_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KyTinhLuong_CongTy'
CREATE INDEX [IX_FK_KyTinhLuong_CongTy]
ON [dbo].[KyTinhLuongs]
    ([CongTy]);
GO

-- Creating foreign key on [IDBoPhan] in table 'CC_ChamCongNgoaiGio'
ALTER TABLE [dbo].[CC_ChamCongNgoaiGio]
ADD CONSTRAINT [FK_CC_ChamCongNgoaiGio_BoPhan]
    FOREIGN KEY ([IDBoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgoaiGio_BoPhan'
CREATE INDEX [IX_FK_CC_ChamCongNgoaiGio_BoPhan]
ON [dbo].[CC_ChamCongNgoaiGio]
    ([IDBoPhan]);
GO

-- Creating foreign key on [TaiBoMon] in table 'ThongTinNhanViens'
ALTER TABLE [dbo].[ThongTinNhanViens]
ADD CONSTRAINT [FK_ThongTinNhanVien_TaiBoMon]
    FOREIGN KEY ([TaiBoMon])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThongTinNhanVien_TaiBoMon'
CREATE INDEX [IX_FK_ThongTinNhanVien_TaiBoMon]
ON [dbo].[ThongTinNhanViens]
    ([TaiBoMon]);
GO

-- Creating foreign key on [BoPhan] in table 'ChucVuKiemNhiems'
ALTER TABLE [dbo].[ChucVuKiemNhiems]
ADD CONSTRAINT [FK_ChucVuKiemNhiem_BoPhan]
    FOREIGN KEY ([BoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChucVuKiemNhiem_BoPhan'
CREATE INDEX [IX_FK_ChucVuKiemNhiem_BoPhan]
ON [dbo].[ChucVuKiemNhiems]
    ([BoPhan]);
GO

-- Creating foreign key on [ChucVu] in table 'ChucVuKiemNhiems'
ALTER TABLE [dbo].[ChucVuKiemNhiems]
ADD CONSTRAINT [FK_ChucVuKiemNhiem_ChucVu]
    FOREIGN KEY ([ChucVu])
    REFERENCES [dbo].[ChucVus]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChucVuKiemNhiem_ChucVu'
CREATE INDEX [IX_FK_ChucVuKiemNhiem_ChucVu]
ON [dbo].[ChucVuKiemNhiems]
    ([ChucVu]);
GO

-- Creating foreign key on [NhanVien] in table 'ChucVuKiemNhiems'
ALTER TABLE [dbo].[ChucVuKiemNhiems]
ADD CONSTRAINT [FK_ChucVuKiemNhiem_NhanVien]
    FOREIGN KEY ([NhanVien])
    REFERENCES [dbo].[NhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChucVuKiemNhiem_NhanVien'
CREATE INDEX [IX_FK_ChucVuKiemNhiem_NhanVien]
ON [dbo].[ChucVuKiemNhiems]
    ([NhanVien]);
GO

-- Creating foreign key on [WebGroup] in table 'ChucVus'
ALTER TABLE [dbo].[ChucVus]
ADD CONSTRAINT [FK_ChucVu_WebGroup]
    FOREIGN KEY ([WebGroup])
    REFERENCES [dbo].[WebGroups]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChucVu_WebGroup'
CREATE INDEX [IX_FK_ChucVu_WebGroup]
ON [dbo].[ChucVus]
    ([WebGroup]);
GO

-- Creating foreign key on [WebGroupID] in table 'WebMenu_Role'
ALTER TABLE [dbo].[WebMenu_Role]
ADD CONSTRAINT [FK_WebMenu_Role_WebGroup]
    FOREIGN KEY ([WebGroupID])
    REFERENCES [dbo].[WebGroups]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebMenu_Role_WebGroup'
CREATE INDEX [IX_FK_WebMenu_Role_WebGroup]
ON [dbo].[WebMenu_Role]
    ([WebGroupID]);
GO

-- Creating foreign key on [WebGroupID] in table 'WebUsers'
ALTER TABLE [dbo].[WebUsers]
ADD CONSTRAINT [FK_WebUsers_WebGroup]
    FOREIGN KEY ([WebGroupID])
    REFERENCES [dbo].[WebGroups]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUsers_WebGroup'
CREATE INDEX [IX_FK_WebUsers_WebGroup]
ON [dbo].[WebUsers]
    ([WebGroupID]);
GO

-- Creating foreign key on [IDBoPhan] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_Department]
    FOREIGN KEY ([IDBoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_Department'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_Department]
ON [dbo].[CC_ChamCongNgayNghi]
    ([IDBoPhan]);
GO

-- Creating foreign key on [KhaiBaoCongTac] in table 'CC_Attachments'
ALTER TABLE [dbo].[CC_Attachments]
ADD CONSTRAINT [FK_CC_Attachments_CC_KhaiBaoCongTac]
    FOREIGN KEY ([KhaiBaoCongTac])
    REFERENCES [dbo].[CC_KhaiBaoCongTac]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_Attachments_CC_KhaiBaoCongTac'
CREATE INDEX [IX_FK_CC_Attachments_CC_KhaiBaoCongTac]
ON [dbo].[CC_Attachments]
    ([KhaiBaoCongTac]);
GO

-- Creating foreign key on [IDNhanVien] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_IDNhanVien]
    FOREIGN KEY ([IDNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_IDNhanVien'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_IDNhanVien]
ON [dbo].[CC_ChamCongNgayNghi]
    ([IDNhanVien]);
GO

-- Creating foreign key on [IDWebUser] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_WebUser]
    FOREIGN KEY ([IDWebUser])
    REFERENCES [dbo].[WebUsers]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_WebUser'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_WebUser]
ON [dbo].[CC_ChamCongNgayNghi]
    ([IDWebUser]);
GO

-- Creating foreign key on [IDNhanVien] in table 'CC_KhaiBaoCongTac'
ALTER TABLE [dbo].[CC_KhaiBaoCongTac]
ADD CONSTRAINT [FK_CC_KhaiBaoCongTac_HoSo]
    FOREIGN KEY ([IDNhanVien])
    REFERENCES [dbo].[HoSoes]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_KhaiBaoCongTac_HoSo'
CREATE INDEX [IX_FK_CC_KhaiBaoCongTac_HoSo]
ON [dbo].[CC_KhaiBaoCongTac]
    ([IDNhanVien]);
GO

-- Creating foreign key on [NguoiKy] in table 'CC_KhaiBaoCongTac'
ALTER TABLE [dbo].[CC_KhaiBaoCongTac]
ADD CONSTRAINT [FK_CC_KhaiBaoCongTac_NguoiKy]
    FOREIGN KEY ([NguoiKy])
    REFERENCES [dbo].[HoSoes]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_KhaiBaoCongTac_NguoiKy'
CREATE INDEX [IX_FK_CC_KhaiBaoCongTac_NguoiKy]
ON [dbo].[CC_KhaiBaoCongTac]
    ([NguoiKy]);
GO

-- Creating foreign key on [CongTy] in table 'CC_NgayNghiTrongNam'
ALTER TABLE [dbo].[CC_NgayNghiTrongNam]
ADD CONSTRAINT [FK_CC_NgayNghiTrongNam_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_NgayNghiTrongNam_CongTy'
CREATE INDEX [IX_FK_CC_NgayNghiTrongNam_CongTy]
ON [dbo].[CC_NgayNghiTrongNam]
    ([CongTy]);
GO

-- Creating foreign key on [BoPhanID] in table 'WebUser_BoPhan'
ALTER TABLE [dbo].[WebUser_BoPhan]
ADD CONSTRAINT [FK_WebUser_BoPhan_BoPhanID]
    FOREIGN KEY ([BoPhanID])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUser_BoPhan_BoPhanID'
CREATE INDEX [IX_FK_WebUser_BoPhan_BoPhanID]
ON [dbo].[WebUser_BoPhan]
    ([BoPhanID]);
GO

-- Creating foreign key on [ThongTinNhanVien] in table 'WebUser_BoPhan'
ALTER TABLE [dbo].[WebUser_BoPhan]
ADD CONSTRAINT [FK_WebUser_BoPhan_ThongTinNhanVien]
    FOREIGN KEY ([ThongTinNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUser_BoPhan_ThongTinNhanVien'
CREATE INDEX [IX_FK_WebUser_BoPhan_ThongTinNhanVien]
ON [dbo].[WebUser_BoPhan]
    ([ThongTinNhanVien]);
GO

-- Creating foreign key on [WebGroup] in table 'WebUser_BoPhan'
ALTER TABLE [dbo].[WebUser_BoPhan]
ADD CONSTRAINT [FK_WebUser_BoPhan_WebGroup]
    FOREIGN KEY ([WebGroup])
    REFERENCES [dbo].[WebGroups]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUser_BoPhan_WebGroup'
CREATE INDEX [IX_FK_WebUser_BoPhan_WebGroup]
ON [dbo].[WebUser_BoPhan]
    ([WebGroup]);
GO

-- Creating foreign key on [IDWebUser] in table 'WebUser_BoPhan'
ALTER TABLE [dbo].[WebUser_BoPhan]
ADD CONSTRAINT [FK_WebUser_BoPhan_WebUsers]
    FOREIGN KEY ([IDWebUser])
    REFERENCES [dbo].[WebUsers]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUser_BoPhan_WebUsers'
CREATE INDEX [IX_FK_WebUser_BoPhan_WebUsers]
ON [dbo].[WebUser_BoPhan]
    ([IDWebUser]);
GO

-- Creating foreign key on [CC_CaChamCong] in table 'CC_ChamCongTheoNgay'
ALTER TABLE [dbo].[CC_ChamCongTheoNgay]
ADD CONSTRAINT [FK_CC_ChamCongTheoNgay_CC_CaChamCong]
    FOREIGN KEY ([CC_CaChamCong])
    REFERENCES [dbo].[CC_CaChamCong]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongTheoNgay_CC_CaChamCong'
CREATE INDEX [IX_FK_CC_ChamCongTheoNgay_CC_CaChamCong]
ON [dbo].[CC_ChamCongTheoNgay]
    ([CC_CaChamCong]);
GO

-- Creating foreign key on [CaChamCong] in table 'CC_DangKyKhungGioLamViec'
ALTER TABLE [dbo].[CC_DangKyKhungGioLamViec]
ADD CONSTRAINT [FK_CC_DangKyKhungGioLamViec_CC_CaChamCong]
    FOREIGN KEY ([CaChamCong])
    REFERENCES [dbo].[CC_CaChamCong]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_DangKyKhungGioLamViec_CC_CaChamCong'
CREATE INDEX [IX_FK_CC_DangKyKhungGioLamViec_CC_CaChamCong]
ON [dbo].[CC_DangKyKhungGioLamViec]
    ([CaChamCong]);
GO

-- Creating foreign key on [HinhThucNghi] in table 'CC_CaChamCong'
ALTER TABLE [dbo].[CC_CaChamCong]
ADD CONSTRAINT [FK_CC_CaChamCong_HinhThucNghi]
    FOREIGN KEY ([HinhThucNghi])
    REFERENCES [dbo].[CC_HinhThucNghi]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_CaChamCong_HinhThucNghi'
CREATE INDEX [IX_FK_CC_CaChamCong_HinhThucNghi]
ON [dbo].[CC_CaChamCong]
    ([HinhThucNghi]);
GO

-- Creating foreign key on [IDHinhThucNghi] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_CC_HinhThucNghi]
    FOREIGN KEY ([IDHinhThucNghi])
    REFERENCES [dbo].[CC_HinhThucNghi]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_CC_HinhThucNghi'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_CC_HinhThucNghi]
ON [dbo].[CC_ChamCongNgayNghi]
    ([IDHinhThucNghi]);
GO

-- Creating foreign key on [IDHinhThucNghi] in table 'CC_ChamCongTheoNgay'
ALTER TABLE [dbo].[CC_ChamCongTheoNgay]
ADD CONSTRAINT [FK_CC_ChamCongTheoNgay_CC_HinhThucNghi]
    FOREIGN KEY ([IDHinhThucNghi])
    REFERENCES [dbo].[CC_HinhThucNghi]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongTheoNgay_CC_HinhThucNghi'
CREATE INDEX [IX_FK_CC_ChamCongTheoNgay_CC_HinhThucNghi]
ON [dbo].[CC_ChamCongTheoNgay]
    ([IDHinhThucNghi]);
GO

-- Creating foreign key on [HinhThucChieu] in table 'CC_HinhThucKhac'
ALTER TABLE [dbo].[CC_HinhThucKhac]
ADD CONSTRAINT [FK_CC_HinhThucChieu_CC_HinhThucNghi]
    FOREIGN KEY ([HinhThucChieu])
    REFERENCES [dbo].[CC_HinhThucNghi]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_HinhThucChieu_CC_HinhThucNghi'
CREATE INDEX [IX_FK_CC_HinhThucChieu_CC_HinhThucNghi]
ON [dbo].[CC_HinhThucKhac]
    ([HinhThucChieu]);
GO

-- Creating foreign key on [HinhThucSang] in table 'CC_HinhThucKhac'
ALTER TABLE [dbo].[CC_HinhThucKhac]
ADD CONSTRAINT [FK_CC_HinhThucSang_CC_HinhThucNghi]
    FOREIGN KEY ([HinhThucSang])
    REFERENCES [dbo].[CC_HinhThucNghi]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_HinhThucSang_CC_HinhThucNghi'
CREATE INDEX [IX_FK_CC_HinhThucSang_CC_HinhThucNghi]
ON [dbo].[CC_HinhThucKhac]
    ([HinhThucSang]);
GO

-- Creating foreign key on [CongTy] in table 'CC_HinhThucNghi'
ALTER TABLE [dbo].[CC_HinhThucNghi]
ADD CONSTRAINT [FK_CC_HinhThucNghi_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_HinhThucNghi_CongTy'
CREATE INDEX [IX_FK_CC_HinhThucNghi_CongTy]
ON [dbo].[CC_HinhThucNghi]
    ([CongTy]);
GO

-- Creating foreign key on [NienDoTaiChinh] in table 'CC_QuanLyNghiPhep'
ALTER TABLE [dbo].[CC_QuanLyNghiPhep]
ADD CONSTRAINT [FK_CC_QuanLyNghiPhep_NienDoTaiChinh]
    FOREIGN KEY ([NienDoTaiChinh])
    REFERENCES [dbo].[NienDoTaiChinhs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_QuanLyNghiPhep_NienDoTaiChinh'
CREATE INDEX [IX_FK_CC_QuanLyNghiPhep_NienDoTaiChinh]
ON [dbo].[CC_QuanLyNghiPhep]
    ([NienDoTaiChinh]);
GO

-- Creating foreign key on [CongTy] in table 'CC_QuanLyPhepHe'
ALTER TABLE [dbo].[CC_QuanLyPhepHe]
ADD CONSTRAINT [FK_CC_QuanLyPhepHe_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_QuanLyPhepHe_CongTy'
CREATE INDEX [IX_FK_CC_QuanLyPhepHe_CongTy]
ON [dbo].[CC_QuanLyPhepHe]
    ([CongTy]);
GO

-- Creating foreign key on [NienDoTaiChinh] in table 'CC_QuanLyPhepHe'
ALTER TABLE [dbo].[CC_QuanLyPhepHe]
ADD CONSTRAINT [FK_CC_QuanLyPhepHe_NienDoTaiChinh]
    FOREIGN KEY ([NienDoTaiChinh])
    REFERENCES [dbo].[NienDoTaiChinhs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_QuanLyPhepHe_NienDoTaiChinh'
CREATE INDEX [IX_FK_CC_QuanLyPhepHe_NienDoTaiChinh]
ON [dbo].[CC_QuanLyPhepHe]
    ([NienDoTaiChinh]);
GO

-- Creating foreign key on [CongTy] in table 'NienDoTaiChinhs'
ALTER TABLE [dbo].[NienDoTaiChinhs]
ADD CONSTRAINT [FK_NienDoTaiChinh_CongTy]
    FOREIGN KEY ([CongTy])
    REFERENCES [dbo].[CongTies]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NienDoTaiChinh_CongTy'
CREATE INDEX [IX_FK_NienDoTaiChinh_CongTy]
ON [dbo].[NienDoTaiChinhs]
    ([CongTy]);
GO

-- Creating foreign key on [BoPhan] in table 'CC_ChiTietPhepHe'
ALTER TABLE [dbo].[CC_ChiTietPhepHe]
ADD CONSTRAINT [FK_CC_ChiTietPhepHe_BoPhan]
    FOREIGN KEY ([BoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietPhepHe_BoPhan'
CREATE INDEX [IX_FK_CC_ChiTietPhepHe_BoPhan]
ON [dbo].[CC_ChiTietPhepHe]
    ([BoPhan]);
GO

-- Creating foreign key on [QuanLyPhepHe] in table 'CC_ChiTietPhepHe'
ALTER TABLE [dbo].[CC_ChiTietPhepHe]
ADD CONSTRAINT [FK_CC_ChiTietPhepHe_CC_QuanLyPhepHe]
    FOREIGN KEY ([QuanLyPhepHe])
    REFERENCES [dbo].[CC_QuanLyPhepHe]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietPhepHe_CC_QuanLyPhepHe'
CREATE INDEX [IX_FK_CC_ChiTietPhepHe_CC_QuanLyPhepHe]
ON [dbo].[CC_ChiTietPhepHe]
    ([QuanLyPhepHe]);
GO

-- Creating foreign key on [ThongTinNhanVien] in table 'CC_ChiTietPhepHe'
ALTER TABLE [dbo].[CC_ChiTietPhepHe]
ADD CONSTRAINT [FK_CC_ChiTietPhepHe_ThongTinNhanVien]
    FOREIGN KEY ([ThongTinNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietPhepHe_ThongTinNhanVien'
CREATE INDEX [IX_FK_CC_ChiTietPhepHe_ThongTinNhanVien]
ON [dbo].[CC_ChiTietPhepHe]
    ([ThongTinNhanVien]);
GO

-- Creating foreign key on [ChucDanh] in table 'CC_ChiTietPhepHe'
ALTER TABLE [dbo].[CC_ChiTietPhepHe]
ADD CONSTRAINT [FK_CC_ChiTietPhepHe_ChucDanh]
    FOREIGN KEY ([ChucDanh])
    REFERENCES [dbo].[ChucDanhs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChiTietPhepHe_ChucDanh'
CREATE INDEX [IX_FK_CC_ChiTietPhepHe_ChucDanh]
ON [dbo].[CC_ChiTietPhepHe]
    ([ChucDanh]);
GO

-- Creating foreign key on [ChucVu] in table 'ChucDanhs'
ALTER TABLE [dbo].[ChucDanhs]
ADD CONSTRAINT [FK_ChucDanh_ChucVu]
    FOREIGN KEY ([ChucVu])
    REFERENCES [dbo].[ChucVus]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChucDanh_ChucVu'
CREATE INDEX [IX_FK_ChucDanh_ChucVu]
ON [dbo].[ChucDanhs]
    ([ChucVu]);
GO

-- Creating foreign key on [ChucDanh] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NhanVien_ChucDanh]
    FOREIGN KEY ([ChucDanh])
    REFERENCES [dbo].[ChucDanhs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhanVien_ChucDanh'
CREATE INDEX [IX_FK_NhanVien_ChucDanh]
ON [dbo].[NhanViens]
    ([ChucDanh]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------