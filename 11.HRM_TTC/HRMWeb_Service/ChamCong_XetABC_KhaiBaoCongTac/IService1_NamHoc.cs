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
    public partial interface IService1_NamHoc
    {

        [OperationContract]
        IEnumerable<DTO_CC_CaChamCong> GetNamHocList(String publicKey, String token);
    }
}
