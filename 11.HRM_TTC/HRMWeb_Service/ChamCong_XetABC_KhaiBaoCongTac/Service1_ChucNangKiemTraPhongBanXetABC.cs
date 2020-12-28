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
        public IEnumerable<DTO_KiemTraPhongBanXetABC> KiemTraPhongBanXetABC_Find(String publicKey, String token, int thang, int nam, Boolean? daXetXongAbc, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                IEnumerable<DTO_KiemTraPhongBanXetABC> query = BoPhan_Factory.New().KiemTraPhongBanXetABC_Find(thang, nam, daXetXongAbc,congTy);
                //
                return query;
            }
            else
            {
                return null;
            }
        }

        public String KiemTraPhongBanXetABC_Find_Json(String publicKey, String token, int thang, int nam, Boolean? daXetXongAbc, Guid congTy)
        {//DANG SD
            IEnumerable<DTO_KiemTraPhongBanXetABC> list = KiemTraPhongBanXetABC_Find(publicKey, token, thang, nam, daXetXongAbc,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}
