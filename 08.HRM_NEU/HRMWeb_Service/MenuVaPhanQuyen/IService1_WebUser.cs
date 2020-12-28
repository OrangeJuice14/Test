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
        bool WebUsers_XoaUsers(String publicKey, String token, List<DTO_WebUser> objList);

        [OperationContract]
        bool WebUsers_XoaUsers_Json(String publicKey, String token, string jsonObjectList);
        [OperationContract]
        bool WebUsers_KiemTraTrungUsername(String publicKey, String token, Guid? webUserId, String username);

        [OperationContract]
        DTO_WebUser CheckForLogin_WebUser(String publicKey, String token, string username, string password);

        [OperationContract]
        String CheckForLogin_WebUserr_Json(String publicKey, String token, string username, string password);

        //WebUser_GetByEmail //////////////
        [OperationContract]
        DTO_WebUser WebUser_GetByEmail(String publicKey, String token, string email);

        [OperationContract]
        String WebUser_GetByEmail_Json(String publicKey, String token, string email);
        ///////////////////////////////////////
        [OperationContract]
        bool ChangePassword_WebUser(String publicKey, String token, Guid idWebUser, string newPassword);

        [OperationContract]
        IEnumerable<DTO_WebUser> GetList_WebUser(String publicKey, String token);

        [OperationContract]
        String GetList_WebUser_Json(String publicKey, String token);
        /////////////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_WebUser> GetList_WebUserQuanTri(String publicKey, String token);

        [OperationContract]
        String GetList_WebUserQuanTri_Json(String publicKey, String token);

        /////////////khac quan tri////////////////////////////////////
        [OperationContract]
        IEnumerable<DTO_WebUser> GetList_WebUserKhacQuanTri(String publicKey, String token);

        [OperationContract]
        String GetList_WebUserKhacQuanTri_Json(String publicKey, String token);

        // Save////////////////////////////////////////////
        [OperationContract]
        bool Save_WebUser(String publicKey, String token, DTO_WebUser obj);

        [OperationContract]
        bool Save_WebUser_Json(String publicKey, String token, string jsonObject);
        // ////////////////////////////////////////////
        [OperationContract]
        DTO_WebUser Get_WebUserBy_Id(String publicKey, String token, Guid id);

        [OperationContract]
        String Get_WebUserBy_Id_Json(String publicKey, String token, Guid id);
    }
}
