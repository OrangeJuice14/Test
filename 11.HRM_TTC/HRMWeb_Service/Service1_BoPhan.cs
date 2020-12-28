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
using Newtonsoft.Json;
using HRMWeb_Business.Predefined;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {

        //lấy danh sách bộ phận

        public IEnumerable<DTO_BoPhan> GetList_BoPhan(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                IEnumerable<DTO_BoPhan> list = factory.GetAll_GCRecordIsNull().Map<DTO_BoPhan>(); ;
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_BoPhan_Json(String publicKey, String token)
        {//DANG SD
            IEnumerable<DTO_BoPhan> list = GetList_BoPhan(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        // ////////////////////////////////////////////
        public DTO_BoPhan Get_BoPhanBy_Id(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                DTO_BoPhan obj = factory.GetByID(id).Map<DTO_BoPhan>();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String Get_BoPhanBy_Id_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_BoPhan obj = Get_BoPhanBy_Id(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        // ////////////////////////////////////////////
        public IEnumerable<DTO_BoPhan> GetList_BoPhan_DuocPhanQuyenChoWebUserId(String publicKey, String token, Guid webUserId, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                IEnumerable<DTO_BoPhan> list = factory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull_New(webUserId, congTy).Map<DTO_BoPhan>();

                //
                return list;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<DTO_BoPhan> GetList_BoPhan_DuocPhanQuyenChoWebUserIdAndCompany(String publicKey, String token, Guid webUserId,Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                IEnumerable<DTO_BoPhan> list = factory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserIdAndCompany_GCRecordIsNull(webUserId, congTy).Map<DTO_BoPhan>();

                //
                return list;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<DTO_BoPhan> GetBoPhan_GetLoaiBoPhanByWebGroup(String publicKey, String token, Guid webgroupid, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                IEnumerable<DTO_BoPhan> list = factory.BoPhan_GetLoaiBoPhanByWebGroup(webgroupid,congTy).Map<DTO_BoPhan>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public List<DTO_BoPhan> GetList_Truong_DuocPhanQuyenChoWebUserId(String publicKey, String token, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                List<DTO_BoPhan> list = factory.GetDanhSachTruong_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Map<DTO_BoPhan>().ToList();
                //            
                return list;
            }
            else
            {
                return null;
            }
        }
        public List<DTO_BoPhan> GetList_Truong_DuocPhanQuyenChoWebUserId_New(String publicKey, String token, Guid webUserId)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                List<DTO_BoPhan> list = factory.GetDanhSachTruong_DuocPhanQuyenChoWebUserId_New(webUserId).Map<DTO_BoPhan>().ToList();
                //            
                return list;
            }
            else
            {
                return null;
            }
        }
        public List<DTO_BoPhan> GetList_BoPhan_DuocPhanQuyenChoWebUserId_All(String publicKey, String token, Guid webUserId, Guid groupId, Guid congTy)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                BoPhan_Factory factory = BoPhan_Factory.New();
                List<DTO_BoPhan> list = factory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull_New(webUserId, congTy).Map<DTO_BoPhan>().ToList();
                int i = 1;
                foreach (DTO_BoPhan bp in list)
                {
                    bp.STT = i;
                    bp.TenBoPhanFull = i.ToString() + ". " + bp.TenBoPhan;
                    i++;
                }
                //
                Guid idAdmin = WebGroupConst.QuanTriTruongID;
                Guid idHieuTruong = WebGroupConst.HieuTruongID;
                Guid idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID;
                Guid idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID;
                Guid idHoiDongQuanTriUQ = WebGroupConst.HoiDongQuanTriUyQuyenID;
                //
                if (list.Count > 1 || groupId.Equals(idAdmin) 
                    || groupId.Equals(idHieuTruong) || groupId.Equals(idHieuTruongUQ) 
                    || groupId.Equals(idHoiDongQuanTri) || groupId.Equals(idHoiDongQuanTriUQ))
                {
                    DTO_BoPhan boPhanNew = new DTO_BoPhan();
                    boPhanNew.TenBoPhan = "Tất cả";
                    boPhanNew.MaQuanLy = "All";
                    boPhanNew.TenBoPhanFull = "0. Tất cả";
                    boPhanNew.STT = 0;
                    //
                    list.Insert(0, boPhanNew);
                }
                return list;
            }
            else
            {
                return null;
            }
        }  
        public String GetList_BoPhan_DuocPhanQuyenChoWebUserId_Json(String publicKey, String token, Guid webUserId, Guid congTy)
        {//DANG SD
            IEnumerable<DTO_BoPhan> list = GetList_BoPhan_DuocPhanQuyenChoWebUserId(publicKey, token, webUserId,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String GetList_BoPhan_DuocPhanQuyenChoWebUserIdAndCompany_Json(String publicKey, String token, Guid webUserId,Guid congTy)
        {//DANG SD
            IEnumerable<DTO_BoPhan> list = GetList_BoPhan_DuocPhanQuyenChoWebUserIdAndCompany(publicKey, token, webUserId, congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String GetBoPhan_GetLoaiBoPhanByWebGroup_Json(String publicKey, String token, Guid webgroupid,Guid congTy)
        {//DANG SD
            IEnumerable<DTO_BoPhan> list = GetBoPhan_GetLoaiBoPhanByWebGroup(publicKey, token, webgroupid, congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String GetList_BoPhan_DuocPhanQuyenChoWebUserId_Json_All(String publicKey, String token, Guid webUserId, Guid groupId, Guid congTy)
        {//DANG SD
            List<DTO_BoPhan> list = GetList_BoPhan_DuocPhanQuyenChoWebUserId_All(publicKey, token, webUserId, groupId,congTy);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }

        public String GetList_Truong_DuocPhanQuyenChoWebUserId_Json(String publicKey, String token, Guid webUserId)
        {//DANG SD
            List<DTO_BoPhan> list = GetList_Truong_DuocPhanQuyenChoWebUserId(publicKey, token, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        public String GetList_Truong_DuocPhanQuyenChoWebUserId_New_Json(String publicKey, String token, Guid webUserId)
        {//DANG SD
            List<DTO_BoPhan> list = GetList_Truong_DuocPhanQuyenChoWebUserId_New(publicKey, token, webUserId);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}
