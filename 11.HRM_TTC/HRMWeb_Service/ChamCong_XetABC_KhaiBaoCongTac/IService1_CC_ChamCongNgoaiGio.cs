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
        IEnumerable<DTO_CC_ChamCongNgoaiGio> DangKyChamCongNgoaiGio_Find(String publicKey, String token, DateTime tuNgay, DateTime denNgay, Guid idNhanVien);
        [OperationContract]
        String DangKyChamCongNgoaiGio_Find_Json(String publicKey, String token, DateTime tuNgay, DateTime denNgay, Guid idNhanVien);
        [OperationContract]
        IEnumerable<DTO_CC_ChamCongNgoaiGio> QuanLyChamCongNgoaiGio_Find(String publicKey, String token, DateTime tuNgay, DateTime denNgay, Guid IDBoPhan, int trangthai, Guid userID, Guid congTy);
        [OperationContract]
        String QuanLyChamCongNgoaiGio_Find_Json(String publicKey, String token, DateTime tuNgay, DateTime denNgay, Guid IDBoPhan, int trangthai, Guid userID, Guid congTy);
        [OperationContract]
        bool DangKyChamCongNgoaiGio_DeleteList(String publicKey, String token, List<DTO_CC_ChamCongNgoaiGio> objList);
        [OperationContract]
        bool DangKyChamCongNgoaiGio_DeleteList_Json(String publicKey, String token, string jsonObjectList);
    }
}
