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
        public List<String> GetList_HinhThucNghiKyHieu(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                List<String> list = new List<String>();
                HinhThucNghi_Factory factory = HinhThucNghi_Factory.New();
                IEnumerable<DTO_HinhThucNghi> temp = factory.GetAll_GCRecordIsNull().Map<DTO_HinhThucNghi>();
                foreach (DTO_HinhThucNghi h in temp)
                {
                    list.Add(h.KyHieu);
                }
                list.Add("+");
                list.Add("");
                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_HinhThucNghi> GetList_HinhThucNghiForUpdate(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                HinhThucNghi_Factory factory = HinhThucNghi_Factory.New();
                IEnumerable<DTO_HinhThucNghi> list = factory.GetListForUpdate().Map<DTO_HinhThucNghi>();
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
        public String GetList_HinhThucNghiKyHieu_Json(String publicKey, String token)
        {//DANG SD
            List<String> list = GetList_HinhThucNghiKyHieu(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetList_HinhThucNghiForUpdate_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_HinhThucNghi> list = GetList_HinhThucNghiForUpdate(publicKey, token);
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
