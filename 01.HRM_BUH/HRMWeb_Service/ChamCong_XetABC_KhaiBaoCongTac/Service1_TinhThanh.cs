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
    public partial class Service1 : IService1
    {
        public IEnumerable<DTO_TinhThanh> GetList_TinhThanh(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                TinhThanh_Factory factory = TinhThanh_Factory.New();
                IEnumerable<DTO_TinhThanh> list = factory.GetAll_GCRecordIsNull().Map<DTO_TinhThanh>();
                list = list.Where(q => q.SoNgayDiDuong >= 0).OrderBy(q => q.TenTinhThanh);
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_TinhThanh_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_TinhThanh> list = GetList_TinhThanh(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}