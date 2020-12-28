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
        public DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_Report(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                DTO_ChamCongNgayNghi_Find obj = null;
                obj = factory.ChamCongNgayNghi_Report(id);
                obj.TuNgayString = String.Format("{0:dd/MM/yyyy}", obj.TuNgay);
                obj.DenNgayString = String.Format("{0:dd/MM/yyyy}", obj.DenNgay);
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
        public String ChamCongNgayNghi_Report_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_ChamCongNgayNghi_Find obj = ChamCongNgayNghi_Report(publicKey, token, id);
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
        public IEnumerable<DTO_TinhThanh> GetList_TinhThanh()
        {
                CC_ChamCongNgayNghi_Factory factory = CC_ChamCongNgayNghi_Factory.New();
                IEnumerable<DTO_TinhThanh> list = null;
                list = factory.GetList_TinhThanh();
                return list;
        }
        public String ChamCongNgayNghi_Find_Json(String publicKey, String token, int thang, int nam, Guid? boPhanId, String maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {//DANG SD
            IEnumerable<DTO_ChamCongNgayNghi_Find> list = ChamCongNgayNghi_Find(publicKey, token, thang, nam, boPhanId, maNhanSu, webUserId, idLoaiNhanSu);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String GetList_TinhThanh_Json()
        {//DANG SD
            IEnumerable<DTO_TinhThanh> list = GetList_TinhThanh();
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public DTO_CC_ChamCongNgayNghi ChamCongNgayNghi_TaoMoi(String publicKey, String token, Guid nhanVienID, String noiDung, Guid idHinhThucNghi, DateTime tuNgay, DateTime denNgay, Guid webUserId,Guid tinhThanh,int soNgay,int ngayDiDuong)
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
                    newObj.TinhThanh=tinhThanh;
                    newObj.SoNgayNghiPhepNamTruoc = soNgay;
                    newObj.TruNgayPhepDiDuong = ngayDiDuong;
                    newObj.DienGiai = noiDung.Trim();
                    newObj.IDBoPhan = hoSo.NhanVien.Department;
                    newObj.IDNhanVien = nhanVienID;
                    newObj.IDHinhThucNghi = idHinhThucNghi;
                    newObj.IDWebUser = webUserId;
                    newObj.NgayTao = DateTime.Today;
                    try
                    {
                        factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatHinhThucNghi_VaTrangThaiChamCong(
                                  newObj.IDNhanVien, newObj.TuNgay, newObj.DenNgay, idHinhThucNghi, true);
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


                            CC_ChamCongNgayNghi stupidObj = new CC_ChamCongNgayNghi() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_ChamCongNgayNghi_Factory.FullDelete(factory.Context, stupidObj);

                            CC_ChamCongNgayNghi objFromDB = CC_ChamCongNgayNghi_Factory.New().GetByID(obj.Oid);
                            factory.Context.spd_WebChamCong_CC_ChamCongTheoNgay_CapNhatTrangThaiChamCong(
                                objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, false);


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

                            objFromDB.CopyIncludedPropertiesFrom(obj, new[] { "Oid", "IDHinhThucNghi", "TuNgay", "DenNgay", "DienGiai" });
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
        public bool ChamCongNgayNghi_SaveList_Json(String publicKey, String token, string jsonObjectList)
        {
            //chuyen jsonObject thanh object
            List<DTO_ChamCongNgayNghi_Find> objList = JsonConvert.DeserializeObject<List<DTO_ChamCongNgayNghi_Find>>(jsonObjectList);
            return ChamCongNgayNghi_SaveList(publicKey, token, objList);
        }
        #endregion
    }
}
