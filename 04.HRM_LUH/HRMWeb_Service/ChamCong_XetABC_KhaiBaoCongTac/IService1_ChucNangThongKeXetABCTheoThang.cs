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
        //int ThongKeXetABCTheoThang_FindCount(String publicKey, String token, int thang, int nam, Guid? boPhanId,
        //   string maNhanSu, Guid webUserId);

        [OperationContract]
        IEnumerable<DTO_ThongKeXetABCTheoThang> ThongKeXetABCTheoThang_Find(String publicKey, String token,
           int thang, int nam, Guid? boPhanId, string maNhanSu, Guid webUserId);
        [OperationContract]
        String ThongKeXetABCTheoThang_Find_Json(String publicKey, String token, int thang, int nam,
           Guid? boPhanId, Guid? idLoaiNhanSu, string maNhanSu, Guid webUserId);



        [OperationContract]
        IEnumerable<DTO_ThongKeXetABCTheoThang> ThongKeXetABCTheoThang_Cua1NhanVien_Find(String publicKey, String token,
            int thang, int nam, Guid nhanVienID);

        [OperationContract]
        String ThongKeXetABCTheoThang_Cua1NhanVien_Find_Json(String publicKey, String token, int thang, int nam,
             Guid nhanVienID);





        //[OperationContract]
        //IEnumerable<DTO_ThongKeXetABCTheoThang> ThongKeXetABCTheoThang_Find_PhanTrang(String publicKey,
        //   String token, int thang,
        //   int nam, Guid? boPhanId, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId);

        //[OperationContract]
        //String ThongKeXetABCTheoThang_Find_PhanTrang_Json(String publicKey, String token, int thang, int nam,
        //    Guid? boPhanId, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId);

    }
}
