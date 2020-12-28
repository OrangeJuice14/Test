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
        IEnumerable<DTO_CC_KhaiBaoCongTac> CaNhanKhaiBaoCongTac_Find(String publicKey, String token, int thang, int nam,
            Guid webUserId);

        [OperationContract]
        String CaNhanKhaiBaoCongTac_Find_Json(String publicKey, String token, int thang, int nam,
            Guid webUserId);

        [OperationContract]
        bool CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe(String publicKey, String token, Guid? khaiBaoCongTacOid,
            DateTime tuNgay, DateTime denNgay, Guid webUserId);

        [OperationContract]
        DTO_CC_KhaiBaoCongTac CaNhanKhaiBaoCongTac_KhaiBaoMoi(String publicKey, String token, String noiDung,
            DateTime tuNgay, DateTime denNgay, Guid webUserId);

        [OperationContract] 
        bool CaNhanKhaiBaoCongTac_DeleteList(String publicKey, String token, List<DTO_CC_KhaiBaoCongTac> objList);

        [OperationContract]
        bool CaNhanKhaiBaoCongTac_DeleteList_Json(String publicKey, String token, string jsonObjectList);

    }
}
