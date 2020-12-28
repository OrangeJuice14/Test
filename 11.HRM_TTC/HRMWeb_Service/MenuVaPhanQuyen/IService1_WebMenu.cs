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
        IEnumerable<DTO_WebMenu> WebMenu_GetList(String publicKey, String token);

        [OperationContract]
        String WebMenu_GetList_Json(String publicKey, String token);
        //// ////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_WebMenu> WebMenu_GetListBy_WebUserId(String publicKey, String token, Guid webUserId, Guid congTy);

        [OperationContract]
        String WebMenu_GetListBy_WebUserId_Json(String publicKey, String token, Guid webUserId, Guid congTy);

        [OperationContract]
        IEnumerable<String> WebMenu_GetURLListBy_WebUserId(String publicKey, String token, Guid webUserId);

        [OperationContract]
        String WebMenu_GetURLListBy_WebUserId_Json(String publicKey, String token, Guid webUserId);
        
        /// ////////////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_WebMenu> WebMenu_GetListTop2LevelDeepBy_WebUserId(String publicKey, String token, Guid webUserId, Guid congTy);

        [OperationContract]
        String WebMenu_GetListTop2LevelDeepBy_WebUserId_Json(String publicKey, String token, Guid webUserId, Guid congTy);
        /// ////////////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_WebMenu> WebMenu_GetChildMenuListBy_WebUserId_AndMenuId(String publicKey, String token,
            Guid webUserId, Guid menuId, Guid congTy);

        [OperationContract]
        String WebMenu_GetChildMenuListBy_WebUserId_AndMenuId_Json(String publicKey, String token, Guid webUserId,
            Guid menuId, Guid congTy);
        //// ////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_WebMenu> WebMenu_GetListBy_WebGroupId(String publicKey, String token, Guid webGroupId);

        [OperationContract]
        String WebMenu_GetListBy_WebGroupId_Json(String publicKey, String token, Guid webGroupId);
        //// ////////////////////////////////////////////
        [OperationContract]
        DTO_WebMenu WebMenu_GetById(String publicKey, String token, Guid id);

        [OperationContract]
        String WebMenu_GetById_Json(String publicKey, String token, Guid id);
    }
}
