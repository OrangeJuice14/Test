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

        public String QuanLyPhepHe_GetByID_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_QuanLyPhepHe_Find obj = QuanLyPhepHe_GetByID(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public DTO_QuanLyPhepHe_Find QuanLyPhepHe_GetByID(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                CC_QuanLyPhepHe_Factory factory = CC_QuanLyPhepHe_Factory.New();
                DTO_QuanLyPhepHe_Find obj = null;
                obj = factory.QuanLyPhepHe_ByID(id);
                return obj;
            }
            else
            {
                return null;
            }
        }
        public bool QuanLyPhepHe_CheckExists(String publicKey, String token, Guid nienDoTaiChinh, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                var factory = CC_QuanLyPhepHe_Factory.New();
                bool daTonTai = factory.CheckExistChiTietPhepHe(nienDoTaiChinh, congTy);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }

        public bool QuanLyPhepHe_SaveList(String publicKey, String token, Guid Oid, string SoPhepHe)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_QuanLyPhepHe_Factory factory = CC_QuanLyPhepHe_Factory.New();
                CC_ChiTietPhepHe objFromDB = factory.GetChiTietPhepHeByOid(Oid);
                if (objFromDB != null)
                {
                    if(!string.IsNullOrEmpty(SoPhepHe))
                        objFromDB.SoPhepHe = Convert.ToDecimal(SoPhepHe);
                }
                try
                {
                    factory.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Helper.ErrorLog("Service1_QuanLyPhepHe/QuanLyPhepHe_SaveList", ex);
                    return false;
                }
            }
            return false;
        }

        public bool QuanLyPhepHe_ChotPhepHe(String publicKey, String token, Guid nienDoTaiChinh, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                {
                    try
                    {
                        //kiem tra da tao chua
                        var factory = CC_QuanLyPhepHe_Factory.New();
                        //lay quan ly cu neu co
                        CC_QuanLyPhepHe quanLy = factory.GetByNienDoTaiChinh(nienDoTaiChinh, congTy);
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

                        factory.Context.spd_WebChamCong_ChotPhepHeTheoNienDo(quanLy.Oid, nienDoTaiChinh);

                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Helper.ErrorLog("Service1_QuanLyPhepHe/QuanLyPhepHe_ChotPhepHe", ex);
                    }
                }
            }
            //
            return false;
        }
        public String QuanLyPhepHe_Find_Json(String publicKey, String token, Guid nienDoTaiChinh, Guid boPhanId, Guid nhanVienId, Guid idWebUser,Guid idWebGroup, Guid congTy)
        {
            IEnumerable<DTO_QuanLyPhepHe_Find> list = QuanLyPhepHe_Find(publicKey, token, nienDoTaiChinh, boPhanId, nhanVienId, idWebUser, idWebGroup,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public IEnumerable<DTO_QuanLyPhepHe_Find> QuanLyPhepHe_Find(String publicKey, String token, Guid nienDoTaiChinh, Guid boPhanId, Guid nhanVienId, Guid idWebUser, Guid idWebGroup, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_QuanLyPhepHe_Factory factory = CC_QuanLyPhepHe_Factory.New();
                IEnumerable<DTO_QuanLyPhepHe_Find> list = factory.QuanLyPhepHe_Find(nienDoTaiChinh, boPhanId, nhanVienId, idWebUser, idWebGroup, congTy);
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
