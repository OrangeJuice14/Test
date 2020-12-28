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
        public String GetNienDoTaiChinhList_Json(String publicKey, String token, Guid congTy)
        {
            IEnumerable<DTO_NienDoTaiChinh> list = GetNienDoTaiChinhList(publicKey, token, congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public IEnumerable<DTO_NienDoTaiChinh> GetNienDoTaiChinhList(String publicKey, String token, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                NienDoTaiChinh_Factory factory = NienDoTaiChinh_Factory.New();
                IEnumerable<DTO_NienDoTaiChinh> list = factory.GetList_GCRecordIsNull(congTy);
                return list;
            }
            else
            {
                return null;
            }
        }
    }
}
