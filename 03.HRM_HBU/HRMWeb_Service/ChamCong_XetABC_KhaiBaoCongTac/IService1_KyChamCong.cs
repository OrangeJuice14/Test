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
        IEnumerable<DTO_KyChamCong> KyChamCong_Find(String publicKey, String token, int nam);
        [OperationContract]
        bool KyChamCong_CheckExist(String publicKey, String token, int thang, int nam);
        [OperationContract]
        bool KyChamCong_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, DateTime tuNgay, DateTime denNgay);
        [OperationContract]
        bool KyChamCong_Check(String publicKey, String token, int thang, int nam);
        }
}
