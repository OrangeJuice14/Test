using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Transactions;
using ERP_Core;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using HRMWeb_Business.Predefined;
using Newtonsoft.Json;
using System.Web.Configuration;
using ERP_Core.Common;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {

        public bool ChotChamCongThang_CheckExists(String publicKey, String token, Guid boPhanID, int thang, int nam, Guid congTy)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_ChiTietChamCongNhanVien_Factory.New();
                bool daTonTai = factory.ExistsByThangNamBoPhanID(thang, nam, boPhanID,congTy);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool ChotChamCongThang_CheckDoDuLieuChamCongByThangNam(String publicKey, String token, int thang, int nam, Guid boPhan, Guid congTy)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factoryBoPhan = BoPhan_Factory.New();
                //var boPhanObj = factoryBoPhan.GetByID(boPhan);
                var factoryKyChamCong = CC_KyChamCong_Factory.New();
                var kyChamCong = factoryKyChamCong.GetKyChamCong_ByThangNam(thang, nam, congTy);
                if (kyChamCong != null)
                {
                    var factory = CC_ChamCongTheoNgay_Factory.New();
                    bool daTonTai = factory.ExistsByTuNgayDenNgay(kyChamCong.TuNgay.Value, kyChamCong.DenNgay.Value, boPhan, congTy);
                    return daTonTai;
                }
                else throw new Exception("Không có kỳ chấm công tháng này");
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool ChotChamCongThang_CheckLock(String publicKey, String token, int thang, int nam, Guid congTy)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                bool khoaCC = factory.CheckKhoaQLCCNV(thang, nam,congTy);
                return khoaCC;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool ChotChamCongThang_Create(String publicKey, String token, Guid boPhanID, int thang, int nam, Guid webUserId, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //Lấy kỳ chấm công trước
                CC_KyChamCong_Factory factoryKyChamCong = CC_KyChamCong_Factory.New();
                CC_KyChamCong kyChamCong = factoryKyChamCong.GetKyChamCong_ByThangNam(thang, nam,congTy);
                if (kyChamCong == null) return false;

                // Tạo quản lý quản chấm công
                var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                CC_QuanLyChamCong quanLy = factory.GetByKyChamCong(kyChamCong.Oid,congTy);
                if (quanLy == null)
                {
                    //
                    quanLy = factory.CreateManagedObject();
                    quanLy.Oid = Guid.NewGuid();
                    quanLy.KyChamCong = kyChamCong.Oid;
                    quanLy.NgayLap = DateTime.Today;
                    quanLy.CongTy = congTy;
                }

                //
                var ctccnvFactory = CC_ChiTietChamCongNhanVien_Factory.New();       
                //Lấy danh sách tất cả nhân viên theo bộ phận
                List<DTO_DanhSachChotCong> hsListTheoBoPhan = CC_ChamCongTheoNgay_Factory.New().ChamCongTheoNgay_DanhSachChotCong(kyChamCong.TuNgay.Value,kyChamCong.DenNgay.Value, boPhanID,congTy).Distinct().ToList();
                foreach (var hoso in hsListTheoBoPhan)
                {
                    //Thêm chi tiết
                    if (!ctccnvFactory.ExistsByThangNamBoPhanID(thang, nam, hoso.IDBoPhan, congTy))
                    {
                        //
                        var chiTiet = ctccnvFactory.CreateAloneObject();
                        chiTiet.Oid = Guid.NewGuid();
                        chiTiet.ThongTinNhanVien = hoso.IDNhanVien;
                        chiTiet.BoPhan = hoso.IDBoPhan;
                        chiTiet.IsWeb = true;
                        chiTiet.Khoa = false;
                        //
                        quanLy.CC_ChiTietChamCong.Add(chiTiet);
                    }
                }

                try
                {
                    //Cập nhật số ngày
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                    {
                        factory.SaveChanges();
                        //
                        factory.Context.spd_WebChamCong_ChotChamCongCuoiThang(kyChamCong.TuNgay, kyChamCong.DenNgay, quanLy.Oid, boPhanID);
                        transaction.Complete();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            //
            return false;
        }
        public bool QuanLyChamCong_ChamCongThang_ChamNhanhCaNgay(String publicKey, String token, int thang, int nam, Guid boPhanID)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                              TimeSpan.FromSeconds(180)))
                {
                    factory.Context.spd_WebChamCong_CapNhatHinhThucNghiNeuLaKhongXacDinh(thang, nam, boPhanID);
                    transaction.Complete();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ChotChamCongThang_Delete(String publicKey, String token, Guid boPhanID, int thang, int nam, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_KyChamCong kyChamCong = new CC_KyChamCong_Factory().GetKyChamCong_ByThangNam(thang, nam,congTy);
                if (kyChamCong == null) return false;

                //Tien hanh xoa bang chot
                try
                {
                    //
                    var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                    //
                    factory.Context.spd_WebChamCong_XoaChotChamCongCuoiThang(kyChamCong.Oid, boPhanID);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
    }
}
