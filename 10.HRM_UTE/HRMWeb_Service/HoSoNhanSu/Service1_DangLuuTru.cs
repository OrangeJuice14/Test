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
using Newtonsoft.Json;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        public IEnumerable<DTO_DangLuuTru> DangLuuTru_GetAll(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                DangLuuTru_Factory factory = DangLuuTru_Factory.New();
                IEnumerable<DTO_DangLuuTru> list = factory.GetAll_GCRecordIsNull().Map<DTO_DangLuuTru>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String DangLuuTru_GetAll_Json(String publicKey, String token)
        {
            IEnumerable<DTO_DangLuuTru> list = DangLuuTru_GetAll(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}
