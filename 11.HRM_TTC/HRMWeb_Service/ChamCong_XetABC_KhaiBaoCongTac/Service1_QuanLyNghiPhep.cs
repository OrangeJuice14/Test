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

        public String QuanLyNghiPhepNam_GetByID_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_QuanLyNghiPhep_Find obj = QuanLyNghiPhepNam_GetByID(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_QuanLyNghiPhep_Find QuanLyNghiPhepNam_GetByID(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                CC_QuanLyNghiPhep_Factory factory = CC_QuanLyNghiPhep_Factory.New();
                DTO_QuanLyNghiPhep_Find obj = null;
                obj = factory.QuanLyNghiPhepNam_ByID(id);
                return obj;
            }
            else
            {
                return null;
            }
        }
        public bool QuanLyNghiPhep_CheckExists(String publicKey, String token, Guid nienDoTaiChinh, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                var factory = CC_QuanLyNghiPhep_Factory.New();
                bool daTonTai = factory.CheckExistChiTietNghiPhep(nienDoTaiChinh, congTy);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }

        public bool QuanLyNghiPhepNam_SaveList(String publicKey, String token, Guid Oid, string TongSoNgayPhep, string SoNgayPhepCongThem, string SoNgayPhepNamTruoc, string SoNgayTamUngHienTai)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_QuanLyNghiPhep_Factory factory = CC_QuanLyNghiPhep_Factory.New();
                //
                CC_ChiTietNghiPhep objFromDB = factory.GetChiTietNghiPhepByOid(Oid);
                if (objFromDB != null)
                {
                    //
                    if(!string.IsNullOrEmpty(TongSoNgayPhep))
                    objFromDB.TongSoNgayPhep = Convert.ToDecimal(TongSoNgayPhep);
                    if (!string.IsNullOrEmpty(SoNgayPhepCongThem))
                        objFromDB.SoNgayPhepCongThem = Convert.ToDecimal(SoNgayPhepCongThem);
                    if (!string.IsNullOrEmpty(SoNgayPhepNamTruoc))
                        objFromDB.SoNgayPhepNamTruoc = Convert.ToDecimal(SoNgayPhepNamTruoc);
                    if (!string.IsNullOrEmpty(SoNgayTamUngHienTai))
                        objFromDB.SoNgayTamUngHienTai = Convert.ToDecimal(SoNgayTamUngHienTai);
                    //
                        objFromDB.SoNgayPhepConLai =  objFromDB.SoNgayTamUngHienTai;
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

        public bool QuanLyNghiPhepNam_Create(String publicKey, String token, Guid nienDoTaiChinh, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                {
                    try
                    {
                        //kiem tra da tao chua
                        var factory = CC_QuanLyNghiPhep_Factory.New();
                        //lay quan ly cu neu co
                        CC_QuanLyNghiPhep quanLy = factory.GetByNienDoTaiChinh(nienDoTaiChinh, congTy);
                        if (quanLy == null)
                        {
                            //tao quan ly moi
                            quanLy = factory.CreateManagedObject();
                            quanLy.Oid = Guid.NewGuid();
                            quanLy.NienDoTaiChinh = nienDoTaiChinh;
                            quanLy.CongTy = congTy;
                        }

                        //Lưu dữ liệu
                        factory.SaveChanges();
                        //
                        factory.Context.spd_WebChamCong_TaoDanhSachPhepDauNam(quanLy.CongTy, quanLy.Oid, nienDoTaiChinh, false, Guid.Empty);
                        //
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Helper.ErrorLog("Service1_QuanLyNghiPhep/QuanLyNghiPhepNam_Create", ex);
                    }
                }
            }
            //
            return false;
        }
        public bool QuanLyNghiPhepNam_Remove(String publicKey, String token, Guid nienDoTaiChinh, Guid congTy, Guid chiTietPhepOid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                {
                    try
                    {
                        //
                        var factory = CC_QuanLyNghiPhep_Factory.New();
                        //
                        factory.Context.spd_WebChamCong_TaoDanhSachPhepDauNam(congTy, Guid.Empty, nienDoTaiChinh, true, chiTietPhepOid);
                        //
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Helper.ErrorLog("Service1_QuanLyNghiPhep/QuanLyNghiPhepNam_Remove", ex);
                    }
                }
            }
            //
            return false;
        }
        public bool QuanLyNghiPhepNam_Update(String publicKey, String token, Guid nienDoTaiChinh, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                var factory = CC_QuanLyNghiPhep_Factory.New();
                CC_QuanLyNghiPhep quanLy = factory.GetByNienDoTaiChinh(nienDoTaiChinh, congTy);
                if (quanLy != null)
                {
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                    {
                        //
                        //factory.Context.spd_WebChamCong_TinhSoNgayPhepConLaiTrongNam(quanLy.Oid,Guid.Empty,congTy);
                        //change - 2020/02/29
                        factory.Context.spd_WebChamCong_CapNhatPhepTon(Guid.Empty, congTy, nienDoTaiChinh, quanLy.Oid);
                        transaction.Complete();
                    }
                }
                return true;
            }
            //
            return false;
        }
        public String QuanLyNghiPhepNam_Find_Json(String publicKey, String token, Guid nienDoTaiChinh, Guid boPhanId, Guid nhanVienId, Guid idWebUser,Guid idWebGroup, Guid congTy)
        {
            IEnumerable<DTO_QuanLyNghiPhep_Find> list = QuanLyNghiPhepNam_Find(publicKey, token, nienDoTaiChinh, boPhanId, nhanVienId, idWebUser, idWebGroup,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_QuanLyNghiPhep_Find> QuanLyNghiPhepNam_Find(String publicKey, String token, Guid nienDoTaiChinh, Guid boPhanId, Guid nhanVienId, Guid idWebUser, Guid idWebGroup, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_QuanLyNghiPhep_Factory factory = CC_QuanLyNghiPhep_Factory.New();
                IEnumerable<DTO_QuanLyNghiPhep_Find> list = factory.QuanLyNghiPhepNam_Find(nienDoTaiChinh, boPhanId, nhanVienId, idWebUser, idWebGroup, congTy);
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
