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
using System.Net;
using System.Net.Mail;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        public String DangKyNgayNghi_SoNgayConLai_Json(String publicKey, String token, Guid idhinhthucnghi, int nam, Guid idnhanvien)
        {
            decimal soNgayPhepConLai = DangKyNgayNghi_SoNgayConLai(publicKey, token, idhinhthucnghi,nam,idnhanvien);
            String json = JsonConvert.SerializeObject(soNgayPhepConLai);
            return json;
        }
        public decimal DangKyNgayNghi_SoNgayConLai(String publicKey, String token, Guid idhinhthucnghi, int nam, Guid idnhanvien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                decimal result = 0;
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                if (idhinhthucnghi == HinhThucNghiConst.NghiPhepId
                    || idhinhthucnghi == HinhThucNghiConst.NghiPhepBuoiSangId
                    || idhinhthucnghi == HinhThucNghiConst.NghiPhepBuoiChieuId)
                {
                    result = factory.DangKyNghiPhep_SoNgayPhepConLai(nam, idnhanvien);
                }
                //else
                //{
                //    var obj = factory.Context.spd_WebChamCong_TinhSoNgayNghiConLaiTrongNam(idhinhthucnghi, idnhanvien);
                //    if (obj != null)
                //    {
                //        result = obj.FirstOrDefault().Value;
                //    }
                //}
                return result;
            }
            else
            {
                return 0;
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


        public IEnumerable<DTO_ChamCongNgayNghi_Find> ChamCongNgayNghi_Find(String publicKey, String token, int thang, int nam, Guid? boPhanId, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu, int trangThai, bool? isAdmin)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_ChamCongNgayNghi_Find> list = null;
                //
                list = factory.FindForChamCongNgayNghi(thang, nam, boPhanId,maNhanSu, webUserId, idLoaiNhanSu,trangThai, isAdmin);
                return list;
            }
            else
            {
                return null;
            }
        }
        public bool ChamCongNgayNghi_AcceptList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList, int isAdmin)
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
                            CC_ChamCongNgayNghi objFromDB = factory.CreateAloneObject();
                            objFromDB = factory.GetByID(obj.Oid);
                            if (isAdmin == 1)
                                objFromDB.TrangThaiAdmin = 1;
                            if (isAdmin == 0)
                                objFromDB.TrangThai = 1;

                            //Dưới 2 ngày trưởng đơn vị duyệt là được
                            if ((objFromDB.SoNgay < 2 && (objFromDB.TrangThai == 1 || objFromDB.TrangThaiAdmin == 1)) || (objFromDB.TrangThai == 1 && objFromDB.TrangThaiAdmin == 1))
                            {
                                try
                                {
                                    factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatHinhThucNghi_VaTrangThaiChamCong(
                                    objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, true);
                                    factory.SaveChangesWithoutTransactionScope();
                                    transaction.Complete();
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }                             
                            }
                            factory.SaveChangesWithoutTransactionScope();
                            transaction.Complete();
                        }
                    }
                }
                return false;
            }
            return false;
        }

        public bool ChamCongNgayNghi_CancelList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList, bool isAdmin)
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
                            CC_ChamCongNgayNghi objFromDB = factory.CreateAloneObject();
                            objFromDB = factory.GetByID(obj.Oid);
                            if (isAdmin == true)
                            {
                                objFromDB.TrangThaiAdmin = 0;
                                //
                                try
                                {
                                    factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatHinhThucNghi_VaTrangThaiChamCong(
                                    objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, false);
                                    factory.SaveChangesWithoutTransactionScope();
                                    transaction.Complete();
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            if (isAdmin == false)
                            {
                                objFromDB.TrangThai = 0;

                                // Nếu số ngày nhỏ hơn 2 thì cho phép hủy (Trưởng phòng)
                                if (objFromDB.SoNgay <= 2)
                                {
                                    try
                                    {
                                        factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatHinhThucNghi_VaTrangThaiChamCong(
                                        objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, false);
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
                            factory.SaveChanges();
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
                return false;
            }
            return false;
        }
        public String ChamCongNgayNghi_Find_Json(String publicKey, String token, int thang, int nam, Guid? boPhanId, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu, int trangThai, bool? isAdmin)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = ChamCongNgayNghi_Find(publicKey, token, thang, nam, boPhanId, maNhanSu, webUserId, idLoaiNhanSu, trangThai, isAdmin);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String DangKyChamCongNgayNghi_Find_Json(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = DangKyChamCongNgayNghi_Find(publicKey, token, thang, nam, idNhanVien);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public bool ChamCongNgayNghi_AcceptList_Json(String publicKey, String token, string jsonObjectList, int isAdmin)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_AcceptList(publicKey, token, objList, isAdmin);
        }
        public bool ChamCongNgayNghi_CancelList_Json(String publicKey, String token, string jsonObjectList, bool isAdmin)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_CancelList(publicKey, token, objList, isAdmin);
        }
        public IEnumerable<DTO_ChamCongNgayNghi_Find> DangKyChamCongNgayNghi_Find(String publicKey, String token, int thang, int nam, Guid idNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_ChamCongNgayNghi_Find> list = null;


                list = factory.DangKyChamCongNgayNghi_Find(thang, nam, idNhanVien);
                return list;
            }
            else
            {
                return null;
            }
        }
        public int GetBusinessDays(DateTime fromDate, DateTime toDate)
        {
            int result = 0;
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday)
                    result++;
            }
            return result;
        }
        public DTO_CC_ChamCongNgayNghi ChamCongNgayNghi_TaoMoi(String publicKey, String token, Guid nhanVienID, String noiDung, String noiNghiPhep, String diaChiLienHe, string idHinhThucNghi, int loaiDonXinNghi, DateTime tuNgay, DateTime denNgay, Guid webUserId, bool isAdmin)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                Guid hinhThucNghiId = new Guid(idHinhThucNghi);
                //
                using (var tran = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
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
                    newObj.IDHinhThucNghi = hinhThucNghiId;
                    newObj.IDWebUser = webUserId;
                    newObj.NgayTao = DateTime.Today;
                    newObj.NoiNghi = noiNghiPhep;
                    newObj.DiaChiLienHe = diaChiLienHe;
                    newObj.LoaiNghiPhep = loaiDonXinNghi;
                    if (newObj.LoaiNghiPhep == 0)
                    {
                        newObj.TenGiayXinPhep = "NGHỈ PHÉP NĂM";
                    }
                    else if (newObj.LoaiNghiPhep == 1)
                    {
                        newObj.TenGiayXinPhep = "NGHỈ PHÉP NĂM Ở NƯỚC NGOÀI";
                    }
                    else if (newObj.LoaiNghiPhep == 2)
                    {
                        newObj.TenGiayXinPhep = newObj.HinhThucNghi1.TenHinhThucNghi.ToUpper();
                    }
                    else if (newObj.LoaiNghiPhep == 1)
                    {
                        newObj.TenGiayXinPhep = "NGHỈ VIỆC RIÊNG";
                    }
                    newObj.TrangThai = -1;
                    newObj.TrangThaiAdmin = -1;
                    newObj.SoNgay = GetBusinessDays(tuNgay, denNgay);
                    if (newObj.IDHinhThucNghi == HinhThucNghiConst.NghiPhepBuoiSangId
                        || newObj.IDHinhThucNghi == HinhThucNghiConst.NghiPhepBuoiChieuId)
                    {
                        newObj.SoNgay = newObj.SoNgay.Value / 2;
                    }
                    try
                    {
                        if (isAdmin)
                        {
                            newObj.TrangThai = 1;
                            newObj.TrangThaiAdmin = 1;
                            factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatHinhThucNghi_VaTrangThaiChamCong(
                                      newObj.IDNhanVien, newObj.TuNgay, newObj.DenNgay, hinhThucNghiId, true);
                        }
                        factory.SaveChangesWithoutTransactionScope();

                        if (!isAdmin)
                        {
                            try
                            {
                                //Gửi email thông báo cho trưởng đơn vị
                                var emailTruongDonVi = hoSo_Factory.GetTruongDonViByBoPhan(hoSo.NhanVien.BoPhan.Value)?.Email;
                                string tieudeguimail = "DUYỆT " + newObj.HinhThucNghi1.TenHinhThucNghi.ToUpper();
                                string noidungguimail = "Họ tên: [" + newObj.ThongTinNhanVien.NhanVien.HoSo.HoTen.ToUpper() + "] Đơn vị: [" + newObj.BoPhan.TenBoPhan.ToUpper() + "] xin nghỉ: [" + newObj.HinhThucNghi1.TenHinhThucNghi + "] Từ ngày [" + String.Format("{0:dd/MM/yyyy}", newObj.TuNgay) + "] Đến ngày [" + String.Format("{0:dd/MM/yyyy}", newObj.DenNgay) + "]";
                                noidungguimail = noidungguimail + "<br><a href='http://kpis.hcmute.edu.vn/kpi/chamcongngaynghi' target='_blank'>Duyệt</a>";
                                QuanLyGuiEmail_SendMail("kpis@hcmute.edu.vn", "ute@2016", emailTruongDonVi, tieudeguimail, noidungguimail);
                            }
                            catch (Exception ex)
                            {
                                Helper.ErrorLog("Service1_ChucNangChamCongNgayNghi/ChamCongNgayNghi_TaoMoi/Gui email thong bao cho truong don vi", ex);
                            }
                        }
                        tran.Complete();

                        var returnObj = newObj.Map<DTO_CC_ChamCongNgayNghi>();
                        return returnObj;
                    }
                    catch (Exception ex)
                    {
                        Helper.ErrorLog("Service1_ChucNangChamCongNgayNghi/ChamCongNgayNghi_TaoMoi", ex);
                        throw ex;
                    }
                }
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
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

                            CC_ChamCongNgayNghi objFromDB = CC_ChamCongNgayNghi_Factory.New().GetByID(obj.Oid);
                            if (objFromDB.TrangThai == 1 && objFromDB.TrangThaiAdmin == 1)
                            {
                                factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatTrangThaiChamCong(
                                    objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, false);
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
                return false;
            }
            return false;
        }

        public bool ChamCongNgayNghi_DeleteList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_DeleteList(publicKey, token, objList);
        }


        public bool ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, Guid? chamCongNgayNghiOid, DateTime tuNgay, DateTime denNgay, Guid nhanVienID)
        {//DANG SD
            tuNgay = tuNgay.Date;
            denNgay = denNgay.Date;
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                bool isNew = true;
                if (chamCongNgayNghiOid != null)
                {
                    var objFromDb = factory.GetByID(chamCongNgayNghiOid.Value);
                    if (objFromDb != null)
                        isNew = false;
                }
                //bat dau kiem tra hop le
                var trungHoacGiaoNgay_ChamCongNgayNghi = (from o in factory.Context.CC_ChamCongNgayNghi
                                                          where o.IDNhanVien == nhanVienID
                                                             && (isNew || o.Oid != chamCongNgayNghiOid)
                                                              && (
                                                                 (o.TuNgay <= tuNgay && tuNgay <= o.DenNgay)
                                                                 ||
                                                                 (o.TuNgay <= denNgay && denNgay <= o.DenNgay)
                                                              )

                                                          select true).FirstOrDefault();

                var trungHoacGiaoNgay_KhaiBaoCongTac = (from o in factory.Context.CC_KhaiBaoCongTac
                                                        where o.IDNhanVien == nhanVienID
                                                            && o.TrangThai == 1
                                                           && (isNew || o.Oid != chamCongNgayNghiOid)
                                                            && (
                                                               (o.TuNgay <= tuNgay && tuNgay <= o.DenNgay)
                                                               ||
                                                               (o.TuNgay <= denNgay && denNgay <= o.DenNgay)
                                                            )

                                                        select true).FirstOrDefault();
                var hopLe = (!trungHoacGiaoNgay_ChamCongNgayNghi && !trungHoacGiaoNgay_KhaiBaoCongTac);
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

                            objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "IDHinhThucNghi", "TuNgay", "DenNgay", "DienGiai","DaInNghiPhepNam" });
                            objFromDB.TuNgay = objFromDB.TuNgay.Value.Date;
                            objFromDB.DenNgay = objFromDB.DenNgay.Value.Date;

                            factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatHinhThucNghi_VaTrangThaiChamCong(
                                objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, true);
                            try
                            {

                                factory.SaveChangesWithoutTransactionScope();
                                tran.Complete();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                return false;
                            }
                        }
                        //////////////

                    }
                }
                return false;
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

                            CC_ChamCongNgayNghi objFromDB = factory.GetByID(obj.Oid);
                            if (objFromDB != null)
                            {
                                objFromDB.CopyIncludedPropertiesFrom(obj,
                                    new[] { "Oid", "IDHinhThucNghi", "TuNgay", "DenNgay", "DienGiai" });
                                objFromDB.TuNgay = objFromDB.TuNgay.Value.Date;
                                objFromDB.DenNgay = objFromDB.DenNgay.Value.Date;
                                factory.Context
                                    .spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatHinhThucNghi_VaTrangThaiChamCong(
                                        objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay,
                                        objFromDB.IDHinhThucNghi, true);
                            }

                        }
                    }
                    //////////////
                    try
                    {

                        factory.SaveChangesWithoutTransactionScope();
                        tran.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                return false;
            }
            return false;
        }
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
                obj.ChucDanh = obj.ChucVu + " / " +obj.ChucDanh;
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
        public bool ChamCongNgayNghi_SaveList_Json(String publicKey, String token, string jsonObjectList)
        {
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_SaveList(publicKey, token, objList);
        }
        #endregion

        #region Gửi mail
        public bool QuanLyGuiEmail_SendMail(string emailgui, string passgui, string emailnhan, string tieude, string noidung)
        {
            //
            try
            {   //
                bool sucess = false;
                //
                if (!string.IsNullOrEmpty(emailnhan))
                {
                    //
                    sucess = SendMail(tieude, noidung, emailgui, passgui, emailnhan);
                }
                return sucess;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("Service1_ChucNangChamCongNgayNghi/QuanLyGuiEmail_SendMail", ex);
                return false;
            }
        }
        private bool SendMail(string tieude, string noidung, string emailgui, string passgui, string emailnhan)
        {
            bool sucess = false;
            //
            var loginInfo = new NetworkCredential(emailgui, passgui);
            var msg = new System.Net.Mail.MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            //
            msg.To.Add(new MailAddress(emailnhan));
            //
            msg.From = new MailAddress(emailgui);
            //msg.To.Add(new MailAddress(tags.Text));
            msg.Subject = tieude;
            msg.Body = noidung;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            //smtpClient.Send(msg);
            try
            {
                smtpClient.Send(msg);
                sucess = true;
            }
            catch (Exception ex)
            {//
                sucess = false;
                Helper.ErrorLog("Service1_ChucNangChamCongNgayNghi/SendMail", ex);
            }
            //
            return sucess;
        }
        #endregion
    }
}
