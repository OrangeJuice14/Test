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
using System.Net;
using System.Net.Mail;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {

        public bool QuanLyNghiPhep_CheckExists(String publicKey, String token, int nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                var factory = CC_QuanLyNghiPhep_Factory.New();
                bool daTonTai = factory.CheckExistChiTietNghiPhep(nam);
                return daTonTai;
            }
            else
            {
                throw new Exception("Chứng thực không hợp lệ");
            }
        }
        public bool QuanLyNghiPhepNam_Update(String publicKey, String token, int nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //
                var factory = CC_QuanLyNghiPhep_Factory.New();
                CC_QuanLyNghiPhep quanLy = factory.GetByNam(nam);
                if (quanLy != null)
                {
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                    {
                        //
                        factory.Context.spd_WebChamCong_TinhSoNgayPhepConLaiTrongNam(quanLy.Oid);
                        transaction.Complete();
                    }
                }
                return true;
            }
            //
            return false;
        }

        public String QuanLyNghiPhepNam_Find_Json(String publicKey, String token, int nam, Guid boPhanId, Guid nhanVienId)
        {
            IEnumerable<DTO_QuanLyNghiPhep_Find> list = QuanLyNghiPhepNam_Find(publicKey, token, nam, boPhanId, nhanVienId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public bool QuanLyNghiPhepNam_Create(String publicKey, String token, int nam)
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
                        CC_QuanLyNghiPhep quanLy = factory.GetByNam(nam);
                        if (quanLy == null)
                        {
                            //tao quan ly moi
                            quanLy = factory.CreateManagedObject();
                            quanLy.Oid = Guid.NewGuid();
                            quanLy.Nam = nam;
                        }

                        //Lưu dữ liệu
                        factory.SaveChanges();
                        //
                        factory.Context.spd_WebChamCong_TinhSoNgayNghiPhepDauNam(quanLy.Oid, nam, false);
                        //
                        transaction.Complete();
                        return true;
                    }
                    catch (Exception ex) { }
                }
            }
            //
            return false;
        }
        public bool QuanLyNghiPhepNam_Remove(String publicKey, String token, int nam)
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
                        factory.Context.spd_WebChamCong_TinhSoNgayNghiPhepDauNam(Guid.Empty, nam, true);
                        //
                        transaction.Complete();
                        return true;
                    }
                    catch { }
                }
            }
            //
            return false;
        }

        public IEnumerable<DTO_QuanLyNghiPhep_Find> QuanLyNghiPhepNam_Find(String publicKey, String token, int nam, Guid boPhanId, Guid nhanVienId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_QuanLyNghiPhep_Factory factory = CC_QuanLyNghiPhep_Factory.New();
                IEnumerable<DTO_QuanLyNghiPhep_Find> list = factory.QuanLyNghiPhepNam_Find(nam, boPhanId, nhanVienId);
                //
                return list;
            }
            else
            {
                return null;
            }
        }

        public bool QuanLyNghiPhepNam_SaveList_Json(String publicKey, String token, string jsonObjectList)
        {
            //chuyen jsonObject thanh object
            List<DTO_QuanLyNghiPhep_Find> objList = JsonConvert.DeserializeObject<List<DTO_QuanLyNghiPhep_Find>>(jsonObjectList);
            return QuanLyNghiPhepNam_SaveList(publicKey, token, objList);
        }

        public bool QuanLyNghiPhepNam_SaveList(String publicKey, String token, List<DTO_QuanLyNghiPhep_Find> objList)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_QuanLyNghiPhep_Factory factory = CC_QuanLyNghiPhep_Factory.New();
                foreach (var obj in objList)
                {
                    if (obj != null)
                    {

                        CC_ChiTietNghiPhep objFromDB = factory.GetChiTietNghiPhepByOid(obj.Oid);
                        if (objFromDB != null)
                        {
                            //cap nhat
                            objFromDB.TongSoNgayPhep = Convert.ToDecimal(obj.TongSoNgayPhep);
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

    }
}
