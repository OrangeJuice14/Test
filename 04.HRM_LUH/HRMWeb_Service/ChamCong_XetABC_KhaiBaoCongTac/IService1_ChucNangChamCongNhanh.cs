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
        bool ChamCongNhanh(String publicKey, String token, int ngay, int thang, int nam, Guid? idHinhThucNghi, Guid? idBoPhan
            , Guid? idLoaiNhanSu, Guid webUserId);
    }
}
