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

        public bool ChotChamCongThang_CheckExists(String publicKey, String token, Guid boPhanID, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_ChiTietChamCongNhanVien_Factory.New();
                bool daTonTai = factory.ExistsByThangNamBoPhanID(thang, nam, boPhanID);
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
        public bool ChotChamCongThang_Create(String publicKey, String token, Guid boPhanID, int thang, int nam, Guid webUserId)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                var ctccnvFactory = CC_ChiTietChamCongNhanVien_Factory.New();
                //
                bool daTonTaiBangCon = ctccnvFactory.ExistsByThangNamBoPhanID(thang, nam, boPhanID);
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
                            quanLy.ThongTinTruong = new Guid("E26D029D-1FED-46DE-8DC7-0FAC6276E468");
                            quanLy.CreatedUser = webUserId;
                        }
                        

                        //thêm chi tiết
                        //lấy danh sách tất cả nhân viên
                        List<HoSo> hsListTheoBoPhan = HoSo_Factory.New().GetListByMaBoPhan_GCRecordIsNull(boPhanID).ToList();
                        foreach (var hoSo in hsListTheoBoPhan)
                        {
                            var chiTiet = ctccnvFactory.CreateAloneObject();
                            chiTiet.Oid = Guid.NewGuid();
                            chiTiet.ThongTinNhanVien = hoSo.Oid;
                            chiTiet.BoPhan = hoSo.NhanVien.BoPhan;
                            chiTiet.BoPhanTheoBangCong = hoSo.NhanVien.BoPhan1.TenBoPhan;
                            chiTiet.TrangThai = false;
                            chiTiet.DanhGia = "A";

                            quanLy.CC_ChiTietChamCongNhanVien.Add(chiTiet);

                        }
                        using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                            TimeSpan.FromSeconds(180)))
                        {
                            factory.SaveChanges();

                            factory.Context.spd_WebChamCong_ChotChamCongThang_Create(thang, nam,quanLy.Oid, boPhanID);
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

        public bool ChotChamCongThang_Delete(String publicKey, String token, Guid boPhanID, int thang, int nam)
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
                        factory.Context.spd_WebChamCong_ChotChamCongThang_Delete(thang, nam, boPhanID);
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
