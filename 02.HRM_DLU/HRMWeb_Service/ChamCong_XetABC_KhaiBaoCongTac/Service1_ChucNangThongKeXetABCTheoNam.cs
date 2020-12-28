using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ERP_Core;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using HRMWeb_Business.Predefined;
using Newtonsoft.Json;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        #region QuanLyXetABC_Find
        //dem so mau tin
        /*
        public int ThongKeXetABCTheoNam_FindCount(String publicKey, String token, int nam, Guid? boPhanId, string maNhanSu, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                int count = 0;
                ChiTietChamCongNhanVien_Factory factory = ChiTietChamCongNhanVien_Factory.New();
                count = factory.ThongKeXetABCTheoNam_FindCount(nam, boPhanId, maNhanSu, webUserId);


                return count;
            }
            else
            {
                return 0;
            }
        }
        */



        public IEnumerable<DTO_ThongKeXetABCTheoNam> ThongKeXetABCTheoNam_Find(String publicKey, String token, int nam, Guid? boPhanId, string maNhanSu, Guid webUserId)
        {

            if (Helper.TrustTest(publicKey, token))
            {
                var factory = CC_ChiTietChamCongNhanVien_Factory.New();
                //
                IEnumerable<DTO_ThongKeXetABCTheoNam> list = factory.ThongKeXetABCTheoNam_Find(nam, boPhanId, maNhanSu, webUserId).ToList();
                return list;

            }
            else
            {
                return null;
            }
        }
        public String ThongKeXetABCTheoNam_Find_Json(String publicKey, String token, int nam, Guid? boPhanId, Guid? idLoaiNhanSu, string maNhanSu, Guid webUserId)
        {//DANG SD
            IEnumerable<DTO_ThongKeXetABCTheoNam> list = ThongKeXetABCTheoNam_Find(publicKey, token, nam, boPhanId, maNhanSu, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public IEnumerable<DTO_ThongKeXetABCTheoNam> ThongKeXetABCTheoNam_Cua1NhanVien_Find(String publicKey, String token, int nam, Guid nhanVienID)
        {

            if (Helper.TrustTest(publicKey, token))
            {
                var factory = CC_ChiTietChamCongNhanVien_Factory.New();
                //
                IEnumerable<DTO_ThongKeXetABCTheoNam> list = factory.ThongKeXetABCTheoNam_Cua1NhanVien_Find(nam, nhanVienID).ToList();
                return list;


            }
            else
            {
                return null;
            }
        }
        public String ThongKeXetABCTheoNam_Cua1NhanVien_Find_Json(String publicKey, String token, int nam, Guid nhanVienID)
        {//DANG SD
            IEnumerable<DTO_ThongKeXetABCTheoNam> list = ThongKeXetABCTheoNam_Cua1NhanVien_Find(publicKey, token, nam, nhanVienID);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        #endregion
    }
}
