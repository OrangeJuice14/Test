using System;
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

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        public IEnumerable<DTO_KyTinhLuong> KyTinhLuong_GetAll(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                KyTinhLuong_Factory factory = KyTinhLuong_Factory.New();
                IEnumerable<DTO_KyTinhLuong> list = factory.GetAll_GCRecordIsNull().Map<DTO_KyTinhLuong>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String KyTinhLuong_GetAll_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_KyTinhLuong> list = KyTinhLuong_GetAll(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        // /////////////////////////////////////////////////////
        public DTO_KyTinhLuong KyTinhLuong_ByMonthAndYear(String publicKey, String token,int month,int year)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                KyTinhLuong_Factory factory = KyTinhLuong_Factory.New();
                DTO_KyTinhLuong obj = factory.GetKyTinhLuong_GCRecordIsNull_ByMonthAndYear(month,year).Map<DTO_KyTinhLuong>();
                return obj;
            }
            else
            {
                return null;
            }
        }


        public String KyTinhLuong_ByMonthAndYear_Json(String publicKey, String token, int month, int year)
        {//DANG SD
            DTO_KyTinhLuong obj = KyTinhLuong_ByMonthAndYear(publicKey, token,month,year);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public bool KiemTraKhoaSo_KyTinhLuong(String publicKey, String token, int month, int year)
        {
            KyTinhLuong_Factory factory = KyTinhLuong_Factory.New();
            var obj = factory.GetKyTinhLuong_GCRecordIsNull_ByMonthAndYear(month, year);
            if (obj != null)
                return obj.KhoaSo ?? false;
            else return false;
        }
    }
}
