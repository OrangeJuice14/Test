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

        public bool QuanLyNghiPhep_CheckExists(String publicKey, String token, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = QuanLyNghiPhep_Factory.New();
                bool daTonTai = factory.CheckExistChiTietNghiPhep(nam);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool QuanLyNghiPhep_Create(String publicKey, String token, int nam)
        {//DANG SD
            if (Helper.TrustTest(publicKey, token))
            {
                //kiem tra da tao chua
                var factory = QuanLyNghiPhep_Factory.New();
                bool daTonTaiBangCon = false;
                if (daTonTaiBangCon)
                {
                    throw new Exception(string.Format("Không thể chốt vì đã tồn tại bảng chốt năm {1} của bộ phận", nam));
                }
                else
                {
                    //try
                    //{
                    //lay quan ly cu neu co
                    CC_QuanLyNghiPhep quanLy = factory.GetByNam(nam);
                    if (quanLy == null)
                    {
                        //tao quan ly moi
                        quanLy = factory.CreateManagedObject();
                        quanLy.Oid = Guid.NewGuid();
                        quanLy.Nam = nam;
                    }
                    factory.SaveChanges();
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                                TimeSpan.FromSeconds(180)))
                    {
                        factory.Context.spd_WebChamCong_QuanLyNghiPhep_TaoMoiChiTietNghiPhep(quanLy.Oid);
                        transaction.Complete();
                    }
                    return true;


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
        public bool QuanLyNghiPhep_Update(String publicKey, String token, int nam, Guid boPhanId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = QuanLyNghiPhep_Factory.New();
                var tatCa = boPhanId == Guid.Empty ? true : false;
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                            TimeSpan.FromSeconds(180)))
                {
                    factory.Context.spd_WebChamCong_CapNhatGiayNghiPhep(nam, boPhanId, tatCa);
                    transaction.Complete();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool QuanLyNghiPhep_Delete(String publicKey, String token, int nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                var factory = QuanLyNghiPhep_Factory.New();
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                            TimeSpan.FromSeconds(180)))
                {
                    factory.Context.spd_WebChamCong_XoaGiayNghiPhep(nam);
                    transaction.Complete();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public String QuanLyNghiPhep_Find_Json(String publicKey, String token, int nam, string boPhanId)
        {//DANG SD
            IEnumerable<DTO_QuanLyNghiPhep_Find> list = QuanLyNghiPhep_Find(publicKey, token, nam, boPhanId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String QuanLyNghiPhep_InGiayNghiPhepNam_Json(String publicKey, String token, Guid NhanVien, int Nam)
        {//DANG SD
            DTO_QuanLyNghiPhep_InGiayNghiPhep list = QuanLyNghiPhep_InGiayNghiPhepNam(publicKey, token, NhanVien, Nam);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public DTO_QuanLyNghiPhep_InGiayNghiPhep QuanLyNghiPhep_InGiayNghiPhepNam(String publicKey, String token, Guid NhanVien, int Nam)
        {//DANG SD
            {
                if (Helper.TrustTest(publicKey, token))
                {
                    QuanLyNghiPhep_Factory factory = QuanLyNghiPhep_Factory.New();
                    DTO_QuanLyNghiPhep_InGiayNghiPhep list = factory.QuanLyNghiPhep_InGiayNghiPhepNam(NhanVien, Nam);
                    return list;
                }
                else
                {
                    return null;
                }
            }
        } 
        public IEnumerable<DTO_QuanLyNghiPhep_Find> QuanLyNghiPhep_Find(String publicKey, String token, int nam, string boPhanId)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                QuanLyNghiPhep_Factory factory = QuanLyNghiPhep_Factory.New();
                IEnumerable<DTO_QuanLyNghiPhep_Find> list = null;

                CC_QuanLyNghiPhep ql = factory.GetByNam(nam);
                if (ql!=null)
                {
                    list = factory.QuanLyNghiPhep_Find(ql.Oid, boPhanId);
                }
                
                return list;
            }
            else
            {
                return null;
            }
        }
    }
}
