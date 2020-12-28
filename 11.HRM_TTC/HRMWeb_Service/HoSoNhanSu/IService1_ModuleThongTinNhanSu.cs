using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using HRMWeb_Business.Model;

namespace HRMWeb_Service
{
    public partial interface IService1
    {
        //bang luong va phu cap theo ky
        [OperationContract]
        DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN ModuleThongTinNhanSu_BANGLUONGNHANVIEN(String publicKey, String token, Guid idNhanVien, Guid kyTinhLuong, Guid congTy);
        [OperationContract]
        String ModuleThongTinNhanSu_BANGLUONGNHANVIEN_Json(String publicKey, String token, Guid idNhanVien, Guid kyTinhLuong, Guid congTy);
   
        //so yeu ly lich
        [OperationContract]
        DTO_ModuleThongTinNhanSu_SoYeuLyLich ModuleThongTinNhanSu_SoYeuLyLich(String publicKey, String token, Guid idNhanVien);

        //trinh do chuyen mon
        [OperationContract]
        String ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_Json(String publicKey, String token, Guid idNhanVien);

        //hop dong lao dong
        [OperationContract]
        String ModuleThongTinNhanSu_HOPDONGLAODONG_Json(String publicKey, String token, Guid idNhanVien);


        //quan he gia dinh//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuanHeGiaDinh> ModuleThongTinNhanSu_QuanHeGiaDinh(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuanHeGiaDinh_Json(String publicKey, String token, Guid idNhanVien);


        //dien bien luong//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_DienBienLuong> ModuleThongTinNhanSu_DienBienLuong(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_DienBienLuong_Json(String publicKey, String token, Guid idNhanVien);


        //LichSuBanThan//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_LichSuBanThan> ModuleThongTinNhanSu_LichSuBanThan(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_LichSuBanThan_Json(String publicKey, String token, Guid idNhanVien);
        //qua trinh cong tac
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhCongTac> ModuleThongTinNhanSu_QuaTrinhCongTac(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhCongTac_Json(String publicKey, String token, Guid idNhanVien);
        //QuaTrinhDaoTao//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao> ModuleThongTinNhanSu_QuaTrinhDaoTao(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhDaoTao_Json(String publicKey, String token, Guid idNhanVien);

        //QuaTrinhBoiDuong//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDieuDong> ModuleThongTinNhanSu_QuaTrinhDieuDong(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhDieuDong_Json(String publicKey, String token, Guid idNhanVien);

        //QuaTrinhBoNhiem//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiem> ModuleThongTinNhanSu_QuaTrinhBoNhiem(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhBoNhiem_Json(String publicKey, String token, Guid idNhanVien);

        //QuaTrinhDiNuocNgoai//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai> ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_Json(String publicKey, String token, Guid idNhanVien);

        //QuaTrinhKhenThuong//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong> ModuleThongTinNhanSu_QuaTrinhKhenThuong(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhKhenThuong_Json(String publicKey, String token, Guid idNhanVien);

        //QuaTrinhKyLuat//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhKyLuat> ModuleThongTinNhanSu_QuaTrinhKyLuat(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhKyLuat_Json(String publicKey, String token, Guid idNhanVien);

        //QuaTrinhNghienCuuKhoaHoc//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc> ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_Json(String publicKey, String token, Guid idNhanVien);

        //QuaTrinhThamGiaHoatDongXaHoi//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi> ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_Json(String publicKey, String token, Guid idNhanVien);

        //QuaTrinhThamGiaHoiThao//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao> ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_Json(String publicKey, String token, Guid idNhanVien);

        //QuaTrinhThamGiaLucLuongVuTrang//////
        [OperationContract]
        IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang> ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_Json(String publicKey, String token, Guid idNhanVien);

        //DangVien//////
        [OperationContract]
        DTO_ModuleThongTinNhanSu_DangVien ModuleThongTinNhanSu_DangVien(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_DangVien_Json(String publicKey, String token, Guid idNhanVien);

        //DoanVien//////
        [OperationContract]
        DTO_ModuleThongTinNhanSu_DoanVien ModuleThongTinNhanSu_DoanVien(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_DoanVien_Json(String publicKey, String token, Guid idNhanVien);

        //CongDoan//////
        [OperationContract]
        DTO_ModuleThongTinNhanSu_CongDoan ModuleThongTinNhanSu_CongDoan(String publicKey,
            String token, Guid idNhanVien);
        [OperationContract]
        String ModuleThongTinNhanSu_CongDoan_Json(String publicKey, String token, Guid idNhanVien);
    }
}
