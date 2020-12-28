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

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
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


        public IEnumerable<DTO_ChamCongNgayNghi_Find> ChamCongNgayNghi_Find(String publicKey, String token, int thang, int nam, Guid? boPhanId, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_ChamCongNgayNghi_Find> list = null;


                list = factory.FindForChamCongNgayNghi(thang, nam, boPhanId,
                      maNhanSu, webUserId, idLoaiNhanSu);
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
                            

                            //sửa lại đăng ký nghỉ phép chỉ cần trưởng phòng duyệt
                            //trưởng phòng nghỉ thì BGH duyệt
                            //if (objFromDB.TrangThai == 1 && objFromDB.TrangThaiAdmin == 1)
                            if (objFromDB.TrangThai != 1)
                            {
                                try
                                {
                                    //factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatHinhThucNghi_VaTrangThaiChamCong(
                                    //objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, objFromDB.IDHinhThucNghi, true);
                                    if (isAdmin == 1)
                                        objFromDB.TrangThaiAdmin = 1;
                                    if (isAdmin == 0)
                                        objFromDB.TrangThai = 1;
                                    factory.SaveChanges();

                                    factory.Context.spd_WebChamCong_CC_ChamCongNgayNghi_Duyet(objFromDB.Oid);
                                    factory.Context.spd_WebChamCong_CC_ChamCongNgayNghi_QuanLyNghiPhep_Duyet(objFromDB.Oid);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }                             
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

                            if (objFromDB.TrangThai != 0)
                            {
                                try
                                {
                                    if (isAdmin == true)
                                        objFromDB.TrangThaiAdmin = 0;
                                    if (isAdmin == false)
                                        objFromDB.TrangThai = 0;
                                    factory.SaveChanges();

                                    factory.Context.spd_WebChamCong_CC_ChamCongNgayNghi_HuyDuyet(objFromDB.Oid);
                                    factory.Context.spd_WebChamCong_CC_ChamCongNgayNghi_QuanLyNghiPhep_HuyDuyet(objFromDB.Oid);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
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
        public String ChamCongNgayNghi_Find_Json(String publicKey, String token, int thang, int nam, Guid? boPhanId, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = ChamCongNgayNghi_Find(publicKey, token, thang, nam, boPhanId, maNhanSu, webUserId, idLoaiNhanSu);
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
        public bool KiemTraSoNgayDangKyNghi(Guid idHinhThucNghi, DateTime tuNgay, DateTime denNgay, Guid buoiTuNgay, Guid buoiDenNgay)
        {
            var soNgayToiDa = HinhThucNghi_Factory.New().GetByID(idHinhThucNghi).SoNgayToiDa;
            if (soNgayToiDa > 0)
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                var soNgay = factory.LaySoNgayDangKyNghi(tuNgay, denNgay, buoiTuNgay, buoiDenNgay);
                if (soNgay > soNgayToiDa)
                    return false;
            }

            return true;
        }
        public bool KiemTraSoNgayDiDuong(decimal soNgayDiDuong, DateTime tuNgay, DateTime denNgay, Guid buoiTuNgay, Guid buoiDenNgay)
        {
            CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
            var soNgay = factory.LaySoNgayDangKyNghi(tuNgay, denNgay, buoiTuNgay, buoiDenNgay);
            if (soNgay - soNgayDiDuong < 0)
                return false;

            return true;
        }
        public bool KiemTraSoNgayDangKyNghiPhep(Guid idHinhThucNghi, DateTime tuNgay, DateTime denNgay, Guid buoiTuNgay, Guid buoiDenNgay, decimal soNgayToiDa, decimal soNgayDiDuong)
        {
            if (soNgayToiDa > 0)
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                var soNgay = factory.LaySoNgayDangKyNghi(tuNgay, denNgay, buoiTuNgay, buoiDenNgay);
                soNgay = soNgay - soNgayDiDuong;
                if (soNgay > soNgayToiDa)
                    return false;
            }

            return true;
        }
        public DTO_CC_ChamCongNgayNghi ChamCongNgayNghi_TaoMoi(String publicKey, String token, Guid nhanVienID, String noiDung, String noiNghiPhep,String tenDonXinNghi, String diaChiLienHe, Guid idHinhThucNghi, int loaiDonXinNghi, DateTime tuNgay, DateTime denNgay, Guid tinhThanh, bool truNgayDiDuong, int soNgayDiDuong, Guid webUserId, Guid buoiTuNgay, Guid buoiDenNgay, decimal? soNgayPhepConLai, bool isAdmin)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
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
                    newObj.IDHinhThucNghi = idHinhThucNghi;
                    newObj.IDWebUser = webUserId;
                    newObj.NgayTao = DateTime.Now;
                    newObj.LoaiNghiPhep = loaiDonXinNghi;
                    newObj.NoiNghi = noiNghiPhep;
                    newObj.DiaChiLienHe = diaChiLienHe;
                    newObj.TrangThai = -1;
                    newObj.TrangThaiAdmin = -1;
                    newObj.TenGiayXinPhep = tenDonXinNghi;
                    newObj.CacBuoiTrongNgay_TuNgay = buoiTuNgay;
                    newObj.CacBuoiTrongNgay_DenNgay = buoiDenNgay;
                    newObj.SoNgayPhepConLai = soNgayPhepConLai;

                    //trừ 2 ngày thứ 7 và chủ nhật, trừ số ngày đi đường nếu có
                    //newObj.SoNgay = GetBusinessDays(tuNgay, denNgay);
                    newObj.SoNgay = factory.LaySoNgayDangKyNghi(tuNgay, denNgay, buoiTuNgay, buoiDenNgay);
                    if (truNgayDiDuong == true)
                    {
                        newObj.SoNgay = newObj.SoNgay - soNgayDiDuong;
                        newObj.SoNgayDiDuong = soNgayDiDuong;
                    }


                    if (tinhThanh == Guid.Empty)
                    {
                        newObj.TinhThanh = null;
                    }
                    else newObj.TinhThanh = tinhThanh;
                    newObj.TruNgayDiDuong = truNgayDiDuong;
                    try
                    {
                        //if (isAdmin)
                        //{
                        //    newObj.TrangThai = 1;
                        //    newObj.TrangThaiAdmin = 1;
                        //    factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatHinhThucNghi_VaTrangThaiChamCong(
                        //              newObj.IDNhanVien, newObj.TuNgay, newObj.DenNgay, idHinhThucNghi, true);
                        //}

                        factory.SaveChangesWithoutTransactionScope();
                        tran.Complete();
                        var returnObj = newObj.Map<DTO_CC_ChamCongNgayNghi>();
                        return returnObj;
                    }
                    catch (Exception ex)
                    {
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
                            CC_ChamCongNgayNghi objFromDB = CC_ChamCongNgayNghi_Factory.New().GetByID(obj.Oid);
                            //nếu trạng thái chờ xét hoặc không chấp nhận thì được phép xóa
                            if (objFromDB.TrangThai != 1)
                            {
                                CC_ChamCongNgayNghi stupidObj = new CC_ChamCongNgayNghi() { Oid = obj.Oid };
                                factory.Attach(stupidObj);
                                CC_ChamCongNgayNghi_Factory.FullDelete(factory.Context, stupidObj);

                                if (objFromDB.TrangThai == 1 && objFromDB.TrangThaiAdmin == 1)
                                {
                                    factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatTrangThaiChamCong(
                                        objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, false);
                                }
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

        public int ChamCongNgayNghi_LaySoDangKyNghiDangChoXet(String publicKey, String token, int thang, int nam, Guid boPhanId, Guid webUserId)
        {
            var result = 0;
            if (Helper.TrustTest(publicKey, token))
            {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                result = factory.LaySoDangKyNghiDangChoXet(thang, nam, boPhanId, null, webUserId);
            }

            return result;
        }

        public String ChamCongNgayNghi_CacBuoiTrongNgay_Json(String publicKey, String token)
        {
            IEnumerable<DTO_CacBuoiTrongNgay> list = ChamCongNgayNghi_CacBuoiTrongNgay(publicKey, token);
            var json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_CacBuoiTrongNgay> ChamCongNgayNghi_CacBuoiTrongNgay(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_CacBuoiTrongNgay> list = null;


                list = factory.CacBuoiTrongNgay().Map<DTO_CacBuoiTrongNgay>();
                return list;
            }
            else
            {
                return null;
            }
        }

        #region SAVE
        //kiem tra duoc phep luu

        // Save////////////////////////////////////////////
        public bool ChamCongNgayNghi_Save(String publicKey, String token, DTO_ChamCongNgayNghi_Find obj)
        {//KHONG SU DUNG NUA
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

                            objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "IDHinhThucNghi", "TuNgay", "DenNgay", "DienGiai" , "SoNgayDiDuong"});
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
        {//KHONG SU DUNG NUA
            //chuyen jsonObject thanh object
            DTO_ChamCongNgayNghi_Find obj = JsonConvert.DeserializeObject<DTO_ChamCongNgayNghi_Find>(jsonObject);
            return ChamCongNgayNghi_Save(publicKey, token, obj);
        }

        public bool ChamCongNgayNghi_SaveList(String publicKey, String token, List<DTO_ChamCongNgayNghi_Find> objList)
        {//KHONG SU DUNG NUA
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
        {//KHONG SU DUNG NUA
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_SaveList(publicKey, token, objList);
        }
        #endregion
    }
}
