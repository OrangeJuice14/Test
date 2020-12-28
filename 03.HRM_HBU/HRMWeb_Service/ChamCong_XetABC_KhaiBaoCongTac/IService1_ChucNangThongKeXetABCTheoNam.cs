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
        //int ThongKeXetABCTheoNam_FindCount(String publicKey, String token, int nam, Guid? boPhanId,
        //   string maNhanSu, Guid webUserId);



        [OperationContract]
        IEnumerable<DTO_ThongKeXetABCTheoNam> ThongKeXetABCTheoNam_Find(String publicKey, String token,
            int nam, Guid? boPhanId, string maNhanSu, Guid webUserId);
        [OperationContract]
        String ThongKeXetABCTheoNam_Find_Json(String publicKey, String token, int nam,
           Guid? boPhanId, Guid? idLoaiNhanSu, string maNhanSu, Guid webUserId);




        [OperationContract]
        IEnumerable<DTO_ThongKeXetABCTheoNam> ThongKeXetABCTheoNam_Cua1NhanVien_Find(String publicKey, String token,
            int nam, Guid nhanVienID);

        [OperationContract]
        String ThongKeXetABCTheoNam_Cua1NhanVien_Find_Json(String publicKey, String token, int nam , Guid nhanVienID);




        //[OperationContract]
        //IEnumerable<DTO_ThongKeXetABCTheoNam> ThongKeXetABCTheoNam_Find_PhanTrang(String publicKey,
        //   String token,
        //   int nam, Guid? boPhanId, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId);

        //[OperationContract]
        //String ThongKeXetABCTheoNam_Find_PhanTrang_Json(String publicKey, String token, int nam,
        //    Guid? boPhanId, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId);

    }
}
