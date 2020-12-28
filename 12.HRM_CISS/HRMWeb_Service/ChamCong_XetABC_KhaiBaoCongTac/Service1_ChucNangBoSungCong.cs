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
using System.IO;
using System.Diagnostics;
using System.Web;
using HRMWeb_Business.Predefined;
using ERP_Core.Common;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        public IEnumerable<DTO_CC_BoSungCong> CaNhanBoSungCong_Find(String publicKey, String token, DateTime tungay, DateTime denngay, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_BoSungCong_Factory factory = CC_BoSungCong_Factory.New();
                var objList = factory.CaNhanBoSungCong_Find(tungay, denngay, webUserId).ToList();

                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String CaNhanBoSungCong_Find_Json(String publicKey, String token, DateTime tungay, DateTime denngay, Guid webUserId)
        {//DANG SD
            IEnumerable<DTO_CC_BoSungCong> list = CaNhanBoSungCong_Find(publicKey, token, tungay, denngay, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public bool CaNhanBoSungCong_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, Guid? boSungCongOid, DateTime tuNgay, DateTime denNgay, Guid webUserId, string IDNhanVien, int buoi)
        {//DANG SD
            tuNgay = tuNgay.Date;
            denNgay = denNgay.Date;
            if (Helper.TrustTest(publicKey, token))
            {
                if (string.IsNullOrEmpty(IDNhanVien))
                {
                    WebUser_Factory webUser_Factory = WebUser_Factory.New();
                    WebUser webUser = webUser_Factory.GetByID(webUserId);

                    CC_BoSungCong_Factory factory = CC_BoSungCong_Factory.New();

                    //bat dau kiem tra hop le
                    var trungHoacGiaoNgayBoSungCong = (from o in factory.Context.CC_BoSungCong
                                                       where o.IDNhanVien == webUser.ThongTinNhanVien
                                                              && (tuNgay <= o.DenNgay && denNgay >= o.TuNgay)
                                                              && (o.Buoi == buoi || buoi == 1)
                                                       select true).FirstOrDefault();

                    var trungHoacGiaoNgayKhaiBaoCongTac = (from o in factory.Context.CC_KhaiBaoCongTac
                                                           where o.IDNhanVien == webUser.ThongTinNhanVien
                                                                  && (tuNgay <= o.DenNgay && denNgay >= o.TuNgay)
                                                                  && (o.Buoi == buoi || buoi == 1)
                                                           select true).FirstOrDefault();

                    var trungHoacGiaoNgay_ChamCongNgayNghi = (from o in factory.Context.CC_ChamCongNgayNghi
                                                              where o.IDNhanVien == webUser.ThongTinNhanVien
                                                                  && (tuNgay <= o.DenNgay && denNgay >= o.TuNgay)
                                                                  && (o.Buoi == buoi || buoi == 1)
                                                              select true).FirstOrDefault();
                    var hopLe = false;
                    if (trungHoacGiaoNgayBoSungCong || trungHoacGiaoNgayKhaiBaoCongTac || trungHoacGiaoNgay_ChamCongNgayNghi)
                        hopLe = true;

                    return hopLe;
                }
                else
                {
                    Guid id = new Guid(IDNhanVien);
                    CC_BoSungCong_Factory factory = CC_BoSungCong_Factory.New();

                    //bat dau kiem tra hop le
                    var trungHoacGiaoNgayBoSungCong = (from o in factory.Context.CC_BoSungCong
                                                       where o.IDNhanVien == id
                                                              && (tuNgay <= o.DenNgay && denNgay >= o.TuNgay)
                                                              && (o.Buoi == buoi || buoi == 1)
                                                       select true).FirstOrDefault();

                    var trungHoacGiaoNgayKhaiBaoCongTac = (from o in factory.Context.CC_KhaiBaoCongTac
                                                           where o.IDNhanVien == id
                                                                  && (tuNgay <= o.DenNgay && denNgay >= o.TuNgay)
                                                                  && (o.Buoi == buoi || buoi == 1)
                                                           select true).FirstOrDefault();

                    var trungHoacGiaoNgay_ChamCongNgayNghi = (from o in factory.Context.CC_ChamCongNgayNghi
                                                              where o.IDNhanVien == id
                                                                  && (tuNgay <= o.DenNgay && denNgay >= o.TuNgay)
                                                                  && (o.Buoi == buoi || buoi == 1)
                                                              select true).FirstOrDefault();

                    var hopLe = false;
                    if (trungHoacGiaoNgayBoSungCong || trungHoacGiaoNgayKhaiBaoCongTac || trungHoacGiaoNgay_ChamCongNgayNghi)
                        hopLe = true;

                    return hopLe;
                }
            }
            return false;
        }


        public bool CaNhanBoSungCong_KhaiBaoMoi(String publicKey, String token, DateTime tuNgay, DateTime denNgay, byte buoi, string lyDo, Guid webUserId, string IDNhanVien, Guid congTy)
        {
            try
            {
                if (Helper.TrustTest(publicKey, token))
                {
                    Guid idCaNhan = WebGroupConst.TaiKhoanCaNhanID;
                    Guid idYersin = BoPhanConst.YerSinGuid;
                    //
                    WebUser_Factory webUser_Factory = WebUser_Factory.New();
                    WebUser webUser = webUser_Factory.GetByID(webUserId);

                    if (string.IsNullOrEmpty(IDNhanVien))
                    {
                        //
                        CC_BoSungCong_Factory factory = CC_BoSungCong_Factory.New();
                        CC_BoSungCong newObj = factory.CreateManagedObject();
                        newObj.Oid = Guid.NewGuid();
                        newObj.TuNgay = tuNgay;
                        newObj.DenNgay = denNgay;
                        newObj.Buoi = buoi;
                        newObj.LyDo = lyDo;
                        newObj.IDNhanVien = webUser.ThongTinNhanVien.Value;
                        newObj.NguoiTao = webUserId;
                        newObj.NgayTao = DateTime.Now;

                        if (idCaNhan.Equals(webUser.WebGroupID)) // Cá nhân phải đợi duyệt
                        {
                            newObj.TrangThai = -1;//Trạng thái chờ
                        }
                        else
                        {
                            newObj.TrangThai = 1;
                        }

                        factory.SaveChanges();

                        //Cập nhật hình thức nghỉ trong chấm công theo ngày là: Làm cả ngày
                        if (newObj.TrangThai == 1)
                        {
                            //Cập nhật hình thức nghỉ trong chấm công theo ngày là: Làm cả ngày
                            factory.Context.spd_WebChamCong_CapNhatBoSungCong(newObj.IDNhanVien, newObj.TuNgay, newObj.DenNgay, Convert.ToByte(newObj.Buoi), true);
                        }

                        //
                        if (idCaNhan.Equals(webUser.WebGroupID))
                        {
                            //Gửi đến trưởng phòng
                            DTO_CC_CauHinhChamCong cauHinh = (new CC_CauHinhChamCong_Factory()).GetCauHinhChamCongByCongTy(congTy);
                            if (cauHinh != null)
                            {
                                BoSungCong_SetDataSendMail(newObj, cauHinh.EmailSender, cauHinh.PassSender, webUser.EmailTP, webUser.Oid);
                            }
                        }
                        //
                        return true;
                    }
                    else
                    {
                        using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
                        {
                            HoSo hoSo = HoSo_Factory.New().GetByID(new Guid(IDNhanVien));
                            CC_BoSungCong_Factory factory = CC_BoSungCong_Factory.New();
                            CC_BoSungCong newObj = factory.CreateManagedObject();
                            newObj.Oid = Guid.NewGuid();
                            newObj.TuNgay = tuNgay;
                            newObj.DenNgay = denNgay;
                            newObj.Buoi = buoi;
                            newObj.LyDo = lyDo;
                            newObj.IDNhanVien = new Guid(IDNhanVien);
                            newObj.NguoiTao = webUserId;
                            newObj.NgayTao = DateTime.Now;
                            newObj.TrangThai = 1;

                            factory.SaveChanges();

                            //Cập nhật hình thức nghỉ trong chấm công theo ngày là: Làm cả ngày
                            factory.Context.spd_WebChamCong_CapNhatBoSungCong(newObj.IDNhanVien, newObj.TuNgay, newObj.DenNgay, Convert.ToByte(newObj.Buoi), true);

                            transaction.Complete();
                        }
                        //
                        return true;
                    }
                }
                else
                {
                    throw new Exception("Chứng thực không hợp lệ");
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CaNhanBoSungCong_KhaiBaoMoi", ex);
                throw ex;
            }
        }
        public void BoSungCong_SetDataSendMail(CC_BoSungCong newObj, string emailSender, string passSender, string emailReceiver, Guid user)
        {
            string tieudeguimail = "DUYỆT BỔ SUNG NGÀY CÔNG";
            string noidungguimail = "Họ tên: [" + newObj.HoSo.HoTen.ToUpper() + "] Đơn vị: [" + newObj.HoSo.NhanVien.BoPhan1.TenBoPhan.ToUpper() + "] đăng ký bổ sung ngày công Từ ngày [" + String.Format("{0:dd/MM/yyyy}", newObj.TuNgay) + "] Đến ngày [" + String.Format("{0:dd/MM/yyyy}", newObj.DenNgay) + "]";
            //Gửi mail
            if (!string.IsNullOrEmpty(emailSender) && !string.IsNullOrEmpty(passSender))
                QuanLyGuiEmail_SendMail(emailSender, passSender, emailReceiver, tieudeguimail, noidungguimail, user);

        }

        public bool CaNhanBoSungCong_DeleteList(String publicKey, String token, List<DTO_CC_BoSungCong> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(360)))
                {
                    CC_BoSungCong_Factory factory = CC_BoSungCong_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_BoSungCong stupidObj = new CC_BoSungCong() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_BoSungCong_Factory.FullDelete(factory.Context, stupidObj);
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

        public bool CaNhanBoSungCong_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_CC_BoSungCong> objList = JsonConvert.DeserializeObject<List<DTO_CC_BoSungCong>>(jsonObjectList);
            return CaNhanBoSungCong_DeleteList(publicKey, token, objList);
        }

        #region Quản lý bổ sung công

        public IEnumerable<DTO_QuanLyBoSungCong_Find> QuanLyBoSungCong_Find(String publicKey, String token, DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_BoSungCong_Factory factory = CC_BoSungCong_Factory.New();
                IEnumerable<DTO_QuanLyBoSungCong_Find> objList = factory.QuanLyBoSungCong_Find(tungay, denngay, boPhanId, trangThai, maNhanSu, webUserId, congTy).ToList();
                
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }

        public String QuanLyBoSungCong_Find_Json(String publicKey, String token, DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy)
        {//DANG SD
            IEnumerable<DTO_QuanLyBoSungCong_Find> list = QuanLyBoSungCong_Find(publicKey, token, tungay, denngay, boPhanId, trangThai, maNhanSu, webUserId, congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public bool QuanLyBoSungCong_ThayDoiTrangThaiList(String publicKey, String token, List<DTO_QuanLyBoSungCong_Find> objList, int trangThai, string userId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_BoSungCong_Factory factory = CC_BoSungCong_Factory.New();
                foreach (var obj in objList)
                {
                    if (obj != null)
                    {
                        //
                        CC_BoSungCong objFromDB = factory.GetByID(obj.Oid);
                        if (objFromDB != null)
                        {
                            objFromDB.TrangThai = trangThai;
                            objFromDB.NguoiDuyet = new Guid(userId);
                            objFromDB.PhanHoi_NguoiDuyet = obj.PhanHoi_NguoiDuyet;
                            //////////////
                            try
                            {
                                factory.SaveChangesWithoutTransactionScope();
                            }
                            catch { return false; }

                            if (objFromDB.TrangThai == 1)
                            {
                                //Cập nhật hình thức nghỉ trong chấm công theo ngày là: Làm cả ngày
                                factory.Context.spd_WebChamCong_CapNhatBoSungCong(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, Convert.ToByte(objFromDB.Buoi), true);
                            }
                            else if (objFromDB.TrangThai == 0)
                            {
                                //Cập nhật hình thức nghỉ trong chấm công theo ngày là: Không xác định
                                factory.Context.spd_WebChamCong_CapNhatBoSungCong(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, Convert.ToByte(objFromDB.Buoi), false);
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public bool QuanLyBoSungCong_ThayDoiTrangThaiList_Json(String publicKey, String token, string jsonObjectList, int trangThai, string userId)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_QuanLyBoSungCong_Find> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyBoSungCong_Find>>(jsonObjectList);
            return QuanLyBoSungCong_ThayDoiTrangThaiList(publicKey, token, objList, trangThai, userId);
        }
        
        public bool QuanLyBoSungCong_DeleteList(String publicKey, String token, List<DTO_QuanLyBoSungCong_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_BoSungCong_Factory factory = CC_BoSungCong_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            CC_BoSungCong stupidObj = new CC_BoSungCong() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_BoSungCong_Factory.FullDelete(factory.Context, stupidObj);

                            //Cập nhật bảng chấm công
                            CC_BoSungCong objFromDB = CC_BoSungCong_Factory.New().GetByID(obj.Oid);
                            factory.Context.spd_WebChamCong_CapNhatBoSungCong(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, Convert.ToByte(objFromDB.Buoi), false);
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
                        return false;
                    }
                }
            }
            return false;
        }

        public bool QuanLyBoSungCong_DeleteListList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_QuanLyBoSungCong_Find> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyBoSungCong_Find>>(jsonObjectList);
            return QuanLyBoSungCong_DeleteList(publicKey, token, objList);
        }

        public IEnumerable<DTO_QuanLyBoSungCong_Find> QuanLyBoSungCong_Find_NhacViec(String publicKey, String token, DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy, bool tatCaDonChuaDuyet)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_BoSungCong_Factory factory = CC_BoSungCong_Factory.New();
                IEnumerable<DTO_QuanLyBoSungCong_Find> objList = factory.QuanLyBoSungCong_Find_NhacViec(tungay, denngay, boPhanId, trangThai, maNhanSu, webUserId, congTy, tatCaDonChuaDuyet).ToList();
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        #endregion
    }
}
