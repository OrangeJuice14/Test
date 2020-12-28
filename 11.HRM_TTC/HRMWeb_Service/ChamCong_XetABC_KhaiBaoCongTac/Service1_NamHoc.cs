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
        public String GetNamHocHienTai_Json(String publicKey, String token)
        {
            DTO_NamHoc namHoc = (new NamHoc_Factory()).GetListByNam(DateTime.Now.Year);
            String json = "";
            if (namHoc != null)
                json = JsonConvert.SerializeObject(namHoc);
            return json;
        }

        public String GetNamHocList_Json(String publicKey, String token)
        {
            IEnumerable<DTO_NamHoc> list = GetNamHocList(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public IEnumerable<DTO_NamHoc> GetNamHocList(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                NamHoc_Factory factory = NamHoc_Factory.New();
                IEnumerable<DTO_NamHoc> list = factory.GetList_GCRecordIsNull();
                return list;
            }
            else
            {
                return null;
            }
        }
    }
}
