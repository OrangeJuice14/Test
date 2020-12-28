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
        [OperationContract]
        IEnumerable<DTO_CC_DangKyChamCongNgoaiGio> CaNhanDangKyChamCongNgoaiGio_Find(String publicKey, String token, int thang, int nam, Guid idNhanVien);
        [OperationContract]
        String CaNhanDangKyChamCongNgoaiGio_Find_Json(String publicKey, String token, int thang, int nam, Guid idNhanVien);
        [OperationContract]
        bool CaNhanDangKyChamCongNgoaiGio_DeleteList(String publicKey, String token, List<DTO_CC_DangKyChamCongNgoaiGio> objList);
        [OperationContract]
        bool CaNhanDangKyChamCongNgoaiGio_DeleteList_Json(String publicKey, String token, string jsonObjectList);
    }
}
