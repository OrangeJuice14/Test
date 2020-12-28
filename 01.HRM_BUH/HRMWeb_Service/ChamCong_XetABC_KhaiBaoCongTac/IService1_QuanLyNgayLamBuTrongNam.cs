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
    public partial class Service1 : IService1
    {
        // Start Edit By Vinh
        public String NgayLamBuTrongNam_Find_Json(String publicKey, String token, int nam)
        {//DANG SD
            IEnumerable<DTO_NgayLamBuTrongNam_Find> list = NgayLamBuTrongNam_Find(publicKey, token, nam);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public IEnumerable<DTO_NgayLamBuTrongNam_Find> NgayLamBuTrongNam_Find(String publicKey, String token, int nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                NgayLamBuTrongNam_Factory factory = NgayLamBuTrongNam_Factory.New();
                IEnumerable<DTO_NgayLamBuTrongNam_Find> list = null;
                list = factory.NgayLamBuTrongNam_Find(nam).Map<DTO_NgayLamBuTrongNam_Find>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public bool NgayLamBuTrongNam_Save(String publicKey, String token, String TenNgayLamBu, DateTime TuNgay, DateTime DenNgay, int Nam)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                QuanLyNgayLamBuTrongNam_Factory qlfac = QuanLyNgayLamBuTrongNam_Factory.New();
                NgayLamBuTrongNam_Factory factory = NgayLamBuTrongNam_Factory.New();
                if(TuNgay > DenNgay)
                    return false;
                QuanLyNgayLamBuTrongNam ql = qlfac.GetByYear(TuNgay.Year);
                //Kiểm tra có quản lý ngày làm bù chưa, nếu chưa có tạo mới
                if (ql == null)
                {
                    ql = qlfac.CreateManagedObject();
                    ql.Oid = Guid.NewGuid();
                    ql.Nam = TuNgay.Year;
                    ql.OptimisticLockField = 0;
                    qlfac.SaveChanges();
                }
                try
                {
                    for (DateTime Date = TuNgay; Date <= DenNgay; Date = Date.AddDays(1))
                    {
                        //Tạo mới ngày làm bù trong năm
                        NgayLamBuTrongNam obj = factory.CreateManagedObject();
                        obj.Oid = Guid.NewGuid();
                        obj.TenNgayLamBu = TenNgayLamBu;
                        obj.NgayLamBu = Date;
                        obj.QuanLyNgayLamBuTrongNam = ql.Oid;

                        factory.SaveChanges();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
            return false;
        }

        public bool NgayLamBuTrongNam_Delete(String publicKey, String token, Guid Oid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    NgayLamBuTrongNam_Factory factory = NgayLamBuTrongNam_Factory.New();
                    try
                    {
                        var obj = factory.NgayLamBuTrongNam_Find_ByOid(Oid);
                        obj.GCRecord = 1;
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
        // End Edit By Vinh
    }
}