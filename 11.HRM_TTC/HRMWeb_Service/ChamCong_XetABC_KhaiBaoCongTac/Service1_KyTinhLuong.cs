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
        public IEnumerable<DTO_KyTinhLuong> KyTinhLuong_GetAll(String publicKey, String token, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                KyTinhLuong_Factory factory = KyTinhLuong_Factory.New();
                IEnumerable<DTO_KyTinhLuong> list = factory.GetKyTinhLuongList_All(congTy).Map<DTO_KyTinhLuong>();
                return list;
            }
            else
            {
                return null;
            }
        }
   
        public String KyTinhLuong_GetAll_Json(String publicKey, String token, Guid congTy)
        {
            IEnumerable<DTO_KyTinhLuong> list = KyTinhLuong_GetAll(publicKey, token,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}
