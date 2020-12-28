using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ERP_Core;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using HRMWeb_Business.Predefined;
using Newtonsoft.Json;
using System.Transactions;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        public String NgayNghiTrongNam_Find_Json(String publicKey, String token, int nam)
        {//DANG SD
            IEnumerable<DTO_QuanLyNgayNghiTrongNam_Find> list = NgayNghiTrongNam_Find(publicKey, token, nam);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }


        public IEnumerable<DTO_QuanLyNgayNghiTrongNam_Find> NgayNghiTrongNam_Find(String publicKey, String token, int nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                NgayNghiTrongNam_Factory factory = NgayNghiTrongNam_Factory.New();
                IEnumerable<DTO_QuanLyNgayNghiTrongNam_Find> list = null;
                list = factory.NgayNghiTrongNam_Find(nam).Map<DTO_QuanLyNgayNghiTrongNam_Find>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public bool NgayNghiTrongNam_Save(String publicKey, String token, String TenNgayNghi, DateTime TuNgay, DateTime DenNgay, int Nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                //QuanLyNgayNghiTrongNam_Factory qlfac = QuanLyNgayNghiTrongNam_Factory.New();
                //NgayNghiTrongNam_Factory factory = NgayNghiTrongNam_Factory.New();

                //QuanLyNgayNghiTrongNam ql = qlfac.GetByYear(Nam);
                ////Kiểm tra có quản lý ngày nghỉ chưa, nếu chưa có tạo mới
                //if (ql == null)
                //{
                //    ql = qlfac.CreateManagedObject();
                //    ql.Oid = Guid.NewGuid();
                //    ql.Nam = Nam;
                //    qlfac.SaveChanges();
                //}
                ////Tạo mới ngày nghỉ trong năm
                //NgayNghiTrongNam obj = factory.CreateManagedObject();
                //obj.Oid = Guid.NewGuid();
                //obj.TenNgayNghi = TenNgayNghi;
                //obj.QuanLyNgayNghiTrongNam = ql.Oid;
                //try
                //{
                //    factory.SaveChanges();
                //    return true;
                //}
                //catch (Exception ex)
                //{
                //    return false;
                //}
                using (var tran = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    NgayNghiTrongNam_Factory factory = NgayNghiTrongNam_Factory.New();
                    try
                    {
                        factory.Context.spd_WebChamCong_NgayNghiTrongNam_ThemMoi(TenNgayNghi, TuNgay, DenNgay);
                        factory.SaveChangesWithoutTransactionScope();
                        tran.Complete();
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
        public bool NgayNghiTrongNam_Delete(String publicKey, String token, Guid Oid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    NgayNghiTrongNam_Factory factory = NgayNghiTrongNam_Factory.New();
                    //NgayNghiTrongNam obj = new NgayNghiTrongNam() { Oid = Oid };
                    //factory.Attach(obj);
                    //NgayNghiTrongNam_Factory.FullDelete(factory.Context, obj);
                    //////////////
                    try
                    {
                        factory.Context.spd_WebChamCong_NgayNghiTrongNam_Xoa(Oid);
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
    }
}
