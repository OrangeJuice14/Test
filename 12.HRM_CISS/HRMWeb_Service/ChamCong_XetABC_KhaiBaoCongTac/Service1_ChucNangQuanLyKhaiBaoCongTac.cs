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

        public IEnumerable<DTO_QuanLyKhaiBaoCongTac_Find> QuanLyKhaiBaoCongTac_Find(String publicKey, String token, DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                IEnumerable<DTO_QuanLyKhaiBaoCongTac_Find> objList = factory.QuanLyKhaiBaoCongTac_Find(tungay,denngay, boPhanId, trangThai, maNhanSu, webUserId, congTy).ToList();

                /*
                foreach (DTO_QuanLyKhaiBaoCongTac_Find cc in objList)
                {
                    cc.TuNgayString = "Từ ngày " + FormatTime(cc.TuNgay.Value.Day) + "/" + FormatTime(cc.TuNgay.Value.Month) + "/" + cc.TuNgay.Value.Year.ToString();
                    cc.DenNgayString = "Đến ngày " + FormatTime(cc.DenNgay.Value.Day) + "/" + FormatTime(cc.DenNgay.Value.Month) + "/" + cc.DenNgay.Value.Year.ToString();
                    switch (cc.Buoi)
                    {
                        case "0":
                            {
                                cc.Buoi = "Cả ngày";
                            }
                            break;
                        case "1":
                            {
                                cc.Buoi = "Buổi sáng";
                            }
                            break;
                        case "2":
                            {
                                cc.Buoi = "Buổi chiều";
                            }
                            break;
                    }
                }*/
                return objList;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public String QuanLyKhaiBaoCongTac_Find_Json(String publicKey, String token, DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy)
        {//DANG SD
            IEnumerable<DTO_QuanLyKhaiBaoCongTac_Find> list = QuanLyKhaiBaoCongTac_Find(publicKey, token, tungay,denngay, boPhanId, trangThai, maNhanSu, webUserId,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }


        public bool QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList(String publicKey, String token, List<DTO_QuanLyKhaiBaoCongTac_Find> objList, int trangThai, string userId)
        {
            try
            {
                if (Helper.TrustTest(publicKey, token))
                {
                    CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            //
                            CC_KhaiBaoCongTac objFromDB = factory.GetByID(obj.Oid);
                            if (objFromDB != null)
                            {
                                objFromDB.TrangThai = trangThai;
                                objFromDB.WebUserDuyet = new Guid(userId);
                                objFromDB.PhanHoi_NguoiDuyet = obj.PhanHoi_NguoiDuyet;
                                //////////////
                                try
                                {
                                    factory.SaveChangesWithoutTransactionScope();
                                }
                                catch { return false; }
                                //
                                if (objFromDB.TrangThai == 1)
                                {
                                    //Cập nhật hình thức nghỉ trong chấm công theo ngày là: Làm cả ngày
                                    factory.Context.spd_WebChamCong_CapNhatKhaiBaoCongTac(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, true, objFromDB.Buoi);
                                }
                                else if (objFromDB.TrangThai == 0)
                                {
                                    //Cập nhật hình thức nghỉ trong chấm công theo ngày là: Không xác định
                                    factory.Context.spd_WebChamCong_CapNhatKhaiBaoCongTac(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, false, objFromDB.Buoi);
                                }
                            }
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList", ex);
                throw ex;
            }
        }

        public bool QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList_Json(String publicKey, String token, string jsonObjectList, int trangThai, string userId)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_QuanLyKhaiBaoCongTac_Find> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyKhaiBaoCongTac_Find>>(jsonObjectList);
            return QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList(publicKey, token, objList, trangThai, userId);
        }



        public bool QuanLyKhaiBaoCongTac_DeleteList(String publicKey, String token, List<DTO_QuanLyKhaiBaoCongTac_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                    foreach (var obj in objList)
                    {
                        if (obj != null)
                        {
                            try
                            {
                                //Xóa quản lý tài liệu
                                CC_Attachments_Factory fileFatory = new CC_Attachments_Factory();
                                List<CC_Attachments> fileList = fileFatory.GetAttachmentList_By(obj.Oid).ToList();
                                foreach (var item in fileList)
                                {
                                    //
                                    fileFatory.DeleteObject(item);
                                }
                                fileFatory.SaveChanges();
                                //
                            }
                            catch (Exception) { return false; }

                            //Xóa khai báo công tác
                            CC_KhaiBaoCongTac stupidObj = new CC_KhaiBaoCongTac() { Oid = obj.Oid };
                            factory.Attach(stupidObj);
                            CC_KhaiBaoCongTac_Factory.FullDelete(factory.Context, stupidObj);

                            //Cập nhật bảng chấm công
                            CC_KhaiBaoCongTac objFromDB = CC_KhaiBaoCongTac_Factory.New().GetByID(obj.Oid);
                            factory.Context.spd_WebChamCong_CapNhatKhaiBaoCongTac(objFromDB.IDNhanVien, objFromDB.TuNgay, objFromDB.DenNgay, false, objFromDB.Buoi);
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

        public bool QuanLyKhaiBaoCongTac_DeleteListList_Json(String publicKey, String token, string jsonObjectList)
        {//DANG SD
            //chuyen jsonObject thanh object
            List<DTO_QuanLyKhaiBaoCongTac_Find> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyKhaiBaoCongTac_Find>>(jsonObjectList);
            return QuanLyKhaiBaoCongTac_DeleteList(publicKey, token, objList);
        }

        public String QuanLyKhaiBaoCongTac_DanhSachFile_Json(String publicKey, String token, Guid oid)
        {//DANG SD
            IEnumerable<DTO_DanhSachFile> list = QuanLyKhaiBaoCongTac_DanhSachFile(publicKey, token, oid);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public List<DTO_DanhSachFile> QuanLyKhaiBaoCongTac_DanhSachFile(String publicKey, String token, Guid oid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                List<DTO_DanhSachFile> filelist = factory.GetDanhSachFile_ByOidKhaiBaoCongTac(oid).ToList();
                //
                int stt = 1;
                foreach (var item in filelist)
                {
                    item.FullFileName = stt + ". " + item.Date.ToString("dd/MM/yyyy") + "_" + item.FileName;
                    //
                    stt += 1;
                }
                return filelist;
            }
            else
            {
                return null;
            }
        }
        #region Nhắc việc
        public IEnumerable<DTO_QuanLyKhaiBaoCongTac_Find> QuanLyKhaiBaoCongTac_Find_NhacViec(String publicKey, String token, DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy, bool tatCaDonChuaDuyet)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_KhaiBaoCongTac_Factory factory = CC_KhaiBaoCongTac_Factory.New();
                IEnumerable<DTO_QuanLyKhaiBaoCongTac_Find> objList = factory.QuanLyKhaiBaoCongTac_Find_NhacViec(tungay, denngay, boPhanId, trangThai, maNhanSu, webUserId, congTy, tatCaDonChuaDuyet).ToList();
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
