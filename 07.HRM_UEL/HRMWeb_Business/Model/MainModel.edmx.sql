
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/25/2019 10:31:05
-- Generated from EDMX file: D:\Projects\PSC\HRMWebApp\Releases\07.HRM_UEL\HRMWeb_Business\Model\MainModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PSC_HRM3_UEL_Bao];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BangLuongNhanVien_KyTinhLuong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BangLuongNhanVien] DROP CONSTRAINT [FK_BangLuongNhanVien_KyTinhLuong];
GO
IF OBJECT_ID(N'[dbo].[FK_BoPhan_BoPhanCha]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BoPhan] DROP CONSTRAINT [FK_BoPhan_BoPhanCha];
GO
IF OBJECT_ID(N'[dbo].[FK_BoPhan_BoPhanChaOld]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BoPhan] DROP CONSTRAINT [FK_BoPhan_BoPhanChaOld];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_CacBuoiTrongNgay]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_CacBuoiTrongNgay];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_CacBuoiTrongNgay1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_CacBuoiTrongNgay1];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_HinhThucNghi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_HinhThucNghi];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_IDBoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_IDBoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_IDHinhThucNghi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_IDHinhThucNghi];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_IDNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_IDNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_IDWebUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_IDWebUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongNgayNghi_TinhThanh]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongNgayNghi] DROP CONSTRAINT [FK_CC_ChamCongNgayNghi_TinhThanh];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongTheoNgay_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongTheoNgay] DROP CONSTRAINT [FK_CC_ChamCongTheoNgay_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongTheoNgay_CC_CaChamCong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongTheoNgay] DROP CONSTRAINT [FK_CC_ChamCongTheoNgay_CC_CaChamCong];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongTheoNgay_HinhThucNghi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongTheoNgay] DROP CONSTRAINT [FK_CC_ChamCongTheoNgay_HinhThucNghi];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_ChamCongTheoNgay_HoSo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_ChamCongTheoNgay] DROP CONSTRAINT [FK_CC_ChamCongTheoNgay_HoSo];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_DangKyChamCongNgoaiGio_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_DangKyChamCongNgoaiGio] DROP CONSTRAINT [FK_CC_DangKyChamCongNgoaiGio_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_DangKyChamCongNgoaiGio_WebUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_DangKyChamCongNgoaiGio] DROP CONSTRAINT [FK_CC_DangKyChamCongNgoaiGio_WebUsers];
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
IF OBJECT_ID(N'[dbo].[FK_CC_KhaiBaoChamCongGiangVien_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_KhaiBaoChamCongGiangVien] DROP CONSTRAINT [FK_CC_KhaiBaoChamCongGiangVien_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_CC_KhaiBaoCongTac_HoSo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_KhaiBaoCongTac] DROP CONSTRAINT [FK_CC_KhaiBaoCongTac_HoSo];
GO
IF OBJECT_ID(N'[dbo].[FK_cc_QuanLyViPham_CC_ChamCongTheoNgay]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyViPham] DROP CONSTRAINT [FK_cc_QuanLyViPham_CC_ChamCongTheoNgay];
GO
IF OBJECT_ID(N'[dbo].[FK_cc_QuanLyViPham_cc_HinhThucViPham]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyViPham] DROP CONSTRAINT [FK_cc_QuanLyViPham_cc_HinhThucViPham];
GO
IF OBJECT_ID(N'[dbo].[FK_cc_QuanLyViPham_New_CC_ChamCongTheoNgay]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CC_QuanLyViPham_New] DROP CONSTRAINT [FK_cc_QuanLyViPham_New_CC_ChamCongTheoNgay];
GO
IF OBJECT_ID(N'[dbo].[FK_ChiTietChamCongNhanVien_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietChamCongNhanVien] DROP CONSTRAINT [FK_ChiTietChamCongNhanVien_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_ChiTietChamCongNhanVien_QuanLyChamCongNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietChamCongNhanVien] DROP CONSTRAINT [FK_ChiTietChamCongNhanVien_QuanLyChamCongNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_ChiTietChamCongNhanVien_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietChamCongNhanVien] DROP CONSTRAINT [FK_ChiTietChamCongNhanVien_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_ChiTietNghiPhep_ThongTinNghiPhep]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChiTietNghiPhep] DROP CONSTRAINT [FK_ChiTietNghiPhep_ThongTinNghiPhep];
GO
IF OBJECT_ID(N'[dbo].[FK_GiayToHoSo_DangLuuTru]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiayToHoSo] DROP CONSTRAINT [FK_GiayToHoSo_DangLuuTru];
GO
IF OBJECT_ID(N'[dbo].[FK_GiayToHoSo_GiayTo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiayToHoSo] DROP CONSTRAINT [FK_GiayToHoSo_GiayTo];
GO
IF OBJECT_ID(N'[dbo].[FK_GiayToHoSo_HoSo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GiayToHoSo] DROP CONSTRAINT [FK_GiayToHoSo_HoSo];
GO
IF OBJECT_ID(N'[dbo].[FK_HoSo_NoiCap]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoSo] DROP CONSTRAINT [FK_HoSo_NoiCap];
GO
IF OBJECT_ID(N'[dbo].[FK_KyTinhLuong_BangChotThongTinTinhLuong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KyTinhLuong] DROP CONSTRAINT [FK_KyTinhLuong_BangChotThongTinTinhLuong];
GO
IF OBJECT_ID(N'[dbo].[FK_NgayNghiTrongNam_QuanLyNgayNghiTrongNam]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NgayNghiTrongNam] DROP CONSTRAINT [FK_NgayNghiTrongNam_QuanLyNgayNghiTrongNam];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_BoPhanCu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_BoPhanCu];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_BoPhanTinhLuong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_BoPhanTinhLuong];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_ChucDanh]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_ChucDanh];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_Oid]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_Oid];
GO
IF OBJECT_ID(N'[dbo].[FK_NhanVien_TinhTrang]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NhanVien] DROP CONSTRAINT [FK_NhanVien_TinhTrang];
GO
IF OBJECT_ID(N'[dbo].[FK_QuanLyChamCongNhanVien_KyTinhLuong]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuanLyChamCongNhanVien] DROP CONSTRAINT [FK_QuanLyChamCongNhanVien_KyTinhLuong];
GO
IF OBJECT_ID(N'[dbo].[FK_ThongTinNghiPhep_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThongTinNghiPhep] DROP CONSTRAINT [FK_ThongTinNghiPhep_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_ThongTinNghiPhep_QuanLyNghiPhep]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThongTinNghiPhep] DROP CONSTRAINT [FK_ThongTinNghiPhep_QuanLyNghiPhep];
GO
IF OBJECT_ID(N'[dbo].[FK_ThongTinNghiPhep_ThongTinNhanVien]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThongTinNghiPhep] DROP CONSTRAINT [FK_ThongTinNghiPhep_ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[FK_ThongTinNhanVien_ChucVu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThongTinNhanVien] DROP CONSTRAINT [FK_ThongTinNhanVien_ChucVu];
GO
IF OBJECT_ID(N'[dbo].[FK_ThongTinNhanVien_ChucVuCoQuanCaoNhat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThongTinNhanVien] DROP CONSTRAINT [FK_ThongTinNhanVien_ChucVuCoQuanCaoNhat];
GO
IF OBJECT_ID(N'[dbo].[FK_ThongTinNhanVien_ChucVuKiemNhiem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ThongTinNhanVien] DROP CONSTRAINT [FK_ThongTinNhanVien_ChucVuKiemNhiem];
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
IF OBJECT_ID(N'[dbo].[FK_WebMenu_Role_WebMenu1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebMenu_Role] DROP CONSTRAINT [FK_WebMenu_Role_WebMenu1];
GO
IF OBJECT_ID(N'[dbo].[FK_WebMenu_Role_webRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebMenu_Role] DROP CONSTRAINT [FK_WebMenu_Role_webRole];
GO
IF OBJECT_ID(N'[dbo].[FK_WebMenu_WebMenu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebMenu] DROP CONSTRAINT [FK_WebMenu_WebMenu];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUser_BoPhan_BoPhan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUser_BoPhan] DROP CONSTRAINT [FK_WebUser_BoPhan_BoPhan];
GO
IF OBJECT_ID(N'[dbo].[FK_WebUser_BoPhan_WebUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WebUser_BoPhan] DROP CONSTRAINT [FK_WebUser_BoPhan_WebUsers];
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

IF OBJECT_ID(N'[dbo].[BangChotThongTinTinhLuong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BangChotThongTinTinhLuong];
GO
IF OBJECT_ID(N'[dbo].[BangLuongNhanVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BangLuongNhanVien];
GO
IF OBJECT_ID(N'[dbo].[BoPhan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BoPhan];
GO
IF OBJECT_ID(N'[dbo].[CacBuoiTrongNgay]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CacBuoiTrongNgay];
GO
IF OBJECT_ID(N'[dbo].[CauHinhXetABC]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CauHinhXetABC];
GO
IF OBJECT_ID(N'[dbo].[CC_CaChamCong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_CaChamCong];
GO
IF OBJECT_ID(N'[dbo].[CC_ChamCongNgayNghi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ChamCongNgayNghi];
GO
IF OBJECT_ID(N'[dbo].[CC_ChamCongTheoNgay]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ChamCongTheoNgay];
GO
IF OBJECT_ID(N'[dbo].[CC_DangKyChamCongNgoaiGio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_DangKyChamCongNgoaiGio];
GO
IF OBJECT_ID(N'[dbo].[CC_DangKyKhungGioLamViec]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_DangKyKhungGioLamViec];
GO
IF OBJECT_ID(N'[dbo].[CC_HinhThucViPham]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_HinhThucViPham];
GO
IF OBJECT_ID(N'[dbo].[CC_KhaiBaoChamCongGiangVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_KhaiBaoChamCongGiangVien];
GO
IF OBJECT_ID(N'[dbo].[CC_KhaiBaoCongTac]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_KhaiBaoCongTac];
GO
IF OBJECT_ID(N'[dbo].[CC_KyDangKyKhungGio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_KyDangKyKhungGio];
GO
IF OBJECT_ID(N'[dbo].[CC_LyDoDangKyChamCongNgoaiGio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_LyDoDangKyChamCongNgoaiGio];
GO
IF OBJECT_ID(N'[dbo].[CC_QuanLyViPham]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_QuanLyViPham];
GO
IF OBJECT_ID(N'[dbo].[CC_QuanLyViPham_New]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_QuanLyViPham_New];
GO
IF OBJECT_ID(N'[dbo].[CC_ThoiGianDangKyKhungGioLamViec]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CC_ThoiGianDangKyKhungGioLamViec];
GO
IF OBJECT_ID(N'[dbo].[ChiTietChamCongNhanVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChiTietChamCongNhanVien];
GO
IF OBJECT_ID(N'[dbo].[ChiTietNghiPhep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChiTietNghiPhep];
GO
IF OBJECT_ID(N'[dbo].[ChucDanh]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChucDanh];
GO
IF OBJECT_ID(N'[dbo].[ChucVu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChucVu];
GO
IF OBJECT_ID(N'[dbo].[DangLuuTru]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DangLuuTru];
GO
IF OBJECT_ID(N'[dbo].[GiayTo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GiayTo];
GO
IF OBJECT_ID(N'[dbo].[GiayToHoSo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GiayToHoSo];
GO
IF OBJECT_ID(N'[dbo].[HinhThucNghi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HinhThucNghi];
GO
IF OBJECT_ID(N'[dbo].[HoSo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HoSo];
GO
IF OBJECT_ID(N'[dbo].[KyTinhLuong]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KyTinhLuong];
GO
IF OBJECT_ID(N'[dbo].[LoaiNhanSu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LoaiNhanSu];
GO
IF OBJECT_ID(N'[dbo].[NgayNghiTrongNam]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NgayNghiTrongNam];
GO
IF OBJECT_ID(N'[dbo].[NhanVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NhanVien];
GO
IF OBJECT_ID(N'[dbo].[QuanLyChamCongNhanVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuanLyChamCongNhanVien];
GO
IF OBJECT_ID(N'[dbo].[QuanLyNgayNghiTrongNam]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuanLyNgayNghiTrongNam];
GO
IF OBJECT_ID(N'[dbo].[QuanLyNghiPhep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuanLyNghiPhep];
GO
IF OBJECT_ID(N'[dbo].[ThongTinNghiPhep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ThongTinNghiPhep];
GO
IF OBJECT_ID(N'[dbo].[ThongTinNhanVien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ThongTinNhanVien];
GO
IF OBJECT_ID(N'[dbo].[TinhThanh]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TinhThanh];
GO
IF OBJECT_ID(N'[dbo].[TinhTrang]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TinhTrang];
GO
IF OBJECT_ID(N'[dbo].[Web_GiayChungNhan_AutoNumber]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Web_GiayChungNhan_AutoNumber];
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

-- Creating table 'BangChotThongTinTinhLuongs'
CREATE TABLE [dbo].[BangChotThongTinTinhLuongs] (
    [Oid] uniqueidentifier  NOT NULL,
    [Thang] datetime  NULL,
    [KhoaSo] bit  NULL,
    [DaCapNhatThamNienCuaThang] bit  NULL,
    [LoaiLuong] tinyint  NULL,
    [ThongTinTruong] uniqueidentifier  NULL,
    [Nam] int  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'BangLuongNhanViens'
CREATE TABLE [dbo].[BangLuongNhanViens] (
    [Oid] uniqueidentifier  NOT NULL,
    [KyTinhLuong] uniqueidentifier  NULL,
    [NgayLap] datetime  NULL,
    [HienLenWeb] bit  NULL,
    [ThongTinTruong] uniqueidentifier  NULL,
    [LoaiLuong] tinyint  NULL,
    [ChungTu] uniqueidentifier  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'BoPhans'
CREATE TABLE [dbo].[BoPhans] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [STT] int  NULL,
    [TenBoPhan] nvarchar(100)  NULL,
    [LoaiBoPhan] tinyint  NULL,
    [BoPhanCha] uniqueidentifier  NULL,
    [BoPhanChaOld] uniqueidentifier  NULL,
    [NgungHoatDong] bit  NULL,
    [TenBoPhanVietTat] nvarchar(100)  NULL,
    [ThongTinTruong] uniqueidentifier  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [ObjectType] int  NULL,
    [MThamSoPCTrachNhiem] decimal(19,4)  NULL,
    [MThamSoLuongKy2] decimal(19,4)  NULL,
    [SoGiangVien] int  NULL,
    [SoSinhVien] int  NULL,
    [TrinhDoChuyenMonCaoNhat] uniqueidentifier  NULL,
    [TuChiLuong] bit  NULL,
    [TenBoPhanENG] nvarchar(100)  NULL
);
GO

-- Creating table 'CacBuoiTrongNgays'
CREATE TABLE [dbo].[CacBuoiTrongNgays] (
    [Oid] uniqueidentifier  NOT NULL,
    [TenBuoi] nvarchar(50)  NULL,
    [GiaTri] decimal(18,2)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'CC_CaChamCong'
CREATE TABLE [dbo].[CC_CaChamCong] (
    [Oid] uniqueidentifier  NOT NULL,
    [ThoiGianVaoSang] nvarchar(50)  NULL,
    [ThoiGianRaSang] nvarchar(50)  NULL,
    [ThoiGianBatDauNghiGiuaCa] nvarchar(50)  NULL,
    [ThoiGianKetThucNghiGiuaCa] nvarchar(50)  NULL,
    [ThoiGianVaoChieu] nvarchar(50)  NULL,
    [ThoiGianRaChieu] nvarchar(50)  NULL,
    [SoPhutCong] int  NULL,
    [SoPhutTru] int  NULL,
    [TongSoGioLamViec] int  NULL,
    [Active] bit  NULL,
    [LoaiCa] tinyint  NOT NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [TongSoGioLamViecBuoiSang] decimal(18,1)  NULL,
    [TongSoGioLamViecBuoiChieu] decimal(18,1)  NULL,
    [TongSoGioLamViecCaNgay] decimal(18,1)  NULL,
    [TenCa] nvarchar(max)  NULL,
    [TongSoGioNghiGiuaCa] decimal(18,1)  NULL,
    [ThoiGianBDQuetBuoiSang] nvarchar(max)  NULL,
    [ThoiGianKTQuetBuoiChieu] nvarchar(max)  NULL,
    [LamThu7] bit  NULL
);
GO

-- Creating table 'CC_ChamCongNgayNghi'
CREATE TABLE [dbo].[CC_ChamCongNgayNghi] (
    [Oid] uniqueidentifier  NOT NULL,
    [BangChamCongNgayNghi] uniqueidentifier  NULL,
    [IDBoPhan] uniqueidentifier  NULL,
    [IDNhanVien] uniqueidentifier  NULL,
    [CC_HinhThucNghi] uniqueidentifier  NULL,
    [IDHinhThucNghi] uniqueidentifier  NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL,
    [CacBuoiTrongNgay_TuNgay] uniqueidentifier  NULL,
    [CacBuoiTrongNgay_DenNgay] uniqueidentifier  NULL,
    [SoNgay] decimal(18,2)  NULL,
    [XepLoaiDanhGia] tinyint  NULL,
    [DienGiai] nvarchar(100)  NULL,
    [IDWebUsers] uniqueidentifier  NULL,
    [NgayTao] datetime  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [IDWebUser] uniqueidentifier  NULL,
    [LoaiNghiPhep] int  NULL,
    [NoiNghi] nvarchar(max)  NULL,
    [DiaChiLienHe] nvarchar(max)  NULL,
    [TenGiayXinPhep] nvarchar(max)  NULL,
    [TrangThai] int  NULL,
    [TrangThaiAdmin] int  NULL,
    [TinhThanh] uniqueidentifier  NULL,
    [TruNgayDiDuong] bit  NULL,
    [SoNgayDiDuong] decimal(18,2)  NULL,
    [HinhThucNghi] uniqueidentifier  NULL,
    [NuaBuoiTuNgay] nchar(10)  NULL,
    [SoNgayPhepConLai] decimal(18,2)  NULL
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
    [IDHinhThucNghiOld] uniqueidentifier  NULL,
    [CC_CaChamCong] uniqueidentifier  NULL,
    [GhiChu] nvarchar(max)  NULL,
    [NguoiDungChinhSua] bit  NULL,
    [IDWebUsers_ChinhSua] uniqueidentifier  NULL,
    [ChuoiGioQuet] nvarchar(max)  NULL
);
GO

-- Creating table 'CC_DangKyChamCongNgoaiGio'
CREATE TABLE [dbo].[CC_DangKyChamCongNgoaiGio] (
    [Oid] uniqueidentifier  NOT NULL,
    [IDNhanVien] uniqueidentifier  NULL,
    [IDBoPhan] uniqueidentifier  NULL,
    [Ngay] datetime  NULL,
    [SoPhutThucTe] decimal(18,1)  NULL,
    [SoPhutDangKy] decimal(18,1)  NULL,
    [LyDo] nvarchar(max)  NULL,
    [Duyet] tinyint  NULL,
    [TuGio] nvarchar(50)  NULL,
    [DenGio] nvarchar(50)  NULL,
    [NgayTao] datetime  NULL,
    [NguoiTao] uniqueidentifier  NULL
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

-- Creating table 'CC_KhaiBaoChamCongGiangVien'
CREATE TABLE [dbo].[CC_KhaiBaoChamCongGiangVien] (
    [Oid] uniqueidentifier  NOT NULL,
    [Ngay] datetime  NULL,
    [Buoi] tinyint  NULL,
    [IDNhanVien] uniqueidentifier  NULL
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
    [DiaDiem] nvarchar(max)  NULL
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

-- Creating table 'CC_QuanLyViPham'
CREATE TABLE [dbo].[CC_QuanLyViPham] (
    [Oid] uniqueidentifier  NOT NULL,
    [cc_HinhThucViPham] uniqueidentifier  NULL,
    [ThoiGianTre] int  NULL,
    [ThoiGianSom] int  NULL,
    [ChamCongTheoNgay] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_QuanLyViPham_New'
CREATE TABLE [dbo].[CC_QuanLyViPham_New] (
    [Oid] uniqueidentifier  NOT NULL,
    [ChuoiHinhThucViPham] nvarchar(max)  NULL,
    [ChuoiGioQuet] nvarchar(max)  NULL,
    [CC_ChamCongTheoNgay] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_ThoiGianDangKyKhungGioLamViec'
CREATE TABLE [dbo].[CC_ThoiGianDangKyKhungGioLamViec] (
    [Oid] uniqueidentifier  NOT NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL
);
GO

-- Creating table 'ChiTietChamCongNhanViens'
CREATE TABLE [dbo].[ChiTietChamCongNhanViens] (
    [Oid] uniqueidentifier  NOT NULL,
    [QuanLyChamCongNhanVien] uniqueidentifier  NULL,
    [BoPhan] uniqueidentifier  NULL,
    [ThongTinNhanVien] uniqueidentifier  NULL,
    [SoNgayCong] decimal(18,1)  NULL,
    [NghiNuaNgay] decimal(18,1)  NULL,
    [NghiCoPhep] decimal(18,1)  NULL,
    [NghiRo] decimal(18,1)  NULL,
    [NghiThaiSan] decimal(18,1)  NULL,
    [NghiHe] decimal(18,1)  NULL,
    [DanhGia] nvarchar(100)  NULL,
    [DanhGiaTruocDieuChinh] nvarchar(100)  NULL,
    [DienGiai] nvarchar(max)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [TrangThai] bit  NULL,
    [Khoa] bit  NULL,
    [NghiOm] decimal(18,1)  NULL,
    [SoNgayCongSauDieuChinh] decimal(19,4)  NULL,
    [SoNgayCongTruocDieuChinh] decimal(19,4)  NULL,
    [TongNgayCong] decimal(19,4)  NULL,
    [BoPhanTheoBangCong] nvarchar(100)  NULL,
    [NghiDiHocKhongLuong] decimal(18,1)  NULL,
    [NghiDiHocCoLuong] decimal(18,1)  NULL,
    [XepLoaiDanhGia] uniqueidentifier  NULL,
    [SoNgayHuongPCSauDieuChinh] decimal(19,4)  NULL,
    [SoNgayHuongPCTruocDieuChinh] decimal(19,4)  NULL
);
GO

-- Creating table 'ChiTietNghiPheps'
CREATE TABLE [dbo].[ChiTietNghiPheps] (
    [Oid] uniqueidentifier  NOT NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [ThongTinNghiPhep] uniqueidentifier  NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL,
    [TongSoNgayNghiPhep] int  NULL,
    [SoNgayQuaHan] int  NULL,
    [Khoa] bit  NULL,
    [GhiChu] nvarchar(100)  NULL
);
GO

-- Creating table 'ChucDanhs'
CREATE TABLE [dbo].[ChucDanhs] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenChucDanh] nvarchar(100)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [TenChucDanhENG] nvarchar(100)  NULL
);
GO

-- Creating table 'ChucVus'
CREATE TABLE [dbo].[ChucVus] (
    [Oid] uniqueidentifier  NOT NULL,
    [ThuTu] int  NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenChucVu] nvarchar(100)  NULL,
    [PhanLoai] tinyint  NULL,
    [HSPCChucVu] decimal(19,4)  NULL,
    [HSPCQuanLy] decimal(19,4)  NULL,
    [PhuCapDienThoai] decimal(19,4)  NULL,
    [SoLitXang] decimal(19,4)  NULL,
    [LaQuanLy] bit  NULL,
    [GhiChu] nvarchar(100)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [TyTrongDiemDanhGia] decimal(19,4)  NULL,
    [AgentObjectTypeId] int  NULL,
    [HeSoTCDLD] decimal(19,4)  NULL,
    [HeSoTNTT] decimal(19,4)  NULL,
    [PhuCapDienThoaiNR] decimal(19,4)  NULL,
    [PhuCapDieuHanhCV] decimal(19,4)  NULL
);
GO

-- Creating table 'DangLuuTrus'
CREATE TABLE [dbo].[DangLuuTrus] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenDangLuuTru] nvarchar(100)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'GiayToes'
CREATE TABLE [dbo].[GiayToes] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenGiayTo] nvarchar(100)  NULL,
    [LoaiGiayTo] uniqueidentifier  NULL,
    [BatBuoc] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'GiayToHoSoes'
CREATE TABLE [dbo].[GiayToHoSoes] (
    [Oid] uniqueidentifier  NOT NULL,
    [HoSo] uniqueidentifier  NULL,
    [GiayTo] uniqueidentifier  NULL,
    [SoGiayTo] nvarchar(100)  NULL,
    [NgayBanHanh] datetime  NULL,
    [NgayLap] datetime  NULL,
    [DuongDanFile] nvarchar(max)  NULL,
    [DangLuuTru] uniqueidentifier  NULL,
    [SoBan] int  NULL,
    [TrichYeu] nvarchar(500)  NULL,
    [LuuTru] nvarchar(max)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'HinhThucNghis'
CREATE TABLE [dbo].[HinhThucNghis] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenHinhThucNghi] nvarchar(100)  NULL,
    [KyHieu] nvarchar(100)  NULL,
    [PhanLoai] tinyint  NULL,
    [GiaTri] decimal(18,1)  NULL,
    [SoNgayToiDa] decimal(19,4)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [PhanLoaiUTE] tinyint  NULL
);
GO

-- Creating table 'HoSoes'
CREATE TABLE [dbo].[HoSoes] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
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
    [QuocTich] uniqueidentifier  NULL,
    [HinhThucTuyenDung] tinyint  NULL,
    [GhiChu] nvarchar(max)  NULL,
    [BanThoiGian] bit  NULL,
    [IDNhanSu_ChamCong] int  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [ObjectType] int  NULL,
    [STT] nvarchar(100)  NULL,
    [HinhAnh] varbinary(max)  NULL
);
GO

-- Creating table 'KyTinhLuongs'
CREATE TABLE [dbo].[KyTinhLuongs] (
    [Oid] uniqueidentifier  NOT NULL,
    [Thang] int  NULL,
    [Nam] int  NULL,
    [TuNgay] datetime  NULL,
    [DenNgay] datetime  NULL,
    [SoNgay] decimal(19,4)  NULL,
    [KhoaSo] bit  NULL,
    [BangChotThongTinTinhLuong] uniqueidentifier  NULL,
    [ThongTinChung] uniqueidentifier  NULL,
    [MocTinhThueTNCN] uniqueidentifier  NULL,
    [ThongTinTruong] uniqueidentifier  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
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

-- Creating table 'NgayNghiTrongNams'
CREATE TABLE [dbo].[NgayNghiTrongNams] (
    [Oid] uniqueidentifier  NOT NULL,
    [QuanLyNgayNghiTrongNam] uniqueidentifier  NULL,
    [TenNgayNghi] nvarchar(100)  NULL,
    [NgayNghi] datetime  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [NgayNghiBu] datetime  NULL
);
GO

-- Creating table 'NhanViens'
CREATE TABLE [dbo].[NhanViens] (
    [Oid] uniqueidentifier  NOT NULL,
    [NgayNghiViec] datetime  NULL,
    [LaDangVien] bit  NULL,
    [ChucDanh] uniqueidentifier  NULL,
    [BangCapDaKiemDuyet] bit  NULL,
    [HinhAnh] varbinary(max)  NULL,
    [BoPhan] uniqueidentifier  NULL,
    [BoPhanTinhLuong] uniqueidentifier  NULL,
    [ThanhPhanXuatThan] uniqueidentifier  NULL,
    [UuTienGiaDinh] uniqueidentifier  NULL,
    [UuTienBanThan] uniqueidentifier  NULL,
    [CongViecHienNay] uniqueidentifier  NULL,
    [HopDongHienTai] uniqueidentifier  NULL,
    [NgayVaoNganhGiaoDuc] datetime  NULL,
    [NgayVaoNganhNganHang] datetime  NULL,
    [NgayTuyenDung] datetime  NULL,
    [DonViTuyenDung] nvarchar(100)  NULL,
    [CongViecTuyenDung] nvarchar(100)  NULL,
    [CongViecDuocGiao] uniqueidentifier  NULL,
    [NgayVaoCoQuan] datetime  NULL,
    [NhanVienThongTinLuong] uniqueidentifier  NULL,
    [NhanVienTrinhDo] uniqueidentifier  NULL,
    [TinhTrang] uniqueidentifier  NULL,
    [BoPhanCu] uniqueidentifier  NULL,
    [SoThangKhongTinhPhep] decimal(19,4)  NULL,
    [ThongTinTruong] uniqueidentifier  NULL
);
GO

-- Creating table 'QuanLyChamCongNhanViens'
CREATE TABLE [dbo].[QuanLyChamCongNhanViens] (
    [Oid] uniqueidentifier  NOT NULL,
    [ThongTinTruong] uniqueidentifier  NULL,
    [KyTinhLuong] uniqueidentifier  NULL,
    [NgayLap] datetime  NULL,
    [KhoaChamCong] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [CreatedUser] uniqueidentifier  NULL,
    [KyChamCong] uniqueidentifier  NULL,
    [LoaiLuong] tinyint  NULL
);
GO

-- Creating table 'QuanLyNgayNghiTrongNams'
CREATE TABLE [dbo].[QuanLyNgayNghiTrongNams] (
    [Oid] uniqueidentifier  NOT NULL,
    [Nam] int  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'QuanLyNghiPheps'
CREATE TABLE [dbo].[QuanLyNghiPheps] (
    [Oid] uniqueidentifier  NOT NULL,
    [Nam] int  NULL,
    [NgayBatDau] datetime  NULL,
    [NgayKetThuc] datetime  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [ThongTinTruong] uniqueidentifier  NULL
);
GO

-- Creating table 'ThongTinNghiPheps'
CREATE TABLE [dbo].[ThongTinNghiPheps] (
    [Oid] uniqueidentifier  NOT NULL,
    [QuanLyNghiPhep] uniqueidentifier  NULL,
    [BoPhan] uniqueidentifier  NULL,
    [ThongTinNhanVien] uniqueidentifier  NULL,
    [SoNgayPhepCoBan] decimal(18,2)  NULL,
    [SoNgayPhepCongThem] decimal(18,2)  NULL,
    [SoNgayPhepDaNghi] decimal(18,2)  NULL,
    [SoNgayPhepConLai] decimal(18,2)  NULL,
    [GhiChu] nvarchar(max)  NULL,
    [TruNgayDiDuong] bit  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [PhepNamTruocConLai] decimal(18,2)  NULL,
    [PhepNamNayConLai] decimal(18,2)  NULL,
    [SoNgayPhepNamTruoc] decimal(19,4)  NULL,
    [SoNgayThanhToan] decimal(19,4)  NULL
);
GO

-- Creating table 'ThongTinNhanViens'
CREATE TABLE [dbo].[ThongTinNhanViens] (
    [Oid] uniqueidentifier  NOT NULL,
    [KhoaHoSo] bit  NULL,
    [BienChe] bit  NULL,
    [NgayNghiHuu] datetime  NULL,
    [ChucVu] uniqueidentifier  NULL,
    [LanBoNhiemChucVu] int  NULL,
    [Password] nvarchar(200)  NULL,
    [ChucVuKiemNhiem] uniqueidentifier  NULL,
    [NgayBoNhiemKiemNhiem] datetime  NULL,
    [LoaiLuongChinh] tinyint  NULL,
    [LoaiNhanVien] uniqueidentifier  NULL,
    [LoaiNhanSu] uniqueidentifier  NULL,
    [ThamGiaGiangDay] bit  NULL,
    [TaiBoMon] uniqueidentifier  NULL,
    [SoHieuCongChuc] nvarchar(100)  NULL,
    [SoHoSo] nvarchar(100)  NULL,
    [DienThoaiCoQuan] nvarchar(100)  NULL,
    [ChucVuCoQuanCaoNhat] uniqueidentifier  NULL,
    [NgayBoNhiem] datetime  NULL,
    [NgayVaoBienChe] datetime  NULL,
    [NgayTinhThamNienNhaGiao] datetime  NULL,
    [NhomMau] uniqueidentifier  NULL,
    [ChieuCao] int  NULL,
    [CanNang] int  NULL,
    [TinhTrangSucKhoe] uniqueidentifier  NULL,
    [ChamCong] bit  NULL,
    [NgayVaoDangChinhThuc] datetime  NULL,
    [NgayVaoDangDuBi] datetime  NULL,
    [HopDongLaoDong] bit  NULL,
    [HopDongKhoan] bit  NULL,
    [LamTheoCa] bit  NULL,
    [ChucVuDang] uniqueidentifier  NULL,
    [NgayBoNhiemDang] datetime  NULL,
    [NhomGiangVien] uniqueidentifier  NULL
);
GO

-- Creating table 'TinhThanhs'
CREATE TABLE [dbo].[TinhThanhs] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenTinhThanh] nvarchar(100)  NULL,
    [QuocGia] uniqueidentifier  NULL,
    [SoNgayDiDuong] decimal(19,4)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'TinhTrangs'
CREATE TABLE [dbo].[TinhTrangs] (
    [Oid] uniqueidentifier  NOT NULL,
    [MaQuanLy] nvarchar(100)  NULL,
    [TenTinhTrang] nvarchar(100)  NULL,
    [KhongConCongTacTaiTruong] bit  NULL,
    [KhongTinhTNTT] bit  NULL,
    [LoaiTinhTrang] tinyint  NULL,
    [PhanTramHuongLuong] decimal(19,4)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL
);
GO

-- Creating table 'Web_GiayChungNhan_AutoNumber'
CREATE TABLE [dbo].[Web_GiayChungNhan_AutoNumber] (
    [Id] uniqueidentifier  NOT NULL,
    [SoThuTuPhieu] bigint IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'WebMenus'
CREATE TABLE [dbo].[WebMenus] (
    [Oid] uniqueidentifier  NOT NULL,
    [Name] nvarchar(200)  NULL,
    [Url] nvarchar(max)  NULL,
    [ParentId] uniqueidentifier  NULL,
    [Global_idx] int  NULL,
    [Local_idx] int  NULL
);
GO

-- Creating table 'WebMenu_Role'
CREATE TABLE [dbo].[WebMenu_Role] (
    [WebMenuID] uniqueidentifier  NOT NULL,
    [WebGroupID] uniqueidentifier  NOT NULL,
    [Description] nvarchar(500)  NULL
);
GO

-- Creating table 'CauHinhXetABCs'
CREATE TABLE [dbo].[CauHinhXetABCs] (
    [Oid] uniqueidentifier  NOT NULL,
    [ThoiGian] int  NULL
);
GO

-- Creating table 'WebGroups'
CREATE TABLE [dbo].[WebGroups] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(200)  NULL
);
GO

-- Creating table 'WebUser_BoPhan'
CREATE TABLE [dbo].[WebUser_BoPhan] (
    [IDWebUser] uniqueidentifier  NOT NULL,
    [BoPhanID] uniqueidentifier  NOT NULL,
    [DienGiai] nvarchar(max)  NULL
);
GO

-- Creating table 'WebUsers'
CREATE TABLE [dbo].[WebUsers] (
    [Oid] uniqueidentifier  NOT NULL,
    [ThongTinNhanVien] uniqueidentifier  NULL,
    [UserName] nvarchar(100)  NULL,
    [Password] nvarchar(100)  NULL,
    [HoatDong] bit  NULL,
    [UserChamCong] bit  NULL,
    [WebGroupID] uniqueidentifier  NULL,
    [AdminEmail] nvarchar(50)  NULL,
    [OptimisticLockField] int  NULL,
    [GCRecord] int  NULL,
    [AgentObjectTypeId] int  NULL,
    [DepartmentId] uniqueidentifier  NULL
);
GO

-- Creating table 'CC_LyDoDangKyChamCongNgoaiGio'
CREATE TABLE [dbo].[CC_LyDoDangKyChamCongNgoaiGio] (
    [Oid] uniqueidentifier  NOT NULL,
    [LyDo] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Oid] in table 'BangChotThongTinTinhLuongs'
ALTER TABLE [dbo].[BangChotThongTinTinhLuongs]
ADD CONSTRAINT [PK_BangChotThongTinTinhLuongs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'BangLuongNhanViens'
ALTER TABLE [dbo].[BangLuongNhanViens]
ADD CONSTRAINT [PK_BangLuongNhanViens]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'BoPhans'
ALTER TABLE [dbo].[BoPhans]
ADD CONSTRAINT [PK_BoPhans]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CacBuoiTrongNgays'
ALTER TABLE [dbo].[CacBuoiTrongNgays]
ADD CONSTRAINT [PK_CacBuoiTrongNgays]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_CaChamCong'
ALTER TABLE [dbo].[CC_CaChamCong]
ADD CONSTRAINT [PK_CC_CaChamCong]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [PK_CC_ChamCongNgayNghi]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ChamCongTheoNgay'
ALTER TABLE [dbo].[CC_ChamCongTheoNgay]
ADD CONSTRAINT [PK_CC_ChamCongTheoNgay]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_DangKyChamCongNgoaiGio'
ALTER TABLE [dbo].[CC_DangKyChamCongNgoaiGio]
ADD CONSTRAINT [PK_CC_DangKyChamCongNgoaiGio]
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

-- Creating primary key on [Oid] in table 'CC_KhaiBaoChamCongGiangVien'
ALTER TABLE [dbo].[CC_KhaiBaoChamCongGiangVien]
ADD CONSTRAINT [PK_CC_KhaiBaoChamCongGiangVien]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_KhaiBaoCongTac'
ALTER TABLE [dbo].[CC_KhaiBaoCongTac]
ADD CONSTRAINT [PK_CC_KhaiBaoCongTac]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_KyDangKyKhungGio'
ALTER TABLE [dbo].[CC_KyDangKyKhungGio]
ADD CONSTRAINT [PK_CC_KyDangKyKhungGio]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_QuanLyViPham'
ALTER TABLE [dbo].[CC_QuanLyViPham]
ADD CONSTRAINT [PK_CC_QuanLyViPham]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_QuanLyViPham_New'
ALTER TABLE [dbo].[CC_QuanLyViPham_New]
ADD CONSTRAINT [PK_CC_QuanLyViPham_New]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_ThoiGianDangKyKhungGioLamViec'
ALTER TABLE [dbo].[CC_ThoiGianDangKyKhungGioLamViec]
ADD CONSTRAINT [PK_CC_ThoiGianDangKyKhungGioLamViec]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'ChiTietChamCongNhanViens'
ALTER TABLE [dbo].[ChiTietChamCongNhanViens]
ADD CONSTRAINT [PK_ChiTietChamCongNhanViens]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'ChiTietNghiPheps'
ALTER TABLE [dbo].[ChiTietNghiPheps]
ADD CONSTRAINT [PK_ChiTietNghiPheps]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'ChucDanhs'
ALTER TABLE [dbo].[ChucDanhs]
ADD CONSTRAINT [PK_ChucDanhs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'ChucVus'
ALTER TABLE [dbo].[ChucVus]
ADD CONSTRAINT [PK_ChucVus]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'DangLuuTrus'
ALTER TABLE [dbo].[DangLuuTrus]
ADD CONSTRAINT [PK_DangLuuTrus]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'GiayToes'
ALTER TABLE [dbo].[GiayToes]
ADD CONSTRAINT [PK_GiayToes]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'GiayToHoSoes'
ALTER TABLE [dbo].[GiayToHoSoes]
ADD CONSTRAINT [PK_GiayToHoSoes]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'HinhThucNghis'
ALTER TABLE [dbo].[HinhThucNghis]
ADD CONSTRAINT [PK_HinhThucNghis]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'HoSoes'
ALTER TABLE [dbo].[HoSoes]
ADD CONSTRAINT [PK_HoSoes]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'KyTinhLuongs'
ALTER TABLE [dbo].[KyTinhLuongs]
ADD CONSTRAINT [PK_KyTinhLuongs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'LoaiNhanSus'
ALTER TABLE [dbo].[LoaiNhanSus]
ADD CONSTRAINT [PK_LoaiNhanSus]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'NgayNghiTrongNams'
ALTER TABLE [dbo].[NgayNghiTrongNams]
ADD CONSTRAINT [PK_NgayNghiTrongNams]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [PK_NhanViens]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'QuanLyChamCongNhanViens'
ALTER TABLE [dbo].[QuanLyChamCongNhanViens]
ADD CONSTRAINT [PK_QuanLyChamCongNhanViens]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'QuanLyNgayNghiTrongNams'
ALTER TABLE [dbo].[QuanLyNgayNghiTrongNams]
ADD CONSTRAINT [PK_QuanLyNgayNghiTrongNams]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'QuanLyNghiPheps'
ALTER TABLE [dbo].[QuanLyNghiPheps]
ADD CONSTRAINT [PK_QuanLyNghiPheps]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'ThongTinNghiPheps'
ALTER TABLE [dbo].[ThongTinNghiPheps]
ADD CONSTRAINT [PK_ThongTinNghiPheps]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'ThongTinNhanViens'
ALTER TABLE [dbo].[ThongTinNhanViens]
ADD CONSTRAINT [PK_ThongTinNhanViens]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'TinhThanhs'
ALTER TABLE [dbo].[TinhThanhs]
ADD CONSTRAINT [PK_TinhThanhs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'TinhTrangs'
ALTER TABLE [dbo].[TinhTrangs]
ADD CONSTRAINT [PK_TinhTrangs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Id] in table 'Web_GiayChungNhan_AutoNumber'
ALTER TABLE [dbo].[Web_GiayChungNhan_AutoNumber]
ADD CONSTRAINT [PK_Web_GiayChungNhan_AutoNumber]
    PRIMARY KEY CLUSTERED ([Id] ASC);
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

-- Creating primary key on [Oid] in table 'CauHinhXetABCs'
ALTER TABLE [dbo].[CauHinhXetABCs]
ADD CONSTRAINT [PK_CauHinhXetABCs]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [ID] in table 'WebGroups'
ALTER TABLE [dbo].[WebGroups]
ADD CONSTRAINT [PK_WebGroups]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [IDWebUser], [BoPhanID] in table 'WebUser_BoPhan'
ALTER TABLE [dbo].[WebUser_BoPhan]
ADD CONSTRAINT [PK_WebUser_BoPhan]
    PRIMARY KEY CLUSTERED ([IDWebUser], [BoPhanID] ASC);
GO

-- Creating primary key on [Oid] in table 'WebUsers'
ALTER TABLE [dbo].[WebUsers]
ADD CONSTRAINT [PK_WebUsers]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- Creating primary key on [Oid] in table 'CC_LyDoDangKyChamCongNgoaiGio'
ALTER TABLE [dbo].[CC_LyDoDangKyChamCongNgoaiGio]
ADD CONSTRAINT [PK_CC_LyDoDangKyChamCongNgoaiGio]
    PRIMARY KEY CLUSTERED ([Oid] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [BangChotThongTinTinhLuong] in table 'KyTinhLuongs'
ALTER TABLE [dbo].[KyTinhLuongs]
ADD CONSTRAINT [FK_KyTinhLuong_BangChotThongTinTinhLuong]
    FOREIGN KEY ([BangChotThongTinTinhLuong])
    REFERENCES [dbo].[BangChotThongTinTinhLuongs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KyTinhLuong_BangChotThongTinTinhLuong'
CREATE INDEX [IX_FK_KyTinhLuong_BangChotThongTinTinhLuong]
ON [dbo].[KyTinhLuongs]
    ([BangChotThongTinTinhLuong]);
GO

-- Creating foreign key on [KyTinhLuong] in table 'BangLuongNhanViens'
ALTER TABLE [dbo].[BangLuongNhanViens]
ADD CONSTRAINT [FK_BangLuongNhanVien_KyTinhLuong]
    FOREIGN KEY ([KyTinhLuong])
    REFERENCES [dbo].[KyTinhLuongs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BangLuongNhanVien_KyTinhLuong'
CREATE INDEX [IX_FK_BangLuongNhanVien_KyTinhLuong]
ON [dbo].[BangLuongNhanViens]
    ([KyTinhLuong]);
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

-- Creating foreign key on [BoPhanChaOld] in table 'BoPhans'
ALTER TABLE [dbo].[BoPhans]
ADD CONSTRAINT [FK_BoPhan_BoPhanChaOld]
    FOREIGN KEY ([BoPhanChaOld])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BoPhan_BoPhanChaOld'
CREATE INDEX [IX_FK_BoPhan_BoPhanChaOld]
ON [dbo].[BoPhans]
    ([BoPhanChaOld]);
GO

-- Creating foreign key on [IDBoPhan] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_IDBoPhan]
    FOREIGN KEY ([IDBoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_IDBoPhan'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_IDBoPhan]
ON [dbo].[CC_ChamCongNgayNghi]
    ([IDBoPhan]);
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

-- Creating foreign key on [BoPhan] in table 'ChiTietChamCongNhanViens'
ALTER TABLE [dbo].[ChiTietChamCongNhanViens]
ADD CONSTRAINT [FK_ChiTietChamCongNhanVien_BoPhan]
    FOREIGN KEY ([BoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChiTietChamCongNhanVien_BoPhan'
CREATE INDEX [IX_FK_ChiTietChamCongNhanVien_BoPhan]
ON [dbo].[ChiTietChamCongNhanViens]
    ([BoPhan]);
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

-- Creating foreign key on [BoPhanCu] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NhanVien_BoPhanCu]
    FOREIGN KEY ([BoPhanCu])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhanVien_BoPhanCu'
CREATE INDEX [IX_FK_NhanVien_BoPhanCu]
ON [dbo].[NhanViens]
    ([BoPhanCu]);
GO

-- Creating foreign key on [BoPhanTinhLuong] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NhanVien_BoPhanTinhLuong]
    FOREIGN KEY ([BoPhanTinhLuong])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NhanVien_BoPhanTinhLuong'
CREATE INDEX [IX_FK_NhanVien_BoPhanTinhLuong]
ON [dbo].[NhanViens]
    ([BoPhanTinhLuong]);
GO

-- Creating foreign key on [BoPhan] in table 'ThongTinNghiPheps'
ALTER TABLE [dbo].[ThongTinNghiPheps]
ADD CONSTRAINT [FK_ThongTinNghiPhep_BoPhan]
    FOREIGN KEY ([BoPhan])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThongTinNghiPhep_BoPhan'
CREATE INDEX [IX_FK_ThongTinNghiPhep_BoPhan]
ON [dbo].[ThongTinNghiPheps]
    ([BoPhan]);
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

-- Creating foreign key on [CacBuoiTrongNgay_TuNgay] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_CacBuoiTrongNgay]
    FOREIGN KEY ([CacBuoiTrongNgay_TuNgay])
    REFERENCES [dbo].[CacBuoiTrongNgays]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_CacBuoiTrongNgay'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_CacBuoiTrongNgay]
ON [dbo].[CC_ChamCongNgayNghi]
    ([CacBuoiTrongNgay_TuNgay]);
GO

-- Creating foreign key on [CacBuoiTrongNgay_DenNgay] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_CacBuoiTrongNgay1]
    FOREIGN KEY ([CacBuoiTrongNgay_DenNgay])
    REFERENCES [dbo].[CacBuoiTrongNgays]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_CacBuoiTrongNgay1'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_CacBuoiTrongNgay1]
ON [dbo].[CC_ChamCongNgayNghi]
    ([CacBuoiTrongNgay_DenNgay]);
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

-- Creating foreign key on [HinhThucNghi] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_HinhThucNghi]
    FOREIGN KEY ([HinhThucNghi])
    REFERENCES [dbo].[HinhThucNghis]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_HinhThucNghi'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_HinhThucNghi]
ON [dbo].[CC_ChamCongNgayNghi]
    ([HinhThucNghi]);
GO

-- Creating foreign key on [IDHinhThucNghi] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_IDHinhThucNghi]
    FOREIGN KEY ([IDHinhThucNghi])
    REFERENCES [dbo].[HinhThucNghis]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_IDHinhThucNghi'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_IDHinhThucNghi]
ON [dbo].[CC_ChamCongNgayNghi]
    ([IDHinhThucNghi]);
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

-- Creating foreign key on [TinhThanh] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_TinhThanh]
    FOREIGN KEY ([TinhThanh])
    REFERENCES [dbo].[TinhThanhs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_TinhThanh'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_TinhThanh]
ON [dbo].[CC_ChamCongNgayNghi]
    ([TinhThanh]);
GO

-- Creating foreign key on [IDHinhThucNghi] in table 'CC_ChamCongTheoNgay'
ALTER TABLE [dbo].[CC_ChamCongTheoNgay]
ADD CONSTRAINT [FK_CC_ChamCongTheoNgay_HinhThucNghi]
    FOREIGN KEY ([IDHinhThucNghi])
    REFERENCES [dbo].[HinhThucNghis]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongTheoNgay_HinhThucNghi'
CREATE INDEX [IX_FK_CC_ChamCongTheoNgay_HinhThucNghi]
ON [dbo].[CC_ChamCongTheoNgay]
    ([IDHinhThucNghi]);
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

-- Creating foreign key on [ChamCongTheoNgay] in table 'CC_QuanLyViPham'
ALTER TABLE [dbo].[CC_QuanLyViPham]
ADD CONSTRAINT [FK_cc_QuanLyViPham_CC_ChamCongTheoNgay]
    FOREIGN KEY ([ChamCongTheoNgay])
    REFERENCES [dbo].[CC_ChamCongTheoNgay]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_cc_QuanLyViPham_CC_ChamCongTheoNgay'
CREATE INDEX [IX_FK_cc_QuanLyViPham_CC_ChamCongTheoNgay]
ON [dbo].[CC_QuanLyViPham]
    ([ChamCongTheoNgay]);
GO

-- Creating foreign key on [CC_ChamCongTheoNgay] in table 'CC_QuanLyViPham_New'
ALTER TABLE [dbo].[CC_QuanLyViPham_New]
ADD CONSTRAINT [FK_cc_QuanLyViPham_New_CC_ChamCongTheoNgay]
    FOREIGN KEY ([CC_ChamCongTheoNgay])
    REFERENCES [dbo].[CC_ChamCongTheoNgay]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_cc_QuanLyViPham_New_CC_ChamCongTheoNgay'
CREATE INDEX [IX_FK_cc_QuanLyViPham_New_CC_ChamCongTheoNgay]
ON [dbo].[CC_QuanLyViPham_New]
    ([CC_ChamCongTheoNgay]);
GO

-- Creating foreign key on [IDNhanVien] in table 'CC_DangKyChamCongNgoaiGio'
ALTER TABLE [dbo].[CC_DangKyChamCongNgoaiGio]
ADD CONSTRAINT [FK_CC_DangKyChamCongNgoaiGio_ThongTinNhanVien]
    FOREIGN KEY ([IDNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_DangKyChamCongNgoaiGio_ThongTinNhanVien'
CREATE INDEX [IX_FK_CC_DangKyChamCongNgoaiGio_ThongTinNhanVien]
ON [dbo].[CC_DangKyChamCongNgoaiGio]
    ([IDNhanVien]);
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

-- Creating foreign key on [IDNhanVien] in table 'CC_KhaiBaoChamCongGiangVien'
ALTER TABLE [dbo].[CC_KhaiBaoChamCongGiangVien]
ADD CONSTRAINT [FK_CC_KhaiBaoChamCongGiangVien_ThongTinNhanVien]
    FOREIGN KEY ([IDNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_KhaiBaoChamCongGiangVien_ThongTinNhanVien'
CREATE INDEX [IX_FK_CC_KhaiBaoChamCongGiangVien_ThongTinNhanVien]
ON [dbo].[CC_KhaiBaoChamCongGiangVien]
    ([IDNhanVien]);
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

-- Creating foreign key on [QuanLyChamCongNhanVien] in table 'ChiTietChamCongNhanViens'
ALTER TABLE [dbo].[ChiTietChamCongNhanViens]
ADD CONSTRAINT [FK_ChiTietChamCongNhanVien_QuanLyChamCongNhanVien]
    FOREIGN KEY ([QuanLyChamCongNhanVien])
    REFERENCES [dbo].[QuanLyChamCongNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChiTietChamCongNhanVien_QuanLyChamCongNhanVien'
CREATE INDEX [IX_FK_ChiTietChamCongNhanVien_QuanLyChamCongNhanVien]
ON [dbo].[ChiTietChamCongNhanViens]
    ([QuanLyChamCongNhanVien]);
GO

-- Creating foreign key on [ThongTinNhanVien] in table 'ChiTietChamCongNhanViens'
ALTER TABLE [dbo].[ChiTietChamCongNhanViens]
ADD CONSTRAINT [FK_ChiTietChamCongNhanVien_ThongTinNhanVien]
    FOREIGN KEY ([ThongTinNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChiTietChamCongNhanVien_ThongTinNhanVien'
CREATE INDEX [IX_FK_ChiTietChamCongNhanVien_ThongTinNhanVien]
ON [dbo].[ChiTietChamCongNhanViens]
    ([ThongTinNhanVien]);
GO

-- Creating foreign key on [ThongTinNghiPhep] in table 'ChiTietNghiPheps'
ALTER TABLE [dbo].[ChiTietNghiPheps]
ADD CONSTRAINT [FK_ChiTietNghiPhep_ThongTinNghiPhep]
    FOREIGN KEY ([ThongTinNghiPhep])
    REFERENCES [dbo].[ThongTinNghiPheps]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChiTietNghiPhep_ThongTinNghiPhep'
CREATE INDEX [IX_FK_ChiTietNghiPhep_ThongTinNghiPhep]
ON [dbo].[ChiTietNghiPheps]
    ([ThongTinNghiPhep]);
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

-- Creating foreign key on [ChucVuCoQuanCaoNhat] in table 'ThongTinNhanViens'
ALTER TABLE [dbo].[ThongTinNhanViens]
ADD CONSTRAINT [FK_ThongTinNhanVien_ChucVuCoQuanCaoNhat]
    FOREIGN KEY ([ChucVuCoQuanCaoNhat])
    REFERENCES [dbo].[ChucVus]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThongTinNhanVien_ChucVuCoQuanCaoNhat'
CREATE INDEX [IX_FK_ThongTinNhanVien_ChucVuCoQuanCaoNhat]
ON [dbo].[ThongTinNhanViens]
    ([ChucVuCoQuanCaoNhat]);
GO

-- Creating foreign key on [ChucVuKiemNhiem] in table 'ThongTinNhanViens'
ALTER TABLE [dbo].[ThongTinNhanViens]
ADD CONSTRAINT [FK_ThongTinNhanVien_ChucVuKiemNhiem]
    FOREIGN KEY ([ChucVuKiemNhiem])
    REFERENCES [dbo].[ChucVus]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThongTinNhanVien_ChucVuKiemNhiem'
CREATE INDEX [IX_FK_ThongTinNhanVien_ChucVuKiemNhiem]
ON [dbo].[ThongTinNhanViens]
    ([ChucVuKiemNhiem]);
GO

-- Creating foreign key on [DangLuuTru] in table 'GiayToHoSoes'
ALTER TABLE [dbo].[GiayToHoSoes]
ADD CONSTRAINT [FK_GiayToHoSo_DangLuuTru]
    FOREIGN KEY ([DangLuuTru])
    REFERENCES [dbo].[DangLuuTrus]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GiayToHoSo_DangLuuTru'
CREATE INDEX [IX_FK_GiayToHoSo_DangLuuTru]
ON [dbo].[GiayToHoSoes]
    ([DangLuuTru]);
GO

-- Creating foreign key on [GiayTo] in table 'GiayToHoSoes'
ALTER TABLE [dbo].[GiayToHoSoes]
ADD CONSTRAINT [FK_GiayToHoSo_GiayTo]
    FOREIGN KEY ([GiayTo])
    REFERENCES [dbo].[GiayToes]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GiayToHoSo_GiayTo'
CREATE INDEX [IX_FK_GiayToHoSo_GiayTo]
ON [dbo].[GiayToHoSoes]
    ([GiayTo]);
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

-- Creating foreign key on [NoiCap] in table 'HoSoes'
ALTER TABLE [dbo].[HoSoes]
ADD CONSTRAINT [FK_HoSo_NoiCap]
    FOREIGN KEY ([NoiCap])
    REFERENCES [dbo].[TinhThanhs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HoSo_NoiCap'
CREATE INDEX [IX_FK_HoSo_NoiCap]
ON [dbo].[HoSoes]
    ([NoiCap]);
GO

-- Creating foreign key on [Oid] in table 'NhanViens'
ALTER TABLE [dbo].[NhanViens]
ADD CONSTRAINT [FK_NhanVien_Oid]
    FOREIGN KEY ([Oid])
    REFERENCES [dbo].[HoSoes]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [KyTinhLuong] in table 'QuanLyChamCongNhanViens'
ALTER TABLE [dbo].[QuanLyChamCongNhanViens]
ADD CONSTRAINT [FK_QuanLyChamCongNhanVien_KyTinhLuong]
    FOREIGN KEY ([KyTinhLuong])
    REFERENCES [dbo].[KyTinhLuongs]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuanLyChamCongNhanVien_KyTinhLuong'
CREATE INDEX [IX_FK_QuanLyChamCongNhanVien_KyTinhLuong]
ON [dbo].[QuanLyChamCongNhanViens]
    ([KyTinhLuong]);
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

-- Creating foreign key on [QuanLyNgayNghiTrongNam] in table 'NgayNghiTrongNams'
ALTER TABLE [dbo].[NgayNghiTrongNams]
ADD CONSTRAINT [FK_NgayNghiTrongNam_QuanLyNgayNghiTrongNam]
    FOREIGN KEY ([QuanLyNgayNghiTrongNam])
    REFERENCES [dbo].[QuanLyNgayNghiTrongNams]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NgayNghiTrongNam_QuanLyNgayNghiTrongNam'
CREATE INDEX [IX_FK_NgayNghiTrongNam_QuanLyNgayNghiTrongNam]
ON [dbo].[NgayNghiTrongNams]
    ([QuanLyNgayNghiTrongNam]);
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

-- Creating foreign key on [QuanLyNghiPhep] in table 'ThongTinNghiPheps'
ALTER TABLE [dbo].[ThongTinNghiPheps]
ADD CONSTRAINT [FK_ThongTinNghiPhep_QuanLyNghiPhep]
    FOREIGN KEY ([QuanLyNghiPhep])
    REFERENCES [dbo].[QuanLyNghiPheps]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThongTinNghiPhep_QuanLyNghiPhep'
CREATE INDEX [IX_FK_ThongTinNghiPhep_QuanLyNghiPhep]
ON [dbo].[ThongTinNghiPheps]
    ([QuanLyNghiPhep]);
GO

-- Creating foreign key on [ThongTinNhanVien] in table 'ThongTinNghiPheps'
ALTER TABLE [dbo].[ThongTinNghiPheps]
ADD CONSTRAINT [FK_ThongTinNghiPhep_ThongTinNhanVien]
    FOREIGN KEY ([ThongTinNhanVien])
    REFERENCES [dbo].[ThongTinNhanViens]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThongTinNghiPhep_ThongTinNhanVien'
CREATE INDEX [IX_FK_ThongTinNghiPhep_ThongTinNhanVien]
ON [dbo].[ThongTinNghiPheps]
    ([ThongTinNhanVien]);
GO

-- Creating foreign key on [WebMenuID] in table 'WebMenu_Role'
ALTER TABLE [dbo].[WebMenu_Role]
ADD CONSTRAINT [FK_WebMenu_Role_WebMenu1]
    FOREIGN KEY ([WebMenuID])
    REFERENCES [dbo].[WebMenus]
        ([Oid])
    ON DELETE CASCADE ON UPDATE NO ACTION;
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

-- Creating foreign key on [BoPhanID] in table 'WebUser_BoPhan'
ALTER TABLE [dbo].[WebUser_BoPhan]
ADD CONSTRAINT [FK_WebUser_BoPhan_BoPhan]
    FOREIGN KEY ([BoPhanID])
    REFERENCES [dbo].[BoPhans]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUser_BoPhan_BoPhan'
CREATE INDEX [IX_FK_WebUser_BoPhan_BoPhan]
ON [dbo].[WebUser_BoPhan]
    ([BoPhanID]);
GO

-- Creating foreign key on [IDWebUsers] in table 'CC_ChamCongNgayNghi'
ALTER TABLE [dbo].[CC_ChamCongNgayNghi]
ADD CONSTRAINT [FK_CC_ChamCongNgayNghi_IDWebUsers]
    FOREIGN KEY ([IDWebUsers])
    REFERENCES [dbo].[WebUsers]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_ChamCongNgayNghi_IDWebUsers'
CREATE INDEX [IX_FK_CC_ChamCongNgayNghi_IDWebUsers]
ON [dbo].[CC_ChamCongNgayNghi]
    ([IDWebUsers]);
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

-- Creating foreign key on [WebGroupID] in table 'WebMenu_Role'
ALTER TABLE [dbo].[WebMenu_Role]
ADD CONSTRAINT [FK_WebMenu_Role_webRole]
    FOREIGN KEY ([WebGroupID])
    REFERENCES [dbo].[WebGroups]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebMenu_Role_webRole'
CREATE INDEX [IX_FK_WebMenu_Role_webRole]
ON [dbo].[WebMenu_Role]
    ([WebGroupID]);
GO

-- Creating foreign key on [WebGroupID] in table 'WebUsers'
ALTER TABLE [dbo].[WebUsers]
ADD CONSTRAINT [FK_WebUsers_WebGroup]
    FOREIGN KEY ([WebGroupID])
    REFERENCES [dbo].[WebGroups]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WebUsers_WebGroup'
CREATE INDEX [IX_FK_WebUsers_WebGroup]
ON [dbo].[WebUsers]
    ([WebGroupID]);
GO

-- Creating foreign key on [IDWebUser] in table 'WebUser_BoPhan'
ALTER TABLE [dbo].[WebUser_BoPhan]
ADD CONSTRAINT [FK_WebUser_BoPhan_WebUsers]
    FOREIGN KEY ([IDWebUser])
    REFERENCES [dbo].[WebUsers]
        ([Oid])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [NguoiTao] in table 'CC_DangKyChamCongNgoaiGio'
ALTER TABLE [dbo].[CC_DangKyChamCongNgoaiGio]
ADD CONSTRAINT [FK_CC_DangKyChamCongNgoaiGio_WebUsers]
    FOREIGN KEY ([NguoiTao])
    REFERENCES [dbo].[WebUsers]
        ([Oid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CC_DangKyChamCongNgoaiGio_WebUsers'
CREATE INDEX [IX_FK_CC_DangKyChamCongNgoaiGio_WebUsers]
ON [dbo].[CC_DangKyChamCongNgoaiGio]
    ([NguoiTao]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------