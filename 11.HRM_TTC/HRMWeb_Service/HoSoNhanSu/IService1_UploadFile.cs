using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using HRMWeb_Business.Model;

namespace HRMWeb_Service
{
    public partial interface IService1
    {

        // ///////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_UploadFile> UploadFile_GetList_ByNhanVienId(String publicKey, String token, Guid nhanVienId);

        [OperationContract]
        String UploadFile_GetList_ByNhanVienId_Json(String publicKey, String token, Guid nhanVienId);
    }
}
