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
        IEnumerable<DTO_KyTinhLuong> KyTinhLuong_GetAll(String publicKey, String token);
        [OperationContract]
        String KyTinhLuong_GetAll_Json(String publicKey, String token);

        ////////////////
        [OperationContract]
        DTO_KyTinhLuong KyTinhLuong_ByMonthAndYear(String publicKey, String token, int month, int year);
        [OperationContract]
        String KyTinhLuong_ByMonthAndYear_Json(String publicKey, String token, int month, int year);
       
    }
}
