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
        //[OperationContract]
        //IEnumerable<DTO_QuanLyChamCongNhanVien> GetList_QuanLyChamCongNhanVien(String publicKey, String token);

        //[OperationContract]
        //String GetList_QuanLyChamCongNhanVien_Json(String publicKey, String token);


        //// ////////////////////////////////////////////
        //[OperationContract]
        //DTO_QuanLyChamCongNhanVien Get_QuanLyChamCongNhanVienBy_Id(String publicKey, String token, Guid id);

        //[OperationContract]
        //String Get_QuanLyChamCongNhanVienBy_Id_Json(String publicKey, String token, Guid id);

        //// Save////////////////////////////////////////////
        //[OperationContract]
        //bool Save_QuanLyChamCongNhanVien(String publicKey, String token, DTO_QuanLyChamCongNhanVien obj);

        //[OperationContract]
        //bool Save_QuanLyChamCongNhanVien_Json(String publicKey, String token, string jsonObject);
        //// XOA////////////////////////////////////////////
        //[OperationContract]
        //bool Delete_QuanLyChamCongNhanVienBy_Id(String publicKey, String token, Guid id);
        //[OperationContract]
        //bool DeleteList_QuanLyChamCongNhanVienBy_IdList(String publicKey, String token, List<Guid> idList);
    }
}
