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
        IEnumerable<DTO_QuanLyKhaiBaoCongTac_Find> QuanLyKhaiBaoCongTac_Find(String publicKey, String token, DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy);

        [OperationContract]
        String QuanLyKhaiBaoCongTac_Find_Json(String publicKey, String token, DateTime tungay, DateTime denngay, Guid? boPhanId,
            int? trangThai, string maNhanSu, Guid webUserId, Guid congTy);

        [OperationContract]
        bool QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList(String publicKey, String token,
            List<DTO_QuanLyKhaiBaoCongTac_Find> objList, int trangThai, string userId);

        [OperationContract]
        bool QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList_Json(String publicKey, String token, string jsonObjectList,
            int trangThai, string userId);


        [OperationContract]
        bool QuanLyKhaiBaoCongTac_DeleteList(String publicKey, String token, List<DTO_QuanLyKhaiBaoCongTac_Find> objList);

        [OperationContract]
        bool QuanLyKhaiBaoCongTac_DeleteListList_Json(String publicKey, String token, string jsonObjectList);

    }
}
