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
        bool ChotChamCongThang_CheckExists(String publicKey, String token, Guid boPhanID, int thang, int nam, int loaicanbo);
        [OperationContract]
        bool ChotChamCongThang_Create(String publicKey, String token, Guid boPhanID, int thang, int nam, Guid webUserId,int loaicanbo);

        [OperationContract]
        bool ChotChamCongThang_Delete(String publicKey, String token, Guid boPhanID, int thang, int nam, int loaicanbo);
    }
}
