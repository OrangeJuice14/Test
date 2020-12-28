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
        //tao danh sach ca thang
        [OperationContract]
        bool DoDuLieuChamCongThang(String publicKey, String token, int thang, int nam, Guid idHinhThucNghi, Guid webUserId);
        [OperationContract]
        bool DoDuLieuChamCongThang_CheckExists(String publicKey, String token, int thang, int nam);

    }
}
