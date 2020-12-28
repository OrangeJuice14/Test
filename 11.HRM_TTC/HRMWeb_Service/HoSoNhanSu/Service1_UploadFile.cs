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


        // ///////////////////////////////////////
        public IEnumerable<DTO_UploadFile> UploadFile_GetList_ByNhanVienId(String publicKey, String token, Guid nhanVienId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                UploadFile_Factory factory = UploadFile_Factory.New();
                IEnumerable<DTO_UploadFile> list = factory.GetListDTO_UploadFileBy_NhanVienId_GCRecordIsNull(nhanVienId).ToList();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String UploadFile_GetList_ByNhanVienId_Json(String publicKey, String token, Guid nhanVienId)
        {//DANG SD
            IEnumerable<DTO_UploadFile> list = UploadFile_GetList_ByNhanVienId(publicKey, token, nhanVienId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}
