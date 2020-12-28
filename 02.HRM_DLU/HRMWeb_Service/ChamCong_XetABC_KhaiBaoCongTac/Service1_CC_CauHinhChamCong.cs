using System;
using System.Collections;
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
        public String CauHinhChamCong_Find_Json(String publicKey, String token)
        {//DANG SD
            DTO_CC_CauHinhChamCong  list = GetCauHinhChamCong(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public DTO_CC_CauHinhChamCong GetCauHinhChamCong(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_CauHinhChamCong_Factory factory = CC_CauHinhChamCong_Factory.New();
                DTO_CC_CauHinhChamCong list = factory.GetCauHinhCauCong().Map<DTO_CC_CauHinhChamCong>();

                return list;
            }
            else
            {
                return null;
            }
        }
        public bool CauHinhChamCong_Save(String publicKey, String token,Guid oid, string sogiaydiduong,string songaynghiphep)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    TimeSpan.FromSeconds(360)))
                {
                    //
                    try
                    {
                        var factory = CC_CauHinhChamCong_Factory.New();
                        CC_CauHinhChamCong cauHinh = factory.GetCauHinhCauCongByOid(oid);
                        cauHinh.SoGiayDiDuong = Convert.ToInt32(sogiaydiduong);
                        cauHinh.SoNgayNghiPhep = Convert.ToInt32(songaynghiphep);
                        //
                        factory.SaveChangesWithoutTransactionScope();
                        transaction.Complete();
                        //
                        return true;
                    }
                    catch (Exception ex) {  }
                }
            }
            //
            return false;
        }
    }
}
