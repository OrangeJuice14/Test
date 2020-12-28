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
                var factory = QuanLyChamCongNhanVien_Factory.New();
                bool daTonTai = factory.ExistsByThangNamBoPhanID(thang, nam, boPhanID);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool ChotChamCongThangDonVi_CheckExists(String publicKey, String token, Guid boPhanID, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = QuanLyChamCongNhanVien_Factory.New();
                bool daTonTai = factory.ExistsByThangNamBoPhanIDDonVi(thang, nam, boPhanID);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String KiemTraPhongBanChot_Find_Json(String publicKey, String token, int thang, int nam, Boolean? daXetXongAbc)
        {//DANG SD
            IEnumerable<DTO_KiemTraPhongBanXetABC> list = KiemTraPhongBanChot_Find(publicKey, token, thang, nam, daXetXongAbc);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_KiemTraPhongBanXetABC> KiemTraPhongBanChot_Find(String publicKey, String token, int thang, int nam, Boolean? daXetXongAbc)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                IEnumerable<DTO_KiemTraPhongBanXetABC> query = BoPhan_Factory.New()
                    .KiemTraPhongBanChot_Find(thang, nam, daXetXongAbc);

                return query;
            }
            else
            {
                return null;
            }
        }

        public bool ChotChamCongThang_Create(String publicKey, String token, Guid boPhanID, int thang, int nam, Guid webUserId)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = QuanLyChamCongNhanVien_Factory.New();
                //bool daTonTaiBangCha = factory.ExistsByThangNam(thang, nam);
                bool daTonTaiBangCon = false; factory.ExistsByThangNamBoPhanID(thang, nam, boPhanID);
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
                        CC_KyChamCong kyChamCong = CC_KyChamCong_Factory.New().GetByMonthAndYear(thang, nam);
                        if (kyTinhLuong == null)
                            throw new Exception(string.Format("Kỳ tính lương tháng {0} năm {1} không tìm thấy", thang, nam));
                        else
                        if (kyChamCong==null)
                            throw new Exception(string.Format("Kỳ chấm công tháng {0} năm {1} không tìm thấy", thang, nam));
                        else
                        {
                            //lay quan ly cu neu co
                            QuanLyChamCongNhanVien quanLy = factory.GetByThangNam(thang, nam);
                            if (quanLy == null)
                            {
                                //tao quan ly moi
                                quanLy = factory.CreateManagedObject();
                                quanLy.Oid = Guid.NewGuid();
                                quanLy.KyTinhLuong = kyTinhLuong.Oid;
                                quanLy.KyChamCong = kyChamCong.Oid;
                                quanLy.NgayLap = DateTime.Today;
                            //quanLy.ThongTinTruong = new Guid("4095451C-79CD-4A65-BAD8-432C3D52218A");
                            quanLy.ThongTinTruong = new Guid("77FC97D9-6EE2-410A-B730-0444FE7AF7AE");
                            quanLy.CreatedUser = webUserId;
                            }
                            var ctccnvFactory = ChiTietChamCongNhanVien_Factory.New();

                        //thêm chi tiết
                        //lấy danh sách tất cả nhân viên
                        //List<HoSo> hsListTheoBoPhan = HoSo_Factory.New().GetListByMaBoPhan_GCRecordIsNull(boPhanID, kyTinhLuong.DenNgay.Value).ToList();

                        //Chốt tất cả đơn vị
                        if (boPhanID==Guid.Empty)
                        {
                            var facbp = BoPhan_Factory.New();
                            var bophanlist = facbp.GetAll_GCRecordIsNull();
                            foreach (BoPhan bp in bophanlist)
                            {
                                List<HoSo> hsListTheoBoPhan = HoSo_Factory.New().GetListByMaBoPhan_GCRecordIsNull(bp.Oid, kyChamCong.DenNgay).ToList();
                                //lay danh sach nhan vien có trong quyết định thôi việc và quyết định nghỉ hưu
                                List<HoSo> hsListTheoBoPhan2 = HoSo_Factory.New().GetListByMaBoPhan_NhanVienThoiViec(bp.Oid,kyChamCong.TuNgay, kyChamCong.DenNgay).ToList();
                                var listNV = hsListTheoBoPhan.Union(hsListTheoBoPhan2);
                                foreach (var hoSo in listNV)
                                {
                                    ChiTietChamCongNhanVien chiTiet = ctccnvFactory.GetChiTietChamCongNhanVienByNhanVien(quanLy.Oid, hoSo.Oid, bp.Oid);
                                    if (chiTiet == null)
                                    {
                                        chiTiet = ctccnvFactory.CreateAloneObject();
                                        chiTiet.Oid = Guid.NewGuid();
                                        chiTiet.ThongTinNhanVien = hoSo.Oid;
                                        chiTiet.BoPhan = hoSo.NhanVien.BoPhan;
                                        chiTiet.BoPhanTheoBangCong = hoSo.NhanVien.BoPhan1.TenBoPhan;
                                        chiTiet.TrangThai = false;
                                        chiTiet.DanhGia = "A";
                                        chiTiet.Khoa = false;

                                        quanLy.ChiTietChamCongNhanViens.Add(chiTiet);
                                    }

                                }
                                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                                    TimeSpan.FromSeconds(180)))
                                {
                                    factory.SaveChanges();

                                    factory.Context.spd_WebChamCong_ChotChamCongThang_CapNhatSoNgayCongNgayNghi(thang, nam,
                                        quanLy.Oid, boPhanID);
                                    transaction.Complete();
                                }

                            }
                        }
                        //Chốt 1 đơn vị
                        else
                        {
                            List<HoSo> hsListTheoBoPhan = HoSo_Factory.New().GetListByMaBoPhan_GCRecordIsNull(boPhanID, kyChamCong.DenNgay).ToList();
                            //lay danh sach nhan vien có trong quyết định thôi việc và quyết định nghỉ hưu
                            List<HoSo> hsListTheoBoPhan2 = HoSo_Factory.New().GetListByMaBoPhan_NhanVienThoiViec(boPhanID, kyChamCong.TuNgay, kyChamCong.DenNgay).ToList();
                            var listNV = hsListTheoBoPhan.Union(hsListTheoBoPhan2);
                            foreach (var hoSo in listNV)
                            {
                                ChiTietChamCongNhanVien chiTiet = ctccnvFactory.GetChiTietChamCongNhanVienByNhanVien(quanLy.Oid, hoSo.Oid, boPhanID);
                                if (chiTiet == null)
                                {
                                    chiTiet = ctccnvFactory.CreateAloneObject();
                                    chiTiet.Oid = Guid.NewGuid();
                                    chiTiet.ThongTinNhanVien = hoSo.Oid;
                                    chiTiet.BoPhan = hoSo.NhanVien.BoPhan;
                                    chiTiet.BoPhanTheoBangCong = hoSo.NhanVien.BoPhan1.TenBoPhan;
                                    chiTiet.TrangThai = false;
                                    chiTiet.DanhGia = "A";
                                    chiTiet.Khoa = false;

                                    quanLy.ChiTietChamCongNhanViens.Add(chiTiet);
                                }

                            }
                            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                                TimeSpan.FromSeconds(180)))
                            {
                                factory.SaveChanges();

                                factory.Context.spd_WebChamCong_ChotChamCongThang_CapNhatSoNgayCongNgayNghi(thang, nam,
                                    quanLy.Oid, boPhanID);
                                transaction.Complete();
                            }
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
        public bool ChotChamCongThangDonVi_Create(String publicKey, String token, Guid boPhanID, int thang, int nam, Guid webUserId)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = QuanLyChamCongNhanVien_Factory.New();
                //bool daTonTaiBangCha = factory.ExistsByThangNam(thang, nam);
                bool daTonTaiBangCon = false; factory.ExistsByThangNamBoPhanID(thang, nam, boPhanID);
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
                    CC_KyChamCong kyChamCong = CC_KyChamCong_Factory.New().GetByMonthAndYear(thang, nam);
                    if (kyTinhLuong == null)
                        throw new Exception(string.Format("Kỳ tính lương tháng {0} năm {1} không tìm thấy", thang, nam));
                    else
                    if (kyChamCong == null)
                        throw new Exception(string.Format("Kỳ chấm công tháng {0} năm {1} không tìm thấy", thang, nam));
                    else
                    {
                        //lay quan ly cu neu co
                        QuanLyChamCongNhanVien quanLy = factory.GetByThangNam(thang, nam);
                        if (quanLy == null)
                        {
                            //tao quan ly moi
                            quanLy = factory.CreateManagedObject();
                            quanLy.Oid = Guid.NewGuid();
                            quanLy.KyTinhLuong = kyTinhLuong.Oid;
                            quanLy.KyChamCong = kyChamCong.Oid;
                            quanLy.NgayLap = DateTime.Today;
                            //quanLy.ThongTinTruong = new Guid("4095451C-79CD-4A65-BAD8-432C3D52218A");
                            quanLy.ThongTinTruong = new Guid("77FC97D9-6EE2-410A-B730-0444FE7AF7AE");
                            quanLy.CreatedUser = webUserId;
                        }
                        var ctccnvFactory = ChiTietChamCongNhanVienDonVi_Factory.New();

                        //thêm chi tiết
                        //lấy danh sách tất cả nhân viên
                        //List<HoSo> hsListTheoBoPhan = HoSo_Factory.New().GetListByMaBoPhan_GCRecordIsNull(boPhanID, kyTinhLuong.DenNgay.Value).ToList();
                        List<HoSo> hsListTheoBoPhan = HoSo_Factory.New().GetListByMaBoPhan_GCRecordIsNull(boPhanID, kyChamCong.DenNgay).ToList();
                        //lay danh sach nhan vien có trong quyết định thôi việc và quyết định nghỉ hưu
                        List<HoSo> hsListTheoBoPhan2 = HoSo_Factory.New().GetListByMaBoPhan_NhanVienThoiViec(boPhanID, kyChamCong.TuNgay, kyChamCong.DenNgay).ToList();
                        var listNV = hsListTheoBoPhan.Union(hsListTheoBoPhan2);
                        foreach (var hoSo in listNV)
                        {
                            CC_ChiTietChamCongNhanVien_DonVi chiTiet = ctccnvFactory.GetChiTietChamCongNhanVienByNhanVien(quanLy.Oid, hoSo.Oid, boPhanID);
                            if (chiTiet == null)
                            {
                                chiTiet = ctccnvFactory.CreateAloneObject();
                                chiTiet.Oid = Guid.NewGuid();
                                chiTiet.ThongTinNhanVien = hoSo.Oid;
                                chiTiet.BoPhan = hoSo.NhanVien.BoPhan;
                                chiTiet.BoPhanTheoBangCong = hoSo.NhanVien.BoPhan1.TenBoPhan;
                                chiTiet.TrangThai = false;
                                chiTiet.DanhGia = "A";
                                chiTiet.Khoa = false;
                                quanLy.CC_ChiTietChamCongNhanVien_DonVi.Add(chiTiet);
                            }

                        }
                        using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                            TimeSpan.FromSeconds(180)))
                        {
                            factory.SaveChanges();

                            factory.Context.spd_WebChamCong_ChotChamCongThang_CapNhatSoNgayCongNgayNghi_DonViChot(thang, nam,
                                quanLy.Oid, boPhanID);
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
        public bool ChotChamCongThang_Delete(String publicKey, String token, Guid boPhanID, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = QuanLyChamCongNhanVien_Factory.New();
                bool daTonTai = factory.ExistsByThangNam(thang, nam);
                if (daTonTai)
                {
                    //tien hanh xoa bang chot
                    //try
                    //{

                    if (boPhanID==Guid.Empty)
                    {
                        var facbp = BoPhan_Factory.New();
                        var bophanlist = facbp.GetAll_GCRecordIsNull();
                        foreach (BoPhan bp in bophanlist)
                        {
                            factory.Context.spd_WebChamCong_ChotChamCongThang_Delete(thang, nam, bp.Oid);
                        }
                    }
                    else
                    {
                        factory.Context.spd_WebChamCong_ChotChamCongThang_Delete(thang, nam, boPhanID);
                    }
                    
                        
                        return true;
                    //}
                    //catch (Exception)
                    //{
                    //    throw new Exception(string.Format("Có lỗi khi xóa bảng chốt của tháng {0} năm {1}", thang, nam));
                    //}

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
        public bool ChotChamCongThangDonVi_Delete(String publicKey, String token, Guid boPhanID, int thang, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = QuanLyChamCongNhanVien_Factory.New();
                bool daTonTai = factory.ExistsByThangNam(thang, nam);
                if (daTonTai)
                {
                    //tien hanh xoa bang chot
                    //try
                    //{
                    factory.Context.spd_WebChamCong_ChotChamCongThangDonVi_Delete(thang, nam, boPhanID);
                    return true;
                    //}
                    //catch (Exception)
                    //{
                    //    throw new Exception(string.Format("Có lỗi khi xóa bảng chốt của tháng {0} năm {1}", thang, nam));
                    //}

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
