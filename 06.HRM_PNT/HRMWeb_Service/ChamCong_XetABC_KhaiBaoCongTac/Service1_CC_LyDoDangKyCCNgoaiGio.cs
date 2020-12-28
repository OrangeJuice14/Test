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

        public IEnumerable<DTO_CC_LyDoDangKyCCNgoaiGio> GetList_LyDo(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_LyDoDangKyCCNgoaiGio_Factory factory = CC_LyDoDangKyCCNgoaiGio_Factory.New();
                IEnumerable<DTO_CC_LyDoDangKyCCNgoaiGio> list = factory.GetAll().Map<DTO_CC_LyDoDangKyCCNgoaiGio>();
                return list;
            }
            else
            {
                return null;
            }
        }
       
        public String GetList_LyDo_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_CC_LyDoDangKyCCNgoaiGio> list = GetList_LyDo(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
           public CC_LyDoDangKyChamCongNgoaiGio Get_LyDoBy_Id(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                CC_LyDoDangKyCCNgoaiGio_Factory factory = CC_LyDoDangKyCCNgoaiGio_Factory.New();
                CC_LyDoDangKyChamCongNgoaiGio obj = factory.GetByID(id);
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String Get_LyDoBy_Id_Json(String publicKey, String token, Guid id)
        {
            CC_LyDoDangKyChamCongNgoaiGio obj = Get_LyDoBy_Id(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }


    }
}
