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
        public IEnumerable<DTO_LoaiNhanSu> GetList_LoaiNhanSu(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                LoaiNhanSu_Factory factory = LoaiNhanSu_Factory.New();
                List<DTO_LoaiNhanSu> list = factory.GetAll_GCRecordIsNull().Map<DTO_LoaiNhanSu>().ToList();
                DTO_LoaiNhanSu item = new DTO_LoaiNhanSu();
                item.TenLoaiNhanSu = "Tất cả";
                item.Oid = Guid.Empty;
                item.MaQuanLy = "0";
                //
                list.Insert(0, item);
                //
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_LoaiNhanSu_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_LoaiNhanSu> list = GetList_LoaiNhanSu(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}
