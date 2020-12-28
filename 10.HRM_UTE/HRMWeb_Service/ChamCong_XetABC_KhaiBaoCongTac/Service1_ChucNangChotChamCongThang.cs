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

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {

        public bool ChotChamCongThang_CheckExists(String publicKey, String token, Guid boPhanID, int thang, int nam, int loaicanbo)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_ChiTietChamCongNhanVien_Factory.New();
                bool daTonTai = factory.ExistsByThangNamBoPhanID(thang, nam, boPhanID,loaicanbo);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool ChotChamCongThang_CheckLock(String publicKey, String token, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                bool khoaCC = factory.CheckKhoaQLCCNV(thang, nam);
                return khoaCC;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool ChotChamCongThang_Create(String publicKey, String token, Guid boPhanID, int thang, int nam, Guid webUserId,int loaicanbo)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                var ctccnvFactory = CC_ChiTietChamCongNhanVien_Factory.New();
                //
                bool daTonTaiBangCon = ctccnvFactory.ExistsByThangNamBoPhanID(thang, nam, boPhanID,loaicanbo);
                //
                if (daTonTaiBangCon)
                {
                    throw new Exception(string.Format("Không thể chốt vì đã tồn tại bảng chốt của tháng {0} năm {1} của bộ phận", thang, nam));
                }
                else
                {
                    //try
                    //{
                    //tim ky tinh luong thich hop
                    KyTinhLuong kyTinhLuong = KyTinhLuong_Factory.New().GetObjByThangNam_GCRecordIsNull(thang, nam);
                    if (kyTinhLuong == null)
                        throw new Exception(string.Format("Kỳ tính lương tháng {0} năm {1} không tìm thấy", thang, nam));
                    else
                    {
                        //lay quan ly cu neu co
                        CC_QuanLyChamCongNhanVien quanLy = factory.GetByThangNam(thang, nam);
                        if (quanLy == null)
                        {
                            //tao quan ly moi
                            quanLy = factory.CreateManagedObject();
                            quanLy.Oid = Guid.NewGuid();
                            quanLy.KyTinhLuong = kyTinhLuong.Oid;
                            quanLy.NgayLap = DateTime.Today;
                            quanLy.ThongTinTruong = new Guid("E054A602-E077-444C-B843-E856D643CA7F");
                            quanLy.CreatedUser = webUserId;
                        }
                        
                        using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                            TimeSpan.FromSeconds(180)))
                        {
                            factory.SaveChanges();

                            factory.Context.spd_WebChamCong_ChotChamCongThang(thang,nam,quanLy.Oid,boPhanID,loaicanbo);

                            //Gộp lao động hợp đồng vào trang Quản lý chấm công
                            //nếu chốt ở trang Quản lý chấm công thì chốt luôn Lao động hợp đồng
                            if (loaicanbo == 1)
                            {
                                factory.Context.spd_WebChamCong_ChotChamCongThang(thang, nam, quanLy.Oid, boPhanID, 2);
                            }

                            transaction.Complete();
                        }
                        return true;
                    }

                    //}
                    //catch (Exception ex)
                    //{
                    //    return false;
                    //}
                }

                return false;
            }
            else
            {
                return false;
            }
        }
        public bool QuanLyChamCong_ChamCongThang_ChamNhanhCaNgay(String publicKey, String token, int thang, int nam, Guid boPhanID)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                Guid KhongXacDinh = new Guid("DAA9EA01-92F7-4EC8-928E-9065ACD156F5");
                var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                              TimeSpan.FromSeconds(180)))
                {
                    factory.SaveChanges();

                    factory.Context.spd_WebChamCong_UpdateLamCaNgayChoCaThangNeuHinhThucNghiLaKhongXacDinh(thang, nam, boPhanID, KhongXacDinh);
                    transaction.Complete();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool QuanLyChamCong_ChamCongThang_KhoaVaMoKhoa(String publicKey, String token, int thang, int nam, Guid boPhanID,int loaicanbo, bool loai)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongTheoNgay_Factory factory = new CC_ChamCongTheoNgay_Factory();
                try
                {
                    factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_KhoaVaMoKhoa(boPhanID, thang, nam, loaicanbo, loai);
                    return true;
                }
                catch { return true; }
              
            }
            else
            {
                return false;
            }
        }
        public bool ChotChamCongThang_Delete(String publicKey, String token, Guid boPhanID, int thang, int nam,int loaicanbo)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                bool daTonTai = factory.ExistsByThangNam(thang, nam);

                if (daTonTai)
                {
                    //tien hanh xoa bang chot
                    try
                    {
                        factory.Context.spd_WebChamCong_ChotChamCongThang_XoaChotCongThang(thang, nam, boPhanID, loaicanbo);
                        return true;
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format("Có lỗi khi xóa bảng chốt của tháng {0} năm {1}", thang, nam));
                    }

                }
                else
                {
                    throw new Exception(string.Format("Không thể xóa chốt vì không tồn tại bảng chốt của tháng {0} năm {1}", thang, nam));
                }

            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
    }
}
