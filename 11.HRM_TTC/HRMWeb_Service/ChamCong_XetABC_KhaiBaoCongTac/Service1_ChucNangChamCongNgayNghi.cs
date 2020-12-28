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
using Newtonsoft.Json;
using System.Web.Configuration;
using HRMWeb_Business.Predefined;
using ERP_Core.Common;
using System.Net;
using System.Net.Mail;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        public String ChamCongNgayNghi_Report_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_ChamCongNgayNghi_Find obj = ChamCongNgayNghi_Report(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_Report(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                DTO_ChamCongNgayNghi_Find obj = null;
                obj = factory.ChamCongNgayNghi_Report(id);
                obj.TuNgayString = String.Format("{0:dd/MM/yyyy}", obj.TuNgay);
                obj.DenNgayString = String.Format("{0:dd/MM/yyyy}", obj.DenNgay);
                obj.NgaySinhString = String.Format("{0:dd/MM/yyyy}", obj.NgaySinh);
                return obj;
            }
            else
            {
                return null;
            }
        }

        public DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_GetByID(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                DTO_ChamCongNgayNghi_Find obj = null;
                obj = factory.GetDTO_ChamCongNgayNghi_Find_ByID(id);
                return obj;
            }
            else
            {
                return null;
            }
        }
        public String ChamCongNgayNghi_GetByID_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_ChamCongNgayNghi_Find obj = ChamCongNgayNghi_GetByID(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public decimal ChamCongNgayNghi_SoNgayHopLe(String publicKey, String token, Guid idHinhThucNghi, Guid idNhanVien, Guid congTy, DateTime tuNgay)
        {
            decimal result = 0;
            if (Helper.TrustTest(publicKey, token))
            {
                //
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                //
                result = factory.ChamCongNgayNghi_SoNgayHopLe(idHinhThucNghi, idNhanVien, congTy, tuNgay);
                //
            }
            //
            return result;
        }

        public IEnumerable<DTO_ChamCongNgayNghi_Find> ChamCongNgayNghi_Find(String publicKey, String token, DateTime tungay, DateTime denngay, string manhansu, Guid idBoPhan, Guid idLoaiNhanSu, int? trangthai, Guid idWebUser, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IQueryable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.FindForChamCongNgayNghi(tungay, denngay, manhansu, idBoPhan, idLoaiNhanSu, trangthai, idWebUser, congTy);
                //
                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_ChamCongNgayNghi_Find> QuanLyNghiPhep_Find(String publicKey, String token, int thang, int nam, string maNhanSu, Guid idBoPhan, Guid idLoaiNhanSu)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.QuanLyNghiPhep_Find(thang, nam, maNhanSu, idBoPhan, idLoaiNhanSu);
                return list;
            }
            else
            {
                return null;
            }
        }
        public String ChamCongNgayNghi_SoNgayHopLe_Json(String publicKey, String token, Guid idHinhThucNghi, Guid idNhanVien, Guid congTy, DateTime tuNgay)
        {
            decimal soNgayHopLe = ChamCongNgayNghi_SoNgayHopLe(publicKey, token, idHinhThucNghi, idNhanVien, congTy, tuNgay);
            String json = JsonConvert.SerializeObject(soNgayHopLe);
            //
            return json;
        }
        public String ChamCongNgayNghi_Find_Json(String publicKey, String token, DateTime tungay, DateTime denngay, string manhansu, Guid idBoPhan, Guid idLoaiNhanSu, int? trangthai, Guid idWebUser, Guid congTy)
        {
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = ChamCongNgayNghi_Find(publicKey, token, tungay, denngay, manhansu, idBoPhan, idLoaiNhanSu, trangthai, idWebUser,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String QuanLyNghiPhep_Find_Json(String publicKey, String token, int thang, int nam, string maNhanSu, Guid idBoPhan, Guid idLoaiNhanSu)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = QuanLyNghiPhep_Find(publicKey, token, thang, nam, maNhanSu, idBoPhan, idLoaiNhanSu);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String DangKyNghiPhep_Find_Json(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = DangKyNghiPhep_Find(publicKey, token, thang, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String DangKyNghiPhep_SoNgayPhepConLai_Json(String publicKey, String token, int nam, Guid idNhanVien)
        {
            decimal soNgayPhepConLai = DangKyNghiPhep_SoNgayPhepConLai(publicKey, token, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(soNgayPhepConLai);
            return json;
        }
        public decimal DangKyNghiPhep_SoNgayPhepConLai(String publicKey, String token, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                decimal soNgayPhepConLai = 0;//factory.DangKyNghiPhep_SoNgayPhepConLai(nam, idNhanVien);
                return soNgayPhepConLai;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable<DTO_ChamCongNgayNghi_Find> DangKyNghiPhep_Find(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.DangKyNghiPhep_Find(thang, nam, idNhanVien);
                return list;
            }
            else
            {
                return null;
            }
        }

        public bool ChamCongNgayNghi_AcceptList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList, Guid userId, Guid congTy)
        {
            try
            {
                if (Helper.TrustTest(publicKey, token))
                {
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                        TimeSpan.FromSeconds(360)))
                    {
                        //Lấy nhóm quyền
                        string idAdminToanQuyen = WebGroupConst.AdminId.ToString().ToUpper();
                        string idQuanTriKhoi = WebGroupConst.QuanTriKhoiID.ToString().ToUpper();
                        string idQuanTriTruong = WebGroupConst.QuanTriTruongID.ToString().ToUpper();
                        string idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID.ToString().ToUpper();
                        string idHoiDongQuanTriUyQuyen = WebGroupConst.HoiDongQuanTriUyQuyenID.ToString().ToUpper();
                        string idHieuTruong = WebGroupConst.HieuTruongID.ToString().ToUpper();
                        string idHieuTruongUyQuyen = WebGroupConst.HieuTruongUyQuyenID.ToString().ToUpper();
                        string idTruongPhong = WebGroupConst.TruongPhongID.ToString().ToUpper();
                        string idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID.ToString().ToUpper();
                        //Lấy cấu hình chấm công
                        int soNgayHieuTruongDuyet = 3;
                        DTO_CC_CauHinhChamCong cauHinh = (new CC_CauHinhChamCong_Factory()).GetCauHinhChamCongByCongTy(congTy);
                        if (cauHinh != null)
                        {
                            soNgayHieuTruongDuyet = cauHinh.SoNgayHieuTruongDuyet;
                        }
                        //
                        WebUser user = (new WebUser_Factory()).GetByID(userId);
                        if (user == null) return false;

                        //lấy chức vụ kiêm nhiệm
                        List<HRMWeb_Business.Model.DTO.ChucNang.ChamCong.DTO_WebGroup_BoPhan> listWebGroup_BoPhan = WebUser_BoPhan_Factory.New().GetAllWebGroupByUser(userId).ToList();

                        //
                        CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                        foreach (var obj in objList)
                        {
                            if (obj != null)
                            {
                                CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                                if (objFromDB != null)
                                {
                                    foreach (var item in listWebGroup_BoPhan)
                                    {
                                        if (item.BoPhanIds.Contains(objFromDB.IDBoPhan.Value))
                                        {
                                            //Admin
                                            if (idQuanTriTruong.Equals(item.WebGroupId.ToString().ToUpper()) 
                                                || idAdminToanQuyen.Equals(item.WebGroupId.ToString().ToUpper())
                                                || idQuanTriKhoi.Equals(item.WebGroupId.ToString().ToUpper()))
                                            {
                                                objFromDB.TrangThai_Admin = 1;

                                                //Gửi mail cho người nghỉ
                                                ChamCongNgayNghi_GuiMailChoNguoiDangKy(objFromDB, cauHinh, objFromDB.ThongTinNhanVien?.NhanVien?.HoSo?.Email, user.Oid);

                                                if (!objFromDB.IsBanGiamHieu.Value)
                                                {
                                                    if (objFromDB.SoNgay <= soNgayHieuTruongDuyet)
                                                    {
                                                        //
                                                        objFromDB.TrangThai_TP = 1;
                                                        objFromDB.WebUserDuyet_TP = userId;
                                                    }
                                                    if (objFromDB.SoNgay > soNgayHieuTruongDuyet)
                                                    {
                                                        //
                                                        objFromDB.TrangThai_TP = 1;
                                                        objFromDB.TrangThai_HT = 1;
                                                        objFromDB.WebUserDuyet_TP = userId;
                                                        objFromDB.WebUserDuyet_HT = userId;
                                                    }
                                                }
                                                if (objFromDB.IsBanGiamHieu.Value)
                                                {
                                                    objFromDB.TrangThai_HT = 1;
                                                    objFromDB.TrangThai_HDQT = 1;
                                                    objFromDB.WebUserDuyet_HT = userId;
                                                    objFromDB.WebUserDuyet_HDQT = userId;
                                                }
                                            }
                                            //Hội đồng quản trị
                                            if (idHoiDongQuanTriUyQuyen.Equals(item.WebGroupId.ToString().ToUpper())
                                                || idHoiDongQuanTri.Equals(item.WebGroupId.ToString().ToUpper()))
                                            {
                                                objFromDB.TrangThai_HDQT = 1;
                                                objFromDB.WebUserDuyet_HDQT = userId;

                                                //Gửi mail cho người nghỉ
                                                ChamCongNgayNghi_GuiMailChoNguoiDangKy(objFromDB, cauHinh, objFromDB.ThongTinNhanVien?.NhanVien?.HoSo?.Email, user.Oid);
                                            }
                                            //Hiệu trưởng
                                            if (idHieuTruong.Equals(item.WebGroupId.ToString().ToUpper())
                                                     || idHieuTruongUyQuyen.Equals(item.WebGroupId.ToString().ToUpper()))
                                            {
                                                objFromDB.TrangThai_HT = 1;
                                                objFromDB.WebUserDuyet_HT = userId;

                                                //Gửi mail cho người nghỉ
                                                ChamCongNgayNghi_GuiMailChoNguoiDangKy(objFromDB, cauHinh, objFromDB.ThongTinNhanVien?.NhanVien?.HoSo?.Email, user.Oid);

                                                //
                                                if (user != null && objFromDB.IsBanGiamHieu.Value)
                                                {
                                                    //Gửi đến hội đồng quản trị
                                                    ChamCongNgayNghi_SetDataSendMail(objFromDB, cauHinh, user.EmailHDQT, user.Oid);
                                                }
                                            }
                                            //Trưởng phòng
                                            if (idTruongPhong.Equals(item.WebGroupId.ToString().ToUpper())
                                                     || idTruongPhongUyQuyen.Equals(item.WebGroupId.ToString().ToUpper()))
                                            {
                                                objFromDB.TrangThai_TP = 1;
                                                objFromDB.WebUserDuyet_TP = userId;
                                                //Gửi mail cho người nghỉ
                                                ChamCongNgayNghi_GuiMailChoNguoiDangKy(objFromDB, cauHinh, objFromDB.ThongTinNhanVien?.NhanVien?.HoSo?.Email, user.Oid);

                                                //
                                                if (user != null && !objFromDB.IsBanGiamHieu.Value)
                                                {
                                                    if (!objFromDB.IsTruongPhong.Value && objFromDB.SoNgay > soNgayHieuTruongDuyet)
                                                    {
                                                        //Gửi đến hiệu trưởng
                                                        ChamCongNgayNghi_SetDataSendMail(objFromDB, cauHinh, user.EmailHT, user.Oid);
                                                    }
                                                    if (objFromDB.IsTruongPhong.Value)
                                                    {
                                                        //Gửi đến hiệu trưởng
                                                        ChamCongNgayNghi_SetDataSendMail(objFromDB, cauHinh, user.EmailHT, user.Oid);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    ///////////////// Kiểm tra xem có cập nhật bảng công hay ?////////////
                                    bool capNhat = false;

                                    // 1. Không phải ban giám hiệu
                                    if (!objFromDB.IsBanGiamHieu.Value)
                                    {
                                        // Cá nhân bình thường
                                        if (!objFromDB.IsTruongPhong.Value)
                                        {
                                            if (objFromDB.SoNgay <= soNgayHieuTruongDuyet)
                                            {
                                                if (objFromDB.TrangThai_TP == 1)
                                                {
                                                    capNhat = true;
                                                }
                                            }
                                            else
                                            {
                                                if (objFromDB.TrangThai_TP == 1 && objFromDB.TrangThai_HT == 1)
                                                {
                                                    capNhat = true;
                                                }
                                            }
                                        }
                                        else // Trưởng phòng
                                        {
                                            if (objFromDB.TrangThai_HT == 1)
                                            {
                                                capNhat = true;
                                            }
                                        }
                                    }
                                    else // Ban giám hiệu
                                    {
                                        if (objFromDB.TrangThai_HDQT == 1)
                                        {
                                            capNhat = true;
                                        }
                                    }

                                    //Cập nhật bảng công
                                    if (capNhat || objFromDB.TrangThai_Admin == 1)
                                    {
                                        //Cập nhật chấm công ngày nghỉ
                                        factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, objFromDB.Buoi, true);
                                    }
                                }
                            }
                        }
                        factory.SaveChangesWithoutTransactionScope();
                        //
                        foreach (var obj in objList)
                        {
                            CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                            if (objFromDB != null)
                            {
                                if (objFromDB != null && objFromDB.IDHinhThucNghi.Equals(HinhThucNghiConst.NghiPhepId))
                                {
                                    //Cập nhật số ngày phép năm
                                    //1. Lấy niên độ tài chính hiện tại
                                    DTO_NienDoTaiChinh nienDoHienTai = (new NienDoTaiChinh_Factory()).GetListByNam(objFromDB.TuNgay.Value.Year, objFromDB.ThongTinNhanVien.NhanVien.CongTy.Value);
                                    if (nienDoHienTai != null)
                                    {
                                        //2. Lấy quản lý theo niên độ tài chính
                                        CC_QuanLyNghiPhep quanLy = CC_QuanLyNghiPhep_Factory.New().GetByNienDoTaiChinh(nienDoHienTai.Oid, objFromDB.ThongTinNhanVien.NhanVien.CongTy.Value); // Lưu ý
                                        if (quanLy != null)
                                        {
                                            //
                                            factory.Context.spd_WebChamCong_TinhSoNgayPhepConLaiTrongNam(quanLy.Oid, obj.IDNhanVien, objFromDB.ThongTinNhanVien.NhanVien.CongTy.Value); // Lưu ý
                                        }
                                    }
                                }
                            }
                        }
                        factory.SaveChangesWithoutTransactionScope();
                        //
                        transaction.Complete();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Service1_ChucNangChamCongNgayNghi/ChamCongNgayNghi_AcceptList", ex);
                throw ex;
            }
        }

        public bool ChamCongNgayNghi_CancelList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList, Guid userId, Guid congTy)
        {
            try
            {
                if (Helper.TrustTest(publicKey, token))
                {
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                        TimeSpan.FromSeconds(360)))
                    {
                        //
                        CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();

                        //Lấy nhóm quyền
                        string idAdminToanQuyen = WebGroupConst.AdminId.ToString().ToUpper();
                        string idQuanTriKhoi = WebGroupConst.QuanTriKhoiID.ToString().ToUpper();
                        string idQuanTriTruong = WebGroupConst.QuanTriTruongID.ToString().ToUpper();
                        string idHieuTruong = WebGroupConst.HieuTruongID.ToString().ToUpper();
                        string idHieuTruongUyQuyen = WebGroupConst.HieuTruongUyQuyenID.ToString().ToUpper();
                        string idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID.ToString().ToUpper();
                        string idHoiDongQuanTriUyQuyen = WebGroupConst.HoiDongQuanTriUyQuyenID.ToString().ToUpper();
                        string idTruongPhong = WebGroupConst.TruongPhongID.ToString().ToUpper();
                        string idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID.ToString().ToUpper();
                        //Lấy cấu hình chấm công
                        int soNgayHieuTruongDuyet = 2;
                        DTO_CC_CauHinhChamCong cauHinh = (new CC_CauHinhChamCong_Factory()).GetCauHinhChamCongByCongTy(congTy);
                        if (cauHinh != null)
                        {
                            soNgayHieuTruongDuyet = cauHinh.SoNgayHieuTruongDuyet;
                        }
                        //

                        //lấy chức vụ kiêm nhiệm
                        List<HRMWeb_Business.Model.DTO.ChucNang.ChamCong.DTO_WebGroup_BoPhan> listWebGroup_BoPhan = WebUser_BoPhan_Factory.New().GetAllWebGroupByUser(userId).ToList();

                        foreach (var obj in objList)
                        {
                            if (obj != null)
                            {
                                CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                                if (objFromDB != null)
                                {
                                    foreach (var item in listWebGroup_BoPhan)
                                    {
                                        if (item.BoPhanIds.Contains(objFromDB.IDBoPhan.Value))
                                        {
                                            // Admin
                                            if (item.WebGroupId.ToString().ToUpper().Equals(idQuanTriTruong) 
                                                || item.WebGroupId.ToString().ToUpper().Equals(idAdminToanQuyen)
                                                || item.WebGroupId.ToString().ToUpper().Equals(idQuanTriKhoi))
                                            {
                                                objFromDB.TrangThai_Admin = 0;
                                                objFromDB.TrangThai_HDQT = 0;
                                                objFromDB.TrangThai_HT = 0;
                                                objFromDB.TrangThai_TP = 0;
                                                objFromDB.WebUserDuyet_Admin = userId;
                                                objFromDB.WebUserDuyet_HDQT = userId;
                                                objFromDB.WebUserDuyet_HT = userId;
                                                objFromDB.WebUserDuyet_TP = userId;
                                            }
                                            // Hội đồng quản trị
                                            if (item.WebGroupId.ToString().ToUpper().Equals(idHoiDongQuanTri)
                                                || item.WebGroupId.ToString().ToUpper().Equals(idHoiDongQuanTriUyQuyen))
                                            {
                                                objFromDB.TrangThai_HDQT = 0;
                                                objFromDB.WebUserDuyet_HDQT = userId;
                                            }
                                            // Hiệu trưởng
                                            if (item.WebGroupId.ToString().ToUpper().Equals(idHieuTruong)
                                                || item.WebGroupId.ToString().ToUpper().Equals(idHieuTruongUyQuyen))
                                            {
                                                if (objFromDB.TrangThai_HDQT != 1)
                                                {
                                                    objFromDB.TrangThai_HT = 0;
                                                    objFromDB.WebUserDuyet_HT = userId;
                                                }
                                            }
                                            // Trưởng phòng
                                            if (item.WebGroupId.ToString().ToUpper().Equals(idTruongPhong)
                                                || item.WebGroupId.ToString().ToUpper().Equals(idTruongPhongUyQuyen))
                                            {
                                                if (objFromDB.TrangThai_HT != 1)
                                                {
                                                    objFromDB.TrangThai_TP = 0;
                                                    objFromDB.WebUserDuyet_TP = userId;
                                                }
                                            }
                                        }
                                    }

                                    ///////////////// Kiểm tra xem có cập nhật bảng công hay ?////////////
                                    bool huyCong = false;

                                    // 1. Không phải ban giám hiệu
                                    if (!objFromDB.IsBanGiamHieu.Value)
                                    {
                                        // Cá nhân bình thường
                                        if (!objFromDB.IsTruongPhong.Value)
                                        {
                                            if (objFromDB.SoNgay <= soNgayHieuTruongDuyet)
                                            {
                                                if (objFromDB.TrangThai_TP == 0)
                                                {
                                                    huyCong = true;
                                                }
                                            }
                                            else
                                            {
                                                if (objFromDB.TrangThai_HT == 0)
                                                {
                                                    huyCong = true;
                                                }
                                            }
                                        }
                                        else // Trưởng phòng
                                        {
                                            if (objFromDB.TrangThai_HT == 0)
                                            {
                                                huyCong = true;
                                            }
                                        }
                                    }
                                    else // Hiệu trưởng
                                    {
                                        if (objFromDB.TrangThai_HDQT == 0)
                                        {
                                            huyCong = true;
                                        }
                                    }

                                    //Cập nhật bảng công
                                    if (huyCong || objFromDB.TrangThai_Admin == 0)
                                    {
                                        //
                                        factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, objFromDB.Buoi, false);
                                    }
                                }
                            }
                        }
                        //Lưu dữ liệu
                        factory.SaveChangesWithoutTransactionScope();

                        //Cập nhật số ngày phép năm
                        foreach (var obj in objList)
                        {
                            CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                            if (objFromDB != null)
                            {
                                //1. Lấy niên độ tài chính hiện tại
                                DTO_NienDoTaiChinh nienDoHienTai = (new NienDoTaiChinh_Factory()).GetListByNam(objFromDB.TuNgay.Value.Year, congTy);
                                if (nienDoHienTai != null)
                                {
                                    //2. Lấy quản lý theo niên độ tài chính
                                    CC_QuanLyNghiPhep quanLy = CC_QuanLyNghiPhep_Factory.New().GetByNienDoTaiChinh(nienDoHienTai.Oid, congTy);
                                    if (quanLy != null)
                                    {
                                        //
                                        if (objFromDB != null && objFromDB.IDHinhThucNghi.Equals(HinhThucNghiConst.NghiPhepId))
                                        {
                                            //
                                            factory.Context.spd_WebChamCong_TinhSoNgayPhepConLaiTrongNam(quanLy.Oid, obj.IDNhanVien, congTy);
                                        }
                                    }
                                }
                            }
                        }
                        //
                        transaction.Complete();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Service1_ChucNangChamCongNgayNghi/ChamCongNgayNghi_CancelList", ex);
                throw ex;
            }
        }
        public bool DangKyNghiPhep_AcceptList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList, int isAdmin)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    CC_QuanLyNghiPhep_Factory qlfac = CC_QuanLyNghiPhep_Factory.New();

                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                            //
                            if (isAdmin == 1)
                                objFromDB.TrangThai_HDQT = 1;
                            if (isAdmin == 0)
                                objFromDB.TrangThai_TP = 1;
                            if (objFromDB.TrangThai_HT == 1)
                            {
                                //Cập nhật chấm công theo ngày
                                factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, objFromDB.Buoi, true);
                            }
                        }
                    }
                    try
                    {
                        //
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            return false;
        }

        public bool DangKyNghiPhep_CancelList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList, int isAdmin)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                            //
                            if (isAdmin == 1)
                                objFromDB.TrangThai_HT = 0;
                            if (isAdmin == 0)
                                objFromDB.TrangThai_HDQT = 0;
                            //
                            if (objFromDB.TrangThai_TP == 0)
                            {
                                //
                                factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, objFromDB.Buoi, false);
                            }
                        }
                    }
                    //////////////
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }
        public bool ChamCongNgayNghi_AcceptList_Json(String publicKey, String token, string jsonObjectList, Guid userId, Guid congTy)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_AcceptList(publicKey, token, objList, userId,congTy);
        }
        public bool ChamCongNgayNghi_CancelList_Json(String publicKey, String token, string jsonObjectList, Guid userId, Guid congTy)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_CancelList(publicKey, token, objList, userId, congTy);
        }
        public bool DangKyNghiPhep_AcceptList_Json(String publicKey, String token, string jsonObjectList, int isAdmin)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return DangKyNghiPhep_AcceptList(publicKey, token, objList, isAdmin);
        }
        public bool DangKyNghiPhep_CancelList_Json(String publicKey, String token, string jsonObjectList, int isAdmin)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return DangKyNghiPhep_CancelList(publicKey, token, objList, isAdmin);
        }
        public String DangKyChamCongNgayNghi_Find_Json(String publicKey, String token, DateTime tungay, DateTime dengnay, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = DangKyChamCongNgayNghi_Find(publicKey, token, tungay, dengnay, idNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_ChamCongNgayNghi_Find> DangKyChamCongNgayNghi_Find(String publicKey, String token, DateTime tungay, DateTime dengnay, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.DangKyChamCongNgayNghi_Find(tungay, dengnay, idNhanVien);
                return list;
            }
            else
            {
                return null;
            }
        }
        public bool ChamCongNgayNghi_TaoMoi(String publicKey, String token, Guid nhanVienID, String noiDung, String nguoibangiao, String tenDonXinNghi, String diaChiLienHe, Guid idHinhThucNghi, int loaiDonXinNghi, DateTime tuNgay, DateTime denNgay, Guid webUserId, int buoi, Guid congTy)
        {//
            try
            {
                if (Helper.TrustTest(publicKey, token))
                {
                    using (var tran = new TransactionScope(TransactionScopeOption.Required,
                        TimeSpan.FromSeconds(360)))
                    {
                        Guid idYerSin = BoPhanConst.YerSinGuid;

                        //
                        HoSo_Factory hoSo_Factory = HoSo_Factory.New();
                        HoSo hoSo = hoSo_Factory.GetByID(nhanVienID);
                        if (hoSo != null)
                        {
                            CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                            CC_ChamCongNgayNghi newObj = factory.CreateManagedObject();
                            newObj.Oid = Guid.NewGuid();
                            newObj.TuNgay = tuNgay.Date;
                            newObj.DenNgay = denNgay.Date;
                            newObj.DienGiai = noiDung.Trim();
                            newObj.IDBoPhan = hoSo.NhanVien.BoPhan;
                            newObj.IDNhanVien = nhanVienID;
                            newObj.IDHinhThucNghi = idHinhThucNghi;
                            newObj.IDWebUser = webUserId;
                            newObj.NgayTao = DateTime.Now;
                            newObj.LoaiNghiPhep = loaiDonXinNghi;
                            newObj.NguoiBanGiao = nguoibangiao;
                            newObj.DiaChiLienHe = diaChiLienHe;
                            newObj.TrangThai_TP = -1;
                            newObj.TrangThai_HDQT = -1;
                            newObj.TrangThai_HT = -1;
                            newObj.Buoi = Convert.ToByte(buoi);
                            //
                            if (hoSo.NhanVien.BoPhan1.TenBoPhan.ToLower().Contains("ban giám hiệu"))
                            {
                                newObj.IsBanGiamHieu = true;
                                newObj.IsTruongPhong = false;
                            }
                            else
                            {
                                newObj.IsBanGiamHieu = false;
                                //
                                //if (hoSo.NhanVien.ThongTinNhanVien.ChucVu1 != null && hoSo.NhanVien.ThongTinNhanVien.ChucVu1.LaTruongDonVi == true)
                                if (hoSo.NhanVien.ThongTinNhanVien.WebUsers.Select(q => q.WebGroupID).Contains(WebGroupConst.TruongPhongID))
                                //if (hoSo.NhanVien.ThongTinNhanVien.ChucVu1?.LaTruongDonVi == true 
                                //    && (hoSo.NhanVien.BoPhan1.BoPhan2 == null || hoSo.NhanVien.BoPhan1.BoPhan2.LoaiBoPhan == 0))
                                {
                                    newObj.IsTruongPhong = true;
                                }
                                else
                                {
                                    newObj.IsTruongPhong = false;
                                }
                            }
                            //
                            if (idYerSin.Equals(hoSo.NhanVien.CongTy))
                            {
                                newObj.SoNgay = Helper.GetBusinessDays(tuNgay, denNgay, hoSo.NhanVien.CongTy); // Nghỉ t7
                            }
                            else
                            {
                                newObj.SoNgay = Helper.GetBusinessDays1(tuNgay, denNgay, hoSo.NhanVien.CongTy); // Làm thứ 7
                            }
                            //Nếu là nữa buổi thì chia lại
                            if (newObj.Buoi != 1)
                            {
                                newObj.SoNgay = newObj.SoNgay / 2;
                            }
                            //
                            newObj.TenGiayXinPhep = tenDonXinNghi;

                            //
                            DTO_WebUser userHienTai = (new WebUser_Factory()).GetDTO_WebUser_ById(webUserId);
                            if (userHienTai == null) return false;
                            //
                            Guid idAdminToanQuyen = WebGroupConst.AdminId;
                            Guid idQuanTriKhoi = WebGroupConst.QuanTriKhoiID;
                            Guid idQuanTriTruong = WebGroupConst.QuanTriTruongID;
                            Guid idTruongPhong = WebGroupConst.TruongPhongID;
                            Guid idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID;
                            Guid idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID;
                            Guid idHoiDongQuanTriUQ = WebGroupConst.HoiDongQuanTriUyQuyenID;
                            Guid idHieuTruong = WebGroupConst.HieuTruongID;
                            Guid idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID;
                            Guid idCaNhan = WebGroupConst.TaiKhoanCaNhanID;

                            //Cấu hình chấm công
                            int soNgayHieuTruongDuyet = 2;
                            DTO_CC_CauHinhChamCong cauHinh = (new CC_CauHinhChamCong_Factory()).GetCauHinhChamCongByCongTy(hoSo.NhanVien.CongTy.Value);
                            if (cauHinh != null)
                            {
                                soNgayHieuTruongDuyet = cauHinh.SoNgayHieuTruongDuyet;
                            }
                            //Admin
                            if (userHienTai.WebGroupID.Equals(idQuanTriTruong) 
                                || userHienTai.WebGroupID.Equals(idAdminToanQuyen)
                                || userHienTai.WebGroupID.Equals(idQuanTriKhoi))
                            {
                                newObj.TrangThai_Admin = 1;

                                if (!newObj.IsBanGiamHieu.Value)
                                {
                                    if (newObj.SoNgay <= soNgayHieuTruongDuyet)
                                    {
                                        //
                                        newObj.TrangThai_TP = 1;
                                    }
                                    if (newObj.SoNgay > soNgayHieuTruongDuyet || newObj.IsTruongPhong.Value)
                                    {
                                        //
                                        newObj.TrangThai_TP = 1;
                                        newObj.TrangThai_HT = 1;
                                    }
                                }
                                if (newObj.IsBanGiamHieu.Value)
                                {
                                    newObj.TrangThai_HDQT = 1;
                                }
                            }
                            //Hội đồng quản trị
                            if (userHienTai.WebGroupID.Equals(idHoiDongQuanTri)
                                || userHienTai.WebGroupID.Equals(idHoiDongQuanTriUQ))
                            {
                                newObj.TrangThai_HDQT = 1;
                            }
                            //Hiệu trưởng
                            if (userHienTai.WebGroupID.Equals(idHieuTruong)
                                || userHienTai.WebGroupID.Equals(idHieuTruongUQ))
                            {
                                newObj.TrangThai_HT = 1;
                                //
                                if (newObj.IsBanGiamHieu.Value)
                                {
                                    //Gửi đến hội đồng quản trị
                                    ChamCongNgayNghi_SetDataSendMail(newObj, cauHinh, userHienTai.EmailHDQT, userHienTai.Oid);
                                }
                            }
                            //Trường phòng or Trường phòng ủy quyền
                            if (userHienTai.WebGroupID.Equals(idTruongPhong)
                                || userHienTai.WebGroupID.Equals(idTruongPhongUyQuyen))
                            {
                                newObj.TrangThai_TP = 1;
                                //

                                if (!newObj.IsBanGiamHieu.Value)
                                {
                                    if (!newObj.IsTruongPhong.Value && newObj.SoNgay > soNgayHieuTruongDuyet)
                                    {
                                        //Gửi đến hiệu trưởng
                                        ChamCongNgayNghi_SetDataSendMail(newObj, cauHinh, userHienTai.EmailHT, userHienTai.Oid);
                                    }
                                    if (newObj.IsTruongPhong.Value)
                                    {
                                        //Gửi đến hiệu trưởng
                                        ChamCongNgayNghi_SetDataSendMail(newObj, cauHinh, userHienTai.EmailHT, userHienTai.Oid);
                                    }
                                }
                                if (newObj.IsBanGiamHieu.Value)
                                {
                                    //Gửi đến hội đồng quản trị
                                    ChamCongNgayNghi_SetDataSendMail(newObj, cauHinh, userHienTai.EmailHDQT, userHienTai.Oid);
                                }
                            }

                            //Cá nhân đăng ký
                            if (userHienTai.WebGroupID.Equals(idCaNhan))
                            {
                                if (newObj.IsBanGiamHieu.Value)
                                {
                                    //Gửi đến Hội đồng quản trị
                                    ChamCongNgayNghi_SetDataSendMail(newObj, cauHinh, userHienTai.EmailHDQT, userHienTai.Oid);

                                }
                                if (newObj.IsTruongPhong.Value)
                                {
                                    //Gửi đến hiệu trưởng
                                    ChamCongNgayNghi_SetDataSendMail(newObj, cauHinh, userHienTai.EmailHT, userHienTai.Oid);
                                }
                                else
                                {
                                    if (cauHinh.TruongDonViKhongDuyet == true)
                                    {
                                        //Gửi đến Hiệu trưởng
                                        ChamCongNgayNghi_SetDataSendMail(newObj, cauHinh, userHienTai.EmailHT, userHienTai.Oid);
                                    }
                                    else
                                    {
                                        //Gửi đến trưởng phòng
                                        ChamCongNgayNghi_SetDataSendMail(newObj, cauHinh, userHienTai.EmailTP, userHienTai.Oid);
                                    }
                                }
                            }

                            ///////////////// Kiểm tra xem có cập nhật bảng công hay ?////////////
                            bool capNhat = false;
                            // 1. Không phải ban giám hiệu
                            if (!newObj.IsBanGiamHieu.Value)
                            {
                                //Cá nhân bình thường
                                if (!newObj.IsTruongPhong.Value)
                                {
                                    if (newObj.SoNgay <= soNgayHieuTruongDuyet)
                                    {
                                        if (newObj.TrangThai_TP == 1)
                                        {
                                            capNhat = true;
                                        }
                                    }
                                    else
                                    {
                                        if (newObj.TrangThai_HT == 1)
                                        {
                                            capNhat = true;
                                        }
                                    }
                                }
                                else // Trưởng đơn vị
                                {
                                    if (newObj.TrangThai_HT == 1)
                                    {
                                        capNhat = true;
                                    }
                                }
                            }
                            else // Ban giám hiệu
                            {
                                if (newObj.TrangThai_HDQT == 1)
                                {
                                    capNhat = true;
                                }
                            }

                            //Lưu dữ liệu
                            factory.SaveChangesWithoutTransactionScope();

                            //
                            if (capNhat || newObj.TrangThai_Admin == 1)
                            {
                                //Cập nhật bảng chấm công ngày nghỉ
                                factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(newObj.IDNhanVien, newObj.TuNgay, newObj.DenNgay, newObj.IDHinhThucNghi, newObj.Buoi, true);

                                //Câp nhật số ngày phép năm
                                if (newObj.IDHinhThucNghi == HinhThucNghiConst.NghiPhepId) //Cập nhật quản lý nghỉ phép năm
                                {
                                    DTO_NienDoTaiChinh nienDoHienTai = (new NienDoTaiChinh_Factory()).GetListByNam(newObj.TuNgay.Value.Year, hoSo.NhanVien.CongTy.Value);
                                    if (nienDoHienTai != null)
                                    {
                                        CC_QuanLyNghiPhep quanLy = CC_QuanLyNghiPhep_Factory.New().GetByNienDoTaiChinh(nienDoHienTai.Oid, hoSo.NhanVien.CongTy.Value);
                                        if (quanLy != null)
                                        {
                                            //
                                            factory.Context.spd_WebChamCong_TinhSoNgayPhepConLaiTrongNam(quanLy.Oid, newObj.IDNhanVien, hoSo.NhanVien.CongTy.Value);
                                        }
                                    }
                                }
                            }

                            //Hoàn tất
                            tran.Complete();
                            //
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Service1_ChucNangChamCongNgayNghi/ChamCongNgayNghi_TaoMoi", ex);
                throw;
            }
        }

        /// <summary>
        /// Gửi email cho cấp trên duyệt
        /// </summary>
        public void ChamCongNgayNghi_SetDataSendMail(CC_ChamCongNgayNghi newObj, DTO_CC_CauHinhChamCong cauHinh, string emailReceiver, Guid user)
        {
            var URL = Helper.GetCurrentDomainName();
            var webMailTemplate = new WebMailTemplate_Factory().GetByCongTyVaLoaiGuiMail(newObj.BoPhan.CongTy ?? Guid.Empty, WebMailTemplateTypeConst.DuyetDonXinNghi);

            string tieudeguimail = "";
            string noidungguimail = "";

            if (webMailTemplate != null && webMailTemplate.TieuDe != null && webMailTemplate.NoiDung != null)
            {
                tieudeguimail = ChamCongNgayNghi_MailTemplateReplaceText(newObj, cauHinh, webMailTemplate.TieuDe);
                noidungguimail = ChamCongNgayNghi_MailTemplateReplaceText(newObj, cauHinh, webMailTemplate.NoiDung);
            }
            else
            {
                tieudeguimail = "DUYỆT " + newObj.CC_HinhThucNghi.TenHinhThucNghi.ToUpper();
                noidungguimail = "Họ tên: [" + newObj.ThongTinNhanVien.NhanVien.HoSo.HoTen.ToUpper() + "] Đơn vị: [" + newObj.BoPhan.TenBoPhan.ToUpper() + "] xin nghỉ: [" + newObj.CC_HinhThucNghi.TenHinhThucNghi + "] Từ ngày [" + String.Format("{0:dd/MM/yyyy}", newObj.TuNgay) + "] Đến ngày [" + String.Format("{0:dd/MM/yyyy}", newObj.DenNgay) + "]";
                noidungguimail += "<br/><a target=\"_blank\" href=\"" + URL + "/kpi/chamcongngaynghi?id=" + newObj.Oid + "\">" + "Duyệt" + "</a><br/>";
            }
            
            //Gửi mail
            QuanLyGuiEmail_SendMail(cauHinh, emailReceiver, tieudeguimail, noidungguimail, user);

        }

        /// <summary>
        /// Gửi email cho người nghỉ phép sau khi cấp trên đã duyệt đơn
        /// </summary>
        public void ChamCongNgayNghi_GuiMailChoNguoiDangKy(CC_ChamCongNgayNghi newObj, DTO_CC_CauHinhChamCong cauHinh, string emailReceiver, Guid user)
        {
            var webMailTemplate = new WebMailTemplate_Factory().GetByCongTyVaLoaiGuiMail(newObj.BoPhan.CongTy ?? Guid.Empty, WebMailTemplateTypeConst.PhanHoiDenNguoiNghi);

            string tieudeguimail = "";
            string noidungguimail = "";

            if (webMailTemplate != null && webMailTemplate.TieuDe != null && webMailTemplate.NoiDung != null)
            {
                tieudeguimail = ChamCongNgayNghi_MailTemplateReplaceText(newObj, cauHinh, webMailTemplate.TieuDe);
                noidungguimail = ChamCongNgayNghi_MailTemplateReplaceText(newObj, cauHinh, webMailTemplate.NoiDung);
            }
            else
            {
                tieudeguimail = "ĐƠN XIN " + newObj.CC_HinhThucNghi.TenHinhThucNghi.ToUpper() + " ĐÃ ĐƯỢC DUYỆT";
                noidungguimail = "Họ tên: [" + newObj.ThongTinNhanVien.NhanVien.HoSo.HoTen.ToUpper() + "] Đơn vị: [" + newObj.BoPhan.TenBoPhan.ToUpper() + "] xin nghỉ: [" + newObj.CC_HinhThucNghi.TenHinhThucNghi + "] Từ ngày [" + String.Format("{0:dd/MM/yyyy}", newObj.TuNgay) + "] Đến ngày [" + String.Format("{0:dd/MM/yyyy}", newObj.DenNgay) + "]";
            }

            //Gửi mail
            QuanLyGuiEmail_SendMail(cauHinh, emailReceiver, tieudeguimail, noidungguimail, user);

        }

        public string ChamCongNgayNghi_MailTemplateReplaceText(CC_ChamCongNgayNghi newObj, DTO_CC_CauHinhChamCong cauHinh, string template)
        {
            var result = string.Empty;
            if (template != null)
            {
                var URL = Helper.GetCurrentDomainName();
                result = template.Replace("HinhThucNghi", newObj.CC_HinhThucNghi.TenHinhThucNghi)
                    .Replace("HoTen", newObj.ThongTinNhanVien.NhanVien.HoSo.HoTen)
                    .Replace("DonVi", newObj.BoPhan.TenBoPhan)
                    .Replace("NghiTuNgay", String.Format("{0:dd/MM/yyyy}", newObj.TuNgay))
                    .Replace("NghiDenNgay", String.Format("{0:dd/MM/yyyy}", newObj.DenNgay))
                    .Replace("http://linkduyet/", URL + "/kpi/chamcongngaynghi?id=" + newObj.Oid);
            }
            return result;
        }

        public bool DangKyNghiPhep_TaoMoi(String publicKey, String token, Guid nhanVienID, String noiDung, String noiNghiPhep, String tenDonXinNghi, String diaChiLienHe, int loaiDonXinNghi, DateTime tuNgay, DateTime denNgay, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    Guid idHinhThucNghi = Guid.Empty;
                    //
                    HoSo_Factory hoSo_Factory = HoSo_Factory.New();
                    HoSo hoSo = hoSo_Factory.GetByID(nhanVienID);
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    CC_ChamCongNgayNghi newObj = factory.CreateManagedObject();
                    newObj.Oid = Guid.NewGuid();
                    newObj.TuNgay = tuNgay.Date;
                    newObj.DenNgay = denNgay.Date;
                    newObj.DienGiai = noiDung.Trim();
                    newObj.IDBoPhan = hoSo.NhanVien.BoPhan;
                    newObj.IDNhanVien = nhanVienID;
                    newObj.IDHinhThucNghi = idHinhThucNghi;
                    newObj.IDWebUser = webUserId;
                    newObj.NgayTao = DateTime.Now;
                    newObj.LoaiNghiPhep = loaiDonXinNghi;
                    newObj.NguoiBanGiao = string.Empty;
                    newObj.DiaChiLienHe = diaChiLienHe;
                    newObj.TrangThai_HDQT = -1;
                    newObj.TrangThai_TP = -1;
                    newObj.SoNgay = Helper.GetBusinessDays(tuNgay, denNgay, hoSo.NhanVien.CongTy);
                    newObj.TenGiayXinPhep = tenDonXinNghi;
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        tran.Complete();
                        //
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }

        public bool DangKyChamCongNgayNghi_DeleteList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_ChamCongNgayNghi stupidObj = new CC_ChamCongNgayNghi() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_ChamCongNgayNghi_Factory.FullDelete(factory.Context, stupidObj);
                        }
                    }
                    //////////////
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }
        public bool DangKyChamCongNgayNghi_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return DangKyChamCongNgayNghi_DeleteList(publicKey, token, objList);
        }
        public bool ChamCongNgayNghi_DeleteList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_ChamCongNgayNghi stupidObj = new CC_ChamCongNgayNghi() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_ChamCongNgayNghi_Factory.FullDelete(factory.Context, stupidObj);
                        }
                    }
                    //////////////
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }

        public bool ChamCongNgayNghi_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_DeleteList(publicKey, token, objList);
        }


        public bool ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, Guid? chamCongNgayNghiOid, DateTime tuNgay, DateTime denNgay, Guid nhanVienID, int buoi)
        {//DANG SD
            tuNgay = tuNgay.Date;
            denNgay = denNgay.Date;
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();

                //bat dau kiem tra hop le
                var trungHoacGiaoNgay_ChamCongNgayNghi = (from o in factory.Context.CC_ChamCongNgayNghi
                                                          where o.IDNhanVien == nhanVienID
                                                                  && (
                                                                     (o.TuNgay <= tuNgay && tuNgay <= o.DenNgay)
                                                                     ||
                                                                     (o.TuNgay <= denNgay && denNgay <= o.DenNgay)
                                                                    ||
                                                                    (tuNgay <= o.TuNgay && o.TuNgay <= denNgay)
                                                                    ||
                                                                    (tuNgay <= o.DenNgay && o.DenNgay <= denNgay)
                                                                  )
                                                                  && (o.Buoi == buoi || buoi == 1)
                                                          select true).FirstOrDefault();

                var trungHoacGiaoNgay_KhaiBaoCongTac = (from o in factory.Context.CC_KhaiBaoCongTac
                                                        where o.IDNhanVien == nhanVienID
                                                              && (
                                                               (o.TuNgay <= tuNgay && tuNgay <= o.DenNgay)
                                                               ||
                                                               (o.TuNgay <= denNgay && denNgay <= o.DenNgay)
                                                                ||
                                                                (tuNgay <= o.TuNgay && o.TuNgay <= denNgay)
                                                                ||
                                                                (tuNgay <= o.DenNgay && o.DenNgay <= denNgay)
                                                              )
                                                              //&& (o.Buoi == buoi || buoi == 1) Tạm thời tắt

                                                        select true).FirstOrDefault();
                var hopLe = false;
                if (trungHoacGiaoNgay_KhaiBaoCongTac)
                    hopLe = trungHoacGiaoNgay_KhaiBaoCongTac;
                if (trungHoacGiaoNgay_ChamCongNgayNghi)
                    hopLe = trungHoacGiaoNgay_ChamCongNgayNghi;
                //
                return hopLe;

            }
            return false;
        }

        #region SAVE
        //kiem tra duoc phep luu

        // Save////////////////////////////////////////////
        public bool ChamCongNgayNghi_Save(String publicKey, String token, DTO_ChamCongNgayNghi_Find obj)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    using (var tran = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                    {
                        CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                        CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                        if (objFromDB != null)
                        {

                            objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "IDHinhThucNghi", "TuNgay", "DenNgay", "DienGiai" });
                            objFromDB.TuNgay = objFromDB.TuNgay.Value.Date;
                            objFromDB.DenNgay = objFromDB.DenNgay.Value.Date;
                            //
                            factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, objFromDB.Buoi, true);
                            try
                            {
                                factory.SaveChangesWithoutTransactionScope();
                                tran.Complete();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool ChamCongNgayNghi_Save_Json(String publicKey, String token, string jsonObject)
        {
            //chuyen jsonObject thanh object
            DTO_ChamCongNgayNghi_Find obj = JsonConvert.DeserializeObject<DTO_ChamCongNgayNghi_Find>(jsonObject);
            return ChamCongNgayNghi_Save(publicKey, token, obj);
        }

        public bool ChamCongNgayNghi_SaveList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var tran = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            //
                            CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                            if (objFromDB != null)
                            {
                                objFromDB.CopyIncludedPropertiesFrom(obj,
                                    new[] { "Oid", "IDHinhThucNghi", "TuNgay", "DenNgay", "DienGiai" });
                                objFromDB.TuNgay = objFromDB.TuNgay.Value.Date;
                                objFromDB.DenNgay = objFromDB.DenNgay.Value.Date;
                                factory.Context.spd_WebChamCong_CapNhatChamCongNgayNghi(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, objFromDB.Buoi, true);
                            }
                        }
                    }
                    ////
                    try
                    {
                        factory.SaveChangesWithoutTransactionScope();
                        tran.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return false;
        }
        public bool ChamCongNgayNghi_SaveList_Json(String publicKey, String token, string jsonObjectList)
        {
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_SaveList(publicKey, token, objList);
        }

        public bool QuanLyNghiPhep_LuuTraPhep(String publicKey, String token, Guid oid, DateTime ngaytraphep)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    //
                    try
                    {
                        var factory = CC_ChamCongNgayNghi_Factory.New();
                        CC_ChamCongNgayNghi quanlynghiphep = factory.GetByID(oid);
                        //quanlynghiphep.NgayTraPhep = ngaytraphep;
                        //
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        //
                        return true;
                    }
                    catch (Exception ex) { }
                }
            }
            //
            return false;
        }

        public bool QuanLyNghiPhep_HuyTraPhep(String publicKey, String token, Guid oid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    //
                    try
                    {
                        var factory = CC_ChamCongNgayNghi_Factory.New();
                        CC_ChamCongNgayNghi quanlynghiphep = factory.GetByID(oid);
                        //quanlynghiphep.NgayTraPhep = null;
                        //
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        //
                        return true;
                    }
                    catch (Exception ex) { }
                }
            }
            //
            return false;
        }

        public void QuanLyGuiEmail_SendMail(DTO_CC_CauHinhChamCong cauHinh, string emailnhan, string tieude, string noidung, Guid user)
        {
            try
            {
                bool sucess = false;
                var URL = Helper.GetCurrentDomainName();
                if (cauHinh != null && !string.IsNullOrEmpty(cauHinh.EmailSender) && !string.IsNullOrEmpty(cauHinh.PassSender) && !string.IsNullOrEmpty(cauHinh.Host) && cauHinh.Port != null
                    && !string.IsNullOrEmpty(emailnhan) && !URL.Contains("localhost") && !URL.Contains("psctelecom.com.vn"))
                {
                    sucess = GuiMail(tieude, noidung, cauHinh.EmailSender, cauHinh.PassSender, emailnhan, cauHinh.Host, cauHinh.Port);
                }
                if (sucess)
                {
                    CC_EmailManager_Factory factory = new CC_EmailManager_Factory();
                    CC_MailManager sendMail = factory.CreateManagedObject();
                    sendMail.Oid = Guid.NewGuid();
                    sendMail.Title = tieude;
                    sendMail.Contents = noidung;
                    //
                    sendMail.SendEmail = cauHinh.EmailSender;
                    sendMail.SendPass = cauHinh.PassSender;
                    //
                    sendMail.ReceiverEmail = emailnhan;
                    sendMail.SendDate = DateTime.Now;
                    sendMail.IDWebUser = user;
                    //
                    factory.SaveChanges();
                    //
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Service1_ChucNangChamCongNgayNghi/QuanLyGuiEmail_SendMail", ex);
            }
        }

        private bool GuiMail(string tieude, string noidung, string emailgui, string passgui, string emailnhan, string host, int? port)
        {
            port = port ?? 25;
            bool sucess = false;
            try
            {
                var msg = new System.Net.Mail.MailMessage();
                var smtpClient = new SmtpClient()
                {
                    Host = host,
                    Port = port.Value,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailgui, passgui)
                };
                msg.From = new MailAddress(emailgui);
                msg.To.Add(new MailAddress(emailnhan));
                msg.Subject = tieude;
                msg.Body = noidung;
                msg.IsBodyHtml = true;
                
                smtpClient.Send(msg);
                sucess = true;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Service1_ChucNangChamCongNgayNghi/GuiMail: email = " + emailgui + ", pass = " + passgui + ", emailnhan = " + emailnhan + ", host = " + host + ", port = " + port, ex);
                sucess = false;
            }
            return sucess;
        }

        #endregion

        #region Nhắc việc

        public IEnumerable<DTO_ChamCongNgayNghi_Find> ChamCongNgayNghi_Find_NhacViec(String publicKey, String token, DateTime tungay, DateTime denngay, string manhansu, Guid idBoPhan, Guid idLoaiNhanSu, int? trangthai, Guid idWebUser, Guid congTy, bool tatCaDonChuaDuyet)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IQueryable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.FindForChamCongNgayNghi_NhacViec(tungay, denngay, manhansu, idBoPhan, idLoaiNhanSu, trangthai, idWebUser, congTy, tatCaDonChuaDuyet);
                //
                return list;
            }
            else
            {
                return null;
            }
        }
        #endregion
        
        public IEnumerable<DTO_ChamCongNgayNghi_Find> ChamCongNgayNghi_Find_ById(String publicKey, String token, Guid chamCongNgayNghiId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                var x = Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault();
                var domainName1 = Dns.GetHostEntry(x);
                var domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                Helper.ErrorLog("ChamCongNgayNghi_Find_ById, domainName = " + domainName + ", domainName1 = " + domainName1.HostName, null);
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IQueryable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.FindForChamCongNgayNghi_ById(chamCongNgayNghiId);
                //
                return list;
            }
            else
            {
                return null;
            }
        }
    }
}
