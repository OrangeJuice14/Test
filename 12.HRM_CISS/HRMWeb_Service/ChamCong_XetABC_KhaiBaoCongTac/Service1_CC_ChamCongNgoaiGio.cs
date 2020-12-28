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
using HRMWeb_Business.Predefined;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {

        public IEnumerable<DTO_CC_ChamCongNgoaiGio> DangKyChamCongNgoaiGio_Find(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgoaiGio_Factory factory = CC_ChamCongNgoaiGio_Factory.New();
                IEnumerable<DTO_CC_ChamCongNgoaiGio> objList = factory.ChamCongNgoaiGio_Find(thang, nam, idNhanVien);
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public IEnumerable<DTO_CC_ChamCongNgoaiGio> QuanLyChamCongNgoaiGio_Find(String publicKey, String token, int ngay, int thang, int nam, Guid IDBoPhan, int trangthai, Guid userID, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgoaiGio_Factory factory = CC_ChamCongNgoaiGio_Factory.New();
                var tmpList = factory.QuanLyChamCongNgoaiGio_Find(ngay, thang, nam, IDBoPhan, trangthai, userID, congTy).ToList();
                IEnumerable<DTO_CC_ChamCongNgoaiGio> objList = tmpList.Map<DTO_CC_ChamCongNgoaiGio>();
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public IEnumerable<DTO_CC_ChamCongNgoaiGio> QuanLyChamCongNgoaiGio_Find_NhacViec(String publicKey, String token, int ngay, int thang, int nam, Guid IDBoPhan, int trangthai, Guid userID, Guid congTy, bool tatCaDonChuaDuyet)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgoaiGio_Factory factory = CC_ChamCongNgoaiGio_Factory.New();
                var tmpList = factory.QuanLyChamCongNgoaiGio_Find_NhacViec(ngay, thang, nam, IDBoPhan, trangthai, userID, congTy, tatCaDonChuaDuyet).ToList();
                IEnumerable<DTO_CC_ChamCongNgoaiGio> objList = tmpList.Map<DTO_CC_ChamCongNgoaiGio>();
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String DangKyChamCongNgoaiGio_Find_Json(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_CC_ChamCongNgoaiGio> list = DangKyChamCongNgoaiGio_Find(publicKey, token, thang, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String QuanLyChamCongNgoaiGio_Find_Json(String publicKey, String token, int ngay, int thang, int nam, Guid IDBoPhan, int trangthai, Guid userID, Guid congTy)
        {//DANG SD
            IEnumerable<DTO_CC_ChamCongNgoaiGio> list = QuanLyChamCongNgoaiGio_Find(publicKey, token, ngay, thang, nam, IDBoPhan, trangthai, userID, congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public bool Save_DangKyChamCongNgoaiGio_Json(String publicKey, String token, string jsonObject, Guid idwebuser)
        {//DANG SD
            //
            var obj = JsonConvert.DeserializeObject<DTO_CC_ChamCongNgoaiGio>(jsonObject);
            return Save_DangKyChamCongNgoaiGio(publicKey, token, obj, idwebuser);
        }
        public bool ChamCongNgoaiGio_SaveList_Json(String publicKey, String token, string jsonObjectList)
        {
            //chuyen jsonObject thanh object
            List<DTO_CC_ChamCongNgoaiGio> objList = JsonConvert.DeserializeObject<List<DTO_CC_ChamCongNgoaiGio>>(jsonObjectList);
            return ChamCongNgoaiGio_SaveList(publicKey, token, objList);
        }
        public bool ChamCongNgoaiGio_SaveList(String publicKey, String token, List<DTO_CC_ChamCongNgoaiGio> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgoaiGio_Factory factory = CC_ChamCongNgoaiGio_Factory.New();
                foreach (var obj in objList)
                {
                    if (obj != null)
                    {
                        CC_ChamCongNgoaiGio objFromDB = factory.GetByID(obj.Oid);
                        if (objFromDB != null)
                        {
                            //cap nhat
                            objFromDB.SoGioThucTe = Convert.ToDecimal(obj.SoGioThucTe);
                        }

                    }
                }
                //////////////
                try
                {
                    factory.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        public bool Save_DangKyChamCongNgoaiGio(String publicKey, String token, DTO_CC_ChamCongNgoaiGio obj, Guid idwebuser)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                if (obj != null)
                {
                    try
                    {
                        var factory = CC_ChamCongNgoaiGio_Factory.New();
                        //
                        var currentUser = (new WebUser_Factory()).GetByID(idwebuser);
                        if (currentUser == null) return false;
                        //
                        Guid idTaiKhoanTruongPhong = WebGroupConst.TruongPhongID;
                        Guid idTaiKhoanTruongPhongUQ = WebGroupConst.TruongPhongUyQuyenID;
                        Guid idTaiKhoanQuanTri = WebGroupConst.QuanTriTruongID;
                        Guid idAdminToanQuyen = WebGroupConst.AdminId;
                        Guid idQuanTriKhoi = WebGroupConst.QuanTriKhoiID;
                        Guid idTaiKhoanBinhThuong = WebGroupConst.TaiKhoanCaNhanID;

                        //
                        List<int> ListThu = new List<int>();
                        foreach (DTO_Thu thu in obj.DanhSachDTO_Thu)
                        {
                            if (thu.Chon == true)
                            {
                                //
                                ListThu.Add(thu.Id);
                            }
                        }
                        //Lấy nhân viên
                        var nhanVien = (from o in factory.Context.ThongTinNhanViens
                                        where o.Oid == obj.IDNhanVien
                                        select o).SingleOrDefault();
                        if (nhanVien == null) return false;

                        var inserted = 0;
                        //
                        DateTime date = obj.TuNgay;
                        while (date <= obj.DenNgay)
                        {
                            byte Thu = Convert.ToByte(date.DayOfWeek);
                            //
                            foreach (int thu in ListThu)
                            {
                                if (Thu == thu)
                                {
                                    //
                                    CC_ChamCongNgoaiGio newDBObject = factory.CreateManagedObject();
                                    newDBObject.Oid = Guid.NewGuid();
                                    newDBObject.CongTy = nhanVien.NhanVien.CongTy;
                                    newDBObject.IDNhanVien = obj.IDNhanVien;
                                    newDBObject.IDBoPhan = nhanVien.NhanVien.BoPhan;
                                    newDBObject.ThongTinNhanVien = nhanVien;
                                    newDBObject.Ngay = date;
                                    newDBObject.LyDo = obj.LyDo;
                                    newDBObject.TrangThai_TP = 0;
                                    newDBObject.TrangThai_Admin = 0;
                                    newDBObject.TrangThai_BGH = 0;

                                    if (nhanVien.WebUsers.Select(q => q.WebGroupID).Contains(WebGroupConst.TruongPhongID))
                                    {
                                        newDBObject.IsTruongPhong = true;
                                    }
                                    else
                                    {
                                        newDBObject.IsTruongPhong = false;
                                    }
                                    //
                                    if (currentUser.WebGroupID.Equals(idTaiKhoanBinhThuong))
                                    {
                                        newDBObject.NgoaiKeHoach = true;
                                    }
                                    else
                                    {
                                        newDBObject.NgoaiKeHoach = false;
                                    }
                                    //
                                    if (currentUser.WebGroupID.Equals(idTaiKhoanTruongPhong)
                                        || currentUser.WebGroupID.Equals(idTaiKhoanTruongPhongUQ))
                                    {
                                        newDBObject.TrangThai_TP = 1;
                                    }
                                    if (currentUser.WebGroupID.Equals(idTaiKhoanQuanTri)
                                        || currentUser.WebGroupID.Equals(idAdminToanQuyen)
                                        || currentUser.WebGroupID.Equals(idQuanTriKhoi))
                                    {
                                        newDBObject.TrangThai_Admin = 1;
                                    }
                                    //
                                    newDBObject.IDWebUsers = idwebuser;
                                    newDBObject.DaChamCong = false;
                                    newDBObject.SoPhutDangKy = (ParseTime(obj.GioKetThuc, obj.PhutKetThuc) - ParseTime(obj.GioBatDau, obj.PhutBatDau)) * 60;
                                    try
                                    {
                                        var loaiNgayNgoaiGio = factory.Context.spd_WebChamCong_LayLoaiNgayNgoaiGio(date.Date, obj.GioBatDau.ToString(), obj.PhutBatDau.ToString(), currentUser.CongTyId, obj.IDNhanVien).FirstOrDefault();
                                        if (loaiNgayNgoaiGio != null)
                                        {
                                            newDBObject.LoaiNgayNgoaiGio = loaiNgayNgoaiGio.LoaiNgayNgoaiGio;
                                            if (obj.GioBatDau <= 11 && obj.GioKetThuc >= 13 && loaiNgayNgoaiGio.SoGioNghiTruaNgoaiGio != null)
                                            {
                                                newDBObject.SoPhutDangKy = newDBObject.SoPhutDangKy - loaiNgayNgoaiGio.SoGioNghiTruaNgoaiGio * 60;
                                            }
                                        }
                                    }
                                    catch { }
                                    //
                                    newDBObject.TuGio = ParseTimeString(obj.GioBatDau, obj.PhutBatDau);
                                    newDBObject.DenGio = ParseTimeString(obj.GioKetThuc, obj.PhutKetThuc);
                                    newDBObject.TuGioThucTe = newDBObject.TuGio;
                                    newDBObject.DenGioThucTe = newDBObject.DenGio;
                                    //
                                    decimal soGio = newDBObject.SoPhutDangKy.Value / 60;
                                    //
                                    newDBObject.SoGioDangKy = Math.Round(soGio, 2);
                                    //
                                    newDBObject.SoPhutThucTe = newDBObject.SoPhutDangKy;
                                    newDBObject.SoGioThucTe = newDBObject.SoGioDangKy;
                                    newDBObject.SoNgayQuyDoi = Math.Round(newDBObject.SoGioThucTe.Value / 8, 1, MidpointRounding.AwayFromZero);
                                    newDBObject.NgayTao = DateTime.Now;

                                    //
                                    factory.SaveChanges();
                                    inserted++;
                                }
                            }
                            date = date.AddDays(1.0);
                        }
                        if (inserted > 0)
                            return true;
                    }
                    catch (Exception ex)
                    {
                        Helper.ErrorLog("Save_DangKyChamCongNgoaiGio", ex);
                        return false;
                    }
                }
                return false;
            }
            return false;
            //
        }
        public bool DangKyChamCongNgoaiGio_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD

            List<DTO_CC_ChamCongNgoaiGio> objList = JsonConvert.DeserializeObject<List<DTO_CC_ChamCongNgoaiGio>>(jsonObjectList);
            //
            return DangKyChamCongNgoaiGio_DeleteList(publicKey, token, objList);
        }
        public bool DangKyChamCongNgoaiGio_DeleteList(String publicKey, String token, List<DTO_CC_ChamCongNgoaiGio> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_ChamCongNgoaiGio_Factory factory = CC_ChamCongNgoaiGio_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_ChamCongNgoaiGio stupidObj = new CC_ChamCongNgoaiGio() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            //
                            CC_ChamCongNgoaiGio_Factory.FullDelete(factory.Context, stupidObj);
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

        public bool QuanLyChamCongNgoaiGio_DuyetList(String publicKey, String token, List<DTO_CC_ChamCongNgoaiGio> objList, byte trangthai, Guid userId)
        {
            CC_ChamCongNgoaiGio_Factory tmpFactory = CC_ChamCongNgoaiGio_Factory.New();
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
            {
                string idAdminToanQuyen = WebGroupConst.AdminId.ToString().ToUpper();
                string idQuanTriKhoi = WebGroupConst.QuanTriKhoiID.ToString().ToUpper();
                string idQuanTriTruong = WebGroupConst.QuanTriTruongID.ToString().ToUpper();
                string idTruongPhong = WebGroupConst.TruongPhongID.ToString().ToUpper();
                string idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID.ToString().ToUpper();
                string idBanGiamHieu = WebGroupConst.HieuTruongUyQuyenID.ToString().ToUpper();
                string idBanGiamHieuUQ = WebGroupConst.HieuTruongID.ToString().ToUpper();
                //
                WebUser user = (new WebUser_Factory()).GetByID(userId);
                if (user == null) return false;

                //lấy chức vụ kiêm nhiệm
                List<HRMWeb_Business.Model.DTO.ChucNang.ChamCong.DTO_WebGroup_BoPhan> listWebGroup_BoPhan = WebUser_BoPhan_Factory.New().GetAllWebGroupByUser(userId).ToList();
                //
                foreach (var obj in objList)
                {
                    var objFromDB = tmpFactory.GetByID(obj.Oid);
                    if (objFromDB != null)
                    {
                        //
                        foreach (var item in listWebGroup_BoPhan)
                        {
                            if (item.BoPhanIds.Contains(objFromDB.IDBoPhan.Value))
                            {
                                if (idQuanTriTruong.Equals(item.WebGroupId.ToString().ToUpper())
                                    || idQuanTriKhoi.Equals(item.WebGroupId.ToString().ToUpper())
                                    || idAdminToanQuyen.Equals(item.WebGroupId.ToString().ToUpper()))
                                {
                                    objFromDB.TrangThai_Admin = trangthai;
                                    objFromDB.NguoiDuyet_Admin = userId;
                                    objFromDB.PhanHoi_Admin = obj.PhanHoi_Admin;
                                }
                                if (idTruongPhong.Equals(item.WebGroupId.ToString().ToUpper())
                                    || idTruongPhongUyQuyen.Equals(item.WebGroupId.ToString().ToUpper()))
                                {
                                    objFromDB.TrangThai_TP = trangthai;
                                    objFromDB.NguoiDuyet_TP = userId;
                                    objFromDB.PhanHoi_TP = obj.PhanHoi_TP;
                                }
                                if (idBanGiamHieu.Equals(item.WebGroupId.ToString().ToUpper())
                                    || idBanGiamHieuUQ.Equals(item.WebGroupId.ToString().ToUpper()))
                                {
                                    objFromDB.TrangThai_BGH = trangthai;
                                    objFromDB.NguoiDuyet_BGH = userId;
                                    objFromDB.PhanHoi_BGH = obj.PhanHoi_BGH;
                                }
                            }
                        }
                    }
                    //
                    tmpFactory.SaveChangesWithoutTransactionScope();
                }
                transaction.Complete();
            }
            return false;
        }
        public bool QuanLyDangKyChamCongNgoaiGio_DuyetList_Json(String publicKey, String token, string jsonObjectList, byte trangthai, Guid userId)
        {//DANG SD

            List<DTO_CC_ChamCongNgoaiGio> objList = JsonConvert.DeserializeObject<List<DTO_CC_ChamCongNgoaiGio>>(jsonObjectList);
            return QuanLyChamCongNgoaiGio_DuyetList(publicKey, token, objList, trangthai, userId);
        }
        public bool ChotChamCongNgoaiGio_CheckLock(String publicKey, String token, int thang, int nam, Guid congTy)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = CC_QuanLyCongNgoaiGio_Factory.New();
                bool khoaCC = factory.CheckKhoaQLCCNG(thang, nam, congTy);
                return khoaCC;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool ChotCongNgoaiGioCCuoiThang(String publicKey, String token, int thang, int nam, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_QuanLyCongNgoaiGio_Factory factory = new CC_QuanLyCongNgoaiGio_Factory();
                //Cập nhật số ngày
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                {
                    try
                    {
                        //
                        factory.Context.spd_WebChamCong_ChotCongNgoaiGioCuoiThang(thang, nam, congTy);
                        transaction.Complete();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            //
            return false;
        }
        public bool XoaChotCongNgoaiGioCCuoiThang(String publicKey, String token, int thang, int nam, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_KyChamCong kyChamCong = new CC_KyChamCong_Factory().GetKyChamCong_ByThangNam(thang, nam, congTy);
                if (kyChamCong == null) return false;
                //
                try
                {
                    //
                    var factory = CC_QuanLyChamCongNhanVien_Factory.New();
                    //
                    factory.Context.spd_WebChamCong_XoaChotCongNgoaiGioCuoiThang(kyChamCong.Oid, congTy);
                    return true;
                }
                catch (Exception)
                {
                    throw new Exception(string.Format("Có lỗi khi xóa bảng chốt của tháng {0} năm {1}", kyChamCong.Thang, kyChamCong.Nam));
                }
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }

        public DateTime ChamCongNgoaiGio_NgayHopLe(String publicKey, String token, Guid congTy)
        {
            DateTime result = DateTime.MinValue;
            //
            if (Helper.TrustTest(publicKey, token))
            {
                //
                DTO_CC_CauHinhChamCong cauHinh = (new CC_CauHinhChamCong_Factory()).GetCauHinhChamCongByCongTy(congTy);
                if (cauHinh != null)
                {
                    result = DateTime.Now.Date.AddDays(cauHinh.SoNgayDangKyNgoaiGio);
                }
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
            //
            return result;
        }

        public string ChamCongNgoaiGio_NgayHopLe_Json(String publicKey, String token, Guid congTy)
        {
            //
            var obj = ChamCongNgoaiGio_NgayHopLe(publicKey, token, congTy);
            //
            String json = JsonConvert.SerializeObject(obj);
            //
            return json;
        }
    }
}
