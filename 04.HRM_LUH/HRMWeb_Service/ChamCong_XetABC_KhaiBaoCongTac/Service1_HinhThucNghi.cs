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

        public IEnumerable<DTO_HinhThucNghi> GetList_HinhThucNghi(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                HinhThucNghi_Factory factory = HinhThucNghi_Factory.New();
                IEnumerable<DTO_HinhThucNghi> list = factory.GetAll_GCRecordIsNull().Map<DTO_HinhThucNghi>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_HinhThucNghi_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_HinhThucNghi> list = GetList_HinhThucNghi(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        // ////////////////////////////////////////////
        // ////////////////////////////////////////////
        public DTO_BoPhan Get_HinhThucNghiBy_Id(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                DTO_BoPhan obj = factory.GetByID(id).Map<DTO_BoPhan>();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String Get_HinhThucNghiBy_Id_Json(String publicKey, String token, Guid id)
        {
            DTO_BoPhan obj = Get_HinhThucNghiBy_Id(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

    }
}
