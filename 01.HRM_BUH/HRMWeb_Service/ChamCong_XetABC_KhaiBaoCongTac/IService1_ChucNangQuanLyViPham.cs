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
        IEnumerable<DTO_QuanLyViPham_Find> QuanLyViPham_Find(String publicKey, String token,int ngay, int thang, int nam, Guid boPhanId);

        [OperationContract]
        String QuanLyViPham_Find_Json(String publicKey, String token,int ngay, int thang, int nam, Guid boPhanId);
    }
}
