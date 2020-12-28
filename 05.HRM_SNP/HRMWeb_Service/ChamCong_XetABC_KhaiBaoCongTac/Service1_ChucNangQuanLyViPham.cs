using System;
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
using Newtonsoft.Json;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
      
        public IEnumerable<DTO_QuanLyViPham_Find> QuanLyViPham_Find(String publicKey, String token,int ngay, int thang, int nam, Guid boPhanId)
        {
            if (Helper.TrustTest(publicKey, token))
            {

                CC_QuanLyViPham_Factory factory = new CC_QuanLyViPham_Factory();
                IEnumerable<DTO_QuanLyViPham_Find> list = null;
                list = factory.FindForQuanLyViPham(ngay,thang, nam, boPhanId);
                return list;
            }
            else
            {
                return null;
            }
        }
        public String QuanLyViPham_Find_Json(String publicKey, String token,int ngay, int thang, int nam, Guid boPhanId)
        {//DANG SD
            IEnumerable<DTO_QuanLyViPham_Find> list = QuanLyViPham_Find(publicKey, token, ngay, thang, nam, boPhanId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}
